using VisingPackSolution.ViewModels.Common;
using VisingPackSolution.ViewModels.System.Languages;
using VisingPackSolution.ViewModels.System.Users;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace VisingPackSolution.Application.System.Languages
{
    public interface ILanguageService
    {
        Task<ApiResult<List<LanguageVm>>> GetAll();
    }
}