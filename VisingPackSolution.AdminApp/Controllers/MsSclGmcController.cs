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
    public class MsSclGmcController : BaseController
    {
        private readonly IMachineStateApiClient _msApiClient;
        private readonly IConfiguration _configuration;
        public MsSclGmcController(IMachineStateApiClient msApiClient,
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

            var data = await _msApiClient.GetMsSclGmcBytime(request);

            ViewBag.SclSpeedDataPoints = JsonConvert.SerializeObject(data.SclSpeeds, _jsonSetting);
            ViewBag.Gmc1SpeedDataPoints = JsonConvert.SerializeObject(data.Gmc1Speeds, _jsonSetting);
            ViewBag.Gmc2SpeedDataPoints = JsonConvert.SerializeObject(data.Gmc2Speeds, _jsonSetting);

            ViewBag.From = from;
            ViewBag.To = to;
            if (TempData["result"] != null)
            {
                ViewBag.SuccessMsg = TempData["result"];
            }
            return View("~/Views/MachineState/SclGmc.cshtml", data);
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
            return View("~/Views/MachineState/SclGmc.cshtml", result);
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
            return View("~/Views/MachineState/SclGmc.cshtml", result);
        }
        public async Task<IActionResult> GetDataWeek()
        {
            var request = new GetMsByTimeRequest()
            {
                Selected = "Week",
                From = StartOfWeek(DateTime.Now, DayOfWeek.Monday),
                To = DateTime.Now,
            };
            var result = await GetData(request);
            return View("~/Views/MachineState/SclGmc.cshtml", result);
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
            return View("~/Views/MachineState/SclGmc.cshtml", result);
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
            return View("~/Views/MachineState/SclGmc.cshtml", result);
        }

        private async Task<MsSclGmcVM> GetData(GetMsByTimeRequest request)
        {
            var data = await _msApiClient.GetMsSclGmcBytime(request);

            ViewBag.SclSpeedDataPoints = JsonConvert.SerializeObject(data.SclSpeeds, _jsonSetting);
            ViewBag.Gmc1SpeedDataPoints = JsonConvert.SerializeObject(data.Gmc1Speeds, _jsonSetting);
            ViewBag.Gmc2SpeedDataPoints = JsonConvert.SerializeObject(data.Gmc2Speeds, _jsonSetting);

            ViewBag.From = request.From;
            ViewBag.To = request.To;
            if (TempData["result"] != null)
            {
                ViewBag.SuccessMsg = TempData["result"];
            }
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
