/*
 * SPDX - FileCopyrightText: Copyright © 2022 Christian Günther <cg-ite@gmx.de>
 * SPDX - License - Identifier: GPL - 3.0 - or - later
 */

using System.Text.Json.Serialization;

namespace TrackAndFieldResults.Omega
{
    // created with the help of https://json2csharp.com/

    /// <summary>
    /// CHild of a competitor. A relay is a competitor
    /// with 4 childrend competitor
    /// </summary>
    public class CompetitorChild
    {
        /// <summary>
        /// Competitior Id of a athlet of a relay
        /// </summary>
        [JsonPropertyName("Id")]
        public string Id { get; set; }

    }
    /// <summary>
    /// Athlete of the Event
    /// </summary>
    public class Athlete
    {
        /// <summary>
        /// All athletes in case of a relay
        /// </summary>
        [JsonPropertyName("Children")]
        public List<CompetitorChild> Children { get; set; }
        /// <summary>
        /// Startnumber
        /// </summary>
        [JsonPropertyName("Bib")]
        public string Bib { get; set; }
        /// <summary>
        /// Vielleicht Startpos im Finale? Athleten ohne
        /// Finale haben keine Startpos. Muss über Startliste
        /// gesucht werden.
        /// Initial Startposition. Can change througout the
        /// event (see 
        /// </summary>
        [JsonPropertyName("StartPos")]
        public string StartPos { get; set; }
        /// <summary>
        /// Startposition zu Beginn des Wettkampfes
        /// </summary>
        public int InitialStartPos { get; set; }

        [JsonPropertyName("RankIndex")]
        public int RankIndex { get; set; }

        [JsonPropertyName("FedCode")]
        public string FedCode { get; set; }

        [JsonPropertyName("Stats")]
        public Stats Stats { get; set; }

        [JsonPropertyName("Details")]
        public Details Details { get; set; }

        [JsonPropertyName("Intermediate")]
        public List<Intermediate> Intermediate { get; set; }

        [JsonPropertyName("RecordsMaj")]
        public List<object> RecordsMaj { get; set; }

        [JsonPropertyName("RecordsMin")]
        public List<object> RecordsMin { get; set; }

        [JsonPropertyName("Id")]
        public string Id { get; set; }

        [JsonPropertyName("AthleteId")]
        public string AthleteId { get; set; }

        [JsonPropertyName("Nationality")]
        public string Nationality { get; set; }
        /// <summary>
        /// Nachname des Athleten
        /// </summary>
        [JsonPropertyName("Name")]
        public string Name { get; set; }
        /// <summary>
        /// Vorname des Athleten
        /// </summary>
        [JsonPropertyName("FirstName")]
        public string FirstName { get; set; }

        [JsonPropertyName("FirstNameShort")]
        public string FirstNameShort { get; set; }
        /// <summary>
        /// Geschlecht des Athleten
        /// </summary>
        [JsonPropertyName("Gender")]
        public string Gender { get; set; }

        [JsonPropertyName("Dob")]
        public string Dob { get; set; }

        [JsonPropertyName("Age")]
        public string Age { get; set; }

        [JsonPropertyName("Club")]
        public string Club { get; set; }

        [JsonPropertyName("ListIndex")]
        public int ListIndex { get; set; }
        /// <summary>
        /// Ergebnis, wird von Schedule Klasse gesetzt
        /// </summary>
        [JsonPropertyName("Result")]
        public string Result { get; set; }

        public override string ToString()
        {
            return $"{FirstName} {Name}";
        }
    }
    /// <summary>
    /// Details of an Event/Disciplin
    /// </summary>
    public class EventDetails
    {
        [JsonPropertyName("Records")]
        public List<Record> Records { get; set; }

        [JsonPropertyName("AttemptSeparators")]
        public int[] AttemptSeparators { get; set; }
        /// <summary>
        /// Keys of different settings (?)
        /// </summary>
        [JsonPropertyName("Rsc")]
        public Rsc Rsc { get; set; }

        [JsonPropertyName("EventName")]
        public string EventName { get; set; }

        [JsonPropertyName("EventName_Translations")]
        public Dictionary<string, string> EventNameTranslations { get; set; }

        [JsonPropertyName("PhaseName")]
        public string PhaseName { get; set; }

        [JsonPropertyName("PhaseName_Translations")]
        public Dictionary<string, string> PhaseNameTranslations { get; set; }

        [JsonPropertyName("UnitName")]
        public string UnitName { get; set; }

        [JsonPropertyName("UnitName_Translations")]
        public Dictionary<string, string> UnitNameTranslations { get; set; }

        [JsonPropertyName("NameShort")]
        public string NameShort { get; set; }

        [JsonPropertyName("NameShort_Translations")]
        public Dictionary<string, string> NameShortTranslations { get; set; }

        [JsonPropertyName("Status")]
        public string Status { get; set; }

        [JsonPropertyName("StartTime")]
        public long StartTime { get; set; }

        [JsonPropertyName("EndTime")]
        public long EndTime { get; set; }

        [JsonPropertyName("Medal")]
        public string Medal { get; set; }

        [JsonPropertyName("MaxChildCount")]
        public int MaxChildCount { get; set; }

        [JsonPropertyName("Stats")]
        public Stats Stats { get; set; }

        [JsonPropertyName("SplitCount")]
        public int SplitCount { get; set; }

        [JsonPropertyName("Splits")]
        public List<Split> Splits { get; set; }

        [JsonPropertyName("Officials")]
        public List<object> Officials { get; set; }

        [JsonPropertyName("CompetitorDetails")]
        public Dictionary<string, Athlete> CompetitorDetails { get; set; }

