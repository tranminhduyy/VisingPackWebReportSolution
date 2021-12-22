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
    public class WtSclGmcController : BaseController
    {
        private readonly IWorkTimeManageApiClient _wtApiClient;
        private readonly IConfiguration _configuration;
        public WtSclGmcController(IWorkTimeManageApiClient wtApiClient,
            IConfiguration configuration)
        {
            _configuration = configuration;
            _wtApiClient = wtApiClient;
        }

        public async Task<IActionResult> GetData(string Selected, DateTime from, DateTime to)
        {
            var result = new WtSclGmcVM();
            if (Selected == null)
            {
                var initialRequest = new TimeRequest()
                {
                    From = DateTime.Now.AddDays(-1),
                    To = DateTime.Now,
                };
                result = await _wtApiClient.GetWtSclGmc(initialRequest);
                SendViewBagValue(result, initialRequest);
                return View("~/Views/WorkTimeManage/SclGmc.cshtml", result);
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
            result = await _wtApiClient.GetWtSclGmc(timeRequest);
            SendViewBagValue(result, timeRequest);

            return View("~/Views/WorkTimeManage/SclGmc.cshtml", result);
        }

        public DateTime StartOfWeek(DateTime dt, DayOfWeek startOfWeek)
        {
            int diff = (7 + (dt.DayOfWeek - startOfWeek)) % 7;
            return dt.AddDays(-1 * diff).Date;
        }

        void SendViewBagValue(WtSclGmcVM vm, TimeRequest tr)
        {
            var sclTotalTime = vm.SCL.RunningTime + vm.SCL.TestingTime +
                                vm.SCL.OtherTime + vm.SCL.BreakTime +
                                vm.SCL.FixingTime + vm.SCL.PendingTime +
                                vm.SCL.MaintenanceTime + vm.SCL.PauseTime;
            var gmc1TotalTime = vm.GMC1.RunningTime + vm.GMC1.TestingTime +
                                vm.GMC1.OtherTime + vm.GMC1.BreakTime +
                                vm.GMC1.FixingTime + vm.GMC1.PendingTime +
                                vm.GMC1.MaintenanceTime + vm.GMC1.PauseTime;
            var gmc2TotalTime = vm.GMC2.RunningTime + vm.GMC2.TestingTime +
                                vm.GMC2.OtherTime + vm.GMC2.BreakTime +
                                vm.GMC2.FixingTime + vm.GMC2.PendingTime +
                                vm.GMC2.MaintenanceTime + vm.GMC2.PauseTime;

            ViewBag.SCLRunPercent = (sclTotalTime != 0) ? Math.Round(Convert.ToDecimal((vm.SCL.RunningTime / sclTotalTime) * 100), 1) : 0;
            ViewBag.GMC1RunPercent = (gmc1TotalTime != 0) ? Math.Round(Convert.ToDecimal((vm.GMC1.RunningTime / gmc1TotalTime) * 100), 1) : 0;
            ViewBag.GMC2RunPercent = (gmc2TotalTime != 0) ? Math.Round(Convert.ToDecimal((vm.GMC2.RunningTime / gmc2TotalTime) * 100), 1) : 0;

            ViewBag.From = tr.From;
            ViewBag.To = tr.To;
        }
    }
}
