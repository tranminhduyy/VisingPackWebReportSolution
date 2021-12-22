using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;
using VisingPackSolution.ApiIntegration.Interfaces;
using VisingPackSolution.ViewModels.Common;
using VisingPackSolution.ViewModels.WorkTimeManage;

namespace VisingPackSolution.AdminApp.Controllers.WorkTimeManage
{
    //[Authorize(Roles = "Admin, Manager, Planner, User")]
    public class WtPrintingController : BaseController
    {
        private readonly IWorkTimeManageApiClient _wtApiClient;
        private readonly IConfiguration _configuration;
        public WtPrintingController(IWorkTimeManageApiClient wtApiClient,
            IConfiguration configuration)
        {
            _configuration = configuration;
            _wtApiClient = wtApiClient;
        }

        public async Task<IActionResult> GetData(string Selected, DateTime from, DateTime to)
        {
            var result = new WtPrintingVM();
            if (Selected == null)
            {
                var initialRequest = new TimeRequest()
                {
                    From = DateTime.Now.AddDays(-1),
                    To = DateTime.Now,
                };
                result = await _wtApiClient.GetWtPrinting(initialRequest);
                SendViewBagValue(result, initialRequest);
                return View("~/Views/WorkTimeManage/Printing.cshtml", result);
            }

            var _from = new DateTime();
            var _to = new DateTime();

            switch (Selected)
            {
                case "Find":
                    _from = from;
                    _to = to;
                    break;
                case "Day":
                    _from = DateTime.Now.AddDays(-1);
                    _to = DateTime.Now;
                    break;
                case "Week":
                    _from = StartOfWeek(DateTime.Now, DayOfWeek.Monday);
                    _to = DateTime.Now;
                    break;
                case "Month":
                    _from = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);                   
                    _to = DateTime.Now;
                    break;
                case "3Month":
                    _from = DateTime.Now.AddMonths(-3);
                    _from = (_from.Year != DateTime.Now.Year) ? new DateTime(DateTime.Now.Year, 1, 1) : _from;
                    _to = DateTime.Now;
                    break;
                case "6Month":
                    _from = DateTime.Now.AddMonths(-6);
                    _from = (_from.Year != DateTime.Now.Year) ? new DateTime(DateTime.Now.Year, 1, 1) : _from;
                    _to = DateTime.Now;
                    break;
                case "Year":
                    _from = new DateTime(DateTime.Now.Year, 1, 1);
                    _to = DateTime.Now;
                    break;
            }
            var timeRequest = new TimeRequest()
            {
                From = _from,
                To = _to,
            };

            result = await _wtApiClient.GetWtPrinting(timeRequest);
            SendViewBagValue(result, timeRequest);

            return View("~/Views/WorkTimeManage/Printing.cshtml", result);
        }

        public DateTime StartOfWeek(DateTime dt, DayOfWeek startOfWeek)
        {
            int diff = (7 + (dt.DayOfWeek - startOfWeek)) % 7;
            return dt.AddDays(-1 * diff).Date;
        }

        void SendViewBagValue(WtPrintingVM vm, TimeRequest tr)
        {
            var p601TotalTime = vm.P601.RunningTime + vm.P601.TestingTime +
                                vm.P601.OtherTime + vm.P601.BreakTime +
                                vm.P601.FixingTime + vm.P601.PendingTime +
                                vm.P601.MaintenanceTime + vm.P601.PauseTime;
            var p604TotalTime = vm.P604.RunningTime + vm.P604.TestingTime +
                                vm.P604.OtherTime + vm.P604.BreakTime +
                                vm.P604.FixingTime + vm.P604.PendingTime +
                                vm.P604.MaintenanceTime + vm.P604.PauseTime;
            var p605TotalTime = vm.P605.RunningTime + vm.P605.TestingTime +
                                vm.P605.OtherTime + vm.P605.BreakTime +
                                vm.P605.FixingTime + vm.P605.PendingTime +
                                vm.P605.MaintenanceTime + vm.P605.PauseTime;
            var p5MTotalTime  = vm.P5M.RunningTime + vm.P5M.TestingTime +
                                vm.P5M.OtherTime + vm.P5M.BreakTime +
                                vm.P5M.FixingTime + vm.P5M.PendingTime +
                                vm.P5M.MaintenanceTime + vm.P5M.PauseTime;

            ViewBag.P601RunPercent = (p601TotalTime != 0) ? Math.Round(Convert.ToDecimal((vm.P601.RunningTime / p601TotalTime) * 100), 1) : 0;
            ViewBag.P604RunPercent = (p604TotalTime != 0) ? Math.Round(Convert.ToDecimal((vm.P604.RunningTime / p604TotalTime) * 100), 1) : 0;
            ViewBag.P605RunPercent = (p605TotalTime != 0) ? Math.Round(Convert.ToDecimal((vm.P605.RunningTime / p605TotalTime) * 100), 1) : 0;
            ViewBag.P5MRunPercent = (p5MTotalTime != 0) ? Math.Round(Convert.ToDecimal((vm.P5M.RunningTime / p5MTotalTime) * 100), 1) : 0;

            ViewBag.From = tr.From;
            ViewBag.To = tr.To;
        }
    }
}
