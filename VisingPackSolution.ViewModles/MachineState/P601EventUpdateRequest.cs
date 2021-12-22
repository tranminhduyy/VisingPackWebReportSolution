using System;
using System.Collections.Generic;
using System.Text;

namespace VisingPackSolution.ViewModels.MachineState
{
    public class P601EventUpdateRequest
    {
        public int Uid { get; set; }
        public string Ws { get; set; }
        public string Event { get; set; }
    }
}
