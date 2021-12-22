using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace VisingPackSolution.ViewModels.MachineState
{
    public class MsEventVM
    {
        //public int UID { get; set; }
        //public string WS { get; set; }

        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy hh:mm:ss tt}",
               ApplyFormatInEditMode = true)]
        public DateTime? Datetime { get; set; }
        public string Event { get; set; }
    }
}
