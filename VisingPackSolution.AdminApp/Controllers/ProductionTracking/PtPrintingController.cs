using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using VisingPackSolution.ApiIntegration.Interfaces;
using VisingPackSolution.ViewModels.Common;
using VisingPackSolution.ViewModels.ProductionTracking;

namespace VisingPackSolution.AdminApp.Controllers.ProductionTracking
{
    //[Authorize(Roles = "Admin, Manager, Planner, User")]
    public class PtPrintingController : BaseController
    {
        private readonly IProductionTrackingApiClient _ptApiClient;
        private readonly IConfiguration _configuration;
        private static bool monthOryear { get; set; }
        private static DateTime sDate { get; set; }
        private static TimeRequest tr { get; set; }
        public PtPrintingController(IProductionTrackingApiClient ptApiClient,
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
            var result = new PtPrinting1HVM();
            result = await _ptApiClient.GetPtPrinting1H(timeRequest);

            return View("~/Views/ProductionTracking/Printing1H.cshtml", result);
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
            var result = new PtPrintingDayVM();
            result = await _ptApiClient.GetPtPrintingDay(timeRequest);

            return View("~/Views/ProductionTracking/PrintingDay.cshtml", result);
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
            var result = new PtPrintingWeekVM();
            result = await _ptApiClient.GetPtPrintingWeek(timeRequest);

            return View("~/Views/ProductionTracking/PrintingWeek.cshtml", result);
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

            var result = new PtPrintingMonthYearVM();
            result = await _ptApiClient.GetPtPrintingMonth(timeRequest);

            var p601_avaibility_uptime = (result.P601PTMonthYear.ActualWorkingTime == 0) ? 0 : Math.Round((decimal)((result.P601PTMonthYear.ActualWorkingTime - result.P601PTMonthYear.Downtime) * 100 / result.P601PTMonthYear.ActualWorkingTime), 1);
            var p601_capacityUtilization = (result.P601PTMonthYear.MaximumProductionCapacity == 0) ? 0 : Math.Round((decimal)(result.P601PTMonthYear.TotalOutput * 100 / result.P601PTMonthYear.MaximumProductionCapacity), 1);
            var p601_productionYield = (result.P601PTMonthYear.TotalOutput == 0) ? 0 : Math.Round((decimal)((result.P601PTMonthYear.TotalOutput - result.P601PTMonthYear.Waste) * 100 / result.P601PTMonthYear.TotalOutput), 1);
            var p601_OEE = Math.Round(((p601_avaibility_uptime / 100) * (p601_capacityUtilization / 100) * (p601_productionYield / 100) * 100), 1);

            ViewBag.p601_AvaiUptime = p601_avaibility_uptime.ToString() + "%";
            ViewBag.p601_CapaUtilication = p601_capacityUtilization.ToString() + "%";
            ViewBag.p601_ProYield = p601_productionYield.ToString() + "%";
            ViewBag.p601_OEE = p601_OEE.ToString() + "%";

            var p604_avaibility_uptime = (result.P604PTMonthYear.ActualWorkingTime == 0) ? 0 : Math.Round((decimal)((result.P604PTMonthYear.ActualWorkingTime - result.P604PTMonthYear.Downtime) * 100 / result.P604PTMonthYear.ActualWorkingTime), 1);
            var p604_capacityUtilization = (result.P604PTMonthYear.MaximumProductionCapacity == 0) ? 0 : Math.Round((decimal)(result.P604PTMonthYear.TotalOutput * 100 / result.P604PTMonthYear.MaximumProductionCapacity), 1);
            var p604_productionYield = (result.P604PTMonthYear.TotalOutput == 0) ? 0 : Math.Round((decimal)((result.P604PTMonthYear.TotalOutput - result.P604PTMonthYear.Waste) * 100 / result.P604PTMonthYear.TotalOutput), 1);
            var p604_OEE = Math.Round(((p604_avaibility_uptime / 100) * (p604_capacityUtilization / 100) * (p604_productionYield / 100) * 100), 1);

            ViewBag.p604_AvaiUptime = p604_avaibility_uptime.ToString() + "%";
            ViewBag.p604_CapaUtilication = p604_capacityUtilization.ToString() + "%";
            ViewBag.p604_ProYield = p604_productionYield.ToString() + "%";
            ViewBag.p604_OEE = p604_OEE.ToString() + "%";

            var p605_avaibility_uptime = (result.P605PTMonthYear.ActualWorkingTime == 0) ? 0 : Math.Round((decimal)((result.P605PTMonthYear.ActualWorkingTime - result.P605PTMonthYear.Downtime) * 100 / result.P605PTMonthYear.ActualWorkingTime), 1);
            var p605_capacityUtilization = (result.P605PTMonthYear.MaximumProductionCapacity == 0) ? 0 : Math.Round((decimal)(result.P605PTMonthYear.TotalOutput * 100 / result.P605PTMonthYear.MaximumProductionCapacity), 1);
            var p605_productionYield = (result.P605PTMonthYear.TotalOutput == 0) ? 0 : Math.Round((decimal)((result.P605PTMonthYear.TotalOutput - result.P605PTMonthYear.Waste) * 100 / result.P605PTMonthYear.TotalOutput), 1);
            var p605_OEE = Math.Round(((p605_avaibility_uptime / 100) * (p605_capacityUtilization / 100) * (p605_productionYield / 100) * 100), 1);

            ViewBag.p605_AvaiUptime = p605_avaibility_uptime.ToString() + "%";
            ViewBag.p605_CapaUtilication = p605_capacityUtilization.ToString() + "%";
            ViewBag.p605_ProYield = p605_productionYield.ToString() + "%";
            ViewBag.p605_OEE = p605_OEE.ToString() + "%";

            var p5m_avaibility_uptime = (result.P5mPTMonthYear.ActualWorkingTime == 0) ? 0 : Math.Round((decimal)((result.P5mPTMonthYear.ActualWorkingTime - result.P5mPTMonthYear.Downtime) * 100 / result.P5mPTMonthYear.ActualWorkingTime), 1);
            var p5m_capacityUtilization = (result.P5mPTMonthYear.MaximumProductionCapacity == 0) ? 0 : Math.Round((decimal)(result.P5mPTMonthYear.TotalOutput * 100 / result.P5mPTMonthYear.MaximumProductionCapacity), 1);
            var p5m_productionYield = (result.P5mPTMonthYear.TotalOutput == 0) ? 0 : Math.Round((decimal)((result.P5mPTMonthYear.TotalOutput - result.P5mPTMonthYear.Waste) * 100 / result.P5mPTMonthYear.TotalOutput), 1);
            var p5m_OEE = Math.Round(((p5m_avaibility_uptime / 100) * (p5m_capacityUtilization / 100) * (p5m_productionYield / 100) * 100), 1);

            ViewBag.p5m_AvaiUptime = p5m_avaibility_uptime.ToString() + "%";
            ViewBag.p5m_CapaUtilication = p5m_capacityUtilization.ToString() + "%";
            ViewBag.p5m_ProYield = p5m_productionYield.ToString() + "%";
            ViewBag.p5m_OEE = p5m_OEE.ToString() + "%";

            return View("~/Views/ProductionTracking/PrintingMonth.cshtml", result);


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

            var result = new PtPrintingMonthYearVM();
            result = await _ptApiClient.GetPtPrintingYear(timeRequest);

            var p601_avaibility_uptime = (result.P601PTMonthYear.ActualWorkingTime == 0) ? 0 : Math.Round((decimal)((result.P601PTMonthYear.ActualWorkingTime - result.P601PTMonthYear.Downtime) * 100 / result.P601PTMonthYear.ActualWorkingTime), 1);
            var p601_capacityUtilization = (result.P601PTMonthYear.MaximumProductionCapacity == 0) ? 0 : Math.Round((decimal)(result.P601PTMonthYear.TotalOutput * 100 / result.P601PTMonthYear.MaximumProductionCapacity), 1);
            var p601_productionYield = (result.P601PTMonthYear.TotalOutput == 0) ? 0 : Math.Round((decimal)((result.P601PTMonthYear.TotalOutput - result.P601PTMonthYear.Waste) * 100 / result.P601PTMonthYear.TotalOutput), 1);
            var p601_OEE = Math.Round(((p601_avaibility_uptime / 100) * (p601_capacityUtilization / 100) * (p601_productionYield / 100) * 100), 1);

            ViewBag.p601_AvaiUptime = p601_avaibility_uptime.ToString() + "%";
            ViewBag.p601_CapaUtilication = p601_capacityUtilization.ToString() + "%";
            ViewBag.p601_ProYield = p601_productionYield.ToString() + "%";
            ViewBag.p601_OEE = p601_OEE.ToString() + "%";

            var p604_avaibility_uptime = (result.P604PTMonthYear.ActualWorkingTime == 0) ? 0 : Math.Round((decimal)((result.P604PTMonthYear.ActualWorkingTime - result.P604PTMonthYear.Downtime) * 100 / result.P604PTMonthYear.ActualWorkingTime), 1);
            var p604_capacityUtilization = (result.P604PTMonthYear.MaximumProductionCapacity == 0) ? 0 : Math.Round((decimal)(result.P604PTMonthYear.TotalOutput * 100 / result.P604PTMonthYear.MaximumProductionCapacity), 1);
            var p604_productionYield = (result.P604PTMonthYear.TotalOutput == 0) ? 0 : Math.Round((decimal)((result.P604PTMonthYear.TotalOutput - result.P604PTMonthYear.Waste) * 100 / result.P604PTMonthYear.TotalOutput), 1);
            var p604_OEE = Math.Round(((p604_avaibility_uptime / 100) * (p604_capacityUtilization / 100) * (p604_productionYield / 100) * 100), 1);

            ViewBag.p604_AvaiUptime = p604_avaibility_uptime.ToString() + "%";
            ViewBag.p604_CapaUtilication = p604_capacityUtilization.ToString() + "%";
            ViewBag.p604_ProYield = p604_productionYield.ToString() + "%";
            ViewBag.p604_OEE = p604_OEE.ToString() + "%";

            var p605_avaibility_uptime = (result.P605PTMonthYear.ActualWorkingTime == 0) ? 0 : Math.Round((decimal)((result.P605PTMonthYear.ActualWorkingTime - result.P605PTMonthYear.Downtime) * 100 / result.P605PTMonthYear.ActualWorkingTime), 1);
            var p605_capacityUtilization = (result.P605PTMonthYear.MaximumProductionCapacity == 0) ? 0 : Math.Round((decimal)(result.P605PTMonthYear.TotalOutput * 100 / result.P605PTMonthYear.MaximumProductionCapacity), 1);
            var p605_productionYield = (result.P605PTMonthYear.TotalOutput == 0) ? 0 : Math.Round((decimal)((result.P605PTMonthYear.TotalOutput - result.P605PTMonthYear.Waste) * 100 / result.P605PTMonthYear.TotalOutput), 1);
            var p605_OEE = Math.Round(((p605_avaibility_uptime / 100) * (p605_capacityUtilization / 100) * (p605_productionYield / 100) * 100), 1);

            ViewBag.p605_AvaiUptime = p605_avaibility_uptime.ToString() + "%";
            ViewBag.p605_CapaUtilication = p605_capacityUtilization.ToString() + "%";
            ViewBag.p605_ProYield = p605_productionYield.ToString() + "%";
            ViewBag.p605_OEE = p605_OEE.ToString() + "%";

            var p5m_avaibility_uptime = (result.P5mPTMonthYear.ActualWorkingTime == 0) ? 0 : Math.Round((decimal)((result.P5mPTMonthYear.ActualWorkingTime - result.P5mPTMonthYear.Downtime) * 100 / result.P5mPTMonthYear.ActualWorkingTime), 1);
            var p5m_capacityUtilization = (result.P5mPTMonthYear.MaximumProductionCapacity == 0) ? 0 : Math.Round((decimal)(result.P5mPTMonthYear.TotalOutput * 100 / result.P5mPTMonthYear.MaximumProductionCapacity), 1);
            var p5m_productionYield = (result.P5mPTMonthYear.TotalOutput == 0) ? 0 : Math.Round((decimal)((result.P5mPTMonthYear.TotalOutput - result.P5mPTMonthYear.Waste) * 100 / result.P5mPTMonthYear.TotalOutput), 1);
            var p5m_OEE = Math.Round(((p5m_avaibility_uptime / 100) * (p5m_capacityUtilization / 100) * (p5m_productionYield / 100) * 100), 1);

            ViewBag.p5m_AvaiUptime = p5m_avaibility_uptime.ToString() + "%";
            ViewBag.p5m_CapaUtilication = p5m_capacityUtilization.ToString() + "%";
            ViewBag.p5m_ProYield = p5m_productionYield.ToString() + "%";
            ViewBag.p5m_OEE = p5m_OEE.ToString() + "%";

            return View("~/Views/ProductionTracking/PrintingYear.cshtml", result);
        }

