using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace VisingPackSolution.ViewModels.MachineState
{
    public class MsSpeedVM
    {
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy hh:mm:ss tt}",
               ApplyFormatInEditMode = true)]
        public DateTime? Datetime { get; set; }
        public double? Speed { get; set; }
    }
}
