using TrackAndFieldResults.Seltec;

namespace TrackAndFieldResults.Common
{
    public class CommonSeltecClient : IClient
    {
        private SeltecAthonClient _client;
        private Dictionary<string, AthonCompetition> _competitions = new Dictionary<string, AthonCompetition>();

        public CommonSeltecClient(HttpClient httpClient)
        {
            _client = new SeltecAthonClient(httpClient);
            _client.ReadResponseAsString = true;

            BaseUrl = "https://ergebnisse.leichtathletik.de";
        }

        public string BaseUrl { get => _client.BaseUrl; set => _client.BaseUrl = value; }

        public async Task<Competition> GetCompetitionDetailsAsync(string competitionKey, CancellationToken cancellationToken)
        {
            //caching
            AthonCompetition _competition;
            string respose = "";
            if (_competitions.ContainsKey(competitionKey))
            {
                _competition = _competitions[competitionKey];
            }
            else
            {
                var details = await _client.GetLegacyCompetitionByIdAsync(competitionKey);
                _competition = details.Competitions.First();
                _competitions.Add(competitionKey, _competition);
#if DEBUG
                respose = _client.ResponseText;
                _client.SaveResponseText($"./{competitionKey}.json");
#endif
            }
            var res = new Competition()
            {
                Name = _competition.Name,
                Town = _competition.Town,
                ProviderId = _competition.Id,
                StartDate = _competition.Start.Date,
                EndDate = _competition.End.Date,
                Nation = _competition.Nation,
                ResultProviderId = ProviderId.Seltec,
                ResponseText = respose,
            };
            //TODO: Auf End date prüfen
            res.Schedule = _competition.Events.SelectMany(e => ScheduleItem.FromEventDetails(e, _competition.Start.Date)).ToArray();
            return res;
        }

        public Task<Competition> GetCompetitionDetailsAsync(string competitionKey)
        {
            return GetCompetitionDetailsAsync(competitionKey, System.Threading.CancellationToken.None);
        }

        public async Task<Competition[]> GetCompetitionsAsync(int year, CancellationToken cancellationToken)
        {
            if (year < 2010)
            {
                throw new ArgumentOutOfRangeException(nameof(year), $"Das Jahr {year} ist zu klein, bitte übergeben Sie ein größeres Jahr.");
            }

            var comps = await _client.GetCompetitionsAsync(year);

            return comps.Select((c, i) => new Competition()
            {
                ProviderId = c.ID,
                ResultProviderId = ProviderId.Seltec,
                Name = c.Name,
                StartDate = c.StartDate,
                Id = i
            }).ToArray();
        }

        public Task<Competition[]> GetCompetitionsAsync(int year)
        {
            return GetCompetitionsAsync(year, System.Threading.CancellationToken.None);
        }

        public async Task<Event> GetEventDetailsAsync(string competitionKey, string eventKey, CancellationToken cancellationToken)
        {
            if (_competitions.ContainsKey(competitionKey) == false)
            {
                await GetCompetitionDetailsAsync(competitionKey);
            }
            var _competition = _competitions[competitionKey];
            var evtKey = EventKey.FromString(eventKey);

            IEnumerable<AthonEvent> actEvent = _competition.Events.Where(e => e.Id == evtKey.EventId);
            var evt = actEvent.First();
            if (evt == null) { throw new ArgumentOutOfRangeException(nameof(evt), $"Der Eventkey '{eventKey}' ist nicht vorhanden."); }

            var entries = evt.Entries.Where(e => e.RoundType == Enum.Parse<AthonRoundType>(evtKey.RoundId)
                && e.Heat == evtKey.HeatId);

            AthonEntry firstEntry = new();
            if (entries.Count() == 0)
            {
                // bei geplanten Wettkämpfen gibt es keine Entries
                firstEntry.RoundType = Enum.Parse<AthonRoundType>(evtKey.RoundId);
                firstEntry.Heat = evtKey.HeatId;
                firstEntry.HeatDateTime = _competition.Start.Date;
            }
            else
            {
                firstEntry = entries.First();
            }

            var res = Event.FromEventDetails(evt, firstEntry, _competition.Start.Date);
            res.Athletes = GetAthletesByIds(competitionKey, entries.Select(e => e.CompetitorId)).ToArray();

            res.Entries = entries.Select(e => Entry.FromEntry(e, res.Type)).ToArray();
            res.Results = res.Entries
                .Where(e => e.State != EntryState.None)
                .SelectMany(e => e.Attempts
                    .Where(a => a.Status != AttemptStatus.Unknown && 
                            a.IsBest.HasValue && a.IsBest.Value)).ToArray();
            
            if (res.Type == Type.Run)
            {
                // TODO?
            }
            else
            {
                /*var attempts = new List<Attempt>();
                foreach (var entry in entries)
                {
                    // bei geplanten events wird ein leeres Attempts-Objekt übergeben
                    // deswegen Prüfung auf Status != None, weil sonst
                    // das leere Objekt als Attempt geparst wird
                    if (entry.State == AthonEntryState.None) { continue; }
                    attempts.AddRange(entry.Attempts
                        .Select(a => Attempt
                            .FromIntermediate(a, entry.CompetitorId, res.Type)).ToArray());
                }*/
                res.Attempts = res.Entries
                    .Where(e => e.State != EntryState.None)
                    .SelectMany(e => e.Attempts
                        .Where(a => a.Status != AttemptStatus.Unknown)).ToArray();
                //res.Attempts = attempts.ToArray();
            }
            return res;

        }


        public Task<Event> GetEventDetailsAsync(string competitionKey, string eventKey)
        {
            return GetEventDetailsAsync(competitionKey, eventKey, System.Threading.CancellationToken.None);
        }


        private IEnumerable<Athlete> GetAthletesByIds(string competitionKey, IEnumerable<string> ids)
        {
            if (_competitions.ContainsKey(competitionKey) == false)
            {
                GetCompetitionDetailsAsync(competitionKey);
            }
            // alle Athleten
            var _competition = _competitions[competitionKey];
            var allAthletes = _competition.Clubs.SelectMany(c => c.Competitors,
                (c, a) => Athlete.FromAthlete(a, c));
            var athletIds = ids;
            return allAthletes.Join(athletIds, at => at.Id, id => id,
                (ath, id) => ath);
        }

    }
    /// <summary>
    /// Klasse für den künstlichen EventKey einer Phase 
    /// einer Disziplin, wie bei Omega.
    /// Format: EventId*RoundId*HeatId
    /// </summary>
    /// <remarks>Notwendig um einen einzelnen Lauf auszuwählen</remarks>
    public class EventKey
    {
        private const char Sep = '*';

        public static EventKey FromString(string id)
        {
            var parts = id.Split(Sep);
            if (parts.Length == 2)
            {
                return new EventKey
                {
                    EventId = parts[0],
                    RoundId = parts[1],
                };
                throw new ArgumentException($"Der übergebene Key{id} hat kein gültiges Format");
            }
            return new EventKey
            {
                EventId = parts[0],
                RoundId = parts[1],
                HeatId = parts[2],
            };
        }

        public static string ToEventKey(string eventId, string roundId, string heatId)
        {
            return $"{eventId}{Sep}{roundId}{Sep}{heatId}";
        }

        public string EventId { get; set; }
        public string RoundId { get; set; }
        public string HeatId { get; set; }
    }

}
