/*
 * SPDX - FileCopyrightText: Copyright © 2025 Christian Günther <cg-ite@gmx.de>
 * SPDX - License - Identifier: GPL - 3.0 - or - later
 */

using System.Text.Json.Serialization;

namespace TrackAndFieldResults.Atos
{
    // thanks to https://json2csharp.com/
    public partial class AnalysisExtendedResults
    {
    }

    public partial class Athlete
    {
        [JsonPropertyName("CompetitorCode")]
        public string CompetitorCode { get; set; }

        [JsonPropertyName("Order")]
        public int Order { get; set; }

        [JsonPropertyName("Bib")]
        public string Bib { get; set; }

        [JsonPropertyName("Description")]
        public Description Description { get; set; }

        [JsonPropertyName("EventUnitEntry")]
        public List<EventUnitEntry> EventUnitEntry { get; set; }

        [JsonPropertyName("ExtendedEntry")]
        public List<ExtendedEntry> ExtendedEntry { get; set; }

        [JsonPropertyName("University")]
        public University University { get; set; }
    }

    public partial class Composition
    {
        [JsonPropertyName("Description")]
        public Description Description { get; set; }

        [JsonPropertyName("Athlete")]
        public List<Athlete> Athlete { get; set; }
    }

    public partial class CompositionDescription
    {
    }

    public partial class AthleteEventDetails
    {
        [JsonPropertyName("Code")]
        public string Code { get; set; }

        [JsonPropertyName("DocSubCode")]
        public string DocSubCode { get; set; }

        [JsonPropertyName("DisciplineCode")]
        public string DisciplineCode { get; set; }

        [JsonPropertyName("GenderCode")]
        public string GenderCode { get; set; }

        [JsonPropertyName("EventCode")]
        public string EventCode { get; set; }

        [JsonPropertyName("PhaseCode")]
        public string PhaseCode { get; set; }

        [JsonPropertyName("ParentUnitCode")]
        public string ParentUnitCode { get; set; }

        [JsonPropertyName("SubUnitCode")]
        public string SubUnitCode { get; set; }

        [JsonPropertyName("UnitNum")]
        public object UnitNum { get; set; }

        [JsonPropertyName("UnitDescription")]
        public string UnitDescription { get; set; }

        [JsonPropertyName("UnitLongDescription")]
        public string UnitLongDescription { get; set; }

        [JsonPropertyName("UnitPrintDescription")]
        public string UnitPrintDescription { get; set; }

        [JsonPropertyName("Wind")]
        public object Wind { get; set; }

        [JsonPropertyName("DisciplineInfos")]
        public object DisciplineInfos { get; set; }

        [JsonPropertyName("ResultStatus")]
        public string ResultStatus { get; set; }

        [JsonPropertyName("ODFResultStatus")]
        public string ODFResultStatus { get; set; }

        [JsonPropertyName("ScheduleStatus")]
        public string ScheduleStatus { get; set; }

        [JsonPropertyName("DateEvent")]
        public DateTime DateEvent { get; set; }

        [JsonPropertyName("LogicalDay")]
        public int LogicalDay { get; set; }

        [JsonPropertyName("StartOrder")]
        public int StartOrder { get; set; }

        [JsonPropertyName("StartSortOrder")]
        public int StartSortOrder { get; set; }

        [JsonPropertyName("StartCode")]
        public string StartCode { get; set; }

        [JsonPropertyName("SortOrder")]
        public int SortOrder { get; set; }

        [JsonPropertyName("Rank")]
        public string Rank { get; set; }

        [JsonPropertyName("RankEqual")]
        public object RankEqual { get; set; }

        [JsonPropertyName("Result")]
        public string Result { get; set; }

        [JsonPropertyName("Unchecked")]
        public object Unchecked { get; set; }

        [JsonPropertyName("IRM")]
        public object IRM { get; set; }

        [JsonPropertyName("WLT")]
        public object WLT { get; set; }

