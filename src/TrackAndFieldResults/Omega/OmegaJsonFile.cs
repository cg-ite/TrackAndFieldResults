/*
 * SPDX - FileCopyrightText: Copyright © 2022 Christian Günther <cg-ite@gmx.de>
 * SPDX - License - Identifier: GPL - 3.0 - or - later
 */

using System.Text.Json;

namespace TrackAndFieldResults.Omega
{
    /// <summary>
    /// Parst die verschiedenen json Dateien des Omega
    /// Ergebnis-Dienst
    /// </summary>
    public class OmegaJsonFile
    {
        /// <summary>
        /// Returns the details of the Event
        /// </summary>
        /// <returns></returns>
        public EventRoot GetEventRoot(string path)
        {
            using (StreamReader r = new StreamReader(path))
            {
                string json = r.ReadToEnd();
                return JsonSerializer.Deserialize<EventRoot>(json);
            }
        }
        /// <summary>
        /// GIbt die Details einer Disziplin zurück
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public EventDetails GetEventDetails(string path)
        {

            using (StreamReader r = new StreamReader(path))
            {
                string json = r.ReadToEnd();
                return JsonSerializer.Deserialize<EventDetails>(json);
            }
        }
        /// <summary>
        /// Gibt die Details eines Wettkampfes zurück.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public CompetitionRoot GetCompetitionDetails(string path)
        {
            using (StreamReader r = new StreamReader(path))
            {
                string json = r.ReadToEnd();
                return JsonSerializer.Deserialize<CompetitionRoot>(json);
            }
        }
        /// <summary>
        /// Gibt alle Ids der gefundenen Wettkämpfe zurück
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public CompetitionsRoot GetCompetitions(string path)
        {
            using (StreamReader r = new StreamReader(path))
            {
                string json = r.ReadToEnd();
                return JsonSerializer.Deserialize<CompetitionsRoot>(json);
            }
        }
        /// <summary>
        /// Gibt den Zeitplan eines Wettkampfes zurück
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public ScheduleRoot GetSchedule(string path)
        {

            using (StreamReader r = new StreamReader(path))
            {
                string json = r.ReadToEnd();
                return JsonSerializer.Deserialize<ScheduleRoot>(json);
            }
        }
    }

    /// <summary>
    /// Some Extentions 
    /// </summary>
    public static class Extentions
    {
        /// <summary>
        /// Fast Sortierschlüssel der Startposition für die 
        /// verschiedenen Versuchen
        /// und den entsprechenden Athleten zusammen, so dass
        /// die Athleten für jeden Abschnitt der Disziplin
        /// neu sortiert werden können
        /// </summary>
        public struct AthletePosition
        {
            public double[] Keys { get; set; }

            public Athlete Athlete { get; set; }
        }

        /// <summary>
        /// Erzeugt einen Sortiertschlüssel für einen Athleten, nach dem
        /// die Athleten sich für die verschiedenen Phasen des Wettkampfes
        /// sortieren lassen. Es werden dabei die aktuell gültigen 
        /// IAAF Regeln beachtet:
        /// - Je höher die Leistung, desto später startet der Athlet
        /// - Bei gleichen Leistungen, zählt der zweit weiteste Versuch
        /// - Bei gleichen Zweitversuch, gilt je früher der Versuch, desto später startet der Athlet
        /// - Bei gleicher Position des Zweitversuchs, gilt die Startposition.
        /// </summary>
        /// <param name="result"></param>
        /// <param name="listindex"></param>
        /// <param name="splitCount"></param>
        /// <param name="startpos"></param>
        /// <returns></returns>
        public static double ToSortkey(this string result, int listindex, int splitCount, int startpos)
        {
            double max;
            if (double.TryParse(result, out max))
            {
                double secondSort = (double)(splitCount - listindex) / 1000;
                return max + secondSort + (double)startpos / 10000;
            }
            return double.NaN;
        }

