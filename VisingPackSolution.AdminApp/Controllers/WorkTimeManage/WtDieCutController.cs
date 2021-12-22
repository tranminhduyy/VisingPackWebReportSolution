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
    public class WtDieCutController : BaseController
    {
        private readonly IWorkTimeManageApiClient _wtApiClient;
        private readonly IConfiguration _configuration;
        public WtDieCutController(IWorkTimeManageApiClient wtApiClient,
            IConfiguration configuration)
        {
            _configuration = configuration;
            _wtApiClient = wtApiClient;
        }

        public async Task<IActionResult> GetData(string Selected, DateTime from, DateTime to)
        {
            var result = new WtDieCutVM();
            if (Selected == null)
            {
                var initialRequest = new TimeRequest()
                {
                    From = DateTime.Now.AddDays(-1),
                    To = DateTime.Now,
                };
                result = await _wtApiClient.GetWtDieCut(initialRequest);
                SendViewBagValue(result, initialRequest);
                return View("~/Views/WorkTimeManage/DieCut.cshtml", result);
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

            result = await _wtApiClient.GetWtDieCut(timeRequest);
            SendViewBagValue(result, timeRequest);

            return View("~/Views/WorkTimeManage/DieCut.cshtml", result);
        }

        public DateTime StartOfWeek(DateTime dt, DayOfWeek startOfWeek)
        {
            int diff = (7 + (dt.DayOfWeek - startOfWeek)) % 7;
            return dt.AddDays(-1 * diff).Date;
        }

        void SendViewBagValue(WtDieCutVM vm, TimeRequest tr)
        {
            var btd2TotalTime = vm.BTD2.RunningTime + vm.BTD2.TestingTime +
                                vm.BTD2.OtherTime + vm.BTD2.BreakTime +
                                vm.BTD2.FixingTime + vm.BTD2.PendingTime +
                                vm.BTD2.MaintenanceTime + vm.BTD2.PauseTime;
            var btd3TotalTime = vm.BTD3.RunningTime + vm.BTD3.TestingTime +
                                vm.BTD3.OtherTime + vm.BTD3.BreakTime +
                                vm.BTD3.FixingTime + vm.BTD3.PendingTime +
                                vm.BTD3.MaintenanceTime + vm.BTD3.PauseTime;
            var btd4TotalTime = vm.BTD4.RunningTime + vm.BTD4.TestingTime +
                                vm.BTD4.OtherTime + vm.BTD4.BreakTime +
                                vm.BTD4.FixingTime + vm.BTD4.PendingTime +
                                vm.BTD4.MaintenanceTime + vm.BTD4.PauseTime;
            var btd5TotalTime = vm.BTD5.RunningTime + vm.BTD5.TestingTime +
                                vm.BTD5.OtherTime + vm.BTD5.BreakTime +
                                vm.BTD5.FixingTime + vm.BTD5.PendingTime +
                                vm.BTD5.MaintenanceTime + vm.BTD5.PauseTime;

            ViewBag.BTD2RunPercent = (btd2TotalTime != 0) ? Math.Round(Convert.ToDecimal((vm.BTD2.RunningTime / btd2TotalTime) * 100), 1) : 0;
            ViewBag.BTD3RunPercent = (btd3TotalTime != 0) ? Math.Round(Convert.ToDecimal((vm.BTD3.RunningTime / btd3TotalTime) * 100), 1) : 0;
            ViewBag.BTD4RunPercent = (btd4TotalTime != 0) ? Math.Round(Convert.ToDecimal((vm.BTD4.RunningTime / btd4TotalTime) * 100), 1) : 0;
            ViewBag.BTD5RunPercent = (btd5TotalTime != 0) ? Math.Round(Convert.ToDecimal((vm.BTD5.RunningTime / btd5TotalTime) * 100), 1) : 0;

            ViewBag.From = tr.From;
            ViewBag.To = tr.To;
        }
    }
}
