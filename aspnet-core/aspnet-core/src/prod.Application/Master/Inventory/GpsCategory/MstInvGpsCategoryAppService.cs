using Abp.Application.Services.Dto;
using Abp.Dapper.Repositories;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Microsoft.EntityFrameworkCore;
using prod.Dto;
using prod.Master.Cmm;
using prod.Master.Common.DriveTrain.Dto;
using prod.Master.Common.DriveTrain.Exporting;
using prod.Master.Inventory.GpsCategory.Dto;
using prod.Master.Inventory.GpsCategory.Exporting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prod.Master.Inventory.GpsCategory
{
    public class MstInvGpsCategoryAppService : prodAppServiceBase,IMstInvGpsCategoryAppService
    {
        private readonly IDapperRepository<MstInvGpsCategory, long> _dapperRepo;
        private readonly IRepository<MstInvGpsCategory, long> _repo;
        private readonly IMstInvGpsCategoryExcelExporter _gpscategoryListExcelExporter;

        public MstInvGpsCategoryAppService(IRepository<MstInvGpsCategory, long> repo,
                                         IDapperRepository<MstInvGpsCategory, long> dapperRepo,
                                       IMstInvGpsCategoryExcelExporter gpscategoryListExcelExporter
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _gpscategoryListExcelExporter = gpscategoryListExcelExporter;
        }

        public async Task<PagedResultDto<MstInvGpsCategoryDto>> GetAll(GetMstInvGpsCategoryInput input)
        {
            var filtered = _repo.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.Code), e => e.Code.Contains(input.Code))
                .WhereIf(!string.IsNullOrWhiteSpace(input.Name), e => e.Name.Contains(input.Name));
            var pageAndFiltered = filtered.OrderBy(s => s.Id);


            var system = from o in pageAndFiltered
                         select new MstInvGpsCategoryDto
                         {
                             Id = o.Id,
                             Code = o.Code,
                             Name = o.Name,
                         };

            var totalCount = await filtered.CountAsync();
            var paged = system.PageBy(input);

            return new PagedResultDto<MstInvGpsCategoryDto>(
                totalCount,
                 await paged.ToListAsync()
            );
        }


        public async Task<FileDto> GetCategoryToExcel(MstInvGpsCategoryExportInput input)
        {
            var query = from o in _repo.GetAll()
                        select new MstInvGpsCategoryDto
                        {
                            Id = o.Id,
                            Code = o.Code,
                            Name = o.Name,

                        };
            var exportToExcel = await query.ToListAsync();
            return _gpscategoryListExcelExporter.ExportToFile(exportToExcel);
        }

    }
}
