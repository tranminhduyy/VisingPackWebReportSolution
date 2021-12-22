using System;
using System.Collections.Generic;
using System.Text;

namespace VisingPackSolution.ViewModels.ProductionTracking
{
    public class PtGluingDayVM
    {
        public List<PtDayVM> D650PTDay { get; set; }
        public List<PtDayVM> D750PTDay { get; set; }
        public List<PtDayVM> D1000PTDay { get; set; }
        public List<PtDayVM> D1100PTDay { get; set; }
    }
}
