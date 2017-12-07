using System;

namespace m4rcus.TuyaCore.ConsoleDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            var device = new TuyaPlug()
            {
                IP = "192.168.0.101",
                LocalKey = "5f5f784cd82d449b",
                Id = "0120015260091453a970"
            };
            var status = device.GetStatus().Result;

            Console.WriteLine($"Device[{device.Id}] Status was: {status.Powered}");

            device.SetStatus(!status.Powered).Wait(); // toggle power

            Console.WriteLine($"Device[{device.Id}] Status now: {device.GetStatus().Result.Powered}");
        }
    }
}
