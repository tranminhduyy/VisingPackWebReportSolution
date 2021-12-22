using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VisingPackSolution.ViewModels.Common;
using VisingPackSolution.ViewModels.System.Roles;

namespace VisingPackSolution.ApiIntegration.Interfaces
{
    public interface IRoleApiClient
    {
        Task<ApiResult<List<RoleVm>>> GetAll();
    }
}
