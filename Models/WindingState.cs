using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindingSimulator.Models
{
    public class WindingState
    {
        public double WoundLength { get; set; }          // мм
        public double Position { get; set; }             // мм
        public int CurrentLayerNum { get; set; }
        public double CurrentDiameter { get; set; }      // мм
        public bool Direction { get; set; }              // true = вправо
        public double LinearSpeed { get; set; }          // мм/с
        public double RealTime { get; set; }             // секунды
        public bool IsLayerComplete { get; set; }
        public bool IsLastStepOfIncompleteLayer { get; set; }
    }
}
