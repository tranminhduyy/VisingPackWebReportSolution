using VisingPackSolution.ViewModels.Common;
using VisingPackSolution.ViewModels.System.Languages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VisingPackSolution.ApiIntegration.Interfaces
{
    public interface ILanguageApiClient
    {
        Task<ApiResult<List<LanguageVm>>> GetAll();
    }
}