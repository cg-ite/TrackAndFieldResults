using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TrackAndFieldResults.Omega;
using TrackAndFieldResults.Utils;

namespace TrackAndFieldResults.Common
{
    public class CommonOmegaClient : IClient
    {
        private OmegaClient _client;

        public CommonOmegaClient() {
            // last known url 2025-10
            BaseUrl = "https://ps-cache.web.swisstiming.com";
            
            HttpClient httpClient = new();
            httpClient.DefaultRequestHeaders.Add("Accept", "text/json");
            var version = new Random(2783763).Next(100, 140);
            httpClient.DefaultRequestHeaders.Add("User-Agent",
                $"Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:122.0) Gecko/20100101 Firefox/{version}.0");

            _client = new OmegaClient(httpClient);
            _client.BaseUrl = BaseUrl;
            _client.ReadResponseAsString = true;    //for saving response to file
        }
        public string BaseUrl { get ; set ; }

        /// <summary>
        /// Cached competitions to minimizing requests. Lazy loading competitiondetails, 
        /// if needed
        /// </summary>
        /// <remarks>Not null after calling <see cref="GetCompetitionsAsync()"/></remarks>
        public Competition[] Competitions { get; set ; }
        
        /// <summary>
        /// Gets all verfügbare competitions. expensive call please cache the 
        /// competitions details.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Competition[]> GetCompetitionsAsync(CancellationToken cancellationToken)
        {
            var comps = await _client.GetCompetitionsAsync();
            var events = comps.Content.Full.Eventgroups.Values.SelectMany(eg =>
                eg.Events);

            // Namen noch _ löschen und Datum extrahieren
            var res = events.OrderBy(e=> e.Key).Select((e, i) => new Competition()
            {
                ProviderId = e.Key,
                ResultProviderId = ProviderId.Omega,
                Name = GetName(e.Key),
                StartDate = GetStartdate(e.Key),
                Id = i,
            });

            return res.ToArray();
        }

        private string GetName(string key)
        {
            return key.Substring(0, key.Length - 4).Replace("_", "");
        }

        private DateTime GetStartdate(string key)
        {
            string year = key.Substring(key.Length-4, 4);
            if(year.StartsWith("20"))
            {
                int y;
                if(int.TryParse(year, out y))
                {
                    return new DateTime(y, 1, 1);
                }
            }
            return new DateTime(1,1,1);
        }

        public Task<Competition[]> GetCompetitionsAsync()
        {
            return GetCompetitionsAsync(System.Threading.CancellationToken.None);
        }

        public async Task<Competition> GetCompetitionDetailsAsync(string competitionKey, System.Threading.CancellationToken cancellationToken)
        {
            var res = new Competition();
            // idee an dieser Stelle:
            // - Details laden für namen
            // - schedule laden, damit anfangs und end-datum bestimmt werden können
            // - alle events sind schon da für auswahl

            // kann auch bei Seltec umgesetzt werden
            // WA und Atos funktionieren so ähnlich
            var (details, schedule) = await TaskEx.WhenAll(
                _client.GetCompetitionDetailsAsync(competitionKey, cancellationToken),
                _client.GetScheduleAsync(competitionKey, cancellationToken));

            if (details != null) {
                res.Name = details.Content.Full.EventName;
                res.Town = details.Content.Full.Header2;
                res.ProviderId = details.Content.Full.EventId;
            }
            if (schedule != null)
            {
                var s = schedule.Content.Full;
                res.StartDate = DateTime.Parse(s.ListDay.First());
                res.EndDate = DateTime.Parse(s.ListDay.Last());

                foreach (var ev in s.ListEvent)
                {
                    var cEvent = new Event()
                    {
                        ProviderId = ev.Key,
                        StartDate = 
                    };
                }
            }
            return res;
        }

        /// <summary>
        /// Gets the details of a competition. You will find the name
        /// and date of competition here.
        /// </summary>
        /// <param name="competitionKey">The competition-Id from the competitions list</param>
        /// <returns>the details of a competition</returns>
        /// <exception cref="ApiException">A server side error occurred.</exception>
        public async Task<Competition> GetCompetitionDetailsAsync(string competitionKey)
        {
            return await GetCompetitionDetailsAsync(competitionKey, CancellationToken.None);

        }

    }
}
