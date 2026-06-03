/*
 * SPDX - FileCopyrightText: Copyright © 2025 Christian Günther <cg-ite@gmx.de>
 * SPDX - License - Identifier: GPL - 3.0 - or - later
 */

using TrackAndFieldResults.Seltec;

namespace TrackAndFieldResults.Common
{
    /// <summary>
    /// Die Teilnahme eines Athleten einer Disziplin.
    /// Notwendig um den Fall abzubilden, dass mehrere
    /// AgeGroups in einem Wettkampf starten oder dass
    /// eine Athlet hochgemeldet wird.
    /// Hat auch Einfluss auf die Sortierung der Start- und
    /// Ergebnisliste.
    /// </summary>
    public partial class Entry : IEntry
    {
        public string Id { get ; set ; }
        public string CompetitorId { get ; set ; }
        public string AgegroupId { get ; set ; }
        public string Squad { get ; set ; }
        public Attempt[] Attempts { get ; set ; }= Array.Empty<Attempt>();
        public string Lane { get ; set ; }
        public string Heat { get ; set ; }
        public string Startposition { get ; set ; }
        public EntryState State { get ; set ; }

        public static Entry FromEntry(AthonEntry entry, Type type)
        {
            Entry ent = new Entry();
            ent.Id = entry.Id;
            ent.CompetitorId = entry.CompetitorId;
            ent.AgegroupId = entry.AgegroupId;
            ent.Squad = entry.Squad;
            if (entry.State == AthonEntryState.Finished)
            {
                ent.Attempts =
                    entry.Attempts.Select(a => Attempt.FromIntermediate(a, entry.CompetitorId, type)).ToArray();
            }
            ent.Lane = entry.Lane;
            ent.Heat = entry.Heat;
            if (entry.State.HasValue)
            {
                ent.State = (EntryState)entry.State.Value;
            }
            return ent;
        }

        public override string ToString()
        {
            return $"{CompetitorId} ({State}) L:{Lane} H: {Heat} V:{Attempts.Count()}";
        }
    }
}
