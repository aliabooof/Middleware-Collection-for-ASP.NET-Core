# GeoMiddleware README.md

# GeoMiddleware

`GeoMiddleware` blocks requests from specific countries based on their IP address using an external geolocation service.

---

## Features

- Blocks requests from a configurable list of countries.
- Logs warnings if the IP address cannot be determined.
- Supports both real and mock geolocation services for easy testing.
- Leverages dependency injection for flexibility and maintainability.

---

## Configuration

### **appsettings.json**

Add the following configuration to your `appsettings.json`:

```json
"GeoSettings": {
  "BlockedCountries": ["Russia", "North Korea", "USA"],
  "GeoApi": {
    "ApiKey": "<Your API Key>",
    "BaseUrl": "http://api.ipstack.com/"
  }
}
```

Alternatively, use [User Secrets](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets/) to securely store sensitive information like the API key.

---

## Setup and Registration

### 1. Install Required NuGet Packages

Ensure you have added any necessary packages, such as `Microsoft.Extensions.Configuration` for working with configuration files.

### 2. Add the GeoMiddleware in `Program.cs`

```csharp
var builder = WebApplication.CreateBuilder(args);

// Register GeoLocation services
builder.Services.AddHttpClient<IGeoLocationService,GeoLocationService>();

var app = builder.Build();

// Add GeoMiddleware to the pipeline
app.UseMiddleware<GeoMiddleware>();

app.Run();
```

---

## Usage

### **Testing with Mock Service**

To use the mock service for testing:

1. Replace `GeoLocationService` with `MockGeoLocationService` in your `Program.cs`.
 
   ```csharp
   builder.Services.AddSingleton<IGeoLocationService, MockGeoLocationService>();
   ```
2. Update your `appsettings.json` to use mock data:
 
   ```json
   "GeoSettings": {
     "BlockedCountries": ["Russia", "North Korea", "USA"]
   }
   ```
3. Run the application. The mock service will simulate geolocation results without calling an external API.

### Example Mock Data

The `MockGeoLocationService` will return predefined responses based on the input IP address. For example:

- `8.8.8.8` -> `United States`
- `1.1.1.1` -> `Australia`

You can modify the mock responses in the `MockGeoLocationService` implementation for different scenarios.

---

## Testing a Blocked Country Locally

### **Using Swagger**

1. Add an `X-Forwarded-For` header in your request to simulate an IP.
   - For example: `X-Forwarded-For: 8.8.8.8`
   Replace

  ```csharp
   var clientIp = context.Connection.RemoteIpAddress?.ToString();
   ```
   with 
   
   ```csharp
    var clientIp = context.Request.Headers["X-Forwarded-For"].FirstOrDefault()
                     ?? context.Connection.RemoteIpAddress?.ToString();
   ```
 in your `GeographyMiddleware.cs`.
 
2. Use an IP that belongs to a blocked country (e.g., an IP from Russia).
3. Ensure the response status is `403 Forbidden`.

5. Update your `appsettings.json` to use mock data:
   ```json
   "GeoSettings": {
     "BlockedCountries": ["Russia", "North Korea", "USA"]
   }
   ```
            /* // for testing 
             var clientIp = context.Request.Headers["X-Forwarded-For"].FirstOrDefault()
                     ?? context.Connection.RemoteIpAddress?.ToString();*/
### **Using Postman**

1. Open Postman and create a new request.
2. Add `X-Forwarded-For` header to the request with the desired IP address.
3. Test with an IP from a blocked country and verify the middleware returns `403 Forbidden`.

---

## Real Geolocation Service Setup

1. Obtain an API key from [ipstack](https://ipstack.com/).
2. Update the `GeoApi` section in `appsettings.json` with your API key and base URL:

   ```json
   "GeoApi": {
     "ApiKey": "<Your_API_Key>",
     "BaseUrl": "http://api.ipstack.com/"
   }
   ```
3. Test the middleware by making requests with real IP addresses and validating the geolocation results.

---

## Future Enhancements

- Add support for multiple geolocation services.
- Implement caching for geolocation lookups to reduce API calls.
- Add a feature to whitelist specific IPs regardless of their geolocation.

