using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace VisingPackSolution.ViewModels.MachineState
{
    public class GetMsByTimeRequest
    {
        public string Selected { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
    }
}
