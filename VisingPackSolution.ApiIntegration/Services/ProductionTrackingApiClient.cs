using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using VisingPackSolution.ApiIntegration.Interfaces;
using VisingPackSolution.ViewModels.Common;
using VisingPackSolution.ViewModels.ProductionTracking;

namespace VisingPackSolution.ApiIntegration.Services
{
    public class ProductionTrackingApiClient : BaseApiClient, IProductionTrackingApiClient
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public ProductionTrackingApiClient(IHttpContextAccessor httpContextAccessor,
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration)
            : base(httpClientFactory, httpContextAccessor, configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public async Task<PtPrinting1HVM> GetPtPrinting1H(TimeRequest request)
        {
            var data = await GetAsync<PtPrinting1HVM>(
                $"/api/ProductionTrackings/PtPrinting1H?from={request.From}&to={request.To}");
            return data;
        }

        public async Task<PtPrintingDayVM> GetPtPrintingDay(TimeRequest request)
        {
            var data = await GetAsync<PtPrintingDayVM>(
                $"/api/ProductionTrackings/PtPrintingDay?from={request.From}&to={request.To}");
            return data;
        }

        public async Task<PtPrintingWeekVM> GetPtPrintingWeek(TimeRequest request)
        {
            var data = await GetAsync<PtPrintingWeekVM>(
                $"/api/ProductionTrackings/PtPrintingWeek?from={request.From}&to={request.To}");
            return data;
        }

        public async Task<PtPrintingMonthYearVM> GetPtPrintingMonth(TimeRequest request)
        {
            var data = await GetAsync<PtPrintingMonthYearVM>(
                $"/api/ProductionTrackings/PtPrintingMonth?from={request.From}&to={request.To}");
            return data;
        }

        public async Task<PtPrintingMonthYearVM> GetPtPrintingYear(TimeRequest request)
        {
            var data = await GetAsync<PtPrintingMonthYearVM>(
                $"/api/ProductionTrackings/PtPrintingYear?from={request.From}&to={request.To}");
            return data;
        }

        public async Task<PtDieCut1HVM> GetPtDieCut1H(TimeRequest request)
        {
            var data = await GetAsync<PtDieCut1HVM>(
                $"/api/ProductionTrackings/PtDieCut1H?from={request.From}&to={request.To}");
            return data;
        }

        public async Task<PtDieCutDayVM> GetPtDieCutDay(TimeRequest request)
        {
            var data = await GetAsync<PtDieCutDayVM>(
                $"/api/ProductionTrackings/PtDieCutDay?from={request.From}&to={request.To}");
            return data;
        }

        public async Task<PtDieCutWeekVM> GetPtDieCutWeek(TimeRequest request)
        {
            var data = await GetAsync<PtDieCutWeekVM>(
                $"/api/ProductionTrackings/PtDieCutWeek?from={request.From}&to={request.To}");
            return data;
        }

        public async Task<PtDieCutMonthYearVM> GetPtDieCutMonth(TimeRequest request)
        {
            var data = await GetAsync<PtDieCutMonthYearVM>(
                $"/api/ProductionTrackings/PtDieCutMonth?from={request.From}&to={request.To}");
            return data;
        }

        public async Task<PtDieCutMonthYearVM> GetPtDieCutYear(TimeRequest request)
        {
            var data = await GetAsync<PtDieCutMonthYearVM>(
                $"/api/ProductionTrackings/PtDieCutYear?from={request.From}&to={request.To}");
            return data;
        }

        public async Task<PtGluing1HVM> GetPtGluing1H(TimeRequest request)
        {
            var data = await GetAsync<PtGluing1HVM>(
                $"/api/ProductionTrackings/PtGluing1H?from={request.From}&to={request.To}");
            return data;
        }

        public async Task<PtGluingDayVM> GetPtGluingDay(TimeRequest request)
        {
            var data = await GetAsync<PtGluingDayVM>(
                $"/api/ProductionTrackings/PtGluingDay?from={request.From}&to={request.To}");
            return data;
        }

        public async Task<PtGluingWeekVM> GetPtGluingWeek(TimeRequest request)
        {
            var data = await GetAsync<PtGluingWeekVM>(
                $"/api/ProductionTrackings/PtGluingWeek?from={request.From}&to={request.To}");
            return data;
        }

        public async Task<PtGluingMonthYearVM> GetPtGluingMonth(TimeRequest request)
        {
            var data = await GetAsync<PtGluingMonthYearVM>(
                $"/api/ProductionTrackings/PtGluingMonth?from={request.From}&to={request.To}");
            return data;
        }

        public async Task<PtGluingMonthYearVM> GetPtGluingYear(TimeRequest request)
        {
            var data = await GetAsync<PtGluingMonthYearVM>(
                $"/api/ProductionTrackings/PtGluingYear?from={request.From}&to={request.To}");
            return data;
        }
    }
}
