/*
 * SPDX - FileCopyrightText: Copyright © 2022 Christian Günther <cg-ite@gmx.de>
 * SPDX - License - Identifier: GPL - 3.0 - or - later
 */

using System.Text.Json.Serialization;

namespace TrackAndFieldResults.Omega
{
    /// <summary>
    /// Einstiegspunkt für die Abfrage aller vorhandenen
    /// Wettkämpfe
    /// </summary>
    public class CompetitionsRoot
    {
        [JsonPropertyName("uid")]
        public int Uid { get; set; }

        [JsonPropertyName("hash")]
        public string Hash { get; set; }

        [JsonPropertyName("content")]
        public CompetitionsContent Content { get; set; }
    }

    public class CompetitionsContent
    {
        [JsonPropertyName("full")]
        public CompetitionsDetails Full { get; set; }
    }

    public class CompetitionsDetails
    {
        // ab hier Wettkampfdetails
        [JsonPropertyName("EventGroups")]
        public Dictionary<string, EventGroup> Eventgroups { get; set; }
        /// <summary>
        /// Omega-Id zur Abfrage des Zeitplans und der
        /// Ergebnisse
        /// </summary>
        [JsonPropertyName("Events")]
        public Dictionary<string, @Event> Events { get; set; }

    }

    public class EventGroup
    {
        [JsonPropertyName("Events")]
        public Dictionary<string, @Event> Events { get; set; }
    }

    public class @Event
    {
        [JsonPropertyName("HasCISConfig")]
        public bool HasCISConfig { get; set; }

        public string Name { get; set; }
    }

}
