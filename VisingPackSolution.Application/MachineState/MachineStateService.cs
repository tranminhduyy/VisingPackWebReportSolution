using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VisingPackSolution.Data;
using VisingPackSolution.Data.EF;
using VisingPackSolution.Utilities.Exceptions;
using System.Linq;
using VisingPackSolution.ViewModels.Common;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using System.IO;
using VisingPackSolution.Application.Common;
using Microsoft.EntityFrameworkCore;
using VisingPackSolution.Data.Speed.EF;
using VisingPackSolution.ViewModels.MachineState;

namespace VisingPackSolution.Application.MachineState
{
    public class MachineStateService : IMachineStateService
    {
        private readonly VisingPackMMSDbContext _context;
        private readonly GroupHistoryDBContext _ghContext;
        private readonly IStorageService _storageService;
        //private const string USER_CONTENT_FOLDER_NAME = "user-content";
        public MachineStateService(VisingPackMMSDbContext context, GroupHistoryDBContext ghContext, IStorageService storageService)
        {
            _context = context;
            _ghContext = ghContext;
            _storageService = storageService;
        }

        public async Task<int> Delete(int P601EventId)
        {
            var p601Event = await _context.P601PoEvents.FindAsync(P601EventId);
            if (p601Event == null) throw new VisingPackException($"Can not find P601 Event: {P601EventId}");

            _context.P601PoEvents.Remove(p601Event);
            return await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<MsEventVM>> GetAll()
        {
            var result = _context.P601PoEvents.Select(x => new MsEventVM()
            {
                //UID = x.Uid,
                //WS = x.Ws,
                Datetime = x.Datetime,
                Event = x.Event,
            });

            return await result.ToListAsync();
        }

        public async Task<PagedResult<MsEventVM>> GetAllPaging(GetP601EventPagingRequest request)
        {
            // 1.Select
            var query = from p in _context.P601PoEvents
                        select new { p };
            //2.Filter
            if (!string.IsNullOrEmpty(request.Keyword))
                query = query.Where(x => x.p.Ws.Contains(request.Keyword));

            if (request.Starttime != null)
                query = query.Where(q => q.p.Datetime >= request.Starttime);

            query = query.OrderByDescending(z => z.p.Datetime);
            //3. Paging
            int totalRow = await query.CountAsync();

            var data = await query.Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new MsEventVM()
                {
                    //UID = x.p.Uid,
                    //WS = x.p.Ws,
                    Datetime = x.p.Datetime,
                    Event = x.p.Event,
                }).ToListAsync();
            var pagedResult = new PagedResult<MsEventVM>()
            {
                TotalRecords = totalRow,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                Items = data,
            };
            return pagedResult;
        }

        public async Task<int> Update(P601EventUpdateRequest request)
        {
            var p601Event = await _context.P601PoEvents.FirstOrDefaultAsync(x => x.Uid == request.Uid);
            if (p601Event == null) throw new VisingPackException($"Can not find a Uid number: {request.Uid}");

            p601Event.Event = request.Event;
            p601Event.Ws = request.Ws;
            p601Event.Datetime = DateTime.Now;
            return await _context.SaveChangesAsync();
        }

        private async Task<string> SaveFile(IFormFile file)
        {
            var originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(originalFileName)}";
            await _storageService.SaveFileAsync(file.OpenReadStream(), fileName);
            return fileName;
        }

        public async Task<MsEventVM> GetById(int id)
        {
            var _event = await _context.P601PoEvents.FindAsync(id);

            var p601EventViewModel = new MsEventVM()
            {
                //UID = _event.Uid,
                //WS = _event.Ws,
                Datetime = _event.Datetime,
                Event = _event.Event,
            };

            return p601EventViewModel;
        }

        public async Task<int> Create(P601EventCreateRequest request)
        {
            var p601Event = new P601PoEvent()
            {
                Uid = request.UID,
                Ws = request.WS,
                Datetime = DateTime.Now,
                Event = request.Event,
            };
            _context.P601PoEvents.Add(p601Event);
            await _context.SaveChangesAsync();
            return p601Event.Uid;
        }

        public async Task<PagedResult<MsEventVM>> GetAllByStarttime(GetP601EventPagingRequest request)
        {
            var result = _context.P601PoEvents.Select(x => new MsEventVM()
            {
                //UID = x.Uid,
                //WS = x.Ws,
                Datetime = x.Datetime,
                Event = x.Event,
            }).Where(y => y.Datetime >= request.Starttime);

            var totalRow = result.Count();

            var pagedResult = new PagedResult<MsEventVM>()
            {
                TotalRecords = totalRow,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                Items = result.ToList(),
            };
            return pagedResult;
        }