        public static Attempt[] GetHjStartlist(
            this EventRoot @event)
        {
            var athletes = @event.Content.Full.CompetitorDetails;
            var allAthletes = @event.Content.Full.Startlist
                .OrderBy(s => s.Value.ListIndex)
                .Select(s => athletes.ContainsKey(s.Key) ? athletes[s.Key] : null);

            // Versuchsnr nach der die Reihenfolge angepasst wird: [3,5]
            var separators = @event.Content.Full.AttemptSeparators;
            var splitCount = @event.Content.Full.SplitCount;

            // Startliste zum ersten Versuch
            var startlist = @event.Content.Full.Startlist;
            var temp = new List<AthletePosition>();

            foreach (var ath in allAthletes)
            {
                double athMax = 0.0;
                var athPos = new AthletePosition();
                athPos.Athlete = ath;
                athPos.Keys = (double[])Array.CreateInstance(typeof(double), separators.Length + 1);
                // Startposition zum ersten Start
                athPos.Keys[0] = startlist[ath.Id].ListIndex;

                var sep = separators[0];
                var iKey = 1;
                double maxKey = 0.0;
                // i: 1-3, 4-5
                for (int i = 0; i < splitCount; i++)
                {
                    // keinen Endkampf-Ergebnisse
                    if (ath.Intermediate[i].Result == null) { continue; }

                    double key = ath.Intermediate[i].Result
                        .ToSortkey(i, splitCount, (int)athPos.Keys[0]);

                    if (i == separators.Last()) { break; }
                    if (i == sep)
                    {
                        // nächster separator
                        sep = separators[iKey];
                        iKey++;
                        athPos.Keys[iKey] = athPos.Keys[iKey - 1];
                        // Key vorbelegen, falls keine gültigen Versuche
                        // mehr -> Key bleibt gleich
                    }
                    athPos.Keys[iKey] = key.CompareTo(athPos.Keys[iKey]) > 0 ? key : athPos.Keys[iKey];
                }

                temp.Add(athPos);
            }
            // [0,3,5,6]
            var skip = separators.Prepend(0).Append(splitCount).ToArray();
            var allAttempts = new List<Attempt>();
            for (int i = 0; i < skip.Count() - 1; i++)
            {
                allAttempts.AddRange(
                    temp.SelectMany(ap => ap.Athlete
                        .Intermediate.Take(skip[i + 1]).Skip(skip[i]),
                        (apos, inter) => new
                        {
                            apos.Athlete,
                            Intermediate = inter,
                            Sortkey = apos.Keys[i]
                        })
                    .Where(se => se.Intermediate.Result != null)    // ausgeschiedene Teilnehmer aussortieren
                    .OrderBy(se => se.Intermediate.Number)
                    .ThenBy(se => se.Sortkey)
                    .Select(ano => new Attempt
                    {
                        Athlete = ano.Athlete,
                        Intermediate = ano.Intermediate,
                        IntermediateNr = ano.Intermediate.Number
                    }));
            }

            return allAttempts.ToArray();
            // für jeden Athleten müssen drei/n Keys bestimmt werden
            // dann wird eine schleife über alle keys ausgeführt
            // und jeweils die anzahl an Versuchen extrahiert

            // the list gets ordered seperators.count times
            // in technical discplins like long jump, triple jump
            // and all throws. The order is based on the best
            // performance

            // 1. count == athletes.count: nach Leistung sortieren
            // count != athletes.count: dann Vergleich mit count groupBy leistungVnr
            // gleiche count?
            // nein: 2. sortiere Gruppe nach Leistung absteigend -> früher gesprungen besser
            // count unterschiedlich
            // ja: 3. sortiere Gruppe leistungVnr nach LeistungVnr absteigend und dann
            // ggf. weiter recursiv. Leistungen in stack und gleiches element 
            // löschen, dann wieder GroupBy leistungVnr
            // absteigend wenn gleiche Leistung im gleichen versuch
            // aufsteigend wenn nach Leistung
        }

        public static AthletePosition[] GetStartPosition(
            this EventRoot @event)
        {
            var athletes = @event.Content.Full.CompetitorDetails;
            var allAthletes = @event.Content.Full.Startlist
                .OrderBy(s => s.Value.ListIndex)
                .Select(s => athletes.ContainsKey(s.Key) ? athletes[s.Key] : null);

            // Versuchsnr nach der die Reihenfolge angepasst wird: [3,5]
            var separators = @event.Content.Full.AttemptSeparators;
            var splitCount = @event.Content.Full.SplitCount;

            // Startliste zum ersten Versuch
            var startlist = @event.Content.Full.Startlist;

            var temp = new List<AthletePosition>();

            foreach (var ath in allAthletes)
            {
                var athPos = new AthletePosition();
                athPos.Athlete = ath;
                athPos.Keys = (double[])Array.CreateInstance(typeof(double), separators.Length + 1);
                // Startposition zum ersten Start
                athPos.Keys[0] = startlist[ath.Id].ListIndex;

                var sep = separators[0];
                var iKey = 1;
                // i: 1-3, 4-5
                for (int i = 0; i < splitCount; i++)
                {
                    // keinen Endkampf-Ergebnisse
                    if (ath.Intermediate[i].Result == null) { continue; }

                    double key = ath.Intermediate[i].Result
                        .ToSortkey(i, splitCount, (int)athPos.Keys[0]);

                    if (i == separators.Last()) { break; }
                    if (i == sep)
                    {
                        // nächster separator
                        sep = separators[iKey];
                        iKey++;
                        athPos.Keys[iKey] = athPos.Keys[iKey - 1];
                        // Key vorbelegen, falls keine gültigen Versuche
                        // mehr -> Key bleibt gleich
                    }
                    athPos.Keys[iKey] = key.CompareTo(athPos.Keys[iKey]) > 0 ? key : athPos.Keys[iKey];
                }

                temp.Add(athPos);
            }
            return temp.ToArray();
        }

        public static Attempt[] GetVjStartlist(this EventRoot @event)
        {
            var athletes = @event.Content.Full.CompetitorDetails;
            var allAthletes = @event.Content.Full.Startlist
                .OrderBy(s => s.Value.ListIndex)
                .Select(s => athletes.ContainsKey(s.Key) ? athletes[s.Key] : null)
                .Where(a => a.StartPos != null);
            // Versuchsnr nach der die Reihenfolge angepasst wird: [3,5]
            var heights = @event.Content.Full.Splits;
            var splitCount = @event.Content.Full.SplitCount;

            var attemptsTemp = allAthletes.SelectMany(
                a => a.Intermediate,
                (ath, inter) => new Attempt
                {
                    Athlete = ath,
                    Intermediate = inter
                })
                .Where(at => at.Intermediate.Result != "-" && at.Intermediate.Result != null)
                .OrderBy(at => at.Intermediate.Number)
                .ThenBy(at => at.Athlete.StartPos);

            var allAttempts = attemptsTemp.SelectMany(
                at => at.Intermediate.Result.ToCharArray()
                    .Select((c, i) => new { c, i }),
                (e, a) => new Attempt
                {
                    Athlete = e.Athlete,
                    Intermediate = e.Intermediate,
                    IntermediateNr = a.i + 1,
                    IntermediateResult = a.c.ToString(),
                    IntermediateHeight = heights[e.Intermediate.Number - 1].Name
                })
                .OrderBy(a => a.IntermediateHeight)
                .ThenBy(a => a.IntermediateNr)
                .ThenBy(a => a.Athlete.StartPos)
                ;

            return allAttempts.ToArray();
        }

    }
}