        [JsonPropertyName("ResultType")]
        public string ResultType { get; set; }

        [JsonPropertyName("ResultTypeDescription")]
        public string ResultTypeDescription { get; set; }

        [JsonPropertyName("ResultCode")]
        public object ResultCode { get; set; }

        [JsonPropertyName("ResultCodeDescription")]
        public object ResultCodeDescription { get; set; }

        [JsonPropertyName("Diff")]
        public string Diff { get; set; }

        [JsonPropertyName("QualificatorMark")]
        public object QualificatorMark { get; set; }

        [JsonPropertyName("QualificationGroup")]
        public object QualificationGroup { get; set; }

        [JsonPropertyName("ResultPty")]
        public object ResultPty { get; set; }

        [JsonPropertyName("MedalType")]
        public string MedalType { get; set; }

        [JsonPropertyName("NameSortOrder")]
        public int NameSortOrder { get; set; }

        [JsonPropertyName("AthleteNameSortOrder")]
        public int AthleteNameSortOrder { get; set; }

        [JsonPropertyName("SecondAthleteNameSortOrder")]
        public int SecondAthleteNameSortOrder { get; set; }

        [JsonPropertyName("NocSortOrder")]
        public int NocSortOrder { get; set; }

        [JsonPropertyName("BibSortOrder")]
        public int BibSortOrder { get; set; }

        [JsonPropertyName("CompetitorCode")]
        public string CompetitorCode { get; set; }

        [JsonPropertyName("CompetitorType")]
        public string CompetitorType { get; set; }

        [JsonPropertyName("CompetitorBib")]
        public object CompetitorBib { get; set; }

        [JsonPropertyName("OrganisationCode")]
        public string OrganisationCode { get; set; }

        [JsonPropertyName("Composition")]
        public Composition Composition { get; set; }

        [JsonPropertyName("CompositionDescription")]
        public CompositionDescription CompositionDescription { get; set; }

        [JsonPropertyName("CompositionCoaches")]
        public object CompositionCoaches { get; set; }

        [JsonPropertyName("CompositionEventUnitEntry")]
        public object CompositionEventUnitEntry { get; set; }

        [JsonPropertyName("CompositionStatsItems")]
        public object CompositionStatsItems { get; set; }

        [JsonPropertyName("ExtendedResults")]
        public ExtendedResults ExtendedResults { get; set; }

        [JsonPropertyName("RecordIndicators")]
        public RecordIndicators RecordIndicators { get; set; }

        [JsonPropertyName("ResultItems")]
        public object ResultItems { get; set; }

        [JsonPropertyName("AnalysisExtendedResults")]
        public AnalysisExtendedResults AnalysisExtendedResults { get; set; }

        [JsonPropertyName("InfoExtendedResults")]
        public List<InfoExtendedResult> InfoExtendedResults { get; set; }

        [JsonPropertyName("Periods")]
        public Periods Periods { get; set; }

        [JsonPropertyName("PhotoFinishStatus")]
        public object PhotoFinishStatus { get; set; }

        [JsonPropertyName("PhotoFinish")]
        public object PhotoFinish { get; set; }

        [JsonPropertyName("OtherImage")]
        public object OtherImage { get; set; }

        [JsonPropertyName("UniformImage")]
        public object UniformImage { get; set; }
    }

    public partial class Description
    {
        [JsonPropertyName("GivenName")]
        public string GivenName { get; set; }

        [JsonPropertyName("FamilyName")]
        public string FamilyName { get; set; }

        [JsonPropertyName("TVName")]
        public string TVName { get; set; }

        [JsonPropertyName("TVInitialName")]
        public string TVInitialName { get; set; }

        [JsonPropertyName("PrintName")]
        public string PrintName { get; set; }

        [JsonPropertyName("PrintInitialName")]
        public string PrintInitialName { get; set; }

