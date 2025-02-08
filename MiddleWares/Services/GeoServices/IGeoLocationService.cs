namespace MiddleWares.Services.GeoServices
{
    public interface IGeoLocationService
    {
        Task<string> GetCountryByIpAsync(string ipAddress);
    }
}
