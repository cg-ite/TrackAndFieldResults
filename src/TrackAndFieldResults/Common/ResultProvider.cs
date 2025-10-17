namespace TrackAndFieldResults.Common
{
    /// <summary>
    /// Infos about a Provider for results
    /// </summary>
    public class ResultProvider
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public ProviderId Id { get; set; }

        public IClient Client { get; set; }
    }


    public enum ProviderId
    {
        Omega,
        Seltec,
        Worldathletics,
        Atos
    }

}