using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrackAndFieldResults.Common
{
    /// <summary>
    /// Common API 
    /// </summary>
    public class Api
    {
        public static readonly List<ResultProvider> Provider = new List<ResultProvider>(
            [new ResultProvider() {
                Id = ProviderId.Omega, Name="Omega",
                Client= new CommonOmegaClient() }]
            );

        public static readonly Dictionary<ProviderId, IClient> Clients = Provider.
            OrderBy(p => p.Id).Select(p => new { p.Id, p.Client }).
            ToDictionary(p =>  p.Id, v => v.Client);
    }
}
