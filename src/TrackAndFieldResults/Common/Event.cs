/*
 * SPDX - FileCopyrightText: Copyright © 2025 Olympiastützpunkt Hessen <cguenther@lsbh.de>
 * SPDX - License - Identifier: GPL - 3.0 - or - later
 * code was sent as patch, no public git repo available
 */
using System.Collections.Generic;
using System.Globalization;
using TrackAndFieldResults.Omega;

namespace TrackAndFieldResults.Common
{
    /// <summary>
    /// Status of an event. Determined through the Startime
    /// </summary>
    public enum EventStatus
    {
        Unknown = 0,
        Pending,
        Started,
        Finished
    }
    /// <summary>
    /// Eine Disziplin innerhalb eines Wettkampfes
    /// </summary>
    public partial class Event : ScheduleItem, IEvent
    {
        /// <summary>
        /// Gibt den Status an Hand der Startzeit zurück
        /// </summary>
        public EventStatus Status
        {
            get
            {
                var now = DateTime.Now;
                if (StartDate.HasValue == false && EndDate.HasValue == false)
                { return EventStatus.Unknown; }
                if (StartDate.Value > now)
                { return EventStatus.Pending; }
                if (StartDate.Value < now && EndDate.Value > now)
                { return EventStatus.Started; }
                if (EndDate.Value < now)
                { return EventStatus.Finished; }
                return EventStatus.Unknown;
            }
        }

        /// <summary>
        /// Versuchsnummern nach den neu sortiert wird
        /// </summary>
        public int[] AttemptSeparators { get; set; }
        public ICollection<IEntry> Entries { get; set; }
        /// <summary>
        /// Ein bis vier Startlisten, ja nach Disziplin und Modus
        /// Bei technischen Disziplinen 2-4, durch Neusortierung nach den
        /// Versuchen
        /// </summary>
        public SortedDictionary<string, int>[] Startorders { get; set; }

        /// <summary>
        /// Alle durchgeführten Versuche in der richtigen
        /// Reihenfolge, bei technischen Disziplinen. 
        /// Je nach Anbieter muss entschieden werden, woher die
        /// Daten kommen.
        /// </summary>
        /// <remarks>Kann nach dem Wettkampf nur mit der ersten Startreihenfolge 
        /// rekonstruiert werden. Diese muss vor dem Wettkampf gespeichert werden.</remarks>
        public Attempt[] GetAttemptsInCompetitionOrder(
            SortedDictionary<string, int>[] startorders)
        {
            if(Type != Type.Width)
            {
                return Attempts;
            }

            Startorders= startorders;
            var keys = GetWidthKeys(Attempts, startorders);
            
            var res = new List<Attempt>();
            // MK ohne umsortieren
            if (AttemptSeparators.Length == 0)
            {
                var vorkampf = Attempts
                    .OrderBy(a => a.Number)
                    .ThenBy(a => GetInitialStartposition(startorders, a.AthleteId));
                res.AddRange(vorkampf); // Vorkampf
                return res.ToArray();
            }
            // Standard: Weit, Kugel, ...; 3,5
            var sorted = Attempts.Where(a => a.Number <= AttemptSeparators[0])
                    .OrderBy(a => a.Number)
                    .ThenBy(a => GetInitialStartposition(startorders, a.AthleteId));
            res.AddRange(sorted); // Vorkampf

            var endkampfPosition = keys.Where(a => a.PositionResult <= AttemptSeparators[0])
                    .OrderBy(a => a.Result)
                    .ThenBy(a => a.PositionResult)
                    .ThenBy(a => a.PositionStart)
                    .Reverse().DistinctBy(a => a.AthleteId).Take(8).Reverse();

            for (var i = 0; i < AttemptSeparators[1] - AttemptSeparators[0]; i++)
            {
                foreach (var p in endkampfPosition)
                {
                    res.Add(Attempts
                        .Where(a => a.Number == AttemptSeparators[0] + i + 1 &&
                        a.AthleteId == p.AthleteId).First());
                }
            }

            var finale = keys.Where(a => a.PositionResult <= AttemptSeparators[1])
                    .OrderBy(a => a.Result)
                    .ThenBy(a => a.PositionResult)
                    .ThenBy(a => a.PositionStart)
                    .Reverse().DistinctBy(a => a.AthleteId).Take(8).Reverse();

            foreach (var p in finale)
            {
                res.Add(Attempts
                    .Where(a => a.Number == AttemptSeparators[1] + 1 &&
                    a.AthleteId == p.AthleteId).First());
            }
            return res.ToArray();
        }

