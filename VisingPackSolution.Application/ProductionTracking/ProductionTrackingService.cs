using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisingPackSolution.Data.EF;
using VisingPackSolution.ViewModels.Common;
using VisingPackSolution.ViewModels.ProductionTracking;
using VisingPackSolution.ViewModels.WorkTimeManage;

namespace VisingPackSolution.Application.ProductionTracking
{
    public class ProductionTrackingService : IProductionTrackingService
    {
        private readonly VisingPackMMSDbContext _context;

        public ProductionTrackingService(VisingPackMMSDbContext context)
        {
            _context = context;
        }

        #region Printing
        #region 1Hour
        public async Task<List<Pt1HVM>> GetP601ProductionTracking_1H(TimeRequest request)
        {
            var query =             from a in _context.P601PoKpis
                join b in _context.P601PoInfos on a.Ws equals b.Ws
                where a.Starttime >= request.From && a.Starttime <= request.To && a.Ws != ""
                orderby a.Starttime descending
                select new Pt1HVM
                {
                    Ws = a.Ws,
                    Product = b.Product,
                    Job = a.Job,
                    Target = a.Target,
                };
            var data = await query.ToListAsync();

            return data;
        }
        public async Task<List<Pt1HVM>> GetP604ProductionTracking_1H(TimeRequest request)
        {
            var query = from a in _context.P604PoKpis
                        join b in _context.P604PoInfos on a.Ws equals b.Ws
                        where a.Starttime >= request.From && a.Starttime <= request.To && a.Ws != ""
                        orderby a.Starttime descending
                        select new Pt1HVM
                        {
                            Ws = a.Ws,
                            Product = b.Product,
                            Job = a.Job,
                            Target = a.Target,
                        };
            var data = await query.ToListAsync();

            return data;
        }
        public async Task<List<Pt1HVM>> GetP605ProductionTracking_1H(TimeRequest request)
        {
            var query = from a in _context.P605PoKpis
                        join b in _context.P605PoInfos on a.Ws equals b.Ws
                        where a.Starttime >= request.From && a.Starttime <= request.To && a.Ws != ""
                        orderby a.Starttime descending
                        select new Pt1HVM
                        {
                            Ws = a.Ws,
                            Product = b.Product,
                            Job = a.Job,
                            Target = a.Target,
                        };
            var data = await query.ToListAsync();

            return data;
        }
        public async Task<List<Pt1HVM>> GetP5mProductionTracking_1H(TimeRequest request)
        {
            var query = from a in _context.P5mPoKpis
                        join b in _context.P5mPoInfos on a.Ws equals b.Ws
                        where a.Starttime >= request.From && a.Starttime <= request.To && a.Ws != ""
                        orderby a.Starttime descending
                        select new Pt1HVM
                        {
                            Ws = a.Ws,
                            Product = b.Product,
                            Job = a.Job,
                            Target = a.Target,
                        };
            var data = await query.ToListAsync();

            return data;
        }
        #endregion
        #region Day
        public async Task<List<PtDayVM>> GetP601ProductionTracking_Day(TimeRequest request)
        {
            var query = from a in _context.P601PoKpis
                        join b in _context.P601PoInfos on a.Ws equals b.Ws
                        where a.Starttime >= request.From && a.Starttime <= request.To && a.Ws != ""
                        orderby a.Starttime descending
                        select new PtDayVM
                        {
                            Ws = a.Ws,
                            Product = b.Product,
                            Job = a.Job,
                            Endtime = a.Endtime,
                            Plantime = b.Plantime,
                            PlanEndtime = b.PlanEndtime,
                            Status = a.Status,
                        };
            var data = await query.ToListAsync();

            return data;
        }
        public async Task<List<PtDayVM>> GetP604ProductionTracking_Day(TimeRequest request)
        {
            DateTime dt = DateTime.Now.AddDays(-1);

            var query = from a in _context.P604PoKpis
                        join b in _context.P604PoInfos on a.Ws equals b.Ws
                        where a.Starttime >= request.From && a.Starttime <= request.To && a.Ws != ""
                        orderby a.Starttime descending
                        select new PtDayVM
                        {
                            Ws = a.Ws,
                            Product = b.Product,
                            Job = a.Job,
                            Endtime = a.Endtime,
                            Plantime = b.Plantime,
                            PlanEndtime = b.PlanEndtime,
                            Status = a.Status,
                        };
            var data = await query.ToListAsync();

            return data;
        }
        public async Task<List<PtDayVM>> GetP605ProductionTracking_Day(TimeRequest request)
        {
            DateTime dt = DateTime.Now.AddDays(-1);

            var query = from a in _context.P605PoKpis
                        join b in _context.P605PoInfos on a.Ws equals b.Ws
                        where a.Starttime >= request.From && a.Starttime <= request.To && a.Ws != ""
                        orderby a.Starttime descending
                        select new PtDayVM
                        {
                            Ws = a.Ws,
                            Product = b.Product,
                            Job = a.Job,
                            Endtime = a.Endtime,
                            Plantime = b.Plantime,
                            PlanEndtime = b.PlanEndtime,
                            Status = a.Status,
                        };
            var data = await query.ToListAsync();

            return data;
        }
        public async Task<List<PtDayVM>> GetP5mProductionTracking_Day(TimeRequest request)
        {
            DateTime dt = DateTime.Now.AddDays(-1);

            var query = from a in _context.P5mPoKpis
                        join b in _context.P5mPoInfos on a.Ws equals b.Ws
                        where a.Starttime >= request.From && a.Starttime <= request.To && a.Ws != ""
                        orderby a.Starttime descending
                        select new PtDayVM
                        {
                            Ws = a.Ws,
                            Product = b.Product,
                            Job = a.Job,
                            Endtime = a.Endtime,
                            Plantime = b.Plantime,
                            PlanEndtime = b.PlanEndtime,
                            Status = a.Status,
                        };
            var data = await query.ToListAsync();

            return data;
        }
        #endregion
        #region Week
        public async Task<PtWeekVM> GetP601ProductionTracking_Week(TimeRequest request)
        {
            var query1 = from p in _context.TableDays
                         select new { p };
            query1 = query1.Where(w => w.p.Date >= request.From && w.p.Date <= request.To);
            if (!query1.Any())
            {
                var empty = new WorkTimeVM() { RunningTime = 0, TestingTime = 0, OtherTime = 0, BreakTime = 0, FixingTime = 0, PendingTime = 0, MaintenanceTime = 0, PauseTime = 0 };
                return new PtWeekVM()
                {
                    TestingTime = 0,
                    RunningTime = 0,
                    NotRunningTime = 0,
                    TotalTime = 0,
                    TestingTimeAndPercent = "0h 0%",
                    RunningTimeAndPercent = "0h 0%",
                    NotRunningTimeAndPercent = "0h 0%",
                    ProductCodeChangeCount = 0,
                    Quantity = 0,
                    Waste = 0,
                };
            }

            var wtVM = query1.Where(w => w.p.Machine == "P601")
            .GroupBy(g => g.p.Machine)
            .Select(s => new WorkTimeVM()
            {
                RunningTime = (s.Sum(a => a.p.RunningTime) == null) ? 0 : s.Sum(a => a.p.RunningTime),
                TestingTime = (s.Sum(a => a.p.TestingTime) == null) ? 0 : s.Sum(a => a.p.TestingTime),
                OtherTime = (s.Sum(a => a.p.OtherTime) == null) ? 0 : s.Sum(a => a.p.OtherTime),
                BreakTime = (s.Sum(a => a.p.BreakTime) == null) ? 0 : s.Sum(a => a.p.BreakTime),
                FixingTime = (s.Sum(a => a.p.FixingTime) == null) ? 0 : s.Sum(a => a.p.FixingTime),
                PendingTime = (s.Sum(a => a.p.PendingTime) == null) ? 0 : s.Sum(a => a.p.PendingTime),
                MaintenanceTime = (s.Sum(a => a.p.MaintenanceTime) == null) ? 0 : s.Sum(a => a.p.MaintenanceTime),
                PauseTime = (s.Sum(a => a.p.PauseTime) == null) ? 0 : s.Sum(a => a.p.PauseTime),
            }).FirstOrDefault();

            var query2 = (from a in _context.P601PoKpis
                          join b in _context.P601PoInfos on a.Ws equals b.Ws
                          where a.Starttime >= request.From && a.Starttime <= request.To && a.Ws != ""
                          select new { a, b });



            var totalTime = (wtVM is null) ? 0 : wtVM.TestingTime + wtVM.RunningTime + wtVM.OtherTime + wtVM.BreakTime + wtVM.FixingTime + wtVM.PendingTime + wtVM.MaintenanceTime + wtVM.PauseTime;
            var notRunningTime = (wtVM is null) ? 0 : wtVM.OtherTime + wtVM.BreakTime + wtVM.FixingTime + wtVM.PendingTime + wtVM.MaintenanceTime + wtVM.PauseTime;
            var productCodeChange = await query2.Select(x => x.b.Product).Distinct().CountAsync();
            var quantity = await query2.Select(x => x.a.Job).SumAsync();
            var waste = await query2.Select(x => x.a.DefeatItem1 + x.a.DefeatItem2 + x.a.DefeatItem3 + x.a.DefeatItem4).SumAsync();

            var data = new PtWeekVM()
            {
                TestingTime = (wtVM is null) ? 0 : wtVM.TestingTime,
                RunningTime = (wtVM is null) ? 0 : wtVM.RunningTime,
                NotRunningTime = notRunningTime,
                TotalTime = totalTime,
                TestingTimeAndPercent = (wtVM is null || totalTime == 0) ? "0h 0%" : Math.Round((decimal)wtVM.TestingTime, 1).ToString() + "h " + Math.Round((decimal)(wtVM.TestingTime * 100 / totalTime), 1).ToString() + "%",
                RunningTimeAndPercent = (wtVM is null || totalTime == 0) ? "0h 0%" : Math.Round((decimal)wtVM.RunningTime, 1).ToString() + "h " + Math.Round((decimal)(wtVM.RunningTime * 100 / totalTime), 1).ToString() + "%",
                NotRunningTimeAndPercent = (totalTime == 0) ? "0h 0%" : Math.Round((decimal)notRunningTime, 1).ToString() + "h " + Math.Round((decimal)(notRunningTime * 100 / totalTime), 1).ToString() + "%",
                ProductCodeChangeCount = productCodeChange,
                Quantity = quantity,
                Waste = waste,
            };

            return data;
        }
        public async Task<PtWeekVM> GetP604ProductionTracking_Week(TimeRequest request)
        {
            var query1 = from p in _context.TableDays
                         select new { p };
            query1 = query1.Where(w => w.p.Date >= request.From && w.p.Date <= request.To);
            if (!query1.Any())
            {
                var empty = new WorkTimeVM() { RunningTime = 0, TestingTime = 0, OtherTime = 0, BreakTime = 0, FixingTime = 0, PendingTime = 0, MaintenanceTime = 0, PauseTime = 0 };
                return new PtWeekVM()
                {
                    TestingTime = 0,
                    RunningTime = 0,
                    NotRunningTime = 0,
                    TotalTime = 0,
                    TestingTimeAndPercent = "0h 0%",
                    RunningTimeAndPercent = "0h 0%",
                    NotRunningTimeAndPercent = "0h 0%",
                    ProductCodeChangeCount = 0,
                    Quantity = 0,
                    Waste = 0,
                };
            }

            var wtVM = query1.Where(w => w.p.Machine == "P604")
            .GroupBy(g => g.p.Machine)
            .Select(s => new WorkTimeVM()
            {
                RunningTime = (s.Sum(a => a.p.RunningTime) == null) ? 0 : s.Sum(a => a.p.RunningTime),
                TestingTime = (s.Sum(a => a.p.TestingTime) == null) ? 0 : s.Sum(a => a.p.TestingTime),
                OtherTime = (s.Sum(a => a.p.OtherTime) == null) ? 0 : s.Sum(a => a.p.OtherTime),
                BreakTime = (s.Sum(a => a.p.BreakTime) == null) ? 0 : s.Sum(a => a.p.BreakTime),
                FixingTime = (s.Sum(a => a.p.FixingTime) == null) ? 0 : s.Sum(a => a.p.FixingTime),
                PendingTime = (s.Sum(a => a.p.PendingTime) == null) ? 0 : s.Sum(a => a.p.PendingTime),
                MaintenanceTime = (s.Sum(a => a.p.MaintenanceTime) == null) ? 0 : s.Sum(a => a.p.MaintenanceTime),
                PauseTime = (s.Sum(a => a.p.PauseTime) == null) ? 0 : s.Sum(a => a.p.PauseTime),
            }).FirstOrDefault();

            var query2 = (from a in _context.P604PoKpis
                          join b in _context.P604PoInfos on a.Ws equals b.Ws
                          where a.Starttime >= request.From && a.Starttime <= request.To && a.Ws != ""
                          select new { a, b });



            var totalTime = (wtVM is null) ? 0 : wtVM.TestingTime + wtVM.RunningTime + wtVM.OtherTime + wtVM.BreakTime + wtVM.FixingTime + wtVM.PendingTime + wtVM.MaintenanceTime + wtVM.PauseTime;
            var notRunningTime = (wtVM is null) ? 0 : wtVM.OtherTime + wtVM.BreakTime + wtVM.FixingTime + wtVM.PendingTime + wtVM.MaintenanceTime + wtVM.PauseTime;
            var productCodeChange = await query2.Select(x => x.b.Product).Distinct().CountAsync();
            var quantity = await query2.Select(x => x.a.Job).SumAsync();
            var waste = await query2.Select(x => x.a.DefeatItem1 + x.a.DefeatItem2 + x.a.DefeatItem3 + x.a.DefeatItem4).SumAsync();

            var data = new PtWeekVM()
            {
                TestingTime = (wtVM is null) ? 0 : wtVM.TestingTime,
                RunningTime = (wtVM is null) ? 0 : wtVM.RunningTime,
                NotRunningTime = notRunningTime,
                TotalTime = totalTime,
                TestingTimeAndPercent = (wtVM is null || totalTime == 0) ? "0h 0%" : Math.Round((decimal)wtVM.TestingTime, 1).ToString() + "h " + Math.Round((decimal)(wtVM.TestingTime * 100 / totalTime), 1).ToString() + "%",
                RunningTimeAndPercent = (wtVM is null || totalTime == 0) ? "0h 0%" : Math.Round((decimal)wtVM.RunningTime, 1).ToString() + "h " + Math.Round((decimal)(wtVM.RunningTime * 100 / totalTime), 1).ToString() + "%",
                NotRunningTimeAndPercent = (totalTime == 0) ? "0h 0%" : Math.Round((decimal)notRunningTime, 1).ToString() + "h " + Math.Round((decimal)(notRunningTime * 100 / totalTime), 1).ToString() + "%",
                ProductCodeChangeCount = productCodeChange,
                Quantity = quantity,
                Waste = waste,
            };

            return data;
        }
        public async Task<PtWeekVM> GetP605ProductionTracking_Week(TimeRequest request)
        {
            var query1 = from p in _context.TableDays
                         select new { p };
            query1 = query1.Where(w => w.p.Date >= request.From && w.p.Date <= request.To);
            if (!query1.Any())
            {
                var empty = new WorkTimeVM() { RunningTime = 0, TestingTime = 0, OtherTime = 0, BreakTime = 0, FixingTime = 0, PendingTime = 0, MaintenanceTime = 0, PauseTime = 0 };
                return new PtWeekVM()
                {
                    TestingTime = 0,
                    RunningTime = 0,
                    NotRunningTime = 0,
                    TotalTime = 0,
                    TestingTimeAndPercent = "0h 0%",
                    RunningTimeAndPercent = "0h 0%",
                    NotRunningTimeAndPercent = "0h 0%",
                    ProductCodeChangeCount = 0,
                    Quantity = 0,
                    Waste = 0,
                };
            }

            var wtVM = query1.Where(w => w.p.Machine == "P605")
            .GroupBy(g => g.p.Machine)
            .Select(s => new WorkTimeVM()
            {
                RunningTime = (s.Sum(a => a.p.RunningTime) == null) ? 0 : s.Sum(a => a.p.RunningTime),
                TestingTime = (s.Sum(a => a.p.TestingTime) == null) ? 0 : s.Sum(a => a.p.TestingTime),
                OtherTime = (s.Sum(a => a.p.OtherTime) == null) ? 0 : s.Sum(a => a.p.OtherTime),
                BreakTime = (s.Sum(a => a.p.BreakTime) == null) ? 0 : s.Sum(a => a.p.BreakTime),
                FixingTime = (s.Sum(a => a.p.FixingTime) == null) ? 0 : s.Sum(a => a.p.FixingTime),
                PendingTime = (s.Sum(a => a.p.PendingTime) == null) ? 0 : s.Sum(a => a.p.PendingTime),
                MaintenanceTime = (s.Sum(a => a.p.MaintenanceTime) == null) ? 0 : s.Sum(a => a.p.MaintenanceTime),
                PauseTime = (s.Sum(a => a.p.PauseTime) == null) ? 0 : s.Sum(a => a.p.PauseTime),
            }).FirstOrDefault();

            var query2 = (from a in _context.P605PoKpis
                          join b in _context.P605PoInfos on a.Ws equals b.Ws
                          where a.Starttime >= request.From && a.Starttime <= request.To && a.Ws != ""
                          select new { a, b });



            var totalTime = (wtVM is null) ? 0 : wtVM.TestingTime + wtVM.RunningTime + wtVM.OtherTime + wtVM.BreakTime + wtVM.FixingTime + wtVM.PendingTime + wtVM.MaintenanceTime + wtVM.PauseTime;
            var notRunningTime = (wtVM is null) ? 0 : wtVM.OtherTime + wtVM.BreakTime + wtVM.FixingTime + wtVM.PendingTime + wtVM.MaintenanceTime + wtVM.PauseTime;
            var productCodeChange = await query2.Select(x => x.b.Product).Distinct().CountAsync();
            var quantity = await query2.Select(x => x.a.Job).SumAsync();
            var waste = await query2.Select(x => x.a.DefeatItem1 + x.a.DefeatItem2 + x.a.DefeatItem3 + x.a.DefeatItem4).SumAsync();

            var data = new PtWeekVM()
            {
                TestingTime = (wtVM is null) ? 0 : wtVM.TestingTime,
                RunningTime = (wtVM is null) ? 0 : wtVM.RunningTime,
                NotRunningTime = notRunningTime,
                TotalTime = totalTime,
                TestingTimeAndPercent = (wtVM is null || totalTime == 0) ? "0h 0%" : Math.Round((decimal)wtVM.TestingTime, 1).ToString() + "h " + Math.Round((decimal)(wtVM.TestingTime * 100 / totalTime), 1).ToString() + "%",
                RunningTimeAndPercent = (wtVM is null || totalTime == 0) ? "0h 0%" : Math.Round((decimal)wtVM.RunningTime, 1).ToString() + "h " + Math.Round((decimal)(wtVM.RunningTime * 100 / totalTime), 1).ToString() + "%",
                NotRunningTimeAndPercent = (totalTime == 0) ? "0h 0%" : Math.Round((decimal)notRunningTime, 1).ToString() + "h " + Math.Round((decimal)(notRunningTime * 100 / totalTime), 1).ToString() + "%",
                ProductCodeChangeCount = productCodeChange,
                Quantity = quantity,
                Waste = waste,
            };

            return data;
        }
        public async Task<PtWeekVM> GetP5mProductionTracking_Week(TimeRequest request)
        {
            var query1 = from p in _context.TableDays
                         select new { p };
            query1 = query1.Where(w => w.p.Date >= request.From && w.p.Date <= request.To);
            if (!query1.Any())
            {
                var empty = new WorkTimeVM() { RunningTime = 0, TestingTime = 0, OtherTime = 0, BreakTime = 0, FixingTime = 0, PendingTime = 0, MaintenanceTime = 0, PauseTime = 0 };
                return new PtWeekVM()
                {
                    TestingTime = 0,
                    RunningTime = 0,
                    NotRunningTime = 0,
                    TotalTime = 0,
                    TestingTimeAndPercent = "0h 0%",
                    RunningTimeAndPercent = "0h 0%",
                    NotRunningTimeAndPercent = "0h 0%",
                    ProductCodeChangeCount = 0,
                    Quantity = 0,
                    Waste = 0,
                };
            }

            var wtVM = query1.Where(w => w.p.Machine == "P5m")
            .GroupBy(g => g.p.Machine)
            .Select(s => new WorkTimeVM()
            {
                RunningTime = (s.Sum(a => a.p.RunningTime) == null) ? 0 : s.Sum(a => a.p.RunningTime),
                TestingTime = (s.Sum(a => a.p.TestingTime) == null) ? 0 : s.Sum(a => a.p.TestingTime),
                OtherTime = (s.Sum(a => a.p.OtherTime) == null) ? 0 : s.Sum(a => a.p.OtherTime),
                BreakTime = (s.Sum(a => a.p.BreakTime) == null) ? 0 : s.Sum(a => a.p.BreakTime),
                FixingTime = (s.Sum(a => a.p.FixingTime) == null) ? 0 : s.Sum(a => a.p.FixingTime),
                PendingTime = (s.Sum(a => a.p.PendingTime) == null) ? 0 : s.Sum(a => a.p.PendingTime),
                MaintenanceTime = (s.Sum(a => a.p.MaintenanceTime) == null) ? 0 : s.Sum(a => a.p.MaintenanceTime),
                PauseTime = (s.Sum(a => a.p.PauseTime) == null) ? 0 : s.Sum(a => a.p.PauseTime),
            }).FirstOrDefault();

            var query2 = (from a in _context.P5mPoKpis
                          join b in _context.P5mPoInfos on a.Ws equals b.Ws
                          where a.Starttime >= request.From && a.Starttime <= request.To && a.Ws != ""
                          select new { a, b });



            var totalTime = (wtVM is null) ? 0 : wtVM.TestingTime + wtVM.RunningTime + wtVM.OtherTime + wtVM.BreakTime + wtVM.FixingTime + wtVM.PendingTime + wtVM.MaintenanceTime + wtVM.PauseTime;
            var notRunningTime = (wtVM is null) ? 0 : wtVM.OtherTime + wtVM.BreakTime + wtVM.FixingTime + wtVM.PendingTime + wtVM.MaintenanceTime + wtVM.PauseTime;
            var productCodeChange = await query2.Select(x => x.b.Product).Distinct().CountAsync();
            var quantity = await query2.Select(x => x.a.Job).SumAsync();
            var waste = await query2.Select(x => x.a.DefeatItem1 + x.a.DefeatItem2 + x.a.DefeatItem3 + x.a.DefeatItem4).SumAsync();

            var data = new PtWeekVM()
            {
                TestingTime = (wtVM is null) ? 0 : wtVM.TestingTime,
                RunningTime = (wtVM is null) ? 0 : wtVM.RunningTime,
                NotRunningTime = notRunningTime,
                TotalTime = totalTime,
                TestingTimeAndPercent = (wtVM is null || totalTime == 0) ? "0h 0%" : Math.Round((decimal)wtVM.TestingTime, 1).ToString() + "h " + Math.Round((decimal)(wtVM.TestingTime * 100 / totalTime), 1).ToString() + "%",
                RunningTimeAndPercent = (wtVM is null || totalTime == 0) ? "0h 0%" : Math.Round((decimal)wtVM.RunningTime, 1).ToString() + "h " + Math.Round((decimal)(wtVM.RunningTime * 100 / totalTime), 1).ToString() + "%",
                NotRunningTimeAndPercent = (totalTime == 0) ? "0h 0%" : Math.Round((decimal)notRunningTime, 1).ToString() + "h " + Math.Round((decimal)(notRunningTime * 100 / totalTime), 1).ToString() + "%",
                ProductCodeChangeCount = productCodeChange,
                Quantity = quantity,
                Waste = waste,
            };

            return data;
        }
        #endregion
        #region Month
        public async Task<PtMonthYearVM> GetP601ProductionTracking_Month(TimeRequest request)
        {
            var query1 = from p in _context.TableDays
                         select new { p };
            query1 = query1.Where(w => w.p.Date >= request.From && w.p.Date <= request.To);

            if (!query1.Any())
            {
                return new PtMonthYearVM()
                {
                    ActualWorkingTime = 0,
                    Downtime = 0,
                    TotalOutput = 0,
                    MaximumProductionCapacity = 0,
                    Waste = 0,
                };
            }

            var wtVM = query1.Where(w => w.p.Machine == "P601")
            .GroupBy(g => g.p.Machine)
            .Select(s => new WorkTimeVM()
            {
                RunningTime = (s.Sum(a => a.p.RunningTime) == null) ? 0 : s.Sum(a => a.p.RunningTime),
                TestingTime = (s.Sum(a => a.p.TestingTime) == null) ? 0 : s.Sum(a => a.p.TestingTime),
                OtherTime = (s.Sum(a => a.p.OtherTime) == null) ? 0 : s.Sum(a => a.p.OtherTime),
                BreakTime = (s.Sum(a => a.p.BreakTime) == null) ? 0 : s.Sum(a => a.p.BreakTime),
                FixingTime = (s.Sum(a => a.p.FixingTime) == null) ? 0 : s.Sum(a => a.p.FixingTime),
                PendingTime = (s.Sum(a => a.p.PendingTime) == null) ? 0 : s.Sum(a => a.p.PendingTime),
                MaintenanceTime = (s.Sum(a => a.p.MaintenanceTime) == null) ? 0 : s.Sum(a => a.p.MaintenanceTime),
                PauseTime = (s.Sum(a => a.p.PauseTime) == null) ? 0 : s.Sum(a => a.p.PauseTime),
            }).FirstOrDefault();
            if (wtVM is null)
            {
                wtVM = new WorkTimeVM()
                {
                    RunningTime = 0,
                    TestingTime = 0,
                    OtherTime = 0,
                    BreakTime = 0,
                    FixingTime = 0,
                    PendingTime = 0,
                    MaintenanceTime = 0,
                    PauseTime = 0,
                };
            }

            var query2 = from p in _context.P601PoKpis
                         select new { p };
            query2 = query2.Where(w => w.p.Starttime >= request.From && w.p.Starttime <= request.To && w.p.Ws != "");
            var totalOutput = await query2.Select(s => s.p.Job).SumAsync();
            var waste = await query2.Select(s => s.p.DefeatItem1 + s.p.DefeatItem2 + s.p.DefeatItem3 + s.p.DefeatItem4).SumAsync();

            var query3 = from p in _context.P601PoInfos
                         select new { p };
            query3 = query3.Where(w => w.p.Date >= request.From && w.p.Date <= request.To && w.p.Ws != "");
            var pcList = query3.Select(s => float.Parse(s.p.Plantime) * 7000).ToList();
            var maximumProductionCapacity = 0.0f;
            foreach (float a in pcList)
            {
                maximumProductionCapacity += a;
            }    

            var data = new PtMonthYearVM()
            {
                ActualWorkingTime = wtVM.TestingTime + wtVM.RunningTime + wtVM.OtherTime + wtVM.BreakTime + wtVM.FixingTime + wtVM.PendingTime + wtVM.MaintenanceTime + wtVM.PauseTime,
                Downtime = wtVM.TestingTime + wtVM.OtherTime + wtVM.BreakTime + wtVM.FixingTime + wtVM.PendingTime + wtVM.MaintenanceTime + wtVM.PauseTime,
                TotalOutput = (totalOutput == null) ? 0 : totalOutput,
                MaximumProductionCapacity = (maximumProductionCapacity == null) ? 0 : maximumProductionCapacity,
                Waste = (waste == null) ? 0 : waste,
            };

            return data;
        }
        public async Task<PtMonthYearVM> GetP604ProductionTracking_Month(TimeRequest request)
        {
            var query1 = from p in _context.TableDays
                         select new { p };
            query1 = query1.Where(w => w.p.Date >= request.From && w.p.Date <= request.To);

            if (!query1.Any())
            {               
                return new PtMonthYearVM()
                {
                    ActualWorkingTime = 0,
                    Downtime = 0,
                    TotalOutput = 0,
                    MaximumProductionCapacity = 0,
                    Waste = 0,
                };
            }

            var wtVM = query1.Where(w => w.p.Machine == "P604")
            .GroupBy(g => g.p.Machine)
            .Select(s => new WorkTimeVM()
            {
                RunningTime = (s.Sum(a => a.p.RunningTime) == null) ? 0 : s.Sum(a => a.p.RunningTime),
                TestingTime = (s.Sum(a => a.p.TestingTime) == null) ? 0 : s.Sum(a => a.p.TestingTime),
                OtherTime = (s.Sum(a => a.p.OtherTime) == null) ? 0 : s.Sum(a => a.p.OtherTime),
                BreakTime = (s.Sum(a => a.p.BreakTime) == null) ? 0 : s.Sum(a => a.p.BreakTime),
                FixingTime = (s.Sum(a => a.p.FixingTime) == null) ? 0 : s.Sum(a => a.p.FixingTime),
                PendingTime = (s.Sum(a => a.p.PendingTime) == null) ? 0 : s.Sum(a => a.p.PendingTime),
                MaintenanceTime = (s.Sum(a => a.p.MaintenanceTime) == null) ? 0 : s.Sum(a => a.p.MaintenanceTime),
                PauseTime = (s.Sum(a => a.p.PauseTime) == null) ? 0 : s.Sum(a => a.p.PauseTime),
            }).FirstOrDefault();
            if(wtVM is null)
            {
                wtVM = new WorkTimeVM()
                {
                    RunningTime = 0,
                    TestingTime = 0,
                    OtherTime = 0,
                    BreakTime = 0,
                    FixingTime = 0,
                    PendingTime = 0,
                    MaintenanceTime = 0,
                    PauseTime = 0,
                };
            }    

            var query2 = from p in _context.P604PoKpis
                         select new { p };
            query2 = query2.Where(w => w.p.Starttime >= request.From && w.p.Starttime <= request.To && w.p.Ws != "");
            var totalOutput = await query2.Select(s => s.p.Job).SumAsync();
            var waste = await query2.Select(s => s.p.DefeatItem1 + s.p.DefeatItem2 + s.p.DefeatItem3 + s.p.DefeatItem4).SumAsync();

            var query3 = from p in _context.P604PoInfos
                         select new { p };
            query3 = query3.Where(w => w.p.Date >= request.From && w.p.Date <= request.To && w.p.Ws != "");
            var pcList = query3.Select(s => float.Parse(s.p.Plantime) * 7000).ToList();
            var maximumProductionCapacity = 0.0f;
            foreach (float a in pcList)
            {
                maximumProductionCapacity += a;
            }

            var data = new PtMonthYearVM()
            {
                ActualWorkingTime = wtVM.TestingTime + wtVM.RunningTime + wtVM.OtherTime + wtVM.BreakTime + wtVM.FixingTime + wtVM.PendingTime + wtVM.MaintenanceTime + wtVM.PauseTime,
                Downtime = wtVM.TestingTime + wtVM.OtherTime + wtVM.BreakTime + wtVM.FixingTime + wtVM.PendingTime + wtVM.MaintenanceTime + wtVM.PauseTime,
                TotalOutput = (totalOutput == null) ? 0 : totalOutput,
                MaximumProductionCapacity = (maximumProductionCapacity == null) ? 0 : maximumProductionCapacity,
                Waste = (waste == null) ? 0 : waste,
            };

            return data;
        }
        public async Task<PtMonthYearVM> GetP605ProductionTracking_Month(TimeRequest request)
        {
            var query1 = from p in _context.TableDays
                         select new { p };
            query1 = query1.Where(w => w.p.Date >= request.From && w.p.Date <= request.To);

            if (!query1.Any())
            {
                return new PtMonthYearVM()
                {
                    ActualWorkingTime = 0,
                    Downtime = 0,
                    TotalOutput = 0,
                    MaximumProductionCapacity = 0,
                    Waste = 0,
                };
            }

            var wtVM = query1.Where(w => w.p.Machine == "P605")
            .GroupBy(g => g.p.Machine)
            .Select(s => new WorkTimeVM()
            {
                RunningTime = (s.Sum(a => a.p.RunningTime) == null) ? 0 : s.Sum(a => a.p.RunningTime),
                TestingTime = (s.Sum(a => a.p.TestingTime) == null) ? 0 : s.Sum(a => a.p.TestingTime),
                OtherTime = (s.Sum(a => a.p.OtherTime) == null) ? 0 : s.Sum(a => a.p.OtherTime),
                BreakTime = (s.Sum(a => a.p.BreakTime) == null) ? 0 : s.Sum(a => a.p.BreakTime),
                FixingTime = (s.Sum(a => a.p.FixingTime) == null) ? 0 : s.Sum(a => a.p.FixingTime),
                PendingTime = (s.Sum(a => a.p.PendingTime) == null) ? 0 : s.Sum(a => a.p.PendingTime),
                MaintenanceTime = (s.Sum(a => a.p.MaintenanceTime) == null) ? 0 : s.Sum(a => a.p.MaintenanceTime),
                PauseTime = (s.Sum(a => a.p.PauseTime) == null) ? 0 : s.Sum(a => a.p.PauseTime),
            }).FirstOrDefault();
            if (wtVM is null)
            {
                wtVM = new WorkTimeVM()
                {
                    RunningTime = 0,
                    TestingTime = 0,
                    OtherTime = 0,
                    BreakTime = 0,
                    FixingTime = 0,
                    PendingTime = 0,
                    MaintenanceTime = 0,
                    PauseTime = 0,
                };
            }

            var query2 = from p in _context.P605PoKpis
                         select new { p };
            query2 = query2.Where(w => w.p.Starttime >= request.From && w.p.Starttime <= request.To && w.p.Ws != "");
            var totalOutput = await query2.Select(s => s.p.Job).SumAsync();
            var waste = await query2.Select(s => s.p.DefeatItem1 + s.p.DefeatItem2 + s.p.DefeatItem3 + s.p.DefeatItem4).SumAsync();

            var query3 = from p in _context.P605PoInfos
                         select new { p };
            query3 = query3.Where(w => w.p.Date >= request.From && w.p.Date <= request.To && w.p.Ws != "");
            var pcList = query3.Select(s => float.Parse(s.p.Plantime) * 7000).ToList();
            var maximumProductionCapacity = 0.0f;
            foreach (float a in pcList)
            {
                maximumProductionCapacity += a;
            }

            var data = new PtMonthYearVM()
            {
                ActualWorkingTime = wtVM.TestingTime + wtVM.RunningTime + wtVM.OtherTime + wtVM.BreakTime + wtVM.FixingTime + wtVM.PendingTime + wtVM.MaintenanceTime + wtVM.PauseTime,
                Downtime = wtVM.TestingTime + wtVM.OtherTime + wtVM.BreakTime + wtVM.FixingTime + wtVM.PendingTime + wtVM.MaintenanceTime + wtVM.PauseTime,
                TotalOutput = (totalOutput == null) ? 0 : totalOutput,
                MaximumProductionCapacity = (maximumProductionCapacity == null) ? 0 : maximumProductionCapacity,
                Waste = (waste == null) ? 0 : waste,
            };

            return data;
        }
        public async Task<PtMonthYearVM> GetP5mProductionTracking_Month(TimeRequest request)
        {
            var query1 = from p in _context.TableDays
                         select new { p };
            query1 = query1.Where(w => w.p.Date >= request.From && w.p.Date <= request.To);

            if (!query1.Any())
            {
                return new PtMonthYearVM()
                {
                    ActualWorkingTime = 0,
                    Downtime = 0,
                    TotalOutput = 0,
                    MaximumProductionCapacity = 0,
                    Waste = 0,
                };
            }

            var wtVM = query1.Where(w => w.p.Machine == "P5m")
            .GroupBy(g => g.p.Machine)
            .Select(s => new WorkTimeVM()
            {
                RunningTime = (s.Sum(a => a.p.RunningTime) == null) ? 0 : s.Sum(a => a.p.RunningTime),
                TestingTime = (s.Sum(a => a.p.TestingTime) == null) ? 0 : s.Sum(a => a.p.TestingTime),
                OtherTime = (s.Sum(a => a.p.OtherTime) == null) ? 0 : s.Sum(a => a.p.OtherTime),
                BreakTime = (s.Sum(a => a.p.BreakTime) == null) ? 0 : s.Sum(a => a.p.BreakTime),
                FixingTime = (s.Sum(a => a.p.FixingTime) == null) ? 0 : s.Sum(a => a.p.FixingTime),
                PendingTime = (s.Sum(a => a.p.PendingTime) == null) ? 0 : s.Sum(a => a.p.PendingTime),
                MaintenanceTime = (s.Sum(a => a.p.MaintenanceTime) == null) ? 0 : s.Sum(a => a.p.MaintenanceTime),
                PauseTime = (s.Sum(a => a.p.PauseTime) == null) ? 0 : s.Sum(a => a.p.PauseTime),
            }).FirstOrDefault();
            if (wtVM is null)
            {
                wtVM = new WorkTimeVM()
                {
                    RunningTime = 0,
                    TestingTime = 0,
                    OtherTime = 0,
                    BreakTime = 0,
                    FixingTime = 0,
                    PendingTime = 0,
                    MaintenanceTime = 0,
                    PauseTime = 0,
                };
            }

            var query2 = from p in _context.P5mPoKpis
                         select new { p };
            query2 = query2.Where(w => w.p.Starttime >= request.From && w.p.Starttime <= request.To && w.p.Ws != "");
            var totalOutput = await query2.Select(s => s.p.Job).SumAsync();
            var waste = await query2.Select(s => s.p.DefeatItem1 + s.p.DefeatItem2 + s.p.DefeatItem3 + s.p.DefeatItem4).SumAsync();

            var query3 = from p in _context.P5mPoInfos
                         select new { p };
            query3 = query3.Where(w => w.p.Date >= request.From && w.p.Date <= request.To && w.p.Ws != "");
            var pcList = query3.Select(s => float.Parse(s.p.Plantime) * 7000).ToList();
            var maximumProductionCapacity = 0.0f;
            foreach (float a in pcList)
            {
                maximumProductionCapacity += a;
            }

            var data = new PtMonthYearVM()
            {
                ActualWorkingTime = wtVM.TestingTime + wtVM.RunningTime + wtVM.OtherTime + wtVM.BreakTime + wtVM.FixingTime + wtVM.PendingTime + wtVM.MaintenanceTime + wtVM.PauseTime,
                Downtime = wtVM.TestingTime + wtVM.OtherTime + wtVM.BreakTime + wtVM.FixingTime + wtVM.PendingTime + wtVM.MaintenanceTime + wtVM.PauseTime,
                TotalOutput = (totalOutput == null) ? 0 : totalOutput,
                MaximumProductionCapacity = (maximumProductionCapacity == null) ? 0 : maximumProductionCapacity,
                Waste = (waste == null) ? 0 : waste,
            };

            return data;
        }
        #endregion
        #region Year
        public async Task<PtMonthYearVM> GetP601ProductionTracking_Year(TimeRequest request)
        {
            var query1 = from p in _context.TableDays
                         select new { p };
            query1 = query1.Where(w => w.p.Date >= request.From && w.p.Date <= request.To);

            if (!query1.Any())
            {
                return new PtMonthYearVM()
                {
                    ActualWorkingTime = 0,
                    Downtime = 0,
                    TotalOutput = 0,
                    MaximumProductionCapacity = 0,
                    Waste = 0,
                };
            }

            var wtVM = query1.Where(w => w.p.Machine == "P601")
            .GroupBy(g => g.p.Machine)
            .Select(s => new WorkTimeVM()
            {
                RunningTime = (s.Sum(a => a.p.RunningTime) == null) ? 0 : s.Sum(a => a.p.RunningTime),
                TestingTime = (s.Sum(a => a.p.TestingTime) == null) ? 0 : s.Sum(a => a.p.TestingTime),
                OtherTime = (s.Sum(a => a.p.OtherTime) == null) ? 0 : s.Sum(a => a.p.OtherTime),
                BreakTime = (s.Sum(a => a.p.BreakTime) == null) ? 0 : s.Sum(a => a.p.BreakTime),
                FixingTime = (s.Sum(a => a.p.FixingTime) == null) ? 0 : s.Sum(a => a.p.FixingTime),
                PendingTime = (s.Sum(a => a.p.PendingTime) == null) ? 0 : s.Sum(a => a.p.PendingTime),
                MaintenanceTime = (s.Sum(a => a.p.MaintenanceTime) == null) ? 0 : s.Sum(a => a.p.MaintenanceTime),
                PauseTime = (s.Sum(a => a.p.PauseTime) == null) ? 0 : s.Sum(a => a.p.PauseTime),
            }).FirstOrDefault();
            if (wtVM is null)
            {
                wtVM = new WorkTimeVM()
                {
                    RunningTime = 0,
                    TestingTime = 0,
                    OtherTime = 0,
                    BreakTime = 0,
                    FixingTime = 0,
                    PendingTime = 0,
                    MaintenanceTime = 0,
                    PauseTime = 0,
                };
            }

            var query2 = from p in _context.P601PoKpis
                         select new { p };
            query2 = query2.Where(w => w.p.Starttime >= request.From && w.p.Starttime <= request.To && w.p.Ws != "");
            var totalOutput = await query2.Select(s => s.p.Job).SumAsync();
            var waste = await query2.Select(s => s.p.DefeatItem1 + s.p.DefeatItem2 + s.p.DefeatItem3 + s.p.DefeatItem4).SumAsync();

            var query3 = from p in _context.P601PoInfos
                         select new { p };
            query3 = query3.Where(w => w.p.Date >= request.From && w.p.Date <= request.To && w.p.Ws != "");
            var pcList = query3.Select(s => float.Parse(s.p.Plantime) * 7000).ToList();
            var maximumProductionCapacity = 0.0f;
            foreach (float a in pcList)
            {
                maximumProductionCapacity += a;
            }

            var data = new PtMonthYearVM()
            {
                ActualWorkingTime = wtVM.TestingTime + wtVM.RunningTime + wtVM.OtherTime + wtVM.BreakTime + wtVM.FixingTime + wtVM.PendingTime + wtVM.MaintenanceTime + wtVM.PauseTime,
                Downtime = wtVM.TestingTime + wtVM.OtherTime + wtVM.BreakTime + wtVM.FixingTime + wtVM.PendingTime + wtVM.MaintenanceTime + wtVM.PauseTime,
                TotalOutput = (totalOutput == null) ? 0 : totalOutput,
                MaximumProductionCapacity = (maximumProductionCapacity == null) ? 0 : maximumProductionCapacity,
                Waste = (waste == null) ? 0 : waste,
            };

            return data;
        }
        public async Task<PtMonthYearVM> GetP604ProductionTracking_Year(TimeRequest request)
        {
            var query1 = from p in _context.TableDays
                         select new { p };
            query1 = query1.Where(w => w.p.Date >= request.From && w.p.Date <= request.To);

            if (!query1.Any())
            {
                return new PtMonthYearVM()
                {
                    ActualWorkingTime = 0,
                    Downtime = 0,
                    TotalOutput = 0,
                    MaximumProductionCapacity = 0,
                    Waste = 0,
                };
            }

            var wtVM = query1.Where(w => w.p.Machine == "P604")
            .GroupBy(g => g.p.Machine)
            .Select(s => new WorkTimeVM()
            {
                RunningTime = (s.Sum(a => a.p.RunningTime) == null) ? 0 : s.Sum(a => a.p.RunningTime),
                TestingTime = (s.Sum(a => a.p.TestingTime) == null) ? 0 : s.Sum(a => a.p.TestingTime),
                OtherTime = (s.Sum(a => a.p.OtherTime) == null) ? 0 : s.Sum(a => a.p.OtherTime),
                BreakTime = (s.Sum(a => a.p.BreakTime) == null) ? 0 : s.Sum(a => a.p.BreakTime),
                FixingTime = (s.Sum(a => a.p.FixingTime) == null) ? 0 : s.Sum(a => a.p.FixingTime),
                PendingTime = (s.Sum(a => a.p.PendingTime) == null) ? 0 : s.Sum(a => a.p.PendingTime),
                MaintenanceTime = (s.Sum(a => a.p.MaintenanceTime) == null) ? 0 : s.Sum(a => a.p.MaintenanceTime),
                PauseTime = (s.Sum(a => a.p.PauseTime) == null) ? 0 : s.Sum(a => a.p.PauseTime),
            }).FirstOrDefault();
            if (wtVM is null)
            {
                wtVM = new WorkTimeVM()
                {
                    RunningTime = 0,
                    TestingTime = 0,
                    OtherTime = 0,
                    BreakTime = 0,
                    FixingTime = 0,
                    PendingTime = 0,
                    MaintenanceTime = 0,
                    PauseTime = 0,
                };
            }

            var query2 = from p in _context.P604PoKpis
                         select new { p };
            query2 = query2.Where(w => w.p.Starttime >= request.From && w.p.Starttime <= request.To && w.p.Ws != "");
            var totalOutput = await query2.Select(s => s.p.Job).SumAsync();
            var waste = await query2.Select(s => s.p.DefeatItem1 + s.p.DefeatItem2 + s.p.DefeatItem3 + s.p.DefeatItem4).SumAsync();

            var query3 = from p in _context.P604PoInfos
                         select new { p };
            query3 = query3.Where(w => w.p.Date >= request.From && w.p.Date <= request.To && w.p.Ws != "");
            var pcList = query3.Select(s => float.Parse(s.p.Plantime) * 7000).ToList();
            var maximumProductionCapacity = 0.0f;
            foreach (float a in pcList)
            {
                maximumProductionCapacity += a;
            }

            var data = new PtMonthYearVM()
            {
                ActualWorkingTime = wtVM.TestingTime + wtVM.RunningTime + wtVM.OtherTime + wtVM.BreakTime + wtVM.FixingTime + wtVM.PendingTime + wtVM.MaintenanceTime + wtVM.PauseTime,
                Downtime = wtVM.TestingTime + wtVM.OtherTime + wtVM.BreakTime + wtVM.FixingTime + wtVM.PendingTime + wtVM.MaintenanceTime + wtVM.PauseTime,
                TotalOutput = (totalOutput == null) ? 0 : totalOutput,
                MaximumProductionCapacity = (maximumProductionCapacity == null) ? 0 : maximumProductionCapacity,
                Waste = (waste == null) ? 0 : waste,
            };

            return data;
        }
        public async Task<PtMonthYearVM> GetP605ProductionTracking_Year(TimeRequest request)
        {
            var query1 = from p in _context.TableDays
                         select new { p };
            query1 = query1.Where(w => w.p.Date >= request.From && w.p.Date <= request.To);

            if (!query1.Any())
            {
                return new PtMonthYearVM()
                {
                    ActualWorkingTime = 0,
                    Downtime = 0,
                    TotalOutput = 0,
                    MaximumProductionCapacity = 0,
                    Waste = 0,
                };
            }

            var wtVM = query1.Where(w => w.p.Machine == "P605")
            .GroupBy(g => g.p.Machine)
            .Select(s => new WorkTimeVM()
            {
                RunningTime = (s.Sum(a => a.p.RunningTime) == null) ? 0 : s.Sum(a => a.p.RunningTime),
                TestingTime = (s.Sum(a => a.p.TestingTime) == null) ? 0 : s.Sum(a => a.p.TestingTime),
                OtherTime = (s.Sum(a => a.p.OtherTime) == null) ? 0 : s.Sum(a => a.p.OtherTime),
                BreakTime = (s.Sum(a => a.p.BreakTime) == null) ? 0 : s.Sum(a => a.p.BreakTime),
                FixingTime = (s.Sum(a => a.p.FixingTime) == null) ? 0 : s.Sum(a => a.p.FixingTime),
                PendingTime = (s.Sum(a => a.p.PendingTime) == null) ? 0 : s.Sum(a => a.p.PendingTime),
                MaintenanceTime = (s.Sum(a => a.p.MaintenanceTime) == null) ? 0 : s.Sum(a => a.p.MaintenanceTime),
                PauseTime = (s.Sum(a => a.p.PauseTime) == null) ? 0 : s.Sum(a => a.p.PauseTime),
            }).FirstOrDefault();
            if (wtVM is null)
            {
                wtVM = new WorkTimeVM()
                {
                    RunningTime = 0,
                    TestingTime = 0,
                    OtherTime = 0,
                    BreakTime = 0,
                    FixingTime = 0,
                    PendingTime = 0,
                    MaintenanceTime = 0,
                    PauseTime = 0,
                };
            }

            var query2 = from p in _context.P605PoKpis
                         select new { p };
            query2 = query2.Where(w => w.p.Starttime >= request.From && w.p.Starttime <= request.To && w.p.Ws != "");
            var totalOutput = await query2.Select(s => s.p.Job).SumAsync();
            var waste = await query2.Select(s => s.p.DefeatItem1 + s.p.DefeatItem2 + s.p.DefeatItem3 + s.p.DefeatItem4).SumAsync();

            var query3 = from p in _context.P605PoInfos
                         select new { p };
            query3 = query3.Where(w => w.p.Date >= request.From && w.p.Date <= request.To && w.p.Ws != "");
            var pcList = query3.Select(s => float.Parse(s.p.Plantime) * 7000).ToList();
            var maximumProductionCapacity = 0.0f;
            foreach (float a in pcList)
            {
                maximumProductionCapacity += a;
            }

            var data = new PtMonthYearVM()
            {
                ActualWorkingTime = wtVM.TestingTime + wtVM.RunningTime + wtVM.OtherTime + wtVM.BreakTime + wtVM.FixingTime + wtVM.PendingTime + wtVM.MaintenanceTime + wtVM.PauseTime,
                Downtime = wtVM.TestingTime + wtVM.OtherTime + wtVM.BreakTime + wtVM.FixingTime + wtVM.PendingTime + wtVM.MaintenanceTime + wtVM.PauseTime,
                TotalOutput = (totalOutput == null) ? 0 : totalOutput,
                MaximumProductionCapacity = (maximumProductionCapacity == null) ? 0 : maximumProductionCapacity,
                Waste = (waste == null) ? 0 : waste,
            };

            return data;
        }
        public async Task<PtMonthYearVM> GetP5mProductionTracking_Year(TimeRequest request)
        {
            var query1 = from p in _context.TableDays
                         select new { p };
            query1 = query1.Where(w => w.p.Date >= request.From && w.p.Date <= request.To);

            if (!query1.Any())
            {
                return new PtMonthYearVM()
                {
                    ActualWorkingTime = 0,
                    Downtime = 0,
                    TotalOutput = 0,
                    MaximumProductionCapacity = 0,
                    Waste = 0,
                };
            }

            var wtVM = query1.Where(w => w.p.Machine == "P5m")
            .GroupBy(g => g.p.Machine)
            .Select(s => new WorkTimeVM()
            {
                RunningTime = (s.Sum(a => a.p.RunningTime) == null) ? 0 : s.Sum(a => a.p.RunningTime),
                TestingTime = (s.Sum(a => a.p.TestingTime) == null) ? 0 : s.Sum(a => a.p.TestingTime),
                OtherTime = (s.Sum(a => a.p.OtherTime) == null) ? 0 : s.Sum(a => a.p.OtherTime),
                BreakTime = (s.Sum(a => a.p.BreakTime) == null) ? 0 : s.Sum(a => a.p.BreakTime),
                FixingTime = (s.Sum(a => a.p.FixingTime) == null) ? 0 : s.Sum(a => a.p.FixingTime),
                PendingTime = (s.Sum(a => a.p.PendingTime) == null) ? 0 : s.Sum(a => a.p.PendingTime),
                MaintenanceTime = (s.Sum(a => a.p.MaintenanceTime) == null) ? 0 : s.Sum(a => a.p.MaintenanceTime),
                PauseTime = (s.Sum(a => a.p.PauseTime) == null) ? 0 : s.Sum(a => a.p.PauseTime),
            }).FirstOrDefault();
            if (wtVM is null)
            {
                wtVM = new WorkTimeVM()
                {
                    RunningTime = 0,
                    TestingTime = 0,
                    OtherTime = 0,
                    BreakTime = 0,
                    FixingTime = 0,
                    PendingTime = 0,
                    MaintenanceTime = 0,
                    PauseTime = 0,
                };
            }

            var query2 = from p in _context.P5mPoKpis
                         select new { p };
            query2 = query2.Where(w => w.p.Starttime >= request.From && w.p.Starttime <= request.To && w.p.Ws != "");
            var totalOutput = await query2.Select(s => s.p.Job).SumAsync();
            var waste = await query2.Select(s => s.p.DefeatItem1 + s.p.DefeatItem2 + s.p.DefeatItem3 + s.p.DefeatItem4).SumAsync();

            var query3 = from p in _context.P5mPoInfos
                         select new { p };
            query3 = query3.Where(w => w.p.Date >= request.From && w.p.Date <= request.To && w.p.Ws != "");
            var pcList = query3.Select(s => float.Parse(s.p.Plantime) * 7000).ToList();
            var maximumProductionCapacity = 0.0f;
            foreach (float a in pcList)
            {
                maximumProductionCapacity += a;
            }

            var data = new PtMonthYearVM()
            {
                ActualWorkingTime = wtVM.TestingTime + wtVM.RunningTime + wtVM.OtherTime + wtVM.BreakTime + wtVM.FixingTime + wtVM.PendingTime + wtVM.MaintenanceTime + wtVM.PauseTime,
                Downtime = wtVM.TestingTime + wtVM.OtherTime + wtVM.BreakTime + wtVM.FixingTime + wtVM.PendingTime + wtVM.MaintenanceTime + wtVM.PauseTime,
                TotalOutput = (totalOutput == null) ? 0 : totalOutput,
                MaximumProductionCapacity = (maximumProductionCapacity == null) ? 0 : maximumProductionCapacity,
                Waste = (waste == null) ? 0 : waste,
            };

            return data;
        }
        #endregion
        #endregion


