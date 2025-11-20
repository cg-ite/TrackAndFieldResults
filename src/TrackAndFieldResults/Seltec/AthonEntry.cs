using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackAndFieldResults.Seltec
{
	public partial class AthonEntry
	{
		public AthonAgegroup Agegroup { get; set; }

		public override string ToString()
		{
			return $"#{Lane} Cid:{CompetitorId}";
		}

	}
}
