using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VisingPackSolution.ApiIntegration.Interfaces;
using VisingPackSolution.ViewModels.MachineState;

namespace Test.Controllers
{
    public class ChartController : Controller
    {
        private readonly IMachineStateApiClient _msApiClient;
        private readonly IConfiguration _configuration;
        public ChartController(IMachineStateApiClient msApiClient,
            IConfiguration configuration)
        {
            _configuration = configuration;
            _msApiClient = msApiClient;
        }

        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Chart(DateTime from, DateTime to)
        {
            //if (from.ToShortDateString() == "1/1/0001" && to.ToShortDateString() == "1/1/0001")
            //{
            //    from = DateTime.Now.AddDays(-2);
            //    to = DateTime.Now;
            //}

            var request = new GetMsByTimeRequest()
            {
                From = DateTime.Now.AddDays(-36),
                To = DateTime.Now.AddDays(-34),
            };

            var data = await _msApiClient.GetByTime(request);
            ViewBag.From = from;
            ViewBag.To = to;
            if (TempData["result"] != null)
            {
                ViewBag.SuccessMsg = TempData["result"];
            }
            return View(data);
        }

        public ActionResult PointsAggregation()
        {
            return View();
        }

        [HttpGet]
        public object GetWeatherIndicators()
        {
            return SampleData.WeatherIndicators;
        }
    }
}
