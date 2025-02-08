# Root README.md

# Middleware Collection for ASP.NET Core

This repository contains a collection of reusable middlewares for ASP.NET Core applications. Each middleware is designed to handle a specific aspect of API functionality, and they can be plugged into your projects with minimal effort.

---

## Table of Contents

1. [GeoMiddleware](./Middleware/GeoMiddleware/README.md)
2. [MiddlewareX](./Middleware/MiddlewareX/README.md) (Coming Soon)

---

## How to Use This Repository

1. Clone the repository:

   ```bash
   git clone https://github.com/yourusername/YourRepoName.git
   ```

2. Add the desired middleware to your ASP.NET Core project:

   - Navigate to the middleware's folder for setup instructions.
   - Follow the installation steps in its `README.md`.

3. Register the middleware in your application pipeline in `Program.cs`.

---

## Contributing

Contributions are welcome! If you’d like to add a new middleware, please:

1. Create a folder in the `Middleware` directory.
2. Add your middleware implementation and a `README.md` for documentation.
3. Submit a pull request with your changes.

---

## License

[MIT](LICENSE)

---

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
builder.Services.AddGeoLocationServices();

var app = builder.Build();

// Add GeoMiddleware to the pipeline
app.UseMiddleware<GeoMiddleware>();

app.Run();
```

---

## Usage

### **Testing with Mock Service**

To use the mock service for testing:

1. Replace `IpStackGeoLocationService` with `MockGeoLocationService` in your `Program.cs`.
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
2. Use an IP that belongs to a blocked country (e.g., an IP from Russia).
3. Ensure the response status is `403 Forbidden`.

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

---

# MiddlewareX README.md (Template for future middlewares)

# MiddlewareX

**Description:** MiddlewareX provides [a brief description of functionality].

---

## Features

- Feature 1: [Description]
- Feature 2: [Description]

---

## Configuration

### **appsettings.json**

Add the following configuration to your `appsettings.json`:

```json
"MiddlewareXSettings": {
  "Key": "Value"
}
```

---

## Setup and Registration

### **Register in `Program.cs`**

```csharp
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMiddlewareXServices();
var app = builder.Build();

app.UseMiddleware<MiddlewareX>();
app.Run();
```

---

## Usage

### Example Use Case

Provide examples of how MiddlewareX can be used in real scenarios.

---

## Testing

Describe how to test MiddlewareX, either via integration or unit tests.

---

## Future Enhancements

List any planned updates or features for this middleware.

