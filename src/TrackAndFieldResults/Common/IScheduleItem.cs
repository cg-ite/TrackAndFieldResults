using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackAndFieldResults.Common
{
    public interface IScheduleItem
    {
        /// <summary>
        /// All agegroups of the event
        /// </summary>
        ICollection<IAgegroup> Agegroups { get; set; }
        long CompetitionId { get; set; }
        DateTime? StartDate { get; set; }
        DateTime? EndDate { get; set; }
        string Longname { get; set; }
        /// <summary>
        /// Provider specific ID of this event
        /// </summary>
        string ProviderId { get; set; }
        /// <summary>
        /// All Track&Field event types
        /// </summary>
        Type Type { get; set; }
        /// <summary>
        /// Lauf Nr, Finale (bei Sprüngen oder Würfen) oder Lauf Nr/Gruppe bei
        /// Mehrkämpfen (Sprüngen oder Würfen)
        /// </summary>
        string Unit { get; set; }
        /// <summary>
        /// Phase des Wettkampf-Verlaufs der Disziplin (Vorunde, Halbfinale, Finale)
        /// oder Disziplin innerhalb des Mehrkampfes
        /// </summary>
        string Phase { get; set; }
        string Name { get; set; }

        string ToString();
    }

}
