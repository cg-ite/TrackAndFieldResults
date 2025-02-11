/*
 * SPDX - FileCopyrightText: Copyright © 2022 Christian Günther <cg-ite@gmx.de>
 * SPDX - License - Identifier: GPL - 3.0 - or - later
 */

namespace TrackAndFieldResults.Omega
{
    /// <summary>
    /// A single attempt of a athlet. Basis for the 
    /// full attemptlist
    /// </summary>
    public class Attempt
    {
        /// <summary>
        /// Athlete
        /// </summary>
        public Omega.Athlete Athlete { get; set; }
        /// <summary>
        /// Attempt number. For vertical jumps, all 
        /// attempts of a height are counted onwards of 1
        /// </summary>
        public int IntermediateNr { get; set; }
        /// <summary>
        /// The result of the attempt in vertical jumps: -, o, or x.
        /// </summary>
        public string IntermediateResult { get; set; }
        /// <summary>
        /// Height in vertical jumps.
        /// </summary>
        public string IntermediateHeight { get; set; }

        public Omega.Intermediate Intermediate { get; set; }
    }
}
