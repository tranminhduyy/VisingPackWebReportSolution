using System;
using System.Collections.Generic;
using System.Text;

namespace VisingPackSolution.ViewModels.WorkTimeManage
{
    public class WorkTimeVM
    {
        public double? RunningTime { get; set; }
        public double? TestingTime { get; set; }
        public double? OtherTime { get; set; }
        public double? BreakTime { get; set; }
        public double? FixingTime { get; set; }
        public double? PendingTime { get; set; }
        public double? MaintenanceTime { get; set; }
        public double? PauseTime { get; set; }
    }
}
