using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindingSimulator.Models
{
    public class SimulationResult
    {
        public TimeSpan Duration { get; set; }
        public double TotalWoundLength { get; set; }
        public double FinalDiameter { get; set; }
        public int TotalLayers { get; set; }
        public bool IsLastLayerComplete { get; set; }
        public double EstimatedRealTime { get; set; }
    }
}
