# OctoPrintRestApiSharp
A simple C# library to communicate with an OctoPrint server via REST-API.

# Support me
If you want to support me, you can order over following affilate links (I'll get a small share from your purchase from the corresponding store).

- Prusa: http://www.prusa3d.com/#a_aid=AndreasReitberger *
- Jake3D: https://tidd.ly/3x9JOBp * 
- Amazon: https://amzn.to/2Z8PrDu *
- Coinbase: https://advanced.coinbase.com/join/KTKSEBP * (10€ in BTC for you if you open an account)
- TradeRepublic: https://refnocode.trade.re/wfnk80zm * (10€ in stocks for you if you open an account)

(*) Affiliate link
Thank you very much for supporting me!

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

# Platform specific setup

## Android

On `Android` you need to allow local connections in the `AndroidManifest.xml`.
For this, create a new xml file and link to it in your manifest at `android:networkSecurityConfig`

Content of the `network_security_config.xml` file
```
<?xml version="1.0" encoding="utf-8" ?>
<network-security-config>
	<base-config cleartextTrafficPermitted="true" />
</network-security-config>

```

The manifest
```xml
<?xml version="1.0" encoding="utf-8"?>
<manifest
	xmlns:android="http://schemas.android.com/apk/res/android"
	android:versionName="1.0.0"
	android:versionCode="1"
	package="com.company.app"
	>
	<application
		android:label="App Name"
		android:allowBackup="true"
		android:icon="@mipmap/appicon" 
		android:roundIcon="@mipmap/appicon_round"
		android:supportsRtl="true"
		android:networkSecurityConfig="@xml/network_security_config"
		>
	</application>
	<uses-permission android:name="android.permission.READ_EXTERNAL_STORAGE" />
	<uses-permission android:name="android.permission.WRITE_EXTERNAL_STORAGE" />
	<uses-permission android:name="android.permission.ACCESS_NETWORK_STATE" />
	<uses-permission android:name="android.permission.ACCESS_WIFI_STATE" />
	<uses-permission android:name="android.permission.FOREGROUND_SERVICE" />
	<uses-permission android:name="android.permission.WAKE_LOCK" />
	<uses-permission android:name="android.permission.INTERNET" />
</manifest>
```

## Init a new server
Just create a new `OctoPrintClient` object by passing the host, api, port and ssl connection type.
```csharp
OctoPrintClient _server = new OctoPrintClient(_host, _api, _port, _ssl);
await _server.CheckOnlineAsync();
if (_server.IsOnline)
{
    // Sets the first printer active
    if (_server.ActivePrinter is null)
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
