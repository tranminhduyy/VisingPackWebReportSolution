using System;
using System.Collections.Generic;
using System.Text;

namespace VisingPackSolution.ViewModels.MachineState
{
    public class MsGluingVM
    {
        public List<MsEventVM> D650Events { get; set; }
        public List<MsEventVM> D750Events { get; set; }
        public List<MsEventVM> D1000Events { get; set; }
        public List<MsEventVM> D1100Events { get; set; }
        public List<MsSpeedVM> D650Speeds { get; set; }
        public List<MsSpeedVM> D750Speeds { get; set; }
        public List<MsSpeedVM> D1000Speeds { get; set; }
        public List<MsSpeedVM> D1100Speeds { get; set; }
        public int D650ProductCodeChangeCount { get; set; }
        public int D750ProductCodeChangeCount { get; set; }
        public int D1000ProductCodeChangeCount { get; set; }
        public int D1100ProductCodeChangeCount { get; set; }
        public int? D650JobCount { get; set; }
        public int? D750JobCount { get; set; }
        public int? D1000JobCount { get; set; }
        public int? D1100JobCount { get; set; }
    }
}