        #region DieCut
        #region 1Hour
        public async Task<List<Pt1HVM>> GetBtd2ProductionTracking_1H(TimeRequest request)
        {
            var query = from a in _context.Btd2PoKpis
                        join b in _context.Btd2PoInfos on a.Ws equals b.Ws
                        where a.Starttime >= request.From && a.Starttime <= request.To && a.Ws != ""
                        orderby a.Starttime descending
                        select new Pt1HVM
                        {
                            Ws = a.Ws,
                            Product = b.Product,
                            Job = a.Job,
                            Target = a.Target,
                        };
            var data = await query.ToListAsync();

            return data;
        }
        public async Task<List<Pt1HVM>> GetBtd3ProductionTracking_1H(TimeRequest request)
        {
            var query = from a in _context.Btd3PoKpis
                        join b in _context.Btd3PoInfos on a.Ws equals b.Ws
                        where a.Starttime >= request.From && a.Starttime <= request.To && a.Ws != ""
                        orderby a.Starttime descending
                        select new Pt1HVM
                        {
                            Ws = a.Ws,
                            Product = b.Product,
                            Job = a.Job,
                            Target = a.Target,
                        };
            var data = await query.ToListAsync();

            return data;
        }
        public async Task<List<Pt1HVM>> GetBtd4ProductionTracking_1H(TimeRequest request)
        {
            var query = from a in _context.Btd4PoKpis
                        join b in _context.Btd4PoInfos on a.Ws equals b.Ws
                        where a.Starttime >= request.From && a.Starttime <= request.To && a.Ws != ""
                        orderby a.Starttime descending
                        select new Pt1HVM
                        {
                            Ws = a.Ws,
                            Product = b.Product,
                            Job = a.Job,
                            Target = a.Target,
                        };
            var data = await query.ToListAsync();

            return data;
        }
        public async Task<List<Pt1HVM>> GetBtd5ProductionTracking_1H(TimeRequest request)
        {
            var query = from a in _context.Btd5PoKpis
                        join b in _context.Btd5PoInfos on a.Ws equals b.Ws
                        where a.Starttime >= request.From && a.Starttime <= request.To && a.Ws != ""
                        orderby a.Starttime descending
                        select new Pt1HVM
                        {
                            Ws = a.Ws,
                            Product = b.Product,
                            Job = a.Job,
                            Target = a.Target,
                        };
            var data = await query.ToListAsync();

            return data;
        }
        #endregion
        #region Day
        public async Task<List<PtDayVM>> GetBtd2ProductionTracking_Day(TimeRequest request)
        {
            var query = from a in _context.Btd2PoKpis
                        join b in _context.Btd2PoInfos on a.Ws equals b.Ws
                        where a.Starttime >= request.From && a.Starttime <= request.To && a.Ws != ""
                        orderby a.Starttime descending
                        select new PtDayVM
                        {
                            Ws = a.Ws,
                            Product = b.Product,
                            Job = a.Job,
                            Endtime = a.Endtime,
                            Plantime = b.Plantime,
                            PlanEndtime = b.PlanEndtime,
                            Status = a.Status,
                        };
            var data = await query.ToListAsync();

            return data;
        }
        public async Task<List<PtDayVM>> GetBtd3ProductionTracking_Day(TimeRequest request)
        {
            var query = from a in _context.Btd3PoKpis
                        join b in _context.Btd3PoInfos on a.Ws equals b.Ws
                        where a.Starttime >= request.From && a.Starttime <= request.To && a.Ws != ""
                        orderby a.Starttime descending
                        select new PtDayVM
                        {
                            Ws = a.Ws,
                            Product = b.Product,
                            Job = a.Job,
                            Endtime = a.Endtime,
                            Plantime = b.Plantime,
                            PlanEndtime = b.PlanEndtime,
                            Status = a.Status,
                        };
            var data = await query.ToListAsync();

            return data;
        }
        public async Task<List<PtDayVM>> GetBtd4ProductionTracking_Day(TimeRequest request)
        {
            var query = from a in _context.Btd4PoKpis
                        join b in _context.Btd4PoInfos on a.Ws equals b.Ws
                        where a.Starttime >= request.From && a.Starttime <= request.To && a.Ws != ""
                        orderby a.Starttime descending
                        select new PtDayVM
                        {
                            Ws = a.Ws,
                            Product = b.Product,
                            Job = a.Job,
                            Endtime = a.Endtime,
                            Plantime = b.Plantime,
                            PlanEndtime = b.PlanEndtime,
                            Status = a.Status,
                        };
            var data = await query.ToListAsync();

            return data;
        }
        public async Task<List<PtDayVM>> GetBtd5ProductionTracking_Day(TimeRequest request)
        {
            var query = from a in _context.Btd5PoKpis
                        join b in _context.Btd5PoInfos on a.Ws equals b.Ws
                        where a.Starttime >= request.From && a.Starttime <= request.To && a.Ws != ""
                        orderby a.Starttime descending
                        select new PtDayVM
                        {
                            Ws = a.Ws,
                            Product = b.Product,
                            Job = a.Job,
                            Endtime = a.Endtime,
                            Plantime = b.Plantime,
                            PlanEndtime = b.PlanEndtime,
                            Status = a.Status,
                        };
            var data = await query.ToListAsync();

            return data;
        }
        #endregion
        #region Week
        public async Task<PtWeekVM> GetBtd2ProductionTracking_Week(TimeRequest request)
        {
            var query1 = from p in _context.TableDays
                         select new { p };
            query1 = query1.Where(w => w.p.Date >= request.From && w.p.Date <= request.To);
            if (!query1.Any())
            {
                var empty = new WorkTimeVM() { RunningTime = 0, TestingTime = 0, OtherTime = 0, BreakTime = 0, FixingTime = 0, PendingTime = 0, MaintenanceTime = 0, PauseTime = 0 };
                return new PtWeekVM()
                {
                    TestingTime = 0,
                    RunningTime = 0,
                    NotRunningTime = 0,
                    TotalTime = 0,
                    TestingTimeAndPercent = "0h 0%",
                    RunningTimeAndPercent = "0h 0%",
                    NotRunningTimeAndPercent = "0h 0%",
                    ProductCodeChangeCount = 0,
                    Quantity = 0,
                    Waste = 0,
                };
            }

            var wtVM = query1.Where(w => w.p.Machine == "BTD2")
            .GroupBy(g => g.p.Machine)
            .Select(s => new WorkTimeVM()
            {
                RunningTime = (s.Sum(a => a.p.RunningTime) == null) ? 0 : s.Sum(a => a.p.RunningTime),
                TestingTime = (s.Sum(a => a.p.TestingTime) == null) ? 0 : s.Sum(a => a.p.TestingTime),
                OtherTime = (s.Sum(a => a.p.OtherTime) == null) ? 0 : s.Sum(a => a.p.OtherTime),
                BreakTime = (s.Sum(a => a.p.BreakTime) == null) ? 0 : s.Sum(a => a.p.BreakTime),
                FixingTime = (s.Sum(a => a.p.FixingTime) == null) ? 0 : s.Sum(a => a.p.FixingTime),
                PendingTime = (s.Sum(a => a.p.PendingTime) == null) ? 0 : s.Sum(a => a.p.PendingTime),
                MaintenanceTime = (s.Sum(a => a.p.MaintenanceTime) == null) ? 0 : s.Sum(a => a.p.MaintenanceTime),
                PauseTime = (s.Sum(a => a.p.PauseTime) == null) ? 0 : s.Sum(a => a.p.PauseTime),
            }).FirstOrDefault();

            var query2 = (from a in _context.Btd2PoKpis
                          join b in _context.Btd2PoInfos on a.Ws equals b.Ws
                          where a.Starttime >= request.From && a.Starttime <= request.To && a.Ws != ""
                          select new { a, b });



            var totalTime = (wtVM is null) ? 0 : wtVM.TestingTime + wtVM.RunningTime + wtVM.OtherTime + wtVM.BreakTime + wtVM.FixingTime + wtVM.PendingTime + wtVM.MaintenanceTime + wtVM.PauseTime;
            var notRunningTime = (wtVM is null) ? 0 : wtVM.OtherTime + wtVM.BreakTime + wtVM.FixingTime + wtVM.PendingTime + wtVM.MaintenanceTime + wtVM.PauseTime;
            var productCodeChange = await query2.Select(x => x.b.Product).Distinct().CountAsync();
            var quantity = await query2.Select(x => x.a.Job).SumAsync();
            var waste = await query2.Select(x => x.a.DefeatItem1 + x.a.DefeatItem2 + x.a.DefeatItem3 + x.a.DefeatItem4).SumAsync();

            var data = new PtWeekVM()
            {
                TestingTime = (wtVM is null) ? 0 : wtVM.TestingTime,
                RunningTime = (wtVM is null) ? 0 : wtVM.RunningTime,
                NotRunningTime = notRunningTime,
                TotalTime = totalTime,
                TestingTimeAndPercent = (wtVM is null || totalTime == 0) ? "0h 0%" : Math.Round((decimal)wtVM.TestingTime, 1).ToString() + "h " + Math.Round((decimal)(wtVM.TestingTime * 100 / totalTime), 1).ToString() + "%",
                RunningTimeAndPercent = (wtVM is null || totalTime == 0) ? "0h 0%" : Math.Round((decimal)wtVM.RunningTime, 1).ToString() + "h " + Math.Round((decimal)(wtVM.RunningTime * 100 / totalTime), 1).ToString() + "%",
                NotRunningTimeAndPercent = (totalTime == 0) ? "0h 0%" : Math.Round((decimal)notRunningTime, 1).ToString() + "h " + Math.Round((decimal)(notRunningTime * 100 / totalTime), 1).ToString() + "%",
                ProductCodeChangeCount = productCodeChange,
                Quantity = quantity,
                Waste = waste,
            };

            return data;
        }
        public async Task<PtWeekVM> GetBtd3ProductionTracking_Week(TimeRequest request)
        {
            var query1 = from p in _context.TableDays
                         select new { p };
            query1 = query1.Where(w => w.p.Date >= request.From && w.p.Date <= request.To);
            if (!query1.Any())
            {
                var empty = new WorkTimeVM() { RunningTime = 0, TestingTime = 0, OtherTime = 0, BreakTime = 0, FixingTime = 0, PendingTime = 0, MaintenanceTime = 0, PauseTime = 0 };
                return new PtWeekVM()
                {
                    TestingTime = 0,
                    RunningTime = 0,
                    NotRunningTime = 0,
                    TotalTime = 0,
                    TestingTimeAndPercent = "0h 0%",
                    RunningTimeAndPercent = "0h 0%",
                    NotRunningTimeAndPercent = "0h 0%",
                    ProductCodeChangeCount = 0,
                    Quantity = 0,
                    Waste = 0,
                };
            }

            var wtVM = query1.Where(w => w.p.Machine == "BTD3")
            .GroupBy(g => g.p.Machine)
            .Select(s => new WorkTimeVM()
            {
                RunningTime = (s.Sum(a => a.p.RunningTime) == null) ? 0 : s.Sum(a => a.p.RunningTime),
                TestingTime = (s.Sum(a => a.p.TestingTime) == null) ? 0 : s.Sum(a => a.p.TestingTime),
                OtherTime = (s.Sum(a => a.p.OtherTime) == null) ? 0 : s.Sum(a => a.p.OtherTime),
                BreakTime = (s.Sum(a => a.p.BreakTime) == null) ? 0 : s.Sum(a => a.p.BreakTime),
                FixingTime = (s.Sum(a => a.p.FixingTime) == null) ? 0 : s.Sum(a => a.p.FixingTime),
                PendingTime = (s.Sum(a => a.p.PendingTime) == null) ? 0 : s.Sum(a => a.p.PendingTime),
                MaintenanceTime = (s.Sum(a => a.p.MaintenanceTime) == null) ? 0 : s.Sum(a => a.p.MaintenanceTime),
                PauseTime = (s.Sum(a => a.p.PauseTime) == null) ? 0 : s.Sum(a => a.p.PauseTime),
            }).FirstOrDefault();

            var query2 = (from a in _context.Btd3PoKpis
                          join b in _context.Btd3PoInfos on a.Ws equals b.Ws
                          where a.Starttime >= request.From && a.Starttime <= request.To && a.Ws != ""
                          select new { a, b });



            var totalTime = (wtVM is null) ? 0 : wtVM.TestingTime + wtVM.RunningTime + wtVM.OtherTime + wtVM.BreakTime + wtVM.FixingTime + wtVM.PendingTime + wtVM.MaintenanceTime + wtVM.PauseTime;
            var notRunningTime = (wtVM is null) ? 0 : wtVM.OtherTime + wtVM.BreakTime + wtVM.FixingTime + wtVM.PendingTime + wtVM.MaintenanceTime + wtVM.PauseTime;
            var productCodeChange = await query2.Select(x => x.b.Product).Distinct().CountAsync();
            var quantity = await query2.Select(x => x.a.Job).SumAsync();
            var waste = await query2.Select(x => x.a.DefeatItem1 + x.a.DefeatItem2 + x.a.DefeatItem3 + x.a.DefeatItem4).SumAsync();

            var data = new PtWeekVM()
            {
                TestingTime = (wtVM is null) ? 0 : wtVM.TestingTime,
                RunningTime = (wtVM is null) ? 0 : wtVM.RunningTime,
                NotRunningTime = notRunningTime,
                TotalTime = totalTime,
                TestingTimeAndPercent = (wtVM is null || totalTime == 0) ? "0h 0%" : Math.Round((decimal)wtVM.TestingTime, 1).ToString() + "h " + Math.Round((decimal)(wtVM.TestingTime * 100 / totalTime), 1).ToString() + "%",
                RunningTimeAndPercent = (wtVM is null || totalTime == 0) ? "0h 0%" : Math.Round((decimal)wtVM.RunningTime, 1).ToString() + "h " + Math.Round((decimal)(wtVM.RunningTime * 100 / totalTime), 1).ToString() + "%",
                NotRunningTimeAndPercent = (totalTime == 0) ? "0h 0%" : Math.Round((decimal)notRunningTime, 1).ToString() + "h " + Math.Round((decimal)(notRunningTime * 100 / totalTime), 1).ToString() + "%",
                ProductCodeChangeCount = productCodeChange,
                Quantity = quantity,
                Waste = waste,
            };

            return data;
        }
        public async Task<PtWeekVM> GetBtd4ProductionTracking_Week(TimeRequest request)
        {
            var query1 = from p in _context.TableDays
                         select new { p };
            query1 = query1.Where(w => w.p.Date >= request.From && w.p.Date <= request.To);
            if (!query1.Any())
            {
                var empty = new WorkTimeVM() { RunningTime = 0, TestingTime = 0, OtherTime = 0, BreakTime = 0, FixingTime = 0, PendingTime = 0, MaintenanceTime = 0, PauseTime = 0 };
                return new PtWeekVM()
                {
                    TestingTime = 0,
                    RunningTime = 0,
                    NotRunningTime = 0,
                    TotalTime = 0,
                    TestingTimeAndPercent = "0h 0%",
                    RunningTimeAndPercent = "0h 0%",
                    NotRunningTimeAndPercent = "0h 0%",
                    ProductCodeChangeCount = 0,
                    Quantity = 0,
                    Waste = 0,
                };
            }

            var wtVM = query1.Where(w => w.p.Machine == "BTD4")
            .GroupBy(g => g.p.Machine)
            .Select(s => new WorkTimeVM()
            {
                RunningTime = (s.Sum(a => a.p.RunningTime) == null) ? 0 : s.Sum(a => a.p.RunningTime),
                TestingTime = (s.Sum(a => a.p.TestingTime) == null) ? 0 : s.Sum(a => a.p.TestingTime),
                OtherTime = (s.Sum(a => a.p.OtherTime) == null) ? 0 : s.Sum(a => a.p.OtherTime),
                BreakTime = (s.Sum(a => a.p.BreakTime) == null) ? 0 : s.Sum(a => a.p.BreakTime),
                FixingTime = (s.Sum(a => a.p.FixingTime) == null) ? 0 : s.Sum(a => a.p.FixingTime),
                PendingTime = (s.Sum(a => a.p.PendingTime) == null) ? 0 : s.Sum(a => a.p.PendingTime),
                MaintenanceTime = (s.Sum(a => a.p.MaintenanceTime) == null) ? 0 : s.Sum(a => a.p.MaintenanceTime),
                PauseTime = (s.Sum(a => a.p.PauseTime) == null) ? 0 : s.Sum(a => a.p.PauseTime),
            }).FirstOrDefault();

            var query2 = (from a in _context.Btd4PoKpis
                          join b in _context.Btd4PoInfos on a.Ws equals b.Ws
                          where a.Starttime >= request.From && a.Starttime <= request.To && a.Ws != ""
                          select new { a, b });



            var totalTime = (wtVM is null) ? 0 : wtVM.TestingTime + wtVM.RunningTime + wtVM.OtherTime + wtVM.BreakTime + wtVM.FixingTime + wtVM.PendingTime + wtVM.MaintenanceTime + wtVM.PauseTime;
            var notRunningTime = (wtVM is null) ? 0 : wtVM.OtherTime + wtVM.BreakTime + wtVM.FixingTime + wtVM.PendingTime + wtVM.MaintenanceTime + wtVM.PauseTime;
            var productCodeChange = await query2.Select(x => x.b.Product).Distinct().CountAsync();
            var quantity = await query2.Select(x => x.a.Job).SumAsync();
            var waste = await query2.Select(x => x.a.DefeatItem1 + x.a.DefeatItem2 + x.a.DefeatItem3 + x.a.DefeatItem4).SumAsync();

            var data = new PtWeekVM()
            {
                TestingTime = (wtVM is null) ? 0 : wtVM.TestingTime,
                RunningTime = (wtVM is null) ? 0 : wtVM.RunningTime,
                NotRunningTime = notRunningTime,
                TotalTime = totalTime,
                TestingTimeAndPercent = (wtVM is null || totalTime == 0) ? "0h 0%" : Math.Round((decimal)wtVM.TestingTime, 1).ToString() + "h " + Math.Round((decimal)(wtVM.TestingTime * 100 / totalTime), 1).ToString() + "%",
                RunningTimeAndPercent = (wtVM is null || totalTime == 0) ? "0h 0%" : Math.Round((decimal)wtVM.RunningTime, 1).ToString() + "h " + Math.Round((decimal)(wtVM.RunningTime * 100 / totalTime), 1).ToString() + "%",
                NotRunningTimeAndPercent = (totalTime == 0) ? "0h 0%" : Math.Round((decimal)notRunningTime, 1).ToString() + "h " + Math.Round((decimal)(notRunningTime * 100 / totalTime), 1).ToString() + "%",
                ProductCodeChangeCount = productCodeChange,
                Quantity = quantity,
                Waste = waste,
            };

            return data;
        }
        public async Task<PtWeekVM> GetBtd5ProductionTracking_Week(TimeRequest request)
        {
            var query1 = from p in _context.TableDays
                         select new { p };
            query1 = query1.Where(w => w.p.Date >= request.From && w.p.Date <= request.To);
            if (!query1.Any())
            {
                var empty = new WorkTimeVM() { RunningTime = 0, TestingTime = 0, OtherTime = 0, BreakTime = 0, FixingTime = 0, PendingTime = 0, MaintenanceTime = 0, PauseTime = 0 };
                return new PtWeekVM()
                {
                    TestingTime = 0,
                    RunningTime = 0,
                    NotRunningTime = 0,
                    TotalTime = 0,
                    TestingTimeAndPercent = "0h 0%",
                    RunningTimeAndPercent = "0h 0%",
                    NotRunningTimeAndPercent = "0h 0%",
                    ProductCodeChangeCount = 0,
                    Quantity = 0,
                    Waste = 0,
                };
            }

            var wtVM = query1.Where(w => w.p.Machine == "BTD5")
            .GroupBy(g => g.p.Machine)
            .Select(s => new WorkTimeVM()
            {
                RunningTime = (s.Sum(a => a.p.RunningTime) == null) ? 0 : s.Sum(a => a.p.RunningTime),
                TestingTime = (s.Sum(a => a.p.TestingTime) == null) ? 0 : s.Sum(a => a.p.TestingTime),
                OtherTime = (s.Sum(a => a.p.OtherTime) == null) ? 0 : s.Sum(a => a.p.OtherTime),
                BreakTime = (s.Sum(a => a.p.BreakTime) == null) ? 0 : s.Sum(a => a.p.BreakTime),
                FixingTime = (s.Sum(a => a.p.FixingTime) == null) ? 0 : s.Sum(a => a.p.FixingTime),
                PendingTime = (s.Sum(a => a.p.PendingTime) == null) ? 0 : s.Sum(a => a.p.PendingTime),
                MaintenanceTime = (s.Sum(a => a.p.MaintenanceTime) == null) ? 0 : s.Sum(a => a.p.MaintenanceTime),
                PauseTime = (s.Sum(a => a.p.PauseTime) == null) ? 0 : s.Sum(a => a.p.PauseTime),
            }).FirstOrDefault();

            var query2 = (from a in _context.Btd5PoKpis
                          join b in _context.Btd5PoInfos on a.Ws equals b.Ws
                          where a.Starttime >= request.From && a.Starttime <= request.To && a.Ws != ""
                          select new { a, b });



            var totalTime = (wtVM is null) ? 0 : wtVM.TestingTime + wtVM.RunningTime + wtVM.OtherTime + wtVM.BreakTime + wtVM.FixingTime + wtVM.PendingTime + wtVM.MaintenanceTime + wtVM.PauseTime;
            var notRunningTime = (wtVM is null) ? 0 : wtVM.OtherTime + wtVM.BreakTime + wtVM.FixingTime + wtVM.PendingTime + wtVM.MaintenanceTime + wtVM.PauseTime;
            var productCodeChange = await query2.Select(x => x.b.Product).Distinct().CountAsync();
            var quantity = await query2.Select(x => x.a.Job).SumAsync();
            var waste = await query2.Select(x => x.a.DefeatItem1 + x.a.DefeatItem2 + x.a.DefeatItem3 + x.a.DefeatItem4).SumAsync();

            var data = new PtWeekVM()
            {
                TestingTime = (wtVM is null) ? 0 : wtVM.TestingTime,
                RunningTime = (wtVM is null) ? 0 : wtVM.RunningTime,
                NotRunningTime = notRunningTime,
                TotalTime = totalTime,
                TestingTimeAndPercent = (wtVM is null || totalTime == 0) ? "0h 0%" : Math.Round((decimal)wtVM.TestingTime, 1).ToString() + "h " + Math.Round((decimal)(wtVM.TestingTime * 100 / totalTime), 1).ToString() + "%",
                RunningTimeAndPercent = (wtVM is null || totalTime == 0) ? "0h 0%" : Math.Round((decimal)wtVM.RunningTime, 1).ToString() + "h " + Math.Round((decimal)(wtVM.RunningTime * 100 / totalTime), 1).ToString() + "%",
                NotRunningTimeAndPercent = (totalTime == 0) ? "0h 0%" : Math.Round((decimal)notRunningTime, 1).ToString() + "h " + Math.Round((decimal)(notRunningTime * 100 / totalTime), 1).ToString() + "%",
                ProductCodeChangeCount = productCodeChange,
                Quantity = quantity,
                Waste = waste,
            };

            return data;
        }
        #endregion
        #region Month
        public async Task<PtMonthYearVM> GetBtd2ProductionTracking_Month(TimeRequest request)
        {
            var query1 = from p in _context.TableDays
                         select new { p };
            query1 = query1.Where(w => w.p.Date >= request.From && w.p.Date <= request.To);

            if (!query1.Any())
            {
                return new PtMonthYearVM()
                {
                    ActualWorkingTime = 0,
                    Downtime = 0,
                    TotalOutput = 0,
                    MaximumProductionCapacity = 0,
                    Waste = 0,
                };
            }

            var wtVM = query1.Where(w => w.p.Machine == "BTD2")
            .GroupBy(g => g.p.Machine)
            .Select(s => new WorkTimeVM()
            {
                RunningTime = (s.Sum(a => a.p.RunningTime) == null) ? 0 : s.Sum(a => a.p.RunningTime),
                TestingTime = (s.Sum(a => a.p.TestingTime) == null) ? 0 : s.Sum(a => a.p.TestingTime),
                OtherTime = (s.Sum(a => a.p.OtherTime) == null) ? 0 : s.Sum(a => a.p.OtherTime),
                BreakTime = (s.Sum(a => a.p.BreakTime) == null) ? 0 : s.Sum(a => a.p.BreakTime),
                FixingTime = (s.Sum(a => a.p.FixingTime) == null) ? 0 : s.Sum(a => a.p.FixingTime),
                PendingTime = (s.Sum(a => a.p.PendingTime) == null) ? 0 : s.Sum(a => a.p.PendingTime),
                MaintenanceTime = (s.Sum(a => a.p.MaintenanceTime) == null) ? 0 : s.Sum(a => a.p.MaintenanceTime),
                PauseTime = (s.Sum(a => a.p.PauseTime) == null) ? 0 : s.Sum(a => a.p.PauseTime),
            }).FirstOrDefault();
            if (wtVM is null)
            {
                wtVM = new WorkTimeVM()
                {
                    RunningTime = 0,
                    TestingTime = 0,
                    OtherTime = 0,
                    BreakTime = 0,
                    FixingTime = 0,
                    PendingTime = 0,
                    MaintenanceTime = 0,
                    PauseTime = 0,
                };
            }

            var query2 = from p in _context.Btd2PoKpis
                         select new { p };
            query2 = query2.Where(w => w.p.Starttime >= request.From && w.p.Starttime <= request.To && w.p.Ws != "");
            var totalOutput = await query2.Select(s => s.p.Job).SumAsync();
            var waste = await query2.Select(s => s.p.DefeatItem1 + s.p.DefeatItem2 + s.p.DefeatItem3 + s.p.DefeatItem4).SumAsync();

            var query3 = from p in _context.Btd2PoInfos
                         select new { p };
            query3 = query3.Where(w => w.p.Date >= request.From && w.p.Date <= request.To && w.p.Ws != "");
            var pcList = query3.Select(s => float.Parse(s.p.Plantime) * 4500).ToList();
            var maximumProductionCapacity = 0.0f;
            foreach (float a in pcList)
            {
                maximumProductionCapacity += a;
            }

            var data = new PtMonthYearVM()
            {
                ActualWorkingTime = wtVM.TestingTime + wtVM.RunningTime + wtVM.OtherTime + wtVM.BreakTime + wtVM.FixingTime + wtVM.PendingTime + wtVM.MaintenanceTime + wtVM.PauseTime,
                Downtime = wtVM.TestingTime + wtVM.OtherTime + wtVM.BreakTime + wtVM.FixingTime + wtVM.PendingTime + wtVM.MaintenanceTime + wtVM.PauseTime,
                TotalOutput = (totalOutput == null) ? 0 : totalOutput,
                MaximumProductionCapacity = (maximumProductionCapacity == null) ? 0 : maximumProductionCapacity,
                Waste = (waste == null) ? 0 : waste,
            };

            return data;
        }
        public async Task<PtMonthYearVM> GetBtd3ProductionTracking_Month(TimeRequest request)
        {
            var query1 = from p in _context.TableDays
                         select new { p };
            query1 = query1.Where(w => w.p.Date >= request.From && w.p.Date <= request.To);

            if (!query1.Any())
            {
                return new PtMonthYearVM()
                {
                    ActualWorkingTime = 0,
                    Downtime = 0,
                    TotalOutput = 0,
                    MaximumProductionCapacity = 0,
                    Waste = 0,
                };
            }

            var wtVM = query1.Where(w => w.p.Machine == "Btd3")
            .GroupBy(g => g.p.Machine)
            .Select(s => new WorkTimeVM()
            {
                RunningTime = (s.Sum(a => a.p.RunningTime) == null) ? 0 : s.Sum(a => a.p.RunningTime),
                TestingTime = (s.Sum(a => a.p.TestingTime) == null) ? 0 : s.Sum(a => a.p.TestingTime),
                OtherTime = (s.Sum(a => a.p.OtherTime) == null) ? 0 : s.Sum(a => a.p.OtherTime),
                BreakTime = (s.Sum(a => a.p.BreakTime) == null) ? 0 : s.Sum(a => a.p.BreakTime),
                FixingTime = (s.Sum(a => a.p.FixingTime) == null) ? 0 : s.Sum(a => a.p.FixingTime),
                PendingTime = (s.Sum(a => a.p.PendingTime) == null) ? 0 : s.Sum(a => a.p.PendingTime),
                MaintenanceTime = (s.Sum(a => a.p.MaintenanceTime) == null) ? 0 : s.Sum(a => a.p.MaintenanceTime),
                PauseTime = (s.Sum(a => a.p.PauseTime) == null) ? 0 : s.Sum(a => a.p.PauseTime),
            }).FirstOrDefault();
            if (wtVM is null)
            {
                wtVM = new WorkTimeVM()
                {
                    RunningTime = 0,
                    TestingTime = 0,
                    OtherTime = 0,
                    BreakTime = 0,
                    FixingTime = 0,
                    PendingTime = 0,
                    MaintenanceTime = 0,
                    PauseTime = 0,
                };
            }

            var query2 = from p in _context.Btd3PoKpis
                         select new { p };
            query2 = query2.Where(w => w.p.Starttime >= request.From && w.p.Starttime <= request.To && w.p.Ws != "");
            var totalOutput = await query2.Select(s => s.p.Job).SumAsync();
            var waste = await query2.Select(s => s.p.DefeatItem1 + s.p.DefeatItem2 + s.p.DefeatItem3 + s.p.DefeatItem4).SumAsync();

            var query3 = from p in _context.Btd3PoInfos
                         select new { p };
            query3 = query3.Where(w => w.p.Date >= request.From && w.p.Date <= request.To && w.p.Ws != "");
            var pcList = query3.Select(s => float.Parse(s.p.Plantime) * 4500).ToList();
            var maximumProductionCapacity = 0.0f;
            foreach (float a in pcList)
            {
                maximumProductionCapacity += a;
            }

            var data = new PtMonthYearVM()
            {
                ActualWorkingTime = wtVM.TestingTime + wtVM.RunningTime + wtVM.OtherTime + wtVM.BreakTime + wtVM.FixingTime + wtVM.PendingTime + wtVM.MaintenanceTime + wtVM.PauseTime,
                Downtime = wtVM.TestingTime + wtVM.OtherTime + wtVM.BreakTime + wtVM.FixingTime + wtVM.PendingTime + wtVM.MaintenanceTime + wtVM.PauseTime,
                TotalOutput = (totalOutput == null) ? 0 : totalOutput,
                MaximumProductionCapacity = (maximumProductionCapacity == null) ? 0 : maximumProductionCapacity,
                Waste = (waste == null) ? 0 : waste,
            };

            return data;
        }
        public async Task<PtMonthYearVM> GetBtd4ProductionTracking_Month(TimeRequest request)
        {
            var query1 = from p in _context.TableDays
                         select new { p };
            query1 = query1.Where(w => w.p.Date >= request.From && w.p.Date <= request.To);

            if (!query1.Any())
            {
                return new PtMonthYearVM()
                {
                    ActualWorkingTime = 0,
                    Downtime = 0,
                    TotalOutput = 0,
                    MaximumProductionCapacity = 0,
                    Waste = 0,
                };
            }

            var wtVM = query1.Where(w => w.p.Machine == "Btd4")
            .GroupBy(g => g.p.Machine)
            .Select(s => new WorkTimeVM()
            {
                RunningTime = (s.Sum(a => a.p.RunningTime) == null) ? 0 : s.Sum(a => a.p.RunningTime),
                TestingTime = (s.Sum(a => a.p.TestingTime) == null) ? 0 : s.Sum(a => a.p.TestingTime),
                OtherTime = (s.Sum(a => a.p.OtherTime) == null) ? 0 : s.Sum(a => a.p.OtherTime),
                BreakTime = (s.Sum(a => a.p.BreakTime) == null) ? 0 : s.Sum(a => a.p.BreakTime),
                FixingTime = (s.Sum(a => a.p.FixingTime) == null) ? 0 : s.Sum(a => a.p.FixingTime),
                PendingTime = (s.Sum(a => a.p.PendingTime) == null) ? 0 : s.Sum(a => a.p.PendingTime),
                MaintenanceTime = (s.Sum(a => a.p.MaintenanceTime) == null) ? 0 : s.Sum(a => a.p.MaintenanceTime),
                PauseTime = (s.Sum(a => a.p.PauseTime) == null) ? 0 : s.Sum(a => a.p.PauseTime),
            }).FirstOrDefault();
            if (wtVM is null)
            {
                wtVM = new WorkTimeVM()
                {
                    RunningTime = 0,
                    TestingTime = 0,
                    OtherTime = 0,
                    BreakTime = 0,
                    FixingTime = 0,
                    PendingTime = 0,
                    MaintenanceTime = 0,
                    PauseTime = 0,
                };
            }

            var query2 = from p in _context.Btd4PoKpis
                         select new { p };
            query2 = query2.Where(w => w.p.Starttime >= request.From && w.p.Starttime <= request.To && w.p.Ws != "");
            var totalOutput = await query2.Select(s => s.p.Job).SumAsync();
            var waste = await query2.Select(s => s.p.DefeatItem1 + s.p.DefeatItem2 + s.p.DefeatItem3 + s.p.DefeatItem4).SumAsync();

            var query3 = from p in _context.Btd4PoInfos
                         select new { p };
            query3 = query3.Where(w => w.p.Date >= request.From && w.p.Date <= request.To && w.p.Ws != "");
            var pcList = query3.Select(s => float.Parse(s.p.Plantime) * 4500).ToList();
            var maximumProductionCapacity = 0.0f;
            foreach (float a in pcList)
            {
                maximumProductionCapacity += a;
            }

            var data = new PtMonthYearVM()
            {
                ActualWorkingTime = wtVM.TestingTime + wtVM.RunningTime + wtVM.OtherTime + wtVM.BreakTime + wtVM.FixingTime + wtVM.PendingTime + wtVM.MaintenanceTime + wtVM.PauseTime,
                Downtime = wtVM.TestingTime + wtVM.OtherTime + wtVM.BreakTime + wtVM.FixingTime + wtVM.PendingTime + wtVM.MaintenanceTime + wtVM.PauseTime,
                TotalOutput = (totalOutput == null) ? 0 : totalOutput,
                MaximumProductionCapacity = (maximumProductionCapacity == null) ? 0 : maximumProductionCapacity,
                Waste = (waste == null) ? 0 : waste,
            };

            return data;
        }
        public async Task<PtMonthYearVM> GetBtd5ProductionTracking_Month(TimeRequest request)
        {
            var query1 = from p in _context.TableDays
                         select new { p };
            query1 = query1.Where(w => w.p.Date >= request.From && w.p.Date <= request.To);

            if (!query1.Any())
            {
                return new PtMonthYearVM()
                {
                    ActualWorkingTime = 0,
                    Downtime = 0,
                    TotalOutput = 0,
                    MaximumProductionCapacity = 0,
                    Waste = 0,
                };
            }

            var wtVM = query1.Where(w => w.p.Machine == "BTD5")
            .GroupBy(g => g.p.Machine)
            .Select(s => new WorkTimeVM()
            {
                RunningTime = (s.Sum(a => a.p.RunningTime) == null) ? 0 : s.Sum(a => a.p.RunningTime),
                TestingTime = (s.Sum(a => a.p.TestingTime) == null) ? 0 : s.Sum(a => a.p.TestingTime),
                OtherTime = (s.Sum(a => a.p.OtherTime) == null) ? 0 : s.Sum(a => a.p.OtherTime),
                BreakTime = (s.Sum(a => a.p.BreakTime) == null) ? 0 : s.Sum(a => a.p.BreakTime),
                FixingTime = (s.Sum(a => a.p.FixingTime) == null) ? 0 : s.Sum(a => a.p.FixingTime),
                PendingTime = (s.Sum(a => a.p.PendingTime) == null) ? 0 : s.Sum(a => a.p.PendingTime),
                MaintenanceTime = (s.Sum(a => a.p.MaintenanceTime) == null) ? 0 : s.Sum(a => a.p.MaintenanceTime),
                PauseTime = (s.Sum(a => a.p.PauseTime) == null) ? 0 : s.Sum(a => a.p.PauseTime),
            }).FirstOrDefault();
            if (wtVM is null)
            {
                wtVM = new WorkTimeVM()
                {
                    RunningTime = 0,
                    TestingTime = 0,
                    OtherTime = 0,
                    BreakTime = 0,
                    FixingTime = 0,
                    PendingTime = 0,
                    MaintenanceTime = 0,
                    PauseTime = 0,
                };
            }

            var query2 = from p in _context.Btd5PoKpis
                         select new { p };
            query2 = query2.Where(w => w.p.Starttime >= request.From && w.p.Starttime <= request.To && w.p.Ws != "");
            var totalOutput = await query2.Select(s => s.p.Job).SumAsync();
            var waste = await query2.Select(s => s.p.DefeatItem1 + s.p.DefeatItem2 + s.p.DefeatItem3 + s.p.DefeatItem4).SumAsync();

            var query3 = from p in _context.Btd5PoInfos
                         select new { p };
            query3 = query3.Where(w => w.p.Date >= request.From && w.p.Date <= request.To && w.p.Ws != "");
            var pcList = query3.Select(s => float.Parse(s.p.Plantime) * 4500).ToList();
            var maximumProductionCapacity = 0.0f;
            foreach (float a in pcList)
            {
                maximumProductionCapacity += a;
            }

            var data = new PtMonthYearVM()
            {
                ActualWorkingTime = wtVM.TestingTime + wtVM.RunningTime + wtVM.OtherTime + wtVM.BreakTime + wtVM.FixingTime + wtVM.PendingTime + wtVM.MaintenanceTime + wtVM.PauseTime,
                Downtime = wtVM.TestingTime + wtVM.OtherTime + wtVM.BreakTime + wtVM.FixingTime + wtVM.PendingTime + wtVM.MaintenanceTime + wtVM.PauseTime,
                TotalOutput = (totalOutput == null) ? 0 : totalOutput,
                MaximumProductionCapacity = (maximumProductionCapacity == null) ? 0 : maximumProductionCapacity,
                Waste = (waste == null) ? 0 : waste,
            };

            return data;
        }
        #endregion
        #region Year
        public async Task<PtMonthYearVM> GetBtd2ProductionTracking_Year(TimeRequest request)
        {
            var query1 = from p in _context.TableDays
                         select new { p };
            query1 = query1.Where(w => w.p.Date >= request.From && w.p.Date <= request.To);

            if (!query1.Any())
            {
                return new PtMonthYearVM()
                {
                    ActualWorkingTime = 0,
                    Downtime = 0,
                    TotalOutput = 0,
                    MaximumProductionCapacity = 0,
                    Waste = 0,
                };
            }

            var wtVM = query1.Where(w => w.p.Machine == "BTD2")
            .GroupBy(g => g.p.Machine)
            .Select(s => new WorkTimeVM()
            {
                RunningTime = (s.Sum(a => a.p.RunningTime) == null) ? 0 : s.Sum(a => a.p.RunningTime),
                TestingTime = (s.Sum(a => a.p.TestingTime) == null) ? 0 : s.Sum(a => a.p.TestingTime),
                OtherTime = (s.Sum(a => a.p.OtherTime) == null) ? 0 : s.Sum(a => a.p.OtherTime),
                BreakTime = (s.Sum(a => a.p.BreakTime) == null) ? 0 : s.Sum(a => a.p.BreakTime),
                FixingTime = (s.Sum(a => a.p.FixingTime) == null) ? 0 : s.Sum(a => a.p.FixingTime),
                PendingTime = (s.Sum(a => a.p.PendingTime) == null) ? 0 : s.Sum(a => a.p.PendingTime),
                MaintenanceTime = (s.Sum(a => a.p.MaintenanceTime) == null) ? 0 : s.Sum(a => a.p.MaintenanceTime),
                PauseTime = (s.Sum(a => a.p.PauseTime) == null) ? 0 : s.Sum(a => a.p.PauseTime),
            }).FirstOrDefault();
            if (wtVM is null)
            {
                wtVM = new WorkTimeVM()
                {
                    RunningTime = 0,
                    TestingTime = 0,
                    OtherTime = 0,
                    BreakTime = 0,
                    FixingTime = 0,
                    PendingTime = 0,
                    MaintenanceTime = 0,
                    PauseTime = 0,
                };
            }

            var query2 = from p in _context.Btd2PoKpis
                         select new { p };
            query2 = query2.Where(w => w.p.Starttime >= request.From && w.p.Starttime <= request.To && w.p.Ws != "");
            var totalOutput = await query2.Select(s => s.p.Job).SumAsync();
            var waste = await query2.Select(s => s.p.DefeatItem1 + s.p.DefeatItem2 + s.p.DefeatItem3 + s.p.DefeatItem4).SumAsync();

            var query3 = from p in _context.Btd2PoInfos
                         select new { p };
            query3 = query3.Where(w => w.p.Date >= request.From && w.p.Date <= request.To && w.p.Ws != "");
            var pcList = query3.Select(s => float.Parse(s.p.Plantime) * 4500).ToList();
            var maximumProductionCapacity = 0.0f;
            foreach (float a in pcList)
            {
                maximumProductionCapacity += a;
            }

            var data = new PtMonthYearVM()
            {
                ActualWorkingTime = wtVM.TestingTime + wtVM.RunningTime + wtVM.OtherTime + wtVM.BreakTime + wtVM.FixingTime + wtVM.PendingTime + wtVM.MaintenanceTime + wtVM.PauseTime,
                Downtime = wtVM.TestingTime + wtVM.OtherTime + wtVM.BreakTime + wtVM.FixingTime + wtVM.PendingTime + wtVM.MaintenanceTime + wtVM.PauseTime,
                TotalOutput = (totalOutput == null) ? 0 : totalOutput,
                MaximumProductionCapacity = (maximumProductionCapacity == null) ? 0 : maximumProductionCapacity,
                Waste = (waste == null) ? 0 : waste,
            };

            return data;
        }
        public async Task<PtMonthYearVM> GetBtd3ProductionTracking_Year(TimeRequest request)
        {
            var query1 = from p in _context.TableDays
                         select new { p };
            query1 = query1.Where(w => w.p.Date >= request.From && w.p.Date <= request.To);

            if (!query1.Any())
            {
                return new PtMonthYearVM()
                {
                    ActualWorkingTime = 0,
                    Downtime = 0,
                    TotalOutput = 0,
                    MaximumProductionCapacity = 0,
                    Waste = 0,
                };
            }

            var wtVM = query1.Where(w => w.p.Machine == "BTD3")
            .GroupBy(g => g.p.Machine)
            .Select(s => new WorkTimeVM()
            {
                RunningTime = (s.Sum(a => a.p.RunningTime) == null) ? 0 : s.Sum(a => a.p.RunningTime),
                TestingTime = (s.Sum(a => a.p.TestingTime) == null) ? 0 : s.Sum(a => a.p.TestingTime),
                OtherTime = (s.Sum(a => a.p.OtherTime) == null) ? 0 : s.Sum(a => a.p.OtherTime),
                BreakTime = (s.Sum(a => a.p.BreakTime) == null) ? 0 : s.Sum(a => a.p.BreakTime),
                FixingTime = (s.Sum(a => a.p.FixingTime) == null) ? 0 : s.Sum(a => a.p.FixingTime),
                PendingTime = (s.Sum(a => a.p.PendingTime) == null) ? 0 : s.Sum(a => a.p.PendingTime),
                MaintenanceTime = (s.Sum(a => a.p.MaintenanceTime) == null) ? 0 : s.Sum(a => a.p.MaintenanceTime),
                PauseTime = (s.Sum(a => a.p.PauseTime) == null) ? 0 : s.Sum(a => a.p.PauseTime),
            }).FirstOrDefault();
            if (wtVM is null)
            {
                wtVM = new WorkTimeVM()
                {
                    RunningTime = 0,
                    TestingTime = 0,
                    OtherTime = 0,
                    BreakTime = 0,
                    FixingTime = 0,
                    PendingTime = 0,
                    MaintenanceTime = 0,
                    PauseTime = 0,
                };
            }

            var query2 = from p in _context.Btd3PoKpis
                         select new { p };
            query2 = query2.Where(w => w.p.Starttime >= request.From && w.p.Starttime <= request.To && w.p.Ws != "");
            var totalOutput = await query2.Select(s => s.p.Job).SumAsync();
            var waste = await query2.Select(s => s.p.DefeatItem1 + s.p.DefeatItem2 + s.p.DefeatItem3 + s.p.DefeatItem4).SumAsync();

            var query3 = from p in _context.Btd3PoInfos
                         select new { p };
            query3 = query3.Where(w => w.p.Date >= request.From && w.p.Date <= request.To && w.p.Ws != "");
            var pcList = query3.Select(s => float.Parse(s.p.Plantime) * 4500).ToList();
            var maximumProductionCapacity = 0.0f;
            foreach (float a in pcList)
            {
                maximumProductionCapacity += a;
            }

            var data = new PtMonthYearVM()
            {
                ActualWorkingTime = wtVM.TestingTime + wtVM.RunningTime + wtVM.OtherTime + wtVM.BreakTime + wtVM.FixingTime + wtVM.PendingTime + wtVM.MaintenanceTime + wtVM.PauseTime,
                Downtime = wtVM.TestingTime + wtVM.OtherTime + wtVM.BreakTime + wtVM.FixingTime + wtVM.PendingTime + wtVM.MaintenanceTime + wtVM.PauseTime,
                TotalOutput = (totalOutput == null) ? 0 : totalOutput,
                MaximumProductionCapacity = (maximumProductionCapacity == null) ? 0 : maximumProductionCapacity,
                Waste = (waste == null) ? 0 : waste,
            };

            return data;
        }
        public async Task<PtMonthYearVM> GetBtd4ProductionTracking_Year(TimeRequest request)
        {
            var query1 = from p in _context.TableDays
                         select new { p };
            query1 = query1.Where(w => w.p.Date >= request.From && w.p.Date <= request.To);

            if (!query1.Any())
            {
                return new PtMonthYearVM()
                {
                    ActualWorkingTime = 0,
                    Downtime = 0,
                    TotalOutput = 0,
                    MaximumProductionCapacity = 0,
                    Waste = 0,
                };
            }

            var wtVM = query1.Where(w => w.p.Machine == "BTD4")
            .GroupBy(g => g.p.Machine)
            .Select(s => new WorkTimeVM()
            {
                RunningTime = (s.Sum(a => a.p.RunningTime) == null) ? 0 : s.Sum(a => a.p.RunningTime),
                TestingTime = (s.Sum(a => a.p.TestingTime) == null) ? 0 : s.Sum(a => a.p.TestingTime),
                OtherTime = (s.Sum(a => a.p.OtherTime) == null) ? 0 : s.Sum(a => a.p.OtherTime),
                BreakTime = (s.Sum(a => a.p.BreakTime) == null) ? 0 : s.Sum(a => a.p.BreakTime),
                FixingTime = (s.Sum(a => a.p.FixingTime) == null) ? 0 : s.Sum(a => a.p.FixingTime),
                PendingTime = (s.Sum(a => a.p.PendingTime) == null) ? 0 : s.Sum(a => a.p.PendingTime),
                MaintenanceTime = (s.Sum(a => a.p.MaintenanceTime) == null) ? 0 : s.Sum(a => a.p.MaintenanceTime),
                PauseTime = (s.Sum(a => a.p.PauseTime) == null) ? 0 : s.Sum(a => a.p.PauseTime),
            }).FirstOrDefault();
            if (wtVM is null)
            {
                wtVM = new WorkTimeVM()
                {
                    RunningTime = 0,
                    TestingTime = 0,
                    OtherTime = 0,
                    BreakTime = 0,
                    FixingTime = 0,
                    PendingTime = 0,
                    MaintenanceTime = 0,
                    PauseTime = 0,
                };
            }

            var query2 = from p in _context.Btd4PoKpis
                         select new { p };
            query2 = query2.Where(w => w.p.Starttime >= request.From && w.p.Starttime <= request.To && w.p.Ws != "");
            var totalOutput = await query2.Select(s => s.p.Job).SumAsync();
            var waste = await query2.Select(s => s.p.DefeatItem1 + s.p.DefeatItem2 + s.p.DefeatItem3 + s.p.DefeatItem4).SumAsync();

            var query3 = from p in _context.Btd4PoInfos
                         select new { p };
            query3 = query3.Where(w => w.p.Date >= request.From && w.p.Date <= request.To && w.p.Ws != "");
            var pcList = query3.Select(s => float.Parse(s.p.Plantime) * 4500).ToList();
            var maximumProductionCapacity = 0.0f;
            foreach (float a in pcList)
            {
                maximumProductionCapacity += a;
            }

            var data = new PtMonthYearVM()
            {
                ActualWorkingTime = wtVM.TestingTime + wtVM.RunningTime + wtVM.OtherTime + wtVM.BreakTime + wtVM.FixingTime + wtVM.PendingTime + wtVM.MaintenanceTime + wtVM.PauseTime,
                Downtime = wtVM.TestingTime + wtVM.OtherTime + wtVM.BreakTime + wtVM.FixingTime + wtVM.PendingTime + wtVM.MaintenanceTime + wtVM.PauseTime,
                TotalOutput = (totalOutput == null) ? 0 : totalOutput,
                MaximumProductionCapacity = (maximumProductionCapacity == null) ? 0 : maximumProductionCapacity,
                Waste = (waste == null) ? 0 : waste,
            };

            return data;
        }
        public async Task<PtMonthYearVM> GetBtd5ProductionTracking_Year(TimeRequest request)
        {
            var query1 = from p in _context.TableDays
                         select new { p };
            query1 = query1.Where(w => w.p.Date >= request.From && w.p.Date <= request.To);

            if (!query1.Any())
            {
                return new PtMonthYearVM()
                {
                    ActualWorkingTime = 0,
                    Downtime = 0,
                    TotalOutput = 0,
                    MaximumProductionCapacity = 0,
                    Waste = 0,
                };
            }

            var wtVM = query1.Where(w => w.p.Machine == "BTD5")
            .GroupBy(g => g.p.Machine)
            .Select(s => new WorkTimeVM()
            {
                RunningTime = (s.Sum(a => a.p.RunningTime) == null) ? 0 : s.Sum(a => a.p.RunningTime),
                TestingTime = (s.Sum(a => a.p.TestingTime) == null) ? 0 : s.Sum(a => a.p.TestingTime),
                OtherTime = (s.Sum(a => a.p.OtherTime) == null) ? 0 : s.Sum(a => a.p.OtherTime),
                BreakTime = (s.Sum(a => a.p.BreakTime) == null) ? 0 : s.Sum(a => a.p.BreakTime),
                FixingTime = (s.Sum(a => a.p.FixingTime) == null) ? 0 : s.Sum(a => a.p.FixingTime),
                PendingTime = (s.Sum(a => a.p.PendingTime) == null) ? 0 : s.Sum(a => a.p.PendingTime),
                MaintenanceTime = (s.Sum(a => a.p.MaintenanceTime) == null) ? 0 : s.Sum(a => a.p.MaintenanceTime),
                PauseTime = (s.Sum(a => a.p.PauseTime) == null) ? 0 : s.Sum(a => a.p.PauseTime),
            }).FirstOrDefault();
            if (wtVM is null)
            {
                wtVM = new WorkTimeVM()
                {
                    RunningTime = 0,
                    TestingTime = 0,
                    OtherTime = 0,
                    BreakTime = 0,
                    FixingTime = 0,
                    PendingTime = 0,
                    MaintenanceTime = 0,
                    PauseTime = 0,
                };
            }

            var query2 = from p in _context.Btd5PoKpis
                         select new { p };
            query2 = query2.Where(w => w.p.Starttime >= request.From && w.p.Starttime <= request.To && w.p.Ws != "");
            var totalOutput = await query2.Select(s => s.p.Job).SumAsync();
            var waste = await query2.Select(s => s.p.DefeatItem1 + s.p.DefeatItem2 + s.p.DefeatItem3 + s.p.DefeatItem4).SumAsync();

            var query3 = from p in _context.Btd5PoInfos
                         select new { p };
            query3 = query3.Where(w => w.p.Date >= request.From && w.p.Date <= request.To && w.p.Ws != "");
            var pcList = query3.Select(s => float.Parse(s.p.Plantime) * 4500).ToList();
            var maximumProductionCapacity = 0.0f;
            foreach (float a in pcList)
            {
                maximumProductionCapacity += a;
            }

            var data = new PtMonthYearVM()
            {
                ActualWorkingTime = wtVM.TestingTime + wtVM.RunningTime + wtVM.OtherTime + wtVM.BreakTime + wtVM.FixingTime + wtVM.PendingTime + wtVM.MaintenanceTime + wtVM.PauseTime,
                Downtime = wtVM.TestingTime + wtVM.OtherTime + wtVM.BreakTime + wtVM.FixingTime + wtVM.PendingTime + wtVM.MaintenanceTime + wtVM.PauseTime,
                TotalOutput = (totalOutput == null) ? 0 : totalOutput,
                MaximumProductionCapacity = (maximumProductionCapacity == null) ? 0 : maximumProductionCapacity,
                Waste = (waste == null) ? 0 : waste,
            };

            return data;
        }
        #endregion
        #endregion


