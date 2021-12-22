using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VisingPackSolution.ApiIntegration.Interfaces;
using VisingPackSolution.ViewModels.MachineState;

namespace VisingPackSolution.AdminApp.Controllers
{
    //[Authorize(Roles = "Admin, Manager, Planner, User")]
    public class MsPrintingController : BaseController
    {
        private readonly IMachineStateApiClient _msApiClient;
        private readonly IConfiguration _configuration;
        public MsPrintingController(IMachineStateApiClient msApiClient,
            IConfiguration configuration)
        {
            _configuration = configuration;
            _msApiClient = msApiClient;
        }

        public async Task<IActionResult> Index(DateTime from, DateTime to)
        {
            if (from.ToShortDateString() == "1/1/0001" && to.ToShortDateString() == "1/1/0001")
            {
                from = DateTime.Now.AddDays(-1);
                to = DateTime.Now;
            }
                
            var request = new GetMsByTimeRequest()
            {
                Selected = "Day",
                From = from,
                To = to,
            };

            var data = await _msApiClient.GetMsPrintingByTime(request);

            ViewBag.P601SpeedDataPoints = JsonConvert.SerializeObject(data.P601Speeds, _jsonSetting);
            ViewBag.P604SpeedDataPoints = JsonConvert.SerializeObject(data.P604Speeds, _jsonSetting);
            ViewBag.P605SpeedDataPoints = JsonConvert.SerializeObject(data.P605Speeds, _jsonSetting);
            ViewBag.P5MSpeedDataPoints = JsonConvert.SerializeObject(data.P5MSpeeds, _jsonSetting);

            ViewBag.From = from;
            ViewBag.To = to;
            if (TempData["result"] != null)
            {
                ViewBag.SuccessMsg = TempData["result"];
            }
            return View("~/Views/MachineState/Printing.cshtml", data);
        }

        public async Task<IActionResult> GetDataLastHour()
        {
            var request = new GetMsByTimeRequest()
            {
                Selected = "Hour",
                From = DateTime.Now.AddHours(-1),
                To = DateTime.Now,
            };
            var result = await GetData(request);
            return View("~/Views/MachineState/Printing.cshtml", result);
        }
        public async Task<IActionResult> GetDataDay()
        {
            var request = new GetMsByTimeRequest()
            {
                Selected = "Day",
                From = DateTime.Now.AddDays(-1),
                To = DateTime.Now,
            };
            var result = await GetData(request);
            return View("~/Views/MachineState/Printing.cshtml", result);
        }
        public async Task<IActionResult> GetDataWeek()
        {
            var request = new GetMsByTimeRequest()
            {
                Selected = "Week",
                From = StartOfWeek(DateTime.Now ,DayOfWeek.Monday),
                To = DateTime.Now,
            };
            var result = await GetData(request);
            return View("~/Views/MachineState/Printing.cshtml", result);
        }
        public async Task<IActionResult> GetDataMonth()
        {
            var request = new GetMsByTimeRequest()
            {
                Selected = "Month",
                From = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1),
                To = DateTime.Now,
            };
            var result = await GetData(request);
            return View("~/Views/MachineState/Printing.cshtml", result);
        }
        public async Task<IActionResult> GetDataYear()
        {
            var request = new GetMsByTimeRequest()
            {
                Selected = "Year",
                From = new DateTime(DateTime.Now.Year, 1, 1),
                To = DateTime.Now,
            };
            var result = await GetData(request);
            //return View("Index", result);
            return View("~/Views/MachineState/Printing.cshtml", result);
        }

        //when test ajax
        //public async Task<ActionResult> GetDataYear()
        //{
        //    var request = new GetMsByTimeRequest()
        //    {
        //        Selected = "Year",
        //        From = new DateTime(DateTime.Now.Year, 1, 1),
        //        To = DateTime.Now,
        //    };
        //    var result = await  GetData(request);
        //    return PartialView("_MsPrinting", result);
        //}

        private async Task<MsPrintingVM> GetData(GetMsByTimeRequest request)
        {
            var data = await _msApiClient.GetMsPrintingByTime(request);

            ViewBag.P601SpeedDataPoints = JsonConvert.SerializeObject(data.P601Speeds, _jsonSetting);
            ViewBag.P604SpeedDataPoints = JsonConvert.SerializeObject(data.P604Speeds, _jsonSetting);
            ViewBag.P605SpeedDataPoints = JsonConvert.SerializeObject(data.P605Speeds, _jsonSetting);
            ViewBag.P5MSpeedDataPoints = JsonConvert.SerializeObject(data.P5MSpeeds, _jsonSetting);

            ViewBag.From = request.From;
            ViewBag.To = request.To;
            //if (TempData["result"] != null)
            //{
            //    ViewBag.SuccessMsg = TempData["result"];
            //}
            return data;
        }

        JsonSerializerSettings _jsonSetting = new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore };
        public DateTime StartOfWeek(DateTime dt, DayOfWeek startOfWeek)
        {
            int diff = (7 + (dt.DayOfWeek - startOfWeek)) % 7;
            return dt.AddDays(-1 * diff).Date;
        }
    }
}
