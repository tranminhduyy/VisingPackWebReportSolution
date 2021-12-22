using VisingPackSolution.Application.System.Languages;
using VisingPackSolution.Data.EF;
using VisingPackSolution.Data;
using VisingPackSolution.ViewModels.Common;
using VisingPackSolution.ViewModels.System.Languages;
using VisingPackSolution.ViewModels.System.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace VisingPackSolution.Application.System.Languages
{
    public class LanguageService : ILanguageService
    {
        private readonly IConfiguration _config;
        private readonly VisingPackMMSDbContext _context;

        public LanguageService(VisingPackMMSDbContext context,
            IConfiguration config)
        {
            _config = config;
            _context = context;
        }

        public async Task<ApiResult<List<LanguageVm>>> GetAll()
        {
            //var languages1 = await _context.AppRoles.Select(x => new LanguageVm()
            //{
            //    Id = x.Id,
            //    Name = x.Name,
            //    IsDefault = x.IsDefault
            //})
            var language1 = new LanguageVm()
            {
                Id = "1",
                Name = "en-EN",
                IsDefault = true
            };
            var language2 = new LanguageVm()
            {
                Id = "2",
                Name = "vn-VN",
                IsDefault = false
            };
            List<LanguageVm> lst = new List<LanguageVm> { language1, language2 };
            return new ApiSuccessResult<List<LanguageVm>>(lst);
        }
    }
}