using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VisingPackSolution.Application.ProductionTracking;
using VisingPackSolution.ViewModels.Common;
using VisingPackSolution.ViewModels.ProductionTracking;

namespace VisingPackSolution.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductionTrackingsController : ControllerBase
    {
        private readonly IProductionTrackingService _ptService;
        public ProductionTrackingsController(IProductionTrackingService ptService)
        {
            _ptService = ptService;
        }

        [HttpGet("PtPrinting1H")]
        public async Task<IActionResult> GetPtPrinting1H([FromQuery] TimeRequest request)
        {
            var dataP601 = await _ptService.GetP601ProductionTracking_1H(request);
            var dataP604 = await _ptService.GetP604ProductionTracking_1H(request);
            var dataP605 = await _ptService.GetP605ProductionTracking_1H(request);
            var dataP5m = await _ptService.GetP5mProductionTracking_1H(request);
            var result = new PtPrinting1HVM()
            {
                P601PT1H = dataP601,
                P604PT1H = dataP604,
                P605PT1H = dataP605,
                P5mPT1H = dataP5m,
            };
            return Ok(result);
        }

        [HttpGet("PtPrintingDay")]
        public async Task<IActionResult> GetPtPrintingDay([FromQuery] TimeRequest request)
        {
            var dataP601 = await _ptService.GetP601ProductionTracking_Day(request);
            var dataP604 = await _ptService.GetP604ProductionTracking_Day(request);
            var dataP605 = await _ptService.GetP605ProductionTracking_Day(request);
            var dataP5m = await _ptService.GetP5mProductionTracking_Day(request);
            var result = new PtPrintingDayVM()
            {
                P601PTDay = dataP601,
                P604PTDay = dataP604,
                P605PTDay = dataP605,
                P5mPTDay = dataP5m,
            };
            return Ok(result);
        }

        [HttpGet("PtPrintingWeek")]
        public async Task<IActionResult> GetPtPrintingWeek([FromQuery] TimeRequest request)
        {
            var dataP601 = await _ptService.GetP601ProductionTracking_Week(request);
            var dataP604 = await _ptService.GetP604ProductionTracking_Week(request);
            var dataP605 = await _ptService.GetP605ProductionTracking_Week(request);
            var dataP5m = await _ptService.GetP5mProductionTracking_Week(request);

            var result = new PtPrintingWeekVM()
            {
                P601PTWeek = dataP601,
                P604PTWeek = dataP604,
                P605PTWeek = dataP605,
                P5mPTWeek = dataP5m,
            };
            return Ok(result);
        }

        [HttpGet("PtPrintingMonth")]
        public async Task<IActionResult> GetPtPrintingMonth([FromQuery] TimeRequest request)
        {
            var dataP601 = await _ptService.GetP601ProductionTracking_Month(request);
            var dataP604 = await _ptService.GetP604ProductionTracking_Month(request);
            var dataP605 = await _ptService.GetP605ProductionTracking_Month(request);
            var dataP5m = await _ptService.GetP5mProductionTracking_Month(request);

            var result = new PtPrintingMonthYearVM()
            {
                P601PTMonthYear = dataP601,
                P604PTMonthYear = dataP604,
                P605PTMonthYear = dataP605,
                P5mPTMonthYear = dataP5m,
            };
            return Ok(result);
        }

        [HttpGet("PtPrintingYear")]
        public async Task<IActionResult> GetPtPrintingYear([FromQuery] TimeRequest request)
        {
            var dataP601 = await _ptService.GetP601ProductionTracking_Year(request);
            var dataP604 = await _ptService.GetP604ProductionTracking_Year(request);
            var dataP605 = await _ptService.GetP605ProductionTracking_Year(request);
            var dataP5m = await _ptService.GetP5mProductionTracking_Year(request);

            var result = new PtPrintingMonthYearVM()
            {
                P601PTMonthYear = dataP601,
                P604PTMonthYear = dataP604,
                P605PTMonthYear = dataP605,
                P5mPTMonthYear = dataP5m,
            };
            return Ok(result);
        }


        [HttpGet("PtDieCut1H")]
        public async Task<IActionResult> GetPtDieCut1H([FromQuery] TimeRequest request)
        {
            var dataBtd2 = await _ptService.GetBtd2ProductionTracking_1H(request);
            var dataBtd3 = await _ptService.GetBtd3ProductionTracking_1H(request);
            var dataBtd4 = await _ptService.GetBtd4ProductionTracking_1H(request);
            var dataBtd5 = await _ptService.GetBtd5ProductionTracking_1H(request);
            var result = new PtDieCut1HVM()
            {
                Btd2PT1H = dataBtd2,
                Btd3PT1H = dataBtd3,
                Btd4PT1H = dataBtd4,
                Btd5PT1H = dataBtd5,
            };
            return Ok(result);
        }

        [HttpGet("PtDieCutDay")]
        public async Task<IActionResult> GetPtDieCutDay([FromQuery] TimeRequest request)
        {
            var dataBtd2 = await _ptService.GetBtd2ProductionTracking_Day(request);
            var dataBtd3 = await _ptService.GetBtd3ProductionTracking_Day(request);
            var dataBtd4 = await _ptService.GetBtd4ProductionTracking_Day(request);
            var dataBtd5 = await _ptService.GetBtd5ProductionTracking_Day(request);
            var result = new PtDieCutDayVM()
            {
                Btd2PTDay = dataBtd2,
                Btd3PTDay = dataBtd3,
                Btd4PTDay = dataBtd4,
                Btd5PTDay = dataBtd5,
            };
            return Ok(result);
        }

        [HttpGet("PtDieCutWeek")]
        public async Task<IActionResult> GetPtDieCutWeek([FromQuery] TimeRequest request)
        {
            var dataBtd2 = await _ptService.GetBtd2ProductionTracking_Week(request);
            var dataBtd3 = await _ptService.GetBtd3ProductionTracking_Week(request);
            var dataBtd4 = await _ptService.GetBtd4ProductionTracking_Week(request);
            var dataBtd5 = await _ptService.GetBtd5ProductionTracking_Week(request);

            var result = new PtDieCutWeekVM()
            {
                Btd2PTWeek = dataBtd2,
                Btd3PTWeek = dataBtd3,
                Btd4PTWeek = dataBtd4,
                Btd5PTWeek = dataBtd5,
            };
            return Ok(result);
        }

        [HttpGet("PtDieCutMonth")]
        public async Task<IActionResult> GetPtDieCutMonth([FromQuery] TimeRequest request)
        {
            var dataBtd2 = await _ptService.GetBtd2ProductionTracking_Month(request);
            var dataBtd3 = await _ptService.GetBtd3ProductionTracking_Month(request);
            var dataBtd4 = await _ptService.GetBtd4ProductionTracking_Month(request);
            var dataBtd5 = await _ptService.GetBtd5ProductionTracking_Month(request);

            var result = new PtDieCutMonthYearVM()
            {
                Btd2PTMonthYear = dataBtd2,
                Btd3PTMonthYear = dataBtd3,
                Btd4PTMonthYear = dataBtd4,
                Btd5PTMonthYear = dataBtd5,
            };
            return Ok(result);
        }

        [HttpGet("PtDieCutYear")]
        public async Task<IActionResult> GetPtDieCutYear([FromQuery] TimeRequest request)
        {
            var dataBtd2 = await _ptService.GetBtd2ProductionTracking_Year(request);
            var dataBtd3 = await _ptService.GetBtd3ProductionTracking_Year(request);
            var dataBtd4 = await _ptService.GetBtd4ProductionTracking_Year(request);
            var dataBtd5 = await _ptService.GetBtd5ProductionTracking_Year(request);

            var result = new PtDieCutMonthYearVM()
            {
                Btd2PTMonthYear = dataBtd2,
                Btd3PTMonthYear = dataBtd3,
                Btd4PTMonthYear = dataBtd4,
                Btd5PTMonthYear = dataBtd5,
            };
            return Ok(result);
        }

        [HttpGet("PtGluing1H")]
        public async Task<IActionResult> GetPtGluing1H([FromQuery] TimeRequest request)
        {
            var dataD650 = await _ptService.GetD650ProductionTracking_1H(request);
            var dataD750 = await _ptService.GetD750ProductionTracking_1H(request);
            var dataD1000 = await _ptService.GetD1000ProductionTracking_1H(request);
            var dataD1100 = await _ptService.GetD1100ProductionTracking_1H(request);
            var result = new PtGluing1HVM()
            {
                D650PT1H = dataD650,
                D750PT1H = dataD750,
                D1000PT1H = dataD1000,
                D1100PT1H = dataD1100,
            };
            return Ok(result);
        }

        [HttpGet("PtGluingDay")]
        public async Task<IActionResult> GetPtGluingDay([FromQuery] TimeRequest request)
        {
            var dataD650 = await _ptService.GetD650ProductionTracking_Day(request);
            var dataD750 = await _ptService.GetD750ProductionTracking_Day(request);
            var dataD1000 = await _ptService.GetD1000ProductionTracking_Day(request);
            var dataD1100 = await _ptService.GetD1100ProductionTracking_Day(request);
            var result = new PtGluingDayVM()
            {
                D650PTDay = dataD650,
                D750PTDay = dataD750,
                D1000PTDay = dataD1000,
                D1100PTDay = dataD1100,
            };
            return Ok(result);
        }

        [HttpGet("PtGluingWeek")]
        public async Task<IActionResult> GetPtGluingWeek([FromQuery] TimeRequest request)
        {
            var dataD650 = await _ptService.GetD650ProductionTracking_Week(request);
            var dataD750 = await _ptService.GetD750ProductionTracking_Week(request);
            var dataD1000 = await _ptService.GetD1000ProductionTracking_Week(request);
            var dataD1100 = await _ptService.GetD1100ProductionTracking_Week(request);

            var result = new PtGluingWeekVM()
            {
                D650PTWeek = dataD650,
                D750PTWeek = dataD750,
                D1000PTWeek = dataD1000,
                D1100PTWeek = dataD1100,
            };
            return Ok(result);
        }

        [HttpGet("PtGluingMonth")]
        public async Task<IActionResult> GetPtGluingMonth([FromQuery] TimeRequest request)
        {
            var dataD650 = await _ptService.GetD650ProductionTracking_Month(request);
            var dataD750 = await _ptService.GetD750ProductionTracking_Month(request);
            var dataD1000 = await _ptService.GetD1000ProductionTracking_Month(request);
            var dataD1100 = await _ptService.GetD1100ProductionTracking_Month(request);

            var result = new PtGluingMonthYearVM()
            {
                D650PTMonthYear = dataD650,
                D750PTMonthYear = dataD750,
                D1000PTMonthYear = dataD1000,
                D1100PTMonthYear = dataD1100,
            };
            return Ok(result);
        }

        [HttpGet("PtGluingYear")]
        public async Task<IActionResult> GetPtGluingYear([FromQuery] TimeRequest request)
        {
            var dataD650 = await _ptService.GetD650ProductionTracking_Year(request);
            var dataD750 = await _ptService.GetD750ProductionTracking_Year(request);
            var dataD1000 = await _ptService.GetD1000ProductionTracking_Year(request);
            var dataD1100 = await _ptService.GetD1100ProductionTracking_Year(request);

            var result = new PtGluingMonthYearVM()
            {
                D650PTMonthYear = dataD650,
                D750PTMonthYear = dataD750,
                D1000PTMonthYear = dataD1000,
                D1100PTMonthYear = dataD1100,
            };
            return Ok(result);
        }
    }
}
