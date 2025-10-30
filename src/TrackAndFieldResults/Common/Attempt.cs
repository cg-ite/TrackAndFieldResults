/*
 * SPDX - FileCopyrightText: Copyright © 2025 Olympiastützpunkt Hessen <cguenther@lsbh.de>
 * SPDX - License - Identifier: GPL - 3.0 - or - later
 * code was sent as patch, no public git repo available
 */
using TrackAndFieldResults.Omega;

namespace TrackAndFieldResults.Common
{
    /// <summary>
    /// Ein Versuch einer technischen Disziplin.
    /// Dient dazu eine Liste aller Versuche zu
    /// erzeugen.
    /// </summary>
    public class Attempt
    {
        /// <summary>
        /// Athlete
        /// </summary>
        public string AthleteId { get; set; }
        /// <summary>
        /// Initial startposition of the Athlete
        /// for calculatiing the full attemptlist.
        /// 0-based
        /// </summary>
        //public int InitialStartposition { get; set; }

        /// <summary>
        /// Attempt number. For vertical jumps, all 
        /// attempts of a height are counted onwards of 1
        /// </summary>
        public int? Number { get; set; }
        public decimal? Wind { get; set; }

        /// <summary>
        /// Height in vertical jumps or width in horizontal jumps
        /// </summary>
        public string FormattedResult()
        {
            if (Result.HasValue == false) { return ""; }
            return Result.Value.ToString("0.00");
        }

        public string FormattedWind()
        {
            if (Wind.HasValue == false) return "";
            return Wind.Value.ToString("+0.0;-0.0");
        }

        /// <summary>
        /// Abstand am Brett bei Weit- und Dreisprung
        /// </summary>
        public decimal? Behind { get; set; }
        
        public DateTimeOffset? StartTime { get; set; }
        public bool? IsBest { get; set; }

        /// <summary>
        /// Bei Weit, Wurf:6.34, Bei Hoch: xo-, 
        /// bei Lauf: 3:23,21
        /// </summary>

        /// <summary>
        /// Typ des Ergebnisses
        /// </summary>
        public Type Type { get; set; }
        /// <summary>
        /// Ergebnis zum Rechnen, für die Darstellung funktioniert
        /// FormatedResult.
        /// Wenn Leistungen vergleichen werden sollen.
        /// Sprünge: Weite oder Höhe in m
        /// Würfe: Weite in m
        /// Läufe: Zeit in sec
        /// </summary>
        public decimal? Result { get; set;}

        public static Attempt FromIntermediate(Intermediate intermediate, string athletId,
            Type type)
        {
            Attempt attempt = new Attempt();
            attempt.AthleteId = athletId;
            attempt.Type = type;
            attempt.Number = intermediate.Number;
            
            var result = intermediate.Result;
            // Zeiten können nicht direkt zu decimal geparst werden
            if (attempt.Type == Type.Run || attempt.Type == Type.Relay)
            {
                if (TimeSpan.TryParse(result, out TimeSpan ts))
                {
                    attempt.Result = (decimal)ts.TotalSeconds;
                }
            }
            else
            {
                // alles andere sind Weiten/Höhen m
                if (decimal.TryParse(result, out decimal res))
                {
                    attempt.Result = res;
                }
            }
            if (Decimal.TryParse(intermediate.Wind, out decimal wind)) { 
                attempt.Wind = wind;
            }
            if (Decimal.TryParse(intermediate.Behind, out decimal behind)) { 
                attempt.Behind = behind;
            }
            if (intermediate.Flag != null)
            {
                attempt.IsBest = intermediate.Flag == "1";
            }

            return attempt;
        }
    }
}
