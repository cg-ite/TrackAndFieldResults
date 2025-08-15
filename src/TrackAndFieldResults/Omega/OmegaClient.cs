/*
 * SPDX - FileCopyrightText: Copyright © 2022 Christian Günther <cg-ite@gmx.de>
 * SPDX - License - Identifier: GPL - 3.0 - or - later
 */

using TrackAndFieldResults.Extentions;

namespace TrackAndFieldResults.Omega
{

    //public partial record OmegaUrls
    //{
    //    public string BaseUrl = "https://ps-cache.web.swisstiming.com";        public string Competitions = "node/db/ATH_PROD/CIS_CONFIG_STATUS_JSON.json";
    //    public string CompetitionDetails = "node/db/ATH_PROD/{competition}_CURRENTEVENT_JSON.json?s=unknown&t=0";
    //    public string Schedule = "node/db/ATH_PROD/{competition}_SCHEDULE_JSON.json";
    //    public string EventDetails = "node/db/ATH_PROD/{competition}_TIMING_{eventId}_JSON.json";
    //}

    /// <summary>
    /// Klasse um json Daten vom Omega-Server runterzuladen
    /// </summary>
    public partial class OmegaClient
    {
        private readonly HttpClient _httpClient;

        //public OmegaUrls Urls { get; set; } = new OmegaUrls();

        private Uri _baseuri;
        public OmegaClient(string baseurl)
        {
            //if (urls != null) { Urls = urls; }
            BaseUrl = baseurl;
            _baseuri = new Uri(baseurl);
            // https://stackoverflow.com/questions/29801195/adding-headers-when-using-httpclient-getasync
            _httpClient = new HttpClient();
            // headers for all consecutive requests
            _httpClient.DefaultRequestHeaders.Add("Accept", "text/json");
            var version = new Random(2783763).Next(90, 122);
            _httpClient.DefaultRequestHeaders.Add("User-Agent",
                $"Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:122.0) Gecko/20100101 Firefox/{version}.0");
            Initialize();
        }

        /// <summary>
        /// Läd alle Dateien eines Wettkampfes herunter
        /// </summary>
        /// <param name="competition">Name des Wettkampfes aus Omega-Liste</param>
        /// <param name="path">Ordner in dem alls json Dateien gespeichert werden</param>
        /// <returns></returns>
        public async Task<bool> CompetitionFilesAsync(string competition, string path)
        {
            Directory.CreateDirectory(path);

            await CompetitionDetailsAsync(competition,
                Path.Combine(path, $"{competition}_details.json"));

            var schedule = await ScheduleAsync(competition,
                Path.Combine(path, $"{competition}_schedule.json"));

            foreach (var id in schedule.Content.Full.Units.Keys)
            {
                string fullname = Path.Combine(path, $"{competition}_{id}.json");
                var evt = await EventAsync(competition.ToUpper(), id, fullname);
                Thread.Sleep(500);
            }
            return true;
        }


        /// <summary>
        /// Läd eine Liste von aktuellen und vergangenen 
        /// Wettkämpfen runter, die noch online abrufbar sind.
        /// Ältere Wettkämpfe können auch noch online sein.
        /// </summary>
        /// <param name="fullname"></param>
        /// <returns></returns>
        /// <exception cref="HttpRequestException">Falls ein Problem mit der
        /// Verbindung besteht oder die Wettkämpfe nicht gefunden wird.</exception>
        public async Task<CompetitionsRoot> CompetitionsAsync(string fullname)
        {
            string _address = GetCompetitionsUrl();
            // Send asynchronous request
            await _httpClient.DownloadFileTaskAsync(new Uri(_baseuri, _address), fullname);
            return new OmegaJsonFile().GetCompetitions(fullname); ;
        }
        /// <summary>
        /// Läd die Details eines Wettkampfes herunter
        /// </summary>
        /// <param name="competition">Name des Wettkampfes aus Omega-Liste</param>
        /// <param name="fullname">Dateiname der json Datei</param>
        /// <returns></returns>
        public async Task<CompetitionRoot> CompetitionDetailsAsync(string competition, string fullname)
        {
            string _address = GetCompetitionDetailsUrl(competition);
            // Send asynchronous request
            await _httpClient.DownloadFileTaskAsync(new Uri(_baseuri,_address), fullname);
            return new OmegaJsonFile().GetCompetitionDetails(fullname); ;
        }
        /// <summary>
        /// Läd den Zeitplan eines Wettkampfe herunter
        /// </summary>
        /// <param name="competition">Name des Wettkampfes aus Omega-Liste</param>
        /// <param name="fullname">Dateiname der json Datei</param>
        /// <returns></returns>
        public async Task<ScheduleRoot> ScheduleAsync(string competition, string fullname)
        {
            string _address = GetScheduleUrl(competition);
            // Send asynchronous request
            await _httpClient.DownloadFileTaskAsync(new Uri( _baseuri, _address), fullname);

            return new OmegaJsonFile().GetSchedule(fullname);
        }
        /// <summary>
        /// Läd eine Disziplin eines Wettkampfes herunter
        /// </summary>
        /// <param name="competitionKey">Name des Wettkampfes aus Omega-Liste</param>
        /// <param name="eventKey"></param>
        /// <param name="fullname">Dateiname der json Datei</param>
        /// <returns></returns>
        public async Task<EventRoot> EventAsync(string competitionKey, string eventKey, string fullname)
        {
            string _address = GetEventUrl(competitionKey, eventKey);
            // Send asynchronous request
            await _httpClient.DownloadFileTaskAsync(new Uri(_baseuri, _address), fullname);
            return new OmegaJsonFile().GetEventRoot(fullname);
        }

        //return "https://livecache.sportresult.com";

        /// <summary>
        /// Generiert aus dem Wettkampfnamen die Url
        /// für den Download der Wettkmapf-Details
        /// </summary>
        /// <param name="competitionKey">Name des Wettkampfes aus Omega-Liste</param>
        /// <returns></returns>
        public string GetCompetitionDetailsUrl(string competitionKey)
        {
            return $"node/db/ATH_PROD/{competitionKey.ToUpper()}_CURRENTEVENT_JSON.json?s=unknown&t=0";
        }
        /// <summary>
        /// Gibt die Url für die Übersichtsseite einiger
        /// Wettkämpfe zurück. Meist sind ältere Wettkämpfe
        /// noch erreichbar
        /// </summary>
        /// <returns></returns>
        public string GetCompetitionsUrl()
        {
            return $"node/db/ATH_PROD/CIS_CONFIG_STATUS_JSON.json";
        }
        /// <summary>
        /// Erstellt die Url für den Zeitplan eines Wettkampfes
        /// </summary>
        /// <param name="competitionKey">Event-Name des Wettkampfes aus der competitioins list</param>
        /// <returns></returns>
        public string GetScheduleUrl(string competitionKey)
        {
            return $"node/db/ATH_PROD/{competitionKey.ToUpper()}_SCHEDULE_JSON.json";
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="competitionKey">Event-Id des Wettkampfes</param>
        /// <param name="eventKey"></param>
        /// <returns></returns>
        public string GetEventUrl(string competitionKey, string eventKey)
        {
            return $"node/db/ATH_PROD/{competitionKey}_TIMING_{eventKey}_JSON.json";
        }
    }
}
