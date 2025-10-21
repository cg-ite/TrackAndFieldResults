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
        
    }
}