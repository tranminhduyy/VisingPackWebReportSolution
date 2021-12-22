using System;
using System.Collections.Generic;
using System.Text;

namespace VisingPackSolution.ViewModels.ProductionTracking
{
    public class PtPrintingDayVM
    {
        public List<PtDayVM> P601PTDay { get; set; }
        public List<PtDayVM> P604PTDay { get; set; }
        public List<PtDayVM> P605PTDay { get; set; }
        public List<PtDayVM> P5mPTDay { get; set; }
    }
}
