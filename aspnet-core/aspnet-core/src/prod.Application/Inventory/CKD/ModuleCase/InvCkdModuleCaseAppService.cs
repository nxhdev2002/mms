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
    [AbpAuthorize(AppPermissions.Pages_Ckd_Warehouse_ModuleCase_View)]
    public class InvCkdModuleCaseAppService : prodAppServiceBase, IInvCkdModuleCaseAppService
    {
        private readonly IDapperRepository<InvCkdModuleCase, long> _dapperRepo;
        private readonly IRepository<InvCkdModuleCase, long> _repo;
        private readonly IRepository<InvCkdContainerList, long> _repoContainerList;
        private readonly IInvCkdModuleCaseExcelExporter _invCkdModuleCaseListExcelExporter;

        public InvCkdModuleCaseAppService(IRepository<InvCkdModuleCase, long> repo,
                                        IRepository<InvCkdContainerList, long> repoContainerList,
                                         IDapperRepository<InvCkdModuleCase, long> dapperRepo,
                                        IInvCkdModuleCaseExcelExporter invCkdModuleCaseListExcelExporter
            )
        {
            _repo = repo;
            _repoContainerList = repoContainerList;
            _dapperRepo = dapperRepo;
            _invCkdModuleCaseListExcelExporter = invCkdModuleCaseListExcelExporter;
        }



        public async Task<PagedResultDto<InvCkdModuleCaseDto>> GetAll(GetInvCkdModuleCaseInput input)
        {
            string _sql = "Exec INV_CKD_MODULE_CASE_SEARCH @p_module_case, @p_container_no, @p_invoice_no, " +
               "@p_renban, @p_supplier_no, @p_devan_from, @p_devan_to, @p_unpack_from, " +
               "@p_unpack_to, @p_storage_code, @p_radio, @p_bill_date_from, @p_bill_date_to,@p_CkdPio, @p_OrderTypeCode, @p_BillNo,@p_lotno";

            IEnumerable<InvCkdModuleCaseDto> result = await _dapperRepo.QueryAsync<InvCkdModuleCaseDto>(_sql, new
            {
                p_module_case = input.ModuleCaseNo,
                p_container_no = input.ContainerNo,
                p_invoice_no = input.InvoiceNo,
                p_renban = input.Renban,
                p_supplier_no = input.SupplierNo,
                p_devan_from = input.DevanningFromDate,
                p_devan_to = input.DevanningToDate,
                p_unpack_from = input.UnpackingFromDate,
                p_unpack_to = input.UnpackingToDate,
                p_storage_code = input.StorageLocationCode,
                p_radio = input.radio,
                p_bill_date_from = input.BillDateFrom,
                p_bill_date_to = input.BillDateTo,
                p_CkdPio = input.CkdPio,
                p_OrderTypeCode = input.OrderTypeCode,
                p_BillNo = input.BillNo,
                p_lotno = input.LotNo,

            });

            var listResult = result.ToList();


            var totalCount = result.ToList().Count();
            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            return new PagedResultDto<InvCkdModuleCaseDto>(
                totalCount,
                pagedAndFiltered);

        }

        public async Task<FileDto> GetModuleCaseToExcel(InvCkdModuleCaseExportInput input)
        {
            string _sql = "Exec INV_CKD_MODULE_CASE_SEARCH @p_module_case, @p_container_no, @p_invoice_no, " +
                "@p_renban, @p_supplier_no, @p_devan_from, @p_devan_to, @p_unpack_from, " +
                "@p_unpack_to, @p_storage_code, @p_radio, @p_bill_date_from, @p_bill_date_to,@p_CkdPio, @p_OrderTypeCode, @p_BillNo,@p_lotno";

            IEnumerable<InvCkdModuleCaseDto> result = await _dapperRepo.QueryAsync<InvCkdModuleCaseDto>(_sql, new
            {
                p_module_case = input.ModuleCaseNo,
                p_container_no = input.ContainerNo,
                p_invoice_no = input.InvoiceNo,
                p_renban = input.Renban,
                p_supplier_no = input.SupplierNo,
                p_devan_from = input.DevanningFromDate,
                p_devan_to = input.DevanningToDate,
                p_unpack_from = input.UnpackingFromDate,
                p_unpack_to = input.UnpackingToDate,
                p_storage_code = input.StorageLocationCode,
                p_radio = input.radio,
                p_bill_date_from = input.BillDateFrom,
                p_bill_date_to = input.BillDateTo,
                p_CkdPio = input.CkdPio,
                p_OrderTypeCode = input.OrderTypeCode,
                p_BillNo = input.BillNo,
                p_lotno = input.LotNo,
            });

            var exportToExcel = result.ToList();
            return _invCkdModuleCaseListExcelExporter.ExportToFile(exportToExcel);
        }





    }
}

