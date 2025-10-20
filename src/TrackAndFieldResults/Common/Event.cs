/*
 * SPDX - FileCopyrightText: Copyright © 2025 Olympiastützpunkt Hessen <cguenther@lsbh.de>
 * SPDX - License - Identifier: GPL - 3.0 - or - later
 * code was sent as patch, no public git repo available
 */
namespace TrackAndFieldResults.Common
{
    /// <summary>
    /// Eine Disziplin innerhalb eines Wettkampfes
    /// </summary>
    public partial class Event : IEvent
    {
        public ICollection<IAgegroup> Agegroups { get ; set ; }
        public ICollection<IEntry> Entries { get ; set ; }
        public long Id { get ; set ; }
        public string ProviderId { get ; set ; }
        public long CompetitionId { get ; set ; }
        public DateTime? StartDate { get ; set ; }
        public DateTime? EndDate { get ; set ; }
        public string Longname { get ; set ; }
        public Heat Heat { get ; set ; }
        public string Squad { get ; set ; }

        public override string ToString()
        {
            return $"{Longname} - {Agegroups.First().Shortcode} {Heat}";
        }
    }
}
