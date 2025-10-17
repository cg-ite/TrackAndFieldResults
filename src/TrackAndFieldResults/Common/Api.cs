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

        public static readonly List<IClient> Clients = Provider.
            OrderBy(p => p.Id).Select(p => p.Client).ToList();
    }
}
