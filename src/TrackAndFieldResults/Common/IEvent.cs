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
        public int[] AttemptSeparators { get; set; }
        public SortedDictionary<string, int>[] Startorders { get; set; }
        public EventStatus Status { get; }
        public Attempt[] Attempts { get; set; }
        public Attempt[] Results { get; set; }
        public Athlete[] Athletes { get; set; }
    }
}
