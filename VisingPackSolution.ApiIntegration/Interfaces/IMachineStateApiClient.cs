using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VisingPackSolution.ViewModels.Common;
using VisingPackSolution.ViewModels.MachineState;

namespace VisingPackSolution.ApiIntegration.Interfaces
{
    public interface IMachineStateApiClient
    {
        Task<PagedResult<MsEventVM>> GetPagings(GetP601EventPagingRequest request);    
        Task<MsPrintingVM> GetMsPrintingByTime(GetMsByTimeRequest request);
        Task<MsDieCutVM> GetMsDieCutBytime(GetMsByTimeRequest request);
        Task<MsGluingVM> GetMsGluingByTime(GetMsByTimeRequest request);
        Task<MsSclGmcVM> GetMsSclGmcBytime(GetMsByTimeRequest request);
    }
}
