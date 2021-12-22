using System;
using System.Collections.Generic;

#nullable disable

namespace VisingPackSolution.Data.Speed.Entities
{
    public partial class DivGmc1SpeedHistrecord
    {
        public DateTime TriggerTime { get; set; }
        public double? ColaHistVariable { get; set; }
        public double? ColaMaxSpeed { get; set; }
    }
}