        private int GetInitialStartposition(SortedDictionary<string, int>[] startorders,
            string athleteId)
        {
            return startorders[0][athleteId];
        }

        public WidthSortKey[] GetWidthKeys(Attempt[] attempts,
            SortedDictionary<string, int>[] startorders)
        {
            var widthkeys = attempts.Select(a =>
                new WidthSortKey(
                    a.Result.Value, 
                    GetInitialStartposition(startorders, a.AthleteId),
                    a.Number.Value, a.AthleteId));
            return widthkeys.ToArray();
        }

        /// <summary>
        /// All attempts of a field competition, auch verzichtetet oder abgemeeldete 
        /// Versuche. Keine Versuche bei Läufen, diese Ergebnisse sind in Results
        /// </summary>
        public Attempt[] Attempts { get; set; }
        /// <summary>
        /// Bester Versuch von jedem Athleten, das beste Ergebnis.
        /// Die Ergebnisliste
        /// </summary>
        public IPerformance[] Results { get; set; }
        public Athlete[] Athletes { get; set; }
        public override string ToString()
        {
            return $"{Longname} - {Agegroups.First().Shortcode} {Unit}";
        }

        public static Event FromEventDetails(IEventDetails eventDetails, string language = "de")
        {
            var name = eventDetails.EventName;
            var phaseName = eventDetails.PhaseName;

            if (eventDetails.PhaseNameTranslations.Count() > 0 &&
                    eventDetails.PhaseNameTranslations.ContainsKey(language))
            {

                phaseName = eventDetails.PhaseNameTranslations[language];
            }

            if (eventDetails.EventNameTranslations.Count() > 0 &&
                eventDetails.EventNameTranslations.ContainsKey(language))
            {

                name = eventDetails.EventNameTranslations[language];
            }

            var unitName = eventDetails.UnitName;
            if (eventDetails.UnitNameTranslations.Count() > 0 &&
                    eventDetails.UnitNameTranslations.ContainsKey(language))
            {

                unitName = eventDetails.UnitNameTranslations[language];
            }

            // ValuePhase ist key um nach Heats zu suchen
            // bei deutschen und Ratingen gibt es nur eine AK
            var evt = new Event()
            {
                Longname = $"{name} {phaseName} {unitName}",
                StartDate = eventDetails.StartTime.ToDateTime(),
                EndDate = eventDetails.EndTime.ToDateTime(),
                Agegroups = eventDetails.Rsc.Gender.ToUpper() == "W" ?
                    [GerAgegroups.All("W")] :
                    [GerAgegroups.All("M")],
                ProviderId = eventDetails.Rsc.ValueUnit,
                Unit = unitName,
                Phase = phaseName,
                Name = name,
                Type = Enum.Parse<Type>(eventDetails.Stats.Type, true),
            };

            // Attempts 
            if (eventDetails is EventDetails)
            {
                var evtDetails = (EventDetails)eventDetails;
                // wir haben ein EventDetail aus einem Schedule bekommen
                // CompDetails sind nicht vorhanden -> für ScheduleItem Cast

                // Athleten
                evt.Athletes = evtDetails.CompetitorDetails.
                    Select(c => Athlete.FromAthlete(c.Value)).ToArray();

                // Nur technische Disziplinen haben Startlisten und Versuche

                // Bei Hoch und Stabhochsprung jeden offiziellen "Versuch"
                // teilen, da hier eine Höhe als Versuch gewertet wird

                if (evt.Type == Type.Height)
                {
                    var heights = evtDetails.Splits;

                    var attemptsTemp = evtDetails.CompetitorDetails.SelectMany(
                        d => d.Value.Intermediate,
                        (a, i) =>
                        {
                            return new
                            {
                                Athlete = a,
                                Intermediate = i
                            };
                        })
                        .Where(at => at.Intermediate.Result != "-" && at.Intermediate.Result != null)
                        .OrderBy(at => at.Intermediate.Number)
                        .ThenBy(at => at.Athlete.Value.StartPos);
                    ;
                    evt.Attempts = attemptsTemp.SelectMany(
                                    at => at.Intermediate.Result.ToCharArray()
                                        .Select((c, i) => new { c, i }),
                                    (e, a) =>
                                    {
                                        // hack 
                                        e.Intermediate.Result = a.c.ToString();
                                        var att = Attempt.FromIntermediate(e.Intermediate,
                                            e.Athlete.Key, evt.Type);
                                        att.Number = a.i + 1;
                                        if (Decimal.TryParse(heights[e.Intermediate.Number - 1].Name, out decimal height))
                                        {
                                            att.Height = height;
                                        }
                                        return att;
                                    })
                                    .OrderBy(a => a.Height)
                                    .ThenBy(a => a.Number)
                                    .ToArray();

                    // Startorder wird sich nicht verändern im Wettkampf
                    evt.Startorders = GetStarlist(evtDetails.Startlist);
                }
                if (evt.Type == Type.Width)
                {
                    // können null sein
                    evtDetails.AttemptSeparators = evtDetails.AttemptSeparators == null ?
                        Array.Empty<int>() : evtDetails.AttemptSeparators;

                    var startlist = evtDetails.Startlist;
                    evt.Attempts = evtDetails.CompetitorDetails.SelectMany(
                        d => d.Value.Intermediate,
                        (a, i) =>
                        {
                            return new { Athlete = a, Intermediate = i};
                        })
                        .Where(at => at.Intermediate.Result != null)
                        .Select(dto => 
                            Attempt.FromIntermediate(dto.Intermediate, 
                            dto.Athlete.Key, evt.Type))
                        .ToArray();

                    // für die Startlisten gilt:
                    if (evt.Status == EventStatus.Pending && 
                        (evt.Status == EventStatus.Started && evt.Attempts.Max(a => a.Number) - 1 < evt.AttemptSeparators[1]))
                    {
                        // - vorher: Startlist[0] gültig
                        // - bis zur ersten Umsortierung:  Startlist[0] gültig
                        evt.Startorders = GetStarlist(evtDetails.Startlist);
                        
                    }
                    if(evt.Status == EventStatus.Unknown || 
                        (evt.Status == EventStatus.Started && evt.Attempts.Max(a => a.Number) - 1 >= evt.AttemptSeparators[1])) 
                    {
                        // -danach bis zum Ende: keine gültig
                        return evt; 
                    }
                    if (evt.Status != EventStatus.Finished)
                    {
                        // - danach: genau genommen, gar keine Startlisten gültig
                        // da bei weitengleichheit die erste Startpos benötigt wird.
                        return evt;
                    }
                }
                if (evt.Type == Type.Run)
                {
                    //evtDetails.Startlist != null
                    // Startorder ist Bahneinteilung im Lauf oder Aufstellungsposition bei
                    // längeren Läufen > 1500m
                    evt.Startorders = GetStarlist(evtDetails.Startlist);
                }
                return evt;
            }
            

            return evt;
        }

        private static SortedDictionary<string, int>[] GetStarlist(Dictionary<string, StartlistEntry> entries)
        {
            SortedDictionary<string, int>[] res = [new SortedDictionary<string, int>()];
            res[0] = new SortedDictionary<string, int>(entries
                            .Select(sl => new { i = sl.Value.ListIndex + 1, Key = sl.Key })
                            .ToDictionary(p => p.Key, p => p.i));
            return res;
        }
    }

    public class WidthSortKey
    {
        public WidthSortKey(decimal Result, int PositionStart, int PositionResult, string AthleteId)
        {
            this.Result = Result;
            this.PositionStart = PositionStart;
            this.PositionResult = PositionResult;
            this.AthleteId = AthleteId;
            this.Positions.Add(PositionStart);
        }

        public decimal Result { get; }
        public int PositionStart { get; }
        public int PositionResult { get; }
        public string AthleteId { get; }

        public List<int> Positions = new List<int>();
    }
}
