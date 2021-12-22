using System;
using System.Collections.Generic;

#nullable disable

namespace VisingPackSolution.Data.Speed.Entities
{
    public partial class DivSpeedHistrecord
    {
        public DateTime TriggerTime { get; set; }
        public double? ColaP601 { get; set; }
        public double? ColaP604 { get; set; }
        public double? ColaP605 { get; set; }
        public double? ColaP5m { get; set; }
        public double? ColaBtd2 { get; set; }
        public double? ColaBtd3 { get; set; }
        public double? ColaBtd4 { get; set; }
        public double? ColaBtd5 { get; set; }
        public double? ColaD650 { get; set; }
        public double? ColaD750 { get; set; }
        public double? ColaD1000 { get; set; }
        public double? ColaD1100 { get; set; }
        public double? ColaGmc1 { get; set; }
        public double? ColaGmc2 { get; set; }
        public double? ColaScl { get; set; }
    }
}
