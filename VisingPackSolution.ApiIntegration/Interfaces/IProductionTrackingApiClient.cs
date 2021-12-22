using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VisingPackSolution.ViewModels.Common;
using VisingPackSolution.ViewModels.ProductionTracking;

namespace VisingPackSolution.ApiIntegration.Interfaces
{
    public interface IProductionTrackingApiClient
    {
        Task<PtPrinting1HVM> GetPtPrinting1H(TimeRequest request);
        Task<PtPrintingDayVM> GetPtPrintingDay(TimeRequest request);
        Task<PtPrintingWeekVM> GetPtPrintingWeek(TimeRequest request);
        Task<PtPrintingMonthYearVM> GetPtPrintingMonth(TimeRequest request);
        Task<PtPrintingMonthYearVM> GetPtPrintingYear(TimeRequest request);

        Task<PtDieCut1HVM> GetPtDieCut1H(TimeRequest request);
        Task<PtDieCutDayVM> GetPtDieCutDay(TimeRequest request);
        Task<PtDieCutWeekVM> GetPtDieCutWeek(TimeRequest request);
        Task<PtDieCutMonthYearVM> GetPtDieCutMonth(TimeRequest request);
        Task<PtDieCutMonthYearVM> GetPtDieCutYear(TimeRequest request);

        Task<PtGluing1HVM> GetPtGluing1H(TimeRequest request);
        Task<PtGluingDayVM> GetPtGluingDay(TimeRequest request);
        Task<PtGluingWeekVM> GetPtGluingWeek(TimeRequest request);
        Task<PtGluingMonthYearVM> GetPtGluingMonth(TimeRequest request);
        Task<PtGluingMonthYearVM> GetPtGluingYear(TimeRequest request);
    }
}
