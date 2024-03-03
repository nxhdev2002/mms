using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Dapper.Repositories;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.EntityFrameworkCore.Uow;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using Dapper;
using Microsoft.EntityFrameworkCore;
using NUglify.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using prod.Authorization;
using prod.Dto;
using prod.EntityFrameworkCore;
using prod.LogA.Ekb.Dto;

namespace prod.LogA.Ekb
{ 
    public class LgaEkbEkanbanProgressScreenAppService : prodAppServiceBase, ILgaEkbEkanbanProgressScreenAppService
    {
        private readonly IDapperRepository<LgaEkbProgress, long> _dapperRepo; 

        public LgaEkbEkanbanProgressScreenAppService(IDapperRepository<LgaEkbProgress, long> dapperRepo)
        { 
            _dapperRepo = dapperRepo; 
        } 
	 

        public async Task<List<LgaEkbEkanbanProgressScreenDto>> GetConfigScreen(string prod_line)
        {
            string _sql = "Exec LGA_EKB_EKANBAN_PROGRESS_MONITOR_GET_SCREEN_CONFIG  @prod_line";

            var filtered = await _dapperRepo.QueryAsync<LgaEkbEkanbanProgressScreenDto>(_sql, new {
                prod_line = prod_line
            });

            return filtered.ToList();
        }

        public async Task<List<LgaEkbEkanbanProgressScreenDto>> GetDataEkabanProgressScreen(string prod_line)
        {
            string _sql = "Exec LGA_EKB_EKANBAN_PROGRESS_MONITOR_GET_DATA  @prod_line";

            var filtered = await _dapperRepo.QueryAsync<LgaEkbEkanbanProgressScreenDto>(_sql, new
            {
                prod_line = prod_line
            });

            return filtered.ToList();
        }

    }
}
