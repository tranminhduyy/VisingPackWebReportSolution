using System;
using System.Collections.Generic;

#nullable disable

namespace VisingPackSolution.Data
{
    public partial class Gmc2PoInfo
    {
        public int? Uid { get; set; }
        public DateTime? Date { get; set; }
        public string Ws { get; set; }
        public string Customer { get; set; }
        public string Product { get; set; }
        public string Order { get; set; }
        public string Upe { get; set; }
        public string Material { get; set; }
        public string Quantitative { get; set; }
        public string CutX { get; set; }
        public string CutY { get; set; }
        public string Target { get; set; }
        public string Ra { get; set; }
        public string CropX { get; set; }
        public string CropY { get; set; }
        public string CropTarget { get; set; }
        public string Weight { get; set; }
    }
}
