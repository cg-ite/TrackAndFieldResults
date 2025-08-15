/*
 * SPDX - FileCopyrightText: Copyright © 2022 Christian Günther <cg-ite@gmx.de>
 * SPDX - License - Identifier: GPL - 3.0 - or - later
 */

using System.Text.Json.Serialization;

namespace TrackAndFieldResults.Omega
{
    /// <summary>
    /// Einstiegspunkt für die Abfrage der Wettkampf-Details
    /// </summary>
    public partial class CompetitionRoot
    {
        [JsonPropertyName("uid")]
        public int Uid { get; set; }

        [JsonPropertyName("hash")]
        public string Hash { get; set; }

        [JsonPropertyName("content")]
        public CompetitionContent Content { get; set; }
    }

    public partial class CompetitionContent
    {
        [JsonPropertyName("full")]
        public CompetitionDetails Full { get; set; }
    }

    public partial class CompetitionDetails
    {
        // ab hier Wettkampfdetails
        [JsonPropertyName("SeasonId")]
        public string SeasonId { get; set; }
        /// <summary>
        /// Omega-Id zur Abfrage des Zeitplans und der
        /// Ergebnisse
        /// </summary>
        [JsonPropertyName("EventId")]
        public string EventId { get; set; }
        /// <summary>
        /// Offizieller Name des Events
        /// </summary>
        [JsonPropertyName("EventName")]
        public string EventName { get; set; }

        [JsonPropertyName("Header1")]
        public string Header1 { get; set; }

        [JsonPropertyName("Header2")]
        public string Header2 { get; set; }

        [JsonPropertyName("Header3")]
        public string Header3 { get; set; }

        [JsonPropertyName("Theme")]
        public string Theme { get; set; }
    }

}
