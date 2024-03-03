using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Dapper.Repositories;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.EntityFrameworkCore.Uow;
using Abp.Linq.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using prod.Authorization;
using prod.Dto;
using prod.EntityFrameworkCore;
using prod.LogA.Pcs.Dto;
using prod.LogA.Pcs.Exporting;
using prod.LogA.Pcs.Stock;

namespace prod.LogA.Pcs
{
    //  [AbpAuthorize(AppPermissions.Pages_LogA_Pcs_Stock)]
    public class LgaPcsStockAppService : prodAppServiceBase, ILgaPcsStockAppService
    {
        private readonly IDapperRepository<LgaPcsStock, long> _dapperRepo;
        private readonly IRepository<LgaPcsStock, long> _repo;
        private readonly ILgaPcsStockExcelExporter _calendarListExcelExporter;

        public LgaPcsStockAppService(IRepository<LgaPcsStock, long> repo,
                                         IDapperRepository<LgaPcsStock, long> dapperRepo,
                                        ILgaPcsStockExcelExporter calendarListExcelExporter
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
        }

        //  [AbpAuthorize(AppPermissions.Pages_LogA_Pcs_Stock_Edit)]
        public async Task CreateOrEdit(CreateOrEditLgaPcsStockDto input)
        {
            if (input.Id == null) await Create(input);
            else await Update(input);
        }

        //CREATE
        private async Task Create(CreateOrEditLgaPcsStockDto input)
        {
            var mainObj = ObjectMapper.Map<LgaPcsStock>(input);

            await CurrentUnitOfWork.GetDbContext<prodDbContext>().AddAsync(mainObj);
        }

        // EDIT
        private async Task Update(CreateOrEditLgaPcsStockDto input)
        {
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                var mainObj = await _repo.GetAll()
                .FirstOrDefaultAsync(e => e.Id == input.Id);

                var mainObjToUpdate = ObjectMapper.Map(input, mainObj);
            }
        }

        //  [AbpAuthorize(AppPermissions.Pages_LogA_Pcs_Stock_Delete)]
        public async Task Delete(EntityDto input)
        {
            var mainObj = await _repo.FirstOrDefaultAsync(input.Id);
            _repo.HardDelete(mainObj);
            CurrentUnitOfWork.GetDbContext<prodDbContext>().Remove(mainObj);
        }

        public async Task<PagedResultDto<LgaPcsStockDto>> GetAll(GetLgaPcsStockInput input)
        {
            var filtered = _repo.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.PartNo), e => e.PartNo.Contains(input.PartNo))
                .WhereIf(!string.IsNullOrWhiteSpace(input.PartName), e => e.PartName.Contains(input.PartName))
                .WhereIf(!string.IsNullOrWhiteSpace(input.SupplierNo), e => e.SupplierNo.Contains(input.SupplierNo))
                .WhereIf(!string.IsNullOrWhiteSpace(input.BackNo), e => e.BackNo.Contains(input.BackNo))
                .WhereIf(!string.IsNullOrWhiteSpace(input.PcRackAddress), e => e.PcRackAddress.Contains(input.PcRackAddress))
                .WhereIf(!string.IsNullOrWhiteSpace(input.OutType), e => e.OutType.Contains(input.OutType))
                .WhereIf(!string.IsNullOrWhiteSpace(input.IsActive), e => e.IsActive.Contains(input.IsActive))
                ;
            var pageAndFiltered = filtered.OrderBy(s => s.Id);


            var system = from o in pageAndFiltered
                         select new LgaPcsStockDto
                         {
                             Id = o.Id,
                             PartNo = o.PartNo,
                             PartName = o.PartName,
                             SupplierNo = o.SupplierNo,
                             BackNo = o.BackNo,
                             PcRackAddress = o.PcRackAddress,
                             UsagePerHour = o.UsagePerHour,
                             RackCapBox = o.RackCapBox,
                             OutType = o.OutType,
                             StockQty = o.StockQty,
                             BoxQty = o.BoxQty,
                             IsActive = o.IsActive,
                         };

            var totalCount = await filtered.CountAsync();
            var paged = system.PageBy(input);

            return new PagedResultDto<LgaPcsStockDto>(
                totalCount,
                 await paged.ToListAsync()
            );
        }


        public async Task<FileDto> GetStockToExcel(LgaPcsStockExportInput input)
        {
            var query = from o in _repo.GetAll()
                        select new LgaPcsStockDto
                        {
                            Id = o.Id,
                            PartNo = o.PartNo,
                            PartName = o.PartName,
                            SupplierNo = o.SupplierNo,
                            BackNo = o.BackNo,
                            PcRackAddress = o.PcRackAddress,
                            UsagePerHour = o.UsagePerHour,
                            RackCapBox = o.RackCapBox,
                            OutType = o.OutType,
                            StockQty = o.StockQty,
                            BoxQty = o.BoxQty,
                            IsActive = o.IsActive,
                        };
            var exportToExcel = await query.ToListAsync();
            return _calendarListExcelExporter.ExportToFile(exportToExcel);
        }

        // public async Task GenerateAsync()
        //  {
        //    await _dapperRepo.ExecuteAsync(LgaPcsStockConsts.SP_MST_WPT_CALENDAR_GENERATE);
        // }

    }
}
