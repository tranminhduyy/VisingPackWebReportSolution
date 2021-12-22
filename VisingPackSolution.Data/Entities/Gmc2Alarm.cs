using System;
using System.Collections.Generic;

#nullable disable

namespace VisingPackSolution.Data
{
    public partial class Gmc2Alarm
    {
        public DateTime? Datetime { get; set; }
        public string Alarm { get; set; }
        public string State { get; set; }
    }
}
