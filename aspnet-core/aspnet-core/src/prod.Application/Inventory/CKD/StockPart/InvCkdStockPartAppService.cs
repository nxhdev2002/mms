using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Dapper.Repositories;
using Abp.Domain.Repositories;
using prod.Authorization;
using prod.Dto;
using prod.Inventory.CKD.Dto;
using prod.Inventory.CKD.Exporting;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace prod.Inventory.CKD
{
    [AbpAuthorize(AppPermissions.Pages_Ckd_Warehouse_StockPart_View)]
    public class InvCkdStockPartAppService : prodAppServiceBase, IInvCkdStockPartAppService
    {
        private readonly IDapperRepository<InvCkdStockPart, long> _dapperRepo;
        private readonly IRepository<InvCkdStockPart, long> _repo;
        private readonly IInvCkdStockPartExcelExporter _calendarListExcelExporter;

        public InvCkdStockPartAppService(IRepository<InvCkdStockPart, long> repo,
                                         IDapperRepository<InvCkdStockPart, long> dapperRepo,
                                        IInvCkdStockPartExcelExporter calendarListExcelExporter
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
        }

        public async Task<PagedResultDto<InvCkdStockPartDto>> GetAll(GetInvCkdStockPartInput input)
        {
            string _sql = "Exec INV_CKD_STOCK_PART_GETS @p_PartNo, @p_ColorSfx, @p_WorkingDate, @p_NegativeStock ,@p_Cfc, @p_SupplierNo";

            IEnumerable<InvCkdStockPartDto> result = await _dapperRepo.QueryAsync<InvCkdStockPartDto>(_sql, new
            {
                p_PartNo = input.PartNo,
                p_Cfc = input.Cfc,
                p_SupplierNo = input.SupplierNo,
                p_ColorSfx = input.ColorSfx,
                p_WorkingDate = input.WorkingDate,
                p_NegativeStock = input.NegativeStock
            });

            var listResult = result.ToList();

            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var totalCount = result.ToList().Count();

            if (listResult.Count > 0) listResult[0].GrandTotal = listResult.Sum(e => e.Qty);

            return new PagedResultDto<InvCkdStockPartDto>(
                totalCount,
                pagedAndFiltered);
        }


        public async Task<FileDto> GetStockPartToExcel(InvCkdStockPartExportInput input)
        {
            string _sql = "Exec INV_CKD_STOCK_PART_GETS @p_PartNo, @p_ColorSfx, @p_WorkingDate, @p_NegativeStock ,@p_Cfc, @p_SupplierNo";

            IEnumerable<InvCkdStockPartDto> result = await _dapperRepo.QueryAsync<InvCkdStockPartDto>(_sql, new
            {
                p_PartNo = input.PartNo,
                p_Cfc = input.Cfc,
                p_SupplierNo = input.SupplierNo,
                p_ColorSfx = input.ColorSfx,
                p_WorkingDate = input.WorkingDate,
                p_NegativeStock = input.NegativeStock
            });

            var exportToExcel = result.ToList();
            return _calendarListExcelExporter.ExportToFile(exportToExcel);
        }

        public async Task<FileDto> GetStockPartByMaterialToExcel(InvCkdStockPartExportInput input)
        {
            string _sql = "Exec INV_CKD_STOCK_PART_BY_MATERIAL_GETS @p_PartNo, @p_ColorSfx, @p_WorkingDate, @p_NegativeStock ,@p_Cfc, @p_SupplierNo";

            IEnumerable<InvCkdStockPartByMaterialDto> result = await _dapperRepo.QueryAsync<InvCkdStockPartByMaterialDto>(_sql, new
            {
                p_PartNo = input.PartNo,
                p_Cfc = input.Cfc,
                p_SupplierNo = input.SupplierNo,
                p_ColorSfx = input.ColorSfx,
                p_WorkingDate = input.WorkingDate,
                p_NegativeStock = input.NegativeStock
            });

            var exportToExcel = result.ToList();
            return _calendarListExcelExporter.ExportByMaterialToFile(exportToExcel);
        }

        //get messError Import
        public async Task<PagedResultDto<InvCkdStockReceivingDto>> GetMessageError(PagedAndSortedResultRequestDto input)
        {
            var data = await _dapperRepo.QueryAsync<InvCkdStockReceivingDto>("Exec INV_CKD_STOCK_PART_CHECK_STOCK", new { });
            var rsData = from o in data
                         select new InvCkdStockReceivingDto
                         {
                             Id = o.Id,
                             PartNo = o.PartNo,
                             PartName = o.PartName,
                             ErrDes = o.ErrDes,

                         };

            var totalCount = rsData.Count();
            return new PagedResultDto<InvCkdStockReceivingDto>(
                totalCount,
                 rsData.ToList()
            );
        }

        public async Task<PagedResultDto<InvCkdStockReceivingDto>> GetCheckStockPart(GetCheckStockPart input)
        {
            var data = await _dapperRepo.QueryAsync<InvCkdStockReceivingDto>("Exec INV_CKD_STOCK_PART_CHECK_STOCK", new { });

            var pagedAndFiltered = data.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
            var totalCount = data.Count();


            return new PagedResultDto<InvCkdStockReceivingDto>(
                totalCount,
                 pagedAndFiltered
            );
        }

        public async Task<FileDto> GetCheckStockPartToExcel()
        {
            string _sql = "Exec INV_CKD_STOCK_PART_CHECK_STOCK ";

            IEnumerable<InvCkdStockReceivingDto> result = await _dapperRepo.QueryAsync<InvCkdStockReceivingDto>(_sql, new { });

            var exportToExcel = result.ToList();
            return _calendarListExcelExporter.ExportToFileCheckStock(exportToExcel);
        }


        public async Task EditCKDStockPart(long? id, int? qty, bool isactive)
        {
            string _sql = "Exec INV_CKD_STOCK_PART_EDIT @p_Id, @p_Qty, @p_IsActive, @p_UserId";

            await _dapperRepo.ExecuteAsync(_sql, new 
            {
                p_Id = id,
                p_Qty = qty,
                p_IsActive = isactive,
                p_UserId = AbpSession.UserId
            });
        }
    }
}
