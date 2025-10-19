/*
 * SPDX - FileCopyrightText: Copyright © 2025 Olympiastützpunkt Hessen <cguenther@lsbh.de>
 * SPDX - License - Identifier: GPL - 3.0 - or - later
 * code was sent as patch, no public git repo available
 */
namespace TrackAndFieldResults.Common
{
    /// <summary>
    /// Details eines Wettkampfes
    /// </summary>
    public partial class Competition : ICompetition
    {
        /// <summary>
        /// Local Id of the Competition
        /// </summary>
        public long Id { get ; set ; }
        public string Name { get ; set ; }
        public DateTime StartDate { get ; set ; }
        public DateTime EndDate { get ; set ; }
        public string Town { get ; set ; }
        public string Nation { get ; set ; }
        //public ICollection<IEvent> Events { get ; set ; }
        /// <summary>
        /// Providerspecific Id of the competition
        /// </summary>
        public string ProviderId { get ; set ; }
        /// <summary>
        /// Provider which provides the competition
        /// </summary>
        public ProviderId ResultProviderId { get ; set ; }

        public Event[] Schedule {  get ; set ; }
    }

}
