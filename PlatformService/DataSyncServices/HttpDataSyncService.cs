using System.Text;
using System.Text.Json;
using PlatformService.Dtos;

namespace PlatformService.DataSyncServices
{
    public class HttpDataSyncService : IDataSyncService
    {
        private readonly HttpClient _client;
        private IConfiguration _config;

        public HttpDataSyncService(HttpClient client, IConfiguration config)
        {
            _client = client;
            _config = config;
        }

        public async Task SendPlatform(PlatformReadDto platform)
        {
            var content = new StringContent(
                JsonSerializer.Serialize(platform),
                Encoding.UTF8,
                "application/json"
            );

            var commandServiceBaseUrl = _config["CommandServiceBaseUrl"];
            try
            {
                var response = await _client.PostAsync(
                    $"{commandServiceBaseUrl}/api/commands/platforms",
                    content);

                if (response.IsSuccessStatusCode)
                    Console.WriteLine($"{typeof(HttpDataSyncService)}: POST to {commandServiceBaseUrl} was successful");
                else
                    Console.WriteLine($"{typeof(HttpDataSyncService)}: POST to {commandServiceBaseUrl} was NOT successful");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}