        public async Task<IActionResult> ExportExcel1H()
        {
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            string fileName = sDate.ToString("yyyyMMdd_hhmmsstt") + "_Printing_Report_1H.csv";
            try
            {
                var datas = new PtPrinting1HVM();
                datas = await _ptApiClient.GetPtPrinting1H(tr);

                var workbook = new XLWorkbook();

                IXLWorksheet worksheet = workbook.Worksheets.Add("1 Hour");
                worksheet.Cell(1, 1).Value = "P601";
                worksheet.Cell(1, 8).Value = "P604";
                worksheet.Cell(1, 15).Value = "P605";
                worksheet.Cell(1, 22).Value = "P5m";

                worksheet.Cell(2, 1).Value = worksheet.Cell(2, 8).Value = worksheet.Cell(2, 15).Value = worksheet.Cell(2, 22).Value = "Worksheet";
                worksheet.Cell(2, 2).Value = worksheet.Cell(2, 9).Value = worksheet.Cell(2, 16).Value = worksheet.Cell(2, 23).Value = "ProductCode";
                worksheet.Cell(2, 3).Value = worksheet.Cell(2, 10).Value = worksheet.Cell(2, 17).Value = worksheet.Cell(2, 24).Value = "Quantity";
                worksheet.Cell(2, 4).Value = worksheet.Cell(2, 11).Value = worksheet.Cell(2, 18).Value = worksheet.Cell(2, 25).Value = "Target";

                for (int index = 1; index <= datas.P601PT1H.Count; index++)
                {
                    worksheet.Cell(index + 2, 1).Value = datas.P601PT1H[index - 1].Ws;
                    worksheet.Cell(index + 2, 2).Value = datas.P601PT1H[index - 1].Product;
                    worksheet.Cell(index + 2, 3).Value = datas.P601PT1H[index - 1].Job;
                    worksheet.Cell(index + 2, 4).Value = datas.P601PT1H[index - 1].Target;
                }

                for (int index = 1; index <= datas.P604PT1H.Count; index++)
                {
                    worksheet.Cell(index + 2, 8).Value = datas.P604PT1H[index - 1].Ws;
                    worksheet.Cell(index + 2, 9).Value = datas.P604PT1H[index - 1].Product;
                    worksheet.Cell(index + 2, 10).Value = datas.P604PT1H[index - 1].Job;
                    worksheet.Cell(index + 2, 11).Value = datas.P604PT1H[index - 1].Target;
                }

                for (int index = 1; index <= datas.P605PT1H.Count; index++)
                {
                    worksheet.Cell(index + 2, 15).Value = datas.P605PT1H[index - 1].Ws;
                    worksheet.Cell(index + 2, 16).Value = datas.P605PT1H[index - 1].Product;
                    worksheet.Cell(index + 2, 17).Value = datas.P605PT1H[index - 1].Job;
                    worksheet.Cell(index + 2, 18).Value = datas.P605PT1H[index - 1].Target;
                }

                for (int index = 1; index <= datas.P5mPT1H.Count; index++)
                {
                    worksheet.Cell(index + 2, 22).Value = datas.P5mPT1H[index - 1].Ws;
                    worksheet.Cell(index + 2, 23).Value = datas.P5mPT1H[index - 1].Product;
                    worksheet.Cell(index + 2, 24).Value = datas.P5mPT1H[index - 1].Job;
                    worksheet.Cell(index + 2, 25).Value = datas.P5mPT1H[index - 1].Target;
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
            string fileName = sDate.ToString("yyyyMMdd_hhmmsstt") + "_Printing_Report_Day.csv";
            try
            {
                var datas = new PtPrintingDayVM();
                datas = await _ptApiClient.GetPtPrintingDay(tr);

                var workbook = new XLWorkbook();

                IXLWorksheet worksheet = workbook.Worksheets.Add("Day");
                worksheet.Cell(1, 1).Value = "P601";
                worksheet.Cell(1, 11).Value = "P604";
                worksheet.Cell(1, 21).Value = "P605";
                worksheet.Cell(1, 31).Value = "P5m";

                worksheet.Cell(2, 1).Value = worksheet.Cell(2, 11).Value = worksheet.Cell(2, 21).Value = worksheet.Cell(2, 31).Value = "Worksheet";
                worksheet.Cell(2, 2).Value = worksheet.Cell(2, 12).Value = worksheet.Cell(2, 22).Value = worksheet.Cell(2, 32).Value = "ProductCode";
                worksheet.Cell(2, 3).Value = worksheet.Cell(2, 13).Value = worksheet.Cell(2, 23).Value = worksheet.Cell(2, 33).Value = "Quantity";
                worksheet.Cell(2, 4).Value = worksheet.Cell(2, 14).Value = worksheet.Cell(2, 24).Value = worksheet.Cell(2, 34).Value = "CompleteTime";
                worksheet.Cell(2, 5).Value = worksheet.Cell(2, 15).Value = worksheet.Cell(2, 25).Value = worksheet.Cell(2, 35).Value = "PlanTime";
                worksheet.Cell(2, 6).Value = worksheet.Cell(2, 16).Value = worksheet.Cell(2, 26).Value = worksheet.Cell(2, 36).Value = "PlanEndTime";
                worksheet.Cell(2, 7).Value = worksheet.Cell(2, 17).Value = worksheet.Cell(2, 27).Value = worksheet.Cell(2, 37).Value = "Status";

                for (int index = 1; index <= datas.P601PTDay.Count; index++)
                {
                    worksheet.Cell(index + 2, 1).Value = datas.P601PTDay[index - 1].Ws;
                    worksheet.Cell(index + 2, 2).Value = datas.P601PTDay[index - 1].Product;
                    worksheet.Cell(index + 2, 3).Value = datas.P601PTDay[index - 1].Job;
                    worksheet.Cell(index + 2, 4).Value = datas.P601PTDay[index - 1].Endtime;
                    worksheet.Cell(index + 2, 5).Value = datas.P601PTDay[index - 1].Plantime;
                    worksheet.Cell(index + 2, 6).Value = datas.P601PTDay[index - 1].PlanEndtime;
                    worksheet.Cell(index + 2, 7).Value = datas.P601PTDay[index - 1].Status;
                }

                for (int index = 1; index <= datas.P604PTDay.Count; index++)
                {
                    worksheet.Cell(index + 2, 11).Value = datas.P604PTDay[index - 1].Ws;
                    worksheet.Cell(index + 2, 12).Value = datas.P604PTDay[index - 1].Product;
                    worksheet.Cell(index + 2, 13).Value = datas.P604PTDay[index - 1].Job;
                    worksheet.Cell(index + 2, 14).Value = datas.P604PTDay[index - 1].Endtime;
                    worksheet.Cell(index + 2, 15).Value = datas.P604PTDay[index - 1].Plantime;
                    worksheet.Cell(index + 2, 16).Value = datas.P604PTDay[index - 1].PlanEndtime;
                    worksheet.Cell(index + 2, 17).Value = datas.P604PTDay[index - 1].Status;
                }

                for (int index = 1; index <= datas.P605PTDay.Count; index++)
                {
                    worksheet.Cell(index + 2, 21).Value = datas.P605PTDay[index - 1].Ws;
                    worksheet.Cell(index + 2, 22).Value = datas.P605PTDay[index - 1].Product;
                    worksheet.Cell(index + 2, 23).Value = datas.P605PTDay[index - 1].Job;
                    worksheet.Cell(index + 2, 24).Value = datas.P605PTDay[index - 1].Endtime;
                    worksheet.Cell(index + 2, 25).Value = datas.P605PTDay[index - 1].Plantime;
                    worksheet.Cell(index + 2, 26).Value = datas.P605PTDay[index - 1].PlanEndtime;
                    worksheet.Cell(index + 2, 27).Value = datas.P605PTDay[index - 1].Status;
                }

                for (int index = 1; index <= datas.P5mPTDay.Count; index++)
                {
                    worksheet.Cell(index + 2, 31).Value = datas.P5mPTDay[index - 1].Ws;
                    worksheet.Cell(index + 2, 32).Value = datas.P5mPTDay[index - 1].Product;
                    worksheet.Cell(index + 2, 33).Value = datas.P5mPTDay[index - 1].Job;
                    worksheet.Cell(index + 2, 34).Value = datas.P5mPTDay[index - 1].Endtime;
                    worksheet.Cell(index + 2, 35).Value = datas.P5mPTDay[index - 1].Plantime;
                    worksheet.Cell(index + 2, 36).Value = datas.P5mPTDay[index - 1].PlanEndtime;
                    worksheet.Cell(index + 2, 37).Value = datas.P5mPTDay[index - 1].Status;
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

            string fileName = sDate.ToString("yyyyMMdd_hhmmsstt") + "_Printing_Report_Week.csv";
            try
            {
                var datas = new PtPrintingWeekVM();
                datas = await _ptApiClient.GetPtPrintingWeek(tr);

                var workbook = new XLWorkbook();

                IXLWorksheet worksheet = workbook.Worksheets.Add("Week");
                worksheet.Cell(1, 1).Value = "Statistics";
                worksheet.Cell(1, 2).Value = "P601";
                worksheet.Cell(1, 3).Value = "P604";
                worksheet.Cell(1, 4).Value = "P605";
                worksheet.Cell(1, 5).Value = "P5m";

                worksheet.Cell(2, 1).Value = "Testing time";
                worksheet.Cell(2, 2).Value = datas.P601PTWeek.TestingTimeAndPercent;
                worksheet.Cell(2, 3).Value = datas.P604PTWeek.TestingTimeAndPercent;
                worksheet.Cell(2, 4).Value = datas.P605PTWeek.TestingTimeAndPercent;
                worksheet.Cell(2, 5).Value = datas.P5mPTWeek.TestingTimeAndPercent;

                worksheet.Cell(3, 1).Value = "Running time";
                worksheet.Cell(3, 2).Value = datas.P601PTWeek.RunningTimeAndPercent;
                worksheet.Cell(3, 3).Value = datas.P604PTWeek.RunningTimeAndPercent;
                worksheet.Cell(3, 4).Value = datas.P605PTWeek.RunningTimeAndPercent;
                worksheet.Cell(3, 5).Value = datas.P5mPTWeek.RunningTimeAndPercent;

                worksheet.Cell(4, 1).Value = "Not Running time";
                worksheet.Cell(4, 2).Value = datas.P601PTWeek.NotRunningTimeAndPercent;
                worksheet.Cell(4, 3).Value = datas.P604PTWeek.NotRunningTimeAndPercent;
                worksheet.Cell(4, 4).Value = datas.P605PTWeek.NotRunningTimeAndPercent;
                worksheet.Cell(4, 5).Value = datas.P5mPTWeek.NotRunningTimeAndPercent;

                worksheet.Cell(5, 1).Value = "No. Product Code Change";
                worksheet.Cell(5, 2).Value = datas.P601PTWeek.ProductCodeChangeCount;
                worksheet.Cell(5, 3).Value = datas.P604PTWeek.ProductCodeChangeCount;
                worksheet.Cell(5, 4).Value = datas.P605PTWeek.ProductCodeChangeCount;
                worksheet.Cell(5, 5).Value = datas.P5mPTWeek.ProductCodeChangeCount;

                worksheet.Cell(6, 1).Value = "Quantity";
                worksheet.Cell(6, 2).Value = datas.P601PTWeek.Quantity;
                worksheet.Cell(6, 3).Value = datas.P604PTWeek.Quantity;
                worksheet.Cell(6, 4).Value = datas.P605PTWeek.Quantity;
                worksheet.Cell(6, 5).Value = datas.P5mPTWeek.Quantity;

                worksheet.Cell(7, 1).Value = "Defect";
                worksheet.Cell(7, 2).Value = datas.P601PTWeek.Waste;
                worksheet.Cell(7, 3).Value = datas.P604PTWeek.Waste;
                worksheet.Cell(7, 4).Value = datas.P605PTWeek.Waste;
                worksheet.Cell(7, 5).Value = datas.P5mPTWeek.Waste;

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

            if (!monthOryear)
            {
                string fileName = sDate.ToString("yyyyMMdd_hhmmsstt") + "_Printing_Report_Month.csv";
                try
                {
                    var datas = new PtPrintingMonthYearVM();
                    datas = await _ptApiClient.GetPtPrintingMonth(tr);
                    var p601_avaibility_uptime = (datas.P601PTMonthYear.ActualWorkingTime == 0) ? 0 : Math.Round((decimal)((datas.P601PTMonthYear.ActualWorkingTime - datas.P601PTMonthYear.Downtime) * 100 / datas.P601PTMonthYear.ActualWorkingTime), 1);
                    var p601_capacityUtilization = (datas.P601PTMonthYear.MaximumProductionCapacity == 0) ? 0 : Math.Round((decimal)(datas.P601PTMonthYear.TotalOutput * 100 / datas.P601PTMonthYear.MaximumProductionCapacity), 1);
                    var p601_productionYield = (datas.P601PTMonthYear.TotalOutput == 0) ? 0 : Math.Round((decimal)((datas.P601PTMonthYear.TotalOutput - datas.P601PTMonthYear.Waste) * 100 / datas.P601PTMonthYear.TotalOutput), 1);
                    var p601_OEE = Math.Round(((p601_avaibility_uptime / 100) * (p601_capacityUtilization / 100) * (p601_productionYield / 100) * 100), 1);

                    var p604_avaibility_uptime = (datas.P604PTMonthYear.ActualWorkingTime == 0) ? 0 : Math.Round((decimal)((datas.P604PTMonthYear.ActualWorkingTime - datas.P604PTMonthYear.Downtime) * 100 / datas.P604PTMonthYear.ActualWorkingTime), 1);
                    var p604_capacityUtilization = (datas.P604PTMonthYear.MaximumProductionCapacity == 0) ? 0 : Math.Round((decimal)(datas.P604PTMonthYear.TotalOutput * 100 / datas.P604PTMonthYear.MaximumProductionCapacity), 1);
                    var p604_productionYield = (datas.P604PTMonthYear.TotalOutput == 0) ? 0 : Math.Round((decimal)((datas.P604PTMonthYear.TotalOutput - datas.P604PTMonthYear.Waste) * 100 / datas.P604PTMonthYear.TotalOutput), 1);
                    var p604_OEE = Math.Round(((p604_avaibility_uptime / 100) * (p604_capacityUtilization / 100) * (p604_productionYield / 100) * 100), 1);

                    var p605_avaibility_uptime = (datas.P605PTMonthYear.ActualWorkingTime == 0) ? 0 : Math.Round((decimal)((datas.P605PTMonthYear.ActualWorkingTime - datas.P605PTMonthYear.Downtime) * 100 / datas.P605PTMonthYear.ActualWorkingTime), 1);
                    var p605_capacityUtilization = (datas.P605PTMonthYear.MaximumProductionCapacity == 0) ? 0 : Math.Round((decimal)(datas.P605PTMonthYear.TotalOutput * 100 / datas.P605PTMonthYear.MaximumProductionCapacity), 1);
                    var p605_productionYield = (datas.P605PTMonthYear.TotalOutput == 0) ? 0 : Math.Round((decimal)((datas.P605PTMonthYear.TotalOutput - datas.P605PTMonthYear.Waste) * 100 / datas.P605PTMonthYear.TotalOutput), 1);
                    var p605_OEE = Math.Round(((p605_avaibility_uptime / 100) * (p605_capacityUtilization / 100) * (p605_productionYield / 100) * 100), 1);

                    var p5m_avaibility_uptime = (datas.P5mPTMonthYear.ActualWorkingTime == 0) ? 0 : Math.Round((decimal)((datas.P5mPTMonthYear.ActualWorkingTime - datas.P5mPTMonthYear.Downtime) * 100 / datas.P5mPTMonthYear.ActualWorkingTime), 1);
                    var p5m_capacityUtilization = (datas.P5mPTMonthYear.MaximumProductionCapacity == 0) ? 0 : Math.Round((decimal)(datas.P5mPTMonthYear.TotalOutput * 100 / datas.P5mPTMonthYear.MaximumProductionCapacity), 1);
                    var p5m_productionYield = (datas.P5mPTMonthYear.TotalOutput == 0) ? 0 : Math.Round((decimal)((datas.P5mPTMonthYear.TotalOutput - datas.P5mPTMonthYear.Waste) * 100 / datas.P5mPTMonthYear.TotalOutput), 1);
                    var p5m_OEE = Math.Round(((p5m_avaibility_uptime / 100) * (p5m_capacityUtilization / 100) * (p5m_productionYield / 100) * 100), 1);

                    var workbook = new XLWorkbook();

                    IXLWorksheet worksheet = workbook.Worksheets.Add("Month");
                    worksheet.Cell(1, 1).Value = "Statistics";
                    worksheet.Cell(1, 2).Value = "P601";
                    worksheet.Cell(1, 3).Value = "P604";
                    worksheet.Cell(1, 4).Value = "P605";
                    worksheet.Cell(1, 5).Value = "P5m";

                    worksheet.Cell(2, 1).Value = "Actual working time (h)";
                    worksheet.Cell(2, 2).Value = datas.P601PTMonthYear.ActualWorkingTime;
                    worksheet.Cell(2, 3).Value = datas.P604PTMonthYear.ActualWorkingTime;
                    worksheet.Cell(2, 4).Value = datas.P605PTMonthYear.ActualWorkingTime;
                    worksheet.Cell(2, 5).Value = datas.P5mPTMonthYear.ActualWorkingTime;

                    worksheet.Cell(3, 1).Value = "Downtime (h)";
                    worksheet.Cell(3, 2).Value = datas.P601PTMonthYear.Downtime;
                    worksheet.Cell(3, 3).Value = datas.P604PTMonthYear.Downtime;
                    worksheet.Cell(3, 4).Value = datas.P605PTMonthYear.Downtime;
                    worksheet.Cell(3, 5).Value = datas.P5mPTMonthYear.Downtime;

                    worksheet.Cell(4, 1).Value = "Total Quantity (pcs)";
                    worksheet.Cell(4, 2).Value = datas.P601PTMonthYear.TotalOutput;
                    worksheet.Cell(4, 3).Value = datas.P604PTMonthYear.TotalOutput;
                    worksheet.Cell(4, 4).Value = datas.P605PTMonthYear.TotalOutput;
                    worksheet.Cell(4, 5).Value = datas.P5mPTMonthYear.TotalOutput;

                    worksheet.Cell(5, 1).Value = "Maximum production capacity (pcs)";
                    worksheet.Cell(5, 2).Value = datas.P601PTMonthYear.MaximumProductionCapacity;
                    worksheet.Cell(5, 3).Value = datas.P604PTMonthYear.MaximumProductionCapacity;
                    worksheet.Cell(5, 4).Value = datas.P605PTMonthYear.MaximumProductionCapacity;
                    worksheet.Cell(5, 5).Value = datas.P5mPTMonthYear.MaximumProductionCapacity;

                    worksheet.Cell(6, 1).Value = "Total Defect (pcs)";
                    worksheet.Cell(6, 2).Value = datas.P601PTMonthYear.Waste;
                    worksheet.Cell(6, 3).Value = datas.P604PTMonthYear.Waste;
                    worksheet.Cell(6, 4).Value = datas.P605PTMonthYear.Waste;
                    worksheet.Cell(6, 5).Value = datas.P5mPTMonthYear.Waste;

                    worksheet.Cell(7, 1).Value = "Avaibility/Uptime (%)";
                    worksheet.Cell(7, 2).Value = p601_avaibility_uptime;
                    worksheet.Cell(7, 3).Value = p604_avaibility_uptime;
                    worksheet.Cell(7, 4).Value = p605_avaibility_uptime;
                    worksheet.Cell(7, 5).Value = p5m_avaibility_uptime;

                    worksheet.Cell(8, 1).Value = "Capacity utilization (%)";
                    worksheet.Cell(8, 2).Value = p601_capacityUtilization;
                    worksheet.Cell(8, 3).Value = p604_capacityUtilization;
                    worksheet.Cell(8, 4).Value = p605_capacityUtilization;
                    worksheet.Cell(8, 5).Value = p5m_capacityUtilization;

                    worksheet.Cell(9, 1).Value = "Production yield (%)";
                    worksheet.Cell(9, 2).Value = p601_productionYield;
                    worksheet.Cell(9, 3).Value = p604_productionYield;
                    worksheet.Cell(9, 4).Value = p605_productionYield;
                    worksheet.Cell(9, 5).Value = p5m_productionYield;

                    worksheet.Cell(10, 1).Value = "OEE (%)";
                    worksheet.Cell(10, 2).Value = p601_OEE;
                    worksheet.Cell(10, 3).Value = p604_OEE;
                    worksheet.Cell(10, 4).Value = p605_OEE;
                    worksheet.Cell(10, 5).Value = p5m_OEE;

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
                string fileName = sDate.ToString("yyyyMMdd_hhmmsstt") + "_Printing_Report_Year.csv";
                try
                {
                    var datas = new PtPrintingMonthYearVM();
                    datas = await _ptApiClient.GetPtPrintingYear(tr);
                    var p601_avaibility_uptime = (datas.P601PTMonthYear.ActualWorkingTime == 0) ? 0 : Math.Round((decimal)((datas.P601PTMonthYear.ActualWorkingTime - datas.P601PTMonthYear.Downtime) * 100 / datas.P601PTMonthYear.ActualWorkingTime), 1);
                    var p601_capacityUtilization = (datas.P601PTMonthYear.MaximumProductionCapacity == 0) ? 0 : Math.Round((decimal)(datas.P601PTMonthYear.TotalOutput * 100 / datas.P601PTMonthYear.MaximumProductionCapacity), 1);
                    var p601_productionYield = (datas.P601PTMonthYear.TotalOutput == 0) ? 0 : Math.Round((decimal)((datas.P601PTMonthYear.TotalOutput - datas.P601PTMonthYear.Waste) * 100 / datas.P601PTMonthYear.TotalOutput), 1);
                    var p601_OEE = Math.Round(((p601_avaibility_uptime / 100) * (p601_capacityUtilization / 100) * (p601_productionYield / 100) * 100), 1);

                    var p604_avaibility_uptime = (datas.P604PTMonthYear.ActualWorkingTime == 0) ? 0 : Math.Round((decimal)((datas.P604PTMonthYear.ActualWorkingTime - datas.P604PTMonthYear.Downtime) * 100 / datas.P604PTMonthYear.ActualWorkingTime), 1);
                    var p604_capacityUtilization = (datas.P604PTMonthYear.MaximumProductionCapacity == 0) ? 0 : Math.Round((decimal)(datas.P604PTMonthYear.TotalOutput * 100 / datas.P604PTMonthYear.MaximumProductionCapacity), 1);
                    var p604_productionYield = (datas.P604PTMonthYear.TotalOutput == 0) ? 0 : Math.Round((decimal)((datas.P604PTMonthYear.TotalOutput - datas.P604PTMonthYear.Waste) * 100 / datas.P604PTMonthYear.TotalOutput), 1);
                    var p604_OEE = Math.Round(((p604_avaibility_uptime / 100) * (p604_capacityUtilization / 100) * (p604_productionYield / 100) * 100), 1);

                    var p605_avaibility_uptime = (datas.P605PTMonthYear.ActualWorkingTime == 0) ? 0 : Math.Round((decimal)((datas.P605PTMonthYear.ActualWorkingTime - datas.P605PTMonthYear.Downtime) * 100 / datas.P605PTMonthYear.ActualWorkingTime), 1);
                    var p605_capacityUtilization = (datas.P605PTMonthYear.MaximumProductionCapacity == 0) ? 0 : Math.Round((decimal)(datas.P605PTMonthYear.TotalOutput * 100 / datas.P605PTMonthYear.MaximumProductionCapacity), 1);
                    var p605_productionYield = (datas.P605PTMonthYear.TotalOutput == 0) ? 0 : Math.Round((decimal)((datas.P605PTMonthYear.TotalOutput - datas.P605PTMonthYear.Waste) * 100 / datas.P605PTMonthYear.TotalOutput), 1);
                    var p605_OEE = Math.Round(((p605_avaibility_uptime / 100) * (p605_capacityUtilization / 100) * (p605_productionYield / 100) * 100), 1);

                    var p5m_avaibility_uptime = (datas.P5mPTMonthYear.ActualWorkingTime == 0) ? 0 : Math.Round((decimal)((datas.P5mPTMonthYear.ActualWorkingTime - datas.P5mPTMonthYear.Downtime) * 100 / datas.P5mPTMonthYear.ActualWorkingTime), 1);
                    var p5m_capacityUtilization = (datas.P5mPTMonthYear.MaximumProductionCapacity == 0) ? 0 : Math.Round((decimal)(datas.P5mPTMonthYear.TotalOutput * 100 / datas.P5mPTMonthYear.MaximumProductionCapacity), 1);
                    var p5m_productionYield = (datas.P5mPTMonthYear.TotalOutput == 0) ? 0 : Math.Round((decimal)((datas.P5mPTMonthYear.TotalOutput - datas.P5mPTMonthYear.Waste) * 100 / datas.P5mPTMonthYear.TotalOutput), 1);
                    var p5m_OEE = Math.Round(((p5m_avaibility_uptime / 100) * (p5m_capacityUtilization / 100) * (p5m_productionYield / 100) * 100), 1);

                    var workbook = new XLWorkbook();

                    IXLWorksheet worksheet = workbook.Worksheets.Add("Year");
                    worksheet.Cell(1, 1).Value = "Statistics";
                    worksheet.Cell(1, 2).Value = "P601";
                    worksheet.Cell(1, 3).Value = "P604";
                    worksheet.Cell(1, 4).Value = "P605";
                    worksheet.Cell(1, 5).Value = "P5m";

                    worksheet.Cell(2, 1).Value = "Actual working time (h)";
                    worksheet.Cell(2, 2).Value = datas.P601PTMonthYear.ActualWorkingTime;
                    worksheet.Cell(2, 3).Value = datas.P604PTMonthYear.ActualWorkingTime;
                    worksheet.Cell(2, 4).Value = datas.P605PTMonthYear.ActualWorkingTime;
                    worksheet.Cell(2, 5).Value = datas.P5mPTMonthYear.ActualWorkingTime;

                    worksheet.Cell(3, 1).Value = "Downtime (h)";
                    worksheet.Cell(3, 2).Value = datas.P601PTMonthYear.Downtime;
                    worksheet.Cell(3, 3).Value = datas.P604PTMonthYear.Downtime;
                    worksheet.Cell(3, 4).Value = datas.P605PTMonthYear.Downtime;
                    worksheet.Cell(3, 5).Value = datas.P5mPTMonthYear.Downtime;

                    worksheet.Cell(4, 1).Value = "Total Quantity (pcs)";
                    worksheet.Cell(4, 2).Value = datas.P601PTMonthYear.TotalOutput;
                    worksheet.Cell(4, 3).Value = datas.P604PTMonthYear.TotalOutput;
                    worksheet.Cell(4, 4).Value = datas.P605PTMonthYear.TotalOutput;
                    worksheet.Cell(4, 5).Value = datas.P5mPTMonthYear.TotalOutput;

                    worksheet.Cell(5, 1).Value = "Maximum production capacity (pcs)";
                    worksheet.Cell(5, 2).Value = datas.P601PTMonthYear.MaximumProductionCapacity;
                    worksheet.Cell(5, 3).Value = datas.P604PTMonthYear.MaximumProductionCapacity;
                    worksheet.Cell(5, 4).Value = datas.P605PTMonthYear.MaximumProductionCapacity;
                    worksheet.Cell(5, 5).Value = datas.P5mPTMonthYear.MaximumProductionCapacity;

                    worksheet.Cell(6, 1).Value = "Total Defect (pcs)";
                    worksheet.Cell(6, 2).Value = datas.P601PTMonthYear.Waste;
                    worksheet.Cell(6, 3).Value = datas.P604PTMonthYear.Waste;
                    worksheet.Cell(6, 4).Value = datas.P605PTMonthYear.Waste;
                    worksheet.Cell(6, 5).Value = datas.P5mPTMonthYear.Waste;

                    worksheet.Cell(7, 1).Value = "Avaibility/Uptime (%)";
                    worksheet.Cell(7, 2).Value = p601_avaibility_uptime;
                    worksheet.Cell(7, 3).Value = p604_avaibility_uptime;
                    worksheet.Cell(7, 4).Value = p605_avaibility_uptime;
                    worksheet.Cell(7, 5).Value = p5m_avaibility_uptime;

                    worksheet.Cell(8, 1).Value = "Capacity utilization (%)";
                    worksheet.Cell(8, 2).Value = p601_capacityUtilization;
                    worksheet.Cell(8, 3).Value = p604_capacityUtilization;
                    worksheet.Cell(8, 4).Value = p605_capacityUtilization;
                    worksheet.Cell(8, 5).Value = p5m_capacityUtilization;

                    worksheet.Cell(9, 1).Value = "Production yield (%)";
                    worksheet.Cell(9, 2).Value = p601_productionYield;
                    worksheet.Cell(9, 3).Value = p604_productionYield;
                    worksheet.Cell(9, 4).Value = p605_productionYield;
                    worksheet.Cell(9, 5).Value = p5m_productionYield;

                    worksheet.Cell(10, 1).Value = "OEE (%)";
                    worksheet.Cell(10, 2).Value = p601_OEE;
                    worksheet.Cell(10, 3).Value = p604_OEE;
                    worksheet.Cell(10, 4).Value = p605_OEE;
                    worksheet.Cell(10, 5).Value = p5m_OEE;

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

            string fileName = sDate.ToString("yyyyMMdd_hhmmsstt") + "_Printing_Report.csv";

            try
            {
                var workbook = new XLWorkbook();

                #region 1H
                var timeRequest = new TimeRequest();               

                timeRequest.To = sDate;
                timeRequest.From = sDate.AddHours(-1);

                var datas1H = new PtPrinting1HVM();
                datas1H = await _ptApiClient.GetPtPrinting1H(timeRequest);
                IXLWorksheet ws1H = workbook.Worksheets.Add("1 Hour");
                ws1H.Cell(1, 1).Value = "P601";
                ws1H.Cell(1, 8).Value = "P604";
                ws1H.Cell(1, 15).Value = "P605";
                ws1H.Cell(1, 22).Value = "P5m";

                ws1H.Cell(2, 1).Value = ws1H.Cell(2, 8).Value = ws1H.Cell(2, 15).Value = ws1H.Cell(2, 22).Value = "Worksheet";
                ws1H.Cell(2, 2).Value = ws1H.Cell(2, 9).Value = ws1H.Cell(2, 16).Value = ws1H.Cell(2, 23).Value = "ProductCode";
                ws1H.Cell(2, 3).Value = ws1H.Cell(2, 10).Value = ws1H.Cell(2, 17).Value = ws1H.Cell(2, 24).Value = "Quantity";
                ws1H.Cell(2, 4).Value = ws1H.Cell(2, 11).Value = ws1H.Cell(2, 18).Value = ws1H.Cell(2, 25).Value = "Target";

                for (int index = 1; index <= datas1H.P601PT1H.Count; index++)
                {
                    ws1H.Cell(index + 2, 1).Value = datas1H.P601PT1H[index - 1].Ws;
                    ws1H.Cell(index + 2, 2).Value = datas1H.P601PT1H[index - 1].Product;
                    ws1H.Cell(index + 2, 3).Value = datas1H.P601PT1H[index - 1].Job;
                    ws1H.Cell(index + 2, 4).Value = datas1H.P601PT1H[index - 1].Target;
                }

                for (int index = 1; index <= datas1H.P604PT1H.Count; index++)
                {
                    ws1H.Cell(index + 2, 8).Value = datas1H.P604PT1H[index - 1].Ws;
                    ws1H.Cell(index + 2, 9).Value = datas1H.P604PT1H[index - 1].Product;
                    ws1H.Cell(index + 2, 10).Value = datas1H.P604PT1H[index - 1].Job;
                    ws1H.Cell(index + 2, 11).Value = datas1H.P604PT1H[index - 1].Target;
                }

                for (int index = 1; index <= datas1H.P605PT1H.Count; index++)
                {
                    ws1H.Cell(index + 2, 15).Value = datas1H.P605PT1H[index - 1].Ws;
                    ws1H.Cell(index + 2, 16).Value = datas1H.P605PT1H[index - 1].Product;
                    ws1H.Cell(index + 2, 17).Value = datas1H.P605PT1H[index - 1].Job;
                    ws1H.Cell(index + 2, 18).Value = datas1H.P605PT1H[index - 1].Target;
                }

                for (int index = 1; index <= datas1H.P5mPT1H.Count; index++)
                {
                    ws1H.Cell(index + 2, 22).Value = datas1H.P5mPT1H[index - 1].Ws;
                    ws1H.Cell(index + 2, 23).Value = datas1H.P5mPT1H[index - 1].Product;
                    ws1H.Cell(index + 2, 24).Value = datas1H.P5mPT1H[index - 1].Job;
                    ws1H.Cell(index + 2, 25).Value = datas1H.P5mPT1H[index - 1].Target;
                }
                #endregion
                #region Day
                timeRequest.To = sDate;
                timeRequest.From = sDate.AddDays(-1);

                var datasDay = new PtPrintingDayVM();
                datasDay = await _ptApiClient.GetPtPrintingDay(timeRequest);

                IXLWorksheet wsDay = workbook.Worksheets.Add("Day");
                wsDay.Cell(1, 1).Value = "P601";
                wsDay.Cell(1, 11).Value = "P604";
                wsDay.Cell(1, 21).Value = "P605";
                wsDay.Cell(1, 31).Value = "P5m";

                wsDay.Cell(2, 1).Value = wsDay.Cell(2, 11).Value = wsDay.Cell(2, 21).Value = wsDay.Cell(2, 31).Value = "Worksheet";
                wsDay.Cell(2, 2).Value = wsDay.Cell(2, 12).Value = wsDay.Cell(2, 22).Value = wsDay.Cell(2, 32).Value = "ProductCode";
                wsDay.Cell(2, 3).Value = wsDay.Cell(2, 13).Value = wsDay.Cell(2, 23).Value = wsDay.Cell(2, 33).Value = "Quantity";
                wsDay.Cell(2, 4).Value = wsDay.Cell(2, 14).Value = wsDay.Cell(2, 24).Value = wsDay.Cell(2, 34).Value = "CompleteTime";
                wsDay.Cell(2, 5).Value = wsDay.Cell(2, 15).Value = wsDay.Cell(2, 25).Value = wsDay.Cell(2, 35).Value = "PlanTime";
                wsDay.Cell(2, 6).Value = wsDay.Cell(2, 16).Value = wsDay.Cell(2, 26).Value = wsDay.Cell(2, 36).Value = "PlanEndTime";
                wsDay.Cell(2, 7).Value = wsDay.Cell(2, 17).Value = wsDay.Cell(2, 27).Value = wsDay.Cell(2, 37).Value = "Status";

                for (int index = 1; index <= datasDay.P601PTDay.Count; index++)
                {
                    wsDay.Cell(index + 2, 1).Value = datasDay.P601PTDay[index - 1].Ws;
                    wsDay.Cell(index + 2, 2).Value = datasDay.P601PTDay[index - 1].Product;
                    wsDay.Cell(index + 2, 3).Value = datasDay.P601PTDay[index - 1].Job;
                    wsDay.Cell(index + 2, 4).Value = datasDay.P601PTDay[index - 1].Endtime;
                    wsDay.Cell(index + 2, 5).Value = datasDay.P601PTDay[index - 1].Plantime;
                    wsDay.Cell(index + 2, 6).Value = datasDay.P601PTDay[index - 1].PlanEndtime;
                    wsDay.Cell(index + 2, 7).Value = datasDay.P601PTDay[index - 1].Status;
                }

                for (int index = 1; index <= datasDay.P604PTDay.Count; index++)
                {
                    wsDay.Cell(index + 2, 11).Value = datasDay.P604PTDay[index - 1].Ws;
                    wsDay.Cell(index + 2, 12).Value = datasDay.P604PTDay[index - 1].Product;
                    wsDay.Cell(index + 2, 13).Value = datasDay.P604PTDay[index - 1].Job;
                    wsDay.Cell(index + 2, 14).Value = datasDay.P604PTDay[index - 1].Endtime;
                    wsDay.Cell(index + 2, 15).Value = datasDay.P604PTDay[index - 1].Plantime;
                    wsDay.Cell(index + 2, 16).Value = datasDay.P604PTDay[index - 1].PlanEndtime;
                    wsDay.Cell(index + 2, 17).Value = datasDay.P604PTDay[index - 1].Status;
                }

                for (int index = 1; index <= datasDay.P605PTDay.Count; index++)
                {
                    wsDay.Cell(index + 2, 21).Value = datasDay.P605PTDay[index - 1].Ws;
                    wsDay.Cell(index + 2, 22).Value = datasDay.P605PTDay[index - 1].Product;
                    wsDay.Cell(index + 2, 23).Value = datasDay.P605PTDay[index - 1].Job;
                    wsDay.Cell(index + 2, 24).Value = datasDay.P605PTDay[index - 1].Endtime;
                    wsDay.Cell(index + 2, 25).Value = datasDay.P605PTDay[index - 1].Plantime;
                    wsDay.Cell(index + 2, 26).Value = datasDay.P605PTDay[index - 1].PlanEndtime;
                    wsDay.Cell(index + 2, 27).Value = datasDay.P605PTDay[index - 1].Status;
                }

                for (int index = 1; index <= datasDay.P5mPTDay.Count; index++)
                {
                    wsDay.Cell(index + 2, 31).Value = datasDay.P5mPTDay[index - 1].Ws;
                    wsDay.Cell(index + 2, 32).Value = datasDay.P5mPTDay[index - 1].Product;
                    wsDay.Cell(index + 2, 33).Value = datasDay.P5mPTDay[index - 1].Job;
                    wsDay.Cell(index + 2, 34).Value = datasDay.P5mPTDay[index - 1].Endtime;
                    wsDay.Cell(index + 2, 35).Value = datasDay.P5mPTDay[index - 1].Plantime;
                    wsDay.Cell(index + 2, 36).Value = datasDay.P5mPTDay[index - 1].PlanEndtime;
                    wsDay.Cell(index + 2, 37).Value = datasDay.P5mPTDay[index - 1].Status;
                }
                #endregion
                #region Week
                timeRequest.To = StartOfWeek(sDate, DayOfWeek.Monday).AddDays(7);
                timeRequest.From = StartOfWeek(sDate, DayOfWeek.Monday);

                var datas = new PtPrintingWeekVM();
                datas = await _ptApiClient.GetPtPrintingWeek(timeRequest);

                IXLWorksheet wsWeek = workbook.Worksheets.Add("Week");
                wsWeek.Cell(1, 1).Value = "Statistics";
                wsWeek.Cell(1, 2).Value = "P601";
                wsWeek.Cell(1, 3).Value = "P604";
                wsWeek.Cell(1, 4).Value = "P605";
                wsWeek.Cell(1, 5).Value = "P5m";

                wsWeek.Cell(2, 1).Value = "Testing time";
                wsWeek.Cell(2, 2).Value = datas.P601PTWeek.TestingTimeAndPercent;
                wsWeek.Cell(2, 3).Value = datas.P604PTWeek.TestingTimeAndPercent;
                wsWeek.Cell(2, 4).Value = datas.P605PTWeek.TestingTimeAndPercent;
                wsWeek.Cell(2, 5).Value = datas.P5mPTWeek.TestingTimeAndPercent;

                wsWeek.Cell(3, 1).Value = "Running time";
                wsWeek.Cell(3, 2).Value = datas.P601PTWeek.RunningTimeAndPercent;
                wsWeek.Cell(3, 3).Value = datas.P604PTWeek.RunningTimeAndPercent;
                wsWeek.Cell(3, 4).Value = datas.P605PTWeek.RunningTimeAndPercent;
                wsWeek.Cell(3, 5).Value = datas.P5mPTWeek.RunningTimeAndPercent;

                wsWeek.Cell(4, 1).Value = "Not Running time";
                wsWeek.Cell(4, 2).Value = datas.P601PTWeek.NotRunningTimeAndPercent;
                wsWeek.Cell(4, 3).Value = datas.P604PTWeek.NotRunningTimeAndPercent;
                wsWeek.Cell(4, 4).Value = datas.P605PTWeek.NotRunningTimeAndPercent;
                wsWeek.Cell(4, 5).Value = datas.P5mPTWeek.NotRunningTimeAndPercent;

                wsWeek.Cell(5, 1).Value = "No. Product Code Change";
                wsWeek.Cell(5, 2).Value = datas.P601PTWeek.ProductCodeChangeCount;
                wsWeek.Cell(5, 3).Value = datas.P604PTWeek.ProductCodeChangeCount;
                wsWeek.Cell(5, 4).Value = datas.P605PTWeek.ProductCodeChangeCount;
                wsWeek.Cell(5, 5).Value = datas.P5mPTWeek.ProductCodeChangeCount;

                wsWeek.Cell(6, 1).Value = "Quantity";
                wsWeek.Cell(6, 2).Value = datas.P601PTWeek.Quantity;
                wsWeek.Cell(6, 3).Value = datas.P604PTWeek.Quantity;
                wsWeek.Cell(6, 4).Value = datas.P605PTWeek.Quantity;
                wsWeek.Cell(6, 5).Value = datas.P5mPTWeek.Quantity;

                wsWeek.Cell(7, 1).Value = "Defect";
                wsWeek.Cell(7, 2).Value = datas.P601PTWeek.Waste;
                wsWeek.Cell(7, 3).Value = datas.P604PTWeek.Waste;
                wsWeek.Cell(7, 4).Value = datas.P605PTWeek.Waste;
                wsWeek.Cell(7, 5).Value = datas.P5mPTWeek.Waste;
                #endregion
                #region Month
                timeRequest.To = new DateTime(sDate.Year, sDate.AddMonths(1).Month, 1);
                timeRequest.From = new DateTime(sDate.Year, sDate.Month, 1);

                var datasMonth = new PtPrintingMonthYearVM();
                datasMonth = await _ptApiClient.GetPtPrintingMonth(timeRequest);
                var p601_avaibility_uptime = (datasMonth.P601PTMonthYear.ActualWorkingTime == 0) ? 0 : Math.Round((decimal)((datasMonth.P601PTMonthYear.ActualWorkingTime - datasMonth.P601PTMonthYear.Downtime) * 100 / datasMonth.P601PTMonthYear.ActualWorkingTime), 1);
                var p601_capacityUtilization = (datasMonth.P601PTMonthYear.MaximumProductionCapacity == 0) ? 0 : Math.Round((decimal)(datasMonth.P601PTMonthYear.TotalOutput * 100 / datasMonth.P601PTMonthYear.MaximumProductionCapacity), 1);
                var p601_productionYield = (datasMonth.P601PTMonthYear.TotalOutput == 0) ? 0 : Math.Round((decimal)((datasMonth.P601PTMonthYear.TotalOutput - datasMonth.P601PTMonthYear.Waste) * 100 / datasMonth.P601PTMonthYear.TotalOutput), 1);
                var p601_OEE = Math.Round(((p601_avaibility_uptime / 100) * (p601_capacityUtilization / 100) * (p601_productionYield / 100) * 100), 1);

                var p604_avaibility_uptime = (datasMonth.P604PTMonthYear.ActualWorkingTime == 0) ? 0 : Math.Round((decimal)((datasMonth.P604PTMonthYear.ActualWorkingTime - datasMonth.P604PTMonthYear.Downtime) * 100 / datasMonth.P604PTMonthYear.ActualWorkingTime), 1);
                var p604_capacityUtilization = (datasMonth.P604PTMonthYear.MaximumProductionCapacity == 0) ? 0 : Math.Round((decimal)(datasMonth.P604PTMonthYear.TotalOutput * 100 / datasMonth.P604PTMonthYear.MaximumProductionCapacity), 1);
                var p604_productionYield = (datasMonth.P604PTMonthYear.TotalOutput == 0) ? 0 : Math.Round((decimal)((datasMonth.P604PTMonthYear.TotalOutput - datasMonth.P604PTMonthYear.Waste) * 100 / datasMonth.P604PTMonthYear.TotalOutput), 1);
                var p604_OEE = Math.Round(((p604_avaibility_uptime / 100) * (p604_capacityUtilization / 100) * (p604_productionYield / 100) * 100), 1);

                var p605_avaibility_uptime = (datasMonth.P605PTMonthYear.ActualWorkingTime == 0) ? 0 : Math.Round((decimal)((datasMonth.P605PTMonthYear.ActualWorkingTime - datasMonth.P605PTMonthYear.Downtime) * 100 / datasMonth.P605PTMonthYear.ActualWorkingTime), 1);
                var p605_capacityUtilization = (datasMonth.P605PTMonthYear.MaximumProductionCapacity == 0) ? 0 : Math.Round((decimal)(datasMonth.P605PTMonthYear.TotalOutput * 100 / datasMonth.P605PTMonthYear.MaximumProductionCapacity), 1);
                var p605_productionYield = (datasMonth.P605PTMonthYear.TotalOutput == 0) ? 0 : Math.Round((decimal)((datasMonth.P605PTMonthYear.TotalOutput - datasMonth.P605PTMonthYear.Waste) * 100 / datasMonth.P605PTMonthYear.TotalOutput), 1);
                var p605_OEE = Math.Round(((p605_avaibility_uptime / 100) * (p605_capacityUtilization / 100) * (p605_productionYield / 100) * 100), 1);

                var p5m_avaibility_uptime = (datasMonth.P5mPTMonthYear.ActualWorkingTime == 0) ? 0 : Math.Round((decimal)((datasMonth.P5mPTMonthYear.ActualWorkingTime - datasMonth.P5mPTMonthYear.Downtime) * 100 / datasMonth.P5mPTMonthYear.ActualWorkingTime), 1);
                var p5m_capacityUtilization = (datasMonth.P5mPTMonthYear.MaximumProductionCapacity == 0) ? 0 : Math.Round((decimal)(datasMonth.P5mPTMonthYear.TotalOutput * 100 / datasMonth.P5mPTMonthYear.MaximumProductionCapacity), 1);
                var p5m_productionYield = (datasMonth.P5mPTMonthYear.TotalOutput == 0) ? 0 : Math.Round((decimal)((datasMonth.P5mPTMonthYear.TotalOutput - datasMonth.P5mPTMonthYear.Waste) * 100 / datasMonth.P5mPTMonthYear.TotalOutput), 1);
                var p5m_OEE = Math.Round(((p5m_avaibility_uptime / 100) * (p5m_capacityUtilization / 100) * (p5m_productionYield / 100) * 100), 1);

                IXLWorksheet wsMonth = workbook.Worksheets.Add("Month");
                wsMonth.Cell(1, 1).Value = "Statistics";
                wsMonth.Cell(1, 2).Value = "P601";
                wsMonth.Cell(1, 3).Value = "P604";
                wsMonth.Cell(1, 4).Value = "P605";
                wsMonth.Cell(1, 5).Value = "P5m";

                wsMonth.Cell(2, 1).Value = "Actual working time (h)";
                wsMonth.Cell(2, 2).Value = datasMonth.P601PTMonthYear.ActualWorkingTime;
                wsMonth.Cell(2, 3).Value = datasMonth.P604PTMonthYear.ActualWorkingTime;
                wsMonth.Cell(2, 4).Value = datasMonth.P605PTMonthYear.ActualWorkingTime;
                wsMonth.Cell(2, 5).Value = datasMonth.P5mPTMonthYear.ActualWorkingTime;

                wsMonth.Cell(3, 1).Value = "Downtime (h)";
                wsMonth.Cell(3, 2).Value = datasMonth.P601PTMonthYear.Downtime;
                wsMonth.Cell(3, 3).Value = datasMonth.P604PTMonthYear.Downtime;
                wsMonth.Cell(3, 4).Value = datasMonth.P605PTMonthYear.Downtime;
                wsMonth.Cell(3, 5).Value = datasMonth.P5mPTMonthYear.Downtime;

                wsMonth.Cell(4, 1).Value = "Total Quantity (pcs)";
                wsMonth.Cell(4, 2).Value = datasMonth.P601PTMonthYear.TotalOutput;
                wsMonth.Cell(4, 3).Value = datasMonth.P604PTMonthYear.TotalOutput;
                wsMonth.Cell(4, 4).Value = datasMonth.P605PTMonthYear.TotalOutput;
                wsMonth.Cell(4, 5).Value = datasMonth.P5mPTMonthYear.TotalOutput;

                wsMonth.Cell(5, 1).Value = "Maximum production capacity (pcs)";
                wsMonth.Cell(5, 2).Value = datasMonth.P601PTMonthYear.MaximumProductionCapacity;
                wsMonth.Cell(5, 3).Value = datasMonth.P604PTMonthYear.MaximumProductionCapacity;
                wsMonth.Cell(5, 4).Value = datasMonth.P605PTMonthYear.MaximumProductionCapacity;
                wsMonth.Cell(5, 5).Value = datasMonth.P5mPTMonthYear.MaximumProductionCapacity;

                wsMonth.Cell(6, 1).Value = "Total Defect (pcs)";
                wsMonth.Cell(6, 2).Value = datasMonth.P601PTMonthYear.Waste;
                wsMonth.Cell(6, 3).Value = datasMonth.P604PTMonthYear.Waste;
                wsMonth.Cell(6, 4).Value = datasMonth.P605PTMonthYear.Waste;
                wsMonth.Cell(6, 5).Value = datasMonth.P5mPTMonthYear.Waste;

                wsMonth.Cell(7, 1).Value = "Avaibility/Uptime (%)";
                wsMonth.Cell(7, 2).Value = p601_avaibility_uptime;
                wsMonth.Cell(7, 3).Value = p604_avaibility_uptime;
                wsMonth.Cell(7, 4).Value = p605_avaibility_uptime;
                wsMonth.Cell(7, 5).Value = p5m_avaibility_uptime;

                wsMonth.Cell(8, 1).Value = "Capacity utilization (%)";
                wsMonth.Cell(8, 2).Value = p601_capacityUtilization;
                wsMonth.Cell(8, 3).Value = p604_capacityUtilization;
                wsMonth.Cell(8, 4).Value = p605_capacityUtilization;
                wsMonth.Cell(8, 5).Value = p5m_capacityUtilization;

                wsMonth.Cell(9, 1).Value = "Production yield (%)";
                wsMonth.Cell(9, 2).Value = p601_productionYield;
                wsMonth.Cell(9, 3).Value = p604_productionYield;
                wsMonth.Cell(9, 4).Value = p605_productionYield;
                wsMonth.Cell(9, 5).Value = p5m_productionYield;

                wsMonth.Cell(10, 1).Value = "OEE (%)";
                wsMonth.Cell(10, 2).Value = p601_OEE;
                wsMonth.Cell(10, 3).Value = p604_OEE;
                wsMonth.Cell(10, 4).Value = p605_OEE;
                wsMonth.Cell(10, 5).Value = p5m_OEE;
                #endregion
                #region Year
                timeRequest.To = new DateTime(sDate.AddYears(1).Year, 1, 1);
                timeRequest.From = new DateTime(sDate.Year, 1, 1);

                var datasYear = new PtPrintingMonthYearVM();
                datasYear = await _ptApiClient.GetPtPrintingYear(timeRequest);
                var p601_avaibility_uptime_y = (datasYear.P601PTMonthYear.ActualWorkingTime == 0) ? 0 : Math.Round((decimal)((datasYear.P601PTMonthYear.ActualWorkingTime - datasYear.P601PTMonthYear.Downtime) * 100 / datasYear.P601PTMonthYear.ActualWorkingTime), 1);
                var p601_capacityUtilization_y = (datasYear.P601PTMonthYear.MaximumProductionCapacity == 0) ? 0 : Math.Round((decimal)(datasYear.P601PTMonthYear.TotalOutput * 100 / datasYear.P601PTMonthYear.MaximumProductionCapacity), 1);
                var p601_productionYield_y = (datasYear.P601PTMonthYear.TotalOutput == 0) ? 0 : Math.Round((decimal)((datasYear.P601PTMonthYear.TotalOutput - datasYear.P601PTMonthYear.Waste) * 100 / datasYear.P601PTMonthYear.TotalOutput), 1);
                var p601_OEE_y = Math.Round(((p601_avaibility_uptime_y / 100) * (p601_capacityUtilization_y / 100) * (p601_productionYield_y / 100) * 100), 1);

                var p604_avaibility_uptime_y = (datasYear.P604PTMonthYear.ActualWorkingTime == 0) ? 0 : Math.Round((decimal)((datasYear.P604PTMonthYear.ActualWorkingTime - datasYear.P604PTMonthYear.Downtime) * 100 / datasYear.P604PTMonthYear.ActualWorkingTime), 1);
                var p604_capacityUtilization_y = (datasYear.P604PTMonthYear.MaximumProductionCapacity == 0) ? 0 : Math.Round((decimal)(datasYear.P604PTMonthYear.TotalOutput * 100 / datasYear.P604PTMonthYear.MaximumProductionCapacity), 1);
                var p604_productionYield_y = (datasYear.P604PTMonthYear.TotalOutput == 0) ? 0 : Math.Round((decimal)((datasYear.P604PTMonthYear.TotalOutput - datasYear.P604PTMonthYear.Waste) * 100 / datasYear.P604PTMonthYear.TotalOutput), 1);
                var p604_OEE_y = Math.Round(((p604_avaibility_uptime_y / 100) * (p604_capacityUtilization_y / 100) * (p604_productionYield_y / 100) * 100), 1);

                var p605_avaibility_uptime_y = (datasYear.P605PTMonthYear.ActualWorkingTime == 0) ? 0 : Math.Round((decimal)((datasYear.P605PTMonthYear.ActualWorkingTime - datasYear.P605PTMonthYear.Downtime) * 100 / datasYear.P605PTMonthYear.ActualWorkingTime), 1);
                var p605_capacityUtilization_y = (datasYear.P605PTMonthYear.MaximumProductionCapacity == 0) ? 0 : Math.Round((decimal)(datasYear.P605PTMonthYear.TotalOutput * 100 / datasYear.P605PTMonthYear.MaximumProductionCapacity), 1);
                var p605_productionYield_y = (datasYear.P605PTMonthYear.TotalOutput == 0) ? 0 : Math.Round((decimal)((datasYear.P605PTMonthYear.TotalOutput - datasYear.P605PTMonthYear.Waste) * 100 / datasYear.P605PTMonthYear.TotalOutput), 1);
                var p605_OEE_y = Math.Round(((p605_avaibility_uptime_y / 100) * (p605_capacityUtilization_y / 100) * (p605_productionYield_y / 100) * 100), 1);

                var p5m_avaibility_uptime_y = (datasYear.P5mPTMonthYear.ActualWorkingTime == 0) ? 0 : Math.Round((decimal)((datasYear.P5mPTMonthYear.ActualWorkingTime - datasYear.P5mPTMonthYear.Downtime) * 100 / datasYear.P5mPTMonthYear.ActualWorkingTime), 1);
                var p5m_capacityUtilization_y = (datasYear.P5mPTMonthYear.MaximumProductionCapacity == 0) ? 0 : Math.Round((decimal)(datasYear.P5mPTMonthYear.TotalOutput * 100 / datasYear.P5mPTMonthYear.MaximumProductionCapacity), 1);
                var p5m_productionYield_y = (datasYear.P5mPTMonthYear.TotalOutput == 0) ? 0 : Math.Round((decimal)((datasYear.P5mPTMonthYear.TotalOutput - datasYear.P5mPTMonthYear.Waste) * 100 / datasYear.P5mPTMonthYear.TotalOutput), 1);
                var p5m_OEE_y = Math.Round(((p5m_avaibility_uptime_y / 100) * (p5m_capacityUtilization_y / 100) * (p5m_productionYield_y / 100) * 100), 1);

                IXLWorksheet wsYear = workbook.Worksheets.Add("Year");

                wsYear.Cell(1, 1).Value = "Statistics";
                wsYear.Cell(1, 2).Value = "P601";
                wsYear.Cell(1, 3).Value = "P604";
                wsYear.Cell(1, 4).Value = "P605";
                wsYear.Cell(1, 5).Value = "P5m";

                wsYear.Cell(2, 1).Value = "Actual working time (h)";
                wsYear.Cell(2, 2).Value = datasYear.P601PTMonthYear.ActualWorkingTime;
                wsYear.Cell(2, 3).Value = datasYear.P604PTMonthYear.ActualWorkingTime;
                wsYear.Cell(2, 4).Value = datasYear.P605PTMonthYear.ActualWorkingTime;
                wsYear.Cell(2, 5).Value = datasYear.P5mPTMonthYear.ActualWorkingTime;

                wsYear.Cell(3, 1).Value = "Downtime (h)";
                wsYear.Cell(3, 2).Value = datasYear.P601PTMonthYear.Downtime;
                wsYear.Cell(3, 3).Value = datasYear.P604PTMonthYear.Downtime;
                wsYear.Cell(3, 4).Value = datasYear.P605PTMonthYear.Downtime;
                wsYear.Cell(3, 5).Value = datasYear.P5mPTMonthYear.Downtime;

                wsYear.Cell(4, 1).Value = "Total Quantity (pcs)";
                wsYear.Cell(4, 2).Value = datasYear.P601PTMonthYear.TotalOutput;
                wsYear.Cell(4, 3).Value = datasYear.P604PTMonthYear.TotalOutput;
                wsYear.Cell(4, 4).Value = datasYear.P605PTMonthYear.TotalOutput;
                wsYear.Cell(4, 5).Value = datasYear.P5mPTMonthYear.TotalOutput;

                wsYear.Cell(5, 1).Value = "Maximum production capacity (pcs)";
                wsYear.Cell(5, 2).Value = datasYear.P601PTMonthYear.MaximumProductionCapacity;
                wsYear.Cell(5, 3).Value = datasYear.P604PTMonthYear.MaximumProductionCapacity;
                wsYear.Cell(5, 4).Value = datasYear.P605PTMonthYear.MaximumProductionCapacity;
                wsYear.Cell(5, 5).Value = datasYear.P5mPTMonthYear.MaximumProductionCapacity;

                wsYear.Cell(6, 1).Value = "Total Defect (pcs)";
                wsYear.Cell(6, 2).Value = datasYear.P601PTMonthYear.Waste;
                wsYear.Cell(6, 3).Value = datasYear.P604PTMonthYear.Waste;
                wsYear.Cell(6, 4).Value = datasYear.P605PTMonthYear.Waste;
                wsYear.Cell(6, 5).Value = datasYear.P5mPTMonthYear.Waste;

                wsYear.Cell(7, 1).Value = "Avaibility/Uptime (%)";
                wsYear.Cell(7, 2).Value = p601_avaibility_uptime_y;
                wsYear.Cell(7, 3).Value = p604_avaibility_uptime_y;
                wsYear.Cell(7, 4).Value = p605_avaibility_uptime_y;
                wsYear.Cell(7, 5).Value = p5m_avaibility_uptime_y;

                wsYear.Cell(8, 1).Value = "Capacity utilization (%)";
                wsYear.Cell(8, 2).Value = p601_capacityUtilization_y;
                wsYear.Cell(8, 3).Value = p604_capacityUtilization_y;
                wsYear.Cell(8, 4).Value = p605_capacityUtilization_y;
                wsYear.Cell(8, 5).Value = p5m_capacityUtilization_y;

                wsYear.Cell(9, 1).Value = "Production yield (%)";
                wsYear.Cell(9, 2).Value = p601_productionYield_y;
                wsYear.Cell(9, 3).Value = p604_productionYield_y;
                wsYear.Cell(9, 4).Value = p605_productionYield_y;
                wsYear.Cell(9, 5).Value = p5m_productionYield_y;

                wsYear.Cell(10, 1).Value = "OEE (%)";
                wsYear.Cell(10, 2).Value = p601_OEE_y;
                wsYear.Cell(10, 3).Value = p604_OEE_y;
                wsYear.Cell(10, 4).Value = p605_OEE_y;
                wsYear.Cell(10, 5).Value = p5m_OEE_y;
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
