using System;
using System.Collections.Generic;
using System.Text;

namespace VisingPackSolution.ViewModels.Common
{
    public class PagedResult<T> : PagedResultBase
    {
        public IEnumerable<T> Items { set; get; }
    }
}
