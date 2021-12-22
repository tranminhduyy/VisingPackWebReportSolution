using System;
using System.Collections.Generic;
using System.Text;

namespace VisingPackSolution.ViewModels.ProductionTracking
{
    public class PtDayVM
    {
        public string Ws { get; set; }
        public string Product { get; set; }
        public int? Job { get; set; }
        public DateTime? Endtime { get; set; }
        public string Plantime { get; set; }
        public string PlanEndtime { get; set; }
        public string Status { get; set; }
    }
}
