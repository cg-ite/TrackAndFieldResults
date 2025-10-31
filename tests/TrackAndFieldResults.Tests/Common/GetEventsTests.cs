using Shouldly;
using System.Threading.Tasks;
using TrackAndFieldResults.Common;
using TrackAndFieldResults.Omega;
using TrackAndFieldResults.Tests.Omega;

namespace TrackAndFieldResults.Tests;

/// <summary>
/// Test for a Common Interface Api
/// </summary>
public class GetEventsTests
{
    private OmegaClient _clientOmega;
    private const string downloadPath = "C:\\Temp\\Omega";
    private const string CompetitionKey = "DMDresden_2025";

    [OneTimeSetUp]
    public void Setup()
    {
        HttpClient httpClient = new();
        httpClient.DefaultRequestHeaders.Add("Accept", "text/json");
        var version = new Random(2783763).Next(90, 135);
        httpClient.DefaultRequestHeaders.Add("User-Agent",
            $"Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:122.0) Gecko/20100101 Firefox/{version}.0");

        _clientOmega = new OmegaClient(httpClient);
        _clientOmega.BaseUrl = JsonClientTests.baseUrl;
        _clientOmega.ReadResponseAsString = true;    //for saving response to file
        Directory.CreateDirectory(downloadPath);
    }

    /// <summary>
    /// Typical GUI use case: List all Provider, select one and
    /// get results via client
    /// </summary>
    [Test]
    public async Task Can_List_Provider_And_Clients()
    {
        Api.Provider.Where(p => p.Name == "Omega").Count().ShouldBe(1);
        var omegaProvider = Api.Provider.Where(p => p.Name == "Omega").First();
        omegaProvider.Id.ShouldBe(ProviderId.Omega);

        Api.Clients[omegaProvider.Id].ShouldNotBeNull();
        var c = Api.Clients[omegaProvider.Id];
        c.ShouldNotBeNull();
    }
    
    [Test]
    public async Task Can_List_Omega_Competitions()
    {
        Api.Provider.Where(p => p.Name == "Omega").Count().ShouldBe(1);
        var omegaProvider = Api.Provider.Where(p => p.Name == "Omega").First();
        omegaProvider.Id.ShouldBe(ProviderId.Omega);

        Api.Clients[omegaProvider.Id].ShouldNotBeNull();
        var c = Api.Clients[omegaProvider.Id];
        var events = await c.GetCompetitionsAsync();
    }



    [Test]
    public async Task Can_Get_Omega_CompetitionDetails()
    {
        Api.Clients[ProviderId.Omega].ShouldNotBeNull();
        var c = Api.Clients[ProviderId.Omega];
        //var events = await c.GetCompetitionsAsync();
        var details = await c.GetCompetitionDetailsAsync("DMDresden_2025");
        details.ShouldNotBeNull();
        details.Schedule.Count().ShouldBeGreaterThan(0);
        
    }
    
    [Test]
    public async Task Can_Get_Omega_Event()
    {
        Api.Clients[ProviderId.Omega].ShouldNotBeNull();
        var c = Api.Clients[ProviderId.Omega];
        //var events = await c.GetCompetitionsAsync();
        var details = await c.GetCompetitionDetailsAsync(CompetitionKey);
        details.ShouldNotBeNull();
        details.Schedule.Count().ShouldBeGreaterThan(0);
        
        var eId = details.Schedule.First().ProviderId;
        eId.ShouldNotBeNull();

        var evnt = await c.GetEventDetailsAsync(CompetitionKey, eId);
    }

