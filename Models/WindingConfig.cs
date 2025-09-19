using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using System.Threading.Tasks;

namespace WindingSimulator.Models
{
    /// <summary>
    /// Конфигурация параметров намотки нити
    /// </summary>
    public class WindingConfig
    {
        /// <summary>
        /// Начальный диаметр (мм) [D0]
        /// </summary>
        public double InitialDiameter { get; set; }

        /// <summary>
        /// Диаметр нити (мм) [d]
        /// </summary>
        public double ThreadDiameter { get; set; }

        /// <summary>
        /// Длина рабочей части (мм) [L]
        /// </summary>
        public double WorkingLength { get; set; }

        /// <summary>
        /// Общая длина нити (мм) [S_total]
        /// </summary>
        public double TotalThreadLength { get; set; }

        /// <summary>
        /// Скорость вращения (об/с) [ω]
        /// </summary>
        public double RotationSpeed { get; set; }

        /// <summary>
        /// Кол-во шагов до логирования информации
        /// </summary>
        public int ReportInterval { get; set; }

        /// <summary>
        /// Создаёт новую конфигурацию с параметрами по умолчанию
        /// </summary>
        public WindingConfig()
        {
            InitialDiameter = 20.0;
            ThreadDiameter = 0.5;
            WorkingLength = 50.0;
            TotalThreadLength = 10000.0;
            RotationSpeed = 2.0;
            ReportInterval = 100;
        }

        /// <summary>
        /// Создаёт конфигурацию с параметрами по умолчанию
        /// </summary>
        /// <returns>Конфигурация с параметрами по умолчанию</returns>
        public static WindingConfig CreateDefault()
        {
            return new WindingConfig();
        }
    }
}
