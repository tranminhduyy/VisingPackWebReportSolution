using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using VisingPackSolution.ApiIntegration.Interfaces;
using VisingPackSolution.ViewModels.Common;
using VisingPackSolution.ViewModels.MachineState;

namespace VisingPackSolution.ApiIntegration.Services
{
    public class MachineStateApiClient : BaseApiClient, IMachineStateApiClient
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public MachineStateApiClient(IHttpClientFactory httpClientFactory,
                   IHttpContextAccessor httpContextAccessor,
                    IConfiguration configuration)
            : base(httpClientFactory, httpContextAccessor, configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
        }
        public async Task<PagedResult<MsEventVM>> GetPagings(GetP601EventPagingRequest request)
        {
            var data = await GetAsync<PagedResult<MsEventVM>>(
                $"/api/p601Events/paging?pageIndex={request.PageIndex}" +
                $"&pageSize={request.PageSize}" +
                $"&keyword={request.Keyword}&starttime={request.Starttime}");

            return data;
        }
        //public async Task<List<P601EventVM>> GetByTime(GetEventRequest request)
        //{
        //    var data = await GetListAsync<P601EventVM>(
        //        $"/api/p601Events/bytime?from={request.From}&to={request.To}");
        //    return data;
        //}

        public async Task<MsPrintingVM> GetMsPrintingByTime(GetMsByTimeRequest request)
        {
            var data = await GetAsync<MsPrintingVM>(
                $"/api/MachineStates/MsPrintingBytime?selected={request.Selected}&from={request.From}&to={request.To}");
            return data;
        }

        public async Task<MsDieCutVM> GetMsDieCutBytime(GetMsByTimeRequest request)
        {
            var data = await GetAsync<MsDieCutVM>(
                $"/api/MachineStates/MsDieCutBytime?selected={request.Selected}&from={request.From}&to={request.To}");
            return data;
        }

        public async Task<MsGluingVM> GetMsGluingByTime(GetMsByTimeRequest request)
        {
            var data = await GetAsync<MsGluingVM>(
                $"/api/MachineStates/MsGluingBytime?selected={request.Selected}&from={request.From}&to={request.To}");
            return data;
        }

        public async Task<MsSclGmcVM> GetMsSclGmcBytime(GetMsByTimeRequest request)
        {
            var data = await GetAsync<MsSclGmcVM>(
                $"/api/MachineStates/MsSclGmcBytime?selected={request.Selected}&from={request.From}&to={request.To}");
            return data;
        }
    }
}
