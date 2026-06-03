/*
 * SPDX - FileCopyrightText: Copyright © 2025 Olympiastützpunkt Hessen <cguenther@lsbh.de>
 * SPDX - License - Identifier: GPL - 3.0 - or - later
 * code was sent as patch, no public git repo available
 */
namespace TrackAndFieldResults.Common
{
    /// <summary>
    /// Die Teilnahme eines Athleten einer Disziplin
    /// </summary>
    public interface IEntry
    {
        /// <summary>ID inside Document or Database</summary>
        string Id { get; set; }
        /// <summary>ID reference to competitor</summary>
        string CompetitorId { get; set; }
        /// <summary>ID reference to agegroup (must be part of this event)</summary>
        string AgegroupId { get; set; }
        /// <summary>Name or number of squad ('Riege')</summary>
        string Squad { get; set; }
        /// <summary>List of attempts or single performance. Can be omitted of this is only an entry before event start</summary>
        Attempt[] Attempts { get; set; }
        /// <summary>Lane Number or Number of original Starting order in technical event. If lane is double set use 1/1 and 1/2 for lane 1 positions</summary>
        string Lane { get; set; }
        /// <summary>Name or number of heat. If omitted but RoundType is present: competitor should be DNS/CAN</summary>
        string Heat { get; set; }
        /// <summary>
        /// Position in technical events
        /// </summary>
        string Startposition { get; set; }
        /// <summary>
        /// Zeigt den Status des Athleten im Wettkampf
        /// </summary>
        EntryState State { get; set; }
    }

    /// <summary>
    /// State eine Athleten in einer Disziplin
    /// Vorher None, nach dem Wettkampf
    /// entsprechend der Leistung
    /// </summary>
    public enum EntryState
    {
        None = 0,
        Set = 1,
        Finished = 2,
        Dnf = 3,
        Can = 4,
        Dns = 5,
        Dsq = 6,
    }
}
