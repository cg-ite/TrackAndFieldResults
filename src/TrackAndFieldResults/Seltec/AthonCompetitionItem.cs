using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackAndFieldResults.Seltec
{
    /// <summary>
    /// DLV Leichtathletik Event. Für die Auswahl in
    /// der GUI gedacht
    /// </summary>
    public class AthonCompetitionItem : IEquatable<AthonCompetitionItem>
    {
        /// <summary>
        /// Offizieler Name des Wettkampfes 
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Datum des ersten Wettkampftages ohne Uhrzeit (00:00)
        /// </summary>
        public DateTime StartDate { get; set; }
        /// <summary>
        /// Seltec Storage
        /// </summary>
        public string Storage { get; set; }
        /// <summary>
        /// Seltec Id des Wettkampfes. Wird zur Abfrage der Ergebnisse
        /// benötigt.
        /// </summary>
        public string ID { get; set; }

        public bool Equals(AthonCompetitionItem other)
        {
            return Object.Equals(other.Name, Name);
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }
    }
}
