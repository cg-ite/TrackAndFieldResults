/*
 * SPDX - FileCopyrightText: Copyright © 2025 Christian Günther <cg-ite@gmx.de>
 * SPDX - License - Identifier: GPL - 3.0 - or - later
 */

using System.Text.Json;

namespace TrackAndFieldResults.Atos
{
    /// <summary>
    /// Parst die verschiedenen json Dateien des Omega
    /// Ergebnis-Dienst
    /// </summary>
    public class AtosJsonFile
    {
        /// <summary>
        /// Gibt die Details eines Wettkampfes zurück
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public AthleteEventRoot GetEvent(string path)
        {

            using (StreamReader r = new StreamReader(path))
            {
                string json = r.ReadToEnd();
                return JsonSerializer.Deserialize<AthleteEventRoot>(json);
            }
        }
    }
}
