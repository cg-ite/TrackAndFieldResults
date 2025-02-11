/*
 * SPDX - FileCopyrightText: Copyright © 2022 Christian Günther <cg-ite@gmx.de>
 * SPDX - License - Identifier: GPL - 3.0 - or - later
 */


using Shouldly;
using System.Threading.Tasks;
using TrackAndFieldResults.Omega;

namespace TrackAndFieldResults.Tests.Omega
{
    public class JsonClientTests
    {
        private const string baseUrl = "https://ps-cache-ath.ath.swisstiming.com";

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task Can_Get_CompetitionsList()
        {
            var client = new OmegaClient(baseUrl);
            var comps = await client.GetCompetitionsAsync();
            comps.ShouldNotBeNull();
            comps.Content.Full.Events.Count().ShouldBeGreaterThan(0);
            comps.Content.Full.Eventgroups.Count().ShouldBeGreaterThan(0);

            var first = comps.Content.Full.Events.First();
            first.Key.ShouldNotBeEmpty();   // EventID

            var group = comps.Content.Full.Eventgroups.First();
            group.Value.Events.Count().ShouldBeGreaterThan(0);
            first = group.Value.Events.First();
            first.Key.ShouldNotBeEmpty();   // EventID

        }

        [Test]
        public async Task Can_Get_CompetitionDetail()
        {
            var client = new OmegaClient(baseUrl);
            var comps = await client.GetCompetitionsAsync();
            comps.ShouldNotBeNull();

            // gets the newest competition, older comps can be unavailable
            var first = comps.Content.Full.Eventgroups.Last().Value.Events.Last();
            first.Key.ShouldNotBeEmpty();   // EventID

            var details = await client.GetCompetitionDetailsAsync(first.Key);
            details.Content.Full.EventId.ShouldNotBeEmpty();
            details.Content.Full.EventName.ShouldNotBeEmpty();
            details.Content.Full.Header1.ShouldNotBeEmpty();
        }
        
        [Test]
        public async Task Can_Get_Schedule()
        {
            var client = new OmegaClient(baseUrl);
            var comps = await client.GetCompetitionsAsync();
            comps.ShouldNotBeNull();

            // gets the newest competition, older comps can be unavailable
            var first = comps.Content.Full.Eventgroups.Last().Value.Events.Last();
            first.Key.ShouldNotBeEmpty();   // EventID

            var details = await client.GetScheduleAsync(first.Key);
            details.Content.Full.DayTimes.Count().ShouldBeGreaterThan(0);
            details.Content.Full.ListEvent.Count().ShouldBeGreaterThan(0);
            details.Content.Full.Units.Count().ShouldBeGreaterThan(0);
        }

        [Test]
        public async Task Can_Get_EventDetails()
        {
            var client = new OmegaClient(baseUrl);
            var comps = await client.GetCompetitionsAsync();
            comps.ShouldNotBeNull();

            // gets the newest competition, older comps can be unavailable
            var first = comps.Content.Full.Eventgroups.Last().Value.Events.Last();
            first.Key.ShouldNotBeEmpty();   // EventID

            var details = await client.GetScheduleAsync(first.Key);
            details.Content.Full.DayTimes.Count().ShouldBeGreaterThan(0);
            details.Content.Full.ListEvent.Count().ShouldBeGreaterThan(0);
            details.Content.Full.Units.Count().ShouldBeGreaterThan(0);

            // testing basic mappings
            var disciplin = await client.GetEventDetailsAsync(first.Key, 
                details.Content.Full.Units.First().Key);
            disciplin.Content.Full.CompetitorDetails.Count().ShouldBeGreaterThan(0);
            disciplin.Content.Full.UnitName.ShouldNotBeEmpty();
            disciplin.Content.Full.EventName.ShouldNotBeEmpty();
            disciplin.Content.Full.StartTime.ShouldBeGreaterThan(0);
            disciplin.Content.Full.Startlist.Count().ShouldBeGreaterThan(0);
            disciplin.Content.Full.Resultlist.Count().ShouldBeGreaterThan(0);
        }
    }
}