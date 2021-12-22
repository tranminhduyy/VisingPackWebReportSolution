using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VisingPackSolution.ViewModels.System.Roles;

namespace VisingPackSolution.Application.System.Roles
{
    public interface IRoleService
    {
        Task<List<RoleVm>> GetAll();
    }
}
