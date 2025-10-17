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
    public interface IEvent
    {
        /// <summary>ID inside Database</summary>
        long Id { get; set; }
        /// <summary>
        /// Provider specific Id of the event to
        /// get the details/attempts of the 
        /// event
        /// </summary>
        public string ProviderId { get; set; }

        /// <summary>
        /// ID of the corresponding competition
        /// </summary>
        long CompetitinonId { get; set; }
        /// <summary>Freetext name of Event, in local language</summary>
        string Longname { get; set; }
        /// <summary>
        /// Runde oder Lauf des Events: 1. Lauf, Finale, ...
        /// </summary>
        public Heat Heat { get; set; }
        /// <summary>
        /// Riege des Events
        /// </summary>
        public string Squad { get; set; }


        /// <summary>First day of event</summary>
        DateTime? StartDate { get; set; }

        /// <summary>Last day of event</summary>
        DateTime? EndDate { get; set; }
        /// <summary>List of agegroups that will compete in this event</summary>
        ICollection<IAgegroup> Agegroups { get; set; }

        /// <summary>List of entries/w results inside this event</summary>
        ICollection<IEntry> Entries { get; set; }
    }
}
