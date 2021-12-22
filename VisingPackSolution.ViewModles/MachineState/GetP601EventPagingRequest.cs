using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using VisingPackSolution.ViewModels.Common;

namespace VisingPackSolution.ViewModels.MachineState
{
    public class GetP601EventPagingRequest : PagingRequestBase
    {
        public string Keyword { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}",
               ApplyFormatInEditMode = true)]
        public DateTime Starttime { get; set; }
    }
}
