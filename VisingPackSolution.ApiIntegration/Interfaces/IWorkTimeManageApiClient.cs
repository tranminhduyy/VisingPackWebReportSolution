using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VisingPackSolution.ViewModels.Common;
using VisingPackSolution.ViewModels.MachineState;
using VisingPackSolution.ViewModels.WorkTimeManage;

namespace VisingPackSolution.ApiIntegration.Interfaces
{
    public interface IWorkTimeManageApiClient
    {
        Task<WtPrintingVM> GetWtPrinting(TimeRequest request);
        Task<WtDieCutVM> GetWtDieCut(TimeRequest request);
        Task<WtGluingVM> GetWtGluing(TimeRequest request);
        Task<WtSclGmcVM> GetWtSclGmc(TimeRequest request);
    }
}
