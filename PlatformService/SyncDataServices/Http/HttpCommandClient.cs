using PlatformService.Dtos;
using Microsoft.Extensions.Configuration;
using System.Text.Json;
using System.Text;

namespace PlatformService.SyncDataServices.Http
{
    public class HttpCommandClient : ICommandClient
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;

        public HttpCommandClient(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _config = config;
        }
        public async Task SendPlatformToCommand(PlatformReadDto platform)
        {
            var httpContent = new StringContent(
                JsonSerializer.Serialize(platform),
                Encoding.UTF8,
                "application/json");
            var response = await _httpClient.PostAsync($"{_config["CommandService"]}", httpContent);
            
            if(response.IsSuccessStatusCode)
            {
                Console.WriteLine("--> Sync Post to CommandService was OK!");
            }
            else
            {
                Console.WriteLine("--> Sync POST to CommandService disrupted!");
            }
        }
    }
}