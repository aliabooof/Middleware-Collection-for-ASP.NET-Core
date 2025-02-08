namespace MiddleWares.Configureation
{
    public class GeoSettings
    {
        public List<string> BlockedCountries { get; set; } = new List<string>();
        public GeoApiConfig GeoApi { get; set; }
    }

    public class GeoApiConfig
    {
        public string ApiKey { get; set; }
        public string BaseUrl { get; set; }
    }
}
