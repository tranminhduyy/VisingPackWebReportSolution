using System;
using System.Collections.Generic;

#nullable disable

namespace VisingPackSolution.Data.Speed.Entities
{
    public partial class DivGmc1LengthHistrecord
    {
        public DateTime TriggerTime { get; set; }
        public double? ColaRealLength { get; set; }
        public double? ColaSetupLength { get; set; }
    }
}
