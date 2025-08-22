/*
 * SPDX - FileCopyrightText: Copyright © 2022 Christian Günther <cg-ite@gmx.de>
 * SPDX - License - Identifier: GPL - 3.0 - or - later
 */


using Shouldly;
using System.Net.Http;
using System.Threading.Tasks;
using TrackAndFieldResults.Common;
using TrackAndFieldResults.Omega;
using TrackAndFieldResults.Seltec;

namespace TrackAndFieldResults.Tests.Omega
{
    public class JsonClientTests
    {
        //changes sometimes
        private const string baseUrl = "https://ps-cache.web.swisstiming.com";

        private OmegaClient _client;
        private const string downloadPath = "C:\\Temp\\Omega";

        [OneTimeSetUp]
        public void Setup()
        {
            HttpClient httpClient = new();
            httpClient.DefaultRequestHeaders.Add("Accept", "text/json");
            var version = new Random(2783763).Next(90, 135);
            httpClient.DefaultRequestHeaders.Add("User-Agent",
                $"Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:122.0) Gecko/20100101 Firefox/{version}.0");

            _client = new OmegaClient(httpClient);
            _client.BaseUrl = baseUrl;
            _client.ReadResponseAsString = true;    //for saving response to file
            Directory.CreateDirectory(downloadPath);
        }

        [Test]
        public async Task Can_Download_Competitions()
        {
            var comps = await _client.GetCompetitionsAsync();
            comps.ShouldNotBeNull();
            comps.Content.Full.Events.Count().ShouldBeGreaterThan(0);
            comps.Content.Full.Eventgroups.Count().ShouldBeGreaterThan(0);

            var first = comps.Content.Full.Events.First();
            first.Key.ShouldNotBeEmpty();   // EventID

            var group = comps.Content.Full.Eventgroups.First();
            group.Value.Events.Count().ShouldBeGreaterThan(0);
            first = group.Value.Events.First();
            first.Key.ShouldNotBeEmpty();   // EventID

            string _fullname = Path.Combine(downloadPath, $"competitions.json");
            _client.SaveResponseText(_fullname);
            File.Exists(_fullname).ShouldBeTrue();
            File.ReadAllText(_fullname).ShouldNotBeNull();
        }

        [Test]
        public async Task Can_Download_Competition_Details()
        {
            var comps = await _client.GetCompetitionsAsync();
            comps.ShouldNotBeNull();

            // gets the newest competition, older comps can be unavailable
            var first = comps.Content.Full.Eventgroups.Last().Value.Events.Last();
            first.Key.ShouldNotBeEmpty();   // EventID

            var details = await _client.GetCompetitionDetailsAsync(first.Key);
            details.Content.Full.EventId.ShouldNotBeEmpty();
            details.Content.Full.EventName.ShouldNotBeEmpty();
            details.Content.Full.Header1.ShouldNotBeEmpty();

            string _fullname = Path.Combine(downloadPath, $"{first.Key}_Details.json");
            _client.SaveResponseText(_fullname);
            File.Exists(_fullname).ShouldBeTrue();
            File.ReadAllText(_fullname).ShouldNotBeNull();
        }
        
        [Test]
        public async Task Can_Download_Schedule()
        {
            var comps = await _client.GetCompetitionsAsync();
            comps.ShouldNotBeNull();

            // gets the newest competition, older comps can be unavailable
            var first = comps.Content.Full.Eventgroups.Last().Value.Events.Last();
            first.Key.ShouldNotBeEmpty();   // EventID

            var details = await _client.GetScheduleAsync(first.Key);
            details.Content.Full.DayTimes.Count().ShouldBeGreaterThan(0);
            details.Content.Full.ListEvent.Count().ShouldBeGreaterThan(0);
            details.Content.Full.Units.Count().ShouldBeGreaterThan(0);

            string _fullname = Path.Combine(downloadPath, $"{first.Key}_Schedule.json");
            _client.SaveResponseText(_fullname);
            File.Exists(_fullname).ShouldBeTrue();
            File.ReadAllText(_fullname).ShouldNotBeNull();
        }

        [Test]
        public async Task Can_Download_Event()
        {
            var comps = await _client.GetCompetitionsAsync();
            comps.ShouldNotBeNull();

            // gets the newest competition, older comps can be unavailable
            var first = comps.Content.Full.Eventgroups.Last().Value.Events.Last();
            first.Key.ShouldNotBeEmpty();   // EventID

            var details = await _client.GetScheduleAsync(first.Key);
            details.Content.Full.DayTimes.Count().ShouldBeGreaterThan(0);
            details.Content.Full.ListEvent.Count().ShouldBeGreaterThan(0);
            details.Content.Full.Units.Count().ShouldBeGreaterThan(0);

            // testing basic mappings
            var disciplin = await _client.GetEventDetailsAsync(first.Key, 
                details.Content.Full.Units.First().Key);
            disciplin.Content.Full.CompetitorDetails.Count().ShouldBeGreaterThan(0);
            disciplin.Content.Full.UnitName.ShouldNotBeEmpty();
            disciplin.Content.Full.EventName.ShouldNotBeEmpty();
            disciplin.Content.Full.StartTime.ShouldBeGreaterThan(0);
            disciplin.Content.Full.Startlist.Count().ShouldBeGreaterThan(0);
            disciplin.Content.Full.Resultlist.Count().ShouldBeGreaterThan(0);

            string _fullname = Path.Combine(downloadPath, $"{first.Key}_Event.json");
            _client.SaveResponseText(_fullname);
            File.Exists(_fullname).ShouldBeTrue();
            File.ReadAllText(_fullname).ShouldNotBeNull();
        }

        /// <summary>
        /// Läd all Dateien eines Wettkampfes runter.
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task Can_Download_Whole_Event()
        {
            throw new NotImplementedException();

            //var download = new OmegaClient(baseUrl);
            //string path = Path.Combine(downloadPath, competition);

            //var res = await download.CompetitionFilesAsync(competition, path);
            //string schedulePath = Path.Combine(downloadPath, $"{competition}_schedule.json");
            //File.Exists(schedulePath).ShouldBeTrue("Schedule File not found");

            //var compRoot = new OmegaJsonFile().GetSchedule(schedulePath);
            //compRoot.ShouldNotBeNull();
            //compRoot.Content.ShouldNotBeNull();
            //compRoot.Content.Full.ShouldNotBeNull();
            //var details = compRoot.Content.Full;
            //details.ShouldNotBeNull();

            //Directory.Exists(path).ShouldBeTrue();
            //Directory.GetFiles(path).Count().ShouldBe(details.Units.Count() + 2);
            //// +2: schedule and details.json
        }
    }
}