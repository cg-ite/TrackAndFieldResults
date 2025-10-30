/*
 * SPDX - FileCopyrightText: Copyright © 2025 Olympiastützpunkt Hessen <cguenther@lsbh.de>
 * SPDX - License - Identifier: GPL - 3.0 - or - later
 * code was sent as patch, no public git repo available
 */
using System.Collections.Generic;
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
        public EventStatus Status { get
            {
                var now = DateTime.Now;
                if(StartDate.HasValue == false && EndDate.HasValue == false) 
                    { return EventStatus.Unknown; }
                if(StartDate.Value > now) 
                    { return EventStatus.Pending; }
                if(StartDate.Value < now && EndDate.Value > now) 
                    { return EventStatus.Started; }
                if(EndDate.Value < now) 
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
        public SortedDictionary<int, string>[] Startorders { get; set; }

        /// <summary>
        /// Alle durchgeführten Versuche in der richtigen
        /// Reihenfolge, bei technischen Disziplinen. 
        /// </summary>
        /// <remarks>Kann nach dem Wettkampf nicht rekonstruiert werden.</remarks>
        //public IPerformance[] GetAttemptsInCompetitionOrder(
        //    SortedDictionary<int, string[]> startorders)
        //{

        //}

        /// <summary>
        /// All attempts of a field competition, auch verzichtetet oder abgemeeldete 
        /// Versuche. Keine Versuche bei Läufen, diese Ergebnisse sind in Results
        /// </summary>
        public Attempt[] Attemps {  get; set; }
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
                Type = Enum.Parse<Type>(eventDetails.Stats.Type, true)
            };

            if (eventDetails is EventDetails)
            {
                var evtDetails = (EventDetails) eventDetails;
                // wir haben ein EventDetail aus einem Schedule bekommen
                // CompDetails sind nicht vorhanden -> für ScheduleItem Cast

                // Athleten
                evt.Athletes = evtDetails.CompetitorDetails.
                    Select(c => Athlete.FromAthlete(c.Value)).ToArray();

                // Nur technische Disziplinen haben Startlisten und Versuche
                if (evt.Type == Type.Height || evt.Type == Type.Width)
                {
                    // können null sein
                    evtDetails.AttemptSeparators = evtDetails.AttemptSeparators == null ?
                        Array.Empty<int>() : evtDetails.AttemptSeparators;

                    //if(evt.Status == EventStatus.Unknown) { return evt; }
                    
                    var startlist = evtDetails.Startlist;
                    evt.Attemps = evtDetails.CompetitorDetails.SelectMany(
                        d => d.Value.Intermediate,
                        (a, i) =>
                        {
                            return Attempt.FromIntermediate(i, a.Key, evt.Type);
                        }).ToArray();

                }
                // Läufe haben keine Startlisten
                return evt;
            }
            return evt;
        }
    }
}
