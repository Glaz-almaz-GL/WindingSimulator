using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindingSimulator.Models;

namespace WindingSimulator.Helpers
{
    public static class WindingDisplayManager
    {
        /// <summary>
        /// Отображает заголовок симуляции
        /// </summary>
        public static void DisplaySimulationHeader()
        {
            Console.Clear();
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            ConsoleDisplayManager.PrintColoredMessage("\n╔══════════════════════════════════════════════════════════════════════════════╗", ConsoleColor.Cyan, true);
            ConsoleDisplayManager.PrintColoredMessage("║                                                                              ║", ConsoleColor.Cyan, true);
            ConsoleDisplayManager.PrintColoredMessage("║                          СИМУЛЯЦИЯ НАМОТКИ НИТИ                              ║", ConsoleColor.Cyan, true);
            ConsoleDisplayManager.PrintColoredMessage("║                                                                              ║", ConsoleColor.Cyan, true);
            ConsoleDisplayManager.PrintColoredMessage("╚══════════════════════════════════════════════════════════════════════════════╝\n", ConsoleColor.Cyan, true);
        }

        /// <summary>
        /// Отображает информацию о параметрах симуляции
        /// </summary>
        public static void DisplaySimulationInfo(WindingConfig config, WindingState state)
        {
            ConsoleDisplayManager.PrintSectionHeader("ПАРАМЕТРЫ НАМОТКИ");

            ConsoleDisplayManager.PrintKeyValue("Диаметр нити", $"{config.ThreadDiameter:F2} мм", ConsoleColor.DarkCyan, ConsoleColor.Gray);
            ConsoleDisplayManager.PrintKeyValue("Скорость вращения", $"{config.RotationSpeed:F2} об/с", ConsoleColor.DarkCyan, ConsoleColor.Gray);
            ConsoleDisplayManager.PrintKeyValue("Скорость перемещения", $"{state.LinearSpeed:F2} мм/с", ConsoleColor.DarkCyan, ConsoleColor.Gray);
            ConsoleDisplayManager.PrintKeyValue("Начальный диаметр", $"{config.InitialDiameter:F2} мм", ConsoleColor.DarkCyan, ConsoleColor.Gray);
            ConsoleDisplayManager.PrintKeyValue("Длина рабочей части", $"{config.WorkingLength:F2} мм", ConsoleColor.DarkCyan, ConsoleColor.Gray);
            ConsoleDisplayManager.PrintKeyValue("Общая длина нити", $"{config.TotalThreadLength:F2} мм", ConsoleColor.DarkCyan, ConsoleColor.Gray);
            ConsoleDisplayManager.PrintKeyValue("Интервал логирования", $"{config.ReportInterval} шагов", ConsoleColor.DarkCyan, ConsoleColor.Gray);

            if (state.RealTime > 0)
            {
                ConsoleDisplayManager.PrintKeyValue("Реальное время намотки", $"{state.RealTime:F1} секунд", ConsoleColor.DarkCyan, ConsoleColor.Gray);
            }
            else
            {
                ConsoleDisplayManager.PrintKeyValue("Реальное время намотки", "Невозможно рассчитать", ConsoleColor.DarkCyan, ConsoleColor.Gray);
            }

            Console.WriteLine();
        }

        /// <summary>
        /// Отображает результаты симуляции
        /// </summary>
        public static void DisplayResults(SimulationResult result)
        {
            ConsoleDisplayManager.PrintSectionHeader("РЕЗУЛЬТАТЫ НАМОТКИ");

            ConsoleDisplayManager.PrintKeyValue("Общая намотанная длина", $"{result.TotalWoundLength:F1} мм", ConsoleColor.Yellow, ConsoleColor.White);
            ConsoleDisplayManager.PrintKeyValue("Конечный диаметр катушки", $"{result.FinalDiameter:F2} мм", ConsoleColor.Yellow, ConsoleColor.White);
            ConsoleDisplayManager.PrintKeyValue("Завершено слоёв", $"{result.TotalLayers} ({(result.IsLastLayerComplete ? "полный" : "неполный")} последний слой)", ConsoleColor.Yellow, ConsoleColor.White);
            ConsoleDisplayManager.PrintKeyValue("Время симуляции", $"{result.Duration.TotalSeconds:F4} секунд", ConsoleColor.Yellow, ConsoleColor.White);

            if (result.EstimatedRealTime > 0)
            {
                ConsoleDisplayManager.PrintKeyValue("Средняя скорость намотки", $"{result.TotalWoundLength / result.EstimatedRealTime:F1} мм/с", ConsoleColor.Yellow, ConsoleColor.White);
            }

            Console.WriteLine();
            ConsoleDisplayManager.PrintColoredMessage("\u2714 Симуляция завершена успешно!", ConsoleColor.Green, true);
            Console.WriteLine();
        }

        public static bool ShouldLogProgress(int stepCount, int reportInterval, bool isLastStep, bool isComplete)
        {
            return reportInterval > 0 &&
                   (stepCount % reportInterval == 0 ||
                    isLastStep ||
                    isComplete);
        }

        public static void ReportLayerComplete(WindingState state)
        {
            string progressInfo = $"Завершён слой {state.CurrentLayerNum + 1} {(state.IsLayerComplete ? "(полный)" : "(неполный)")}. Новый диаметр: {state.CurrentDiameter} мм";
            ConsoleDisplayManager.PrintColoredMessage(progressInfo, ConsoleColor.Green, true);
        }

        public static void ReportProgress(WindingState state)
        {
            string layerStatus = state.IsLastStepOfIncompleteLayer ?
                $" (слой {state.CurrentLayerNum + 1}, последний шаг)" :
                $" (слой {state.CurrentLayerNum + 1})";

            string progressInfo = $"Намотано: {state.WoundLength:F1} мм{layerStatus} | " +
                                 $"Диаметр: {state.CurrentDiameter:F2} мм | " +
                                 $"Позиция: {state.Position:F2} мм";

            ConsoleDisplayManager.PrintColoredMessage(progressInfo, ConsoleColor.Gray, true);

            if (state.IsLastStepOfIncompleteLayer)
            {
                ReportLayerComplete(state);
            }
        }
    }
}
