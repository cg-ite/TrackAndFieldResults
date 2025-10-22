/*
 * SPDX - FileCopyrightText: Copyright © 2022 Christian Günther <cg-ite@gmx.de>
 * SPDX - License - Identifier: GPL - 3.0 - or - later
 */

using System.Text.Json.Serialization;

namespace TrackAndFieldResults.Omega
{
    /// <summary>
    /// Root Klasse für den Zeitplan eines
    /// Wettkampfes
    /// </summary>
    public class ScheduleRoot
    {
        [JsonPropertyName("uid")]
        public int Uid { get; set; }

        [JsonPropertyName("hash")]
        public string Hash { get; set; }

        [JsonPropertyName("content")]
        public ScheduleContent Content { get; set; }
    }

    public class ScheduleContent
    {
        [JsonPropertyName("full")]
        public ScheduleDetails Full { get; set; }
    }


    public class ScheduleDetails
    {
        [JsonPropertyName("LineLabels")]
        public Dictionary<string, LineLabel> LineLabels { get; set; }

        /// <summary>
        /// Anfangs- und Endzeit des Wettkampfes
        /// </summary>
        [JsonPropertyName("DayTimes")]
        public Dictionary<string, Daytimes> DayTimes { get; set; }

        [JsonPropertyName("LiveDate")]
        public string LiveDate { get; set; }

        /// <summary>
        /// Jeder Abschnitt (Semi-, Final) aller Disziplinen 
        /// </summary>
        [JsonPropertyName("Units")]
        public Dictionary<string, EventSchedule> Units { get; set; }

        [JsonPropertyName("ListEventFirstKey")]
        public string ListEventFirstKey { get; set; }

        /// <summary>
        /// Alle Disziplinen und ihre Details
        /// </summary>
        [JsonPropertyName("ListEvent")]
        public Dictionary<string, EventSummary> ListEvent { get; set; }
        /// <summary>
        /// Alle Veranstaltungstage
        /// </summary>
        [JsonPropertyName("ListDay")]
        public List<string> ListDay { get; set; }

        [JsonPropertyName("ListPhase")]
        public List<Phase> ListPhase { get; set; }

        [JsonPropertyName("ListLocation")]
        public List<Location> ListLocation { get; set; }
        /// <summary>
        /// Alle Geschlechter
        /// </summary>
        [JsonPropertyName("ListGender")]
        public List<Gender> ListGender { get; set; }
    }
    /// <summary>
    /// Start- und Endzeit des Wettkampfes
    /// </summary>
    public class Daytimes
    {
        [JsonPropertyName("StartTime")]
        public long StartTime { get; set; }

        [JsonPropertyName("EndTime")]
        public long EndTime { get; set; }
    }

    public class LineLabel
    {
        [JsonPropertyName("Name")]
        public string Name { get; set; }

        [JsonPropertyName("ListIndex")]
        public int ListIndex { get; set; }
    }
    /// <summary>
    /// Phase einer Disziplin?
    /// </summary>
    public class Phase
    {
        [JsonPropertyName("Group")]
        public string Group { get; set; }

        [JsonPropertyName("Name")]
        public string Name { get; set; }
    }
    /// <summary>
    /// Keine Infos vorhanden
    /// </summary>
    public class Location
    {
    }
    /// <summary>
    /// Geschlecht eines Athleten in einer Disziplin
    /// </summary>
    public class Gender
    {
        [JsonPropertyName("Code")]
        public string Code { get; set; }

        [JsonPropertyName("Name")]
        public string Name { get; set; }
    }
    /// <summary>
    /// Kurze Zusammenfassung der Details einer Disziplin
    /// </summary>
    public class EventSummary
    {
        [JsonPropertyName("Code")]
        public string Code { get; set; }

        [JsonPropertyName("ListIndex")]
        public int ListIndex { get; set; }

        [JsonPropertyName("Name")]
        public string Name { get; set; }

        [JsonPropertyName("Name_Translations")]
        public Dictionary<string, string> NameTranslations { get; set; }

        [JsonPropertyName("ListDay")]
        public List<string> ListDay { get; set; }

        [JsonPropertyName("ListPhase")]
        public List<Phase> ListPhase { get; set; }

        [JsonPropertyName("ListLocation")]
        public List<Location> ListLocation { get; set; }

        [JsonPropertyName("ListSession")]
        public List<object> ListSession { get; set; }
    }
    /// <summary>
    /// Intterface for the common properties of the two
    /// classes <see cref="EventSchedule"/> and 
    /// <see cref="EventDetails"/>
    /// </summary>
    public interface IEventDetails
    {
        Rsc Rsc { get; set; }

        string EventName { get; set; }
        Dictionary<string, string> EventNameTranslations { get; set; }

        string PhaseName { get; set; }
        Dictionary<string, string> PhaseNameTranslations { get; set; }

        string UnitName { get; set; }
        Dictionary<string, string> UnitNameTranslations { get; set; }

        string EventNameShort { get; set; }

        string Status { get; set; }

        long StartTime { get; set; }
        long EndTime { get; set; }

        string Medal { get; set; }

        Stats Stats { get; set; }
    }

    /// <summary>
    /// Zeitplan Infos zu einer Disziplin
    /// </summary>
    public class EventSchedule : IEventDetails
    {
        [JsonPropertyName("Rsc")]
        public Rsc Rsc { get; set; }

        [JsonPropertyName("Indexes")]
        public Index Indexes { get; set; }

        [JsonPropertyName("MatchNumber")]
        public string MatchNumber { get; set; }

        [JsonPropertyName("EventName")]
        public string EventName { get; set; }

        [JsonPropertyName("EventNameShort")]
        public string EventNameShort { get; set; }

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

        [JsonPropertyName("Date")]
        public string Date { get; set; }

        [JsonPropertyName("StartTime")]
        public long StartTime { get; set; }

        [JsonPropertyName("EndTime")]
        public long EndTime { get; set; }

        [JsonPropertyName("ChildCount")]
        public int ChildCount { get; set; }

        [JsonPropertyName("Session")]
        public string Session { get; set; }

        [JsonPropertyName("Status")]
        public string Status { get; set; }

        [JsonPropertyName("Medal")]
        public string Medal { get; set; }

        [JsonPropertyName("HasMedal")]
        public bool HasMedal { get; set; }

        [JsonPropertyName("Stats")]
        public Stats Stats { get; set; }

        [JsonPropertyName("Winner")]
        public List<Athlete> Winner { get; set; }
    }
    /// <summary>
    /// Keine Infos dazu?
    /// </summary>
    public class Index
    {
        [JsonPropertyName("All")]
        public AllIndices All { get; set; }

        [JsonPropertyName("Evt")]
        public EventIndices Evt { get; set; }
    }

    public class EventIndices
    {
        [JsonPropertyName("All")]
        public int All { get; set; }

        [JsonPropertyName("Day")]
        public int Day { get; set; }

        [JsonPropertyName("Phase")]
        public int Phase { get; set; }

        [JsonPropertyName("Loc")]
        public int Loc { get; set; }

        [JsonPropertyName("Session")]
        public int Session { get; set; }
    }
    public class AllIndices
    {
        [JsonPropertyName("All")]
        public int All { get; set; }

        [JsonPropertyName("Day")]
        public int Day { get; set; }

        [JsonPropertyName("Phase")]
        public int Phase { get; set; }

        [JsonPropertyName("Loc")]
        public int Loc { get; set; }

        [JsonPropertyName("Session")]
        public int Session { get; set; }
    }

}