    [Test]
    public async Task Can_Get_Omega_Attempts()
    {
        Api.Clients[ProviderId.Omega].ShouldNotBeNull();
        var c = Api.Clients[ProviderId.Omega];
        //var events = await c.GetCompetitionsAsync();
        var details = await c.GetCompetitionDetailsAsync(CompetitionKey);
        details.ShouldNotBeNull();
        details.Schedule.Count().ShouldBeGreaterThan(0);

        var eId = details.Schedule.Where(e => e.Name.Contains("Hochs")).First().ProviderId;
        eId.ShouldNotBeNull();

        var evt = await c.GetEventDetailsAsync(CompetitionKey, eId);
        evt.Attemps.ShouldNotBeNull();
        evt.Attemps.Count().ShouldBeGreaterThan(0);
        evt.Attemps[0].ShouldNotBeNull();
        // Result kann auch null sein, wenn der Athlet einen Fehlversuch
        // hatte oder er verzichtet hat
        evt.Attemps[0].Result.HasValue.ShouldBeFalse();
        evt.Attemps[0].ResultRaw.ShouldNotBeEmpty();
        evt.Attemps[0].FormattedResult().ShouldNotBeEmpty();
        evt.Attemps[0].Height.Value.ShouldBeGreaterThan(1);
    
        eId = details.Schedule.Where(e => e.Name.Contains("Weits")).First().ProviderId;
        eId.ShouldNotBeNull();

        evt = await c.GetEventDetailsAsync(CompetitionKey, eId);
        evt.Attemps.ShouldNotBeNull();
        evt.Attemps.Count().ShouldBeGreaterThan(0);
        evt.Attemps[0].ShouldNotBeNull();
        // Result kann auch null sein, wenn der Athlet einen Fehlversuch
        // hatte oder er verzichtet hat
        evt.Attemps[0].Result.HasValue.ShouldBeTrue();
        evt.Attemps[0].ResultRaw.ShouldNotBeEmpty();
        evt.Attemps[0].FormattedResult().ShouldNotBeEmpty();
        evt.Attemps[0].Result.Value.ShouldBeGreaterThan(1);
    }

    /// <summary>
    /// Gibt alle Startlisten zurück. Bei technischen Disziplinen
    /// erfolgen nach 3 und ggf.5 neue Startlisten, da Athleten
    /// nue nach ihren Leistungen sortiert werden.
    /// Bei Läufen werden die Versuche nach Lauf und Bahn sortíert
    /// zurückgegeben. Es gibt nur eine Startliste. Oder lieber pro
    /// Lauf eine Startliste.
    /// Bei der WM 2025 wurde nach dem 3., dem 4. und dem 5. neu
    /// sortiert.
    /// </summary>
    /// <returns></returns>
    [Test]
    public async Task Can_Get_Startlists()
    {
        Api.Clients[ProviderId.Omega].ShouldNotBeNull();
        var c = Api.Clients[ProviderId.Omega];
        //var events = await c.GetCompetitionsAsync();
        var details = await c.GetCompetitionDetailsAsync(CompetitionKey);
        details.ShouldNotBeNull();
        details.Schedule.Count().ShouldBeGreaterThan(0);
        
        var eId = details.Schedule.Where(e => e.Name.Contains("Weitsp")).First().ProviderId;
        eId.ShouldNotBeNull();

        var evt = await c.GetEventDetailsAsync(CompetitionKey, eId);
        var startlists = evt.Startorders;
        // besser so, da die listen syncron erstellt werden
        // können ohne das event nochmal zu laden
        // sonst würde es aussehen, als müsste man 
        // immer das event runterladen
        //var ranking = evt.GetRanking();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    [Test]
    public async Task Can_Get_Attemptlist()
    {
        Api.Clients[ProviderId.Omega].ShouldNotBeNull();
        var c = Api.Clients[ProviderId.Omega];
        //var events = await c.GetCompetitionsAsync();
        var details = await c.GetCompetitionDetailsAsync(CompetitionKey);
        details.ShouldNotBeNull();
        details.Schedule.Count().ShouldBeGreaterThan(0);
        
        var eId = details.Schedule.First().ProviderId;
        eId.ShouldNotBeNull();

        //var evnt = c.GetEventAttemptlistAsync(CompetitionKey, eId);
    }
    [Test]
    public async Task Can_Get_Ranking()
    {
        Api.Clients[ProviderId.Omega].ShouldNotBeNull();
        var c = Api.Clients[ProviderId.Omega];
        //var events = await c.GetCompetitionsAsync();
        var details = await c.GetCompetitionDetailsAsync(CompetitionKey);
        details.ShouldNotBeNull();
        details.Schedule.Count().ShouldBeGreaterThan(0);
        
        var eId = details.Schedule.First().ProviderId;
        eId.ShouldNotBeNull();

        //var evnt = c.GetEventRankingAsync(CompetitionKey, eId);
    }

    [Test]
    public void Can_Convert_Time_To_Decimal()
    {

        if (TimeSpan.TryParse("1:24,23".Replace(",", "."), out TimeSpan ts))
        {
            ts.TotalSeconds.ShouldBe(84.23);
        }
        if (decimal.TryParse("1:24,23".Replace(",", "."), out decimal result))
        {
            result.ShouldBe((decimal)84.23);
        }
        Assert.Fail("Zeit zu decimal funktioniert so nicht");
    }
}
