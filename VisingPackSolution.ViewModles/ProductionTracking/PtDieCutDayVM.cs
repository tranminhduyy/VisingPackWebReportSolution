using System;
using System.Collections.Generic;
using System.Text;

namespace VisingPackSolution.ViewModels.ProductionTracking
{
    public class PtDieCutDayVM
    {
        public List<PtDayVM> Btd2PTDay { get; set; }
        public List<PtDayVM> Btd3PTDay { get; set; }
        public List<PtDayVM> Btd4PTDay { get; set; }
        public List<PtDayVM> Btd5PTDay { get; set; }
    }
}
