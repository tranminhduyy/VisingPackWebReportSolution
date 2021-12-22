using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VisingPackSolution.Application.WorkTimeManage;
using VisingPackSolution.ViewModels.Common;

namespace VisingPackSolution.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class WorkTimeManagesController : ControllerBase
    {       
        private readonly IWorkTimeManageService _wtService;
        public WorkTimeManagesController(IWorkTimeManageService wtService)
        {
            _wtService = wtService;
        }

        [HttpGet("WtPrinting")]
        public async Task<IActionResult> GetWtPrinting([FromQuery] TimeRequest request)
        {
            var result = await _wtService.GetPrintingWorkTimeMgt(request);
            return Ok(result);
        }

        [HttpGet("WtDieCut")]
        public async Task<IActionResult> GetWtDieCut([FromQuery] TimeRequest request)
        {
            var result = await _wtService.GetDieCutWorkTimeMgt(request);
            return Ok(result);
        }

        [HttpGet("WtGluing")]
        public async Task<IActionResult> GetWtGluing([FromQuery] TimeRequest request)
        {
            var result = await _wtService.GetGluingWorkTimeMgt(request);
            return Ok(result);
        }

        [HttpGet("WtSclGmc")]
        public async Task<IActionResult> GetWtSclGmc([FromQuery] TimeRequest request)
        {
            var result = await _wtService.GetSclGmcWorkTimeMgt(request);
            return Ok(result);
        }
    }
}