using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VisingPackSolution.ViewModels.Common;
using VisingPackSolution.ViewModels.MachineState;

namespace VisingPackSolution.Application.MachineState
{
    public interface IMachineStateService
    {
        Task<int> Create(P601EventCreateRequest request);

        Task<int> Update(P601EventUpdateRequest request);

        Task<int> Delete(int P601EventId);

        Task<IEnumerable<MsEventVM>> GetAll();

        Task<PagedResult<MsEventVM>> GetAllPaging(GetP601EventPagingRequest request);

        Task<MsEventVM> GetById(int id);

        #region Printing
        Task<List<MsEventVM>> GetP601EventsByTime(GetMsByTimeRequest request);
        Task<List<MsEventVM>> GetP604EventsByTime(GetMsByTimeRequest request);
        Task<List<MsEventVM>> GetP605EventsByTime(GetMsByTimeRequest request);
        Task<List<MsEventVM>> GetP5MEventsByTime(GetMsByTimeRequest request);
        Task<List<MsSpeedVM>> GetP601SpeedsByTime(GetMsByTimeRequest request);
        Task<List<MsSpeedVM>> GetP604SpeedsByTime(GetMsByTimeRequest request);
        Task<List<MsSpeedVM>> GetP605SpeedsByTime(GetMsByTimeRequest request);
        Task<List<MsSpeedVM>> GetP5MSpeedsByTime(GetMsByTimeRequest request);
        Task<int> GetP601ProductCodeChangeByTime(GetMsByTimeRequest request);
        Task<int> GetP604ProductCodeChangeByTime(GetMsByTimeRequest request);
        Task<int> GetP605ProductCodeChangeByTime(GetMsByTimeRequest request);
        Task<int> GetP5MProductCodeChangeByTime(GetMsByTimeRequest request);
        Task<int?> GetP601JobCountByTime(GetMsByTimeRequest request);
        Task<int?> GetP604JobCountByTime(GetMsByTimeRequest request);
        Task<int?> GetP605JobCountByTime(GetMsByTimeRequest request);
        Task<int?> GetP5MJobCountByTime(GetMsByTimeRequest request);
        #endregion
        #region Die-cut
        Task<List<MsEventVM>> GetBTD2EventsByTime(GetMsByTimeRequest request);
        Task<List<MsEventVM>> GetBTD3EventsByTime(GetMsByTimeRequest request);
        Task<List<MsEventVM>> GetBTD4EventsByTime(GetMsByTimeRequest request);
        Task<List<MsEventVM>> GetBTD5EventsByTime(GetMsByTimeRequest request);
        Task<List<MsSpeedVM>> GetBTD2SpeedsByTime(GetMsByTimeRequest request);
        Task<List<MsSpeedVM>> GetBTD3SpeedsByTime(GetMsByTimeRequest request);
        Task<List<MsSpeedVM>> GetBTD4SpeedsByTime(GetMsByTimeRequest request);
        Task<List<MsSpeedVM>> GetBTD5SpeedsByTime(GetMsByTimeRequest request);
        Task<int> GetBTD2ProductCodeChangeByTime(GetMsByTimeRequest request);
        Task<int> GetBTD3ProductCodeChangeByTime(GetMsByTimeRequest request);
        Task<int> GetBTD4ProductCodeChangeByTime(GetMsByTimeRequest request);
        Task<int> GetBTD5ProductCodeChangeByTime(GetMsByTimeRequest request);
        Task<int?> GetBTD2JobCountByTime(GetMsByTimeRequest request);
        Task<int?> GetBTD3JobCountByTime(GetMsByTimeRequest request);
        Task<int?> GetBTD4JobCountByTime(GetMsByTimeRequest request);
        Task<int?> GetBTD5JobCountByTime(GetMsByTimeRequest request);
        #endregion
        #region Gluing
        Task<List<MsEventVM>> GetD650EventsByTime(GetMsByTimeRequest request);
        Task<List<MsEventVM>> GetD750EventsByTime(GetMsByTimeRequest request);
        Task<List<MsEventVM>> GetD1000EventsByTime(GetMsByTimeRequest request);
        Task<List<MsEventVM>> GetD1100EventsByTime(GetMsByTimeRequest request);
        Task<List<MsSpeedVM>> GetD650SpeedsByTime(GetMsByTimeRequest request);
        Task<List<MsSpeedVM>> GetD750SpeedsByTime(GetMsByTimeRequest request);
        Task<List<MsSpeedVM>> GetD1000SpeedsByTime(GetMsByTimeRequest request);
        Task<List<MsSpeedVM>> GetD1100SpeedsByTime(GetMsByTimeRequest request);
        Task<int> GetD650ProductCodeChangeByTime(GetMsByTimeRequest request);
        Task<int> GetD750ProductCodeChangeByTime(GetMsByTimeRequest request);
        Task<int> GetD1000ProductCodeChangeByTime(GetMsByTimeRequest request);
        Task<int> GetD1100ProductCodeChangeByTime(GetMsByTimeRequest request);
        Task<int?> GetD650JobCountByTime(GetMsByTimeRequest request);
        Task<int?> GetD750JobCountByTime(GetMsByTimeRequest request);
        Task<int?> GetD1000JobCountByTime(GetMsByTimeRequest request);
        Task<int?> GetD1100JobCountByTime(GetMsByTimeRequest request);
        #endregion
        #region Scl-Gmc
        Task<List<MsEventVM>> GetSclEventsByTime(GetMsByTimeRequest request);
        Task<List<MsEventVM>> GetGmc1EventsByTime(GetMsByTimeRequest request);
        Task<List<MsEventVM>> GetGmc2EventsByTime(GetMsByTimeRequest request);
        Task<List<MsSpeedVM>> GetSclSpeedsByTime(GetMsByTimeRequest request);
        Task<List<MsSpeedVM>> GetGmc1SpeedsByTime(GetMsByTimeRequest request);
        Task<List<MsSpeedVM>> GetGmc2SpeedsByTime(GetMsByTimeRequest request);
        Task<int> GetSclProductCodeChangeByTime(GetMsByTimeRequest request);
        Task<int> GetGmc1ProductCodeChangeByTime(GetMsByTimeRequest request);
        Task<int> GetGmc2ProductCodeChangeByTime(GetMsByTimeRequest request);
        Task<int?> GetSclJobCountByTime(GetMsByTimeRequest request);
        Task<int?> GetGmc1JobCountByTime(GetMsByTimeRequest request);
        Task<int?> GetGmc2JobCountByTime(GetMsByTimeRequest request);
        #endregion
    }
}
