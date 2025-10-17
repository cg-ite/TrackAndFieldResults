/*
 * SPDX - FileCopyrightText: Copyright © 2025 Olympiastützpunkt Hessen <cguenther@lsbh.de>
 * SPDX - License - Identifier: GPL - 3.0 - or - later
 * code was sent as patch, no public git repo available
 */
namespace TrackAndFieldResults.Common
{
    /// <summary>
    /// Ein Wettkampf
    /// </summary>
    public interface ICompetition
    {
        /// <summary>Local database Id</summary>
        long Id { get; set; }
        /// <summary>
        /// Id of the competition from the provider (seltec, Omega, ...)
        /// </summary>
        string ProviderId { get; set; }
        /// <summary>
        /// Provider of the result data
        /// </summary>
        Provider Provider { get; set; }
        
        /// <summary>Name of competition</summary>
        string Name { get; set; }
        /// <summary>First day of competition</summary>
        DateTime StartDate { get; set; }

        /// <summary>Last day of competition</summary>
        DateTime EndDate { get; set; }

        /// <summary>Name of town where competition takes place</summary>
        string Town { get; set; }

        /// <summary>3 Letter IOC/ISO Code for Nation. E.g. GER</summary>
        string Nation { get; set; }

        /// <summary>List of competition events</summary>
        //ICollection<IEvent> Events { get; set; }
    }
}
