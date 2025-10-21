/*
 * SPDX - FileCopyrightText: Copyright © 2025 Olympiastützpunkt Hessen <cguenther@lsbh.de>
 * SPDX - License - Identifier: GPL - 3.0 - or - later
 * code was sent as patch, no public git repo available
 */
using TrackAndFieldResults.Omega;

namespace TrackAndFieldResults.Common
{
    /// <summary>
    /// Inforamtions about an event 
    /// </summary>
    public class ScheduleItem : IScheduleItem
    {
        public ICollection<IAgegroup> Agegroups { get; set; }
        public long CompetitionId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Longname { get; set; }
        public string ProviderId { get; set; }

        public Type Type { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Unit { get; set; }
        public string Phase { get; set; }
        public string Name { get; set; }

        public override string ToString()
        {
            return $"{Longname} - {Agegroups.First().Shortcode}";
        }

        public static ScheduleItem FromEvent(EventSchedule evt, string language = "de")
        {
            // Bei DM ist EventName die Disziplin
            // Bei MK ist EventName Zehnkampf
            // Bei DM ist Phasename Finale o.ä.
            // Bei MK ist Phasename die Disziplin
            // Bei MK und DM sind Unit-Name Finale
            var isCombined = evt.EventName.ToUpper().Contains("DECA") ||
                evt.EventName.ToUpper().Contains("HEPTA") ||
                evt.EventName.ToUpper().Contains("SIEBEN") ||
                evt.EventName.ToUpper().Contains("ZEHN");

            var name = evt.EventName;
            var phaseName = evt.PhaseName;

            if (evt.PhaseName_Translations.Count() > 0 &&
                    evt.PhaseName_Translations.ContainsKey(language))
            {

                phaseName = evt.PhaseName_Translations[language];
            }

            if (evt.EventName_Translations.Count() > 0 &&
                evt.EventName_Translations.ContainsKey(language))
            {

                name = evt.EventName_Translations[language];
            }

            var unitName = evt.UnitName;
            if (evt.UnitName_Translations.Count() > 0 &&
                    evt.UnitName_Translations.ContainsKey(language))
            {

                unitName = evt.UnitName_Translations[language];
            }

            // ValuePhase ist key um nach Heats zu suchen
            // bei deutschen und Ratingen gibt es nur eine AK
            return new ScheduleItem()
            {
                Longname = $"{name} {phaseName} {unitName}",
                StartDate = evt.StartTime.ToDateTime(),
                EndDate = evt.EndTime.ToDateTime(),
                Agegroups = evt.Rsc.Gender.ToUpper() == "W" ?
                    [GerAgegroups.All("W")] :
                    [GerAgegroups.All("M")],
                ProviderId = evt.Rsc.ValueUnit,
                Unit = unitName,
                Phase = phaseName,
                Name = name,
                Type = Enum.Parse<Type>(evt.Stats.Type,true)
            };
        }
    }

    public static class CommonExtentions
    {
        /// <summary>
        /// Wandelt eine long 1688904000000 in ein Datum
        /// Unix-ms
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(this long dt)
        {
            return DateTimeOffset.FromUnixTimeMilliseconds(dt).DateTime;
        }

        public static int ToYoB(this string age)
        {
            if (age == null) return 0;
            if (age.Length == 0) return 0;

            if (Int32.TryParse(age, out int a))
            {
                return DateTime.Now.Year - a;
            }
            return 0;
        }

    }

    public enum Type
    {
        Run,
        Height,
        Width, 
        Relay
    }
}