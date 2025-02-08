namespace MiddleWares.Services.GeoServices
{
    public class MockGeoLocationService : IGeoLocationService
    {
        private readonly Dictionary<string, string> _mockIpToCountry = new()
        {
            { "123.123.123.123", "Russia" },
            { "111.111.111.111", "USA" },
            { "222.222.222.222", "Iran" }
        };

        public Task<string> GetCountryByIpAsync(string ipAddress)
        {

            _mockIpToCountry.TryGetValue(ipAddress, out var country);
            return Task.FromResult(country ?? "Unknown");
        }
    }

}
