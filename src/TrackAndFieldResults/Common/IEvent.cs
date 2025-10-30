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
        /// <summary>List of entries/w results inside this event</summary>
        ICollection<IEntry> Entries { get; set; }
    }
}
