using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Dapper.Repositories;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Microsoft.EntityFrameworkCore;
using NPOI.SS.Formula.Functions;
using prod.Authorization;
using prod.Dto;
using prod.Master.Inv.Dto;
using prod.Master.Inv.Exporting;
using prod.Master.Inventory.Dto;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace prod.Master.Inventory
{
    [AbpAuthorize(AppPermissions.Pages_Master_Inventory_SupplierList_View)]
    public class MstInvSupplierListAppService : prodAppServiceBase, IMstInvSupplierListAppService
    {
        private readonly IDapperRepository<MstInvSupplierList, long> _dapperRepo;
        private readonly IRepository<MstInvSupplierList, long> _repo;
        private readonly IMstInvSupplierListExcelExporter _calendarListExcel;


        public MstInvSupplierListAppService(IRepository<MstInvSupplierList, long> repo,
                                         IDapperRepository<MstInvSupplierList, long> dapperRepo,
                                         IMstInvSupplierListExcelExporter calendarListExcel
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcel = calendarListExcel;

        }

    public async Task<PagedResultDto<MstInvSupplierListDto>> GetAll(GetMstInvSupplierListInput input)
        {
            var filtered = _repo.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.SupplierNo), e => e.SupplierNo.Contains(input.SupplierNo))
                .WhereIf(!string.IsNullOrWhiteSpace(input.SupplierName), e => e.SupplierName.Contains(input.SupplierName))
                .WhereIf(!string.IsNullOrWhiteSpace(input.Remarks), e => e.Remarks.Contains(input.Remarks))
                .WhereIf(!string.IsNullOrWhiteSpace(input.SupplierType), e => e.SupplierType.Contains(input.SupplierType))
                .WhereIf(!string.IsNullOrWhiteSpace(input.SupplierNameVn), e => e.SupplierNameVn.Contains(input.SupplierNameVn))
                .WhereIf(!string.IsNullOrWhiteSpace(input.Exporter), e => e.Exporter.Contains(input.Exporter))
                ;
            var pageAndFiltered = filtered.OrderBy(s => s.Id);


            var system = from o in pageAndFiltered
                         select new MstInvSupplierListDto
                         {
                             Id = o.Id,
                             SupplierNo = o.SupplierNo,
                             SupplierName = o.SupplierName,
                             Remarks = o.Remarks,
                             SupplierType = o.SupplierType,
                             SupplierNameVn = o.SupplierNameVn,
                             Exporter = o.Exporter,
                         };

            var totalCount = await filtered.CountAsync();
            var paged = system.PageBy(input);

            return new PagedResultDto<MstInvSupplierListDto>(
                totalCount,
                 await paged.ToListAsync()
            );
        }

    public async Task<FileDto> GetGpsSupplierListToExcel(MstInvSupplierListInput input)
    {
        var filtered =_repo.GetAll()
           .WhereIf(!string.IsNullOrWhiteSpace(input.SupplierNo), e => e.SupplierNo.Contains(input.SupplierNo))
                .WhereIf(!string.IsNullOrWhiteSpace(input.SupplierName), e => e.SupplierName.Contains(input.SupplierName))
                .WhereIf(!string.IsNullOrWhiteSpace(input.Remarks), e => e.Remarks.Contains(input.Remarks))
                .WhereIf(!string.IsNullOrWhiteSpace(input.SupplierType), e => e.SupplierType.Contains(input.SupplierType))
                .WhereIf(!string.IsNullOrWhiteSpace(input.SupplierNameVn), e => e.SupplierNameVn.Contains(input.SupplierNameVn))
                .WhereIf(!string.IsNullOrWhiteSpace(input.Exporter), e => e.Exporter.Contains(input.Exporter))
                ;
        var pageAndFiltered = filtered.OrderBy(s => s.Id);


        var query = from o in pageAndFiltered
                    select new MstInvSupplierListDto
                    {
                        Id = o.Id,
                        SupplierNo = o.SupplierNo,
                        SupplierName = o.SupplierName,
                        Remarks = o.Remarks,
                        SupplierType = o.SupplierType,
                        SupplierNameVn = o.SupplierNameVn,
                        Exporter = o.Exporter,
                    };
        var exportToExcel = await query.ToListAsync();
        return _calendarListExcel.ExportToFile(exportToExcel);
    }
    
    }
}
