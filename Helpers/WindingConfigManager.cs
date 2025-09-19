using System;
using System.IO;
using System.Text.Json;
using WindingSimulator.Models;

namespace WindingSimulator.Helpers
{
    /// <summary>
    /// Менеджер конфигурации для симулятора намотки
    /// </summary>
    public static class WindingConfigManager
    {
        private const string _configFileName = "winding_config.json";
        private static readonly JsonSerializerOptions _options = new() { WriteIndented = true };

        /// <summary>
        /// Сохраняет конфигурацию в JSON-файл
        /// </summary>
        /// <param name="config">Конфигурация для сохранения</param>
        public static void SaveConfig(WindingConfig config)
        {
            try
            {
                string jsonString = JsonSerializer.Serialize(config, _options);
                File.WriteAllText(_configFileName, jsonString);
                Console.WriteLine($"Конфигурация сохранена в {_configFileName}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка сохранения конфигурации: {ex.Message}");
            }
        }

        /// <summary>
        /// Загружает конфигурацию из JSON-файла
        /// </summary>
        /// <returns>Загруженная конфигурация или конфигурация по умолчанию при ошибке</returns>
        public static WindingConfig LoadConfig()
        {
            try
            {
                if (ConfigFileExists())
                {
                    string jsonString = File.ReadAllText(_configFileName);
                    WindingConfig? windingConfig = JsonSerializer.Deserialize<WindingConfig>(jsonString);

                    if (windingConfig == null)
                    {
                        Console.WriteLine("Файл конфигурации пуст. Создаю новый.");
                        windingConfig = WindingConfig.CreateDefault();
                        SaveConfig(windingConfig);
                    }

                    return windingConfig;
                }
                else
                {
                    Console.WriteLine("Файл конфигурации не найден. Создаю новый.");
                    var defaultConfig = WindingConfig.CreateDefault();
                    SaveConfig(defaultConfig);
                    return defaultConfig;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка загрузки конфигурации: {ex.Message}");
                return WindingConfig.CreateDefault();
            }
        }

        /// <summary>
        /// Проверяет существование файла конфигурации
        /// </summary>
        /// <returns>True, если файл конфигурации существует</returns>
        public static bool ConfigFileExists()
        {
            return File.Exists(_configFileName);
        }

        /// <summary>
        /// Удаляет файл конфигурации (если существует)
        /// </summary>
        public static void DeleteConfigFile()
        {
            try
            {
                if (File.Exists(_configFileName))
                {
                    File.Delete(_configFileName);
                    Console.WriteLine($"Файл конфигурации {_configFileName} удален.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка удаления файла конфигурации: {ex.Message}");
            }
        }

        /// <summary>
        /// Проверяет корректность конфигурационных данных
        /// </summary>
        /// <returns>True, если все параметры корректны</returns>
        public static bool ValidateConfig(WindingConfig _config)
        {
            var errors = new List<string>();

            if (_config.InitialDiameter <= 0)
                errors.Add("Начальный диаметр должен быть больше 0");

            if (_config.ThreadDiameter <= 0)
                errors.Add("Диаметр нити должен быть больше 0");

            if (_config.WorkingLength <= 0)
                errors.Add("Длина рабочей части должна быть больше 0");

            if (_config.TotalThreadLength <= 0)
                errors.Add("Общая длина нити должна быть больше 0");

            if (_config.RotationSpeed <= 0)
                errors.Add("Скорость вращения должна быть больше 0");

            if (_config.ReportInterval < 0)
                errors.Add("Интервал отчета не может быть отрицательным");

            if (errors.Count > 0)
            {
                ConsoleDisplayManager.PrintSectionHeader("ОШИБКИ В КОНФИГУРАЦИИ");
                foreach (var error in errors)
                {
                    ConsoleDisplayManager.PrintColoredMessage($"   \u2718 {error}", ConsoleColor.Red, true);
                }
                Console.WriteLine();
                return false;
            }

            return true;
        }
    }
}