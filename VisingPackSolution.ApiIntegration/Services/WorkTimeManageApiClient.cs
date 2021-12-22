using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using VisingPackSolution.ApiIntegration.Interfaces;
using VisingPackSolution.ViewModels.Common;
using VisingPackSolution.ViewModels.WorkTimeManage;

namespace VisingPackSolution.ApiIntegration.Services
{
    public class WorkTimeManageApiClient : BaseApiClient, IWorkTimeManageApiClient
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public WorkTimeManageApiClient(IHttpContextAccessor httpContextAccessor,
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration)
            : base(httpClientFactory, httpContextAccessor, configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public async Task<WtPrintingVM> GetWtPrinting(TimeRequest request)
        {
            var data = await GetAsync<WtPrintingVM>(
                $"/api/WorkTimeManages/WtPrinting?from={request.From}&to={request.To}");
            return data;
        }

        public async Task<WtDieCutVM> GetWtDieCut(TimeRequest request)
        {
            var data = await GetAsync<WtDieCutVM>(
                $"/api/WorkTimeManages/WtDieCut?from={request.From}&to={request.To}");
            return data;
        }

        public async Task<WtGluingVM> GetWtGluing(TimeRequest request)
        {
            var data = await GetAsync<WtGluingVM>(
                $"/api/WorkTimeManages/WtGluing?from={request.From}&to={request.To}");
            return data;
        }

        public async Task<WtSclGmcVM> GetWtSclGmc(TimeRequest request)
        {
            var data = await GetAsync<WtSclGmcVM>(
                $"/api/WorkTimeManages/WtSclGmc?from={request.From}&to={request.To}");
            return data;
        }
    }
}
