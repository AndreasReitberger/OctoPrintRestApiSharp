# OctoPrintRestApiSharp
A simple C# library to communicate with an OctoPrint server via REST-API.

# Nuget
Get the latest version from nuget.org<br>
[![NuGet](https://img.shields.io/nuget/v/RepetierServerSharpApi.svg?style=flat-square&label=nuget)](https://www.nuget.org/packages/RepetierServerSharpApi/)
[![NuGet](https://img.shields.io/nuget/dt/RepetierServerSharpApi.svg)](https://www.nuget.org/packages/RepetierServerSharpApi)

# Usage
You can find some usage examples in the TestProject of the source code.

## Init a new server
Just create a new `OctoPrintServer` object by passing the host, api, port and ssl connection type.
```csharp
OctoPrintServer _server = new OctoPrintServer(_host, _api, _port, _ssl);
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
If you want to use the OctoPrintServer from different places, use the `Instance`.
```csharp
OctoPrintServer.Instance = new OctoPrintServer(_host, _api, _port, _ssl);
await OctoPrintServer.Instance.CheckOnlineAsync();
```

Aferwards you can use the OctoPrintServer.Instance property to access all functions 
through your project.
```csharp
var files = await OctoPrintServer.Instance.GetAllFilesAsync("local");
```

# Available methods
Please find the some usage examples for the methods below.

```csharp
// Load all files from the Server
var files = await OctoPrintServer.Instance.GetAllFilesAsync("local");

// Get all printer profiles
var printers = await OctoPrintServer.Instance.GetAllPrinterProfilesAsync();

// Update heated bed temperature & read back the state
bool result = await OctoPrintServer.Instance.SetBedTemperatureAsync(25);
var state = await OctoPrintServer.Instance.GetCurrentBedStateAsync(true);

// Update extruder (tool0 or / and tool1) & read back the state
bool result = await _OctoPrintServer.Instance.SetToolTemperatureAsync(30);
var state = await OctoPrintServer.Instance.GetCurrentToolStateAsync(true);

var jobinfo = await OctoPrintServer.Instance.GetJobInfoAsync();
```

# Dependencies
RCoreSharp: https://github.com/AndreasReitberger/CoreSharp
