/*
 * SPDX - FileCopyrightText: Copyright © 2025 Olympiastützpunkt Hessen <cguenther@lsbh.de>
 * SPDX - License - Identifier: GPL - 3.0 - or - later
 * code was sent as patch, no public git repo available
 */
using System.Reflection.Metadata.Ecma335;
using TrackAndFieldResults.Omega;
using TrackAndFieldResults.Seltec;

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
        public string ProviderId { get; set; }

        public string Longname => $"{Name} {Phase} {Unit}";
        public Type Type { get; set; }

        public string Unit { get; set; }
        public string Phase { get; set; }
        public string Name { get; set; }

        public override string ToString()
        {
            return $"{Longname} - {Agegroups.First().Shortcode}";
        }

        public static ScheduleItem FromEventDetails(IEventDetails evt, string language = "de")
        {
            return (ScheduleItem)Event.FromEventDetails(evt, language);
        }
        public static ScheduleItem[] FromEventDetails(AthonEvent eventDetails)
        {
            // evt hat in entries alle phasen in einem Array
            var heats = eventDetails.Entries.Where(e=> e.Heat != null).GroupBy(e => new { e.RoundType, e.Heat }, evt => evt)
                .OrderBy(ev => ev.Key.Heat)
                .OrderBy(ev => ev.Key.RoundType);
            return heats.Select(e => (ScheduleItem)Event.FromEventDetails(eventDetails, e.First())).ToArray();
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
        Height,
        Width, 
        Run,
        Relay
    }
}