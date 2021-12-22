using System;
using System.Collections.Generic;
using System.Text;

namespace VisingPackSolution.ViewModels.MachineState
{
    public class MsPrintingVM
    {
        public List<MsEventVM> P601Events { get; set; }
        public List<MsEventVM> P604Events { get; set; }
        public List<MsEventVM> P605Events { get; set; }
        public List<MsEventVM> P5MEvents { get; set; }
        public List<MsSpeedVM> P601Speeds { get; set; }
        public List<MsSpeedVM> P604Speeds { get; set; }
        public List<MsSpeedVM> P605Speeds { get; set; }
        public List<MsSpeedVM> P5MSpeeds { get; set; }
        public int P601ProductCodeChangeCount { get; set; }
        public int P604ProductCodeChangeCount { get; set; }
        public int P605ProductCodeChangeCount { get; set; }
        public int P5MProductCodeChangeCount { get; set; }
        public int? P601JobCount { get; set; }
        public int? P604JobCount { get; set; }
        public int? P605JobCount { get; set; }
        public int? P5MJobCount { get; set; }
    }
}
