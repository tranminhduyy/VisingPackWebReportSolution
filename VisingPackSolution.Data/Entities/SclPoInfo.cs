using System;
using System.Collections.Generic;

#nullable disable

namespace VisingPackSolution.Data
{
    public partial class SclPoInfo
    {
        public int? Uid { get; set; }
        public DateTime? Date { get; set; }
        public string Ws { get; set; }
        public string Customer { get; set; }
        public string Product { get; set; }
        public string Material { get; set; }
        public string Quantitative { get; set; }
        public string RollLength { get; set; }
        public string RollNumber { get; set; }
        public string Note { get; set; }
    }
}
