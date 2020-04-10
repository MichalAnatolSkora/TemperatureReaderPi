using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Iot.Device.OneWire;

namespace TemperatureReaderPi
{
    class Program
    {
        /// <summary>
        /// https://github.com/dotnet/iot/tree/master/src/devices/OneWire/samples
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        static async Task Main(string[] args)
        {
            // below ID is valid only for my device
            var ds18b20DeviceId = "28-0119501b89ff";
            // you can remove condition from "FirstOrDefault"
            var oneWireThermometerDevice = OneWireThermometerDevice.EnumerateDevices().FirstOrDefault(e => e.DeviceId == ds18b20DeviceId);
            if (oneWireThermometerDevice == null)
            {
                Console.WriteLine("Cannot find device \"28-0119501b89ff\"");
                return;
            }

            double? lastTemp = null;
            while (true)
            {
                var temp = await oneWireThermometerDevice.ReadTemperatureAsync();
                if (!lastTemp.HasValue || Math.Abs((decimal) (lastTemp.Value - temp.Celsius)) > 1)
                {
                    lastTemp = temp.Celsius;
                    var formattedTemp = temp.Celsius.ToString("F2") + "\u00B0C";
                    Console.WriteLine($"{DateTime.Now:G} {formattedTemp}");
                }

                Thread.Sleep(1000);
            }
        }
    }
}
