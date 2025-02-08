namespace MiddleWares.Services
{
    public interface IGeoLocationService
    {
        Task<string> GetCountryByIpAsync(string ipAddress);
    }
}
