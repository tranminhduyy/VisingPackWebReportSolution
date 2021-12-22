using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisingPackSolution.Data.EF;
using VisingPackSolution.ViewModels.Common;
using VisingPackSolution.ViewModels.MachineState;
using VisingPackSolution.ViewModels.WorkTimeManage;

namespace VisingPackSolution.Application.WorkTimeManage
{
    public class WorkTimeManageService : IWorkTimeManageService
    {
        private readonly VisingPackMMSDbContext _context;
        public WorkTimeManageService(VisingPackMMSDbContext context)
        {
            _context = context;
        }

        public async Task<WtPrintingVM> GetPrintingWorkTimeMgt(TimeRequest request)
        {
            var query = from p in _context.TableDays
                        select new { p };
            query = query.Where(w => w.p.Date >= request.From.Date && w.p.Date <= request.To.Date);
            if (!query.Any())
            {
                var empty = new WorkTimeVM() { RunningTime = 0, TestingTime = 0, OtherTime = 0, BreakTime = 0, FixingTime = 0, PendingTime = 0, MaintenanceTime = 0, PauseTime = 0 };
                return new WtPrintingVM()
                {
                    P601 = empty, P604 = empty, P605 = empty, P5M = empty,
                };
            }

            var nullWorktime = new WorkTimeVM()
            {
                RunningTime = 0,TestingTime = 0,OtherTime = 0,BreakTime = 0,FixingTime = 0,PendingTime = 0,MaintenanceTime = 0,PauseTime = 0,
            };

            var p601 = query.Where(w => w.p.Machine == "P601")
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
            p601 = (p601 is null) ? nullWorktime : p601;

            var p604 = query.Where(w => w.p.Machine == "P604")
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
            p604 = (p604 is null) ? nullWorktime : p604;

            var p605 = query.Where(w => w.p.Machine == "P605")
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
            p605 = (p605 is null) ? nullWorktime : p605;

            var p5m = query.Where(w => w.p.Machine == "P5M")
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
            p5m = (p5m is null) ? nullWorktime : p5m;

            var result = new WtPrintingVM()
            {
                P601 = p601,
                P604 = p604,
                P605 = p605,
                P5M = p5m,
            };
            return result;
        }

        public async Task<WtDieCutVM> GetDieCutWorkTimeMgt(TimeRequest request)
        {
            var query = from p in _context.TableDays
                        select new { p };
            query = query.Where(w => w.p.Date >= request.From.Date && w.p.Date <= request.To.Date);
            if (!query.Any())
            {
                var empty = new WorkTimeVM() { RunningTime = 0, TestingTime = 0, OtherTime = 0, BreakTime = 0, FixingTime = 0, PendingTime = 0, MaintenanceTime = 0, PauseTime = 0 };
                return new WtDieCutVM()
                {
                    BTD2 = empty,
                    BTD3 = empty,
                    BTD4 = empty,
                    BTD5 = empty,
                };
            }

            var nullWorktime = new WorkTimeVM()
            {
                RunningTime = 0,TestingTime = 0,OtherTime = 0,BreakTime = 0,FixingTime = 0,PendingTime = 0,MaintenanceTime = 0,PauseTime = 0,
            };

            var btd2 = query.Where(w => w.p.Machine == "BTD2")
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
            btd2 = (btd2 is null) ? nullWorktime : btd2;

            var btd3 = query.Where(w => w.p.Machine == "BTD3")
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
            btd3 = (btd3 is null) ? nullWorktime : btd3;

            var btd4 = query.Where(w => w.p.Machine == "BTD4")
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
            btd4 = (btd4 is null) ? nullWorktime : btd4;

            var btd5 = query.Where(w => w.p.Machine == "BTD5")
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
            btd5 = (btd5 is null) ? nullWorktime : btd5;

            var result = new WtDieCutVM()
            {
                BTD2 = btd2,
                BTD3 = btd3,
                BTD4 = btd4,
                BTD5 = btd5,
            };
            return result;
        }

        public async Task<WtGluingVM> GetGluingWorkTimeMgt(TimeRequest request)
        {
            var query = from p in _context.TableDays
                        select new { p };
            query = query.Where(w => w.p.Date >= request.From.Date && w.p.Date <= request.To.Date);
            if (!query.Any())
            {
                var empty = new WorkTimeVM() { RunningTime = 0, TestingTime = 0, OtherTime = 0, BreakTime = 0, FixingTime = 0, PendingTime = 0, MaintenanceTime = 0, PauseTime = 0 };
                return new WtGluingVM()
                {
                    D650 = empty,
                    D750 = empty,
                    D1000 = empty,
                    D1100 = empty,
                };
            }

            var nullWorktime = new WorkTimeVM()
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

            var d650 = query.Where(w => w.p.Machine == "D650")
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
            d650 = (d650 is null) ? nullWorktime : d650;

            var d750 = query.Where(w => w.p.Machine == "D750")
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
            d750 = (d750 is null) ? nullWorktime : d750;

            var d1000 = query.Where(w => w.p.Machine == "D1000")
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
            d1000 = (d1000 is null) ? nullWorktime : d1000;

            var d1100 = query.Where(w => w.p.Machine == "D1100")
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
            d1100 = (d1100 is null) ? nullWorktime : d1100;

            var result = new WtGluingVM()
            {
                D650 = d650,
                D750 = d750,
                D1000 = d1000,
                D1100 = d1100,
            };
            return result;
        }

        public async Task<WtSclGmcVM> GetSclGmcWorkTimeMgt(TimeRequest request)
        {
            var query = from p in _context.TableDays
                        select new { p };
            query = query.Where(w => w.p.Date >= request.From.Date && w.p.Date <= request.To.Date);
            if (!query.Any())
            {
                var empty = new WorkTimeVM() { RunningTime = 0, TestingTime = 0, OtherTime = 0, BreakTime = 0, FixingTime = 0, PendingTime = 0, MaintenanceTime = 0, PauseTime = 0 };
                return new WtSclGmcVM()
                {
                    SCL = empty,
                    GMC1 = empty,
                    GMC2 = empty,
                };
            }

            var nullWorktime = new WorkTimeVM()
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

            var scl = query.Where(w => w.p.Machine == "SCL")
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
            scl = (scl is null) ? nullWorktime : scl;

            var gmc1 = query.Where(w => w.p.Machine == "GMC1")
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
            gmc1 = (gmc1 is null) ? nullWorktime : gmc1;

            var gmc2 = query.Where(w => w.p.Machine == "GMC2")
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
            gmc2 = (gmc2 is null) ? nullWorktime : gmc2;

            var result = new WtSclGmcVM()
            {
                SCL = scl,
                GMC1 = gmc1,
                GMC2 = gmc2,
            };
            return result;
        }
    }
}