        [JsonPropertyName("GenderCode")]
        public string GenderCode { get; set; }

        [JsonPropertyName("OrganisationCode")]
        public string OrganisationCode { get; set; }

        [JsonPropertyName("BirthDate")]
        public string BirthDate { get; set; }
    }

    public partial class EventUnitEntry
    {
        [JsonPropertyName("Type")]
        public string Type { get; set; }

        [JsonPropertyName("Code")]
        public string Code { get; set; }

        [JsonPropertyName("Value")]
        public string Value { get; set; }
    }

    public partial class ExtendedEntry
    {
        [JsonPropertyName("Type")]
        public string Type { get; set; }

        [JsonPropertyName("Code")]
        public string Code { get; set; }

        [JsonPropertyName("Value")]
        public string Value { get; set; }
    }

    public partial class ExtendedResult
    {
        [JsonPropertyName("Type")]
        public string Type { get; set; }

        [JsonPropertyName("Code")]
        public string Code { get; set; }

        [JsonPropertyName("Value")]
        public string Value { get; set; }

        [JsonPropertyName("Value2")]
        public string Value2 { get; set; }

        [JsonPropertyName("Pos")]
        public string Pos { get; set; }

        [JsonPropertyName("Extension")]
        public List<Extension> Extension { get; set; }
    }

    public partial class ExtendedResults
    {
        [JsonPropertyName("ExtendedResult")]
        public List<ExtendedResult> ExtendedResult { get; set; }
    }

    public partial class Extension
    {
        [JsonPropertyName("Code")]
        public string Code { get; set; }

        [JsonPropertyName("Value")]
        public string Value { get; set; }

        [JsonPropertyName("Pos")]
        public string Pos { get; set; }
    }

    public partial class InfoExtendedResult
    {
        [JsonPropertyName("Type")]
        public string Type { get; set; }

        [JsonPropertyName("Code")]
        public string Code { get; set; }

        [JsonPropertyName("Pos")]
        public object Pos { get; set; }

        [JsonPropertyName("Value")]
        public string Value { get; set; }

        [JsonPropertyName("Extension")]
        public object Extension { get; set; }
    }

    public partial class Periods
    {
        [JsonPropertyName("NumPeriods")]
        public int NumPeriods { get; set; }
    }

    public partial class RecordIndicator
    {
        [JsonPropertyName("Order")]
        public string Order { get; set; }

        [JsonPropertyName("Code")]
        public string Code { get; set; }

        [JsonPropertyName("RecordType")]
        public string RecordType { get; set; }
    }

    public partial class RecordIndicators
    {
        [JsonPropertyName("RecordIndicator")]
        public RecordIndicator RecordIndicator { get; set; }
    }

    /// <summary>
    /// Root of a event like short put or long jump
    /// </summary>
    public partial class AthleteEventRoot
    {
        [JsonPropertyName("ResponseCode")]
        public int ResponseCode { get; set; }

        [JsonPropertyName("Message")]
        public string Message { get; set; }

        [JsonPropertyName("ValidityTokenRemainingSeconds")]
        public int ValidityTokenRemainingSeconds { get; set; }

        [JsonPropertyName("TotalRows")]
        public int TotalRows { get; set; }

        [JsonPropertyName("Data")]
        public List<AthleteEventDetails> Data { get; set; }
    }

    public partial class University
    {
        [JsonPropertyName("Name")]
        public string Name { get; set; }

        [JsonPropertyName("Country")]
        public string Country { get; set; }

        [JsonPropertyName("City")]
        public string City { get; set; }

        [JsonPropertyName("Abbreviation")]
        public string Abbreviation { get; set; }

        [JsonPropertyName("Faculty")]
        public string Faculty { get; set; }

        [JsonPropertyName("FieldOfStudy")]
        public string FieldOfStudy { get; set; }

        [JsonPropertyName("YearOfStudy")]
        public string YearOfStudy { get; set; }
    }


}
