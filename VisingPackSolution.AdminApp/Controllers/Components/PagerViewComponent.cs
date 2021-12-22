﻿using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using VisingPackSolution.ViewModels.Common;

namespace VisingPackSolution.AdminApp.Controllers.Components
{
    public class PagerViewComponent : ViewComponent
    {
        public Task<IViewComponentResult> InvokeAsync(PagedResultBase result)
        {
            return Task.FromResult((IViewComponentResult)View("Default", result));
        }
    }
}
