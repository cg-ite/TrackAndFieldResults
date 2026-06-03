using System;
using TrackAndFieldResults.Omega;

namespace TrackAndFieldResults.Common
{
    /// <summary>
    /// Manages the provider with cached http-clients
    /// </summary>
    public class ProviderManager
    {
        public Dictionary<ProviderId, IClient> Clients => Provider
            .OrderBy(p => p.Id).Select(p => new { p.Id, p.Client })
            .ToDictionary(p => p.Id, v => v.Client);
        public List<ResultProvider> Provider;

        public ProviderManager(
            IDictionary<ProviderId, HttpClient>? customClients = null)
        {
            var httpClients = BuildHttpClientDictionary(customClients);

            ValidateHttpClients(httpClients);

            Provider =
            [
                new ResultProvider
                {
                    Id = ProviderId.Omega,
                    Name = "Omega",
                    Client = new CommonOmegaClient(httpClients[ProviderId.Omega])
                },
                new ResultProvider
                {
                    Id = ProviderId.Seltec,
                    Name = "Seltec",
                    Client = new CommonSeltecClient(httpClients[ProviderId.Seltec])
                },
            ];  
        }

        private static Dictionary<ProviderId, HttpClient> BuildHttpClientDictionary(
            IDictionary<ProviderId, HttpClient>? custom)
        {
            var dict = new Dictionary<ProviderId, HttpClient>();

            // Alle Provider, die die Library unterstützt
            var allProviders = Enum.GetValues<ProviderId>();

            foreach (var id in allProviders)
            {
                if (custom != null && custom.TryGetValue(id, out var client))
                {
                    // Custom HttpClient – benutzt den des Nutzers
                    dict[id] = client;
                }
                else
                {
                    // Default HttpClient – Library erstellt einen
                    dict[id] = CreateDefaultClient(id);
                }
            }

            return dict;
        }

        private static void ValidateHttpClients(
            IDictionary<ProviderId, HttpClient> clients)
        {
            foreach (var id in Enum.GetValues<ProviderId>())
            {
                if (!clients.ContainsKey(id) || clients[id] == null)
                    throw new InvalidOperationException(
                        $"Missing HttpClient for provider {id}.");
            }
        }

        private static HttpClient CreateDefaultClient(ProviderId id)
        {
            var client = new HttpClient();

            switch (id)
            {
                case ProviderId.Omega:
                    var version = new Random(2783763).Next(90, 140);
                    client.BaseAddress =
                        new Uri("https://ps-cache.web.swisstiming.com");
                    client.DefaultRequestHeaders.Add("User-Agent", $"Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:122.0) Gecko/20100101 Firefox/{version}.0");
                    break;

                case ProviderId.Seltec:
                    client.BaseAddress =
                        new Uri("https://api.seltec.at");
                    client.DefaultRequestHeaders.Add("Accept", "application/json");
                    break;

                    // Wenn später neue Provider kommen:
                    // case ProviderId.XYZ:
                    //     ...
            }

            return client;
        }

        /*public void SetSettings(ProviderSettings settings)
        {
            var http = new HttpClient();
            http.DefaultRequestHeaders.Add("ApiKey", settings.SeltecApiToken);
            SeltecClient = new CommonSeltecClient(http);
            SeltecClient.BaseUrl = settings.SeltecBaseUrl;

            HttpClient httpClient = new();
            httpClient.DefaultRequestHeaders.Add("Accept", "text/json");
            var version = new Random(2783763).Next(90, 135);
            httpClient.DefaultRequestHeaders.Add("User-Agent",
                $"Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:122.0) Gecko/20100101 Firefox/{version}.0");

            OmegaClient = new CommonOmegaClient(httpClient);
            OmegaClient.BaseUrl = settings.OmegaBaseUrl;
        }*/

    }

    /// <summary>
    /// Konnektor-Settings für alle DataSources.
    /// Kann dann als json-schema zurückgegeben werden,
    /// damit daraus automatisch Forms generiert werden können
    /// </summary>
    public class ProviderSettings
    {
        public string OmegaBaseUrl { get; set; }
        public string SeltecBaseUrl { get; set; }
        public string SeltecApiToken { get; set; }
    }
}
