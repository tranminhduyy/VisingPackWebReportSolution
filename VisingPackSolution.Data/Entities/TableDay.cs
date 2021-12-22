using System;
using System.Collections.Generic;
using System.Text;

namespace VisingPackSolution.Data
{
    public partial class TableDay
    {
        public string Machine { get; set; }
        public DateTime? Date { get; set; }
        public double? RunningTime { get; set; }
        public double? TestingTime { get; set; }
        public double? OtherTime { get; set; }
        public double? BreakTime { get; set; }
        public double? FixingTime { get; set; }
        public double? PendingTime { get; set; }
        public double? MaintenanceTime { get; set; }
        public double? PauseTime { get; set; }
        public int? PlannedWS { get; set; }
        public int? CompletedWS { get; set; }
        public int? Job { get; set; }
        public int? Defect { get; set; }
    }
}
