/*
 * SPDX - FileCopyrightText: Copyright © 2025 Olympiastützpunkt Hessen <cguenther@lsbh.de>
 * SPDX - License - Identifier: GPL - 3.0 - or - later
 * code was sent as patch, no public git repo available
 */
namespace TrackAndFieldResults.Common
{
    public interface IAgegroup
    {
        /// <summary>ID inside Document or Database</summary>
        long Id { get; set; }
        /// <summary>Freetext name of Event, in local language</summary>
        string Longname { get; set; }
        /// <summary>Gender of this agegroup. If not determined, use mixed</summary>
        Gender Gender { get; set; }

        /// <summary>National shortcode</summary>
        string Shortcode { get; set; }

        /// <summary>Minimum age this agegroup will allow competitors</summary>
        int FromAge { get; set; }

        /// <summary>Maximum age this agegroup will allow competitors</summary>
        int ToAge { get; set; }

        /// <summary>Height of Hurdles in Centimeters (cm) for this agegroup inside this event</summary>
        int? Height { get; set; }

        /// <summary>Weight of throwing equipment in grams (g) for this agegroup inside this event</summary>
        int? Weight { get; set; }

        /// <summary>Distance between Hurdles in Centimeters (cm) for this agegroup inside this event</summary>
        int? Distance { get; set; }
    }
}
