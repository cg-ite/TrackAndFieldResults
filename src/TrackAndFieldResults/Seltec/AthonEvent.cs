/*
 * SPDX - FileCopyrightText: Copyright © 2021 Olympiastützpunkt Hessen <cguenther@lsbh.de>
 * SPDX - License - Identifier: GPL - 3.0 - or - later
 */

using System.Text.Json.Serialization;

namespace TrackAndFieldResults.Seltec
{
	/// <summary>
	/// Custom extentions of the AthonEvent
	/// </summary>
	public partial class AthonEvent : IEquatable<AthonEvent>, ICloneable
	{
		/// <summary>
		/// Formated string of all agegroups of the Event
		/// </summary>
		[JsonIgnore]
		public string FormatedAgegroups
		{
			get
			{
				if (Agegroups.Count > 0)
				{
					return string.Join(", ", Agegroups.Select(a => a.Shortcode));
				}
				return string.Empty;
			}
		}

		/// <summary>
		/// Umfassende Darstellung für toString()
		/// </summary>
		[JsonIgnore]
		public string Label
		{
			get
			{
				int cHeats = GetHeats().Count() - 1;
				int cSquads = GetSquads().Count() - 1;
				string sHeat = cHeats > 0 ? $" [L: {cHeats}] " : "";
				string sSquad = cSquads > 0 ? $"[R: {cSquads}]" : "";
				return $"{Longname} - {FormatedAgegroups}{sHeat}{sSquad}";
			}
		}

		public object Clone()
		{
			var ev = (AthonEvent)MemberwiseClone();
			return ev;
		}

		public bool Equals(AthonEvent other)
		{
			return Object.Equals(other.Label, Label);
		}

		public override int GetHashCode()
		{
			return Label.GetHashCode();
		}

		public override string ToString()
		{
			return Label;
		}

		/// <summary>
		/// Gibt die Riegen eines einzelnen Wettkampfes zurück
		/// </summary>
		/// <param name="event"></param>
		/// <returns></returns>
		public IEnumerable<string> GetSquads()
		{
			return Entries.GroupBy(ev => ev.Squad).Select(g => g.Key);
		}

		/// <summary>
		/// Gibt die Läufe eines einzelnen Wettkampfes zurück
		/// </summary>
		/// <param name="event"></param>
		/// <returns></returns>
		public IEnumerable<string> GetHeats()
		{
			return Entries.GroupBy(ev => ev.Heat).Select(g => g.Key);
		}
	}
}
