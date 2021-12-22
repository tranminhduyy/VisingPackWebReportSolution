using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VisingPackSolution.ApiIntegration.Interfaces;
using VisingPackSolution.ViewModels.Common;
using VisingPackSolution.ViewModels.WorkTimeManage;

namespace VisingPackSolution.AdminApp.Controllers.WorkTimeManage
{
    //[Authorize(Roles = "Admin, Manager, Planner, User")]
    public class WtGluingController : BaseController
    {
        private readonly IWorkTimeManageApiClient _wtApiClient;
        private readonly IConfiguration _configuration;
        public WtGluingController(IWorkTimeManageApiClient wtApiClient,
            IConfiguration configuration)
        {
            _configuration = configuration;
            _wtApiClient = wtApiClient;
        }

        public async Task<IActionResult> GetData(string Selected, DateTime from, DateTime to)
        {
            var result = new WtGluingVM();
            if (Selected == null)
            {
                var initialRequest = new TimeRequest()
                {
                    From = DateTime.Now.AddDays(-1),
                    To = DateTime.Now,
                };
                result = await _wtApiClient.GetWtGluing(initialRequest);
                SendViewBagValue(result, initialRequest);
                return View("~/Views/WorkTimeManage/Gluing.cshtml", result);
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
            result = await _wtApiClient.GetWtGluing(timeRequest);
            SendViewBagValue(result, timeRequest);

            return View("~/Views/WorkTimeManage/Gluing.cshtml", result);
        }

        public DateTime StartOfWeek(DateTime dt, DayOfWeek startOfWeek)
        {
            int diff = (7 + (dt.DayOfWeek - startOfWeek)) % 7;
            return dt.AddDays(-1 * diff).Date;
        }

        void SendViewBagValue(WtGluingVM vm, TimeRequest tr)
        {
            var d650TotalTime = vm.D650.RunningTime + vm.D650.TestingTime +
                                vm.D650.OtherTime + vm.D650.BreakTime +
                                vm.D650.FixingTime + vm.D650.PendingTime +
                                vm.D650.MaintenanceTime + vm.D650.PauseTime;
            var d750TotalTime = vm.D750.RunningTime + vm.D750.TestingTime +
                                vm.D750.OtherTime + vm.D750.BreakTime +
                                vm.D750.FixingTime + vm.D750.PendingTime +
                                vm.D750.MaintenanceTime + vm.D750.PauseTime;
            var d1000TotalTime = vm.D1000.RunningTime + vm.D1000.TestingTime +
                                vm.D1000.OtherTime + vm.D1000.BreakTime +
                                vm.D1000.FixingTime + vm.D1000.PendingTime +
                                vm.D1000.MaintenanceTime + vm.D1000.PauseTime;
            var d1100TotalTime = vm.D1100.RunningTime + vm.D1100.TestingTime +
                                vm.D1100.OtherTime + vm.D1100.BreakTime +
                                vm.D1100.FixingTime + vm.D1100.PendingTime +
                                vm.D1100.MaintenanceTime + vm.D1100.PauseTime;

            ViewBag.D650RunPercent = (d650TotalTime != 0) ? Math.Round(Convert.ToDecimal((vm.D650.RunningTime / d650TotalTime) * 100), 1) : 0;
            ViewBag.D750RunPercent = (d750TotalTime != 0) ? Math.Round(Convert.ToDecimal((vm.D750.RunningTime / d750TotalTime) * 100), 1) : 0;
            ViewBag.D1000RunPercent = (d1000TotalTime != 0) ? Math.Round(Convert.ToDecimal((vm.D1000.RunningTime / d1000TotalTime) * 100), 1) : 0;
            ViewBag.D1100RunPercent = (d1100TotalTime != 0) ? Math.Round(Convert.ToDecimal((vm.D1100.RunningTime / d1100TotalTime) * 100), 1) : 0;

            ViewBag.From = tr.From;
            ViewBag.To = tr.To;
        }
    }
}
