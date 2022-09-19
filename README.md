# OctoPrintRestApiSharp
A simple C# library to communicate with an OctoPrint server via REST-API.

# Important!
With the upcoming version, starting from `1.2.0`, `OctoPrintServer` become `OctoPrintClient`. also the namespaces will changed and generalized with our other print server api nugets.

| Old                             | New                              |
| ------------------------------- |:--------------------------------:|
| `AndreasReitberger`             | `AndreasReitberger.API.OctoPrint`|
| `OctoPrintServer`               | `OctoPrintClient`                |


# Nuget
Get the latest version from nuget.org<br>
[![NuGet](https://img.shields.io/nuget/v/OctoPrintSharpApi.svg?style=flat-square&label=nuget)](https://www.nuget.org/packages/OctoPrintSharpApi/)
[![NuGet](https://img.shields.io/nuget/dt/OctoPrintSharpApi.svg)](https://www.nuget.org/packages/OctoPrintSharpApi)

# Usage
You can find some usage examples in the TestProject of the source code.

## Init a new server
Just create a new `OctoPrintClient` object by passing the host, api, port and ssl connection type.
```csharp
OctoPrintClient _server = new OctoPrintClient(_host, _api, _port, _ssl);
await _server.CheckOnlineAsync();
if (_server.IsOnline)
{
    // Sets the first printer active
    if (_server.ActivePrinter == null)
        await _server.SetPrinterActiveAsync(0, true);

    await _server.RefreshAllAsync();
    Assert.IsTrue(_server.InitialDataFetched);
}
```

Since then, you can access all functions from the `OctoPrintServer` object.

## Instance
If you want to use the `OctoPrintClient` from different places, use the `Instance`.
```csharp
OctoPrintClient.Instance = new OctoPrintClient(_host, _api, _port, _ssl);
await OctoPrintClient.Instance.CheckOnlineAsync();
```

Aferwards you can use the OctoPrintServer.Instance property to access all functions 
through your project.
```csharp
var files = await OctoPrintClient.Instance.GetAllFilesAsync("local");
```

# Available methods
Please find the some usage examples for the methods below.

```csharp
// Load all files from the Server
var files = await OctoPrintClient.Instance.GetAllFilesAsync("local");

// Get all printer profiles
var printers = await OctoPrintClient.Instance.GetAllPrinterProfilesAsync();

// Update heated bed temperature & read back the state
bool result = await OctoPrintClient.Instance.SetBedTemperatureAsync(25);
var state = await OctoPrintClient.Instance.GetCurrentBedStateAsync(true);

// Update extruder (tool0 or / and tool1) & read back the state
bool result = await OctoPrintClient.Instance.SetToolTemperatureAsync(30);
var state = await OctoPrintClient.Instance.GetCurrentToolStateAsync(true);

var jobinfo = await OctoPrintClient.Instance.GetJobInfoAsync();
```

# Dependencies
RCoreSharp: https://github.com/AndreasReitberger/CoreSharp
