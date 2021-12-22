using System;
using System.Collections.Generic;
using System.Text;
using VisingPackSolution.ViewModels.Common;

namespace VisingPackSolution.ViewModels.System.Users
{
    public class GetUserPagingRequest : PagingRequestBase
    {
        public string Keyword { get; set; }
    }
}
