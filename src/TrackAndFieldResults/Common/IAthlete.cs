/*
 * SPDX - FileCopyrightText: Copyright © 2025 Olympiastützpunkt Hessen <cguenther@lsbh.de>
 * SPDX - License - Identifier: GPL - 3.0 - or - later
 * code was sent as patch, no public git repo available
 */
namespace TrackAndFieldResults.Common
{
    /// <summary>
    /// Kleinste Menge an Inforationen über einen Athleten
    /// aus mehreren Apis, zum Speichern der Daten in
    /// einer Datenbank unabhängig vom verwendeten 
    /// Api Provider
    /// </summary>
    public interface IAthlete
    {
        /// <summary>
        /// National Id of the Athlete
        /// </summary>
        string NationalId { get; set; }
        /// <summary>
        /// Startnumber for a competition
        /// </summary>
        string Bib { get; set; }
        /// <summary>
        /// Year of Birth
        /// </summary>
        int? YoB { get; set; }
        /// <summary>
        /// Firstname
        /// </summary>
        string Firstname { get; set; }
        /// <summary>
        /// Gender
        /// </summary>
        Gender Gender { get; set; }
        /// <summary>
        /// Lastname
        /// </summary>
        string Name { get; set; }
        /// <summary>
        /// 3 Letter IOC/ISO Code for Nation. E.g. GER 
        /// </summary>
        string Nationality { get; set; }
    }

    public enum Gender
    {
        Male = 0,
        Female = 1,
        Mixed = 2,
        NonBinary = 3,
    }
}