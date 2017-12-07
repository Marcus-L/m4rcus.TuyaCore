using McMaster.Extensions.CommandLineUtils;
using System;

namespace m4rcus.TuyaCore.Console
{
    class Program
    {
        static int Main(string[] args)
        {
            var app = new CommandLineApplication
            {
                Name = "m4rcus.TuyaCore.Console",
                Description = "Tuya Plug Console Tool"
            };
            app.HelpOption("-h|--help");

            var ipOption = app.Option("-i|--ip <address>", "Device Address", CommandOptionType.SingleValue).IsRequired();

            var idOption = app.Option("-d|--device <deviceid>", "Device Id", CommandOptionType.SingleValue).IsRequired();

            var keyOption = app.Option("-k|--key <localkey>", "Device LocalKey", CommandOptionType.SingleValue).IsRequired();

            var GetPlug = new Func<TuyaPlug>(() => new TuyaPlug()
            {
                IP = ipOption.Value(),
                LocalKey = keyOption.Value(),
                Id = idOption.Value()
            });

            app.Command("status", (command) =>
            {
                command.Description = "Retrieves the status of the plug.";
                command.OnExecute(() =>
                {
                    var device = GetPlug();
                    System.Console.WriteLine($"Device[{device.Id}] status: {device.GetStatus().Result.Powered}");
                    return 0;
                });
            });

            app.Command("power-on", (command) =>
            {
                command.Description = "Turns the power status to on.";
                command.OnExecute(() =>
                {
                    var device = GetPlug();
                    device.SetStatus(true).Wait();
                    System.Console.WriteLine($"Device[{device.Id}] status: {device.GetStatus().Result.Powered}");
                    return 0;
                });
            });

            app.Command("power-off", (command) =>
            {
                command.Description = "Turns the power status to off.";
                command.OnExecute(() =>
                {
                    var device = GetPlug();
                    device.SetStatus(false).Wait();
                    System.Console.WriteLine($"Device[{device.Id}] status: {device.GetStatus().Result.Powered}");
                    return 0;
                });
            });

            return app.Execute(args);
        }
    }
}
