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
    [AbpAuthorize(AppPermissions.Pages_Ckd_Warehouse_StockReceiving_View)]
    public class InvCkdStockReceivingAppService : prodAppServiceBase, IInvCkdStockReceivingAppService
    {
        private readonly IDapperRepository<InvCkdStockReceiving, long> _dapperRepo;
        private readonly IRepository<InvCkdStockReceiving, long> _repo;
        private readonly IInvCkdStockReceivingExcelExporter _stockReceivingListExcelExporter;
        public InvCkdStockReceivingAppService(IRepository<InvCkdStockReceiving, long> repo,
                                         IDapperRepository<InvCkdStockReceiving, long> dapperRepo,
                                        IInvCkdStockReceivingExcelExporter stockReceivingListExcelExporter
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _stockReceivingListExcelExporter = stockReceivingListExcelExporter;
        }


        public async Task<PagedResultDto<InvCkdStockReceivingDto>> GetAll(GetInvCkdStockReceivingInput input)
        {
            string _sql = "Exec INV_CKD_STOCK_RECEIVING_GET @p_part_no, @p_color_sfx, @p_workingdatefrom, @p_workingdateto, @p_supplier_no, @p_container_no, @p_invoice_no, @p_cfc";
            IEnumerable<InvCkdStockReceivingDto> result = await _dapperRepo.QueryAsync<InvCkdStockReceivingDto>(_sql, new
            {
                p_part_no = input.PartNo,
                p_color_sfx = input.ColorSfx,
                p_workingdatefrom = input.WorkingDateFrom,
                p_workingdateto = input.WorkingDateTo,
                p_supplier_no = input.SupplierNo,
                p_container_no = input.ContainerNo,
                p_invoice_no = input.InvoiceNo,
                p_Cfc = input.Cfc
            });

            var listResult = result.ToList();

            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var totalCount = result.ToList().Count();

            if (listResult.Count > 0) listResult[0].GrandTotal = listResult.Sum(e => e.Qty);

            return new PagedResultDto<InvCkdStockReceivingDto>(
                 totalCount,
                 pagedAndFiltered);
        }


        public async Task<FileDto> GetStockReceivingToExcel(GetInvCkdStockReceivingInput input)
        {
            string _sql = "Exec INV_CKD_STOCK_RECEIVING_GET @p_part_no, @p_color_sfx, @p_workingdatefrom, @p_workingdateto, @p_supplier_no, @p_container_no, @p_invoice_no, @p_cfc";
            IEnumerable<InvCkdStockReceivingDto> result = await _dapperRepo.QueryAsync<InvCkdStockReceivingDto>(_sql, new
            {
                p_part_no = input.PartNo,
                p_color_sfx = input.ColorSfx,
                p_workingdatefrom = input.WorkingDateFrom,
                p_workingdateto = input.WorkingDateTo,
                p_supplier_no = input.SupplierNo,
                p_container_no = input.ContainerNo,
                p_invoice_no = input.InvoiceNo,
                p_Cfc = input.Cfc
            });

            var exportToExcel = result.ToList();
            return _stockReceivingListExcelExporter.ExportToFile(exportToExcel);

        }

        public async Task<FileDto> GetStockReceivingByMaterialToExcel(GetInvCkdStockReceivingInput input)
        {
            string _sql = "Exec INV_CKD_STOCK_RECEIVING_BY_MATERIAL_GET @p_part_no, @p_color_sfx, @p_workingdatefrom, @p_workingdateto, @p_supplier_no, @p_container_no, @p_invoice_no, @p_cfc";
            IEnumerable<InvCkdStockPartByMaterialDto> result = await _dapperRepo.QueryAsync<InvCkdStockPartByMaterialDto>(_sql, new
            {
                p_part_no = input.PartNo,
                p_color_sfx = input.ColorSfx,
                p_workingdatefrom = input.WorkingDateFrom,
                p_workingdateto = input.WorkingDateTo,
                p_supplier_no = input.SupplierNo,
                p_container_no = input.ContainerNo,
                p_invoice_no = input.InvoiceNo,
                p_Cfc = input.Cfc
            });

            var exportToExcel = result.ToList();
            return _stockReceivingListExcelExporter.ExportByMaterialToFile(exportToExcel);

        }

        public async Task<PagedResultDto<InvCkdStockReceivingValidateDto>> GetCheckValidateStockReceiving(PagedAndSortedResultRequestDto input)
        {
            string _sqlsearch = "Exec INV_CKD_STOCK_RECEIVING_VALIDATE";

            IEnumerable<InvCkdStockReceivingValidateDto> result = await _dapperRepo.QueryAsync<InvCkdStockReceivingValidateDto>(_sqlsearch);

            var listResult = result.ToList();

            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var totalCount = result.ToList().Count();

            return new PagedResultDto<InvCkdStockReceivingValidateDto>(
                 totalCount,
                 pagedAndFiltered);
        }
        public async Task<FileDto> GetValidateStockReceivingToExcel()
        {
            string _sql = "Exec INV_CKD_STOCK_RECEIVING_VALIDATE";

            IEnumerable<InvCkdStockReceivingValidateDto> result = await _dapperRepo.QueryAsync<InvCkdStockReceivingValidateDto>(_sql, new { });
            var exportValidateToExcel = result.ToList();
            return _stockReceivingListExcelExporter.ExportValidateToFile(exportValidateToExcel);

        }

    }
}
