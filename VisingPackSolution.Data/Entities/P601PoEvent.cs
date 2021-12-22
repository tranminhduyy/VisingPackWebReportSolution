using System;
using System.Collections.Generic;

#nullable disable

namespace VisingPackSolution.Data
{
    public partial class P601PoEvent
    {
        public int Uid { get; set; }
        public string Ws { get; set; }
        public DateTime? Datetime { get; set; }
        public string Event { get; set; }
    }
}
