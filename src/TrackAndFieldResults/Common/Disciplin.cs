/*
 * SPDX - FileCopyrightText: Copyright © 2025 Christian Günther <cg-ite@gmx.de>
 * SPDX - License - Identifier: GPL - 3.0 - or - later
 */

namespace TrackAndFieldResults.Common
{
    /// <summary>
    /// TRack and Field Disciplin
    /// </summary>
    public class Disciplin
    {
        public string Name { get; set; }
        public string Shortcode { get; set; }

        public Type Type { get; set; }

        public static FromAthon(string shortcode)
        {
            // könnten vom Server geladen und gespeichert werden
        }

        public static FromEvent()
        {
            // mit deutscher beschreibung und dann
            // aus dem ID
        }

        public bool IsOlympic(string shortcode)
        {
            // nur die interessanten Disziplinen anhand
            // des Shortcodes filtern; später über json in
            // den Einstellungen speichern

        }
    }
}
