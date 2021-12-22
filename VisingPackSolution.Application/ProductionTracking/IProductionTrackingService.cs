using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VisingPackSolution.ViewModels.Common;
using VisingPackSolution.ViewModels.ProductionTracking;

namespace VisingPackSolution.Application.ProductionTracking
{
    public interface IProductionTrackingService
    {
        #region Printing
        Task<List<Pt1HVM>> GetP601ProductionTracking_1H(TimeRequest request);
        Task<List<Pt1HVM>> GetP604ProductionTracking_1H(TimeRequest request);
        Task<List<Pt1HVM>> GetP605ProductionTracking_1H(TimeRequest request);
        Task<List<Pt1HVM>> GetP5mProductionTracking_1H(TimeRequest request);

        Task<List<PtDayVM>> GetP601ProductionTracking_Day(TimeRequest request);
        Task<List<PtDayVM>> GetP604ProductionTracking_Day(TimeRequest request);
        Task<List<PtDayVM>> GetP605ProductionTracking_Day(TimeRequest request);
        Task<List<PtDayVM>> GetP5mProductionTracking_Day(TimeRequest request);

        Task<PtWeekVM> GetP601ProductionTracking_Week(TimeRequest request);
        Task<PtWeekVM> GetP604ProductionTracking_Week(TimeRequest request);
        Task<PtWeekVM> GetP605ProductionTracking_Week(TimeRequest request);
        Task<PtWeekVM> GetP5mProductionTracking_Week(TimeRequest request);

        Task<PtMonthYearVM> GetP601ProductionTracking_Month(TimeRequest request);
        Task<PtMonthYearVM> GetP604ProductionTracking_Month(TimeRequest request);
        Task<PtMonthYearVM> GetP605ProductionTracking_Month(TimeRequest request);
        Task<PtMonthYearVM> GetP5mProductionTracking_Month(TimeRequest request);

        Task<PtMonthYearVM> GetP601ProductionTracking_Year(TimeRequest request);
        Task<PtMonthYearVM> GetP604ProductionTracking_Year(TimeRequest request);
        Task<PtMonthYearVM> GetP605ProductionTracking_Year(TimeRequest request);
        Task<PtMonthYearVM> GetP5mProductionTracking_Year(TimeRequest request);
        #endregion

        #region DieCut
        Task<List<Pt1HVM>> GetBtd2ProductionTracking_1H(TimeRequest request);
        Task<List<Pt1HVM>> GetBtd3ProductionTracking_1H(TimeRequest request);
        Task<List<Pt1HVM>> GetBtd4ProductionTracking_1H(TimeRequest request);
        Task<List<Pt1HVM>> GetBtd5ProductionTracking_1H(TimeRequest request);

        Task<List<PtDayVM>> GetBtd2ProductionTracking_Day(TimeRequest request);
        Task<List<PtDayVM>> GetBtd3ProductionTracking_Day(TimeRequest request);
        Task<List<PtDayVM>> GetBtd4ProductionTracking_Day(TimeRequest request);
        Task<List<PtDayVM>> GetBtd5ProductionTracking_Day(TimeRequest request);

        Task<PtWeekVM> GetBtd2ProductionTracking_Week(TimeRequest request);
        Task<PtWeekVM> GetBtd3ProductionTracking_Week(TimeRequest request);
        Task<PtWeekVM> GetBtd4ProductionTracking_Week(TimeRequest request);
        Task<PtWeekVM> GetBtd5ProductionTracking_Week(TimeRequest request);

        Task<PtMonthYearVM> GetBtd2ProductionTracking_Month(TimeRequest request);
        Task<PtMonthYearVM> GetBtd3ProductionTracking_Month(TimeRequest request);
        Task<PtMonthYearVM> GetBtd4ProductionTracking_Month(TimeRequest request);
        Task<PtMonthYearVM> GetBtd5ProductionTracking_Month(TimeRequest request);

        Task<PtMonthYearVM> GetBtd2ProductionTracking_Year(TimeRequest request);
        Task<PtMonthYearVM> GetBtd3ProductionTracking_Year(TimeRequest request);
        Task<PtMonthYearVM> GetBtd4ProductionTracking_Year(TimeRequest request);
        Task<PtMonthYearVM> GetBtd5ProductionTracking_Year(TimeRequest request);
        #endregion

        #region Gluing
        Task<List<Pt1HVM>> GetD650ProductionTracking_1H(TimeRequest request);
        Task<List<Pt1HVM>> GetD750ProductionTracking_1H(TimeRequest request);
        Task<List<Pt1HVM>> GetD1000ProductionTracking_1H(TimeRequest request);
        Task<List<Pt1HVM>> GetD1100ProductionTracking_1H(TimeRequest request);

        Task<List<PtDayVM>> GetD650ProductionTracking_Day(TimeRequest request);
        Task<List<PtDayVM>> GetD750ProductionTracking_Day(TimeRequest request);
        Task<List<PtDayVM>> GetD1000ProductionTracking_Day(TimeRequest request);
        Task<List<PtDayVM>> GetD1100ProductionTracking_Day(TimeRequest request);

        Task<PtWeekVM> GetD650ProductionTracking_Week(TimeRequest request);
        Task<PtWeekVM> GetD750ProductionTracking_Week(TimeRequest request);
        Task<PtWeekVM> GetD1000ProductionTracking_Week(TimeRequest request);
        Task<PtWeekVM> GetD1100ProductionTracking_Week(TimeRequest request);

        Task<PtMonthYearVM> GetD650ProductionTracking_Month(TimeRequest request);
        Task<PtMonthYearVM> GetD750ProductionTracking_Month(TimeRequest request);
        Task<PtMonthYearVM> GetD1000ProductionTracking_Month(TimeRequest request);
        Task<PtMonthYearVM> GetD1100ProductionTracking_Month(TimeRequest request);

        Task<PtMonthYearVM> GetD650ProductionTracking_Year(TimeRequest request);
        Task<PtMonthYearVM> GetD750ProductionTracking_Year(TimeRequest request);
        Task<PtMonthYearVM> GetD1000ProductionTracking_Year(TimeRequest request);
        Task<PtMonthYearVM> GetD1100ProductionTracking_Year(TimeRequest request);
        #endregion
    }
}
