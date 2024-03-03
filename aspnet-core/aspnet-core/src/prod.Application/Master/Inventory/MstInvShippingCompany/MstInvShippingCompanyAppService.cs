using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Dapper.Repositories;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Microsoft.EntityFrameworkCore;
using prod.Authorization;
using prod.Dto;
using prod.Master.Inventory.Dto;
using prod.Master.Inventory.Exporting;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace prod.Master.Inventory
{
    [AbpAuthorize(AppPermissions.Pages_Master_Ckd_ShippingCompany_View)]
    public class MstInvShippingCompanyAppService : prodAppServiceBase, IMstInvShippingCompanyAppService
    {
        private readonly IDapperRepository<MstInvShippingCompany, long> _dapperRepo;
        private readonly IRepository<MstInvShippingCompany, long> _repo;
        private readonly IMstInvShippingCompanyExcelExporter _calendarListExcelExporter;

        public MstInvShippingCompanyAppService(IRepository<MstInvShippingCompany, long> repo,
                                         IDapperRepository<MstInvShippingCompany, long> dapperRepo,
                                        IMstInvShippingCompanyExcelExporter calendarListExcelExporter
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
        }

        public async Task<PagedResultDto<MstInvShippingCompanyDto>> GetAll(GetMstInvShippingCompanyInput input)
        {
            var filtered = _repo.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.Code), e => e.Code.Contains(input.Code))
                .WhereIf(!string.IsNullOrWhiteSpace(input.Name), e => e.Name.Contains(input.Name))

                ;
            var pageAndFiltered = filtered.OrderBy(s => s.Id);


            var system = from o in pageAndFiltered
                         select new MstInvShippingCompanyDto
                         {
                             Id = o.Id,
                             Code = o.Code,
                             Name = o.Name,
                             Description = o.Description,
                             IsActive = o.IsActive,
                         };

            var totalCount = await filtered.CountAsync();
            var paged = system.PageBy(input);

            return new PagedResultDto<MstInvShippingCompanyDto>(
                totalCount,
                 await paged.ToListAsync()
            );
        }


        public async Task<FileDto> GetShippingCompanyToExcel(MstInvShippingCompanyExportInput input)
        {
            var filtered = _repo.GetAll()
            .WhereIf(!string.IsNullOrWhiteSpace(input.Code), e => e.Code.Contains(input.Code))
            .WhereIf(!string.IsNullOrWhiteSpace(input.Name), e => e.Name.Contains(input.Name));
            var pageAndFiltered = filtered.OrderBy(s => s.Id);

            var query = from o in pageAndFiltered
                        select new MstInvShippingCompanyDto
                        {
                            Id = o.Id,
                            Code = o.Code,
                            Name = o.Name,
                            Description = o.Description,
                            IsActive = o.IsActive,
                        };
            var exportToExcel = await query.ToListAsync();
            return _calendarListExcelExporter.ExportToFile(exportToExcel);
        }

    }
}
