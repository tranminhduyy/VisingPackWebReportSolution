using System;
using System.Collections.Generic;
using System.Text;

namespace VisingPackSolution.ViewModels.MachineState
{
    public class P601EventCreateRequest
    {
        public int UID { get; set; }
        public string WS { get; set; }
        public string Event { get; set; }
    }
}
