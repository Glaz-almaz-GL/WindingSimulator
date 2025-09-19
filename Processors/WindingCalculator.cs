using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindingSimulator.Models;

namespace WindingSimulator.Processors
{
    public static class WindingCalculator
    {
        public static double CalculateLinearSpeed(WindingConfig config)
        {
            return config.RotationSpeed * config.ThreadDiameter;
        }

        public static double CalculateRealWindingTime(WindingConfig config)
        {
            try
            {
                if (config.RotationSpeed <= 0 || config.InitialDiameter <= 0)
                    return 0;

                double averageDiameter = config.InitialDiameter +
                    config.ThreadDiameter * config.TotalThreadLength / (config.WorkingLength * Math.PI);
                double averageCircumference = Math.PI * ((config.InitialDiameter + averageDiameter) / 2);

                if (averageCircumference > 0 && config.RotationSpeed > 0)
                {
                    return config.TotalThreadLength / (config.RotationSpeed * averageCircumference);
                }

                return 0;
            }
            catch
            {
                return 0;
            }
        }

        public static double CalculateCircumference(double diameter)
        {
            return Math.PI * diameter;
        }

        public static int CalculateStepsInLayer(WindingConfig config)
        {
            return (int)(config.WorkingLength / config.ThreadDiameter);
        }

        public static double CalculateLengthInLayer(WindingConfig config, double diameter)
        {
            double circumference = CalculateCircumference(diameter);
            double stepsInLayer = CalculateStepsInLayer(config);
            return stepsInLayer * circumference;
        }
    }
}
