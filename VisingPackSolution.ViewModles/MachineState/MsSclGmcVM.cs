using System;
using System.Collections.Generic;
using System.Text;

namespace VisingPackSolution.ViewModels.MachineState
{
    public class MsSclGmcVM
    {
        public List<MsEventVM> SclEvents { get; set; }
        public List<MsEventVM> Gmc1Events { get; set; }
        public List<MsEventVM> Gmc2Events { get; set; }
        public List<MsSpeedVM> SclSpeeds { get; set; }
        public List<MsSpeedVM> Gmc1Speeds { get; set; }
        public List<MsSpeedVM> Gmc2Speeds { get; set; }
        public int SclProductCodeChangeCount { get; set; }
        public int Gmc1ProductCodeChangeCount { get; set; }
        public int Gmc2ProductCodeChangeCount { get; set; }
        public int? SclJobCount { get; set; }
        public int? Gmc1JobCount { get; set; }
        public int? Gmc2JobCount { get; set; }
    }
}
