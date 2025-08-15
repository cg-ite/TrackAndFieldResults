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
    public partial class CompetitionsRoot
    {
        [JsonPropertyName("uid")]
        public int Uid { get; set; }

        [JsonPropertyName("hash")]
        public string Hash { get; set; }

        [JsonPropertyName("content")]
        public CompetitionsContent Content { get; set; }
    }

    public partial class CompetitionsContent
    {
        [JsonPropertyName("full")]
        public CompetitionsDetails Full { get; set; }
    }

    public partial class CompetitionsDetails
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

    public partial class EventGroup
    {
        [JsonPropertyName("Events")]
        public Dictionary<string, @Event> Events { get; set; }
    }

    public partial class @Event
    {
        [JsonPropertyName("HasCISConfig")]
        public bool HasCISConfig { get; set; }

        public string Name { get; set; }
    }

}
