// WindingSimulator.cs
using System;
using WindingSimulator.Helpers;
using WindingSimulator.Models;
using WindingSimulator.Processors;

namespace WindingSimulator
{
    public class WindingSimulator
    {
        private readonly WindingConfig _config;
        private readonly WindingState _state;

        public WindingSimulator(WindingConfig config)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));

            _state = new WindingState();
            Initialize();
        }

        private void Initialize()
        {
            _state.WoundLength = 0;
            _state.Position = 0;
            _state.CurrentLayerNum = 0;
            _state.CurrentDiameter = _config.InitialDiameter;
            _state.Direction = true;                                                    // начинаем движение вправо
            _state.LinearSpeed = WindingCalculator.CalculateLinearSpeed(_config);
            _state.IsLayerComplete = true;                                              // Первый слой считаем новым
            _state.IsLastStepOfIncompleteLayer = false;
            _state.RealTime = WindingCalculator.CalculateRealWindingTime(_config);
        }

        public SimulationResult? RunSimulation()
        {
            try
            {
                WindingDisplayManager.DisplaySimulationHeader();

                if (!WindingConfigManager.ValidateConfig(_config))
                {
                    ConsoleDisplayManager.PrintColoredMessage("Симуляция прервана из-за ошибок в конфигурации.", ConsoleColor.Red, true);
                    return null;
                }

                WindingDisplayManager.DisplaySimulationInfo(_config, _state);

                DateTime startTime = DateTime.Now;
                int stepCount = 0;

                ConsoleDisplayManager.PrintSectionHeader("ПРОЦЕСС НАМОТКИ");

                RunWindingProcess(ref stepCount);

                DateTime endTime = DateTime.Now;

                var result = new SimulationResult
                {
                    Duration = endTime - startTime,
                    TotalWoundLength = _state.WoundLength,
                    FinalDiameter = _state.CurrentDiameter,
                    TotalLayers = _state.CurrentLayerNum + 1,
                    IsLastLayerComplete = _state.IsLayerComplete,
                    EstimatedRealTime = _state.RealTime
                };

                WindingDisplayManager.DisplayResults(result);

                return result;
            }
            catch (Exception ex)
            {
                ConsoleDisplayManager.PrintColoredMessage($"Ошибка во время симуляции: {ex.Message}", ConsoleColor.Red, true);
                return null;
            }
        }

        private void RunWindingProcess(ref int stepCount)
        {
            while (_state.WoundLength < _config.TotalThreadLength)
            {
                if (ProcessLayer(ref stepCount))
                    break;
            }
        }

        private bool ProcessLayer(ref int stepCount)
        {
            double remainingLength = _config.TotalThreadLength - _state.WoundLength;
            double circumference = WindingCalculator.CalculateCircumference(_state.CurrentDiameter);
            double stepsInFullLayer = _config.WorkingLength / _config.ThreadDiameter;
            double lengthInFullLayer = stepsInFullLayer * circumference;

            if (remainingLength <= lengthInFullLayer)
            {
                return WindingProcessor.ProcessIncompleteLayer(_state, _config, remainingLength, circumference, ref stepCount);
            }
            else
            {
                int stepsInLayer = (int)stepsInFullLayer;
                return WindingProcessor.ProcessFullLayer(_state, _config, ref stepCount, stepsInLayer);
            }
        }
    }
}