/*
 * SPDX - FileCopyrightText: Copyright © 2025 Olympiastützpunkt Hessen <cguenther@lsbh.de>
 * SPDX - License - Identifier: GPL - 3.0 - or - later
 * code was sent as patch, no public git repo available
 */
using TrackAndFieldResults.Common;
using TrackAndFieldResults.Omega;
using TrackAndFieldResults.Seltec;

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
            if(Type == Type.Height) { return ResultRaw; }
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
        /// Versuch ungültig
        /// </summary>
        public bool IsInvalid { get; set; } = true;
        /// <summary>
        /// Versuch ausgelassen
        /// </summary>
        public bool IsPassed { get; set; }
        /// <summary>
        /// Athlet hat sich vor Wettkampf abgemeldet
        /// </summary>
        public bool IsCanceled { get; set; }
        /// <summary>
        /// Athlet wurde disqualifiziert
        /// </summary>
        public bool IsDisqualified { get; set; }

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
        public decimal? Result { get; set; }
        public string ResultRaw { get; set; }
        public decimal? Height { get; internal set; }

        public static Attempt FromIntermediate(Intermediate intermediate, string athleteId,
            Type type)
        {
            Attempt attempt = new Attempt();
            attempt.AthleteId = athleteId;
            attempt.Type = type;
            attempt.Number = intermediate.Number;

            attempt.ResultRaw = intermediate.Result;
            // Zeiten können nicht direkt zu decimal geparst werden
            if (attempt.Type == Type.Run || attempt.Type == Type.Relay)
            {
                throw new InvalidOperationException("Events of type 'Run' or 'Relay' don't have attempts.");
            }

            attempt.IsInvalid = attempt.ResultRaw.ToLower() == "x";
            attempt.IsPassed = attempt.ResultRaw.ToLower() == "-";
            attempt.IsBest = attempt.ResultRaw.ToLower() == "o";
            attempt.IsDisqualified = attempt.ResultRaw.ToLower().Contains("disq");
            // alles andere sind Weiten/Höhen m
            // hier fehlt noch verzichtet, ungültig, disq
            if (decimal.TryParse(attempt.ResultRaw, out decimal res))
            {
                attempt.Result = res;

            }

            if (Decimal.TryParse(intermediate.Wind, out decimal wind))
            {
                attempt.Wind = wind;
            }
            if (Decimal.TryParse(intermediate.Behind, out decimal behind))
            {
                attempt.Behind = behind;
            }
            if (intermediate.Flag != null)
            {
                attempt.IsBest = intermediate.Flag == "1";
            }

            return attempt;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="athlete"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public static Attempt FromCompetitor(Omega.Athlete athlete,
            Type type, string windRaw)
        {
            Attempt attempt = new Attempt();
            attempt.AthleteId = athlete.Id;
            attempt.Type = type;
            attempt.Number = 1; // für läufe
            attempt.IsBest = true;

            if (type == Type.Height || type == Type.Width)
            {
                throw new InvalidOperationException("Events of type 'Width' or 'Height' should copy best attempt from all attempts.");
            }

            attempt.ResultRaw = athlete.Result;
            
            var attempts = new List<Attempt>();
            attempt.IsInvalid = attempt.ResultRaw.ToLower() == "x";
            attempt.IsPassed = attempt.ResultRaw.ToLower() == "-";
            attempt.IsDisqualified = attempt.ResultRaw.ToLower().Contains("disq");
            // alles andere sind Weiten/Höhen m
            // hier fehlt noch verzichtet, ungültig, disq
            if (decimal.TryParse(attempt.ResultRaw, out decimal res))
            {
                attempt.Result = res;

            }

            if (Decimal.TryParse(windRaw, out decimal wind))
            {
                attempt.Wind = wind;
            }
            if (Decimal.TryParse(athlete.Behind, out decimal behind))
            {
                attempt.Behind = behind;
            }
            
            return attempt;
        }

        
        
        public static Attempt FromIntermediate(AthonPerformance intermediate, string athletId,
            Type type)
        {
            Attempt attempt = new Attempt();
            attempt.AthleteId = athletId;
            attempt.Type = type;
            attempt.Number = intermediate.Attempt;

            attempt.ResultRaw = intermediate.FormattedPerformance;
            //// Zeiten können nicht direkt zu decimal geparst werden
            //if (attempt.Type == Type.Run || attempt.Type == Type.Relay)
            //{
            //    throw new InvalidOperationException("Events of type 'Run' or 'Relay' don't have attempts.");
            //}

            var attempts = new List<Attempt>();
            attempt.IsCanceled = attempt.ResultRaw.ToLower() == "can";
            attempt.IsInvalid = attempt.ResultRaw.ToLower() == "x";
            attempt.IsPassed = attempt.ResultRaw.ToLower() == "-";
            attempt.IsDisqualified = attempt.ResultRaw.ToLower().Contains("dis");
            // alles andere sind Weiten/Höhen m
            // hier fehlt noch verzichtet, ungültig, disq
            if (decimal.TryParse(attempt.ResultRaw, out decimal res))
            {
                attempt.Result = res;

            }

            if (intermediate.Wind.HasValue)
            {
                attempt.Wind = intermediate.Wind.Value;
            }
            // noch nicht im json gesehen
            //if (Decimal.TryParse(intermediate.Behind, out decimal behind))
            //{
            //    attempt.Behind = behind;
            //}
            if (intermediate.IsBest.HasValue)
            {
                attempt.IsBest = intermediate.IsBest.Value;
            }

            return attempt;
        }

        
    }
}