        #region Gluing
        #region 1Hour
        public async Task<List<Pt1HVM>> GetD650ProductionTracking_1H(TimeRequest request)
        {
            var query = from a in _context.D650PoKpis
                        join b in _context.D650PoInfos on a.Ws equals b.Ws
                        where a.Starttime >= request.From && a.Starttime <= request.To && a.Ws != ""
                        orderby a.Starttime descending
                        select new Pt1HVM
                        {
                            Ws = a.Ws,
                            Product = b.Product,
                            Job = a.Job,
                            Target = a.Target,
                        };
            var data = await query.ToListAsync();

            return data;
        }
        public async Task<List<Pt1HVM>> GetD750ProductionTracking_1H(TimeRequest request)
        {
            var query = from a in _context.D750PoKpis
                        join b in _context.D750PoInfos on a.Ws equals b.Ws
                        where a.Starttime >= request.From && a.Starttime <= request.To && a.Ws != ""
                        orderby a.Starttime descending
                        select new Pt1HVM
                        {
                            Ws = a.Ws,
                            Product = b.Product,
                            Job = a.Job,
                            Target = a.Target,
                        };
            var data = await query.ToListAsync();

            return data;
        }
        public async Task<List<Pt1HVM>> GetD1000ProductionTracking_1H(TimeRequest request)
        {
            var query = from a in _context.D1000PoKpis
                        join b in _context.D1000PoInfos on a.Ws equals b.Ws
                        where a.Starttime >= request.From && a.Starttime <= request.To && a.Ws != ""
                        orderby a.Starttime descending
                        select new Pt1HVM
                        {
                            Ws = a.Ws,
                            Product = b.Product,
                            Job = a.Job,
                            Target = a.Target,
                        };
            var data = await query.ToListAsync();

            return data;
        }
        public async Task<List<Pt1HVM>> GetD1100ProductionTracking_1H(TimeRequest request)
        {
            var query = from a in _context.D1100PoKpis
                        join b in _context.D1100PoInfos on a.Ws equals b.Ws
                        where a.Starttime >= request.From && a.Starttime <= request.To && a.Ws != ""
                        orderby a.Starttime descending
                        select new Pt1HVM
                        {
                            Ws = a.Ws,
                            Product = b.Product,
                            Job = a.Job,
                            Target = a.Target,
                        };
            var data = await query.ToListAsync();

            return data;
        }
        #endregion
        #region Day
        public async Task<List<PtDayVM>> GetD650ProductionTracking_Day(TimeRequest request)
        {
            var query = from a in _context.D650PoKpis
                        join b in _context.D650PoInfos on a.Ws equals b.Ws
                        where a.Starttime >= request.From && a.Starttime <= request.To && a.Ws != ""
                        orderby a.Starttime descending
                        select new PtDayVM
                        {
                            Ws = a.Ws,
                            Product = b.Product,
                            Job = a.Job,
                            Endtime = a.Endtime,
                            Plantime = b.Plantime,
                            PlanEndtime = b.PlanEndtime,
                            Status = a.Status,
                        };
            var data = await query.ToListAsync();

            return data;
        }
        public async Task<List<PtDayVM>> GetD750ProductionTracking_Day(TimeRequest request)
        {
            var query = from a in _context.D750PoKpis
                        join b in _context.D750PoInfos on a.Ws equals b.Ws
                        where a.Starttime >= request.From && a.Starttime <= request.To && a.Ws != ""
                        orderby a.Starttime descending
                        select new PtDayVM
                        {
                            Ws = a.Ws,
                            Product = b.Product,
                            Job = a.Job,
                            Endtime = a.Endtime,
                            Plantime = b.Plantime,
                            PlanEndtime = b.PlanEndtime,
                            Status = a.Status,
                        };
            var data = await query.ToListAsync();

            return data;
        }
        public async Task<List<PtDayVM>> GetD1000ProductionTracking_Day(TimeRequest request)
        {
            var query = from a in _context.D1000PoKpis
                        join b in _context.D1000PoInfos on a.Ws equals b.Ws
                        where a.Starttime >= request.From && a.Starttime <= request.To && a.Ws != ""
                        orderby a.Starttime descending
                        select new PtDayVM
                        {
                            Ws = a.Ws,
                            Product = b.Product,
                            Job = a.Job,
                            Endtime = a.Endtime,
                            Plantime = b.Plantime,
                            PlanEndtime = b.PlanEndtime,
                            Status = a.Status,
                        };
            var data = await query.ToListAsync();

            return data;
        }
        public async Task<List<PtDayVM>> GetD1100ProductionTracking_Day(TimeRequest request)
        {
            var query = from a in _context.D1100PoKpis
                        join b in _context.D1100PoInfos on a.Ws equals b.Ws
                        where a.Starttime >= request.From && a.Starttime <= request.To && a.Ws != ""
                        orderby a.Starttime descending
                        select new PtDayVM
                        {
                            Ws = a.Ws,
                            Product = b.Product,
                            Job = a.Job,
                            Endtime = a.Endtime,
                            Plantime = b.Plantime,
                            PlanEndtime = b.PlanEndtime,
                            Status = a.Status,
                        };
            var data = await query.ToListAsync();

            return data;
        }
        #endregion
        #region Week
        public async Task<PtWeekVM> GetD650ProductionTracking_Week(TimeRequest request)
        {
            var query1 = from p in _context.TableDays
                         select new { p };
            query1 = query1.Where(w => w.p.Date >= request.From && w.p.Date <= request.To);
            if (!query1.Any())
            {
                var empty = new WorkTimeVM() { RunningTime = 0, TestingTime = 0, OtherTime = 0, BreakTime = 0, FixingTime = 0, PendingTime = 0, MaintenanceTime = 0, PauseTime = 0 };
                return new PtWeekVM()
                {
                    TestingTime = 0,
                    RunningTime = 0,
                    NotRunningTime = 0,
                    TotalTime = 0,
                    TestingTimeAndPercent = "0h 0%",
                    RunningTimeAndPercent = "0h 0%",
                    NotRunningTimeAndPercent = "0h 0%",
                    ProductCodeChangeCount = 0,
                    Quantity = 0,
                    Waste = 0,
                };
            }

            var wtVM = query1.Where(w => w.p.Machine == "D650")
            .GroupBy(g => g.p.Machine)
            .Select(s => new WorkTimeVM()
            {
                RunningTime = (s.Sum(a => a.p.RunningTime) == null) ? 0 : s.Sum(a => a.p.RunningTime),
                TestingTime = (s.Sum(a => a.p.TestingTime) == null) ? 0 : s.Sum(a => a.p.TestingTime),
                OtherTime = (s.Sum(a => a.p.OtherTime) == null) ? 0 : s.Sum(a => a.p.OtherTime),
                BreakTime = (s.Sum(a => a.p.BreakTime) == null) ? 0 : s.Sum(a => a.p.BreakTime),
                FixingTime = (s.Sum(a => a.p.FixingTime) == null) ? 0 : s.Sum(a => a.p.FixingTime),
                PendingTime = (s.Sum(a => a.p.PendingTime) == null) ? 0 : s.Sum(a => a.p.PendingTime),
                MaintenanceTime = (s.Sum(a => a.p.MaintenanceTime) == null) ? 0 : s.Sum(a => a.p.MaintenanceTime),
                PauseTime = (s.Sum(a => a.p.PauseTime) == null) ? 0 : s.Sum(a => a.p.PauseTime),
            }).FirstOrDefault();

            var query2 = (from a in _context.D650PoKpis
                          join b in _context.D650PoInfos on a.Ws equals b.Ws
                          where a.Starttime >= request.From && a.Starttime <= request.To && a.Ws != ""
                          select new { a, b });



            var totalTime = (wtVM is null) ? 0 : wtVM.TestingTime + wtVM.RunningTime + wtVM.OtherTime + wtVM.BreakTime + wtVM.FixingTime + wtVM.PendingTime + wtVM.MaintenanceTime + wtVM.PauseTime;
            var notRunningTime = (wtVM is null) ? 0 : wtVM.OtherTime + wtVM.BreakTime + wtVM.FixingTime + wtVM.PendingTime + wtVM.MaintenanceTime + wtVM.PauseTime;
            var productCodeChange = await query2.Select(x => x.b.Product).Distinct().CountAsync();
            var quantity = await query2.Select(x => x.a.Job).SumAsync();
            var waste = await query2.Select(x => x.a.DefeatItem1 + x.a.DefeatItem2 + x.a.DefeatItem3 + x.a.DefeatItem4).SumAsync();

            var data = new PtWeekVM()
            {
                TestingTime = (wtVM is null) ? 0 : wtVM.TestingTime,
                RunningTime = (wtVM is null) ? 0 : wtVM.RunningTime,
                NotRunningTime = notRunningTime,
                TotalTime = totalTime,
                TestingTimeAndPercent = (wtVM is null || totalTime == 0) ? "0h 0%" : Math.Round((decimal)wtVM.TestingTime, 1).ToString() + "h " + Math.Round((decimal)(wtVM.TestingTime * 100 / totalTime), 1).ToString() + "%",
                RunningTimeAndPercent = (wtVM is null || totalTime == 0) ? "0h 0%" : Math.Round((decimal)wtVM.RunningTime, 1).ToString() + "h " + Math.Round((decimal)(wtVM.RunningTime * 100 / totalTime), 1).ToString() + "%",
                NotRunningTimeAndPercent = (totalTime == 0) ? "0h 0%" : Math.Round((decimal)notRunningTime, 1).ToString() + "h " + Math.Round((decimal)(notRunningTime * 100 / totalTime), 1).ToString() + "%",
                ProductCodeChangeCount = productCodeChange,
                Quantity = quantity,
                Waste = waste,
            };

            return data;
        }
        public async Task<PtWeekVM> GetD750ProductionTracking_Week(TimeRequest request)
        {
            var query1 = from p in _context.TableDays
                         select new { p };
            query1 = query1.Where(w => w.p.Date >= request.From && w.p.Date <= request.To);
            if (!query1.Any())
            {
                var empty = new WorkTimeVM() { RunningTime = 0, TestingTime = 0, OtherTime = 0, BreakTime = 0, FixingTime = 0, PendingTime = 0, MaintenanceTime = 0, PauseTime = 0 };
                return new PtWeekVM()
                {
                    TestingTime = 0,
                    RunningTime = 0,
                    NotRunningTime = 0,
                    TotalTime = 0,
                    TestingTimeAndPercent = "0h 0%",
                    RunningTimeAndPercent = "0h 0%",
                    NotRunningTimeAndPercent = "0h 0%",
                    ProductCodeChangeCount = 0,
                    Quantity = 0,
                    Waste = 0,
                };
            }

            var wtVM = query1.Where(w => w.p.Machine == "D750")
            .GroupBy(g => g.p.Machine)
            .Select(s => new WorkTimeVM()
            {
                RunningTime = (s.Sum(a => a.p.RunningTime) == null) ? 0 : s.Sum(a => a.p.RunningTime),
                TestingTime = (s.Sum(a => a.p.TestingTime) == null) ? 0 : s.Sum(a => a.p.TestingTime),
                OtherTime = (s.Sum(a => a.p.OtherTime) == null) ? 0 : s.Sum(a => a.p.OtherTime),
                BreakTime = (s.Sum(a => a.p.BreakTime) == null) ? 0 : s.Sum(a => a.p.BreakTime),
                FixingTime = (s.Sum(a => a.p.FixingTime) == null) ? 0 : s.Sum(a => a.p.FixingTime),
                PendingTime = (s.Sum(a => a.p.PendingTime) == null) ? 0 : s.Sum(a => a.p.PendingTime),
                MaintenanceTime = (s.Sum(a => a.p.MaintenanceTime) == null) ? 0 : s.Sum(a => a.p.MaintenanceTime),
                PauseTime = (s.Sum(a => a.p.PauseTime) == null) ? 0 : s.Sum(a => a.p.PauseTime),
            }).FirstOrDefault();

            var query2 = (from a in _context.D750PoKpis
                          join b in _context.D750PoInfos on a.Ws equals b.Ws
                          where a.Starttime >= request.From && a.Starttime <= request.To && a.Ws != ""
                          select new { a, b });



            var totalTime = (wtVM is null) ? 0 : wtVM.TestingTime + wtVM.RunningTime + wtVM.OtherTime + wtVM.BreakTime + wtVM.FixingTime + wtVM.PendingTime + wtVM.MaintenanceTime + wtVM.PauseTime;
            var notRunningTime = (wtVM is null) ? 0 : wtVM.OtherTime + wtVM.BreakTime + wtVM.FixingTime + wtVM.PendingTime + wtVM.MaintenanceTime + wtVM.PauseTime;
            var productCodeChange = await query2.Select(x => x.b.Product).Distinct().CountAsync();
            var quantity = await query2.Select(x => x.a.Job).SumAsync();
            var waste = await query2.Select(x => x.a.DefeatItem1 + x.a.DefeatItem2 + x.a.DefeatItem3 + x.a.DefeatItem4).SumAsync();

            var data = new PtWeekVM()
            {
                TestingTime = (wtVM is null) ? 0 : wtVM.TestingTime,
                RunningTime = (wtVM is null) ? 0 : wtVM.RunningTime,
                NotRunningTime = notRunningTime,
                TotalTime = totalTime,
                TestingTimeAndPercent = (wtVM is null || totalTime == 0) ? "0h 0%" : Math.Round((decimal)wtVM.TestingTime, 1).ToString() + "h " + Math.Round((decimal)(wtVM.TestingTime * 100 / totalTime), 1).ToString() + "%",
                RunningTimeAndPercent = (wtVM is null || totalTime == 0) ? "0h 0%" : Math.Round((decimal)wtVM.RunningTime, 1).ToString() + "h " + Math.Round((decimal)(wtVM.RunningTime * 100 / totalTime), 1).ToString() + "%",
                NotRunningTimeAndPercent = (totalTime == 0) ? "0h 0%" : Math.Round((decimal)notRunningTime, 1).ToString() + "h " + Math.Round((decimal)(notRunningTime * 100 / totalTime), 1).ToString() + "%",
                ProductCodeChangeCount = productCodeChange,
                Quantity = quantity,
                Waste = waste,
            };

            return data;
        }
        public async Task<PtWeekVM> GetD1000ProductionTracking_Week(TimeRequest request)
        {
            var query1 = from p in _context.TableDays
                         select new { p };
            query1 = query1.Where(w => w.p.Date >= request.From && w.p.Date <= request.To);
            if (!query1.Any())
            {
                var empty = new WorkTimeVM() { RunningTime = 0, TestingTime = 0, OtherTime = 0, BreakTime = 0, FixingTime = 0, PendingTime = 0, MaintenanceTime = 0, PauseTime = 0 };
                return new PtWeekVM()
                {
                    TestingTime = 0,
                    RunningTime = 0,
                    NotRunningTime = 0,
                    TotalTime = 0,
                    TestingTimeAndPercent = "0h 0%",
                    RunningTimeAndPercent = "0h 0%",
                    NotRunningTimeAndPercent = "0h 0%",
                    ProductCodeChangeCount = 0,
                    Quantity = 0,
                    Waste = 0,
                };
            }

            var wtVM = query1.Where(w => w.p.Machine == "D1000")
            .GroupBy(g => g.p.Machine)
            .Select(s => new WorkTimeVM()
            {
                RunningTime = (s.Sum(a => a.p.RunningTime) == null) ? 0 : s.Sum(a => a.p.RunningTime),
                TestingTime = (s.Sum(a => a.p.TestingTime) == null) ? 0 : s.Sum(a => a.p.TestingTime),
                OtherTime = (s.Sum(a => a.p.OtherTime) == null) ? 0 : s.Sum(a => a.p.OtherTime),
                BreakTime = (s.Sum(a => a.p.BreakTime) == null) ? 0 : s.Sum(a => a.p.BreakTime),
                FixingTime = (s.Sum(a => a.p.FixingTime) == null) ? 0 : s.Sum(a => a.p.FixingTime),
                PendingTime = (s.Sum(a => a.p.PendingTime) == null) ? 0 : s.Sum(a => a.p.PendingTime),
                MaintenanceTime = (s.Sum(a => a.p.MaintenanceTime) == null) ? 0 : s.Sum(a => a.p.MaintenanceTime),
                PauseTime = (s.Sum(a => a.p.PauseTime) == null) ? 0 : s.Sum(a => a.p.PauseTime),
            }).FirstOrDefault();

            var query2 = (from a in _context.D1000PoKpis
                          join b in _context.D1000PoInfos on a.Ws equals b.Ws
                          where a.Starttime >= request.From && a.Starttime <= request.To && a.Ws != ""
                          select new { a, b });



            var totalTime = (wtVM is null) ? 0 : wtVM.TestingTime + wtVM.RunningTime + wtVM.OtherTime + wtVM.BreakTime + wtVM.FixingTime + wtVM.PendingTime + wtVM.MaintenanceTime + wtVM.PauseTime;
            var notRunningTime = (wtVM is null) ? 0 : wtVM.OtherTime + wtVM.BreakTime + wtVM.FixingTime + wtVM.PendingTime + wtVM.MaintenanceTime + wtVM.PauseTime;
            var productCodeChange = await query2.Select(x => x.b.Product).Distinct().CountAsync();
            var quantity = await query2.Select(x => x.a.Job).SumAsync();
            var waste = await query2.Select(x => x.a.DefeatItem1 + x.a.DefeatItem2 + x.a.DefeatItem3 + x.a.DefeatItem4).SumAsync();

            var data = new PtWeekVM()
            {
                TestingTime = (wtVM is null) ? 0 : wtVM.TestingTime,
                RunningTime = (wtVM is null) ? 0 : wtVM.RunningTime,
                NotRunningTime = notRunningTime,
                TotalTime = totalTime,
                TestingTimeAndPercent = (wtVM is null || totalTime == 0) ? "0h 0%" : Math.Round((decimal)wtVM.TestingTime, 1).ToString() + "h " + Math.Round((decimal)(wtVM.TestingTime * 100 / totalTime), 1).ToString() + "%",
                RunningTimeAndPercent = (wtVM is null || totalTime == 0) ? "0h 0%" : Math.Round((decimal)wtVM.RunningTime, 1).ToString() + "h " + Math.Round((decimal)(wtVM.RunningTime * 100 / totalTime), 1).ToString() + "%",
                NotRunningTimeAndPercent = (totalTime == 0) ? "0h 0%" : Math.Round((decimal)notRunningTime, 1).ToString() + "h " + Math.Round((decimal)(notRunningTime * 100 / totalTime), 1).ToString() + "%",
                ProductCodeChangeCount = productCodeChange,
                Quantity = quantity,
                Waste = waste,
            };

            return data;
        }
        public async Task<PtWeekVM> GetD1100ProductionTracking_Week(TimeRequest request)
        {
            var query1 = from p in _context.TableDays
                         select new { p };
            query1 = query1.Where(w => w.p.Date >= request.From && w.p.Date <= request.To);
            if (!query1.Any())
            {
                var empty = new WorkTimeVM() { RunningTime = 0, TestingTime = 0, OtherTime = 0, BreakTime = 0, FixingTime = 0, PendingTime = 0, MaintenanceTime = 0, PauseTime = 0 };
                return new PtWeekVM()
                {
                    TestingTime = 0,
                    RunningTime = 0,
                    NotRunningTime = 0,
                    TotalTime = 0,
                    TestingTimeAndPercent = "0h 0%",
                    RunningTimeAndPercent = "0h 0%",
                    NotRunningTimeAndPercent = "0h 0%",
                    ProductCodeChangeCount = 0,
                    Quantity = 0,
                    Waste = 0,
                };
            }

            var wtVM = query1.Where(w => w.p.Machine == "D1100")
            .GroupBy(g => g.p.Machine)
            .Select(s => new WorkTimeVM()
            {
                RunningTime = (s.Sum(a => a.p.RunningTime) == null) ? 0 : s.Sum(a => a.p.RunningTime),
                TestingTime = (s.Sum(a => a.p.TestingTime) == null) ? 0 : s.Sum(a => a.p.TestingTime),
                OtherTime = (s.Sum(a => a.p.OtherTime) == null) ? 0 : s.Sum(a => a.p.OtherTime),
                BreakTime = (s.Sum(a => a.p.BreakTime) == null) ? 0 : s.Sum(a => a.p.BreakTime),
                FixingTime = (s.Sum(a => a.p.FixingTime) == null) ? 0 : s.Sum(a => a.p.FixingTime),
                PendingTime = (s.Sum(a => a.p.PendingTime) == null) ? 0 : s.Sum(a => a.p.PendingTime),
                MaintenanceTime = (s.Sum(a => a.p.MaintenanceTime) == null) ? 0 : s.Sum(a => a.p.MaintenanceTime),
                PauseTime = (s.Sum(a => a.p.PauseTime) == null) ? 0 : s.Sum(a => a.p.PauseTime),
            }).FirstOrDefault();

            var query2 = (from a in _context.D1100PoKpis
                          join b in _context.D1100PoInfos on a.Ws equals b.Ws
                          where a.Starttime >= request.From && a.Starttime <= request.To && a.Ws != ""
                          select new { a, b });



            var totalTime = (wtVM is null) ? 0 : wtVM.TestingTime + wtVM.RunningTime + wtVM.OtherTime + wtVM.BreakTime + wtVM.FixingTime + wtVM.PendingTime + wtVM.MaintenanceTime + wtVM.PauseTime;
            var notRunningTime = (wtVM is null) ? 0 : wtVM.OtherTime + wtVM.BreakTime + wtVM.FixingTime + wtVM.PendingTime + wtVM.MaintenanceTime + wtVM.PauseTime;
            var productCodeChange = await query2.Select(x => x.b.Product).Distinct().CountAsync();
            var quantity = await query2.Select(x => x.a.Job).SumAsync();
            var waste = await query2.Select(x => x.a.DefeatItem1 + x.a.DefeatItem2 + x.a.DefeatItem3 + x.a.DefeatItem4).SumAsync();

            var data = new PtWeekVM()
            {
                TestingTime = (wtVM is null) ? 0 : wtVM.TestingTime,
                RunningTime = (wtVM is null) ? 0 : wtVM.RunningTime,
                NotRunningTime = notRunningTime,
                TotalTime = totalTime,
                TestingTimeAndPercent = (wtVM is null || totalTime == 0) ? "0h 0%" : Math.Round((decimal)wtVM.TestingTime, 1).ToString() + "h " + Math.Round((decimal)(wtVM.TestingTime * 100 / totalTime), 1).ToString() + "%",
                RunningTimeAndPercent = (wtVM is null || totalTime == 0) ? "0h 0%" : Math.Round((decimal)wtVM.RunningTime, 1).ToString() + "h " + Math.Round((decimal)(wtVM.RunningTime * 100 / totalTime), 1).ToString() + "%",
                NotRunningTimeAndPercent = (totalTime == 0) ? "0h 0%" : Math.Round((decimal)notRunningTime, 1).ToString() + "h " + Math.Round((decimal)(notRunningTime * 100 / totalTime), 1).ToString() + "%",
                ProductCodeChangeCount = productCodeChange,
                Quantity = quantity,
                Waste = waste,
            };

            return data;
        }
        #endregion
        #region Month
        public async Task<PtMonthYearVM> GetD650ProductionTracking_Month(TimeRequest request)
        {
            var query1 = from p in _context.TableDays
                         select new { p };
            query1 = query1.Where(w => w.p.Date >= request.From && w.p.Date <= request.To);

            if (!query1.Any())
            {
                return new PtMonthYearVM()
                {
                    ActualWorkingTime = 0,
                    Downtime = 0,
                    TotalOutput = 0,
                    MaximumProductionCapacity = 0,
                    Waste = 0,
                };
            }

            var wtVM = query1.Where(w => w.p.Machine == "D650")
            .GroupBy(g => g.p.Machine)
            .Select(s => new WorkTimeVM()
            {
                RunningTime = (s.Sum(a => a.p.RunningTime) == null) ? 0 : s.Sum(a => a.p.RunningTime),
                TestingTime = (s.Sum(a => a.p.TestingTime) == null) ? 0 : s.Sum(a => a.p.TestingTime),
                OtherTime = (s.Sum(a => a.p.OtherTime) == null) ? 0 : s.Sum(a => a.p.OtherTime),
                BreakTime = (s.Sum(a => a.p.BreakTime) == null) ? 0 : s.Sum(a => a.p.BreakTime),
                FixingTime = (s.Sum(a => a.p.FixingTime) == null) ? 0 : s.Sum(a => a.p.FixingTime),
                PendingTime = (s.Sum(a => a.p.PendingTime) == null) ? 0 : s.Sum(a => a.p.PendingTime),
                MaintenanceTime = (s.Sum(a => a.p.MaintenanceTime) == null) ? 0 : s.Sum(a => a.p.MaintenanceTime),
                PauseTime = (s.Sum(a => a.p.PauseTime) == null) ? 0 : s.Sum(a => a.p.PauseTime),
            }).FirstOrDefault();
            if (wtVM is null)
            {
                wtVM = new WorkTimeVM()
                {
                    RunningTime = 0,
                    TestingTime = 0,
                    OtherTime = 0,
                    BreakTime = 0,
                    FixingTime = 0,
                    PendingTime = 0,
                    MaintenanceTime = 0,
                    PauseTime = 0,
                };
            }

            var query2 = from p in _context.D650PoKpis
                         select new { p };
            query2 = query2.Where(w => w.p.Starttime >= request.From && w.p.Starttime <= request.To && w.p.Ws != "");
            var totalOutput = await query2.Select(s => s.p.Job).SumAsync();
            var waste = await query2.Select(s => s.p.DefeatItem1 + s.p.DefeatItem2 + s.p.DefeatItem3 + s.p.DefeatItem4).SumAsync();

            var query3 = from p in _context.D650PoInfos
                         select new { p };
            query3 = query3.Where(w => w.p.Date >= request.From && w.p.Date <= request.To && w.p.Ws != "");
            var pcList = query3.Select(s => float.Parse(s.p.Plantime) * 18000).ToList();
            var maximumProductionCapacity = 0.0f;
            foreach (float a in pcList)
            {
                maximumProductionCapacity += a;
            }

            var data = new PtMonthYearVM()
            {
                ActualWorkingTime = wtVM.TestingTime + wtVM.RunningTime + wtVM.OtherTime + wtVM.BreakTime + wtVM.FixingTime + wtVM.PendingTime + wtVM.MaintenanceTime + wtVM.PauseTime,
                Downtime = wtVM.TestingTime + wtVM.OtherTime + wtVM.BreakTime + wtVM.FixingTime + wtVM.PendingTime + wtVM.MaintenanceTime + wtVM.PauseTime,
                TotalOutput = (totalOutput == null) ? 0 : totalOutput,
                MaximumProductionCapacity = (maximumProductionCapacity == null) ? 0 : maximumProductionCapacity,
                Waste = (waste == null) ? 0 : waste,
            };

            return data;
        }
        public async Task<PtMonthYearVM> GetD750ProductionTracking_Month(TimeRequest request)
        {
            var query1 = from p in _context.TableDays
                         select new { p };
            query1 = query1.Where(w => w.p.Date >= request.From && w.p.Date <= request.To);

            if (!query1.Any())
            {
                return new PtMonthYearVM()
                {
                    ActualWorkingTime = 0,
                    Downtime = 0,
                    TotalOutput = 0,
                    MaximumProductionCapacity = 0,
                    Waste = 0,
                };
            }

            var wtVM = query1.Where(w => w.p.Machine == "D750")
            .GroupBy(g => g.p.Machine)
            .Select(s => new WorkTimeVM()
            {
                RunningTime = (s.Sum(a => a.p.RunningTime) == null) ? 0 : s.Sum(a => a.p.RunningTime),
                TestingTime = (s.Sum(a => a.p.TestingTime) == null) ? 0 : s.Sum(a => a.p.TestingTime),
                OtherTime = (s.Sum(a => a.p.OtherTime) == null) ? 0 : s.Sum(a => a.p.OtherTime),
                BreakTime = (s.Sum(a => a.p.BreakTime) == null) ? 0 : s.Sum(a => a.p.BreakTime),
                FixingTime = (s.Sum(a => a.p.FixingTime) == null) ? 0 : s.Sum(a => a.p.FixingTime),
                PendingTime = (s.Sum(a => a.p.PendingTime) == null) ? 0 : s.Sum(a => a.p.PendingTime),
                MaintenanceTime = (s.Sum(a => a.p.MaintenanceTime) == null) ? 0 : s.Sum(a => a.p.MaintenanceTime),
                PauseTime = (s.Sum(a => a.p.PauseTime) == null) ? 0 : s.Sum(a => a.p.PauseTime),
            }).FirstOrDefault();
            if (wtVM is null)
            {
                wtVM = new WorkTimeVM()
                {
                    RunningTime = 0,
                    TestingTime = 0,
                    OtherTime = 0,
                    BreakTime = 0,
                    FixingTime = 0,
                    PendingTime = 0,
                    MaintenanceTime = 0,
                    PauseTime = 0,
                };
            }

            var query2 = from p in _context.D750PoKpis
                         select new { p };
            query2 = query2.Where(w => w.p.Starttime >= request.From && w.p.Starttime <= request.To && w.p.Ws != "");
            var totalOutput = await query2.Select(s => s.p.Job).SumAsync();
            var waste = await query2.Select(s => s.p.DefeatItem1 + s.p.DefeatItem2 + s.p.DefeatItem3 + s.p.DefeatItem4).SumAsync();

            var query3 = from p in _context.D750PoInfos
                         select new { p };
            query3 = query3.Where(w => w.p.Date >= request.From && w.p.Date <= request.To && w.p.Ws != "");
            var pcList = query3.Select(s => float.Parse(s.p.Plantime) * 18000).ToList();
            var maximumProductionCapacity = 0.0f;
            foreach (float a in pcList)
            {
                maximumProductionCapacity += a;
            }

            var data = new PtMonthYearVM()
            {
                ActualWorkingTime = wtVM.TestingTime + wtVM.RunningTime + wtVM.OtherTime + wtVM.BreakTime + wtVM.FixingTime + wtVM.PendingTime + wtVM.MaintenanceTime + wtVM.PauseTime,
                Downtime = wtVM.TestingTime + wtVM.OtherTime + wtVM.BreakTime + wtVM.FixingTime + wtVM.PendingTime + wtVM.MaintenanceTime + wtVM.PauseTime,
                TotalOutput = (totalOutput == null) ? 0 : totalOutput,
                MaximumProductionCapacity = (maximumProductionCapacity == null) ? 0 : maximumProductionCapacity,
                Waste = (waste == null) ? 0 : waste,
            };

            return data;
        }
        public async Task<PtMonthYearVM> GetD1000ProductionTracking_Month(TimeRequest request)
        {
            var query1 = from p in _context.TableDays
                         select new { p };
            query1 = query1.Where(w => w.p.Date >= request.From && w.p.Date <= request.To);

            if (!query1.Any())
            {
                return new PtMonthYearVM()
                {
                    ActualWorkingTime = 0,
                    Downtime = 0,
                    TotalOutput = 0,
                    MaximumProductionCapacity = 0,
                    Waste = 0,
                };
            }

            var wtVM = query1.Where(w => w.p.Machine == "D1000")
            .GroupBy(g => g.p.Machine)
            .Select(s => new WorkTimeVM()
            {
                RunningTime = (s.Sum(a => a.p.RunningTime) == null) ? 0 : s.Sum(a => a.p.RunningTime),
                TestingTime = (s.Sum(a => a.p.TestingTime) == null) ? 0 : s.Sum(a => a.p.TestingTime),
                OtherTime = (s.Sum(a => a.p.OtherTime) == null) ? 0 : s.Sum(a => a.p.OtherTime),
                BreakTime = (s.Sum(a => a.p.BreakTime) == null) ? 0 : s.Sum(a => a.p.BreakTime),
                FixingTime = (s.Sum(a => a.p.FixingTime) == null) ? 0 : s.Sum(a => a.p.FixingTime),
                PendingTime = (s.Sum(a => a.p.PendingTime) == null) ? 0 : s.Sum(a => a.p.PendingTime),
                MaintenanceTime = (s.Sum(a => a.p.MaintenanceTime) == null) ? 0 : s.Sum(a => a.p.MaintenanceTime),
                PauseTime = (s.Sum(a => a.p.PauseTime) == null) ? 0 : s.Sum(a => a.p.PauseTime),
            }).FirstOrDefault();
            if (wtVM is null)
            {
                wtVM = new WorkTimeVM()
                {
                    RunningTime = 0,
                    TestingTime = 0,
                    OtherTime = 0,
                    BreakTime = 0,
                    FixingTime = 0,
                    PendingTime = 0,
                    MaintenanceTime = 0,
                    PauseTime = 0,
                };
            }

            var query2 = from p in _context.D1000PoKpis
                         select new { p };
            query2 = query2.Where(w => w.p.Starttime >= request.From && w.p.Starttime <= request.To && w.p.Ws != "");
            var totalOutput = await query2.Select(s => s.p.Job).SumAsync();
            var waste = await query2.Select(s => s.p.DefeatItem1 + s.p.DefeatItem2 + s.p.DefeatItem3 + s.p.DefeatItem4).SumAsync();

            var query3 = from p in _context.D1000PoInfos
                         select new { p };
            query3 = query3.Where(w => w.p.Date >= request.From && w.p.Date <= request.To && w.p.Ws != "");
            var pcList = query3.Select(s => float.Parse(s.p.Plantime) * 18000).ToList();
            var maximumProductionCapacity = 0.0f;
            foreach (float a in pcList)
            {
                maximumProductionCapacity += a;
            }

            var data = new PtMonthYearVM()
            {
                ActualWorkingTime = wtVM.TestingTime + wtVM.RunningTime + wtVM.OtherTime + wtVM.BreakTime + wtVM.FixingTime + wtVM.PendingTime + wtVM.MaintenanceTime + wtVM.PauseTime,
                Downtime = wtVM.TestingTime + wtVM.OtherTime + wtVM.BreakTime + wtVM.FixingTime + wtVM.PendingTime + wtVM.MaintenanceTime + wtVM.PauseTime,
                TotalOutput = (totalOutput == null) ? 0 : totalOutput,
                MaximumProductionCapacity = (maximumProductionCapacity == null) ? 0 : maximumProductionCapacity,
                Waste = (waste == null) ? 0 : waste,
            };

            return data;
        }
        public async Task<PtMonthYearVM> GetD1100ProductionTracking_Month(TimeRequest request)
        {
            var query1 = from p in _context.TableDays
                         select new { p };
            query1 = query1.Where(w => w.p.Date >= request.From && w.p.Date <= request.To);

            if (!query1.Any())
            {
                return new PtMonthYearVM()
                {
                    ActualWorkingTime = 0,
                    Downtime = 0,
                    TotalOutput = 0,
                    MaximumProductionCapacity = 0,
                    Waste = 0,
                };
            }

            var wtVM = query1.Where(w => w.p.Machine == "D1100")
            .GroupBy(g => g.p.Machine)
            .Select(s => new WorkTimeVM()
            {
                RunningTime = (s.Sum(a => a.p.RunningTime) == null) ? 0 : s.Sum(a => a.p.RunningTime),
                TestingTime = (s.Sum(a => a.p.TestingTime) == null) ? 0 : s.Sum(a => a.p.TestingTime),
                OtherTime = (s.Sum(a => a.p.OtherTime) == null) ? 0 : s.Sum(a => a.p.OtherTime),
                BreakTime = (s.Sum(a => a.p.BreakTime) == null) ? 0 : s.Sum(a => a.p.BreakTime),
                FixingTime = (s.Sum(a => a.p.FixingTime) == null) ? 0 : s.Sum(a => a.p.FixingTime),
                PendingTime = (s.Sum(a => a.p.PendingTime) == null) ? 0 : s.Sum(a => a.p.PendingTime),
                MaintenanceTime = (s.Sum(a => a.p.MaintenanceTime) == null) ? 0 : s.Sum(a => a.p.MaintenanceTime),
                PauseTime = (s.Sum(a => a.p.PauseTime) == null) ? 0 : s.Sum(a => a.p.PauseTime),
            }).FirstOrDefault();
            if (wtVM is null)
            {
                wtVM = new WorkTimeVM()
                {
                    RunningTime = 0,
                    TestingTime = 0,
                    OtherTime = 0,
                    BreakTime = 0,
                    FixingTime = 0,
                    PendingTime = 0,
                    MaintenanceTime = 0,
                    PauseTime = 0,
                };
            }

            var query2 = from p in _context.D1100PoKpis
                         select new { p };
            query2 = query2.Where(w => w.p.Starttime >= request.From && w.p.Starttime <= request.To && w.p.Ws != "");
            var totalOutput = await query2.Select(s => s.p.Job).SumAsync();
            var waste = await query2.Select(s => s.p.DefeatItem1 + s.p.DefeatItem2 + s.p.DefeatItem3 + s.p.DefeatItem4).SumAsync();

            var query3 = from p in _context.D1100PoInfos
                         select new { p };
            query3 = query3.Where(w => w.p.Date >= request.From && w.p.Date <= request.To && w.p.Ws != "");
            var pcList = query3.Select(s => float.Parse(s.p.Plantime) * 18000).ToList();
            var maximumProductionCapacity = 0.0f;
            foreach (float a in pcList)
            {
                maximumProductionCapacity += a;
            }

            var data = new PtMonthYearVM()
            {
                ActualWorkingTime = wtVM.TestingTime + wtVM.RunningTime + wtVM.OtherTime + wtVM.BreakTime + wtVM.FixingTime + wtVM.PendingTime + wtVM.MaintenanceTime + wtVM.PauseTime,
                Downtime = wtVM.TestingTime + wtVM.OtherTime + wtVM.BreakTime + wtVM.FixingTime + wtVM.PendingTime + wtVM.MaintenanceTime + wtVM.PauseTime,
                TotalOutput = (totalOutput == null) ? 0 : totalOutput,
                MaximumProductionCapacity = (maximumProductionCapacity == null) ? 0 : maximumProductionCapacity,
                Waste = (waste == null) ? 0 : waste,
            };

            return data;
        }
        #endregion
        #region Year
        public async Task<PtMonthYearVM> GetD650ProductionTracking_Year(TimeRequest request)
        {
            var query1 = from p in _context.TableDays
                         select new { p };
            query1 = query1.Where(w => w.p.Date >= request.From && w.p.Date <= request.To);

            if (!query1.Any())
            {
                return new PtMonthYearVM()
                {
                    ActualWorkingTime = 0,
                    Downtime = 0,
                    TotalOutput = 0,
                    MaximumProductionCapacity = 0,
                    Waste = 0,
                };
            }

            var wtVM = query1.Where(w => w.p.Machine == "D650")
            .GroupBy(g => g.p.Machine)
            .Select(s => new WorkTimeVM()
            {
                RunningTime = (s.Sum(a => a.p.RunningTime) == null) ? 0 : s.Sum(a => a.p.RunningTime),
                TestingTime = (s.Sum(a => a.p.TestingTime) == null) ? 0 : s.Sum(a => a.p.TestingTime),
                OtherTime = (s.Sum(a => a.p.OtherTime) == null) ? 0 : s.Sum(a => a.p.OtherTime),
                BreakTime = (s.Sum(a => a.p.BreakTime) == null) ? 0 : s.Sum(a => a.p.BreakTime),
                FixingTime = (s.Sum(a => a.p.FixingTime) == null) ? 0 : s.Sum(a => a.p.FixingTime),
                PendingTime = (s.Sum(a => a.p.PendingTime) == null) ? 0 : s.Sum(a => a.p.PendingTime),
                MaintenanceTime = (s.Sum(a => a.p.MaintenanceTime) == null) ? 0 : s.Sum(a => a.p.MaintenanceTime),
                PauseTime = (s.Sum(a => a.p.PauseTime) == null) ? 0 : s.Sum(a => a.p.PauseTime),
            }).FirstOrDefault();
            if (wtVM is null)
            {
                wtVM = new WorkTimeVM()
                {
                    RunningTime = 0,
                    TestingTime = 0,
                    OtherTime = 0,
                    BreakTime = 0,
                    FixingTime = 0,
                    PendingTime = 0,
                    MaintenanceTime = 0,
                    PauseTime = 0,
                };
            }

            var query2 = from p in _context.D650PoKpis
                         select new { p };
            query2 = query2.Where(w => w.p.Starttime >= request.From && w.p.Starttime <= request.To && w.p.Ws != "");
            var totalOutput = await query2.Select(s => s.p.Job).SumAsync();
            var waste = await query2.Select(s => s.p.DefeatItem1 + s.p.DefeatItem2 + s.p.DefeatItem3 + s.p.DefeatItem4).SumAsync();

            var query3 = from p in _context.D650PoInfos
                         select new { p };
            query3 = query3.Where(w => w.p.Date >= request.From && w.p.Date <= request.To && w.p.Ws != "");
            var pcList = query3.Select(s => float.Parse(s.p.Plantime) * 18000).ToList();
            var maximumProductionCapacity = 0.0f;
            foreach (float a in pcList)
            {
                maximumProductionCapacity += a;
            }

            var data = new PtMonthYearVM()
            {
                ActualWorkingTime = wtVM.TestingTime + wtVM.RunningTime + wtVM.OtherTime + wtVM.BreakTime + wtVM.FixingTime + wtVM.PendingTime + wtVM.MaintenanceTime + wtVM.PauseTime,
                Downtime = wtVM.TestingTime + wtVM.OtherTime + wtVM.BreakTime + wtVM.FixingTime + wtVM.PendingTime + wtVM.MaintenanceTime + wtVM.PauseTime,
                TotalOutput = (totalOutput == null) ? 0 : totalOutput,
                MaximumProductionCapacity = (maximumProductionCapacity == null) ? 0 : maximumProductionCapacity,
                Waste = (waste == null) ? 0 : waste,
            };

            return data;
        }
        public async Task<PtMonthYearVM> GetD750ProductionTracking_Year(TimeRequest request)
        {
            var query1 = from p in _context.TableDays
                         select new { p };
            query1 = query1.Where(w => w.p.Date >= request.From && w.p.Date <= request.To);

            if (!query1.Any())
            {
                return new PtMonthYearVM()
                {
                    ActualWorkingTime = 0,
                    Downtime = 0,
                    TotalOutput = 0,
                    MaximumProductionCapacity = 0,
                    Waste = 0,
                };
            }

            var wtVM = query1.Where(w => w.p.Machine == "D750")
            .GroupBy(g => g.p.Machine)
            .Select(s => new WorkTimeVM()
            {
                RunningTime = (s.Sum(a => a.p.RunningTime) == null) ? 0 : s.Sum(a => a.p.RunningTime),
                TestingTime = (s.Sum(a => a.p.TestingTime) == null) ? 0 : s.Sum(a => a.p.TestingTime),
                OtherTime = (s.Sum(a => a.p.OtherTime) == null) ? 0 : s.Sum(a => a.p.OtherTime),
                BreakTime = (s.Sum(a => a.p.BreakTime) == null) ? 0 : s.Sum(a => a.p.BreakTime),
                FixingTime = (s.Sum(a => a.p.FixingTime) == null) ? 0 : s.Sum(a => a.p.FixingTime),
                PendingTime = (s.Sum(a => a.p.PendingTime) == null) ? 0 : s.Sum(a => a.p.PendingTime),
                MaintenanceTime = (s.Sum(a => a.p.MaintenanceTime) == null) ? 0 : s.Sum(a => a.p.MaintenanceTime),
                PauseTime = (s.Sum(a => a.p.PauseTime) == null) ? 0 : s.Sum(a => a.p.PauseTime),
            }).FirstOrDefault();
            if (wtVM is null)
            {
                wtVM = new WorkTimeVM()
                {
                    RunningTime = 0,
                    TestingTime = 0,
                    OtherTime = 0,
                    BreakTime = 0,
                    FixingTime = 0,
                    PendingTime = 0,
                    MaintenanceTime = 0,
                    PauseTime = 0,
                };
            }

            var query2 = from p in _context.D750PoKpis
                         select new { p };
            query2 = query2.Where(w => w.p.Starttime >= request.From && w.p.Starttime <= request.To && w.p.Ws != "");
            var totalOutput = await query2.Select(s => s.p.Job).SumAsync();
            var waste = await query2.Select(s => s.p.DefeatItem1 + s.p.DefeatItem2 + s.p.DefeatItem3 + s.p.DefeatItem4).SumAsync();

            var query3 = from p in _context.D750PoInfos
                         select new { p };
            query3 = query3.Where(w => w.p.Date >= request.From && w.p.Date <= request.To && w.p.Ws != "");
            var pcList = query3.Select(s => float.Parse(s.p.Plantime) * 18000).ToList();
            var maximumProductionCapacity = 0.0f;
            foreach (float a in pcList)
            {
                maximumProductionCapacity += a;
            }

            var data = new PtMonthYearVM()
            {
                ActualWorkingTime = wtVM.TestingTime + wtVM.RunningTime + wtVM.OtherTime + wtVM.BreakTime + wtVM.FixingTime + wtVM.PendingTime + wtVM.MaintenanceTime + wtVM.PauseTime,
                Downtime = wtVM.TestingTime + wtVM.OtherTime + wtVM.BreakTime + wtVM.FixingTime + wtVM.PendingTime + wtVM.MaintenanceTime + wtVM.PauseTime,
                TotalOutput = (totalOutput == null) ? 0 : totalOutput,
                MaximumProductionCapacity = (maximumProductionCapacity == null) ? 0 : maximumProductionCapacity,
                Waste = (waste == null) ? 0 : waste,
            };

            return data;
        }
        public async Task<PtMonthYearVM> GetD1000ProductionTracking_Year(TimeRequest request)
        {
            var query1 = from p in _context.TableDays
                         select new { p };
            query1 = query1.Where(w => w.p.Date >= request.From && w.p.Date <= request.To);

            if (!query1.Any())
            {
                return new PtMonthYearVM()
                {
                    ActualWorkingTime = 0,
                    Downtime = 0,
                    TotalOutput = 0,
                    MaximumProductionCapacity = 0,
                    Waste = 0,
                };
            }

            var wtVM = query1.Where(w => w.p.Machine == "D1000")
            .GroupBy(g => g.p.Machine)
            .Select(s => new WorkTimeVM()
            {
                RunningTime = (s.Sum(a => a.p.RunningTime) == null) ? 0 : s.Sum(a => a.p.RunningTime),
                TestingTime = (s.Sum(a => a.p.TestingTime) == null) ? 0 : s.Sum(a => a.p.TestingTime),
                OtherTime = (s.Sum(a => a.p.OtherTime) == null) ? 0 : s.Sum(a => a.p.OtherTime),
                BreakTime = (s.Sum(a => a.p.BreakTime) == null) ? 0 : s.Sum(a => a.p.BreakTime),
                FixingTime = (s.Sum(a => a.p.FixingTime) == null) ? 0 : s.Sum(a => a.p.FixingTime),
                PendingTime = (s.Sum(a => a.p.PendingTime) == null) ? 0 : s.Sum(a => a.p.PendingTime),
                MaintenanceTime = (s.Sum(a => a.p.MaintenanceTime) == null) ? 0 : s.Sum(a => a.p.MaintenanceTime),
                PauseTime = (s.Sum(a => a.p.PauseTime) == null) ? 0 : s.Sum(a => a.p.PauseTime),
            }).FirstOrDefault();
            if (wtVM is null)
            {
                wtVM = new WorkTimeVM()
                {
                    RunningTime = 0,
                    TestingTime = 0,
                    OtherTime = 0,
                    BreakTime = 0,
                    FixingTime = 0,
                    PendingTime = 0,
                    MaintenanceTime = 0,
                    PauseTime = 0,
                };
            }

            var query2 = from p in _context.D1000PoKpis
                         select new { p };
            query2 = query2.Where(w => w.p.Starttime >= request.From && w.p.Starttime <= request.To && w.p.Ws != "");
            var totalOutput = await query2.Select(s => s.p.Job).SumAsync();
            var waste = await query2.Select(s => s.p.DefeatItem1 + s.p.DefeatItem2 + s.p.DefeatItem3 + s.p.DefeatItem4).SumAsync();

            var query3 = from p in _context.D1000PoInfos
                         select new { p };
            query3 = query3.Where(w => w.p.Date >= request.From && w.p.Date <= request.To && w.p.Ws != "");
            var pcList = query3.Select(s => float.Parse(s.p.Plantime) * 18000).ToList();
            var maximumProductionCapacity = 0.0f;
            foreach (float a in pcList)
            {
                maximumProductionCapacity += a;
            }

            var data = new PtMonthYearVM()
            {
                ActualWorkingTime = wtVM.TestingTime + wtVM.RunningTime + wtVM.OtherTime + wtVM.BreakTime + wtVM.FixingTime + wtVM.PendingTime + wtVM.MaintenanceTime + wtVM.PauseTime,
                Downtime = wtVM.TestingTime + wtVM.OtherTime + wtVM.BreakTime + wtVM.FixingTime + wtVM.PendingTime + wtVM.MaintenanceTime + wtVM.PauseTime,
                TotalOutput = (totalOutput == null) ? 0 : totalOutput,
                MaximumProductionCapacity = (maximumProductionCapacity == null) ? 0 : maximumProductionCapacity,
                Waste = (waste == null) ? 0 : waste,
            };

            return data;
        }
        public async Task<PtMonthYearVM> GetD1100ProductionTracking_Year(TimeRequest request)
        {
            var query1 = from p in _context.TableDays
                         select new { p };
            query1 = query1.Where(w => w.p.Date >= request.From && w.p.Date <= request.To);

            if (!query1.Any())
            {
                return new PtMonthYearVM()
                {
                    ActualWorkingTime = 0,
                    Downtime = 0,
                    TotalOutput = 0,
                    MaximumProductionCapacity = 0,
                    Waste = 0,
                };
            }

            var wtVM = query1.Where(w => w.p.Machine == "D1100")
            .GroupBy(g => g.p.Machine)
            .Select(s => new WorkTimeVM()
            {
                RunningTime = (s.Sum(a => a.p.RunningTime) == null) ? 0 : s.Sum(a => a.p.RunningTime),
                TestingTime = (s.Sum(a => a.p.TestingTime) == null) ? 0 : s.Sum(a => a.p.TestingTime),
                OtherTime = (s.Sum(a => a.p.OtherTime) == null) ? 0 : s.Sum(a => a.p.OtherTime),
                BreakTime = (s.Sum(a => a.p.BreakTime) == null) ? 0 : s.Sum(a => a.p.BreakTime),
                FixingTime = (s.Sum(a => a.p.FixingTime) == null) ? 0 : s.Sum(a => a.p.FixingTime),
                PendingTime = (s.Sum(a => a.p.PendingTime) == null) ? 0 : s.Sum(a => a.p.PendingTime),
                MaintenanceTime = (s.Sum(a => a.p.MaintenanceTime) == null) ? 0 : s.Sum(a => a.p.MaintenanceTime),
                PauseTime = (s.Sum(a => a.p.PauseTime) == null) ? 0 : s.Sum(a => a.p.PauseTime),
            }).FirstOrDefault();
            if (wtVM is null)
            {
                wtVM = new WorkTimeVM()
                {
                    RunningTime = 0,
                    TestingTime = 0,
                    OtherTime = 0,
                    BreakTime = 0,
                    FixingTime = 0,
                    PendingTime = 0,
                    MaintenanceTime = 0,
                    PauseTime = 0,
                };
            }

            var query2 = from p in _context.D1100PoKpis
                         select new { p };
            query2 = query2.Where(w => w.p.Starttime >= request.From && w.p.Starttime <= request.To && w.p.Ws != "");
            var totalOutput = await query2.Select(s => s.p.Job).SumAsync();
            var waste = await query2.Select(s => s.p.DefeatItem1 + s.p.DefeatItem2 + s.p.DefeatItem3 + s.p.DefeatItem4).SumAsync();

            var query3 = from p in _context.D1100PoInfos
                         select new { p };
            query3 = query3.Where(w => w.p.Date >= request.From && w.p.Date <= request.To && w.p.Ws != "");
            var pcList = query3.Select(s => float.Parse(s.p.Plantime) * 18000).ToList();
            var maximumProductionCapacity = 0.0f;
            foreach (float a in pcList)
            {
                maximumProductionCapacity += a;
            }

            var data = new PtMonthYearVM()
            {
                ActualWorkingTime = wtVM.TestingTime + wtVM.RunningTime + wtVM.OtherTime + wtVM.BreakTime + wtVM.FixingTime + wtVM.PendingTime + wtVM.MaintenanceTime + wtVM.PauseTime,
                Downtime = wtVM.TestingTime + wtVM.OtherTime + wtVM.BreakTime + wtVM.FixingTime + wtVM.PendingTime + wtVM.MaintenanceTime + wtVM.PauseTime,
                TotalOutput = (totalOutput == null) ? 0 : totalOutput,
                MaximumProductionCapacity = (maximumProductionCapacity == null) ? 0 : maximumProductionCapacity,
                Waste = (waste == null) ? 0 : waste,
            };

            return data;
        }
        #endregion
        #endregion
    }
}