        #region Printing
        public async Task<List<MsEventVM>> GetP601EventsByTime(GetMsByTimeRequest request)
        {
            var query = from p in _context.P601PoEvents
                        select new { p };

            if (request.From != null && request.To != null)
                query = query.Where(q => q.p.Datetime >= request.From && q.p.Datetime <= request.To);

            var data = await query.OrderByDescending(z => z.p.Datetime)
                .Select(x => new MsEventVM()
                {
                    Datetime = x.p.Datetime,
                    Event = x.p.Event
                }).ToListAsync();

            return data;
        }
        public async Task<List<MsEventVM>> GetP604EventsByTime(GetMsByTimeRequest request)
        {
            var query = from p in _context.P604PoEvents
                        select new { p };

            TimeSpan interval = new TimeSpan(0, 15, 0);
            if (request.From != null && request.To != null)
                query = query.Where(q => q.p.Datetime >= request.From && q.p.Datetime <= request.To);

            //query = query.GroupBy(x=> x.p.Datetime.tick)
            var data = await query.OrderByDescending(z => z.p.Datetime)
                .Select(x => new MsEventVM()
                {
                    Datetime = x.p.Datetime,
                    Event = x.p.Event
                }).ToListAsync();

            return data;
        }
        public async Task<List<MsEventVM>> GetP605EventsByTime(GetMsByTimeRequest request)
        {
            var query = from p in _context.P605PoEvents
                        select new { p };

            if (request.From != null && request.To != null)
                query = query.Where(q => q.p.Datetime >= request.From && q.p.Datetime <= request.To);

            var data = await query.OrderByDescending(z => z.p.Datetime)
                .Select(x => new MsEventVM()
                {
                    Datetime = x.p.Datetime,
                    Event = x.p.Event
                }).ToListAsync();

            return data;
        }
        public async Task<List<MsEventVM>> GetP5MEventsByTime(GetMsByTimeRequest request)
        {
            var query = from p in _context.P5mPoEvents
                        select new { p };

            if (request.From != null && request.To != null)
                query = query.Where(q => q.p.Datetime >= request.From && q.p.Datetime <= request.To);

            var data = await query.OrderByDescending(z => z.p.Datetime)
                .Select(x => new MsEventVM()
                {
                    Datetime = x.p.Datetime,
                    Event = x.p.Event
                }).ToListAsync();

            return data;
        }
        public async Task<List<MsSpeedVM>> GetP601SpeedsByTime(GetMsByTimeRequest request)
        {
            var query = from p in _ghContext.DivP601SpeedHistrecords
                        select new { p };
            var results = new List<MsSpeedVM>();
            switch (request.Selected)
            {
                case "Time":
                    results = await query.GroupBy(i => i.p.TriggerTime.Date)
                          .Select(g => new MsSpeedVM() { Datetime = g.Key, Speed = g.Average(a => a.p.ColaHistVariable) })
                          .Where(w => w.Datetime >= request.From && w.Datetime <= request.To)
                          .OrderBy(o => o.Datetime)
                          .ToListAsync();
                    return results;

                case "Hour":
                    if (request.From != null && request.To != null)
                        query = query.Where(q => q.p.TriggerTime >= request.From && q.p.TriggerTime <= request.To);

                    results = await query
                        .Select(x => new MsSpeedVM()
                        {
                            Datetime = x.p.TriggerTime,
                            Speed = x.p.ColaHistVariable,
                        })
                        .OrderBy(z => z.Datetime)
                        .ToListAsync();
                    return results;

                case "Day":
                    if (request.From != null && request.To != null)
                        query = query.Where(q => q.p.TriggerTime >= request.From && q.p.TriggerTime <= request.To);

                    results = await query
                        .Select(x => new MsSpeedVM()
                        {
                            Datetime = x.p.TriggerTime,
                            Speed = x.p.ColaHistVariable,
                        })
                        .OrderBy(z => z.Datetime)
                        .ToListAsync();
                    return results;

                case "Week":
                    results = await query.GroupBy(i => i.p.TriggerTime.Date)
                          .Select(g => new MsSpeedVM() { Datetime = g.Key, Speed = g.Average(a => a.p.ColaHistVariable) })
                          .Where(w => w.Datetime >= request.From && w.Datetime <= request.To)
                          .OrderBy(o => o.Datetime)
                          .ToListAsync();
                    return results;

                case "Month":
                    results = await query.GroupBy(i => i.p.TriggerTime.Date)
                          .Select(g => new MsSpeedVM() { Datetime = g.Key, Speed = g.Average(a => a.p.ColaHistVariable) })
                          .Where(w => w.Datetime >= request.From && w.Datetime <= request.To)
                          .OrderBy(o => o.Datetime)
                          .ToListAsync();
                    return results;

                case "Year":
                    results = await query.GroupBy(i => i.p.TriggerTime.Date)
                          .Select(g => new MsSpeedVM() { Datetime = g.Key, Speed = g.Average(a => a.p.ColaHistVariable) })
                          .Where(w => w.Datetime >= request.From && w.Datetime <= request.To)
                          .OrderBy(o => o.Datetime)
                          .ToListAsync();
                    return results;
            }
            return null;
        }
        public async Task<List<MsSpeedVM>> GetP604SpeedsByTime(GetMsByTimeRequest request)
        {
            var query = from p in _ghContext.DivP604SpeedHistrecords
                        select new { p };
            var results = new List<MsSpeedVM>();
            switch (request.Selected)
            {
                case "Time":
                    results = await query.GroupBy(i => i.p.TriggerTime.Date)
                          .Select(g => new MsSpeedVM() { Datetime = g.Key, Speed = g.Average(a => a.p.ColaHistVariable) })
                          .Where(w => w.Datetime >= request.From && w.Datetime <= request.To)
                          .OrderBy(o => o.Datetime)
                          .ToListAsync();
                    return results;

                case "Hour":
                    if (request.From != null && request.To != null)
                        query = query.Where(q => q.p.TriggerTime >= request.From && q.p.TriggerTime <= request.To);

                    results = await query.OrderBy(z => z.p.TriggerTime)
                        .Select(x => new MsSpeedVM()
                        {
                            Datetime = x.p.TriggerTime,
                            Speed = x.p.ColaHistVariable,
                        }).ToListAsync();
                    return results;

                case "Day":
                    if (request.From != null && request.To != null)
                        query = query.Where(q => q.p.TriggerTime >= request.From && q.p.TriggerTime <= request.To);

                    results = await query.OrderBy(z => z.p.TriggerTime)
                        .Select(x => new MsSpeedVM()
                        {
                            Datetime = x.p.TriggerTime,
                            Speed = x.p.ColaHistVariable,
                        }).ToListAsync();
                    return results;

                case "Week":
                    results = await query.GroupBy(i => i.p.TriggerTime.Date)
                          .Select(g => new MsSpeedVM() { Datetime = g.Key, Speed = g.Average(a => a.p.ColaHistVariable) })
                          .Where(w => w.Datetime >= request.From && w.Datetime <= request.To)
                          .OrderBy(o => o.Datetime)
                          .ToListAsync();
                    return results;

                case "Month":
                    results = await query.GroupBy(i => i.p.TriggerTime.Date)
                          .Select(g => new MsSpeedVM() { Datetime = g.Key, Speed = g.Average(a => a.p.ColaHistVariable) })
                          .Where(w => w.Datetime >= request.From && w.Datetime <= request.To)
                          .OrderBy(o => o.Datetime)
                          .ToListAsync();
                    return results;

                case "Year":
                    results = await query.GroupBy(i => i.p.TriggerTime.Date)
                          .Select(g => new MsSpeedVM() { Datetime = g.Key, Speed = g.Average(a => a.p.ColaHistVariable) })
                          .Where(w => w.Datetime >= request.From && w.Datetime <= request.To)
                          .OrderBy(o => o.Datetime)
                          .ToListAsync();
                    return results;
            }
            return null;
        }
        public async Task<List<MsSpeedVM>> GetP605SpeedsByTime(GetMsByTimeRequest request)
        {
            var query = from p in _ghContext.DivP605SpeedHistrecords
                        select new { p };
            var results = new List<MsSpeedVM>();
            switch (request.Selected)
            {
                case "Time":
                    results = await query.GroupBy(i => i.p.TriggerTime.Date)
                          .Select(g => new MsSpeedVM() { Datetime = g.Key, Speed = g.Average(a => a.p.ColaHistVariable) })
                          .Where(w => w.Datetime >= request.From && w.Datetime <= request.To)
                          .OrderBy(o => o.Datetime)
                          .ToListAsync();
                    return results;

                case "Hour":
                    if (request.From != null && request.To != null)
                        query = query.Where(q => q.p.TriggerTime >= request.From && q.p.TriggerTime <= request.To);

                    results = await query.OrderBy(z => z.p.TriggerTime)
                        .Select(x => new MsSpeedVM()
                        {
                            Datetime = x.p.TriggerTime,
                            Speed = x.p.ColaHistVariable,
                        }).ToListAsync();
                    return results;

                case "Day":
                    if (request.From != null && request.To != null)
                        query = query.Where(q => q.p.TriggerTime >= request.From && q.p.TriggerTime <= request.To);

                    results = await query.OrderBy(z => z.p.TriggerTime)
                        .Select(x => new MsSpeedVM()
                        {
                            Datetime = x.p.TriggerTime,
                            Speed = x.p.ColaHistVariable,
                        }).ToListAsync();
                    return results;

                case "Week":
                    results = await query.GroupBy(i => i.p.TriggerTime.Date)
                          .Select(g => new MsSpeedVM() { Datetime = g.Key, Speed = g.Average(a => a.p.ColaHistVariable) })
                          .Where(w => w.Datetime >= request.From && w.Datetime <= request.To)
                          .OrderBy(o => o.Datetime)
                          .ToListAsync();
                    return results;

                case "Month":
                    results = await query.GroupBy(i => i.p.TriggerTime.Date)
                          .Select(g => new MsSpeedVM() { Datetime = g.Key, Speed = g.Average(a => a.p.ColaHistVariable) })
                          .Where(w => w.Datetime >= request.From && w.Datetime <= request.To)
                          .OrderBy(o => o.Datetime)
                          .ToListAsync();
                    return results;

                case "Year":
                    results = await query.GroupBy(i => i.p.TriggerTime.Date)
                          .Select(g => new MsSpeedVM() { Datetime = g.Key, Speed = g.Average(a => a.p.ColaHistVariable) })
                          .Where(w => w.Datetime >= request.From && w.Datetime <= request.To)
                          .OrderBy(o => o.Datetime)
                          .ToListAsync();
                    return results;
            }
            return null;
        }
        public async Task<List<MsSpeedVM>> GetP5MSpeedsByTime(GetMsByTimeRequest request)
        {
            var query = from p in _ghContext.DivP5mSpeedHistrecords
                        select new { p };
            var results = new List<MsSpeedVM>();
            switch (request.Selected)
            {
                case "Time":
                    results = await query.GroupBy(i => i.p.TriggerTime.Date)
                          .Select(g => new MsSpeedVM() { Datetime = g.Key, Speed = g.Average(a => a.p.ColaHistVariable) })
                          .Where(w => w.Datetime >= request.From && w.Datetime <= request.To)
                          .OrderBy(o => o.Datetime)
                          .ToListAsync();
                    return results;

                case "Hour":
                    if (request.From != null && request.To != null)
                        query = query.Where(q => q.p.TriggerTime >= request.From && q.p.TriggerTime <= request.To);

                    results = await query.OrderBy(z => z.p.TriggerTime)
                        .Select(x => new MsSpeedVM()
                        {
                            Datetime = x.p.TriggerTime,
                            Speed = x.p.ColaHistVariable,
                        }).ToListAsync();
                    return results;

                case "Day":
                    if (request.From != null && request.To != null)
                        query = query.Where(q => q.p.TriggerTime >= request.From && q.p.TriggerTime <= request.To);

                    results = await query.OrderBy(z => z.p.TriggerTime)
                        .Select(x => new MsSpeedVM()
                        {
                            Datetime = x.p.TriggerTime,
                            Speed = x.p.ColaHistVariable,
                        }).ToListAsync();
                    return results;

                case "Week":
                    results = await query.GroupBy(i => i.p.TriggerTime.Date)
                          .Select(g => new MsSpeedVM() { Datetime = g.Key, Speed = g.Average(a => a.p.ColaHistVariable) })
                          .Where(w => w.Datetime >= request.From && w.Datetime <= request.To)
                          .OrderBy(o => o.Datetime)
                          .ToListAsync();
                    return results;

                case "Month":
                    results = await query.GroupBy(i => i.p.TriggerTime.Date)
                          .Select(g => new MsSpeedVM() { Datetime = g.Key, Speed = g.Average(a => a.p.ColaHistVariable) })
                          .Where(w => w.Datetime >= request.From && w.Datetime <= request.To)
                          .OrderBy(o => o.Datetime)
                          .ToListAsync();
                    return results;

                case "Year":
                    results = await query.GroupBy(i => i.p.TriggerTime.Date)
                          .Select(g => new MsSpeedVM() { Datetime = g.Key, Speed = g.Average(a => a.p.ColaHistVariable) })
                          .Where(w => w.Datetime >= request.From && w.Datetime <= request.To)
                          .OrderBy(o => o.Datetime)
                          .ToListAsync();
                    return results;
            }
            return null;
        }
        public async Task<int> GetP601ProductCodeChangeByTime(GetMsByTimeRequest request)
        {
            var query = from p in _context.P601PoKpis
                        select new { p };

            if (request.From != null && request.To != null)
                query = query.Where(q => q.p.Endtime >= request.From && q.p.Endtime <= request.To);

            var data = await query.CountAsync();
            return data;
        }
        public async Task<int> GetP604ProductCodeChangeByTime(GetMsByTimeRequest request)
        {
            var query = from p in _context.P604PoKpis
                        select new { p };

            if (request.From != null && request.To != null)
                query = query.Where(q => q.p.Endtime >= request.From && q.p.Endtime <= request.To);

            var data = await query.CountAsync();
            return data;
        }
        public async Task<int> GetP605ProductCodeChangeByTime(GetMsByTimeRequest request)
        {
            var query = from p in _context.P605PoKpis
                        select new { p };

            if (request.From != null && request.To != null)
                query = query.Where(q => q.p.Endtime >= request.From && q.p.Endtime <= request.To);

            var data = await query.CountAsync();
            return data;
        }
        public async Task<int> GetP5MProductCodeChangeByTime(GetMsByTimeRequest request)
        {
            var query = from p in _context.P5mPoKpis
                        select new { p };

            if (request.From != null && request.To != null)
                query = query.Where(q => q.p.Endtime >= request.From && q.p.Endtime <= request.To);

            var data = await query.CountAsync();
            return data;
        }
        public async Task<int?> GetP601JobCountByTime(GetMsByTimeRequest request)
        {
            var query = from p in _context.P601PoKpis
                        select new { p };

            if (request.From != null && request.To != null)
                query = query.Where(q => q.p.Endtime >= request.From && q.p.Endtime <= request.To);

            var data = await query.SumAsync(c => c.p.Job);
            return data;
        }
        public async Task<int?> GetP604JobCountByTime(GetMsByTimeRequest request)
        {
            var query = from p in _context.P604PoKpis
                        select new { p };

            if (request.From != null && request.To != null)
                query = query.Where(q => q.p.Endtime >= request.From && q.p.Endtime <= request.To);

            var data = await query.SumAsync(c => c.p.Job);
            return data;
        }
        public async Task<int?> GetP605JobCountByTime(GetMsByTimeRequest request)
        {
            var query = from p in _context.P605PoKpis
                        select new { p };

            if (request.From != null && request.To != null)
                query = query.Where(q => q.p.Endtime >= request.From && q.p.Endtime <= request.To);

            var data = await query.SumAsync(c => c.p.Job);
            return data;
        }
        public async Task<int?> GetP5MJobCountByTime(GetMsByTimeRequest request)
        {
            var query = from p in _context.P5mPoKpis
                        select new { p };

            if (request.From != null && request.To != null)
                query = query.Where(q => q.p.Endtime >= request.From && q.p.Endtime <= request.To);

            var data = await query.SumAsync(c => c.p.Job);
            return data;
        }
        #endregion
        #region Die-cut
        public async Task<List<MsEventVM>> GetBTD2EventsByTime(GetMsByTimeRequest request)
        {
            var query = from p in _context.Btd2PoEvents
                        select new { p };

            if (request.From != null && request.To != null)
                query = query.Where(q => q.p.Datetime >= request.From && q.p.Datetime <= request.To);

            var data = await query.OrderByDescending(z => z.p.Datetime)
                .Select(x => new MsEventVM()
                {
                    Datetime = x.p.Datetime,
                    Event = x.p.Event
                }).ToListAsync();

            return data;
        }
        public async Task<List<MsEventVM>> GetBTD3EventsByTime(GetMsByTimeRequest request)
        {
            var query = from p in _context.Btd3PoEvents
                        select new { p };

            TimeSpan interval = new TimeSpan(0, 15, 0);
            if (request.From != null && request.To != null)
                query = query.Where(q => q.p.Datetime >= request.From && q.p.Datetime <= request.To);

            //query = query.GroupBy(x=> x.p.Datetime.tick)
            var data = await query.OrderByDescending(z => z.p.Datetime)
                .Select(x => new MsEventVM()
                {
                    Datetime = x.p.Datetime,
                    Event = x.p.Event
                }).ToListAsync();

            return data;
        }
        public async Task<List<MsEventVM>> GetBTD4EventsByTime(GetMsByTimeRequest request)
        {
            var query = from p in _context.Btd4PoEvents
                        select new { p };

            if (request.From != null && request.To != null)
                query = query.Where(q => q.p.Datetime >= request.From && q.p.Datetime <= request.To);

            var data = await query.OrderByDescending(z => z.p.Datetime)
                .Select(x => new MsEventVM()
                {
                    Datetime = x.p.Datetime,
                    Event = x.p.Event
                }).ToListAsync();

            return data;
        }
        public async Task<List<MsEventVM>> GetBTD5EventsByTime(GetMsByTimeRequest request)
        {
            var query = from p in _context.Btd5PoEvents
                        select new { p };

            if (request.From != null && request.To != null)
                query = query.Where(q => q.p.Datetime >= request.From && q.p.Datetime <= request.To);

            var data = await query.OrderByDescending(z => z.p.Datetime)
                .Select(x => new MsEventVM()
                {
                    Datetime = x.p.Datetime,
                    Event = x.p.Event
                }).ToListAsync();

            return data;
        }
        public async Task<List<MsSpeedVM>> GetBTD2SpeedsByTime(GetMsByTimeRequest request)
        {
            var query = from p in _ghContext.DivBtd2SpeedHistrecords
                        select new { p };
            var results = new List<MsSpeedVM>();
            switch (request.Selected)
            {
                case "Time":
                    results = await query.GroupBy(i => i.p.TriggerTime.Date)
                          .Select(g => new MsSpeedVM() { Datetime = g.Key, Speed = g.Average(a => a.p.ColaHistVariable) })
                          .Where(w => w.Datetime >= request.From && w.Datetime <= request.To)
                          .OrderBy(o => o.Datetime)
                          .ToListAsync();
                    return results;

                case "Hour":
                    if (request.From != null && request.To != null)
                        query = query.Where(q => q.p.TriggerTime >= request.From && q.p.TriggerTime <= request.To);

                    results = await query.OrderBy(z => z.p.TriggerTime)
                        .Select(x => new MsSpeedVM()
                        {
                            Datetime = x.p.TriggerTime,
                            Speed = x.p.ColaHistVariable,
                        }).ToListAsync();
                    return results;

                case "Day":
                    if (request.From != null && request.To != null)
                        query = query.Where(q => q.p.TriggerTime >= request.From && q.p.TriggerTime <= request.To);

                    results = await query.OrderBy(z => z.p.TriggerTime)
                        .Select(x => new MsSpeedVM()
                        {
                            Datetime = x.p.TriggerTime,
                            Speed = x.p.ColaHistVariable,
                        }).ToListAsync();
                    return results;

                case "Week":
                    results = await query.GroupBy(i => i.p.TriggerTime.Date)
                          .Select(g => new MsSpeedVM() { Datetime = g.Key, Speed = g.Average(a => a.p.ColaHistVariable) })
                          .Where(w => w.Datetime >= request.From && w.Datetime <= request.To)
                          .OrderBy(o => o.Datetime)
                          .ToListAsync();
                    return results;

                case "Month":
                    results = await query.GroupBy(i => i.p.TriggerTime.Date)
                          .Select(g => new MsSpeedVM() { Datetime = g.Key, Speed = g.Average(a => a.p.ColaHistVariable) })
                          .Where(w => w.Datetime >= request.From && w.Datetime <= request.To)
                          .OrderBy(o => o.Datetime)
                          .ToListAsync();
                    return results;

                case "Year":
                    results = await query.GroupBy(i => i.p.TriggerTime.Date)
                          .Select(g => new MsSpeedVM() { Datetime = g.Key, Speed = g.Average(a => a.p.ColaHistVariable) })
                          .Where(w => w.Datetime >= request.From && w.Datetime <= request.To)
                          .OrderBy(o => o.Datetime)
                          .ToListAsync();
                    return results;
            }
            return null;
        }
        public async Task<List<MsSpeedVM>> GetBTD3SpeedsByTime(GetMsByTimeRequest request)
        {
            var query = from p in _ghContext.DivBtd3SpeedHistrecords
                        select new { p };
            var results = new List<MsSpeedVM>();
            switch (request.Selected)
            {
                case "Time":
                    results = await query.GroupBy(i => i.p.TriggerTime.Date)
                          .Select(g => new MsSpeedVM() { Datetime = g.Key, Speed = g.Average(a => a.p.ColaHistVariable) })
                          .Where(w => w.Datetime >= request.From && w.Datetime <= request.To)
                          .OrderBy(o => o.Datetime)
                          .ToListAsync();
                    return results;

                case "Hour":
                    if (request.From != null && request.To != null)
                        query = query.Where(q => q.p.TriggerTime >= request.From && q.p.TriggerTime <= request.To);

                    results = await query.OrderBy(z => z.p.TriggerTime)
                        .Select(x => new MsSpeedVM()
                        {
                            Datetime = x.p.TriggerTime,
                            Speed = x.p.ColaHistVariable,
                        }).ToListAsync();
                    return results;

                case "Day":
                    if (request.From != null && request.To != null)
                        query = query.Where(q => q.p.TriggerTime >= request.From && q.p.TriggerTime <= request.To);

                    results = await query.OrderBy(z => z.p.TriggerTime)
                        .Select(x => new MsSpeedVM()
                        {
                            Datetime = x.p.TriggerTime,
                            Speed = x.p.ColaHistVariable,
                        }).ToListAsync();
                    return results;

                case "Week":
                    results = await query.GroupBy(i => i.p.TriggerTime.Date)
                          .Select(g => new MsSpeedVM() { Datetime = g.Key, Speed = g.Average(a => a.p.ColaHistVariable) })
                          .Where(w => w.Datetime >= request.From && w.Datetime <= request.To)
                          .OrderBy(o => o.Datetime)
                          .ToListAsync();
                    return results;

                case "Month":
                    results = await query.GroupBy(i => i.p.TriggerTime.Date)
                          .Select(g => new MsSpeedVM() { Datetime = g.Key, Speed = g.Average(a => a.p.ColaHistVariable) })
                          .Where(w => w.Datetime >= request.From && w.Datetime <= request.To)
                          .OrderBy(o => o.Datetime)
                          .ToListAsync();
                    return results;

                case "Year":
                    results = await query.GroupBy(i => i.p.TriggerTime.Date)
                          .Select(g => new MsSpeedVM() { Datetime = g.Key, Speed = g.Average(a => a.p.ColaHistVariable) })
                          .Where(w => w.Datetime >= request.From && w.Datetime <= request.To)
                          .OrderBy(o => o.Datetime)
                          .ToListAsync();
                    return results;
            }
            return null;
        }
        public async Task<List<MsSpeedVM>> GetBTD4SpeedsByTime(GetMsByTimeRequest request)
        {
            var query = from p in _ghContext.DivBtd4SpeedHistrecords
                        select new { p };
            var results = new List<MsSpeedVM>();
            switch (request.Selected)
            {
                case "Time":
                    results = await query.GroupBy(i => i.p.TriggerTime.Date)
                          .Select(g => new MsSpeedVM() { Datetime = g.Key, Speed = g.Average(a => a.p.ColaHistVariable) })
                          .Where(w => w.Datetime >= request.From && w.Datetime <= request.To)
                          .OrderBy(o => o.Datetime)
                          .ToListAsync();
                    return results;

                case "Hour":
                    if (request.From != null && request.To != null)
                        query = query.Where(q => q.p.TriggerTime >= request.From && q.p.TriggerTime <= request.To);

                    results = await query.OrderBy(z => z.p.TriggerTime)
                        .Select(x => new MsSpeedVM()
                        {
                            Datetime = x.p.TriggerTime,
                            Speed = x.p.ColaHistVariable,
                        }).ToListAsync();
                    return results;

                case "Day":
                    if (request.From != null && request.To != null)
                        query = query.Where(q => q.p.TriggerTime >= request.From && q.p.TriggerTime <= request.To);

                    results = await query.OrderBy(z => z.p.TriggerTime)
                        .Select(x => new MsSpeedVM()
                        {
                            Datetime = x.p.TriggerTime,
                            Speed = x.p.ColaHistVariable,
                        }).ToListAsync();
                    return results;

                case "Week":
                    results = await query.GroupBy(i => i.p.TriggerTime.Date)
                          .Select(g => new MsSpeedVM() { Datetime = g.Key, Speed = g.Average(a => a.p.ColaHistVariable) })
                          .Where(w => w.Datetime >= request.From && w.Datetime <= request.To)
                          .OrderBy(o => o.Datetime)
                          .ToListAsync();
                    return results;

                case "Month":
                    results = await query.GroupBy(i => i.p.TriggerTime.Date)
                          .Select(g => new MsSpeedVM() { Datetime = g.Key, Speed = g.Average(a => a.p.ColaHistVariable) })
                          .Where(w => w.Datetime >= request.From && w.Datetime <= request.To)
                          .OrderBy(o => o.Datetime)
                          .ToListAsync();
                    return results;

                case "Year":
                    results = await query.GroupBy(i => i.p.TriggerTime.Date)
                          .Select(g => new MsSpeedVM() { Datetime = g.Key, Speed = g.Average(a => a.p.ColaHistVariable) })
                          .Where(w => w.Datetime >= request.From && w.Datetime <= request.To)
                          .OrderBy(o => o.Datetime)
                          .ToListAsync();
                    return results;
            }
            return null;
        }
        public async Task<List<MsSpeedVM>> GetBTD5SpeedsByTime(GetMsByTimeRequest request)
        {
            var query = from p in _ghContext.DivBtd5SpeedHistrecords
                        select new { p };
            var results = new List<MsSpeedVM>();
            switch (request.Selected)
            {
                case "Time":
                    results = await query.GroupBy(i => i.p.TriggerTime.Date)
                          .Select(g => new MsSpeedVM() { Datetime = g.Key, Speed = g.Average(a => a.p.ColaHistVariable) })
                          .Where(w => w.Datetime >= request.From && w.Datetime <= request.To)
                          .OrderBy(o => o.Datetime)
                          .ToListAsync();
                    return results;

                case "Hour":
                    if (request.From != null && request.To != null)
                        query = query.Where(q => q.p.TriggerTime >= request.From && q.p.TriggerTime <= request.To);

                    results = await query.OrderBy(z => z.p.TriggerTime)
                        .Select(x => new MsSpeedVM()
                        {
                            Datetime = x.p.TriggerTime,
                            Speed = x.p.ColaHistVariable,
                        }).ToListAsync();
                    return results;

                case "Day":
                    if (request.From != null && request.To != null)
                        query = query.Where(q => q.p.TriggerTime >= request.From && q.p.TriggerTime <= request.To);

                    results = await query.OrderBy(z => z.p.TriggerTime)
                        .Select(x => new MsSpeedVM()
                        {
                            Datetime = x.p.TriggerTime,
                            Speed = x.p.ColaHistVariable,
                        }).ToListAsync();
                    return results;

                case "Week":
                    results = await query.GroupBy(i => i.p.TriggerTime.Date)
                          .Select(g => new MsSpeedVM() { Datetime = g.Key, Speed = g.Average(a => a.p.ColaHistVariable) })
                          .Where(w => w.Datetime >= request.From && w.Datetime <= request.To)
                          .OrderBy(o => o.Datetime)
                          .ToListAsync();
                    return results;

                case "Month":
                    results = await query.GroupBy(i => i.p.TriggerTime.Date)
                          .Select(g => new MsSpeedVM() { Datetime = g.Key, Speed = g.Average(a => a.p.ColaHistVariable) })
                          .Where(w => w.Datetime >= request.From && w.Datetime <= request.To)
                          .OrderBy(o => o.Datetime)
                          .ToListAsync();
                    return results;

                case "Year":
                    results = await query.GroupBy(i => i.p.TriggerTime.Date)
                          .Select(g => new MsSpeedVM() { Datetime = g.Key, Speed = g.Average(a => a.p.ColaHistVariable) })
                          .Where(w => w.Datetime >= request.From && w.Datetime <= request.To)
                          .OrderBy(o => o.Datetime)
                          .ToListAsync();
                    return results;
            }
            return null;
        }
        public async Task<int> GetBTD2ProductCodeChangeByTime(GetMsByTimeRequest request)
        {
            var query = from p in _context.Btd2PoKpis
                        select new { p };

            if (request.From != null && request.To != null)
                query = query.Where(q => q.p.Endtime >= request.From && q.p.Endtime <= request.To);

            var data = await query.CountAsync();
            return data;
        }
        public async Task<int> GetBTD3ProductCodeChangeByTime(GetMsByTimeRequest request)
        {
            var query = from p in _context.Btd3PoKpis
                        select new { p };

            if (request.From != null && request.To != null)
                query = query.Where(q => q.p.Endtime >= request.From && q.p.Endtime <= request.To);

            var data = await query.CountAsync();
            return data;
        }
        public async Task<int> GetBTD4ProductCodeChangeByTime(GetMsByTimeRequest request)
        {
            var query = from p in _context.Btd4PoKpis
                        select new { p };

            if (request.From != null && request.To != null)
                query = query.Where(q => q.p.Endtime >= request.From && q.p.Endtime <= request.To);

            var data = await query.CountAsync();
            return data;
        }
        public async Task<int> GetBTD5ProductCodeChangeByTime(GetMsByTimeRequest request)
        {
            var query = from p in _context.Btd5PoKpis
                        select new { p };

            if (request.From != null && request.To != null)
                query = query.Where(q => q.p.Endtime >= request.From && q.p.Endtime <= request.To);

            var data = await query.CountAsync();
            return data;
        }
        public async Task<int?> GetBTD2JobCountByTime(GetMsByTimeRequest request)
        {
            var query = from p in _context.Btd2PoKpis
                        select new { p };

            if (request.From != null && request.To != null)
                query = query.Where(q => q.p.Endtime >= request.From && q.p.Endtime <= request.To);

            var data = await query.SumAsync(c => c.p.Job);
            return data;
        }
        public async Task<int?> GetBTD3JobCountByTime(GetMsByTimeRequest request)
        {
            var query = from p in _context.Btd3PoKpis
                        select new { p };

            if (request.From != null && request.To != null)
                query = query.Where(q => q.p.Endtime >= request.From && q.p.Endtime <= request.To);

            var data = await query.SumAsync(c => c.p.Job);
            return data;
        }
        public async Task<int?> GetBTD4JobCountByTime(GetMsByTimeRequest request)
        {
            var query = from p in _context.Btd4PoKpis
                        select new { p };

            if (request.From != null && request.To != null)
                query = query.Where(q => q.p.Endtime >= request.From && q.p.Endtime <= request.To);

            var data = await query.SumAsync(c => c.p.Job);
            return data;
        }
        public async Task<int?> GetBTD5JobCountByTime(GetMsByTimeRequest request)
        {
            var query = from p in _context.Btd5PoKpis
                        select new { p };

            if (request.From != null && request.To != null)
                query = query.Where(q => q.p.Endtime >= request.From && q.p.Endtime <= request.To);

            var data = await query.SumAsync(c => c.p.Job);
            return data;
        }
        #endregion
        #region Gluing
        public async Task<List<MsEventVM>> GetD650EventsByTime(GetMsByTimeRequest request)
        {
            var query = from p in _context.D650PoEvents
                        select new { p };

            if (request.From != null && request.To != null)
                query = query.Where(q => q.p.Datetime >= request.From && q.p.Datetime <= request.To);

            var data = await query.OrderByDescending(z => z.p.Datetime)
                .Select(x => new MsEventVM()
                {
                    Datetime = x.p.Datetime,
                    Event = x.p.Event
                }).ToListAsync();

            return data;
        }
        public async Task<List<MsEventVM>> GetD750EventsByTime(GetMsByTimeRequest request)
        {
            var query = from p in _context.D750PoEvents
                        select new { p };

            TimeSpan interval = new TimeSpan(0, 15, 0);
            if (request.From != null && request.To != null)
                query = query.Where(q => q.p.Datetime >= request.From && q.p.Datetime <= request.To);

            //query = query.GroupBy(x=> x.p.Datetime.tick)
            var data = await query.OrderByDescending(z => z.p.Datetime)
                .Select(x => new MsEventVM()
                {
                    Datetime = x.p.Datetime,
                    Event = x.p.Event
                }).ToListAsync();

            return data;
        }
        public async Task<List<MsEventVM>> GetD1000EventsByTime(GetMsByTimeRequest request)
        {
            var query = from p in _context.D1000PoEvents
                        select new { p };

            if (request.From != null && request.To != null)
                query = query.Where(q => q.p.Datetime >= request.From && q.p.Datetime <= request.To);

            var data = await query.OrderByDescending(z => z.p.Datetime)
                .Select(x => new MsEventVM()
                {
                    Datetime = x.p.Datetime,
                    Event = x.p.Event
                }).ToListAsync();

            return data;
        }
        public async Task<List<MsEventVM>> GetD1100EventsByTime(GetMsByTimeRequest request)
        {
            var query = from p in _context.D1100PoEvents
                        select new { p };

            if (request.From != null && request.To != null)
                query = query.Where(q => q.p.Datetime >= request.From && q.p.Datetime <= request.To);

            var data = await query.OrderByDescending(z => z.p.Datetime)
                .Select(x => new MsEventVM()
                {
                    Datetime = x.p.Datetime,
                    Event = x.p.Event
                }).ToListAsync();

            return data;
        }
        public async Task<List<MsSpeedVM>> GetD650SpeedsByTime(GetMsByTimeRequest request)
        {
            var query = from p in _ghContext.DivD650SpeedHistrecords
                        select new { p };
            var results = new List<MsSpeedVM>();
            switch (request.Selected)
            {
                case "Time":
                    results = await query.GroupBy(i => i.p.TriggerTime.Date)
                          .Select(g => new MsSpeedVM() { Datetime = g.Key, Speed = g.Average(a => a.p.ColaHistVariable) })
                          .Where(w => w.Datetime >= request.From && w.Datetime <= request.To)
                          .OrderBy(o => o.Datetime)
                          .ToListAsync();
                    return results;

                case "Hour":
                    if (request.From != null && request.To != null)
                        query = query.Where(q => q.p.TriggerTime >= request.From && q.p.TriggerTime <= request.To);

                    results = await query.OrderBy(z => z.p.TriggerTime)
                        .Select(x => new MsSpeedVM()
                        {
                            Datetime = x.p.TriggerTime,
                            Speed = x.p.ColaHistVariable,
                        }).ToListAsync();
                    return results;

                case "Day":
                    if (request.From != null && request.To != null)
                        query = query.Where(q => q.p.TriggerTime >= request.From && q.p.TriggerTime <= request.To);

                    results = await query.OrderBy(z => z.p.TriggerTime)
                        .Select(x => new MsSpeedVM()
                        {
                            Datetime = x.p.TriggerTime,
                            Speed = x.p.ColaHistVariable,
                        }).ToListAsync();
                    return results;

                case "Week":
                    results = await query.GroupBy(i => i.p.TriggerTime.Date)
                          .Select(g => new MsSpeedVM() { Datetime = g.Key, Speed = g.Average(a => a.p.ColaHistVariable) })
                          .Where(w => w.Datetime >= request.From && w.Datetime <= request.To)
                          .OrderBy(o => o.Datetime)
                          .ToListAsync();
                    return results;

                case "Month":
                    results = await query.GroupBy(i => i.p.TriggerTime.Date)
                          .Select(g => new MsSpeedVM() { Datetime = g.Key, Speed = g.Average(a => a.p.ColaHistVariable) })
                          .Where(w => w.Datetime >= request.From && w.Datetime <= request.To)
                          .OrderBy(o => o.Datetime)
                          .ToListAsync();
                    return results;

                case "Year":
                    results = await query.GroupBy(i => i.p.TriggerTime.Date)
                          .Select(g => new MsSpeedVM() { Datetime = g.Key, Speed = g.Average(a => a.p.ColaHistVariable) })
                          .Where(w => w.Datetime >= request.From && w.Datetime <= request.To)
                          .OrderBy(o => o.Datetime)
                          .ToListAsync();
                    return results;
            }
            return null;
        }
        public async Task<List<MsSpeedVM>> GetD750SpeedsByTime(GetMsByTimeRequest request)
        {
            var query = from p in _ghContext.DivD750SpeedHistrecords
                        select new { p };
            var results = new List<MsSpeedVM>();
            switch (request.Selected)
            {
                case "Time":
                    results = await query.GroupBy(i => i.p.TriggerTime.Date)
                          .Select(g => new MsSpeedVM() { Datetime = g.Key, Speed = g.Average(a => a.p.ColaHistVariable) })
                          .Where(w => w.Datetime >= request.From && w.Datetime <= request.To)
                          .OrderBy(o => o.Datetime)
                          .ToListAsync();
                    return results;

                case "Hour":
                    if (request.From != null && request.To != null)
                        query = query.Where(q => q.p.TriggerTime >= request.From && q.p.TriggerTime <= request.To);

                    results = await query.OrderBy(z => z.p.TriggerTime)
                        .Select(x => new MsSpeedVM()
                        {
                            Datetime = x.p.TriggerTime,
                            Speed = x.p.ColaHistVariable,
                        }).ToListAsync();
                    return results;

                case "Day":
                    if (request.From != null && request.To != null)
                        query = query.Where(q => q.p.TriggerTime >= request.From && q.p.TriggerTime <= request.To);

                    results = await query.OrderBy(z => z.p.TriggerTime)
                        .Select(x => new MsSpeedVM()
                        {
                            Datetime = x.p.TriggerTime,
                            Speed = x.p.ColaHistVariable,
                        }).ToListAsync();
                    return results;

                case "Week":
                    results = await query.GroupBy(i => i.p.TriggerTime.Date)
                          .Select(g => new MsSpeedVM() { Datetime = g.Key, Speed = g.Average(a => a.p.ColaHistVariable) })
                          .Where(w => w.Datetime >= request.From && w.Datetime <= request.To)
                          .OrderBy(o => o.Datetime)
                          .ToListAsync();
                    return results;

                case "Month":
                    results = await query.GroupBy(i => i.p.TriggerTime.Date)
                          .Select(g => new MsSpeedVM() { Datetime = g.Key, Speed = g.Average(a => a.p.ColaHistVariable) })
                          .Where(w => w.Datetime >= request.From && w.Datetime <= request.To)
                          .OrderBy(o => o.Datetime)
                          .ToListAsync();
                    return results;

                case "Year":
                    results = await query.GroupBy(i => i.p.TriggerTime.Date)
                          .Select(g => new MsSpeedVM() { Datetime = g.Key, Speed = g.Average(a => a.p.ColaHistVariable) })
                          .Where(w => w.Datetime >= request.From && w.Datetime <= request.To)
                          .OrderBy(o => o.Datetime)
                          .ToListAsync();
                    return results;
            }
            return null;
        }
        public async Task<List<MsSpeedVM>> GetD1000SpeedsByTime(GetMsByTimeRequest request)
        {
            var query = from p in _ghContext.DivD1000SpeedHistrecords
                        select new { p };
            var results = new List<MsSpeedVM>();
            switch (request.Selected)
            {
                case "Time":
                    results = await query.GroupBy(i => i.p.TriggerTime.Date)
                          .Select(g => new MsSpeedVM() { Datetime = g.Key, Speed = g.Average(a => a.p.ColaHistVariable) })
                          .Where(w => w.Datetime >= request.From && w.Datetime <= request.To)
                          .OrderBy(o => o.Datetime)
                          .ToListAsync();
                    return results;

                case "Hour":
                    if (request.From != null && request.To != null)
                        query = query.Where(q => q.p.TriggerTime >= request.From && q.p.TriggerTime <= request.To);

                    results = await query.OrderBy(z => z.p.TriggerTime)
                        .Select(x => new MsSpeedVM()
                        {
                            Datetime = x.p.TriggerTime,
                            Speed = x.p.ColaHistVariable,
                        }).ToListAsync();
                    return results;

                case "Day":
                    if (request.From != null && request.To != null)
                        query = query.Where(q => q.p.TriggerTime >= request.From && q.p.TriggerTime <= request.To);

                    results = await query.OrderBy(z => z.p.TriggerTime)
                        .Select(x => new MsSpeedVM()
                        {
                            Datetime = x.p.TriggerTime,
                            Speed = x.p.ColaHistVariable,
                        }).ToListAsync();
                    return results;

                case "Week":
                    results = await query.GroupBy(i => i.p.TriggerTime.Date)
                          .Select(g => new MsSpeedVM() { Datetime = g.Key, Speed = g.Average(a => a.p.ColaHistVariable) })
                          .Where(w => w.Datetime >= request.From && w.Datetime <= request.To)
                          .OrderBy(o => o.Datetime)
                          .ToListAsync();
                    return results;

                case "Month":
                    results = await query.GroupBy(i => i.p.TriggerTime.Date)
                          .Select(g => new MsSpeedVM() { Datetime = g.Key, Speed = g.Average(a => a.p.ColaHistVariable) })
                          .Where(w => w.Datetime >= request.From && w.Datetime <= request.To)
                          .OrderBy(o => o.Datetime)
                          .ToListAsync();
                    return results;

                case "Year":
                    results = await query.GroupBy(i => i.p.TriggerTime.Date)
                          .Select(g => new MsSpeedVM() { Datetime = g.Key, Speed = g.Average(a => a.p.ColaHistVariable) })
                          .Where(w => w.Datetime >= request.From && w.Datetime <= request.To)
                          .OrderBy(o => o.Datetime)
                          .ToListAsync();
                    return results;
            }
            return null;
        }
        public async Task<List<MsSpeedVM>> GetD1100SpeedsByTime(GetMsByTimeRequest request)
        {
            var query = from p in _ghContext.DivD1100SpeedHistrecords
                        select new { p };
            var results = new List<MsSpeedVM>();
            switch (request.Selected)
            {
                case "Time":
                    results = await query.GroupBy(i => i.p.TriggerTime.Date)
                          .Select(g => new MsSpeedVM() { Datetime = g.Key, Speed = g.Average(a => a.p.ColaHistVariable) })
                          .Where(w => w.Datetime >= request.From && w.Datetime <= request.To)
                          .OrderBy(o => o.Datetime)
                          .ToListAsync();
                    return results;

                case "Hour":
                    if (request.From != null && request.To != null)
                        query = query.Where(q => q.p.TriggerTime >= request.From && q.p.TriggerTime <= request.To);

                    results = await query.OrderBy(z => z.p.TriggerTime)
                        .Select(x => new MsSpeedVM()
                        {
                            Datetime = x.p.TriggerTime,
                            Speed = x.p.ColaHistVariable,
                        }).ToListAsync();
                    return results;

                case "Day":
                    if (request.From != null && request.To != null)
                        query = query.Where(q => q.p.TriggerTime >= request.From && q.p.TriggerTime <= request.To);

                    results = await query.OrderBy(z => z.p.TriggerTime)
                        .Select(x => new MsSpeedVM()
                        {
                            Datetime = x.p.TriggerTime,
                            Speed = x.p.ColaHistVariable,
                        }).ToListAsync();
                    return results;

                case "Week":
                    results = await query.GroupBy(i => i.p.TriggerTime.Date)
                          .Select(g => new MsSpeedVM() { Datetime = g.Key, Speed = g.Average(a => a.p.ColaHistVariable) })
                          .Where(w => w.Datetime >= request.From && w.Datetime <= request.To)
                          .OrderBy(o => o.Datetime)
                          .ToListAsync();
                    return results;

                case "Month":
                    results = await query.GroupBy(i => i.p.TriggerTime.Date)
                          .Select(g => new MsSpeedVM() { Datetime = g.Key, Speed = g.Average(a => a.p.ColaHistVariable) })
                          .Where(w => w.Datetime >= request.From && w.Datetime <= request.To)
                          .OrderBy(o => o.Datetime)
                          .ToListAsync();
                    return results;

                case "Year":
                    results = await query.GroupBy(i => i.p.TriggerTime.Date)
                          .Select(g => new MsSpeedVM() { Datetime = g.Key, Speed = g.Average(a => a.p.ColaHistVariable) })
                          .Where(w => w.Datetime >= request.From && w.Datetime <= request.To)
                          .OrderBy(o => o.Datetime)
                          .ToListAsync();
                    return results;
            }
            return null;
        }
        public async Task<int> GetD650ProductCodeChangeByTime(GetMsByTimeRequest request)
        {
            var query = from p in _context.D650PoKpis
                        select new { p };

            if (request.From != null && request.To != null)
                query = query.Where(q => q.p.Endtime >= request.From && q.p.Endtime <= request.To);

            var data = await query.CountAsync();
            return data;
        }
        public async Task<int> GetD750ProductCodeChangeByTime(GetMsByTimeRequest request)
        {
            var query = from p in _context.D750PoKpis
                        select new { p };

            if (request.From != null && request.To != null)
                query = query.Where(q => q.p.Endtime >= request.From && q.p.Endtime <= request.To);

            var data = await query.CountAsync();
            return data;
        }
        public async Task<int> GetD1000ProductCodeChangeByTime(GetMsByTimeRequest request)
        {
            var query = from p in _context.D1000PoKpis
                        select new { p };

            if (request.From != null && request.To != null)
                query = query.Where(q => q.p.Endtime >= request.From && q.p.Endtime <= request.To);

            var data = await query.CountAsync();
            return data;
        }
        public async Task<int> GetD1100ProductCodeChangeByTime(GetMsByTimeRequest request)
        {
            var query = from p in _context.D1100PoKpis
                        select new { p };

            if (request.From != null && request.To != null)
                query = query.Where(q => q.p.Endtime >= request.From && q.p.Endtime <= request.To);

            var data = await query.CountAsync();
            return data;
        }
        public async Task<int?> GetD650JobCountByTime(GetMsByTimeRequest request)
        {
            var query = from p in _context.D650PoKpis
                        select new { p };

            if (request.From != null && request.To != null)
                query = query.Where(q => q.p.Endtime >= request.From && q.p.Endtime <= request.To);

            var data = await query.SumAsync(c => c.p.Job);
            return data;
        }
        public async Task<int?> GetD750JobCountByTime(GetMsByTimeRequest request)
        {
            var query = from p in _context.D750PoKpis
                        select new { p };

            if (request.From != null && request.To != null)
                query = query.Where(q => q.p.Endtime >= request.From && q.p.Endtime <= request.To);

            var data = await query.SumAsync(c => c.p.Job);
            return data;
        }
        public async Task<int?> GetD1000JobCountByTime(GetMsByTimeRequest request)
        {
            var query = from p in _context.D1100PoKpis
                        select new { p };

            if (request.From != null && request.To != null)
                query = query.Where(q => q.p.Endtime >= request.From && q.p.Endtime <= request.To);

            var data = await query.SumAsync(c => c.p.Job);
            return data;
        }
        public async Task<int?> GetD1100JobCountByTime(GetMsByTimeRequest request)
        {
            var query = from p in _context.D1100PoKpis
                        select new { p };

            if (request.From != null && request.To != null)
                query = query.Where(q => q.p.Endtime >= request.From && q.p.Endtime <= request.To);

            var data = await query.SumAsync(c => c.p.Job);
            return data;
        }
        #endregion
        #region Scl_Gmc
        public async Task<List<MsEventVM>> GetSclEventsByTime(GetMsByTimeRequest request)
        {
            var query = from p in _context.SclPoEvents
                        select new { p };

            if (request.From != null && request.To != null)
                query = query.Where(q => q.p.Datetime >= request.From && q.p.Datetime <= request.To);

            var data = await query.OrderByDescending(z => z.p.Datetime)
                .Select(x => new MsEventVM()
                {
                    Datetime = x.p.Datetime,
                    Event = x.p.Event
                }).ToListAsync();

            return data;
        }
        public async Task<List<MsEventVM>> GetGmc1EventsByTime(GetMsByTimeRequest request)
        {
            var query = from p in _context.Gmc1PoEvents
                        select new { p };

            TimeSpan interval = new TimeSpan(0, 15, 0);
            if (request.From != null && request.To != null)
                query = query.Where(q => q.p.Datetime >= request.From && q.p.Datetime <= request.To);

            //query = query.GroupBy(x=> x.p.Datetime.tick)
            var data = await query.OrderByDescending(z => z.p.Datetime)
                .Select(x => new MsEventVM()
                {
                    Datetime = x.p.Datetime,
                    Event = x.p.Event
                }).ToListAsync();

            return data;
        }
        public async Task<List<MsEventVM>> GetGmc2EventsByTime(GetMsByTimeRequest request)
        {
            var query = from p in _context.Gmc2PoEvents
                        select new { p };

            if (request.From != null && request.To != null)
                query = query.Where(q => q.p.Datetime >= request.From && q.p.Datetime <= request.To);

            var data = await query.OrderByDescending(z => z.p.Datetime)
                .Select(x => new MsEventVM()
                {
                    Datetime = x.p.Datetime,
                    Event = x.p.Event
                }).ToListAsync();

            return data;
        }
        public async Task<List<MsSpeedVM>> GetSclSpeedsByTime(GetMsByTimeRequest request)
        {
            var query = from p in _ghContext.DivSclSpeedHistrecords
                        select new { p };
            var results = new List<MsSpeedVM>();
            switch (request.Selected)
            {
                case "Time":
                    results = await query.GroupBy(i => i.p.TriggerTime.Date)
                          .Select(g => new MsSpeedVM() { Datetime = g.Key, Speed = g.Average(a => a.p.ColaHistVariable) })
                          .Where(w => w.Datetime >= request.From && w.Datetime <= request.To)
                          .OrderBy(o => o.Datetime)
                          .ToListAsync();
                    return results;

                case "Hour":
                    if (request.From != null && request.To != null)
                        query = query.Where(q => q.p.TriggerTime >= request.From && q.p.TriggerTime <= request.To);

                    results = await query.OrderBy(z => z.p.TriggerTime)
                        .Select(x => new MsSpeedVM()
                        {
                            Datetime = x.p.TriggerTime,
                            Speed = x.p.ColaHistVariable,
                        }).ToListAsync();
                    return results;

                case "Day":
                    if (request.From != null && request.To != null)
                        query = query.Where(q => q.p.TriggerTime >= request.From && q.p.TriggerTime <= request.To);

                    results = await query.OrderBy(z => z.p.TriggerTime)
                        .Select(x => new MsSpeedVM()
                        {
                            Datetime = x.p.TriggerTime,
                            Speed = x.p.ColaHistVariable,
                        }).ToListAsync();
                    return results;

                case "Week":
                    results = await query.GroupBy(i => i.p.TriggerTime.Date)
                          .Select(g => new MsSpeedVM() { Datetime = g.Key, Speed = g.Average(a => a.p.ColaHistVariable) })
                          .Where(w => w.Datetime >= request.From && w.Datetime <= request.To)
                          .OrderBy(o => o.Datetime)
                          .ToListAsync();
                    return results;

                case "Month":
                    results = await query.GroupBy(i => i.p.TriggerTime.Date)
                          .Select(g => new MsSpeedVM() { Datetime = g.Key, Speed = g.Average(a => a.p.ColaHistVariable) })
                          .Where(w => w.Datetime >= request.From && w.Datetime <= request.To)
                          .OrderBy(o => o.Datetime)
                          .ToListAsync();
                    return results;

                case "Year":
                    results = await query.GroupBy(i => i.p.TriggerTime.Date)
                          .Select(g => new MsSpeedVM() { Datetime = g.Key, Speed = g.Average(a => a.p.ColaHistVariable) })
                          .Where(w => w.Datetime >= request.From && w.Datetime <= request.To)
                          .OrderBy(o => o.Datetime)
                          .ToListAsync();
                    return results;
            }
            return null;
        }
        public async Task<List<MsSpeedVM>> GetGmc1SpeedsByTime(GetMsByTimeRequest request)
        {
            var query = from p in _ghContext.DivGmc1SpeedHistrecords
                        select new { p };
            var results = new List<MsSpeedVM>();
            switch (request.Selected)
            {
                case "Time":
                    results = await query.GroupBy(i => i.p.TriggerTime.Date)
                          .Select(g => new MsSpeedVM() { Datetime = g.Key, Speed = g.Average(a => a.p.ColaHistVariable) })
                          .Where(w => w.Datetime >= request.From && w.Datetime <= request.To)
                          .OrderBy(o => o.Datetime)
                          .ToListAsync();
                    return results;

                case "Hour":
                    if (request.From != null && request.To != null)
                        query = query.Where(q => q.p.TriggerTime >= request.From && q.p.TriggerTime <= request.To);

                    results = await query.OrderBy(z => z.p.TriggerTime)
                        .Select(x => new MsSpeedVM()
                        {
                            Datetime = x.p.TriggerTime,
                            Speed = x.p.ColaHistVariable,
                        }).ToListAsync();
                    return results;

                case "Day":
                    if (request.From != null && request.To != null)
                        query = query.Where(q => q.p.TriggerTime >= request.From && q.p.TriggerTime <= request.To);

                    results = await query.OrderBy(z => z.p.TriggerTime)
                        .Select(x => new MsSpeedVM()
                        {
                            Datetime = x.p.TriggerTime,
                            Speed = x.p.ColaHistVariable,
                        }).ToListAsync();
                    return results;

                case "Week":
                    results = await query.GroupBy(i => i.p.TriggerTime.Date)
                          .Select(g => new MsSpeedVM() { Datetime = g.Key, Speed = g.Average(a => a.p.ColaHistVariable) })
                          .Where(w => w.Datetime >= request.From && w.Datetime <= request.To)
                          .OrderBy(o => o.Datetime)
                          .ToListAsync();
                    return results;

                case "Month":
                    results = await query.GroupBy(i => i.p.TriggerTime.Date)
                          .Select(g => new MsSpeedVM() { Datetime = g.Key, Speed = g.Average(a => a.p.ColaHistVariable) })
                          .Where(w => w.Datetime >= request.From && w.Datetime <= request.To)
                          .OrderBy(o => o.Datetime)
                          .ToListAsync();
                    return results;

                case "Year":
                    results = await query.GroupBy(i => i.p.TriggerTime.Date)
                          .Select(g => new MsSpeedVM() { Datetime = g.Key, Speed = g.Average(a => a.p.ColaHistVariable) })
                          .Where(w => w.Datetime >= request.From && w.Datetime <= request.To)
                          .OrderBy(o => o.Datetime)
                          .ToListAsync();
                    return results;
            }
            return null;
        }
        public async Task<List<MsSpeedVM>> GetGmc2SpeedsByTime(GetMsByTimeRequest request)
        {
            var query = from p in _ghContext.DivGmc2SpeedHistrecords
                        select new { p };
            var results = new List<MsSpeedVM>();
            switch (request.Selected)
            {
                case "Time":
                    results = await query.GroupBy(i => i.p.TriggerTime.Date)
                          .Select(g => new MsSpeedVM() { Datetime = g.Key, Speed = g.Average(a => a.p.ColaHistVariable) })
                          .Where(w => w.Datetime >= request.From && w.Datetime <= request.To)
                          .OrderBy(o => o.Datetime)
                          .ToListAsync();
                    return results;

                case "Hour":
                    if (request.From != null && request.To != null)
                        query = query.Where(q => q.p.TriggerTime >= request.From && q.p.TriggerTime <= request.To);

                    results = await query.OrderBy(z => z.p.TriggerTime)
                        .Select(x => new MsSpeedVM()
                        {
                            Datetime = x.p.TriggerTime,
                            Speed = x.p.ColaHistVariable,
                        }).ToListAsync();
                    return results;

                case "Day":
                    if (request.From != null && request.To != null)
                        query = query.Where(q => q.p.TriggerTime >= request.From && q.p.TriggerTime <= request.To);

                    results = await query.OrderBy(z => z.p.TriggerTime)
                        .Select(x => new MsSpeedVM()
                        {
                            Datetime = x.p.TriggerTime,
                            Speed = x.p.ColaHistVariable,
                        }).ToListAsync();
                    return results;

                case "Week":
                    results = await query.GroupBy(i => i.p.TriggerTime.Date)
                          .Select(g => new MsSpeedVM() { Datetime = g.Key, Speed = g.Average(a => a.p.ColaHistVariable) })
                          .Where(w => w.Datetime >= request.From && w.Datetime <= request.To)
                          .OrderBy(o => o.Datetime)
                          .ToListAsync();
                    return results;

                case "Month":
                    results = await query.GroupBy(i => i.p.TriggerTime.Date)
                          .Select(g => new MsSpeedVM() { Datetime = g.Key, Speed = g.Average(a => a.p.ColaHistVariable) })
                          .Where(w => w.Datetime >= request.From && w.Datetime <= request.To)
                          .OrderBy(o => o.Datetime)
                          .ToListAsync();
                    return results;

                case "Year":
                    results = await query.GroupBy(i => i.p.TriggerTime.Date)
                          .Select(g => new MsSpeedVM() { Datetime = g.Key, Speed = g.Average(a => a.p.ColaHistVariable) })
                          .Where(w => w.Datetime >= request.From && w.Datetime <= request.To)
                          .OrderBy(o => o.Datetime)
                          .ToListAsync();
                    return results;
            }
            return null;
        }
        public async Task<int> GetSclProductCodeChangeByTime(GetMsByTimeRequest request)
        {
            var query = from p in _context.SclPoKpis
                        select new { p };

            if (request.From != null && request.To != null)
                query = query.Where(q => q.p.Endtime >= request.From && q.p.Endtime <= request.To);

            var data = await query.CountAsync();
            return data;
        }
        public async Task<int> GetGmc1ProductCodeChangeByTime(GetMsByTimeRequest request)
        {
            var query = from p in _context.Gmc1PoKpis
                        select new { p };

            if (request.From != null && request.To != null)
                query = query.Where(q => q.p.Endtime >= request.From && q.p.Endtime <= request.To);

            var data = await query.CountAsync();
            return data;
        }
        public async Task<int> GetGmc2ProductCodeChangeByTime(GetMsByTimeRequest request)
        {
            var query = from p in _context.Gmc2PoKpis
                        select new { p };

            if (request.From != null && request.To != null)
                query = query.Where(q => q.p.Endtime >= request.From && q.p.Endtime <= request.To);

            var data = await query.CountAsync();
            return data;
        }
        public async Task<int?> GetSclJobCountByTime(GetMsByTimeRequest request)
        {
            var query = from p in _context.SclPoKpis
                        select new { p };

            if (request.From != null && request.To != null)
                query = query.Where(q => q.p.Endtime >= request.From && q.p.Endtime <= request.To);

            var data = await query.SumAsync(c => c.p.Job);
            return data;
        }
        public async Task<int?> GetGmc1JobCountByTime(GetMsByTimeRequest request)
        {
            var query = from p in _context.Gmc1PoKpis
                        select new { p };

            if (request.From != null && request.To != null)
                query = query.Where(q => q.p.Endtime >= request.From && q.p.Endtime <= request.To);

            var data = await query.SumAsync(c => c.p.Job);
            return data;
        }
        public async Task<int?> GetGmc2JobCountByTime(GetMsByTimeRequest request)
        {
            var query = from p in _context.Gmc2PoKpis
                        select new { p };

            if (request.From != null && request.To != null)
                query = query.Where(q => q.p.Endtime >= request.From && q.p.Endtime <= request.To);

            var data = await query.SumAsync(c => c.p.Job);
            return data;
        }
        #endregion
    }
}
