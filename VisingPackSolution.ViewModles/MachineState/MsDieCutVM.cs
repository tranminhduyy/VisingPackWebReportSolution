using System;
using System.Collections.Generic;
using System.Text;

namespace VisingPackSolution.ViewModels.MachineState
{
    public class MsDieCutVM
    {
        public List<MsEventVM> BTD2Events { get; set; }
        public List<MsEventVM> BTD3Events { get; set; }
        public List<MsEventVM> BTD4Events { get; set; }
        public List<MsEventVM> BTD5Events { get; set; }
        public List<MsSpeedVM> BTD2Speeds { get; set; }
        public List<MsSpeedVM> BTD3Speeds { get; set; }
        public List<MsSpeedVM> BTD4Speeds { get; set; }
        public List<MsSpeedVM> BTD5Speeds { get; set; }
        public int BTD2ProductCodeChangeCount { get; set; }
        public int BTD3ProductCodeChangeCount { get; set; }
        public int BTD4ProductCodeChangeCount { get; set; }
        public int BTD5ProductCodeChangeCount { get; set; }
        public int? BTD2JobCount { get; set; }
        public int? BTD3JobCount { get; set; }
        public int? BTD4JobCount { get; set; }
        public int? BTD5JobCount { get; set; }
    }
}
