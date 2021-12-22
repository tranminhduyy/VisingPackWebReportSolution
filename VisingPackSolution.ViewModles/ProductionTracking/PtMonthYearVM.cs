using System;
using System.Collections.Generic;
using System.Text;

namespace VisingPackSolution.ViewModels.ProductionTracking
{
    public class PtMonthYearVM
    {
        public double? ActualWorkingTime { get; set; }
        public double? Downtime { get; set; }
        public int? TotalOutput { get; set; }
        public double? MaximumProductionCapacity { get; set; }
        public int? Waste { get; set; }
    }
}
