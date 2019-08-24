# Tuya Smart Plug API (.NET Standard 2.0)

This .NET Standard 2.0 API allows programmatic control over the basic functions of Tuya Smart Plugs, including getting and setting Power status.

<img src="tuya-plug.jpg">

## Requirements
* .NET Core 2.0

## Known-working devices:
* [Zentec Living Smart Plug Outlet with USB Port](https://www.amazon.com/gp/product/B074YGV2NK)
* [ISELECTOR Mini Smart Plug](https://www.amazon.com/gp/product/B075XL3DRD)
* [Xenon Smart Plug PW701U](https://www.amazon.com/Xenon-PW701U-Socket-Outlet-Android/dp/B06W55BTV5)
* [Teckin Smart Plug SH-SP23-2-UK](https://www.amazon.co.uk/gp/product/B07CVJYV3G)

Many Smart Plug devices compatible with the Tuya Smart Life and Jinvoo Smart app also appear to be compatible with the Tuya protocol.

## Installation
```powershell
Install-Package m4rcus.TuyaCore
```

## Retrieving Tuya Plug ID and LocalKey values:
Check out the instructions at [codetheweb/tuyapi](https://github.com/codetheweb/tuyapi/blob/master/docs/SETUP.md)
   
## Usage

### Console utility

```Powershell
> dotnet m4rcus.TuyaCore.Console.dll -i <ip> -k <localKey> -d <deviceId> [status|power-on|power-off]
```

### Querying status, toggling power (async)

```C#
using m4rcus.TuyaCore;
```

```C#
var device = new TuyaPlug()
{
    IP = "192.168.0.101",
    LocalKey = "5f5f784cd82d449b",
    Id = "0120015260091453a970"
};
var status = await device.GetStatus();
await device.SetStatus(!status.Powered); // toggle power
```

## Credits

Protocol details from @codetheweb and @clach04:
* https://github.com/codetheweb/tuyapi
* https://github.com/clach04/python-tuya/wiki
