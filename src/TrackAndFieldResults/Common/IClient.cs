/*
 * SPDX - FileCopyrightText: Copyright © 2025 Christian Günther <cg-ite@gmx.de>
 * SPDX - License - Identifier: GPL - 3.0 - or - later
 */

namespace TrackAndFieldResults.Common
{
    /// <summary>
    /// Common interface for result provider clients
    /// </summary>
    public interface IClient
    {
        public string BaseUrl { get; set; }

        public Task<Competition[]> GetCompetitionsAsync(CancellationToken cancellationToken);
        public Task<Competition[]> GetCompetitionsAsync();
        
        public Task<Competition> GetCompetitionDetailsAsync(string competitionKey, CancellationToken cancellationToken);
        public Task<Competition> GetCompetitionDetailsAsync(string competitionKey);

        public Task<Event> GetEventDetailsAsync(string competitionKey, string eventKey, CancellationToken cancellationToken);

        /// <summary>
        /// Gets the details of a event of a competition. You will find the start-, 
        /// competitor- and resultlists here.
        /// </summary>
        /// <param name="competitionKey">The competition-Id from the competitions list</param>
        /// <returns>the schedule of a competition</returns>
        /// <exception cref="ApiException">A server side error occurred.</exception>
        public Task<Event> GetEventDetailsAsync(string competitionKey, string eventKey);
        
    }
}