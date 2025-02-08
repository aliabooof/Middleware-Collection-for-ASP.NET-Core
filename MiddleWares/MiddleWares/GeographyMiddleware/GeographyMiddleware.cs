using Microsoft.Extensions.Options;
using MiddleWares.Configureation;
using MiddleWares.Services.GeoServices;

namespace MiddleWares.MiddleWares.GeographyMiddleware
{
    public class GeographyMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GeographyMiddleware> _logger;
        private readonly IOptionsMonitor<GeoSettings> _geoSettings;

        public GeographyMiddleware(RequestDelegate next, ILogger<GeographyMiddleware> logger, IOptionsMonitor<GeoSettings> geoSettings)
        {
            _next = next;
            _logger = logger;
            _geoSettings = geoSettings;
        }

        public async Task Invoke(HttpContext context, IGeoLocationService geoLocationService)
        {
            var clientIp = context.Connection.RemoteIpAddress?.ToString();

            /* // for testing 
             var clientIp = context.Request.Headers["X-Forwarded-For"].FirstOrDefault()
                     ?? context.Connection.RemoteIpAddress?.ToString();*/


            if (string.IsNullOrEmpty(clientIp))
            {
                _logger.LogWarning("Client IP could not be determined. Access denied.");
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                await context.Response.WriteAsync("Access denied: Unable to determine your location.");
                return;
            }

            var country = await geoLocationService.GetCountryByIpAsync(clientIp);

            if (country == "Unknown")
            {
                _logger.LogWarning("Country could not be determined from IP.");
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                await context.Response.WriteAsync("Access denied: Unable to determine your location.");
                return;
            }

            var blockedCountries = _geoSettings.CurrentValue.BlockedCountries;

            if (blockedCountries.Contains(country, StringComparer.OrdinalIgnoreCase))
            {
                _logger.LogWarning($"Access denied for country:{country}");
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                await context.Response.WriteAsync($"Access denied: Your country ({country}) is blocked.");
                return;
            }

            _logger.LogInformation($"Client IP: {clientIp}, Country: {country}");
            await _next(context);

        }


    }
}
