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
    public class PtGluingController : BaseController
    {
        private readonly IProductionTrackingApiClient _ptApiClient;
        private readonly IConfiguration _configuration;
        private static bool monthOryear { get; set; }
        private static DateTime sDate { get; set; }
        private static TimeRequest tr { get; set; }
        public PtGluingController(IProductionTrackingApiClient ptApiClient,
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

            var result = new PtGluing1HVM();
            result = await _ptApiClient.GetPtGluing1H(timeRequest);

            return View("~/Views/ProductionTracking/Gluing1H.cshtml", result);
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

            var result = new PtGluingDayVM();
            result = await _ptApiClient.GetPtGluingDay(timeRequest);

            return View("~/Views/ProductionTracking/GluingDay.cshtml", result);
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

            var result = new PtGluingWeekVM();
            result = await _ptApiClient.GetPtGluingWeek(timeRequest);

            return View("~/Views/ProductionTracking/GluingWeek.cshtml", result);
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

            var result = new PtGluingMonthYearVM();
            result = await _ptApiClient.GetPtGluingMonth(timeRequest);

            var d650_avaibility_uptime = (result.D650PTMonthYear.ActualWorkingTime == 0) ? 0 : Math.Round((decimal)((result.D650PTMonthYear.ActualWorkingTime - result.D650PTMonthYear.Downtime) * 100 / result.D650PTMonthYear.ActualWorkingTime), 1);
            var d650_capacityUtilization = (result.D650PTMonthYear.MaximumProductionCapacity == 0) ? 0 : Math.Round((decimal)(result.D650PTMonthYear.TotalOutput * 100 / result.D650PTMonthYear.MaximumProductionCapacity), 1);
            var d650_productionYield = (result.D650PTMonthYear.TotalOutput == 0) ? 0 : Math.Round((decimal)((result.D650PTMonthYear.TotalOutput - result.D650PTMonthYear.Waste) * 100 / result.D650PTMonthYear.TotalOutput), 1);
            var d650_OEE = Math.Round(((d650_avaibility_uptime / 100) * (d650_capacityUtilization / 100) * (d650_productionYield / 100) * 100), 1);

            ViewBag.d650_AvaiUptime = d650_avaibility_uptime.ToString() + "%";
            ViewBag.d650_CapaUtilication = d650_capacityUtilization.ToString() + "%";
            ViewBag.d650_ProYield = d650_productionYield.ToString() + "%";
            ViewBag.d650_OEE = d650_OEE.ToString() + "%";

            var d750_avaibility_uptime = (result.D750PTMonthYear.ActualWorkingTime == 0) ? 0 : Math.Round((decimal)((result.D750PTMonthYear.ActualWorkingTime - result.D750PTMonthYear.Downtime) * 100 / result.D750PTMonthYear.ActualWorkingTime), 1);
            var d750_capacityUtilization = (result.D750PTMonthYear.MaximumProductionCapacity == 0) ? 0 : Math.Round((decimal)(result.D750PTMonthYear.TotalOutput * 100 / result.D750PTMonthYear.MaximumProductionCapacity), 1);
            var d750_productionYield = (result.D750PTMonthYear.TotalOutput == 0) ? 0 : Math.Round((decimal)((result.D750PTMonthYear.TotalOutput - result.D750PTMonthYear.Waste) * 100 / result.D750PTMonthYear.TotalOutput), 1);
            var d750_OEE = Math.Round(((d750_avaibility_uptime / 100) * (d750_capacityUtilization / 100) * (d750_productionYield / 100) * 100), 1);

            ViewBag.d750_AvaiUptime = d750_avaibility_uptime.ToString() + "%";
            ViewBag.d750_CapaUtilication = d750_capacityUtilization.ToString() + "%";
            ViewBag.d750_ProYield = d750_productionYield.ToString() + "%";
            ViewBag.d750_OEE = d750_OEE.ToString() + "%";

            var d1000_avaibility_uptime = (result.D1000PTMonthYear.ActualWorkingTime == 0) ? 0 : Math.Round((decimal)((result.D1000PTMonthYear.ActualWorkingTime - result.D1000PTMonthYear.Downtime) * 100 / result.D1000PTMonthYear.ActualWorkingTime), 1);
            var d1000_capacityUtilization = (result.D1000PTMonthYear.MaximumProductionCapacity == 0) ? 0 : Math.Round((decimal)(result.D1000PTMonthYear.TotalOutput * 100 / result.D1000PTMonthYear.MaximumProductionCapacity), 1);
            var d1000_productionYield = (result.D1000PTMonthYear.TotalOutput == 0) ? 0 : Math.Round((decimal)((result.D1000PTMonthYear.TotalOutput - result.D1000PTMonthYear.Waste) * 100 / result.D1000PTMonthYear.TotalOutput), 1);
            var d1000_OEE = Math.Round(((d1000_avaibility_uptime / 100) * (d1000_capacityUtilization / 100) * (d1000_productionYield / 100) * 100), 1);

            ViewBag.d1000_AvaiUptime = d1000_avaibility_uptime.ToString() + "%";
            ViewBag.d1000_CapaUtilication = d1000_capacityUtilization.ToString() + "%";
            ViewBag.d1000_ProYield = d1000_productionYield.ToString() + "%";
            ViewBag.d1000_OEE = d1000_OEE.ToString() + "%";

            var d1100_avaibility_uptime = (result.D1100PTMonthYear.ActualWorkingTime == 0) ? 0 : Math.Round((decimal)((result.D1100PTMonthYear.ActualWorkingTime - result.D1100PTMonthYear.Downtime) * 100 / result.D1100PTMonthYear.ActualWorkingTime), 1);
            var d1100_capacityUtilization = (result.D1100PTMonthYear.MaximumProductionCapacity == 0) ? 0 : Math.Round((decimal)(result.D1100PTMonthYear.TotalOutput * 100 / result.D1100PTMonthYear.MaximumProductionCapacity), 1);
            var d1100_productionYield = (result.D1100PTMonthYear.TotalOutput == 0) ? 0 : Math.Round((decimal)((result.D1100PTMonthYear.TotalOutput - result.D1100PTMonthYear.Waste) * 100 / result.D1100PTMonthYear.TotalOutput), 1);
            var d1100_OEE = Math.Round(((d1100_avaibility_uptime / 100) * (d1100_capacityUtilization / 100) * (d1100_productionYield / 100) * 100), 1);

            ViewBag.d1100_AvaiUptime = d1100_avaibility_uptime.ToString() + "%";
            ViewBag.d1100_CapaUtilication = d1100_capacityUtilization.ToString() + "%";
            ViewBag.d1100_ProYield = d1100_productionYield.ToString() + "%";
            ViewBag.d1100_OEE = d1100_OEE.ToString() + "%";

            return View("~/Views/ProductionTracking/GluingMonth.cshtml", result);
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

            var result = new PtGluingMonthYearVM();
            result = await _ptApiClient.GetPtGluingYear(timeRequest);

            var d650_avaibility_uptime = (result.D650PTMonthYear.ActualWorkingTime == 0) ? 0 : Math.Round((decimal)((result.D650PTMonthYear.ActualWorkingTime - result.D650PTMonthYear.Downtime) * 100 / result.D650PTMonthYear.ActualWorkingTime), 1);
            var d650_capacityUtilization = (result.D650PTMonthYear.MaximumProductionCapacity == 0) ? 0 : Math.Round((decimal)(result.D650PTMonthYear.TotalOutput * 100 / result.D650PTMonthYear.MaximumProductionCapacity), 1);
            var d650_productionYield = (result.D650PTMonthYear.TotalOutput == 0) ? 0 : Math.Round((decimal)((result.D650PTMonthYear.TotalOutput - result.D650PTMonthYear.Waste) * 100 / result.D650PTMonthYear.TotalOutput), 1);
            var d650_OEE = Math.Round(((d650_avaibility_uptime / 100) * (d650_capacityUtilization / 100) * (d650_productionYield / 100) * 100), 1);

            ViewBag.d650_AvaiUptime = d650_avaibility_uptime.ToString() + "%";
            ViewBag.d650_CapaUtilication = d650_capacityUtilization.ToString() + "%";
            ViewBag.d650_ProYield = d650_productionYield.ToString() + "%";
            ViewBag.d650_OEE = d650_OEE.ToString() + "%";

            var d750_avaibility_uptime = (result.D750PTMonthYear.ActualWorkingTime == 0) ? 0 : Math.Round((decimal)((result.D750PTMonthYear.ActualWorkingTime - result.D750PTMonthYear.Downtime) * 100 / result.D750PTMonthYear.ActualWorkingTime), 1);
            var d750_capacityUtilization = (result.D750PTMonthYear.MaximumProductionCapacity == 0) ? 0 : Math.Round((decimal)(result.D750PTMonthYear.TotalOutput * 100 / result.D750PTMonthYear.MaximumProductionCapacity), 1);
            var d750_productionYield = (result.D750PTMonthYear.TotalOutput == 0) ? 0 : Math.Round((decimal)((result.D750PTMonthYear.TotalOutput - result.D750PTMonthYear.Waste) * 100 / result.D750PTMonthYear.TotalOutput), 1);
            var d750_OEE = Math.Round(((d750_avaibility_uptime / 100) * (d750_capacityUtilization / 100) * (d750_productionYield / 100) * 100), 1);

            ViewBag.d750_AvaiUptime = d750_avaibility_uptime.ToString() + "%";
            ViewBag.d750_CapaUtilication = d750_capacityUtilization.ToString() + "%";
            ViewBag.d750_ProYield = d750_productionYield.ToString() + "%";
            ViewBag.d750_OEE = d750_OEE.ToString() + "%";

            var d1000_avaibility_uptime = (result.D1000PTMonthYear.ActualWorkingTime == 0) ? 0 : Math.Round((decimal)((result.D1000PTMonthYear.ActualWorkingTime - result.D1000PTMonthYear.Downtime) * 100 / result.D1000PTMonthYear.ActualWorkingTime), 1);
            var d1000_capacityUtilization = (result.D1000PTMonthYear.MaximumProductionCapacity == 0) ? 0 : Math.Round((decimal)(result.D1000PTMonthYear.TotalOutput * 100 / result.D1000PTMonthYear.MaximumProductionCapacity), 1);
            var d1000_productionYield = (result.D1000PTMonthYear.TotalOutput == 0) ? 0 : Math.Round((decimal)((result.D1000PTMonthYear.TotalOutput - result.D1000PTMonthYear.Waste) * 100 / result.D1000PTMonthYear.TotalOutput), 1);
            var d1000_OEE = Math.Round(((d1000_avaibility_uptime / 100) * (d1000_capacityUtilization / 100) * (d1000_productionYield / 100) * 100), 1);

            ViewBag.d1000_AvaiUptime = d1000_avaibility_uptime.ToString() + "%";
            ViewBag.d1000_CapaUtilication = d1000_capacityUtilization.ToString() + "%";
            ViewBag.d1000_ProYield = d1000_productionYield.ToString() + "%";
            ViewBag.d1000_OEE = d1000_OEE.ToString() + "%";

            var d1100_avaibility_uptime = (result.D1100PTMonthYear.ActualWorkingTime == 0) ? 0 : Math.Round((decimal)((result.D1100PTMonthYear.ActualWorkingTime - result.D1100PTMonthYear.Downtime) * 100 / result.D1100PTMonthYear.ActualWorkingTime), 1);
            var d1100_capacityUtilization = (result.D1100PTMonthYear.MaximumProductionCapacity == 0) ? 0 : Math.Round((decimal)(result.D1100PTMonthYear.TotalOutput * 100 / result.D1100PTMonthYear.MaximumProductionCapacity), 1);
            var d1100_productionYield = (result.D1100PTMonthYear.TotalOutput == 0) ? 0 : Math.Round((decimal)((result.D1100PTMonthYear.TotalOutput - result.D1100PTMonthYear.Waste) * 100 / result.D1100PTMonthYear.TotalOutput), 1);
            var d1100_OEE = Math.Round(((d1100_avaibility_uptime / 100) * (d1100_capacityUtilization / 100) * (d1100_productionYield / 100) * 100), 1);

            ViewBag.d1100_AvaiUptime = d1100_avaibility_uptime.ToString() + "%";
            ViewBag.d1100_CapaUtilication = d1100_capacityUtilization.ToString() + "%";
            ViewBag.d1100_ProYield = d1100_productionYield.ToString() + "%";
            ViewBag.d1100_OEE = d1100_OEE.ToString() + "%";

            return View("~/Views/ProductionTracking/GluingYear.cshtml", result);
        }


        public async Task<IActionResult> ExportExcel1H()
        {
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            string fileName = sDate.ToString("yyyyMMdd_hhmmsstt") + "_Gluing_Report_1H.csv";
            try
            {
                var datas = new PtGluing1HVM();
                datas = await _ptApiClient.GetPtGluing1H(tr);

                var workbook = new XLWorkbook();

                IXLWorksheet worksheet = workbook.Worksheets.Add("1 Hour");
                worksheet.Cell(1, 1).Value = "D650";
                worksheet.Cell(1, 8).Value = "D750";
                worksheet.Cell(1, 15).Value = "D1000";
                worksheet.Cell(1, 22).Value = "D1100";

                worksheet.Cell(2, 1).Value = worksheet.Cell(2, 8).Value = worksheet.Cell(2, 15).Value = worksheet.Cell(2, 22).Value = "Worksheet";
                worksheet.Cell(2, 2).Value = worksheet.Cell(2, 9).Value = worksheet.Cell(2, 16).Value = worksheet.Cell(2, 23).Value = "ProductCode";
                worksheet.Cell(2, 3).Value = worksheet.Cell(2, 10).Value = worksheet.Cell(2, 17).Value = worksheet.Cell(2, 24).Value = "Quantity";
                worksheet.Cell(2, 4).Value = worksheet.Cell(2, 11).Value = worksheet.Cell(2, 18).Value = worksheet.Cell(2, 25).Value = "Target";

                for (int index = 1; index <= datas.D650PT1H.Count; index++)
                {
                    worksheet.Cell(index + 2, 1).Value = datas.D650PT1H[index - 1].Ws;
                    worksheet.Cell(index + 2, 2).Value = datas.D650PT1H[index - 1].Product;
                    worksheet.Cell(index + 2, 3).Value = datas.D650PT1H[index - 1].Job;
                    worksheet.Cell(index + 2, 4).Value = datas.D650PT1H[index - 1].Target;
                }

                for (int index = 1; index <= datas.D750PT1H.Count; index++)
                {
                    worksheet.Cell(index + 2, 8).Value = datas.D750PT1H[index - 1].Ws;
                    worksheet.Cell(index + 2, 9).Value = datas.D750PT1H[index - 1].Product;
                    worksheet.Cell(index + 2, 10).Value = datas.D750PT1H[index - 1].Job;
                    worksheet.Cell(index + 2, 11).Value = datas.D750PT1H[index - 1].Target;
                }

                for (int index = 1; index <= datas.D1000PT1H.Count; index++)
                {
                    worksheet.Cell(index + 2, 15).Value = datas.D1000PT1H[index - 1].Ws;
                    worksheet.Cell(index + 2, 16).Value = datas.D1000PT1H[index - 1].Product;
                    worksheet.Cell(index + 2, 17).Value = datas.D1000PT1H[index - 1].Job;
                    worksheet.Cell(index + 2, 18).Value = datas.D1000PT1H[index - 1].Target;
                }

                for (int index = 1; index <= datas.D1100PT1H.Count; index++)
                {
                    worksheet.Cell(index + 2, 22).Value = datas.D1100PT1H[index - 1].Ws;
                    worksheet.Cell(index + 2, 23).Value = datas.D1100PT1H[index - 1].Product;
                    worksheet.Cell(index + 2, 24).Value = datas.D1100PT1H[index - 1].Job;
                    worksheet.Cell(index + 2, 25).Value = datas.D1100PT1H[index - 1].Target;
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
            string fileName = sDate.ToString("yyyyMMdd_hhmmsstt") + "_Gluing_Report_Day.csv";
            try
            {
                var datas = new PtGluingDayVM();
                datas = await _ptApiClient.GetPtGluingDay(tr);

                var workbook = new XLWorkbook();

                IXLWorksheet worksheet = workbook.Worksheets.Add("Day");
                worksheet.Cell(1, 1).Value = "D650";
                worksheet.Cell(1, 11).Value = "D750";
                worksheet.Cell(1, 21).Value = "D1000";
                worksheet.Cell(1, 31).Value = "D1100";

                worksheet.Cell(2, 1).Value = worksheet.Cell(2, 11).Value = worksheet.Cell(2, 21).Value = worksheet.Cell(2, 31).Value = "Worksheet";
                worksheet.Cell(2, 2).Value = worksheet.Cell(2, 12).Value = worksheet.Cell(2, 22).Value = worksheet.Cell(2, 32).Value = "ProductCode";
                worksheet.Cell(2, 3).Value = worksheet.Cell(2, 13).Value = worksheet.Cell(2, 23).Value = worksheet.Cell(2, 33).Value = "Quantity";
                worksheet.Cell(2, 4).Value = worksheet.Cell(2, 14).Value = worksheet.Cell(2, 24).Value = worksheet.Cell(2, 34).Value = "CompleteTime";
                worksheet.Cell(2, 5).Value = worksheet.Cell(2, 15).Value = worksheet.Cell(2, 25).Value = worksheet.Cell(2, 35).Value = "PlanTime";
                worksheet.Cell(2, 6).Value = worksheet.Cell(2, 16).Value = worksheet.Cell(2, 26).Value = worksheet.Cell(2, 36).Value = "PlanEndTime";
                worksheet.Cell(2, 7).Value = worksheet.Cell(2, 17).Value = worksheet.Cell(2, 27).Value = worksheet.Cell(2, 37).Value = "Status";

                for (int index = 1; index <= datas.D650PTDay.Count; index++)
                {
                    worksheet.Cell(index + 2, 1).Value = datas.D650PTDay[index - 1].Ws;
                    worksheet.Cell(index + 2, 2).Value = datas.D650PTDay[index - 1].Product;
                    worksheet.Cell(index + 2, 3).Value = datas.D650PTDay[index - 1].Job;
                    worksheet.Cell(index + 2, 4).Value = datas.D650PTDay[index - 1].Endtime;
                    worksheet.Cell(index + 2, 5).Value = datas.D650PTDay[index - 1].Plantime;
                    worksheet.Cell(index + 2, 6).Value = datas.D650PTDay[index - 1].PlanEndtime;
                    worksheet.Cell(index + 2, 7).Value = datas.D650PTDay[index - 1].Status;
                }

                for (int index = 1; index <= datas.D750PTDay.Count; index++)
                {
                    worksheet.Cell(index + 2, 11).Value = datas.D750PTDay[index - 1].Ws;
                    worksheet.Cell(index + 2, 12).Value = datas.D750PTDay[index - 1].Product;
                    worksheet.Cell(index + 2, 13).Value = datas.D750PTDay[index - 1].Job;
                    worksheet.Cell(index + 2, 14).Value = datas.D750PTDay[index - 1].Endtime;
                    worksheet.Cell(index + 2, 15).Value = datas.D750PTDay[index - 1].Plantime;
                    worksheet.Cell(index + 2, 16).Value = datas.D750PTDay[index - 1].PlanEndtime;
                    worksheet.Cell(index + 2, 17).Value = datas.D750PTDay[index - 1].Status;
                }

                for (int index = 1; index <= datas.D1000PTDay.Count; index++)
                {
                    worksheet.Cell(index + 2, 21).Value = datas.D1000PTDay[index - 1].Ws;
                    worksheet.Cell(index + 2, 22).Value = datas.D1000PTDay[index - 1].Product;
                    worksheet.Cell(index + 2, 23).Value = datas.D1000PTDay[index - 1].Job;
                    worksheet.Cell(index + 2, 24).Value = datas.D1000PTDay[index - 1].Endtime;
                    worksheet.Cell(index + 2, 25).Value = datas.D1000PTDay[index - 1].Plantime;
                    worksheet.Cell(index + 2, 26).Value = datas.D1000PTDay[index - 1].PlanEndtime;
                    worksheet.Cell(index + 2, 27).Value = datas.D1000PTDay[index - 1].Status;
                }

                for (int index = 1; index <= datas.D1100PTDay.Count; index++)
                {
                    worksheet.Cell(index + 2, 31).Value = datas.D1100PTDay[index - 1].Ws;
                    worksheet.Cell(index + 2, 32).Value = datas.D1100PTDay[index - 1].Product;
                    worksheet.Cell(index + 2, 33).Value = datas.D1100PTDay[index - 1].Job;
                    worksheet.Cell(index + 2, 34).Value = datas.D1100PTDay[index - 1].Endtime;
                    worksheet.Cell(index + 2, 35).Value = datas.D1100PTDay[index - 1].Plantime;
                    worksheet.Cell(index + 2, 36).Value = datas.D1100PTDay[index - 1].PlanEndtime;
                    worksheet.Cell(index + 2, 37).Value = datas.D1100PTDay[index - 1].Status;
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
            string fileName = sDate.ToString("yyyyMMdd_hhmmsstt") + "_Gluing_Report_Week.csv";
            try
            {
                var datas = new PtGluingWeekVM();
                datas = await _ptApiClient.GetPtGluingWeek(tr);

                var workbook = new XLWorkbook();

                IXLWorksheet worksheet = workbook.Worksheets.Add("Week");
                worksheet.Cell(1, 1).Value = "Statistics";
                worksheet.Cell(1, 2).Value = "D650";
                worksheet.Cell(1, 3).Value = "D750";
                worksheet.Cell(1, 4).Value = "D1000";
                worksheet.Cell(1, 5).Value = "D1100";

                worksheet.Cell(2, 1).Value = "Testing time";
                worksheet.Cell(2, 2).Value = datas.D650PTWeek.TestingTimeAndPercent;
                worksheet.Cell(2, 3).Value = datas.D750PTWeek.TestingTimeAndPercent;
                worksheet.Cell(2, 4).Value = datas.D1000PTWeek.TestingTimeAndPercent;
                worksheet.Cell(2, 5).Value = datas.D1100PTWeek.TestingTimeAndPercent;

                worksheet.Cell(3, 1).Value = "Running time";
                worksheet.Cell(3, 2).Value = datas.D650PTWeek.RunningTimeAndPercent;
                worksheet.Cell(3, 3).Value = datas.D750PTWeek.RunningTimeAndPercent;
                worksheet.Cell(3, 4).Value = datas.D1000PTWeek.RunningTimeAndPercent;
                worksheet.Cell(3, 5).Value = datas.D1100PTWeek.RunningTimeAndPercent;

                worksheet.Cell(4, 1).Value = "Not Running time";
                worksheet.Cell(4, 2).Value = datas.D650PTWeek.NotRunningTimeAndPercent;
                worksheet.Cell(4, 3).Value = datas.D750PTWeek.NotRunningTimeAndPercent;
                worksheet.Cell(4, 4).Value = datas.D1000PTWeek.NotRunningTimeAndPercent;
                worksheet.Cell(4, 5).Value = datas.D1100PTWeek.NotRunningTimeAndPercent;

                worksheet.Cell(5, 1).Value = "No. Product Code Change";
                worksheet.Cell(5, 2).Value = datas.D650PTWeek.ProductCodeChangeCount;
                worksheet.Cell(5, 3).Value = datas.D750PTWeek.ProductCodeChangeCount;
                worksheet.Cell(5, 4).Value = datas.D1000PTWeek.ProductCodeChangeCount;
                worksheet.Cell(5, 5).Value = datas.D1100PTWeek.ProductCodeChangeCount;

                worksheet.Cell(6, 1).Value = "Quantity";
                worksheet.Cell(6, 2).Value = datas.D650PTWeek.Quantity;
                worksheet.Cell(6, 3).Value = datas.D750PTWeek.Quantity;
                worksheet.Cell(6, 4).Value = datas.D1000PTWeek.Quantity;
                worksheet.Cell(6, 5).Value = datas.D1100PTWeek.Quantity;

                worksheet.Cell(7, 1).Value = "Defect";
                worksheet.Cell(7, 2).Value = datas.D650PTWeek.Waste;
                worksheet.Cell(7, 3).Value = datas.D750PTWeek.Waste;
                worksheet.Cell(7, 4).Value = datas.D1000PTWeek.Waste;
                worksheet.Cell(7, 5).Value = datas.D1100PTWeek.Waste;

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
                string fileName = sDate.ToString("yyyyMMdd_hhmmsstt") + "_Gluing_Report_Month.csv";
                try
                {
                    var datas = new PtGluingMonthYearVM();
                    datas = await _ptApiClient.GetPtGluingMonth(tr);
                    var d650_avaibility_uptime = (datas.D650PTMonthYear.ActualWorkingTime == 0) ? 0 : Math.Round((decimal)((datas.D650PTMonthYear.ActualWorkingTime - datas.D650PTMonthYear.Downtime) * 100 / datas.D650PTMonthYear.ActualWorkingTime), 1);
                    var d650_capacityUtilization = (datas.D650PTMonthYear.MaximumProductionCapacity == 0) ? 0 : Math.Round((decimal)(datas.D650PTMonthYear.TotalOutput * 100 / datas.D650PTMonthYear.MaximumProductionCapacity), 1);
                    var d650_productionYield = (datas.D650PTMonthYear.TotalOutput == 0) ? 0 : Math.Round((decimal)((datas.D650PTMonthYear.TotalOutput - datas.D650PTMonthYear.Waste) * 100 / datas.D650PTMonthYear.TotalOutput), 1);
                    var d650_OEE = Math.Round(((d650_avaibility_uptime / 100) * (d650_capacityUtilization / 100) * (d650_productionYield / 100) * 100), 1);

                    var d750_avaibility_uptime = (datas.D750PTMonthYear.ActualWorkingTime == 0) ? 0 : Math.Round((decimal)((datas.D750PTMonthYear.ActualWorkingTime - datas.D750PTMonthYear.Downtime) * 100 / datas.D750PTMonthYear.ActualWorkingTime), 1);
                    var d750_capacityUtilization = (datas.D750PTMonthYear.MaximumProductionCapacity == 0) ? 0 : Math.Round((decimal)(datas.D750PTMonthYear.TotalOutput * 100 / datas.D750PTMonthYear.MaximumProductionCapacity), 1);
                    var d750_productionYield = (datas.D750PTMonthYear.TotalOutput == 0) ? 0 : Math.Round((decimal)((datas.D750PTMonthYear.TotalOutput - datas.D750PTMonthYear.Waste) * 100 / datas.D750PTMonthYear.TotalOutput), 1);
                    var d750_OEE = Math.Round(((d750_avaibility_uptime / 100) * (d750_capacityUtilization / 100) * (d750_productionYield / 100) * 100), 1);

                    var d1000_avaibility_uptime = (datas.D1000PTMonthYear.ActualWorkingTime == 0) ? 0 : Math.Round((decimal)((datas.D1000PTMonthYear.ActualWorkingTime - datas.D1000PTMonthYear.Downtime) * 100 / datas.D1000PTMonthYear.ActualWorkingTime), 1);
                    var d1000_capacityUtilization = (datas.D1000PTMonthYear.MaximumProductionCapacity == 0) ? 0 : Math.Round((decimal)(datas.D1000PTMonthYear.TotalOutput * 100 / datas.D1000PTMonthYear.MaximumProductionCapacity), 1);
                    var d1000_productionYield = (datas.D1000PTMonthYear.TotalOutput == 0) ? 0 : Math.Round((decimal)((datas.D1000PTMonthYear.TotalOutput - datas.D1000PTMonthYear.Waste) * 100 / datas.D1000PTMonthYear.TotalOutput), 1);
                    var d1000_OEE = Math.Round(((d1000_avaibility_uptime / 100) * (d1000_capacityUtilization / 100) * (d1000_productionYield / 100) * 100), 1);

                    var d1100_avaibility_uptime = (datas.D1100PTMonthYear.ActualWorkingTime == 0) ? 0 : Math.Round((decimal)((datas.D1100PTMonthYear.ActualWorkingTime - datas.D1100PTMonthYear.Downtime) * 100 / datas.D1100PTMonthYear.ActualWorkingTime), 1);
                    var d1100_capacityUtilization = (datas.D1100PTMonthYear.MaximumProductionCapacity == 0) ? 0 : Math.Round((decimal)(datas.D1100PTMonthYear.TotalOutput * 100 / datas.D1100PTMonthYear.MaximumProductionCapacity), 1);
                    var d1100_productionYield = (datas.D1100PTMonthYear.TotalOutput == 0) ? 0 : Math.Round((decimal)((datas.D1100PTMonthYear.TotalOutput - datas.D1100PTMonthYear.Waste) * 100 / datas.D1100PTMonthYear.TotalOutput), 1);
                    var d1100_OEE = Math.Round(((d1100_avaibility_uptime / 100) * (d1100_capacityUtilization / 100) * (d1100_productionYield / 100) * 100), 1);

                    var workbook = new XLWorkbook();

                    IXLWorksheet worksheet = workbook.Worksheets.Add("Month");
                    worksheet.Cell(1, 1).Value = "Statistics";
                    worksheet.Cell(1, 2).Value = "D650";
                    worksheet.Cell(1, 3).Value = "D750";
                    worksheet.Cell(1, 4).Value = "D1000";
                    worksheet.Cell(1, 5).Value = "D1100";

                    worksheet.Cell(2, 1).Value = "Actual working time (h)";
                    worksheet.Cell(2, 2).Value = datas.D650PTMonthYear.ActualWorkingTime;
                    worksheet.Cell(2, 3).Value = datas.D750PTMonthYear.ActualWorkingTime;
                    worksheet.Cell(2, 4).Value = datas.D1000PTMonthYear.ActualWorkingTime;
                    worksheet.Cell(2, 5).Value = datas.D1100PTMonthYear.ActualWorkingTime;

                    worksheet.Cell(3, 1).Value = "Downtime (h)";
                    worksheet.Cell(3, 2).Value = datas.D650PTMonthYear.Downtime;
                    worksheet.Cell(3, 3).Value = datas.D750PTMonthYear.Downtime;
                    worksheet.Cell(3, 4).Value = datas.D1000PTMonthYear.Downtime;
                    worksheet.Cell(3, 5).Value = datas.D1100PTMonthYear.Downtime;

                    worksheet.Cell(4, 1).Value = "Total Quantity (pcs)";
                    worksheet.Cell(4, 2).Value = datas.D650PTMonthYear.TotalOutput;
                    worksheet.Cell(4, 3).Value = datas.D750PTMonthYear.TotalOutput;
                    worksheet.Cell(4, 4).Value = datas.D1000PTMonthYear.TotalOutput;
                    worksheet.Cell(4, 5).Value = datas.D1100PTMonthYear.TotalOutput;

                    worksheet.Cell(5, 1).Value = "Maximum production capacity (pcs)";
                    worksheet.Cell(5, 2).Value = datas.D650PTMonthYear.MaximumProductionCapacity;
                    worksheet.Cell(5, 3).Value = datas.D750PTMonthYear.MaximumProductionCapacity;
                    worksheet.Cell(5, 4).Value = datas.D1000PTMonthYear.MaximumProductionCapacity;
                    worksheet.Cell(5, 5).Value = datas.D1100PTMonthYear.MaximumProductionCapacity;

                    worksheet.Cell(6, 1).Value = "Total Defect (pcs)";
                    worksheet.Cell(6, 2).Value = datas.D650PTMonthYear.Waste;
                    worksheet.Cell(6, 3).Value = datas.D750PTMonthYear.Waste;
                    worksheet.Cell(6, 4).Value = datas.D1000PTMonthYear.Waste;
                    worksheet.Cell(6, 5).Value = datas.D1100PTMonthYear.Waste;

                    worksheet.Cell(7, 1).Value = "Avaibility/Uptime (%)";
                    worksheet.Cell(7, 2).Value = d650_avaibility_uptime;
                    worksheet.Cell(7, 3).Value = d750_avaibility_uptime;
                    worksheet.Cell(7, 4).Value = d1000_avaibility_uptime;
                    worksheet.Cell(7, 5).Value = d1100_avaibility_uptime;

                    worksheet.Cell(8, 1).Value = "Capacity utilization (%)";
                    worksheet.Cell(8, 2).Value = d650_capacityUtilization;
                    worksheet.Cell(8, 3).Value = d750_capacityUtilization;
                    worksheet.Cell(8, 4).Value = d1000_capacityUtilization;
                    worksheet.Cell(8, 5).Value = d1100_capacityUtilization;

                    worksheet.Cell(9, 1).Value = "Production yield (%)";
                    worksheet.Cell(9, 2).Value = d650_productionYield;
                    worksheet.Cell(9, 3).Value = d750_productionYield;
                    worksheet.Cell(9, 4).Value = d1000_productionYield;
                    worksheet.Cell(9, 5).Value = d1100_productionYield;

                    worksheet.Cell(10, 1).Value = "OEE (%)";
                    worksheet.Cell(10, 2).Value = d650_OEE;
                    worksheet.Cell(10, 3).Value = d750_OEE;
                    worksheet.Cell(10, 4).Value = d1000_OEE;
                    worksheet.Cell(10, 5).Value = d1100_OEE;

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
                string fileName = sDate.ToString("yyyyMMdd_hhmmsstt") + "_Gluing_Report_Year.csv";
                try
                {
                    var datas = new PtGluingMonthYearVM();
                    datas = await _ptApiClient.GetPtGluingYear(tr);
                    var d650_avaibility_uptime = (datas.D650PTMonthYear.ActualWorkingTime == 0) ? 0 : Math.Round((decimal)((datas.D650PTMonthYear.ActualWorkingTime - datas.D650PTMonthYear.Downtime) * 100 / datas.D650PTMonthYear.ActualWorkingTime), 1);
                    var d650_capacityUtilization = (datas.D650PTMonthYear.MaximumProductionCapacity == 0) ? 0 : Math.Round((decimal)(datas.D650PTMonthYear.TotalOutput * 100 / datas.D650PTMonthYear.MaximumProductionCapacity), 1);
                    var d650_productionYield = (datas.D650PTMonthYear.TotalOutput == 0) ? 0 : Math.Round((decimal)((datas.D650PTMonthYear.TotalOutput - datas.D650PTMonthYear.Waste) * 100 / datas.D650PTMonthYear.TotalOutput), 1);
                    var d650_OEE = Math.Round(((d650_avaibility_uptime / 100) * (d650_capacityUtilization / 100) * (d650_productionYield / 100) * 100), 1);

                    var d750_avaibility_uptime = (datas.D750PTMonthYear.ActualWorkingTime == 0) ? 0 : Math.Round((decimal)((datas.D750PTMonthYear.ActualWorkingTime - datas.D750PTMonthYear.Downtime) * 100 / datas.D750PTMonthYear.ActualWorkingTime), 1);
                    var d750_capacityUtilization = (datas.D750PTMonthYear.MaximumProductionCapacity == 0) ? 0 : Math.Round((decimal)(datas.D750PTMonthYear.TotalOutput * 100 / datas.D750PTMonthYear.MaximumProductionCapacity), 1);
                    var d750_productionYield = (datas.D750PTMonthYear.TotalOutput == 0) ? 0 : Math.Round((decimal)((datas.D750PTMonthYear.TotalOutput - datas.D750PTMonthYear.Waste) * 100 / datas.D750PTMonthYear.TotalOutput), 1);
                    var d750_OEE = Math.Round(((d750_avaibility_uptime / 100) * (d750_capacityUtilization / 100) * (d750_productionYield / 100) * 100), 1);

                    var d1000_avaibility_uptime = (datas.D1000PTMonthYear.ActualWorkingTime == 0) ? 0 : Math.Round((decimal)((datas.D1000PTMonthYear.ActualWorkingTime - datas.D1000PTMonthYear.Downtime) * 100 / datas.D1000PTMonthYear.ActualWorkingTime), 1);
                    var d1000_capacityUtilization = (datas.D1000PTMonthYear.MaximumProductionCapacity == 0) ? 0 : Math.Round((decimal)(datas.D1000PTMonthYear.TotalOutput * 100 / datas.D1000PTMonthYear.MaximumProductionCapacity), 1);
                    var d1000_productionYield = (datas.D1000PTMonthYear.TotalOutput == 0) ? 0 : Math.Round((decimal)((datas.D1000PTMonthYear.TotalOutput - datas.D1000PTMonthYear.Waste) * 100 / datas.D1000PTMonthYear.TotalOutput), 1);
                    var d1000_OEE = Math.Round(((d1000_avaibility_uptime / 100) * (d1000_capacityUtilization / 100) * (d1000_productionYield / 100) * 100), 1);

                    var d1100_avaibility_uptime = (datas.D1100PTMonthYear.ActualWorkingTime == 0) ? 0 : Math.Round((decimal)((datas.D1100PTMonthYear.ActualWorkingTime - datas.D1100PTMonthYear.Downtime) * 100 / datas.D1100PTMonthYear.ActualWorkingTime), 1);
                    var d1100_capacityUtilization = (datas.D1100PTMonthYear.MaximumProductionCapacity == 0) ? 0 : Math.Round((decimal)(datas.D1100PTMonthYear.TotalOutput * 100 / datas.D1100PTMonthYear.MaximumProductionCapacity), 1);
                    var d1100_productionYield = (datas.D1100PTMonthYear.TotalOutput == 0) ? 0 : Math.Round((decimal)((datas.D1100PTMonthYear.TotalOutput - datas.D1100PTMonthYear.Waste) * 100 / datas.D1100PTMonthYear.TotalOutput), 1);
                    var d1100_OEE = Math.Round(((d1100_avaibility_uptime / 100) * (d1100_capacityUtilization / 100) * (d1100_productionYield / 100) * 100), 1);

                    var workbook = new XLWorkbook();

                    IXLWorksheet worksheet = workbook.Worksheets.Add("Year");
                    worksheet.Cell(1, 1).Value = "Statistics";
                    worksheet.Cell(1, 2).Value = "D650";
                    worksheet.Cell(1, 3).Value = "D750";
                    worksheet.Cell(1, 4).Value = "D1000";
                    worksheet.Cell(1, 5).Value = "D1100";

                    worksheet.Cell(2, 1).Value = "Actual working time (h)";
                    worksheet.Cell(2, 2).Value = datas.D650PTMonthYear.ActualWorkingTime;
                    worksheet.Cell(2, 3).Value = datas.D750PTMonthYear.ActualWorkingTime;
                    worksheet.Cell(2, 4).Value = datas.D1000PTMonthYear.ActualWorkingTime;
                    worksheet.Cell(2, 5).Value = datas.D1100PTMonthYear.ActualWorkingTime;

                    worksheet.Cell(3, 1).Value = "Downtime (h)";
                    worksheet.Cell(3, 2).Value = datas.D650PTMonthYear.Downtime;
                    worksheet.Cell(3, 3).Value = datas.D750PTMonthYear.Downtime;
                    worksheet.Cell(3, 4).Value = datas.D1000PTMonthYear.Downtime;
                    worksheet.Cell(3, 5).Value = datas.D1100PTMonthYear.Downtime;

                    worksheet.Cell(4, 1).Value = "Total Quantity (pcs)";
                    worksheet.Cell(4, 2).Value = datas.D650PTMonthYear.TotalOutput;
                    worksheet.Cell(4, 3).Value = datas.D750PTMonthYear.TotalOutput;
                    worksheet.Cell(4, 4).Value = datas.D1000PTMonthYear.TotalOutput;
                    worksheet.Cell(4, 5).Value = datas.D1100PTMonthYear.TotalOutput;

                    worksheet.Cell(5, 1).Value = "Maximum production capacity (pcs)";
                    worksheet.Cell(5, 2).Value = datas.D650PTMonthYear.MaximumProductionCapacity;
                    worksheet.Cell(5, 3).Value = datas.D750PTMonthYear.MaximumProductionCapacity;
                    worksheet.Cell(5, 4).Value = datas.D1000PTMonthYear.MaximumProductionCapacity;
                    worksheet.Cell(5, 5).Value = datas.D1100PTMonthYear.MaximumProductionCapacity;

                    worksheet.Cell(6, 1).Value = "Total Defect (pcs)";
                    worksheet.Cell(6, 2).Value = datas.D650PTMonthYear.Waste;
                    worksheet.Cell(6, 3).Value = datas.D750PTMonthYear.Waste;
                    worksheet.Cell(6, 4).Value = datas.D1000PTMonthYear.Waste;
                    worksheet.Cell(6, 5).Value = datas.D1100PTMonthYear.Waste;

                    worksheet.Cell(7, 1).Value = "Avaibility/Uptime (%)";
                    worksheet.Cell(7, 2).Value = d650_avaibility_uptime;
                    worksheet.Cell(7, 3).Value = d750_avaibility_uptime;
                    worksheet.Cell(7, 4).Value = d1000_avaibility_uptime;
                    worksheet.Cell(7, 5).Value = d1100_avaibility_uptime;

                    worksheet.Cell(8, 1).Value = "Capacity utilization (%)";
                    worksheet.Cell(8, 2).Value = d650_capacityUtilization;
                    worksheet.Cell(8, 3).Value = d750_capacityUtilization;
                    worksheet.Cell(8, 4).Value = d1000_capacityUtilization;
                    worksheet.Cell(8, 5).Value = d1100_capacityUtilization;

                    worksheet.Cell(9, 1).Value = "Production yield (%)";
                    worksheet.Cell(9, 2).Value = d650_productionYield;
                    worksheet.Cell(9, 3).Value = d750_productionYield;
                    worksheet.Cell(9, 4).Value = d1000_productionYield;
                    worksheet.Cell(9, 5).Value = d1100_productionYield;

                    worksheet.Cell(10, 1).Value = "OEE (%)";
                    worksheet.Cell(10, 2).Value = d650_OEE;
                    worksheet.Cell(10, 3).Value = d750_OEE;
                    worksheet.Cell(10, 4).Value = d1000_OEE;
                    worksheet.Cell(10, 5).Value = d1100_OEE;

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

            string fileName = sDate.ToString("yyyyMMdd_hhmmsstt") + "_Gluing_Report.csv";

            try
            {
                var workbook = new XLWorkbook();

                #region 1H
                var timeRequest = new TimeRequest();
                timeRequest.To = sDate;
                timeRequest.From = sDate.AddHours(-1);

                var datas1H = new PtGluing1HVM();
                datas1H = await _ptApiClient.GetPtGluing1H(timeRequest);
                IXLWorksheet ws1H = workbook.Worksheets.Add("1 Hour");
                ws1H.Cell(1, 1).Value = "D650";
                ws1H.Cell(1, 8).Value = "D750";
                ws1H.Cell(1, 15).Value = "D1000";
                ws1H.Cell(1, 22).Value = "D1100";

                ws1H.Cell(2, 1).Value = ws1H.Cell(2, 8).Value = ws1H.Cell(2, 15).Value = ws1H.Cell(2, 22).Value = "Worksheet";
                ws1H.Cell(2, 2).Value = ws1H.Cell(2, 9).Value = ws1H.Cell(2, 16).Value = ws1H.Cell(2, 23).Value = "ProductCode";
                ws1H.Cell(2, 3).Value = ws1H.Cell(2, 10).Value = ws1H.Cell(2, 17).Value = ws1H.Cell(2, 24).Value = "Quantity";
                ws1H.Cell(2, 4).Value = ws1H.Cell(2, 11).Value = ws1H.Cell(2, 18).Value = ws1H.Cell(2, 25).Value = "Target";

                for (int index = 1; index <= datas1H.D650PT1H.Count; index++)
                {
                    ws1H.Cell(index + 2, 1).Value = datas1H.D650PT1H[index - 1].Ws;
                    ws1H.Cell(index + 2, 2).Value = datas1H.D650PT1H[index - 1].Product;
                    ws1H.Cell(index + 2, 3).Value = datas1H.D650PT1H[index - 1].Job;
                    ws1H.Cell(index + 2, 4).Value = datas1H.D650PT1H[index - 1].Target;
                }

                for (int index = 1; index <= datas1H.D750PT1H.Count; index++)
                {
                    ws1H.Cell(index + 2, 8).Value = datas1H.D750PT1H[index - 1].Ws;
                    ws1H.Cell(index + 2, 9).Value = datas1H.D750PT1H[index - 1].Product;
                    ws1H.Cell(index + 2, 10).Value = datas1H.D750PT1H[index - 1].Job;
                    ws1H.Cell(index + 2, 11).Value = datas1H.D750PT1H[index - 1].Target;
                }

                for (int index = 1; index <= datas1H.D1000PT1H.Count; index++)
                {
                    ws1H.Cell(index + 2, 15).Value = datas1H.D1000PT1H[index - 1].Ws;
                    ws1H.Cell(index + 2, 16).Value = datas1H.D1000PT1H[index - 1].Product;
                    ws1H.Cell(index + 2, 17).Value = datas1H.D1000PT1H[index - 1].Job;
                    ws1H.Cell(index + 2, 18).Value = datas1H.D1000PT1H[index - 1].Target;
                }

                for (int index = 1; index <= datas1H.D1100PT1H.Count; index++)
                {
                    ws1H.Cell(index + 2, 22).Value = datas1H.D1100PT1H[index - 1].Ws;
                    ws1H.Cell(index + 2, 23).Value = datas1H.D1100PT1H[index - 1].Product;
                    ws1H.Cell(index + 2, 24).Value = datas1H.D1100PT1H[index - 1].Job;
                    ws1H.Cell(index + 2, 25).Value = datas1H.D1100PT1H[index - 1].Target;
                }
                #endregion
                #region Day
                timeRequest.To = sDate;
                timeRequest.From = sDate.AddDays(-1);

                var datasDay = new PtGluingDayVM();
                datasDay = await _ptApiClient.GetPtGluingDay(timeRequest);

                IXLWorksheet wsDay = workbook.Worksheets.Add("Day");
                wsDay.Cell(1, 1).Value = "D650";
                wsDay.Cell(1, 11).Value = "D750";
                wsDay.Cell(1, 21).Value = "D1000";
                wsDay.Cell(1, 31).Value = "D1100";

                wsDay.Cell(2, 1).Value = wsDay.Cell(2, 11).Value = wsDay.Cell(2, 21).Value = wsDay.Cell(2, 31).Value = "Worksheet";
                wsDay.Cell(2, 2).Value = wsDay.Cell(2, 12).Value = wsDay.Cell(2, 22).Value = wsDay.Cell(2, 32).Value = "ProductCode";
                wsDay.Cell(2, 3).Value = wsDay.Cell(2, 13).Value = wsDay.Cell(2, 23).Value = wsDay.Cell(2, 33).Value = "Quantity";
                wsDay.Cell(2, 4).Value = wsDay.Cell(2, 14).Value = wsDay.Cell(2, 24).Value = wsDay.Cell(2, 34).Value = "CompleteTime";
                wsDay.Cell(2, 5).Value = wsDay.Cell(2, 15).Value = wsDay.Cell(2, 25).Value = wsDay.Cell(2, 35).Value = "PlanTime";
                wsDay.Cell(2, 6).Value = wsDay.Cell(2, 16).Value = wsDay.Cell(2, 26).Value = wsDay.Cell(2, 36).Value = "PlanEndTime";
                wsDay.Cell(2, 7).Value = wsDay.Cell(2, 17).Value = wsDay.Cell(2, 27).Value = wsDay.Cell(2, 37).Value = "Status";

                for (int index = 1; index <= datasDay.D650PTDay.Count; index++)
                {
                    wsDay.Cell(index + 2, 1).Value = datasDay.D650PTDay[index - 1].Ws;
                    wsDay.Cell(index + 2, 2).Value = datasDay.D650PTDay[index - 1].Product;
                    wsDay.Cell(index + 2, 3).Value = datasDay.D650PTDay[index - 1].Job;
                    wsDay.Cell(index + 2, 4).Value = datasDay.D650PTDay[index - 1].Endtime;
                    wsDay.Cell(index + 2, 5).Value = datasDay.D650PTDay[index - 1].Plantime;
                    wsDay.Cell(index + 2, 6).Value = datasDay.D650PTDay[index - 1].PlanEndtime;
                    wsDay.Cell(index + 2, 7).Value = datasDay.D650PTDay[index - 1].Status;
                }

                for (int index = 1; index <= datasDay.D750PTDay.Count; index++)
                {
                    wsDay.Cell(index + 2, 11).Value = datasDay.D750PTDay[index - 1].Ws;
                    wsDay.Cell(index + 2, 12).Value = datasDay.D750PTDay[index - 1].Product;
                    wsDay.Cell(index + 2, 13).Value = datasDay.D750PTDay[index - 1].Job;
                    wsDay.Cell(index + 2, 14).Value = datasDay.D750PTDay[index - 1].Endtime;
                    wsDay.Cell(index + 2, 15).Value = datasDay.D750PTDay[index - 1].Plantime;
                    wsDay.Cell(index + 2, 16).Value = datasDay.D750PTDay[index - 1].PlanEndtime;
                    wsDay.Cell(index + 2, 17).Value = datasDay.D750PTDay[index - 1].Status;
                }

                for (int index = 1; index <= datasDay.D1000PTDay.Count; index++)
                {
                    wsDay.Cell(index + 2, 21).Value = datasDay.D1000PTDay[index - 1].Ws;
                    wsDay.Cell(index + 2, 22).Value = datasDay.D1000PTDay[index - 1].Product;
                    wsDay.Cell(index + 2, 23).Value = datasDay.D1000PTDay[index - 1].Job;
                    wsDay.Cell(index + 2, 24).Value = datasDay.D1000PTDay[index - 1].Endtime;
                    wsDay.Cell(index + 2, 25).Value = datasDay.D1000PTDay[index - 1].Plantime;
                    wsDay.Cell(index + 2, 26).Value = datasDay.D1000PTDay[index - 1].PlanEndtime;
                    wsDay.Cell(index + 2, 27).Value = datasDay.D1000PTDay[index - 1].Status;
                }

                for (int index = 1; index <= datasDay.D1100PTDay.Count; index++)
                {
                    wsDay.Cell(index + 2, 31).Value = datasDay.D1100PTDay[index - 1].Ws;
                    wsDay.Cell(index + 2, 32).Value = datasDay.D1100PTDay[index - 1].Product;
                    wsDay.Cell(index + 2, 33).Value = datasDay.D1100PTDay[index - 1].Job;
                    wsDay.Cell(index + 2, 34).Value = datasDay.D1100PTDay[index - 1].Endtime;
                    wsDay.Cell(index + 2, 35).Value = datasDay.D1100PTDay[index - 1].Plantime;
                    wsDay.Cell(index + 2, 36).Value = datasDay.D1100PTDay[index - 1].PlanEndtime;
                    wsDay.Cell(index + 2, 37).Value = datasDay.D1100PTDay[index - 1].Status;
                }
                #endregion
                #region Week
                timeRequest.To = StartOfWeek(sDate, DayOfWeek.Monday).AddDays(7);
                timeRequest.From = StartOfWeek(sDate, DayOfWeek.Monday);

                var datas = new PtGluingWeekVM();
                datas = await _ptApiClient.GetPtGluingWeek(timeRequest);

                IXLWorksheet wsWeek = workbook.Worksheets.Add("Week");
                wsWeek.Cell(1, 1).Value = "Statistics";
                wsWeek.Cell(1, 2).Value = "D650";
                wsWeek.Cell(1, 3).Value = "D750";
                wsWeek.Cell(1, 4).Value = "D1000";
                wsWeek.Cell(1, 5).Value = "D1100";

                wsWeek.Cell(2, 1).Value = "Testing time";
                wsWeek.Cell(2, 2).Value = datas.D650PTWeek.TestingTimeAndPercent;
                wsWeek.Cell(2, 3).Value = datas.D750PTWeek.TestingTimeAndPercent;
                wsWeek.Cell(2, 4).Value = datas.D1000PTWeek.TestingTimeAndPercent;
                wsWeek.Cell(2, 5).Value = datas.D1100PTWeek.TestingTimeAndPercent;

                wsWeek.Cell(3, 1).Value = "Running time";
                wsWeek.Cell(3, 2).Value = datas.D650PTWeek.RunningTimeAndPercent;
                wsWeek.Cell(3, 3).Value = datas.D750PTWeek.RunningTimeAndPercent;
                wsWeek.Cell(3, 4).Value = datas.D1000PTWeek.RunningTimeAndPercent;
                wsWeek.Cell(3, 5).Value = datas.D1100PTWeek.RunningTimeAndPercent;

                wsWeek.Cell(4, 1).Value = "Not Running time";
                wsWeek.Cell(4, 2).Value = datas.D650PTWeek.NotRunningTimeAndPercent;
                wsWeek.Cell(4, 3).Value = datas.D750PTWeek.NotRunningTimeAndPercent;
                wsWeek.Cell(4, 4).Value = datas.D1000PTWeek.NotRunningTimeAndPercent;
                wsWeek.Cell(4, 5).Value = datas.D1100PTWeek.NotRunningTimeAndPercent;

                wsWeek.Cell(5, 1).Value = "No. Product Code Change";
                wsWeek.Cell(5, 2).Value = datas.D650PTWeek.ProductCodeChangeCount;
                wsWeek.Cell(5, 3).Value = datas.D750PTWeek.ProductCodeChangeCount;
                wsWeek.Cell(5, 4).Value = datas.D1000PTWeek.ProductCodeChangeCount;
                wsWeek.Cell(5, 5).Value = datas.D1100PTWeek.ProductCodeChangeCount;

                wsWeek.Cell(6, 1).Value = "Quantity";
                wsWeek.Cell(6, 2).Value = datas.D650PTWeek.Quantity;
                wsWeek.Cell(6, 3).Value = datas.D750PTWeek.Quantity;
                wsWeek.Cell(6, 4).Value = datas.D1000PTWeek.Quantity;
                wsWeek.Cell(6, 5).Value = datas.D1100PTWeek.Quantity;

                wsWeek.Cell(7, 1).Value = "Defect";
                wsWeek.Cell(7, 2).Value = datas.D650PTWeek.Waste;
                wsWeek.Cell(7, 3).Value = datas.D750PTWeek.Waste;
                wsWeek.Cell(7, 4).Value = datas.D1000PTWeek.Waste;
                wsWeek.Cell(7, 5).Value = datas.D1100PTWeek.Waste;
                #endregion
                #region Month
                timeRequest.To = new DateTime(sDate.Year, sDate.AddMonths(1).Month, 1);
                timeRequest.From = new DateTime(sDate.Year, sDate.Month, 1);

                var datasMonth = new PtGluingMonthYearVM();
                datasMonth = await _ptApiClient.GetPtGluingMonth(timeRequest);

                var d650_avaibility_uptime = (datasMonth.D650PTMonthYear.ActualWorkingTime == 0) ? 0 : Math.Round((decimal)((datasMonth.D650PTMonthYear.ActualWorkingTime - datasMonth.D650PTMonthYear.Downtime) * 100 / datasMonth.D650PTMonthYear.ActualWorkingTime), 1);
                var d650_capacityUtilization = (datasMonth.D650PTMonthYear.MaximumProductionCapacity == 0) ? 0 : Math.Round((decimal)(datasMonth.D650PTMonthYear.TotalOutput * 100 / datasMonth.D650PTMonthYear.MaximumProductionCapacity), 1);
                var d650_productionYield = (datasMonth.D650PTMonthYear.TotalOutput == 0) ? 0 : Math.Round((decimal)((datasMonth.D650PTMonthYear.TotalOutput - datasMonth.D650PTMonthYear.Waste) * 100 / datasMonth.D650PTMonthYear.TotalOutput), 1);
                var d650_OEE = Math.Round(((d650_avaibility_uptime / 100) * (d650_capacityUtilization / 100) * (d650_productionYield / 100) * 100), 1);

                var d750_avaibility_uptime = (datasMonth.D750PTMonthYear.ActualWorkingTime == 0) ? 0 : Math.Round((decimal)((datasMonth.D750PTMonthYear.ActualWorkingTime - datasMonth.D750PTMonthYear.Downtime) * 100 / datasMonth.D750PTMonthYear.ActualWorkingTime), 1);
                var d750_capacityUtilization = (datasMonth.D750PTMonthYear.MaximumProductionCapacity == 0) ? 0 : Math.Round((decimal)(datasMonth.D750PTMonthYear.TotalOutput * 100 / datasMonth.D750PTMonthYear.MaximumProductionCapacity), 1);
                var d750_productionYield = (datasMonth.D750PTMonthYear.TotalOutput == 0) ? 0 : Math.Round((decimal)((datasMonth.D750PTMonthYear.TotalOutput - datasMonth.D750PTMonthYear.Waste) * 100 / datasMonth.D750PTMonthYear.TotalOutput), 1);
                var d750_OEE = Math.Round(((d750_avaibility_uptime / 100) * (d750_capacityUtilization / 100) * (d750_productionYield / 100) * 100), 1);

                var d1000_avaibility_uptime = (datasMonth.D1000PTMonthYear.ActualWorkingTime == 0) ? 0 : Math.Round((decimal)((datasMonth.D1000PTMonthYear.ActualWorkingTime - datasMonth.D1000PTMonthYear.Downtime) * 100 / datasMonth.D1000PTMonthYear.ActualWorkingTime), 1);
                var d1000_capacityUtilization = (datasMonth.D1000PTMonthYear.MaximumProductionCapacity == 0) ? 0 : Math.Round((decimal)(datasMonth.D1000PTMonthYear.TotalOutput * 100 / datasMonth.D1000PTMonthYear.MaximumProductionCapacity), 1);
                var d1000_productionYield = (datasMonth.D1000PTMonthYear.TotalOutput == 0) ? 0 : Math.Round((decimal)((datasMonth.D1000PTMonthYear.TotalOutput - datasMonth.D1000PTMonthYear.Waste) * 100 / datasMonth.D1000PTMonthYear.TotalOutput), 1);
                var d1000_OEE = Math.Round(((d1000_avaibility_uptime / 100) * (d1000_capacityUtilization / 100) * (d1000_productionYield / 100) * 100), 1);

                var d1100_avaibility_uptime = (datasMonth.D1100PTMonthYear.ActualWorkingTime == 0) ? 0 : Math.Round((decimal)((datasMonth.D1100PTMonthYear.ActualWorkingTime - datasMonth.D1100PTMonthYear.Downtime) * 100 / datasMonth.D1100PTMonthYear.ActualWorkingTime), 1);
                var d1100_capacityUtilization = (datasMonth.D1100PTMonthYear.MaximumProductionCapacity == 0) ? 0 : Math.Round((decimal)(datasMonth.D1100PTMonthYear.TotalOutput * 100 / datasMonth.D1100PTMonthYear.MaximumProductionCapacity), 1);
                var d1100_productionYield = (datasMonth.D1100PTMonthYear.TotalOutput == 0) ? 0 : Math.Round((decimal)((datasMonth.D1100PTMonthYear.TotalOutput - datasMonth.D1100PTMonthYear.Waste) * 100 / datasMonth.D1100PTMonthYear.TotalOutput), 1);
                var d1100_OEE = Math.Round(((d1100_avaibility_uptime / 100) * (d1100_capacityUtilization / 100) * (d1100_productionYield / 100) * 100), 1);

                IXLWorksheet wsMonth = workbook.Worksheets.Add("Month");
                wsMonth.Cell(1, 1).Value = "Statistics";
                wsMonth.Cell(1, 2).Value = "D650";
                wsMonth.Cell(1, 3).Value = "D750";
                wsMonth.Cell(1, 4).Value = "D1000";
                wsMonth.Cell(1, 5).Value = "D1100";

                wsMonth.Cell(2, 1).Value = "Actual working time (h)";
                wsMonth.Cell(2, 2).Value = datasMonth.D650PTMonthYear.ActualWorkingTime;
                wsMonth.Cell(2, 3).Value = datasMonth.D750PTMonthYear.ActualWorkingTime;
                wsMonth.Cell(2, 4).Value = datasMonth.D1000PTMonthYear.ActualWorkingTime;
                wsMonth.Cell(2, 5).Value = datasMonth.D1100PTMonthYear.ActualWorkingTime;

                wsMonth.Cell(3, 1).Value = "Downtime (h)";
                wsMonth.Cell(3, 2).Value = datasMonth.D650PTMonthYear.Downtime;
                wsMonth.Cell(3, 3).Value = datasMonth.D750PTMonthYear.Downtime;
                wsMonth.Cell(3, 4).Value = datasMonth.D1000PTMonthYear.Downtime;
                wsMonth.Cell(3, 5).Value = datasMonth.D1100PTMonthYear.Downtime;

                wsMonth.Cell(4, 1).Value = "Total Quantity (pcs)";
                wsMonth.Cell(4, 2).Value = datasMonth.D650PTMonthYear.TotalOutput;
                wsMonth.Cell(4, 3).Value = datasMonth.D750PTMonthYear.TotalOutput;
                wsMonth.Cell(4, 4).Value = datasMonth.D1000PTMonthYear.TotalOutput;
                wsMonth.Cell(4, 5).Value = datasMonth.D1100PTMonthYear.TotalOutput;

                wsMonth.Cell(5, 1).Value = "Maximum production capacity (pcs)";
                wsMonth.Cell(5, 2).Value = datasMonth.D650PTMonthYear.MaximumProductionCapacity;
                wsMonth.Cell(5, 3).Value = datasMonth.D750PTMonthYear.MaximumProductionCapacity;
                wsMonth.Cell(5, 4).Value = datasMonth.D1000PTMonthYear.MaximumProductionCapacity;
                wsMonth.Cell(5, 5).Value = datasMonth.D1100PTMonthYear.MaximumProductionCapacity;

                wsMonth.Cell(6, 1).Value = "Total Defect (pcs)";
                wsMonth.Cell(6, 2).Value = datasMonth.D650PTMonthYear.Waste;
                wsMonth.Cell(6, 3).Value = datasMonth.D750PTMonthYear.Waste;
                wsMonth.Cell(6, 4).Value = datasMonth.D1000PTMonthYear.Waste;
                wsMonth.Cell(6, 5).Value = datasMonth.D1100PTMonthYear.Waste;

                wsMonth.Cell(7, 1).Value = "Avaibility/Uptime (%)";
                wsMonth.Cell(7, 2).Value = d650_avaibility_uptime;
                wsMonth.Cell(7, 3).Value = d750_avaibility_uptime;
                wsMonth.Cell(7, 4).Value = d1000_avaibility_uptime;
                wsMonth.Cell(7, 5).Value = d1100_avaibility_uptime;

                wsMonth.Cell(8, 1).Value = "Capacity utilization (%)";
                wsMonth.Cell(8, 2).Value = d650_capacityUtilization;
                wsMonth.Cell(8, 3).Value = d750_capacityUtilization;
                wsMonth.Cell(8, 4).Value = d1000_capacityUtilization;
                wsMonth.Cell(8, 5).Value = d1100_capacityUtilization;

                wsMonth.Cell(9, 1).Value = "Production yield (%)";
                wsMonth.Cell(9, 2).Value = d650_productionYield;
                wsMonth.Cell(9, 3).Value = d750_productionYield;
                wsMonth.Cell(9, 4).Value = d1000_productionYield;
                wsMonth.Cell(9, 5).Value = d1100_productionYield;

                wsMonth.Cell(10, 1).Value = "OEE (%)";
                wsMonth.Cell(10, 2).Value = d650_OEE;
                wsMonth.Cell(10, 3).Value = d750_OEE;
                wsMonth.Cell(10, 4).Value = d1000_OEE;
                wsMonth.Cell(10, 5).Value = d1100_OEE;
                #endregion
                #region Year
                timeRequest.To = new DateTime(sDate.AddYears(1).Year, 1, 1);
                timeRequest.From = new DateTime(sDate.Year, 1, 1);

                var datasYear = new PtGluingMonthYearVM();
                datasYear = await _ptApiClient.GetPtGluingYear(timeRequest);

                var d650_avaibility_uptime_y = (datasYear.D650PTMonthYear.ActualWorkingTime == 0) ? 0 : Math.Round((decimal)((datasYear.D650PTMonthYear.ActualWorkingTime - datasYear.D650PTMonthYear.Downtime) * 100 / datasYear.D650PTMonthYear.ActualWorkingTime), 1);
                var d650_capacityUtilization_y = (datasYear.D650PTMonthYear.MaximumProductionCapacity == 0) ? 0 : Math.Round((decimal)(datasYear.D650PTMonthYear.TotalOutput * 100 / datasYear.D650PTMonthYear.MaximumProductionCapacity), 1);
                var d650_productionYield_y = (datasYear.D650PTMonthYear.TotalOutput == 0) ? 0 : Math.Round((decimal)((datasYear.D650PTMonthYear.TotalOutput - datasYear.D650PTMonthYear.Waste) * 100 / datasYear.D650PTMonthYear.TotalOutput), 1);
                var d650_OEE_y = Math.Round(((d650_avaibility_uptime_y / 100) * (d650_capacityUtilization_y / 100) * (d650_productionYield_y / 100) * 100), 1);

                var d750_avaibility_uptime_y = (datasYear.D750PTMonthYear.ActualWorkingTime == 0) ? 0 : Math.Round((decimal)((datasYear.D750PTMonthYear.ActualWorkingTime - datasYear.D750PTMonthYear.Downtime) * 100 / datasYear.D750PTMonthYear.ActualWorkingTime), 1);
                var d750_capacityUtilization_y = (datasYear.D750PTMonthYear.MaximumProductionCapacity == 0) ? 0 : Math.Round((decimal)(datasYear.D750PTMonthYear.TotalOutput * 100 / datasYear.D750PTMonthYear.MaximumProductionCapacity), 1);
                var d750_productionYield_y = (datasYear.D750PTMonthYear.TotalOutput == 0) ? 0 : Math.Round((decimal)((datasYear.D750PTMonthYear.TotalOutput - datasYear.D750PTMonthYear.Waste) * 100 / datasYear.D750PTMonthYear.TotalOutput), 1);
                var d750_OEE_y = Math.Round(((d750_avaibility_uptime_y / 100) * (d750_capacityUtilization_y / 100) * (d750_productionYield_y / 100) * 100), 1);

                var d1000_avaibility_uptime_y = (datasYear.D1000PTMonthYear.ActualWorkingTime == 0) ? 0 : Math.Round((decimal)((datasYear.D1000PTMonthYear.ActualWorkingTime - datasYear.D1000PTMonthYear.Downtime) * 100 / datasYear.D1000PTMonthYear.ActualWorkingTime), 1);
                var d1000_capacityUtilization_y = (datasYear.D1000PTMonthYear.MaximumProductionCapacity == 0) ? 0 : Math.Round((decimal)(datasYear.D1000PTMonthYear.TotalOutput * 100 / datasYear.D1000PTMonthYear.MaximumProductionCapacity), 1);
                var d1000_productionYield_y = (datasYear.D1000PTMonthYear.TotalOutput == 0) ? 0 : Math.Round((decimal)((datasYear.D1000PTMonthYear.TotalOutput - datasYear.D1000PTMonthYear.Waste) * 100 / datasYear.D1000PTMonthYear.TotalOutput), 1);
                var d1000_OEE_y = Math.Round(((d1000_avaibility_uptime_y / 100) * (d1000_capacityUtilization_y / 100) * (d1000_productionYield_y / 100) * 100), 1);

                var d1100_avaibility_uptime_y = (datasYear.D1100PTMonthYear.ActualWorkingTime == 0) ? 0 : Math.Round((decimal)((datasYear.D1100PTMonthYear.ActualWorkingTime - datasYear.D1100PTMonthYear.Downtime) * 100 / datasYear.D1100PTMonthYear.ActualWorkingTime), 1);
                var d1100_capacityUtilization_y = (datasYear.D1100PTMonthYear.MaximumProductionCapacity == 0) ? 0 : Math.Round((decimal)(datasYear.D1100PTMonthYear.TotalOutput * 100 / datasYear.D1100PTMonthYear.MaximumProductionCapacity), 1);
                var d1100_productionYield_y = (datasYear.D1100PTMonthYear.TotalOutput == 0) ? 0 : Math.Round((decimal)((datasYear.D1100PTMonthYear.TotalOutput - datasYear.D1100PTMonthYear.Waste) * 100 / datasYear.D1100PTMonthYear.TotalOutput), 1);
                var d1100_OEE_y = Math.Round(((d1100_avaibility_uptime_y / 100) * (d1100_capacityUtilization_y / 100) * (d1100_productionYield_y / 100) * 100), 1);

                IXLWorksheet wsYear = workbook.Worksheets.Add("Year");
                wsYear.Cell(1, 1).Value = "Statistics";
                wsYear.Cell(1, 2).Value = "D650";
                wsYear.Cell(1, 3).Value = "D750";
                wsYear.Cell(1, 4).Value = "D1000";
                wsYear.Cell(1, 5).Value = "D1100";

                wsYear.Cell(2, 1).Value = "Actual working time (h)";
                wsYear.Cell(2, 2).Value = datasYear.D650PTMonthYear.ActualWorkingTime;
                wsYear.Cell(2, 3).Value = datasYear.D750PTMonthYear.ActualWorkingTime;
                wsYear.Cell(2, 4).Value = datasYear.D1000PTMonthYear.ActualWorkingTime;
                wsYear.Cell(2, 5).Value = datasYear.D1100PTMonthYear.ActualWorkingTime;

                wsYear.Cell(3, 1).Value = "Downtime (h)";
                wsYear.Cell(3, 2).Value = datasYear.D650PTMonthYear.Downtime;
                wsYear.Cell(3, 3).Value = datasYear.D750PTMonthYear.Downtime;
                wsYear.Cell(3, 4).Value = datasYear.D1000PTMonthYear.Downtime;
                wsYear.Cell(3, 5).Value = datasYear.D1100PTMonthYear.Downtime;

                wsYear.Cell(4, 1).Value = "Total Quantity (pcs)";
                wsYear.Cell(4, 2).Value = datasYear.D650PTMonthYear.TotalOutput;
                wsYear.Cell(4, 3).Value = datasYear.D750PTMonthYear.TotalOutput;
                wsYear.Cell(4, 4).Value = datasYear.D1000PTMonthYear.TotalOutput;
                wsYear.Cell(4, 5).Value = datasYear.D1100PTMonthYear.TotalOutput;

                wsYear.Cell(5, 1).Value = "Maximum production capacity (pcs)";
                wsYear.Cell(5, 2).Value = datasYear.D650PTMonthYear.MaximumProductionCapacity;
                wsYear.Cell(5, 3).Value = datasYear.D750PTMonthYear.MaximumProductionCapacity;
                wsYear.Cell(5, 4).Value = datasYear.D1000PTMonthYear.MaximumProductionCapacity;
                wsYear.Cell(5, 5).Value = datasYear.D1100PTMonthYear.MaximumProductionCapacity;

                wsYear.Cell(6, 1).Value = "Total Defect (pcs)";
                wsYear.Cell(6, 2).Value = datasYear.D650PTMonthYear.Waste;
                wsYear.Cell(6, 3).Value = datasYear.D750PTMonthYear.Waste;
                wsYear.Cell(6, 4).Value = datasYear.D1000PTMonthYear.Waste;
                wsYear.Cell(6, 5).Value = datasYear.D1100PTMonthYear.Waste;

                wsYear.Cell(7, 1).Value = "Avaibility/Uptime (%)";
                wsYear.Cell(7, 2).Value = d650_avaibility_uptime_y;
                wsYear.Cell(7, 3).Value = d750_avaibility_uptime_y;
                wsYear.Cell(7, 4).Value = d1000_avaibility_uptime_y;
                wsYear.Cell(7, 5).Value = d1100_avaibility_uptime_y;

                wsYear.Cell(8, 1).Value = "Capacity utilization (%)";
                wsYear.Cell(8, 2).Value = d650_capacityUtilization_y;
                wsYear.Cell(8, 3).Value = d750_capacityUtilization_y;
                wsYear.Cell(8, 4).Value = d1000_capacityUtilization_y;
                wsYear.Cell(8, 5).Value = d1100_capacityUtilization_y;

                wsYear.Cell(9, 1).Value = "Production yield (%)";
                wsYear.Cell(9, 2).Value = d650_productionYield_y;
                wsYear.Cell(9, 3).Value = d750_productionYield_y;
                wsYear.Cell(9, 4).Value = d1000_productionYield_y;
                wsYear.Cell(9, 5).Value = d1100_productionYield_y;

                wsYear.Cell(10, 1).Value = "OEE (%)";
                wsYear.Cell(10, 2).Value = d650_OEE_y;
                wsYear.Cell(10, 3).Value = d750_OEE_y;
                wsYear.Cell(10, 4).Value = d1000_OEE_y;
                wsYear.Cell(10, 5).Value = d1100_OEE_y;
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
