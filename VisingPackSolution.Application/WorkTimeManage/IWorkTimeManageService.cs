using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VisingPackSolution.ViewModels.Common;
using VisingPackSolution.ViewModels.WorkTimeManage;

namespace VisingPackSolution.Application.WorkTimeManage
{
    public interface IWorkTimeManageService
    {
        Task<WtPrintingVM> GetPrintingWorkTimeMgt(TimeRequest request);
        Task<WtDieCutVM> GetDieCutWorkTimeMgt(TimeRequest request);
        Task<WtGluingVM> GetGluingWorkTimeMgt(TimeRequest request);
        Task<WtSclGmcVM> GetSclGmcWorkTimeMgt(TimeRequest request);

    }
}
