using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindingSimulator.Helpers;
using WindingSimulator.Models;

namespace WindingSimulator
{
    public static class Program
    {
        static void Main()
        {
            // Конфигурация
            WindingConfig config = WindingConfigManager.LoadConfig();

            var simulator = new WindingSimulator(config);
            simulator.RunSimulation();

            Console.ReadKey();
        }
    }
}
