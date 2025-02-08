using Microsoft.Extensions.Options;
using MiddleWares.Configureation;
using System.Text.Json;

namespace MiddleWares.Services.GeoServices
{
    public class GeoLocationService : IGeoLocationService
    {
        private readonly HttpClient _httpClient;
        private readonly GeoSettings _geoSettings;
        private readonly ILogger<GeoLocationService> _logger;

        public GeoLocationService(HttpClient httpClient, IOptions<GeoSettings> geoSettings, ILogger<GeoLocationService> logger)
        {
            _httpClient = httpClient;
            _geoSettings = geoSettings.Value;
            _logger = logger;
        }

        public async Task<string> GetCountryByIpAsync(string ipAddress)
        {
            try
            {
                var apiKey = _geoSettings.GeoApi.ApiKey;
                var baseUrl = _geoSettings.GeoApi.BaseUrl;

                var response = await _httpClient.GetAsync($"{baseUrl}{ipAddress}?access_key={apiKey}");
                response.EnsureSuccessStatusCode();
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var geoData = JsonSerializer.Deserialize<GeoApiResponse>(jsonResponse);
                return geoData?.country_name ?? "Unknown";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching geolocation data for IP: {IpAddress}", ipAddress);
                return "Unknown";
            }
        }



        private class GeoApiResponse
        {
            public string country_name { get; set; }
        }
    }

}
