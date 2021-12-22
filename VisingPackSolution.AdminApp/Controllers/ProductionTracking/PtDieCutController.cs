using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using VisingPackSolution.ApiIntegration.Interfaces;
using VisingPackSolution.ViewModels.Common;
using VisingPackSolution.ViewModels.ProductionTracking;

namespace VisingPackSolution.AdminApp.Controllers.ProductionTracking
{
    public class PtDieCutController : BaseController
    {
        private readonly IProductionTrackingApiClient _ptApiClient;
        private readonly IConfiguration _configuration;
        private static bool monthOryear { get; set; }
        private static DateTime sDate { get; set; }
        private static TimeRequest tr { get; set; }
        public PtDieCutController(IProductionTrackingApiClient ptApiClient,
            IConfiguration configuration)
        {
            _configuration = configuration;
            _ptApiClient = ptApiClient;
        }

        public async Task<IActionResult> GetData1H(DateTime selectedDate)
        {
            var timeRequest = new TimeRequest();
            if (selectedDate.ToShortDateString() == "1/1/0001")
            {
                timeRequest.To = DateTime.Now;
                timeRequest.From = DateTime.Now.AddHours(-1);
                sDate = DateTime.Now;
            }
            else
            {
                timeRequest.To = selectedDate;
                timeRequest.From = selectedDate.AddHours(-1);
                sDate = selectedDate;
            }

            tr = timeRequest;
            ViewBag.SelectedDate = timeRequest.To;
            var result = new PtDieCut1HVM();
            result = await _ptApiClient.GetPtDieCut1H(timeRequest);

            return View("~/Views/ProductionTracking/DieCut1H.cshtml", result);
        }
        public async Task<IActionResult> GetDataDay(DateTime selectedDate)
        {
            var timeRequest = new TimeRequest();
            if (selectedDate.ToShortDateString() == "1/1/0001")
            {
                timeRequest.To = DateTime.Now;
                timeRequest.From = DateTime.Now.AddDays(-1);
                sDate = DateTime.Now;
            }
            else
            {
                timeRequest.To = selectedDate;
                timeRequest.From = selectedDate.AddDays(-1);
                sDate = selectedDate;
            }

            tr = timeRequest;
            ViewBag.SelectedDate = timeRequest.To;
            var result = new PtDieCutDayVM();
            result = await _ptApiClient.GetPtDieCutDay(timeRequest);

            return View("~/Views/ProductionTracking/DieCutDay.cshtml", result);
        }
        public async Task<IActionResult> GetDataWeek(DateTime selectedDate)
        {
            var timeRequest = new TimeRequest();
            if (selectedDate.ToShortDateString() == "1/1/0001")
            {
                timeRequest.To = StartOfWeek(DateTime.Now, DayOfWeek.Monday).AddDays(7);
                timeRequest.From = StartOfWeek(DateTime.Now, DayOfWeek.Monday);
                sDate = DateTime.Now;
            }
            else
            {
                timeRequest.To = StartOfWeek(selectedDate, DayOfWeek.Monday).AddDays(7);
                timeRequest.From = StartOfWeek(selectedDate, DayOfWeek.Monday);
                sDate = selectedDate;
            }

            tr = timeRequest;
            ViewBag.SelectedDate = (selectedDate.ToShortDateString() == "1/1/0001") ? DateTime.Now : selectedDate;
            var result = new PtDieCutWeekVM();
            result = await _ptApiClient.GetPtDieCutWeek(timeRequest);

            return View("~/Views/ProductionTracking/DieCutWeek.cshtml", result);
        }
        public async Task<IActionResult> GetDataMonth(DateTime selectedDate)
        {
            monthOryear = false;

            var timeRequest = new TimeRequest();
            if (selectedDate.ToShortDateString() == "1/1/0001")
            {
                timeRequest.To = new DateTime(DateTime.Now.Year, DateTime.Now.AddMonths(1).Month, 1);
                timeRequest.From = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                sDate = DateTime.Now;
            }
            else
            {
                timeRequest.To = new DateTime(selectedDate.Year, selectedDate.AddMonths(1).Month, 1);
                timeRequest.From = new DateTime(selectedDate.Year, selectedDate.Month, 1);
                sDate = selectedDate;
            }

            tr = timeRequest;
            ViewBag.SelectedDate = (selectedDate.ToShortDateString() == "1/1/0001") ? DateTime.Now : selectedDate;

            var result = new PtDieCutMonthYearVM();
            result = await _ptApiClient.GetPtDieCutMonth(timeRequest);

            var btd2_avaibility_uptime = (result.Btd2PTMonthYear.ActualWorkingTime == 0) ? 0 : Math.Round((decimal)((result.Btd2PTMonthYear.ActualWorkingTime - result.Btd2PTMonthYear.Downtime) * 100 / result.Btd2PTMonthYear.ActualWorkingTime), 1);
            var btd2_capacityUtilization = (result.Btd2PTMonthYear.MaximumProductionCapacity == 0) ? 0 : Math.Round((decimal)(result.Btd2PTMonthYear.TotalOutput * 100 / result.Btd2PTMonthYear.MaximumProductionCapacity), 1);
            var btd2_productionYield = (result.Btd2PTMonthYear.TotalOutput == 0) ? 0 : Math.Round((decimal)((result.Btd2PTMonthYear.TotalOutput - result.Btd2PTMonthYear.Waste) * 100 / result.Btd2PTMonthYear.TotalOutput), 1);
            var btd2_OEE = Math.Round(((btd2_avaibility_uptime / 100) * (btd2_capacityUtilization / 100) * (btd2_productionYield / 100) * 100), 1);

            ViewBag.btd2_AvaiUptime = btd2_avaibility_uptime.ToString() + "%";
            ViewBag.btd2_CapaUtilication = btd2_capacityUtilization.ToString() + "%";
            ViewBag.btd2_ProYield = btd2_productionYield.ToString() + "%";
            ViewBag.btd2_OEE = btd2_OEE.ToString() + "%";

            var btd3_avaibility_uptime = (result.Btd3PTMonthYear.ActualWorkingTime == 0) ? 0 : Math.Round((decimal)((result.Btd3PTMonthYear.ActualWorkingTime - result.Btd3PTMonthYear.Downtime) * 100 / result.Btd3PTMonthYear.ActualWorkingTime), 1);
            var btd3_capacityUtilization = (result.Btd3PTMonthYear.MaximumProductionCapacity == 0) ? 0 : Math.Round((decimal)(result.Btd3PTMonthYear.TotalOutput * 100 / result.Btd3PTMonthYear.MaximumProductionCapacity), 1);
            var btd3_productionYield = (result.Btd3PTMonthYear.TotalOutput == 0) ? 0 : Math.Round((decimal)((result.Btd3PTMonthYear.TotalOutput - result.Btd3PTMonthYear.Waste) * 100 / result.Btd3PTMonthYear.TotalOutput), 1);
            var btd3_OEE = Math.Round(((btd3_avaibility_uptime / 100) * (btd3_capacityUtilization / 100) * (btd3_productionYield / 100) * 100), 1);

            ViewBag.btd3_AvaiUptime = btd3_avaibility_uptime.ToString() + "%";
            ViewBag.btd3_CapaUtilication = btd3_capacityUtilization.ToString() + "%";
            ViewBag.btd3_ProYield = btd3_productionYield.ToString() + "%";
            ViewBag.btd3_OEE = btd3_OEE.ToString() + "%";

            var btd4_avaibility_uptime = (result.Btd4PTMonthYear.ActualWorkingTime == 0) ? 0 : Math.Round((decimal)((result.Btd4PTMonthYear.ActualWorkingTime - result.Btd4PTMonthYear.Downtime) * 100 / result.Btd4PTMonthYear.ActualWorkingTime), 1);
            var btd4_capacityUtilization = (result.Btd4PTMonthYear.MaximumProductionCapacity == 0) ? 0 : Math.Round((decimal)(result.Btd4PTMonthYear.TotalOutput * 100 / result.Btd4PTMonthYear.MaximumProductionCapacity), 1);
            var btd4_productionYield = (result.Btd4PTMonthYear.TotalOutput == 0) ? 0 : Math.Round((decimal)((result.Btd4PTMonthYear.TotalOutput - result.Btd4PTMonthYear.Waste) * 100 / result.Btd4PTMonthYear.TotalOutput), 1);
            var btd4_OEE = Math.Round(((btd4_avaibility_uptime / 100) * (btd4_capacityUtilization / 100) * (btd4_productionYield / 100) * 100), 1);

            ViewBag.btd4_AvaiUptime = btd4_avaibility_uptime.ToString() + "%";
            ViewBag.btd4_CapaUtilication = btd4_capacityUtilization.ToString() + "%";
            ViewBag.btd4_ProYield = btd4_productionYield.ToString() + "%";
            ViewBag.btd4_OEE = btd4_OEE.ToString() + "%";

            var btd5_avaibility_uptime = (result.Btd5PTMonthYear.ActualWorkingTime == 0) ? 0 : Math.Round((decimal)((result.Btd5PTMonthYear.ActualWorkingTime - result.Btd5PTMonthYear.Downtime) * 100 / result.Btd5PTMonthYear.ActualWorkingTime), 1);
            var btd5_capacityUtilization = (result.Btd5PTMonthYear.MaximumProductionCapacity == 0) ? 0 : Math.Round((decimal)(result.Btd5PTMonthYear.TotalOutput * 100 / result.Btd5PTMonthYear.MaximumProductionCapacity), 1);
            var btd5_productionYield = (result.Btd5PTMonthYear.TotalOutput == 0) ? 0 : Math.Round((decimal)((result.Btd5PTMonthYear.TotalOutput - result.Btd5PTMonthYear.Waste) * 100 / result.Btd5PTMonthYear.TotalOutput), 1);
            var btd5_OEE = Math.Round(((btd5_avaibility_uptime / 100) * (btd5_capacityUtilization / 100) * (btd5_productionYield / 100) * 100), 1);

            ViewBag.btd5_AvaiUptime = btd5_avaibility_uptime.ToString() + "%";
            ViewBag.btd5_CapaUtilication = btd5_capacityUtilization.ToString() + "%";
            ViewBag.btd5_ProYield = btd5_productionYield.ToString() + "%";
            ViewBag.btd5_OEE = btd5_OEE.ToString() + "%";

            return View("~/Views/ProductionTracking/DieCutMonth.cshtml", result);
        }
        public async Task<IActionResult> GetDataYear(DateTime selectedDate)
        {
            monthOryear = true;

            var timeRequest = new TimeRequest();
            if (selectedDate.ToShortDateString() == "1/1/0001")
            {
                timeRequest.To = new DateTime(DateTime.Now.AddYears(1).Year, 1, 1);
                timeRequest.From = new DateTime(DateTime.Now.Year, 1, 1);
                sDate = DateTime.Now;
            }
            else
            {
                timeRequest.To = new DateTime(selectedDate.AddYears(1).Year, 1, 1);
                timeRequest.From = new DateTime(selectedDate.Year, 1, 1);
                sDate = selectedDate;
            }

            tr = timeRequest;
            ViewBag.SelectedDate = (selectedDate.ToShortDateString() == "1/1/0001") ? DateTime.Now : selectedDate;

            var result = new PtDieCutMonthYearVM();
            result = await _ptApiClient.GetPtDieCutYear(timeRequest);

            var btd2_avaibility_uptime = (result.Btd2PTMonthYear.ActualWorkingTime == 0) ? 0 : Math.Round((decimal)((result.Btd2PTMonthYear.ActualWorkingTime - result.Btd2PTMonthYear.Downtime) * 100 / result.Btd2PTMonthYear.ActualWorkingTime), 1);
            var btd2_capacityUtilization = (result.Btd2PTMonthYear.MaximumProductionCapacity == 0) ? 0 : Math.Round((decimal)(result.Btd2PTMonthYear.TotalOutput * 100 / result.Btd2PTMonthYear.MaximumProductionCapacity), 1);
            var btd2_productionYield = (result.Btd2PTMonthYear.TotalOutput == 0) ? 0 : Math.Round((decimal)((result.Btd2PTMonthYear.TotalOutput - result.Btd2PTMonthYear.Waste) * 100 / result.Btd2PTMonthYear.TotalOutput), 1);
            var btd2_OEE = Math.Round(((btd2_avaibility_uptime / 100) * (btd2_capacityUtilization / 100) * (btd2_productionYield / 100) * 100), 1);

            ViewBag.btd2_AvaiUptime = btd2_avaibility_uptime.ToString() + "%";
            ViewBag.btd2_CapaUtilication = btd2_capacityUtilization.ToString() + "%";
            ViewBag.btd2_ProYield = btd2_productionYield.ToString() + "%";
            ViewBag.btd2_OEE = btd2_OEE.ToString() + "%";

            var btd3_avaibility_uptime = (result.Btd3PTMonthYear.ActualWorkingTime == 0) ? 0 : Math.Round((decimal)((result.Btd3PTMonthYear.ActualWorkingTime - result.Btd3PTMonthYear.Downtime) * 100 / result.Btd3PTMonthYear.ActualWorkingTime), 1);
            var btd3_capacityUtilization = (result.Btd3PTMonthYear.MaximumProductionCapacity == 0) ? 0 : Math.Round((decimal)(result.Btd3PTMonthYear.TotalOutput * 100 / result.Btd3PTMonthYear.MaximumProductionCapacity), 1);
            var btd3_productionYield = (result.Btd3PTMonthYear.TotalOutput == 0) ? 0 : Math.Round((decimal)((result.Btd3PTMonthYear.TotalOutput - result.Btd3PTMonthYear.Waste) * 100 / result.Btd3PTMonthYear.TotalOutput), 1);
            var btd3_OEE = Math.Round(((btd3_avaibility_uptime / 100) * (btd3_capacityUtilization / 100) * (btd3_productionYield / 100) * 100), 1);

            ViewBag.btd3_AvaiUptime = btd3_avaibility_uptime.ToString() + "%";
            ViewBag.btd3_CapaUtilication = btd3_capacityUtilization.ToString() + "%";
            ViewBag.btd3_ProYield = btd3_productionYield.ToString() + "%";
            ViewBag.btd3_OEE = btd3_OEE.ToString() + "%";

            var btd4_avaibility_uptime = (result.Btd4PTMonthYear.ActualWorkingTime == 0) ? 0 : Math.Round((decimal)((result.Btd4PTMonthYear.ActualWorkingTime - result.Btd4PTMonthYear.Downtime) * 100 / result.Btd4PTMonthYear.ActualWorkingTime), 1);
            var btd4_capacityUtilization = (result.Btd4PTMonthYear.MaximumProductionCapacity == 0) ? 0 : Math.Round((decimal)(result.Btd4PTMonthYear.TotalOutput * 100 / result.Btd4PTMonthYear.MaximumProductionCapacity), 1);
            var btd4_productionYield = (result.Btd4PTMonthYear.TotalOutput == 0) ? 0 : Math.Round((decimal)((result.Btd4PTMonthYear.TotalOutput - result.Btd4PTMonthYear.Waste) * 100 / result.Btd4PTMonthYear.TotalOutput), 1);
            var btd4_OEE = Math.Round(((btd4_avaibility_uptime / 100) * (btd4_capacityUtilization / 100) * (btd4_productionYield / 100) * 100), 1);

            ViewBag.btd4_AvaiUptime = btd4_avaibility_uptime.ToString() + "%";
            ViewBag.btd4_CapaUtilication = btd4_capacityUtilization.ToString() + "%";
            ViewBag.btd4_ProYield = btd4_productionYield.ToString() + "%";
            ViewBag.btd4_OEE = btd4_OEE.ToString() + "%";

            var btd5_avaibility_uptime = (result.Btd5PTMonthYear.ActualWorkingTime == 0) ? 0 : Math.Round((decimal)((result.Btd5PTMonthYear.ActualWorkingTime - result.Btd5PTMonthYear.Downtime) * 100 / result.Btd5PTMonthYear.ActualWorkingTime), 1);
            var btd5_capacityUtilization = (result.Btd5PTMonthYear.MaximumProductionCapacity == 0) ? 0 : Math.Round((decimal)(result.Btd5PTMonthYear.TotalOutput * 100 / result.Btd5PTMonthYear.MaximumProductionCapacity), 1);
            var btd5_productionYield = (result.Btd5PTMonthYear.TotalOutput == 0) ? 0 : Math.Round((decimal)((result.Btd5PTMonthYear.TotalOutput - result.Btd5PTMonthYear.Waste) * 100 / result.Btd5PTMonthYear.TotalOutput), 1);
            var btd5_OEE = Math.Round(((btd5_avaibility_uptime / 100) * (btd5_capacityUtilization / 100) * (btd5_productionYield / 100) * 100), 1);

            ViewBag.btd5_AvaiUptime = btd5_avaibility_uptime.ToString() + "%";
            ViewBag.btd5_CapaUtilication = btd5_capacityUtilization.ToString() + "%";
            ViewBag.btd5_ProYield = btd5_productionYield.ToString() + "%";
            ViewBag.btd5_OEE = btd5_OEE.ToString() + "%";

            return View("~/Views/ProductionTracking/DieCutYear.cshtml", result);
        }

