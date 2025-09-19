using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindingSimulator.Helpers;
using WindingSimulator.Models;

namespace WindingSimulator.Processors
{
    public static class WindingProcessor
    {
        public static void MoveStep(WindingState state, WindingConfig config)
        {
            // Обновляем позицию
            if (state.Direction)
            {
                state.Position += config.ThreadDiameter;
            }
            else
            {
                state.Position -= config.ThreadDiameter;
            }

            // Ограничиваем позицию в пределах рабочей длины
            if (state.Position > config.WorkingLength)
            {
                state.Position = config.WorkingLength;
            }

            if (state.Position < 0)
            {
                state.Position = 0;
            }

            // Добавляем длину одного витка
            double circumference = WindingCalculator.CalculateCircumference(state.CurrentDiameter);
            state.WoundLength += circumference;
        }

        public static void CompleteLayer(WindingState state, WindingConfig config)
        {
            WindingDisplayManager.ReportLayerComplete(state);

            state.CurrentLayerNum++;
            state.Direction = !state.Direction; // Меняем направление
            state.CurrentDiameter = config.InitialDiameter + (2 * state.CurrentLayerNum * config.ThreadDiameter);
            state.IsLayerComplete = true;
            state.IsLastStepOfIncompleteLayer = false;
        }

        public static bool ProcessFullLayer(WindingState state, WindingConfig config, ref int stepCount, int stepsInLayer)
        {
            int stepsInCurrentLayer = 0;

            for (int i = 0; i < stepsInLayer && state.WoundLength < config.TotalThreadLength; i++)
            {
                MoveStep(state, config);
                stepCount++;
                stepsInCurrentLayer++;

                if (config.ReportInterval > 0 && stepCount % config.ReportInterval == 0)
                {
                    WindingDisplayManager.ReportProgress(state);
                }
            }

            if (state.WoundLength < config.TotalThreadLength)
            {
                CompleteLayer(state, config);
            }

            return false;
        }

        public static bool ProcessIncompleteLayer(WindingState state, WindingConfig config,
                                         double remainingLength, double circumference, ref int stepCount)
        {
            state.IsLayerComplete = false;
            int stepsInCurrentLayer = 0;

            double stepsNeeded = remainingLength / circumference;
            int fullSteps = (int)stepsNeeded;

            for (int i = 0; i < fullSteps && state.WoundLength < config.TotalThreadLength; i++)
            {
                MoveStep(state, config);
                stepCount++;
                stepsInCurrentLayer++;

                state.IsLastStepOfIncompleteLayer = i == fullSteps - 1 || state.WoundLength >= config.TotalThreadLength;

                if (WindingDisplayManager.ShouldLogProgress(stepCount, config.ReportInterval,
                                                      state.IsLastStepOfIncompleteLayer,
                                                      state.WoundLength >= config.TotalThreadLength))
                {
                    WindingDisplayManager.ReportProgress(state);
                }
            }

            return true; // Завершаем процесс
        }
    }
}
