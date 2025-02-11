/*
 * SPDX - FileCopyrightText: Copyright © 2022 Christian Günther <cg-ite@gmx.de>
 * SPDX - License - Identifier: GPL - 3.0 - or - later
 */

using Shouldly;
using TrackAndFieldResults.Omega;

namespace TrackAndFieldResults.Tests.Omega
{
    /// <summary>
    /// Tests für das lokale Abspeichern der json-Dateien
    /// </summary>
    public class DownloaderTests
    {
        private const string downloadPath = "C:\\Temp";
        private const string competition = "";
        private const string baseUrl = "https://ps-cache.web.swisstiming.com";

        [SetUp]
        public void Setup()
        {
        }
        // Ablauf:
        // 1. Abfrage aller verfügbaren Wettkämpfe
        // 2. Abfrage CompDetails 
        // 3. Abfrage Schedule
        // 4. Abfrage Startliste eines Events
        // 5. Abfrage Attemptlsite eines Events

        /// <summary>
        /// Erster Schritt: Alle derzeit verfügbaren und
        /// geplanten Wettkämpfe runterladen, um den 
        /// WettkampfId zu bekommen
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task Can_Download_Competitions()
        {
            // https://livecache.sportresult.com
            var download = new OmegaClient(baseUrl);
            string _fullname = Path.Combine(downloadPath, $"competitions.json");

            var res = await download.CompetitionsAsync(_fullname);
            File.Exists(_fullname).ShouldBeTrue();
            res.ShouldNotBeNull();
        }
        /// <summary>
        /// Schritt 2: Läd die Details eines Wettkampfes herunter
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task Can_Download_Competition_Details()
        {
            var download = new OmegaClient(baseUrl);
            string _fullname = Path.Combine(downloadPath, $"{competition}_details.json");

            var res = await download.CompetitionDetailsAsync(competition, _fullname);
            File.Exists(_fullname).ShouldBeTrue();
        }
        /// <summary>
        /// Test für einen nicht vorhandenen Wettkampf-Id
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task Can_Fail_Competition_Details()
        {
            var download = new OmegaClient(baseUrl);
            string _fullname = Path.Combine(downloadPath, $"{competition}_details.json");

            try
            {
                var res = await download.CompetitionDetailsAsync(competition, _fullname);
                res.ShouldNotBeNull();
            }
            catch (HttpRequestException ex)
            {
                // statuscode gibt es erst ab net6.0
                //throw;
                // https://stackoverflow.com/questions/38476796/how-to-set-net-core-in-if-statement-for-compilation
                ex.Message.ShouldContain("404");
            }
        }
        /// <summary>
        /// Schritt 3: Läd den Zeitplan eines Wettkampfe herunter
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task Can_Download_Schedule()
        {
            var download = new OmegaClient(baseUrl);
            string _fullname = Path.Combine(downloadPath, $"{competition}_schedule.json");

            var res = await download.ScheduleAsync(competition, _fullname);
            res.Content.Full.ListEvent.Count.ShouldBeGreaterThan(0);
            File.Exists(_fullname).ShouldBeTrue();
        }

        /// <summary>
        /// Schritt 4: Läd alle Disziplinen eines Zeitplans herunter
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task Can_Download_Schedule_And_Event()
        {
            var download = new OmegaClient(baseUrl);
            string _fullname = Path.Combine(downloadPath, $"{competition}_schedule.json");

            var res = await download.ScheduleAsync(competition, _fullname);
            res.Content.Full.ListEvent.Count.ShouldBeGreaterThan(0);
            File.Exists(_fullname).ShouldBeTrue();

            foreach (var id in res.Content.Full.Units.Keys)
            {
                _fullname = Path.Combine(downloadPath, $"{competition}_{id}.json");
                var evt = await download.EventAsync(competition, id, _fullname);
                File.Exists(_fullname).ShouldBeTrue();
                Thread.Sleep(300);
            }
        }

        /// <summary>
        /// Läd ein bestimmte Disziplin runter.
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task Can_Download_Event()
        {
            var download = new OmegaClient(baseUrl);
            string _fullname = Path.Combine(downloadPath, $"{competition}_schedule.json");

            var schedule = await download.ScheduleAsync(competition, _fullname);
            schedule.Content.Full.ListEvent.Count.ShouldBeGreaterThan(0);
            var id = schedule.Content.Full.Units.Keys.First();
            
            _fullname = Path.Combine(downloadPath, $"{competition}_{id}.json");
            var res = await download.EventAsync(competition, id, _fullname);
            File.Exists(_fullname).ShouldBeTrue();
        }

        /// <summary>
        /// Läd all Dateien eines Wettkampfes runter.
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task Can_Download_Whole_Event()
        {
            var download = new OmegaClient(baseUrl);
            string path = Path.Combine(downloadPath, competition);

            var res = await download.CompetitionFilesAsync(competition, path);
            string schedulePath = Path.Combine(downloadPath, $"{competition}_schedule.json");
            File.Exists(schedulePath).ShouldBeTrue("Schedule File not found");
            
            var compRoot = new OmegaJsonFile().GetSchedule(schedulePath);
            compRoot.ShouldNotBeNull();
            compRoot.Content.ShouldNotBeNull();
            compRoot.Content.Full.ShouldNotBeNull();
            var details = compRoot.Content.Full;
            details.ShouldNotBeNull();

            Directory.Exists(path).ShouldBeTrue();
            Directory.GetFiles(path).Count().ShouldBe(details.Units.Count() + 2);
            // +2: schedule and details.json
        }
    }
}