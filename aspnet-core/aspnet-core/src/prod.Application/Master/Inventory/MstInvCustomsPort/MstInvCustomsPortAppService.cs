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
using prod.Master.Inventory.Dto;
using prod.Master.Inventory.Exporting;

namespace prod.Master.Inventory
{
    [AbpAuthorize(AppPermissions.Pages_Master_Ckd_CustomsPort_View)]
    public class MstInvCustomsPortAppService : prodAppServiceBase, IMstInvCustomsPortAppService
    {
        private readonly IDapperRepository<MstInvCustomsPort, long> _dapperRepo;
        private readonly IRepository<MstInvCustomsPort, long> _repo;
        private readonly IMstInvCustomsPortExcelExporter _calendarListExcelExporter;

        public MstInvCustomsPortAppService(IRepository<MstInvCustomsPort, long> repo,
                                         IDapperRepository<MstInvCustomsPort, long> dapperRepo,
                                        IMstInvCustomsPortExcelExporter calendarListExcelExporter
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
        }

        public async Task<PagedResultDto<MstInvCustomsPortDto>> GetAll(GetMstInvCustomsPortInput input)
        {
            var filtered = _repo.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.Code), e => e.Code.Contains(input.Code))
                .WhereIf(!string.IsNullOrWhiteSpace(input.Name), e => e.Name.Contains(input.Name));

            var pageAndFiltered = filtered.OrderBy(s => s.Id);


            var system = from o in pageAndFiltered
                         select new MstInvCustomsPortDto
                         {
                             Id = o.Id,
                             Code = o.Code,
                             Name = o.Name,
                             AccountNumber = o.AccountNumber,
                             BankName = o.BankName,
                             VendorNumber = o.VendorNumber,
                             IsActive = o.IsActive,
                         };

            var totalCount = await filtered.CountAsync();
            var paged = system.PageBy(input);

            return new PagedResultDto<MstInvCustomsPortDto>(
                totalCount,
                 await paged.ToListAsync()
            );
        }


        public async Task<FileDto> GetCustomsPortToExcel(MstInvCustomsPortExportInput input)
        {
            var filtered = _repo.GetAll()
               .WhereIf(!string.IsNullOrWhiteSpace(input.Code), e => e.Code.Contains(input.Code))
               .WhereIf(!string.IsNullOrWhiteSpace(input.Name), e => e.Name.Contains(input.Name));

            var pageAndFiltered = filtered.OrderBy(s => s.Id);

            var query = from o in pageAndFiltered
                        select new MstInvCustomsPortDto
                        {
                            Id = o.Id,
                            Code = o.Code,
                            Name = o.Name,
                            AccountNumber = o.AccountNumber,
                            BankName = o.BankName,
                            VendorNumber = o.VendorNumber,
                            IsActive = o.IsActive,
                        };
            var exportToExcel = await query.ToListAsync();
            return _calendarListExcelExporter.ExportToFile(exportToExcel);
        }


    }
}
