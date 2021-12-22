using System;
using System.Collections.Generic;

#nullable disable

namespace VisingPackSolution.Data
{
    public partial class D750PoKpi
    {
        public int? Uid { get; set; }
        public string Ws { get; set; }
        public string Status { get; set; }
        public DateTime? Starttime { get; set; }
        public DateTime? Endtime { get; set; }
        public int? Target { get; set; }
        public int? Job { get; set; }
        public double? Runtime { get; set; }
        public double? Pausetime { get; set; }
        public double? Alarmtime { get; set; }
        public double? Perform { get; set; }
        public int? Defect { get; set; }
        public int? DefeatItem1 { get; set; }
        public int? DefeatItem2 { get; set; }
        public int? DefeatItem3 { get; set; }
        public int? DefeatItem4 { get; set; }
        public int? DefectFeedback { get; set; }
    }
}
