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
        }

        public string BaseUrl { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public async Task<Competition> GetCompetitionDetailsAsync(string competitionKey, CancellationToken cancellationToken)
        {
            var details = await _client.GetLegacyCompetitionByIdAsync(competitionKey);
            var _competition = details.Competitions.First();
            _competitions.Add(competitionKey, _competition);
            var res = new Competition()
            {
                Name = _competition.Name,
                Town = _competition.Town,
                ProviderId = _competition.Id,
                StartDate = _competition.Start.Date,
                EndDate = _competition.End.Date,
                Nation = _competition.Nation,
                ResultProviderId = ProviderId.Seltec,
            };
            res.Schedule = _competition.Events.SelectMany(e => ScheduleItem.FromEventDetails(e)).ToArray();
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
                        
            return comps.Select((c,i) => new Competition() {
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
            var parts = eventKey.Split('-');
            var _competition = _competitions[competitionKey];
            var evt = _competition.Events.Where(e=> e.Id == parts[0]).First();
            if (evt == null) { throw new ArgumentOutOfRangeException(nameof(evt), $"Der Eventkey '{eventKey}' ist nicht vorhanden."); }

            var entries = evt.Entries.Where(e => e.RoundType == Enum.Parse<AthonRoundType>(parts[1])
                && e.Heat == parts[2]);

            var res = Event.FromEventDetails(evt, entries.First());
            res.Athletes = GetAthletesByIds(competitionKey, entries.Select(e => e.CompetitorId)).ToArray();

            var results = new List<Attempt>();
            foreach (var athlete in entries)
            {
                results.AddRange(athlete.Attempts.Where(a => a.IsBest.HasValue && a.IsBest.Value)
                    .Select(a => Attempt
                        .FromIntermediate(a, athlete.Id, res.Type)).ToArray());
            }
            res.Results = results.ToArray();

            if (res.Type == Type.Run) 
            {
                
            }
            else
            {
                var attempts = new List<Attempt>();
                foreach (var athlete in entries)
                {
                    attempts.AddRange(athlete.Attempts
                        .Select(a => Attempt
                            .FromIntermediate(a, athlete.Id, res.Type)).ToArray());
                }
                res.Attempts = attempts.ToArray();
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
                (c,a) => Athlete.FromAthlete(a, c));
            var athletIds = ids;
            return  allAthletes.Join(athletIds, at => at.Id, id => id,
                (ath, id) => ath);
        }
    }
}
