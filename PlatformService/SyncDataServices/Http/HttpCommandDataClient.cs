using PlatformService.Contract;
using System.Text;
using System.Text.Json;

namespace PlatformService.SyncDataServices.Http;

public class HttpCommandDataClient : ICommandDataClient
{
    private readonly HttpClient httpClient;
    private readonly IConfiguration configuration;

    public HttpCommandDataClient(HttpClient httpClient, IConfiguration configuration)
    {
        this.httpClient = httpClient;
        this.configuration = configuration;
    }

    public async Task SendPlatformToCommand(PlatformResponse platformResponse)
    {
        var httpContent = new StringContent(
            JsonSerializer.Serialize(platformResponse), 
            Encoding.UTF8,
            "application/json");

        var response = await httpClient.PostAsync($"{configuration["CommandService"]}", httpContent);
        if (response.IsSuccessStatusCode) await Console.Out.WriteLineAsync("--> Sync POST to CommandService was OK");
        else await Console.Out.WriteLineAsync("--> Sync POST to CommandService was not OK ");
    }
}
