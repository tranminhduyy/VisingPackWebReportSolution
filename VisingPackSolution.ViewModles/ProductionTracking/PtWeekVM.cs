using System;
using System.Collections.Generic;
using System.Text;

namespace VisingPackSolution.ViewModels.ProductionTracking
{
    public class PtWeekVM
    {
        public double? TestingTime { get; set; }
        public double? RunningTime { get; set; }
        public double? NotRunningTime { get; set; }
        public double? TotalTime { get; set; }
        public string TestingTimeAndPercent { get; set; }
        public string RunningTimeAndPercent { get; set; }
        public string NotRunningTimeAndPercent { get; set; }
        public int ProductCodeChangeCount { get; set; }
        public int? Quantity { get; set; }
        public int? Waste { get; set; }
    }
}
