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
using NPOI.POIFS.Properties;
using NUglify.Helpers;
using prod;
using prod.Authorization;
using prod.Dto;
using prod.EntityFrameworkCore;
using prod.Inventory.CKD.Invoice.Dto;
using prod.Inventory.CPS;
using prod.Inventory.CPS.Dto;
using prod.Inventory.CPS.Exporting;
using prod.Inventory.Invoice.Dto;
using prod.Master.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace prod.Inventory.CPS
{
      [AbpAuthorize(AppPermissions.Pages_CpsLinkAge_InvoiceHeaders_View)]
    public class InvCpsInvoiceHeadersAppService : prodAppServiceBase, IInvCpsInvoiceHeadersAppService
    {
        private readonly IDapperRepository<InvCpsInvoiceHeaders, long> _dapperRepo;
        private readonly IDapperRepository<InvCpsInvoiceLines, long> _dapperRepoLines;
        private readonly IRepository<InvCpsInvoiceHeaders, long> _repo;
        private readonly IDapperRepository<MstInvCpsInventoryGroup, long> _cbxInventoryGroup;
        private readonly IDapperRepository<MstInvCpsSuppliers, long> _cbxSupplier;
        private readonly IInvCpsInvoiceHeadersExcelExporter _calendarListExcelExporter;

        public InvCpsInvoiceHeadersAppService(IRepository<InvCpsInvoiceHeaders, long> repo,
                                         IDapperRepository<InvCpsInvoiceHeaders, long> dapperRepo,
                                         IDapperRepository<InvCpsInvoiceLines, long> dapperRepoLines,
                                         IDapperRepository<MstInvCpsInventoryGroup, long> cbxInventoryGroup,
                                         IDapperRepository<MstInvCpsSuppliers, long> cbxSupplier,
                                         IInvCpsInvoiceHeadersExcelExporter calendarListExcelExporter
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _dapperRepoLines = dapperRepoLines;
            _cbxInventoryGroup = cbxInventoryGroup;
            _cbxSupplier = cbxSupplier;
            _calendarListExcelExporter = calendarListExcelExporter;
        }

        public async Task<PagedResultDto<InvCpsInvoiceHeadersGrid>> GetInvoiceHeadersSearch(GetInvCpsInvoiceHeadersInput input)
        {
            string _sql = "Exec INV_CPS_INVOICE_HEADERS_SEARCH @p_po_num, @p_invetory_group_id, @p_vendor_id, " +
                "@p_from_date, @p_to_date, @p_invoice_no, @p_invoice_symbol, @p_part_no";

            IEnumerable<InvCpsInvoiceHeadersGrid> result = await _dapperRepo.QueryAsync<InvCpsInvoiceHeadersGrid>(_sql, new
            {
                p_po_num = input.PoNumber,
                p_invetory_group_id = input.InventoryGroup,
                p_vendor_id = input.Supplier,
                p_from_date = input.CreationTimeFrom,
                p_to_date = input.CreationTimeTo,
                p_invoice_no = input.InvoiceNo,
                p_invoice_symbol = input.InvoiceSymbol,
                p_part_no = input.PartNo
            });

            var listResult = result.ToList();

            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var totalCount = result.ToList().Count();

            return new PagedResultDto<InvCpsInvoiceHeadersGrid>(
                totalCount,
                pagedAndFiltered);
        }

        public async Task<PagedResultDto<InvCpsInvoiceLinesDtoGrid>> GetInvoiceLinesGetByInvoiceId(GetInvCpsInvoiceLinesInput input)
        {
            string _sql = "Exec INV_CPS_INVOICE_LINES_GETBYID  @p_id";

            IEnumerable<InvCpsInvoiceLinesDtoGrid> result = await _dapperRepo.QueryAsync<InvCpsInvoiceLinesDtoGrid>(_sql, new
            {
                p_id = input.InvoiceId
            });

            var listResult = result.ToList();

            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var totalCount = result.ToList().Count();

            return new PagedResultDto<InvCpsInvoiceLinesDtoGrid>(
                totalCount,
                pagedAndFiltered);
        }

        public async Task<FileDto> GetInvoiceHeadersToExcel(InvCpsInvoiceHeadersExportInput input)
        {
            string _sql = "Exec INV_CPS_INVOICE_HEADERS_SEARCH @p_po_num, @p_invetory_group_id, @p_vendor_id, " +
               "@p_from_date, @p_to_date, @p_invoice_no, @p_invoice_symbol, @p_part_no";

            IEnumerable<InvCpsInvoiceHeadersGrid> result = await _dapperRepo.QueryAsync<InvCpsInvoiceHeadersGrid>(_sql, new
            {
                p_po_num = input.PoNumber,
                p_invetory_group_id = input.InventoryGroup,
                p_vendor_id = input.Supplier,
                p_from_date = input.CreationTimeFrom,
                p_to_date = input.CreationTimeTo,
                p_invoice_no = input.InvoiceNo,
                p_invoice_symbol = input.InvoiceSymbol,
                p_part_no = input.PartNo
            });
            var exportToExcel = result.ToList();
            return _calendarListExcelExporter.ExportToFileHeaders(exportToExcel);
        }

        public async Task<List<CbxInventoryGroup>> GetCbxInventoryGroup()
        {
            string _sqlSearch = "Exec MST_INV_CPS_INVENTORY_GROUP_GETALL";

            IEnumerable<CbxInventoryGroup> _result = await _cbxInventoryGroup.QueryAsync<CbxInventoryGroup>(_sqlSearch);
            return _result.ToList();
        }

        public async Task<List<CbxSupplier>> GetCbxSupplier()
        {
            string _sqlSearch = "Exec MST_INV_CPS_SUPPLIERS_GETALL";

            IEnumerable<CbxSupplier> _result = await _cbxSupplier.QueryAsync<CbxSupplier>(_sqlSearch);
            return _result.ToList();
        }

        public async Task<FileDto> GetInvoiceLinesToExcel(InvCpsInvoiceLinesExportInput input)
        {
            string _sql = "Exec INV_CPS_INVOICE_LINES_GETBYID  @p_id";

            IEnumerable<InvCpsInvoiceLinesDtoGrid> result = await _dapperRepoLines.QueryAsync<InvCpsInvoiceLinesDtoGrid>(_sql, new
            {
                p_id = input.InvoiceId
            });
            var exportToExcel = result.ToList();
            return _calendarListExcelExporter.ExportToFileLines(exportToExcel);
        }
        // public async Task GenerateAsync()
        //  {
        //    await _dapperRepo.ExecuteAsync(InvCpsInvoiceHeadersConsts.SP_MST_WPT_CALENDAR_GENERATE);
        // }

    }
}