        public async Task<IActionResult> ExportExcel1H()
        {
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            string fileName = sDate.ToString("yyyyMMdd_hhmmsstt") + "_BTD_Report_1H.csv";
            try
            {
                var datas = new PtDieCut1HVM();
                datas = await _ptApiClient.GetPtDieCut1H(tr);

                var workbook = new XLWorkbook();

                IXLWorksheet worksheet = workbook.Worksheets.Add("1 Hour");
                worksheet.Cell(1, 1).Value = "BTD2";
                worksheet.Cell(1, 8).Value = "BTD3";
                worksheet.Cell(1, 15).Value = "BTD4";
                worksheet.Cell(1, 22).Value = "BTD5";

                worksheet.Cell(2, 1).Value = worksheet.Cell(2, 8).Value = worksheet.Cell(2, 15).Value = worksheet.Cell(2, 22).Value = "Worksheet";
                worksheet.Cell(2, 2).Value = worksheet.Cell(2, 9).Value = worksheet.Cell(2, 16).Value = worksheet.Cell(2, 23).Value = "ProductCode";
                worksheet.Cell(2, 3).Value = worksheet.Cell(2, 10).Value = worksheet.Cell(2, 17).Value = worksheet.Cell(2, 24).Value = "Quantity";
                worksheet.Cell(2, 4).Value = worksheet.Cell(2, 11).Value = worksheet.Cell(2, 18).Value = worksheet.Cell(2, 25).Value = "Target";

                for (int index = 1; index <= datas.Btd2PT1H.Count; index++)
                {
                    worksheet.Cell(index + 2, 1).Value = datas.Btd2PT1H[index - 1].Ws;
                    worksheet.Cell(index + 2, 2).Value = datas.Btd2PT1H[index - 1].Product;
                    worksheet.Cell(index + 2, 3).Value = datas.Btd2PT1H[index - 1].Job;
                    worksheet.Cell(index + 2, 4).Value = datas.Btd2PT1H[index - 1].Target;
                }

                for (int index = 1; index <= datas.Btd3PT1H.Count; index++)
                {
                    worksheet.Cell(index + 2, 8).Value = datas.Btd3PT1H[index - 1].Ws;
                    worksheet.Cell(index + 2, 9).Value = datas.Btd3PT1H[index - 1].Product;
                    worksheet.Cell(index + 2, 10).Value = datas.Btd3PT1H[index - 1].Job;
                    worksheet.Cell(index + 2, 11).Value = datas.Btd3PT1H[index - 1].Target;
                }

                for (int index = 1; index <= datas.Btd4PT1H.Count; index++)
                {
                    worksheet.Cell(index + 2, 15).Value = datas.Btd4PT1H[index - 1].Ws;
                    worksheet.Cell(index + 2, 16).Value = datas.Btd4PT1H[index - 1].Product;
                    worksheet.Cell(index + 2, 17).Value = datas.Btd4PT1H[index - 1].Job;
                    worksheet.Cell(index + 2, 18).Value = datas.Btd4PT1H[index - 1].Target;
                }

                for (int index = 1; index <= datas.Btd5PT1H.Count; index++)
                {
                    worksheet.Cell(index + 2, 22).Value = datas.Btd5PT1H[index - 1].Ws;
                    worksheet.Cell(index + 2, 23).Value = datas.Btd5PT1H[index - 1].Product;
                    worksheet.Cell(index + 2, 24).Value = datas.Btd5PT1H[index - 1].Job;
                    worksheet.Cell(index + 2, 25).Value = datas.Btd5PT1H[index - 1].Target;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, contentType, fileName);
                }
            }
            catch
            {
                return Ok();
            }
        }
        public async Task<IActionResult> ExportExcelDay()
        {
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            string fileName = sDate.ToString("yyyyMMdd_hhmmsstt") + "_BTD_Report_Day.csv";
            try
            {
                var datas = new PtDieCutDayVM();
                datas = await _ptApiClient.GetPtDieCutDay(tr);

                var workbook = new XLWorkbook();

                IXLWorksheet worksheet = workbook.Worksheets.Add("Day");
                worksheet.Cell(1, 1).Value = "BTD2";
                worksheet.Cell(1, 11).Value = "BTD3";
                worksheet.Cell(1, 21).Value = "BTD4";
                worksheet.Cell(1, 31).Value = "BTD5";

                worksheet.Cell(2, 1).Value = worksheet.Cell(2, 11).Value = worksheet.Cell(2, 21).Value = worksheet.Cell(2, 31).Value = "Worksheet";
                worksheet.Cell(2, 2).Value = worksheet.Cell(2, 12).Value = worksheet.Cell(2, 22).Value = worksheet.Cell(2, 32).Value = "ProductCode";
                worksheet.Cell(2, 3).Value = worksheet.Cell(2, 13).Value = worksheet.Cell(2, 23).Value = worksheet.Cell(2, 33).Value = "Quantity";
                worksheet.Cell(2, 4).Value = worksheet.Cell(2, 14).Value = worksheet.Cell(2, 24).Value = worksheet.Cell(2, 34).Value = "CompleteTime";
                worksheet.Cell(2, 5).Value = worksheet.Cell(2, 15).Value = worksheet.Cell(2, 25).Value = worksheet.Cell(2, 35).Value = "PlanTime";
                worksheet.Cell(2, 6).Value = worksheet.Cell(2, 16).Value = worksheet.Cell(2, 26).Value = worksheet.Cell(2, 36).Value = "PlanEndTime";
                worksheet.Cell(2, 7).Value = worksheet.Cell(2, 17).Value = worksheet.Cell(2, 27).Value = worksheet.Cell(2, 37).Value = "Status";

                for (int index = 1; index <= datas.Btd2PTDay.Count; index++)
                {
                    worksheet.Cell(index + 2, 1).Value = datas.Btd2PTDay[index - 1].Ws;
                    worksheet.Cell(index + 2, 2).Value = datas.Btd2PTDay[index - 1].Product;
                    worksheet.Cell(index + 2, 3).Value = datas.Btd2PTDay[index - 1].Job;
                    worksheet.Cell(index + 2, 4).Value = datas.Btd2PTDay[index - 1].Endtime;
                    worksheet.Cell(index + 2, 5).Value = datas.Btd2PTDay[index - 1].Plantime;
                    worksheet.Cell(index + 2, 6).Value = datas.Btd2PTDay[index - 1].PlanEndtime;
                    worksheet.Cell(index + 2, 7).Value = datas.Btd2PTDay[index - 1].Status;
                }

                for (int index = 1; index <= datas.Btd3PTDay.Count; index++)
                {
                    worksheet.Cell(index + 2, 11).Value = datas.Btd3PTDay[index - 1].Ws;
                    worksheet.Cell(index + 2, 12).Value = datas.Btd3PTDay[index - 1].Product;
                    worksheet.Cell(index + 2, 13).Value = datas.Btd3PTDay[index - 1].Job;
                    worksheet.Cell(index + 2, 14).Value = datas.Btd3PTDay[index - 1].Endtime;
                    worksheet.Cell(index + 2, 15).Value = datas.Btd3PTDay[index - 1].Plantime;
                    worksheet.Cell(index + 2, 16).Value = datas.Btd3PTDay[index - 1].PlanEndtime;
                    worksheet.Cell(index + 2, 17).Value = datas.Btd3PTDay[index - 1].Status;
                }

                for (int index = 1; index <= datas.Btd4PTDay.Count; index++)
                {
                    worksheet.Cell(index + 2, 21).Value = datas.Btd4PTDay[index - 1].Ws;
                    worksheet.Cell(index + 2, 22).Value = datas.Btd4PTDay[index - 1].Product;
                    worksheet.Cell(index + 2, 23).Value = datas.Btd4PTDay[index - 1].Job;
                    worksheet.Cell(index + 2, 24).Value = datas.Btd4PTDay[index - 1].Endtime;
                    worksheet.Cell(index + 2, 25).Value = datas.Btd4PTDay[index - 1].Plantime;
                    worksheet.Cell(index + 2, 26).Value = datas.Btd4PTDay[index - 1].PlanEndtime;
                    worksheet.Cell(index + 2, 27).Value = datas.Btd4PTDay[index - 1].Status;
                }

                for (int index = 1; index <= datas.Btd5PTDay.Count; index++)
                {
                    worksheet.Cell(index + 2, 31).Value = datas.Btd5PTDay[index - 1].Ws;
                    worksheet.Cell(index + 2, 32).Value = datas.Btd5PTDay[index - 1].Product;
                    worksheet.Cell(index + 2, 33).Value = datas.Btd5PTDay[index - 1].Job;
                    worksheet.Cell(index + 2, 34).Value = datas.Btd5PTDay[index - 1].Endtime;
                    worksheet.Cell(index + 2, 35).Value = datas.Btd5PTDay[index - 1].Plantime;
                    worksheet.Cell(index + 2, 36).Value = datas.Btd5PTDay[index - 1].PlanEndtime;
                    worksheet.Cell(index + 2, 37).Value = datas.Btd5PTDay[index - 1].Status;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, contentType, fileName);
                }
            }
            catch
            {
                return Ok();
            }
        }
        public async Task<IActionResult> ExportExcelWeek()
        {
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            string fileName = sDate.ToString("yyyyMMdd_hhmmsstt") + "_BTD_Report_Week.csv";
            try
            {
                var datas = new PtDieCutWeekVM();
                datas = await _ptApiClient.GetPtDieCutWeek(tr);

                var workbook = new XLWorkbook();

                IXLWorksheet worksheet = workbook.Worksheets.Add("Week");
                worksheet.Cell(1, 1).Value = "Statistics";
                worksheet.Cell(1, 2).Value = "BTD2";
                worksheet.Cell(1, 3).Value = "BTD3";
                worksheet.Cell(1, 4).Value = "BTD4";
                worksheet.Cell(1, 5).Value = "BTD5";

                worksheet.Cell(2, 1).Value = "Testing time";
                worksheet.Cell(2, 2).Value = datas.Btd2PTWeek.TestingTimeAndPercent;
                worksheet.Cell(2, 3).Value = datas.Btd3PTWeek.TestingTimeAndPercent;
                worksheet.Cell(2, 4).Value = datas.Btd4PTWeek.TestingTimeAndPercent;
                worksheet.Cell(2, 5).Value = datas.Btd5PTWeek.TestingTimeAndPercent;

                worksheet.Cell(3, 1).Value = "Running time";
                worksheet.Cell(3, 2).Value = datas.Btd2PTWeek.RunningTimeAndPercent;
                worksheet.Cell(3, 3).Value = datas.Btd3PTWeek.RunningTimeAndPercent;
                worksheet.Cell(3, 4).Value = datas.Btd4PTWeek.RunningTimeAndPercent;
                worksheet.Cell(3, 5).Value = datas.Btd5PTWeek.RunningTimeAndPercent;

                worksheet.Cell(4, 1).Value = "Not Running time";
                worksheet.Cell(4, 2).Value = datas.Btd2PTWeek.NotRunningTimeAndPercent;
                worksheet.Cell(4, 3).Value = datas.Btd3PTWeek.NotRunningTimeAndPercent;
                worksheet.Cell(4, 4).Value = datas.Btd4PTWeek.NotRunningTimeAndPercent;
                worksheet.Cell(4, 5).Value = datas.Btd5PTWeek.NotRunningTimeAndPercent;

                worksheet.Cell(5, 1).Value = "No. Product Code Change";
                worksheet.Cell(5, 2).Value = datas.Btd2PTWeek.ProductCodeChangeCount;
                worksheet.Cell(5, 3).Value = datas.Btd3PTWeek.ProductCodeChangeCount;
                worksheet.Cell(5, 4).Value = datas.Btd4PTWeek.ProductCodeChangeCount;
                worksheet.Cell(5, 5).Value = datas.Btd5PTWeek.ProductCodeChangeCount;

                worksheet.Cell(6, 1).Value = "Quantity";
                worksheet.Cell(6, 2).Value = datas.Btd2PTWeek.Quantity;
                worksheet.Cell(6, 3).Value = datas.Btd3PTWeek.Quantity;
                worksheet.Cell(6, 4).Value = datas.Btd4PTWeek.Quantity;
                worksheet.Cell(6, 5).Value = datas.Btd5PTWeek.Quantity;

                worksheet.Cell(7, 1).Value = "Defect";
                worksheet.Cell(7, 2).Value = datas.Btd2PTWeek.Waste;
                worksheet.Cell(7, 3).Value = datas.Btd3PTWeek.Waste;
                worksheet.Cell(7, 4).Value = datas.Btd4PTWeek.Waste;
                worksheet.Cell(7, 5).Value = datas.Btd5PTWeek.Waste;

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, contentType, fileName);
                }
            }
            catch
            {
                return Ok();
            }
        }
        public async Task<IActionResult> ExportExcelMonthYear()
        {
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            if(!monthOryear)
            {
                string fileName = sDate.ToString("yyyyMMdd_hhmmsstt") + "_BTD_Report_Month.csv";
                try
                {
                    var datas = new PtDieCutMonthYearVM();
                    datas = await _ptApiClient.GetPtDieCutMonth(tr);
                    var btd2_avaibility_uptime = (datas.Btd2PTMonthYear.ActualWorkingTime == 0) ? 0 : Math.Round((decimal)((datas.Btd2PTMonthYear.ActualWorkingTime - datas.Btd2PTMonthYear.Downtime) * 100 / datas.Btd2PTMonthYear.ActualWorkingTime), 1);
                    var btd2_capacityUtilization = (datas.Btd2PTMonthYear.MaximumProductionCapacity == 0) ? 0 : Math.Round((decimal)(datas.Btd2PTMonthYear.TotalOutput * 100 / datas.Btd2PTMonthYear.MaximumProductionCapacity), 1);
                    var btd2_productionYield = (datas.Btd2PTMonthYear.TotalOutput == 0) ? 0 : Math.Round((decimal)((datas.Btd2PTMonthYear.TotalOutput - datas.Btd2PTMonthYear.Waste) * 100 / datas.Btd2PTMonthYear.TotalOutput), 1);
                    var btd2_OEE = Math.Round(((btd2_avaibility_uptime / 100) * (btd2_capacityUtilization / 100) * (btd2_productionYield / 100) * 100), 1);

                    var btd3_avaibility_uptime = (datas.Btd3PTMonthYear.ActualWorkingTime == 0) ? 0 : Math.Round((decimal)((datas.Btd3PTMonthYear.ActualWorkingTime - datas.Btd3PTMonthYear.Downtime) * 100 / datas.Btd3PTMonthYear.ActualWorkingTime), 1);
                    var btd3_capacityUtilization = (datas.Btd3PTMonthYear.MaximumProductionCapacity == 0) ? 0 : Math.Round((decimal)(datas.Btd3PTMonthYear.TotalOutput * 100 / datas.Btd3PTMonthYear.MaximumProductionCapacity), 1);
                    var btd3_productionYield = (datas.Btd3PTMonthYear.TotalOutput == 0) ? 0 : Math.Round((decimal)((datas.Btd3PTMonthYear.TotalOutput - datas.Btd3PTMonthYear.Waste) * 100 / datas.Btd3PTMonthYear.TotalOutput), 1);
                    var btd3_OEE = Math.Round(((btd3_avaibility_uptime / 100) * (btd3_capacityUtilization / 100) * (btd3_productionYield / 100) * 100), 1);

                    var btd4_avaibility_uptime = (datas.Btd4PTMonthYear.ActualWorkingTime == 0) ? 0 : Math.Round((decimal)((datas.Btd4PTMonthYear.ActualWorkingTime - datas.Btd4PTMonthYear.Downtime) * 100 / datas.Btd4PTMonthYear.ActualWorkingTime), 1);
                    var btd4_capacityUtilization = (datas.Btd4PTMonthYear.MaximumProductionCapacity == 0) ? 0 : Math.Round((decimal)(datas.Btd4PTMonthYear.TotalOutput * 100 / datas.Btd4PTMonthYear.MaximumProductionCapacity), 1);
                    var btd4_productionYield = (datas.Btd4PTMonthYear.TotalOutput == 0) ? 0 : Math.Round((decimal)((datas.Btd4PTMonthYear.TotalOutput - datas.Btd4PTMonthYear.Waste) * 100 / datas.Btd4PTMonthYear.TotalOutput), 1);
                    var btd4_OEE = Math.Round(((btd4_avaibility_uptime / 100) * (btd4_capacityUtilization / 100) * (btd4_productionYield / 100) * 100), 1);

                    var btd5_avaibility_uptime = (datas.Btd5PTMonthYear.ActualWorkingTime == 0) ? 0 : Math.Round((decimal)((datas.Btd5PTMonthYear.ActualWorkingTime - datas.Btd5PTMonthYear.Downtime) * 100 / datas.Btd5PTMonthYear.ActualWorkingTime), 1);
                    var btd5_capacityUtilization = (datas.Btd5PTMonthYear.MaximumProductionCapacity == 0) ? 0 : Math.Round((decimal)(datas.Btd5PTMonthYear.TotalOutput * 100 / datas.Btd5PTMonthYear.MaximumProductionCapacity), 1);
                    var btd5_productionYield = (datas.Btd5PTMonthYear.TotalOutput == 0) ? 0 : Math.Round((decimal)((datas.Btd5PTMonthYear.TotalOutput - datas.Btd5PTMonthYear.Waste) * 100 / datas.Btd5PTMonthYear.TotalOutput), 1);
                    var btd5_OEE = Math.Round(((btd5_avaibility_uptime / 100) * (btd5_capacityUtilization / 100) * (btd5_productionYield / 100) * 100), 1);

                    var workbook = new XLWorkbook();

                    IXLWorksheet worksheet = workbook.Worksheets.Add("Month");
                    worksheet.Cell(1, 1).Value = "Statistics";
                    worksheet.Cell(1, 2).Value = "BTD2";
                    worksheet.Cell(1, 3).Value = "BTD3";
                    worksheet.Cell(1, 4).Value = "BTD4";
                    worksheet.Cell(1, 5).Value = "BTD5";

                    worksheet.Cell(2, 1).Value = "Actual working time (h)";
                    worksheet.Cell(2, 2).Value = datas.Btd2PTMonthYear.ActualWorkingTime;
                    worksheet.Cell(2, 3).Value = datas.Btd3PTMonthYear.ActualWorkingTime;
                    worksheet.Cell(2, 4).Value = datas.Btd4PTMonthYear.ActualWorkingTime;
                    worksheet.Cell(2, 5).Value = datas.Btd5PTMonthYear.ActualWorkingTime;

                    worksheet.Cell(3, 1).Value = "Downtime (h)";
                    worksheet.Cell(3, 2).Value = datas.Btd2PTMonthYear.Downtime;
                    worksheet.Cell(3, 3).Value = datas.Btd3PTMonthYear.Downtime;
                    worksheet.Cell(3, 4).Value = datas.Btd4PTMonthYear.Downtime;
                    worksheet.Cell(3, 5).Value = datas.Btd5PTMonthYear.Downtime;

                    worksheet.Cell(4, 1).Value = "Total Quantity (pcs)";
                    worksheet.Cell(4, 2).Value = datas.Btd2PTMonthYear.TotalOutput;
                    worksheet.Cell(4, 3).Value = datas.Btd3PTMonthYear.TotalOutput;
                    worksheet.Cell(4, 4).Value = datas.Btd4PTMonthYear.TotalOutput;
                    worksheet.Cell(4, 5).Value = datas.Btd5PTMonthYear.TotalOutput;

                    worksheet.Cell(5, 1).Value = "Maximum production capacity (pcs)";
                    worksheet.Cell(5, 2).Value = datas.Btd2PTMonthYear.MaximumProductionCapacity;
                    worksheet.Cell(5, 3).Value = datas.Btd3PTMonthYear.MaximumProductionCapacity;
                    worksheet.Cell(5, 4).Value = datas.Btd4PTMonthYear.MaximumProductionCapacity;
                    worksheet.Cell(5, 5).Value = datas.Btd5PTMonthYear.MaximumProductionCapacity;

                    worksheet.Cell(6, 1).Value = "Total Defect (pcs)";
                    worksheet.Cell(6, 2).Value = datas.Btd2PTMonthYear.Waste;
                    worksheet.Cell(6, 3).Value = datas.Btd3PTMonthYear.Waste;
                    worksheet.Cell(6, 4).Value = datas.Btd4PTMonthYear.Waste;
                    worksheet.Cell(6, 5).Value = datas.Btd5PTMonthYear.Waste;

                    worksheet.Cell(7, 1).Value = "Avaibility/Uptime (%)";
                    worksheet.Cell(7, 2).Value = btd2_avaibility_uptime;
                    worksheet.Cell(7, 3).Value = btd3_avaibility_uptime;
                    worksheet.Cell(7, 4).Value = btd4_avaibility_uptime;
                    worksheet.Cell(7, 5).Value = btd5_avaibility_uptime;

                    worksheet.Cell(8, 1).Value = "Capacity utilization (%)";
                    worksheet.Cell(8, 2).Value = btd2_capacityUtilization;
                    worksheet.Cell(8, 3).Value = btd3_capacityUtilization;
                    worksheet.Cell(8, 4).Value = btd4_capacityUtilization;
                    worksheet.Cell(8, 5).Value = btd5_capacityUtilization;

                    worksheet.Cell(9, 1).Value = "Production yield (%)";
                    worksheet.Cell(9, 2).Value = btd2_productionYield;
                    worksheet.Cell(9, 3).Value = btd3_productionYield;
                    worksheet.Cell(9, 4).Value = btd4_productionYield;
                    worksheet.Cell(9, 5).Value = btd5_productionYield;

                    worksheet.Cell(10, 1).Value = "OEE (%)";
                    worksheet.Cell(10, 2).Value = btd2_OEE;
                    worksheet.Cell(10, 3).Value = btd3_OEE;
                    worksheet.Cell(10, 4).Value = btd4_OEE;
                    worksheet.Cell(10, 5).Value = btd5_OEE;

                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        var content = stream.ToArray();
                        return File(content, contentType, fileName);
                    }
                }
                catch
                {
                    return Ok();
                }
            }    
            else
            {
                string fileName = sDate.ToString("yyyyMMdd_hhmmsstt") + "_BTD_Report_Year.csv";
                try
                {
                    var datas = new PtDieCutMonthYearVM();
                    datas = await _ptApiClient.GetPtDieCutYear(tr);
                    var btd2_avaibility_uptime = (datas.Btd2PTMonthYear.ActualWorkingTime == 0) ? 0 : Math.Round((decimal)((datas.Btd2PTMonthYear.ActualWorkingTime - datas.Btd2PTMonthYear.Downtime) * 100 / datas.Btd2PTMonthYear.ActualWorkingTime), 1);
                    var btd2_capacityUtilization = (datas.Btd2PTMonthYear.MaximumProductionCapacity == 0) ? 0 : Math.Round((decimal)(datas.Btd2PTMonthYear.TotalOutput * 100 / datas.Btd2PTMonthYear.MaximumProductionCapacity), 1);
                    var btd2_productionYield = (datas.Btd2PTMonthYear.TotalOutput == 0) ? 0 : Math.Round((decimal)((datas.Btd2PTMonthYear.TotalOutput - datas.Btd2PTMonthYear.Waste) * 100 / datas.Btd2PTMonthYear.TotalOutput), 1);
                    var btd2_OEE = Math.Round(((btd2_avaibility_uptime / 100) * (btd2_capacityUtilization / 100) * (btd2_productionYield / 100) * 100), 1);

                    var btd3_avaibility_uptime = (datas.Btd3PTMonthYear.ActualWorkingTime == 0) ? 0 : Math.Round((decimal)((datas.Btd3PTMonthYear.ActualWorkingTime - datas.Btd3PTMonthYear.Downtime) * 100 / datas.Btd3PTMonthYear.ActualWorkingTime), 1);
                    var btd3_capacityUtilization = (datas.Btd3PTMonthYear.MaximumProductionCapacity == 0) ? 0 : Math.Round((decimal)(datas.Btd3PTMonthYear.TotalOutput * 100 / datas.Btd3PTMonthYear.MaximumProductionCapacity), 1);
                    var btd3_productionYield = (datas.Btd3PTMonthYear.TotalOutput == 0) ? 0 : Math.Round((decimal)((datas.Btd3PTMonthYear.TotalOutput - datas.Btd3PTMonthYear.Waste) * 100 / datas.Btd3PTMonthYear.TotalOutput), 1);
                    var btd3_OEE = Math.Round(((btd3_avaibility_uptime / 100) * (btd3_capacityUtilization / 100) * (btd3_productionYield / 100) * 100), 1);

                    var btd4_avaibility_uptime = (datas.Btd4PTMonthYear.ActualWorkingTime == 0) ? 0 : Math.Round((decimal)((datas.Btd4PTMonthYear.ActualWorkingTime - datas.Btd4PTMonthYear.Downtime) * 100 / datas.Btd4PTMonthYear.ActualWorkingTime), 1);
                    var btd4_capacityUtilization = (datas.Btd4PTMonthYear.MaximumProductionCapacity == 0) ? 0 : Math.Round((decimal)(datas.Btd4PTMonthYear.TotalOutput * 100 / datas.Btd4PTMonthYear.MaximumProductionCapacity), 1);
                    var btd4_productionYield = (datas.Btd4PTMonthYear.TotalOutput == 0) ? 0 : Math.Round((decimal)((datas.Btd4PTMonthYear.TotalOutput - datas.Btd4PTMonthYear.Waste) * 100 / datas.Btd4PTMonthYear.TotalOutput), 1);
                    var btd4_OEE = Math.Round(((btd4_avaibility_uptime / 100) * (btd4_capacityUtilization / 100) * (btd4_productionYield / 100) * 100), 1);

                    var btd5_avaibility_uptime = (datas.Btd5PTMonthYear.ActualWorkingTime == 0) ? 0 : Math.Round((decimal)((datas.Btd5PTMonthYear.ActualWorkingTime - datas.Btd5PTMonthYear.Downtime) * 100 / datas.Btd5PTMonthYear.ActualWorkingTime), 1);
                    var btd5_capacityUtilization = (datas.Btd5PTMonthYear.MaximumProductionCapacity == 0) ? 0 : Math.Round((decimal)(datas.Btd5PTMonthYear.TotalOutput * 100 / datas.Btd5PTMonthYear.MaximumProductionCapacity), 1);
                    var btd5_productionYield = (datas.Btd5PTMonthYear.TotalOutput == 0) ? 0 : Math.Round((decimal)((datas.Btd5PTMonthYear.TotalOutput - datas.Btd5PTMonthYear.Waste) * 100 / datas.Btd5PTMonthYear.TotalOutput), 1);
                    var btd5_OEE = Math.Round(((btd5_avaibility_uptime / 100) * (btd5_capacityUtilization / 100) * (btd5_productionYield / 100) * 100), 1);

                    var workbook = new XLWorkbook();

                    IXLWorksheet worksheet = workbook.Worksheets.Add("Year");
                    worksheet.Cell(1, 1).Value = "Statistics";
                    worksheet.Cell(1, 2).Value = "BTD2";
                    worksheet.Cell(1, 3).Value = "BTD3";
                    worksheet.Cell(1, 4).Value = "BTD4";
                    worksheet.Cell(1, 5).Value = "BTD5";

                    worksheet.Cell(2, 1).Value = "Actual working time (h)";
                    worksheet.Cell(2, 2).Value = datas.Btd2PTMonthYear.ActualWorkingTime;
                    worksheet.Cell(2, 3).Value = datas.Btd3PTMonthYear.ActualWorkingTime;
                    worksheet.Cell(2, 4).Value = datas.Btd4PTMonthYear.ActualWorkingTime;
                    worksheet.Cell(2, 5).Value = datas.Btd5PTMonthYear.ActualWorkingTime;

                    worksheet.Cell(3, 1).Value = "Downtime (h)";
                    worksheet.Cell(3, 2).Value = datas.Btd2PTMonthYear.Downtime;
                    worksheet.Cell(3, 3).Value = datas.Btd3PTMonthYear.Downtime;
                    worksheet.Cell(3, 4).Value = datas.Btd4PTMonthYear.Downtime;
                    worksheet.Cell(3, 5).Value = datas.Btd5PTMonthYear.Downtime;

                    worksheet.Cell(4, 1).Value = "Total Quantity (pcs)";
                    worksheet.Cell(4, 2).Value = datas.Btd2PTMonthYear.TotalOutput;
                    worksheet.Cell(4, 3).Value = datas.Btd3PTMonthYear.TotalOutput;
                    worksheet.Cell(4, 4).Value = datas.Btd4PTMonthYear.TotalOutput;
                    worksheet.Cell(4, 5).Value = datas.Btd5PTMonthYear.TotalOutput;

                    worksheet.Cell(5, 1).Value = "Maximum production capacity (pcs)";
                    worksheet.Cell(5, 2).Value = datas.Btd2PTMonthYear.MaximumProductionCapacity;
                    worksheet.Cell(5, 3).Value = datas.Btd3PTMonthYear.MaximumProductionCapacity;
                    worksheet.Cell(5, 4).Value = datas.Btd4PTMonthYear.MaximumProductionCapacity;
                    worksheet.Cell(5, 5).Value = datas.Btd5PTMonthYear.MaximumProductionCapacity;

                    worksheet.Cell(6, 1).Value = "Total Defect (pcs)";
                    worksheet.Cell(6, 2).Value = datas.Btd2PTMonthYear.Waste;
                    worksheet.Cell(6, 3).Value = datas.Btd3PTMonthYear.Waste;
                    worksheet.Cell(6, 4).Value = datas.Btd4PTMonthYear.Waste;
                    worksheet.Cell(6, 5).Value = datas.Btd5PTMonthYear.Waste;

                    worksheet.Cell(7, 1).Value = "Avaibility/Uptime (%)";
                    worksheet.Cell(7, 2).Value = btd2_avaibility_uptime;
                    worksheet.Cell(7, 3).Value = btd3_avaibility_uptime;
                    worksheet.Cell(7, 4).Value = btd4_avaibility_uptime;
                    worksheet.Cell(7, 5).Value = btd5_avaibility_uptime;

                    worksheet.Cell(8, 1).Value = "Capacity utilization (%)";
                    worksheet.Cell(8, 2).Value = btd2_capacityUtilization;
                    worksheet.Cell(8, 3).Value = btd3_capacityUtilization;
                    worksheet.Cell(8, 4).Value = btd4_capacityUtilization;
                    worksheet.Cell(8, 5).Value = btd5_capacityUtilization;

                    worksheet.Cell(9, 1).Value = "Production yield (%)";
                    worksheet.Cell(9, 2).Value = btd2_productionYield;
                    worksheet.Cell(9, 3).Value = btd3_productionYield;
                    worksheet.Cell(9, 4).Value = btd4_productionYield;
                    worksheet.Cell(9, 5).Value = btd5_productionYield;

                    worksheet.Cell(10, 1).Value = "OEE (%)";
                    worksheet.Cell(10, 2).Value = btd2_OEE;
                    worksheet.Cell(10, 3).Value = btd3_OEE;
                    worksheet.Cell(10, 4).Value = btd4_OEE;
                    worksheet.Cell(10, 5).Value = btd5_OEE;

                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        var content = stream.ToArray();
                        return File(content, contentType, fileName);
                    }
                }
                catch
                {
                    return Ok();
                }
            }    
        }
        public async Task<IActionResult> ExportExcelAll()
        {
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            DateTime dt = DateTime.Now;

            string fileName = sDate.ToString("yyyyMMdd_hhmmsstt") + "_BTD_Report.csv";

            try
            {
                var workbook = new XLWorkbook();

                #region 1H
                var timeRequest = new TimeRequest();
                timeRequest.To = sDate;
                timeRequest.From = sDate.AddHours(-1);

                var datas1H = new PtDieCut1HVM();
                datas1H = await _ptApiClient.GetPtDieCut1H(timeRequest);
                IXLWorksheet ws1H = workbook.Worksheets.Add("1 Hour");
                ws1H.Cell(1, 1).Value = "BTD2";
                ws1H.Cell(1, 8).Value = "BTD3";
                ws1H.Cell(1, 15).Value = "BTD4";
                ws1H.Cell(1, 22).Value = "BTD5";

                ws1H.Cell(2, 1).Value = ws1H.Cell(2, 8).Value = ws1H.Cell(2, 15).Value = ws1H.Cell(2, 22).Value = "Worksheet";
                ws1H.Cell(2, 2).Value = ws1H.Cell(2, 9).Value = ws1H.Cell(2, 16).Value = ws1H.Cell(2, 23).Value = "ProductCode";
                ws1H.Cell(2, 3).Value = ws1H.Cell(2, 10).Value = ws1H.Cell(2, 17).Value = ws1H.Cell(2, 24).Value = "Quantity";
                ws1H.Cell(2, 4).Value = ws1H.Cell(2, 11).Value = ws1H.Cell(2, 18).Value = ws1H.Cell(2, 25).Value = "Target";

                for (int index = 1; index <= datas1H.Btd2PT1H.Count; index++)
                {
                    ws1H.Cell(index + 2, 1).Value = datas1H.Btd2PT1H[index - 1].Ws;
                    ws1H.Cell(index + 2, 2).Value = datas1H.Btd2PT1H[index - 1].Product;
                    ws1H.Cell(index + 2, 3).Value = datas1H.Btd2PT1H[index - 1].Job;
                    ws1H.Cell(index + 2, 4).Value = datas1H.Btd2PT1H[index - 1].Target;
                }

                for (int index = 1; index <= datas1H.Btd3PT1H.Count; index++)
                {
                    ws1H.Cell(index + 2, 8).Value = datas1H.Btd3PT1H[index - 1].Ws;
                    ws1H.Cell(index + 2, 9).Value = datas1H.Btd3PT1H[index - 1].Product;
                    ws1H.Cell(index + 2, 10).Value = datas1H.Btd3PT1H[index - 1].Job;
                    ws1H.Cell(index + 2, 11).Value = datas1H.Btd3PT1H[index - 1].Target;
                }

                for (int index = 1; index <= datas1H.Btd4PT1H.Count; index++)
                {
                    ws1H.Cell(index + 2, 15).Value = datas1H.Btd4PT1H[index - 1].Ws;
                    ws1H.Cell(index + 2, 16).Value = datas1H.Btd4PT1H[index - 1].Product;
                    ws1H.Cell(index + 2, 17).Value = datas1H.Btd4PT1H[index - 1].Job;
                    ws1H.Cell(index + 2, 18).Value = datas1H.Btd4PT1H[index - 1].Target;
                }

                for (int index = 1; index <= datas1H.Btd5PT1H.Count; index++)
                {
                    ws1H.Cell(index + 2, 22).Value = datas1H.Btd5PT1H[index - 1].Ws;
                    ws1H.Cell(index + 2, 23).Value = datas1H.Btd5PT1H[index - 1].Product;
                    ws1H.Cell(index + 2, 24).Value = datas1H.Btd5PT1H[index - 1].Job;
                    ws1H.Cell(index + 2, 25).Value = datas1H.Btd5PT1H[index - 1].Target;
                }
                #endregion
                #region Day
                timeRequest.To = sDate;
                timeRequest.From = sDate.AddDays(-1);

                var datasDay = new PtDieCutDayVM();
                datasDay = await _ptApiClient.GetPtDieCutDay(timeRequest);

                IXLWorksheet wsDay = workbook.Worksheets.Add("Day");
                wsDay.Cell(1, 1).Value = "BTD2";
                wsDay.Cell(1, 11).Value = "BTD3";
                wsDay.Cell(1, 21).Value = "BTD4";
                wsDay.Cell(1, 31).Value = "BTD5";

                wsDay.Cell(2, 1).Value = wsDay.Cell(2, 11).Value = wsDay.Cell(2, 21).Value = wsDay.Cell(2, 31).Value = "Worksheet";
                wsDay.Cell(2, 2).Value = wsDay.Cell(2, 12).Value = wsDay.Cell(2, 22).Value = wsDay.Cell(2, 32).Value = "ProductCode";
                wsDay.Cell(2, 3).Value = wsDay.Cell(2, 13).Value = wsDay.Cell(2, 23).Value = wsDay.Cell(2, 33).Value = "Quantity";
                wsDay.Cell(2, 4).Value = wsDay.Cell(2, 14).Value = wsDay.Cell(2, 24).Value = wsDay.Cell(2, 34).Value = "CompleteTime";
                wsDay.Cell(2, 5).Value = wsDay.Cell(2, 15).Value = wsDay.Cell(2, 25).Value = wsDay.Cell(2, 35).Value = "PlanTime";
                wsDay.Cell(2, 6).Value = wsDay.Cell(2, 16).Value = wsDay.Cell(2, 26).Value = wsDay.Cell(2, 36).Value = "PlanEndTime";
                wsDay.Cell(2, 7).Value = wsDay.Cell(2, 17).Value = wsDay.Cell(2, 27).Value = wsDay.Cell(2, 37).Value = "Status";

                for (int index = 1; index <= datasDay.Btd2PTDay.Count; index++)
                {
                    wsDay.Cell(index + 2, 1).Value = datasDay.Btd2PTDay[index - 1].Ws;
                    wsDay.Cell(index + 2, 2).Value = datasDay.Btd2PTDay[index - 1].Product;
                    wsDay.Cell(index + 2, 3).Value = datasDay.Btd2PTDay[index - 1].Job;
                    wsDay.Cell(index + 2, 4).Value = datasDay.Btd2PTDay[index - 1].Endtime;
                    wsDay.Cell(index + 2, 5).Value = datasDay.Btd2PTDay[index - 1].Plantime;
                    wsDay.Cell(index + 2, 6).Value = datasDay.Btd2PTDay[index - 1].PlanEndtime;
                    wsDay.Cell(index + 2, 7).Value = datasDay.Btd2PTDay[index - 1].Status;
                }

                for (int index = 1; index <= datasDay.Btd3PTDay.Count; index++)
                {
                    wsDay.Cell(index + 2, 11).Value = datasDay.Btd3PTDay[index - 1].Ws;
                    wsDay.Cell(index + 2, 12).Value = datasDay.Btd3PTDay[index - 1].Product;
                    wsDay.Cell(index + 2, 13).Value = datasDay.Btd3PTDay[index - 1].Job;
                    wsDay.Cell(index + 2, 14).Value = datasDay.Btd3PTDay[index - 1].Endtime;
                    wsDay.Cell(index + 2, 15).Value = datasDay.Btd3PTDay[index - 1].Plantime;
                    wsDay.Cell(index + 2, 16).Value = datasDay.Btd3PTDay[index - 1].PlanEndtime;
                    wsDay.Cell(index + 2, 17).Value = datasDay.Btd3PTDay[index - 1].Status;
                }

                for (int index = 1; index <= datasDay.Btd4PTDay.Count; index++)
                {
                    wsDay.Cell(index + 2, 21).Value = datasDay.Btd4PTDay[index - 1].Ws;
                    wsDay.Cell(index + 2, 22).Value = datasDay.Btd4PTDay[index - 1].Product;
                    wsDay.Cell(index + 2, 23).Value = datasDay.Btd4PTDay[index - 1].Job;
                    wsDay.Cell(index + 2, 24).Value = datasDay.Btd4PTDay[index - 1].Endtime;
                    wsDay.Cell(index + 2, 25).Value = datasDay.Btd4PTDay[index - 1].Plantime;
                    wsDay.Cell(index + 2, 26).Value = datasDay.Btd4PTDay[index - 1].PlanEndtime;
                    wsDay.Cell(index + 2, 27).Value = datasDay.Btd4PTDay[index - 1].Status;
                }

                for (int index = 1; index <= datasDay.Btd5PTDay.Count; index++)
                {
                    wsDay.Cell(index + 2, 31).Value = datasDay.Btd5PTDay[index - 1].Ws;
                    wsDay.Cell(index + 2, 32).Value = datasDay.Btd5PTDay[index - 1].Product;
                    wsDay.Cell(index + 2, 33).Value = datasDay.Btd5PTDay[index - 1].Job;
                    wsDay.Cell(index + 2, 34).Value = datasDay.Btd5PTDay[index - 1].Endtime;
                    wsDay.Cell(index + 2, 35).Value = datasDay.Btd5PTDay[index - 1].Plantime;
                    wsDay.Cell(index + 2, 36).Value = datasDay.Btd5PTDay[index - 1].PlanEndtime;
                    wsDay.Cell(index + 2, 37).Value = datasDay.Btd5PTDay[index - 1].Status;
                }
                #endregion
                #region Week
                timeRequest.To = StartOfWeek(sDate, DayOfWeek.Monday).AddDays(7);
                timeRequest.From = StartOfWeek(sDate, DayOfWeek.Monday);

                var datas = new PtDieCutWeekVM();
                datas = await _ptApiClient.GetPtDieCutWeek(timeRequest);

                IXLWorksheet wsWeek = workbook.Worksheets.Add("Week");
                wsWeek.Cell(1, 1).Value = "Statistics";
                wsWeek.Cell(1, 2).Value = "BTD2";
                wsWeek.Cell(1, 3).Value = "BTD3";
                wsWeek.Cell(1, 4).Value = "BTD4";
                wsWeek.Cell(1, 5).Value = "BTD5";

                wsWeek.Cell(2, 1).Value = "Testing time";
                wsWeek.Cell(2, 2).Value = datas.Btd2PTWeek.TestingTimeAndPercent;
                wsWeek.Cell(2, 3).Value = datas.Btd3PTWeek.TestingTimeAndPercent;
                wsWeek.Cell(2, 4).Value = datas.Btd4PTWeek.TestingTimeAndPercent;
                wsWeek.Cell(2, 5).Value = datas.Btd5PTWeek.TestingTimeAndPercent;

                wsWeek.Cell(3, 1).Value = "Running time";
                wsWeek.Cell(3, 2).Value = datas.Btd2PTWeek.RunningTimeAndPercent;
                wsWeek.Cell(3, 3).Value = datas.Btd3PTWeek.RunningTimeAndPercent;
                wsWeek.Cell(3, 4).Value = datas.Btd4PTWeek.RunningTimeAndPercent;
                wsWeek.Cell(3, 5).Value = datas.Btd5PTWeek.RunningTimeAndPercent;

                wsWeek.Cell(4, 1).Value = "Not Running time";
                wsWeek.Cell(4, 2).Value = datas.Btd2PTWeek.NotRunningTimeAndPercent;
                wsWeek.Cell(4, 3).Value = datas.Btd3PTWeek.NotRunningTimeAndPercent;
                wsWeek.Cell(4, 4).Value = datas.Btd4PTWeek.NotRunningTimeAndPercent;
                wsWeek.Cell(4, 5).Value = datas.Btd5PTWeek.NotRunningTimeAndPercent;

                wsWeek.Cell(5, 1).Value = "No. Product Code Change";
                wsWeek.Cell(5, 2).Value = datas.Btd2PTWeek.ProductCodeChangeCount;
                wsWeek.Cell(5, 3).Value = datas.Btd3PTWeek.ProductCodeChangeCount;
                wsWeek.Cell(5, 4).Value = datas.Btd4PTWeek.ProductCodeChangeCount;
                wsWeek.Cell(5, 5).Value = datas.Btd5PTWeek.ProductCodeChangeCount;

                wsWeek.Cell(6, 1).Value = "Quantity";
                wsWeek.Cell(6, 2).Value = datas.Btd2PTWeek.Quantity;
                wsWeek.Cell(6, 3).Value = datas.Btd3PTWeek.Quantity;
                wsWeek.Cell(6, 4).Value = datas.Btd4PTWeek.Quantity;
                wsWeek.Cell(6, 5).Value = datas.Btd5PTWeek.Quantity;

                wsWeek.Cell(7, 1).Value = "Defect";
                wsWeek.Cell(7, 2).Value = datas.Btd2PTWeek.Waste;
                wsWeek.Cell(7, 3).Value = datas.Btd3PTWeek.Waste;
                wsWeek.Cell(7, 4).Value = datas.Btd4PTWeek.Waste;
                wsWeek.Cell(7, 5).Value = datas.Btd5PTWeek.Waste;
                #endregion
                #region Month
                timeRequest.To = new DateTime(sDate.Year, sDate.AddMonths(1).Month, 1);
                timeRequest.From = new DateTime(sDate.Year, sDate.Month, 1);

                var datasMonth = new PtDieCutMonthYearVM();
                datasMonth = await _ptApiClient.GetPtDieCutMonth(timeRequest);
                var btd2_avaibility_uptime = (datasMonth.Btd2PTMonthYear.ActualWorkingTime == 0) ? 0 : Math.Round((decimal)((datasMonth.Btd2PTMonthYear.ActualWorkingTime - datasMonth.Btd2PTMonthYear.Downtime) * 100 / datasMonth.Btd2PTMonthYear.ActualWorkingTime), 1);
                var btd2_capacityUtilization = (datasMonth.Btd2PTMonthYear.MaximumProductionCapacity == 0) ? 0 : Math.Round((decimal)(datasMonth.Btd2PTMonthYear.TotalOutput * 100 / datasMonth.Btd2PTMonthYear.MaximumProductionCapacity), 1);
                var btd2_productionYield = (datasMonth.Btd2PTMonthYear.TotalOutput == 0) ? 0 : Math.Round((decimal)((datasMonth.Btd2PTMonthYear.TotalOutput - datasMonth.Btd2PTMonthYear.Waste) * 100 / datasMonth.Btd2PTMonthYear.TotalOutput), 1);
                var btd2_OEE = Math.Round(((btd2_avaibility_uptime / 100) * (btd2_capacityUtilization / 100) * (btd2_productionYield / 100) * 100), 1);

                var btd3_avaibility_uptime = (datasMonth.Btd3PTMonthYear.ActualWorkingTime == 0) ? 0 : Math.Round((decimal)((datasMonth.Btd3PTMonthYear.ActualWorkingTime - datasMonth.Btd3PTMonthYear.Downtime) * 100 / datasMonth.Btd3PTMonthYear.ActualWorkingTime), 1);
                var btd3_capacityUtilization = (datasMonth.Btd3PTMonthYear.MaximumProductionCapacity == 0) ? 0 : Math.Round((decimal)(datasMonth.Btd3PTMonthYear.TotalOutput * 100 / datasMonth.Btd3PTMonthYear.MaximumProductionCapacity), 1);
                var btd3_productionYield = (datasMonth.Btd3PTMonthYear.TotalOutput == 0) ? 0 : Math.Round((decimal)((datasMonth.Btd3PTMonthYear.TotalOutput - datasMonth.Btd3PTMonthYear.Waste) * 100 / datasMonth.Btd3PTMonthYear.TotalOutput), 1);
                var btd3_OEE = Math.Round(((btd3_avaibility_uptime / 100) * (btd3_capacityUtilization / 100) * (btd3_productionYield / 100) * 100), 1);

                var btd4_avaibility_uptime = (datasMonth.Btd4PTMonthYear.ActualWorkingTime == 0) ? 0 : Math.Round((decimal)((datasMonth.Btd4PTMonthYear.ActualWorkingTime - datasMonth.Btd4PTMonthYear.Downtime) * 100 / datasMonth.Btd4PTMonthYear.ActualWorkingTime), 1);
                var btd4_capacityUtilization = (datasMonth.Btd4PTMonthYear.MaximumProductionCapacity == 0) ? 0 : Math.Round((decimal)(datasMonth.Btd4PTMonthYear.TotalOutput * 100 / datasMonth.Btd4PTMonthYear.MaximumProductionCapacity), 1);
                var btd4_productionYield = (datasMonth.Btd4PTMonthYear.TotalOutput == 0) ? 0 : Math.Round((decimal)((datasMonth.Btd4PTMonthYear.TotalOutput - datasMonth.Btd4PTMonthYear.Waste) * 100 / datasMonth.Btd4PTMonthYear.TotalOutput), 1);
                var btd4_OEE = Math.Round(((btd4_avaibility_uptime / 100) * (btd4_capacityUtilization / 100) * (btd4_productionYield / 100) * 100), 1);

                var btd5_avaibility_uptime = (datasMonth.Btd5PTMonthYear.ActualWorkingTime == 0) ? 0 : Math.Round((decimal)((datasMonth.Btd5PTMonthYear.ActualWorkingTime - datasMonth.Btd5PTMonthYear.Downtime) * 100 / datasMonth.Btd5PTMonthYear.ActualWorkingTime), 1);
                var btd5_capacityUtilization = (datasMonth.Btd5PTMonthYear.MaximumProductionCapacity == 0) ? 0 : Math.Round((decimal)(datasMonth.Btd5PTMonthYear.TotalOutput * 100 / datasMonth.Btd5PTMonthYear.MaximumProductionCapacity), 1);
                var btd5_productionYield = (datasMonth.Btd5PTMonthYear.TotalOutput == 0) ? 0 : Math.Round((decimal)((datasMonth.Btd5PTMonthYear.TotalOutput - datasMonth.Btd5PTMonthYear.Waste) * 100 / datasMonth.Btd5PTMonthYear.TotalOutput), 1);
                var btd5_OEE = Math.Round(((btd5_avaibility_uptime / 100) * (btd5_capacityUtilization / 100) * (btd5_productionYield / 100) * 100), 1);

                IXLWorksheet wsMonth = workbook.Worksheets.Add("Month");
                wsMonth.Cell(1, 1).Value = "Statistics";
                wsMonth.Cell(1, 2).Value = "BTD2";
                wsMonth.Cell(1, 3).Value = "BTD3";
                wsMonth.Cell(1, 4).Value = "BTD4";
                wsMonth.Cell(1, 5).Value = "BTD5";

                wsMonth.Cell(2, 1).Value = "Actual working time (h)";
                wsMonth.Cell(2, 2).Value = datasMonth.Btd2PTMonthYear.ActualWorkingTime;
                wsMonth.Cell(2, 3).Value = datasMonth.Btd3PTMonthYear.ActualWorkingTime;
                wsMonth.Cell(2, 4).Value = datasMonth.Btd4PTMonthYear.ActualWorkingTime;
                wsMonth.Cell(2, 5).Value = datasMonth.Btd5PTMonthYear.ActualWorkingTime;

                wsMonth.Cell(3, 1).Value = "Downtime (h)";
                wsMonth.Cell(3, 2).Value = datasMonth.Btd2PTMonthYear.Downtime;
                wsMonth.Cell(3, 3).Value = datasMonth.Btd3PTMonthYear.Downtime;
                wsMonth.Cell(3, 4).Value = datasMonth.Btd4PTMonthYear.Downtime;
                wsMonth.Cell(3, 5).Value = datasMonth.Btd5PTMonthYear.Downtime;

                wsMonth.Cell(4, 1).Value = "Total Quantity (pcs)";
                wsMonth.Cell(4, 2).Value = datasMonth.Btd2PTMonthYear.TotalOutput;
                wsMonth.Cell(4, 3).Value = datasMonth.Btd3PTMonthYear.TotalOutput;
                wsMonth.Cell(4, 4).Value = datasMonth.Btd4PTMonthYear.TotalOutput;
                wsMonth.Cell(4, 5).Value = datasMonth.Btd5PTMonthYear.TotalOutput;

                wsMonth.Cell(5, 1).Value = "Maximum production capacity (pcs)";
                wsMonth.Cell(5, 2).Value = datasMonth.Btd2PTMonthYear.MaximumProductionCapacity;
                wsMonth.Cell(5, 3).Value = datasMonth.Btd3PTMonthYear.MaximumProductionCapacity;
                wsMonth.Cell(5, 4).Value = datasMonth.Btd4PTMonthYear.MaximumProductionCapacity;
                wsMonth.Cell(5, 5).Value = datasMonth.Btd5PTMonthYear.MaximumProductionCapacity;

                wsMonth.Cell(6, 1).Value = "Total Defect (pcs)";
                wsMonth.Cell(6, 2).Value = datasMonth.Btd2PTMonthYear.Waste;
                wsMonth.Cell(6, 3).Value = datasMonth.Btd3PTMonthYear.Waste;
                wsMonth.Cell(6, 4).Value = datasMonth.Btd4PTMonthYear.Waste;
                wsMonth.Cell(6, 5).Value = datasMonth.Btd5PTMonthYear.Waste;

                wsMonth.Cell(7, 1).Value = "Avaibility/Uptime (%)";
                wsMonth.Cell(7, 2).Value = btd2_avaibility_uptime;
                wsMonth.Cell(7, 3).Value = btd3_avaibility_uptime;
                wsMonth.Cell(7, 4).Value = btd4_avaibility_uptime;
                wsMonth.Cell(7, 5).Value = btd5_avaibility_uptime;

                wsMonth.Cell(8, 1).Value = "Capacity utilization (%)";
                wsMonth.Cell(8, 2).Value = btd2_capacityUtilization;
                wsMonth.Cell(8, 3).Value = btd3_capacityUtilization;
                wsMonth.Cell(8, 4).Value = btd4_capacityUtilization;
                wsMonth.Cell(8, 5).Value = btd5_capacityUtilization;

                wsMonth.Cell(9, 1).Value = "Production yield (%)";
                wsMonth.Cell(9, 2).Value = btd2_productionYield;
                wsMonth.Cell(9, 3).Value = btd3_productionYield;
                wsMonth.Cell(9, 4).Value = btd4_productionYield;
                wsMonth.Cell(9, 5).Value = btd5_productionYield;

                wsMonth.Cell(10, 1).Value = "OEE (%)";
                wsMonth.Cell(10, 2).Value = btd2_OEE;
                wsMonth.Cell(10, 3).Value = btd3_OEE;
                wsMonth.Cell(10, 4).Value = btd4_OEE;
                wsMonth.Cell(10, 5).Value = btd5_OEE;
                #endregion
                #region Year
                timeRequest.To = new DateTime(sDate.AddYears(1).Year, 1, 1);
                timeRequest.From = new DateTime(sDate.Year, 1, 1);

                var datasYear = new PtDieCutMonthYearVM();
                datasYear = await _ptApiClient.GetPtDieCutYear(timeRequest);
                var btd2_avaibility_uptime_y = (datasYear.Btd2PTMonthYear.ActualWorkingTime == 0) ? 0 : Math.Round((decimal)((datasYear.Btd2PTMonthYear.ActualWorkingTime - datasYear.Btd2PTMonthYear.Downtime) * 100 / datasYear.Btd2PTMonthYear.ActualWorkingTime), 1);
                var btd2_capacityUtilization_y = (datasYear.Btd2PTMonthYear.MaximumProductionCapacity == 0) ? 0 : Math.Round((decimal)(datasYear.Btd2PTMonthYear.TotalOutput * 100 / datasYear.Btd2PTMonthYear.MaximumProductionCapacity), 1);
                var btd2_productionYield_y = (datasYear.Btd2PTMonthYear.TotalOutput == 0) ? 0 : Math.Round((decimal)((datasYear.Btd2PTMonthYear.TotalOutput - datasYear.Btd2PTMonthYear.Waste) * 100 / datasYear.Btd2PTMonthYear.TotalOutput), 1);
                var btd2_OEE_y = Math.Round(((btd2_avaibility_uptime_y / 100) * (btd2_capacityUtilization_y / 100) * (btd2_productionYield_y / 100) * 100), 1);

                var btd3_avaibility_uptime_y = (datasYear.Btd3PTMonthYear.ActualWorkingTime == 0) ? 0 : Math.Round((decimal)((datasYear.Btd3PTMonthYear.ActualWorkingTime - datasYear.Btd3PTMonthYear.Downtime) * 100 / datasYear.Btd3PTMonthYear.ActualWorkingTime), 1);
                var btd3_capacityUtilization_y = (datasYear.Btd3PTMonthYear.MaximumProductionCapacity == 0) ? 0 : Math.Round((decimal)(datasYear.Btd3PTMonthYear.TotalOutput * 100 / datasYear.Btd3PTMonthYear.MaximumProductionCapacity), 1);
                var btd3_productionYield_y = (datasYear.Btd3PTMonthYear.TotalOutput == 0) ? 0 : Math.Round((decimal)((datasYear.Btd3PTMonthYear.TotalOutput - datasYear.Btd3PTMonthYear.Waste) * 100 / datasYear.Btd3PTMonthYear.TotalOutput), 1);
                var btd3_OEE_y = Math.Round(((btd3_avaibility_uptime_y / 100) * (btd3_capacityUtilization_y / 100) * (btd3_productionYield_y / 100) * 100), 1);

                var btd4_avaibility_uptime_y = (datasYear.Btd4PTMonthYear.ActualWorkingTime == 0) ? 0 : Math.Round((decimal)((datasYear.Btd4PTMonthYear.ActualWorkingTime - datasYear.Btd4PTMonthYear.Downtime) * 100 / datasYear.Btd4PTMonthYear.ActualWorkingTime), 1);
                var btd4_capacityUtilization_y = (datasYear.Btd4PTMonthYear.MaximumProductionCapacity == 0) ? 0 : Math.Round((decimal)(datasYear.Btd4PTMonthYear.TotalOutput * 100 / datasYear.Btd4PTMonthYear.MaximumProductionCapacity), 1);
                var btd4_productionYield_y = (datasYear.Btd4PTMonthYear.TotalOutput == 0) ? 0 : Math.Round((decimal)((datasYear.Btd4PTMonthYear.TotalOutput - datasYear.Btd4PTMonthYear.Waste) * 100 / datasYear.Btd4PTMonthYear.TotalOutput), 1);
                var btd4_OEE_y = Math.Round(((btd4_avaibility_uptime_y / 100) * (btd4_capacityUtilization_y / 100) * (btd4_productionYield_y / 100) * 100), 1);

                var btd5_avaibility_uptime_y = (datasYear.Btd5PTMonthYear.ActualWorkingTime == 0) ? 0 : Math.Round((decimal)((datasYear.Btd5PTMonthYear.ActualWorkingTime - datasYear.Btd5PTMonthYear.Downtime) * 100 / datasYear.Btd5PTMonthYear.ActualWorkingTime), 1);
                var btd5_capacityUtilization_y = (datasYear.Btd5PTMonthYear.MaximumProductionCapacity == 0) ? 0 : Math.Round((decimal)(datasYear.Btd5PTMonthYear.TotalOutput * 100 / datasYear.Btd5PTMonthYear.MaximumProductionCapacity), 1);
                var btd5_productionYield_y = (datasYear.Btd5PTMonthYear.TotalOutput == 0) ? 0 : Math.Round((decimal)((datasYear.Btd5PTMonthYear.TotalOutput - datasYear.Btd5PTMonthYear.Waste) * 100 / datasYear.Btd5PTMonthYear.TotalOutput), 1);
                var btd5_OEE_y = Math.Round(((btd5_avaibility_uptime_y / 100) * (btd5_capacityUtilization_y / 100) * (btd5_productionYield_y / 100) * 100), 1);

                IXLWorksheet wsYear = workbook.Worksheets.Add("Year");
                wsYear.Cell(1, 1).Value = "Statistics";
                wsYear.Cell(1, 2).Value = "BTD2";
                wsYear.Cell(1, 3).Value = "BTD3";
                wsYear.Cell(1, 4).Value = "BTD4";
                wsYear.Cell(1, 5).Value = "BTD5";

                wsYear.Cell(2, 1).Value = "Actual working time (h)";
                wsYear.Cell(2, 2).Value = datasYear.Btd2PTMonthYear.ActualWorkingTime;
                wsYear.Cell(2, 3).Value = datasYear.Btd3PTMonthYear.ActualWorkingTime;
                wsYear.Cell(2, 4).Value = datasYear.Btd4PTMonthYear.ActualWorkingTime;
                wsYear.Cell(2, 5).Value = datasYear.Btd5PTMonthYear.ActualWorkingTime;

                wsYear.Cell(3, 1).Value = "Downtime (h)";
                wsYear.Cell(3, 2).Value = datasYear.Btd2PTMonthYear.Downtime;
                wsYear.Cell(3, 3).Value = datasYear.Btd3PTMonthYear.Downtime;
                wsYear.Cell(3, 4).Value = datasYear.Btd4PTMonthYear.Downtime;
                wsYear.Cell(3, 5).Value = datasYear.Btd5PTMonthYear.Downtime;

                wsYear.Cell(4, 1).Value = "Total Quantity (pcs)";
                wsYear.Cell(4, 2).Value = datasYear.Btd2PTMonthYear.TotalOutput;
                wsYear.Cell(4, 3).Value = datasYear.Btd3PTMonthYear.TotalOutput;
                wsYear.Cell(4, 4).Value = datasYear.Btd4PTMonthYear.TotalOutput;
                wsYear.Cell(4, 5).Value = datasYear.Btd5PTMonthYear.TotalOutput;

                wsYear.Cell(5, 1).Value = "Maximum production capacity (pcs)";
                wsYear.Cell(5, 2).Value = datasYear.Btd2PTMonthYear.MaximumProductionCapacity;
                wsYear.Cell(5, 3).Value = datasYear.Btd3PTMonthYear.MaximumProductionCapacity;
                wsYear.Cell(5, 4).Value = datasYear.Btd4PTMonthYear.MaximumProductionCapacity;
                wsYear.Cell(5, 5).Value = datasYear.Btd5PTMonthYear.MaximumProductionCapacity;

                wsYear.Cell(6, 1).Value = "Total Defect (pcs)";
                wsYear.Cell(6, 2).Value = datasYear.Btd2PTMonthYear.Waste;
                wsYear.Cell(6, 3).Value = datasYear.Btd3PTMonthYear.Waste;
                wsYear.Cell(6, 4).Value = datasYear.Btd4PTMonthYear.Waste;
                wsYear.Cell(6, 5).Value = datasYear.Btd5PTMonthYear.Waste;

                wsYear.Cell(7, 1).Value = "Avaibility/Uptime (%)";
                wsYear.Cell(7, 2).Value = btd2_avaibility_uptime_y;
                wsYear.Cell(7, 3).Value = btd3_avaibility_uptime_y;
                wsYear.Cell(7, 4).Value = btd4_avaibility_uptime_y;
                wsYear.Cell(7, 5).Value = btd5_avaibility_uptime_y;

                wsYear.Cell(8, 1).Value = "Capacity utilization (%)";
                wsYear.Cell(8, 2).Value = btd2_capacityUtilization_y;
                wsYear.Cell(8, 3).Value = btd3_capacityUtilization_y;
                wsYear.Cell(8, 4).Value = btd4_capacityUtilization_y;
                wsYear.Cell(8, 5).Value = btd5_capacityUtilization_y;

                wsYear.Cell(9, 1).Value = "Production yield (%)";
                wsYear.Cell(9, 2).Value = btd2_productionYield_y;
                wsYear.Cell(9, 3).Value = btd3_productionYield_y;
                wsYear.Cell(9, 4).Value = btd4_productionYield_y;
                wsYear.Cell(9, 5).Value = btd5_productionYield_y;

                wsYear.Cell(10, 1).Value = "OEE (%)";
                wsYear.Cell(10, 2).Value = btd2_OEE_y;
                wsYear.Cell(10, 3).Value = btd3_OEE_y;
                wsYear.Cell(10, 4).Value = btd4_OEE_y;
                wsYear.Cell(10, 5).Value = btd5_OEE_y;
                #endregion
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, contentType, fileName);
                }
            }
            catch
            {
                return Ok();
            }
        }


        public DateTime StartOfWeek(DateTime dt, DayOfWeek startOfWeek)
        {
            int diff = (7 + (dt.DayOfWeek - startOfWeek)) % 7;
            return dt.AddDays(-1 * diff).Date;
        }
    }
}
