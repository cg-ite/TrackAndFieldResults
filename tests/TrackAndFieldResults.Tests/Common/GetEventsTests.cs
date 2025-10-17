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
    public async Task Can_List_Provider()
    {
        Api.Provider.Where(p => p.Name == "Omega").Count().ShouldBe(1);
        var omegaProvider = Api.Provider.Where(p => p.Name == "Omega").First();
        omegaProvider.Id.ShouldBe(0);

        Api.Clients[omegaProvider.Id].ShouldNotBeNull();
        var c = Api.Clients[omegaProvider.Id];
        var events = await c.GetCompetitionsAsync();
    }

    [Test]
    public async Task GetEventsOmegaAsync()
    {
        // Omega specific API
        var comps = await _clientOmega.GetCompetitionsAsync();
        comps.ShouldNotBeNull();
        comps.Content.Full.Events.Count().ShouldBeGreaterThan(0);
        comps.Content.Full.Eventgroups.Count().ShouldBeGreaterThan(0);

        
        //var compsCommon = await _clientOmega.Omega.GetCompetitionsAsync();


    }
}
