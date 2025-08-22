using TrackAndFieldResults.Seltec;

namespace TrackAndFieldResults.Tests;

/// <summary>
/// Tests for the SeltecClient.
/// You need an api-key for access to the result database.
/// The test api-key is '123', but is not working.
/// </summary>
public class SeltecClientTests
{
    private SeltecAthonClient _client ;
    private const string downloadPath = "C:\\Temp\\Seltec";
    
    [OneTimeSetUp]
    public void Setup()
    {
        //nachschauen wie das mit dem apikey funkt
        //    evt andere json clients auf httpclient
        //    umbauen, so dass alle gleich konfiguriert werden.

        //    tojson(), from json ist auch interessant als
        //    Alternative für save

        HttpClient httpClient = new();
        httpClient.DefaultRequestHeaders.Add("ApiKey", "123");
        _client = new SeltecAthonClient(httpClient);
    }

    [Test]
    public async Task Can_Download_Competitions()
    {
        var res = await _client.GetCompetitionsIndexListAsync(2024,true,false);
        _client.SaveResponseText(Path.Combine(downloadPath, "competitions.json"));
    }
    
    [Test]
    public async Task Can_Download_Competition_Details()
    {
        var res = await _client.GetLegacyCompetitionByIdAsync("7066");
        _client.SaveResponseText(Path.Combine(downloadPath, "competition7066.json"));
    }
    
    [Test]
    public async Task Can_Download_Schedule()
    {
        // see Can_Download_Competition_Details(); in this json everything is included
    }
    [Test]
    public async Task Can_Download_Event()
    {
        // see Can_Download_Competition_Details(); in this json everything is included
    }
}