        [JsonPropertyName("Startlist")]
        public Dictionary<string, StartlistEntry> Startlist { get; set; }

        [JsonPropertyName("Resultlist")]
        public Dictionary<string, Athlete> Resultlist { get; set; }
    }

    public class Content
    {
        [JsonPropertyName("full")]
        public EventDetails Full { get; set; }
    }

    /// <summary>
    /// Versuch eines Athleten im Wettkampf, wobei
    /// der Athlet auch auslassen kann und damit
    /// keine Leistung erreicht hat.
    /// </summary>
    public class Intermediate
    {
        /// <summary>
        /// Versuchsnummer bei horizontalen Sprüngen
        /// und Würfen.
        /// Nr der Höhe bei vertikalen Sprüngen.
        /// </summary>
        [JsonPropertyName("Number")]
        public int Number { get; set; }

        /// <summary>
        /// Ergebnis bei Läufen, horizontalen
        /// Sprüngen und Würfen.
        /// Höhe bei vertikalen Sprüngen.
        /// </summary>
        [JsonPropertyName("Result")]
        public string Result { get; set; }

        [JsonPropertyName("Behind")]
        public string Behind { get; set; }

        [JsonPropertyName("SectorResult")]
        public string SectorResult { get; set; }

        [JsonPropertyName("Wind")]
        public string Wind { get; set; }

        [JsonPropertyName("IRM")]
        public string IRM { get; set; }

        [JsonPropertyName("Flag")]
        public string Flag { get; set; }

        public override string ToString()
        {
            if (Wind != null) return $"V{Number}: {Result} [{Wind}] {Behind}";
            return $"V{Number}: {Result} {Behind}";
        }
    }

    /// <summary>
    /// Einstiegspunkt für eine Disziplin eines 
    /// Wettkampfes
    /// </summary>
    public class EventRoot
    {
        [JsonPropertyName("uid")]
        public int Uid { get; set; }

        [JsonPropertyName("hash")]
        public string Hash { get; set; }

        [JsonPropertyName("content")]
        public Content Content { get; set; }
    }
    /// <summary>
    /// Dictionary mit verschiedenen
    /// Ids bzw. Teilen der Ids
    /// </summary>
    public class Rsc
    {
        [JsonPropertyName("Discipline")]
        public string Discipline { get; set; }

        [JsonPropertyName("Gender")]
        public string Gender { get; set; }

        [JsonPropertyName("Event")]
        public string Event { get; set; }

        [JsonPropertyName("Phase")]
        public string Phase { get; set; }

        [JsonPropertyName("Unit")]
        public string Unit { get; set; }

        [JsonPropertyName("Value")]
        public string Value { get; set; }

        [JsonPropertyName("ValueDiscipline")]
        public string ValueDiscipline { get; set; }

        [JsonPropertyName("ValueEvent")]
        public string ValueEvent { get; set; }

        [JsonPropertyName("ValueUnit")]
        public string ValueUnit { get; set; }

        [JsonPropertyName("LongEvent")]
        public string LongEvent { get; set; }
        
        [JsonPropertyName("LongPhase")]
        public string LongPhase { get; set; }

        [JsonPropertyName("ValuePhase")]
        public string ValuePhase { get; set; }
    }
    /// <summary>
    /// Höhen beim Hoch- Stabhochsprung
    /// </summary>
    public class Split
    {
        [JsonPropertyName("Number")]
        public int Number { get; set; }

        [JsonPropertyName("Name")]
        public string Name { get; set; }

        [JsonPropertyName("Name_Translations")]
        public Dictionary<string, string> NameTranslations { get; set; }
    }

    public class Stats
    {
        [JsonPropertyName("Att")]
        public string Att { get; set; }

        [JsonPropertyName("CupId")]
        public string CupId { get; set; }

        [JsonPropertyName("Gap")]
        public string Gap { get; set; }

        [JsonPropertyName("HasWind")]
        public string HasWind { get; set; }

        [JsonPropertyName("NrLanes")]
        public string NrLanes { get; set; }

        [JsonPropertyName("StartType")]
        public string StartType { get; set; }

        [JsonPropertyName("CompetitionName")]
        public string CompetitionName { get; set; }

        [JsonPropertyName("OvrId")]
        public string OvrId { get; set; }

        [JsonPropertyName("RecCode")]
        public string RecCode { get; set; }

        [JsonPropertyName("Row")]
        public string Row { get; set; }

        [JsonPropertyName("StartListAvailable")]
        public string StartListAvailable { get; set; }

        [JsonPropertyName("Type")]
        public string Type { get; set; }

        [JsonPropertyName("LineUp")]
        public string LineUp { get; set; }

        [JsonPropertyName("PB")]
        public string PB { get; set; }

        [JsonPropertyName("ResultAlt")]
        public string ResultAlt { get; set; }

        [JsonPropertyName("SB")]
        public string SB { get; set; }

        [JsonPropertyName("Wind")]
        public string Wind { get; set; }
    }

    public class Record
    {
        [JsonPropertyName("Code")]
        public string Code { get; set; }

        [JsonPropertyName("Region")]
        public string Region { get; set; }

        [JsonPropertyName("Value")]
        public string Value { get; set; }
    }
    /// <summary>
    /// Einträge in der offiziellen Startliste
    /// des Vorkampfes
    /// </summary>
    public class StartlistEntry
    {
        /// <summary>
        /// List of All athletes of a relay 
        /// </summary>
        [JsonPropertyName("Children")]
        public StartlistEntry[] Children { get; set; }

        [JsonPropertyName("ListIndex")]
        public int ListIndex { get; set; }

        [JsonPropertyName("Id")]
        public string Id { get; set; }
    }

    public class Details
    {
    }

}
