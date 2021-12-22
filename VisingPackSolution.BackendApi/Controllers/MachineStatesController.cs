using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VisingPackSolution.Application.MachineState;
using VisingPackSolution.ViewModels.MachineState;

namespace VisingPackSolution.BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MachineStatesController : ControllerBase
    {
        private readonly IMachineStateService _msService;
        public MachineStatesController(IMachineStateService msService)
        {
            _msService = msService;
        }

        //http://localhost:port/P601Event
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var p601Events = await _msService.GetAll();
            return Ok(p601Events);
        }

        //http://localhost:port/P601Event/paging
        [HttpGet("paging")]
        public async Task<IActionResult> Get([FromQuery]GetP601EventPagingRequest request)
        {
            var p601Events = await _msService.GetAllPaging(request);
            return Ok(p601Events);
        }

        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var p601Events = await _msService.GetById(id);
            if (p601Events == null)
                return BadRequest("Can not find product");
            return Ok(p601Events);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm]P601EventCreateRequest request)
        {
            if (!ModelState.IsValid)    //check validation
            {
                return BadRequest(ModelState);
            }
            var eventId = await _msService.Create(request);
            if (eventId == 0)
                return BadRequest();

            var eventResult = await _msService.GetById(eventId);

            return CreatedAtAction(nameof(GetById), new { id = eventId }, eventResult);
        }

        [HttpPatch]
        public async Task<IActionResult> Update([FromForm] P601EventUpdateRequest request)
        {
            if (!ModelState.IsValid)    //check validation
            {
                return BadRequest(ModelState);
            }
            var affectedResult = await _msService.Update(request);
            if (affectedResult == 0)
                return BadRequest();

            return Ok();
        }

        [HttpDelete("{uid}")]
        public async Task<IActionResult> Delete(int uid)
        {
            var affectedResult = await _msService.Delete(uid);
            if (affectedResult == 0)
                return BadRequest();

            return Ok();
        }


        [HttpGet("MsPrintingBytime")]
        public async Task<IActionResult> GetMsPrintingByTime([FromQuery] GetMsByTimeRequest request)
        {
            var events = new MsPrintingVM()
            {
                P601Events = await _msService.GetP601EventsByTime(request),
                P604Events = await _msService.GetP604EventsByTime(request),
                P605Events = await _msService.GetP605EventsByTime(request),
                P5MEvents = await _msService.GetP5MEventsByTime(request),

                P601Speeds = await _msService.GetP601SpeedsByTime(request),
                P604Speeds = await _msService.GetP604SpeedsByTime(request),
                P605Speeds = await _msService.GetP605SpeedsByTime(request),
                P5MSpeeds = await _msService.GetP5MSpeedsByTime(request),

                P601ProductCodeChangeCount = await _msService.GetP601ProductCodeChangeByTime(request),
                P604ProductCodeChangeCount = await _msService.GetP604ProductCodeChangeByTime(request),
                P605ProductCodeChangeCount = await _msService.GetP605ProductCodeChangeByTime(request),
                P5MProductCodeChangeCount = await _msService.GetP5MProductCodeChangeByTime(request),

                P601JobCount = await _msService.GetP601JobCountByTime(request),
                P604JobCount = await _msService.GetP604JobCountByTime(request),
                P605JobCount = await _msService.GetP605JobCountByTime(request),
                P5MJobCount = await _msService.GetP5MJobCountByTime(request),
            };
            return Ok(events);
        }

        [HttpGet("MsDieCutBytime")]
        public async Task<IActionResult> GetMsDieCutBytime([FromQuery] GetMsByTimeRequest request)
        {
            var events = new MsDieCutVM()
            {
                BTD2Events = await _msService.GetBTD2EventsByTime(request),
                BTD3Events = await _msService.GetBTD3EventsByTime(request),
                BTD4Events = await _msService.GetBTD4EventsByTime(request),
                BTD5Events = await _msService.GetBTD5EventsByTime(request),

                BTD2Speeds = await _msService.GetBTD2SpeedsByTime(request),
                BTD3Speeds = await _msService.GetBTD3SpeedsByTime(request),
                BTD4Speeds = await _msService.GetBTD4SpeedsByTime(request),
                BTD5Speeds = await _msService.GetBTD5SpeedsByTime(request),

                BTD2ProductCodeChangeCount = await _msService.GetBTD2ProductCodeChangeByTime(request),
                BTD3ProductCodeChangeCount = await _msService.GetBTD3ProductCodeChangeByTime(request),
                BTD4ProductCodeChangeCount = await _msService.GetBTD4ProductCodeChangeByTime(request),
                BTD5ProductCodeChangeCount = await _msService.GetBTD5ProductCodeChangeByTime(request),

                BTD2JobCount = await _msService.GetBTD2JobCountByTime(request),
                BTD3JobCount = await _msService.GetBTD3JobCountByTime(request),
                BTD4JobCount = await _msService.GetBTD4JobCountByTime(request),
                BTD5JobCount = await _msService.GetBTD5JobCountByTime(request),
            };
            return Ok(events);
        }

        [HttpGet("MsGluingBytime")]
        public async Task<IActionResult> GetMsGluingBytime([FromQuery] GetMsByTimeRequest request)
        {
            var events = new MsGluingVM()
            {
                D650Events = await _msService.GetD650EventsByTime(request),
                D750Events = await _msService.GetD750EventsByTime(request),
                D1000Events = await _msService.GetD1000EventsByTime(request),
                D1100Events = await _msService.GetD1100EventsByTime(request),

                D650Speeds = await _msService.GetD650SpeedsByTime(request),
                D750Speeds = await _msService.GetD750SpeedsByTime(request),
                D1000Speeds = await _msService.GetD1000SpeedsByTime(request),
                D1100Speeds = await _msService.GetD1100SpeedsByTime(request),

                D650ProductCodeChangeCount = await _msService.GetD650ProductCodeChangeByTime(request),
                D750ProductCodeChangeCount = await _msService.GetD750ProductCodeChangeByTime(request),
                D1000ProductCodeChangeCount = await _msService.GetD1000ProductCodeChangeByTime(request),
                D1100ProductCodeChangeCount = await _msService.GetD1100ProductCodeChangeByTime(request),

                D650JobCount = await _msService.GetD650JobCountByTime(request),
                D750JobCount = await _msService.GetD750JobCountByTime(request),
                D1000JobCount = await _msService.GetD1000JobCountByTime(request),
                D1100JobCount = await _msService.GetD1100JobCountByTime(request),
            };
            return Ok(events);
        }

        [HttpGet("MsSclGmcBytime")]
        public async Task<IActionResult> GetMsSclGmcBytime([FromQuery] GetMsByTimeRequest request)
        {
            var events = new MsSclGmcVM()
            {
                SclEvents = await _msService.GetSclEventsByTime(request),
                Gmc1Events = await _msService.GetGmc1EventsByTime(request),
                Gmc2Events = await _msService.GetGmc2EventsByTime(request),

                SclSpeeds = await _msService.GetSclSpeedsByTime(request),
                Gmc1Speeds = await _msService.GetGmc1SpeedsByTime(request),
                Gmc2Speeds = await _msService.GetGmc2SpeedsByTime(request),

                SclProductCodeChangeCount = await _msService.GetSclProductCodeChangeByTime(request),
                Gmc1ProductCodeChangeCount = await _msService.GetGmc1ProductCodeChangeByTime(request),
                Gmc2ProductCodeChangeCount = await _msService.GetGmc2ProductCodeChangeByTime(request),

                SclJobCount = await _msService.GetSclJobCountByTime(request),
                Gmc1JobCount = await _msService.GetGmc1JobCountByTime(request),
                Gmc2JobCount = await _msService.GetGmc2JobCountByTime(request),
            };
            return Ok(events);
        }
    }
}
