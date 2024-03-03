using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Dapper.Repositories;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.EntityFrameworkCore.Uow;
using Microsoft.EntityFrameworkCore;
using prod.Authorization;
using prod.Dto;
using prod.EntityFrameworkCore;
using prod.HistoricalData;
using prod.Inventory.CKD.Dto;
using prod.Inventory.CKD.Exporting;
using prod.Master.Inventory;
using prod.Master.Inventory.ContainerStatus.Dto;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace prod.Inventory.CKD
{
    [AbpAuthorize(AppPermissions.Pages_Ckd_Intransit_ContainerInvoice_View)]
    public class InvCkdContainerInvoiceAppService : prodAppServiceBase, IInvCkdContainerInvoiceAppService
    {
        private readonly IDapperRepository<InvCkdContainerInvoice, long> _dapperRepo;
        private readonly IRepository<InvCkdContainerInvoice, long> _repo;
        private readonly IInvCkdContainerInvoiceExcelExporter _calendarListExcelExporter;
        private readonly IRepository<MstInvContainerStatus, long> _containerStatusRepo;
        private readonly IHistoricalDataAppService _historicalDataAppService;


        public InvCkdContainerInvoiceAppService(IRepository<InvCkdContainerInvoice, long> repo,
                                         IDapperRepository<InvCkdContainerInvoice, long> dapperRepo,
                                         IInvCkdContainerInvoiceExcelExporter calendarListExcelExporter,
                                         IRepository<MstInvContainerStatus, long> containerStatusRepo,
                                        IHistoricalDataAppService historicalDataAppService

            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
            _containerStatusRepo = containerStatusRepo;
            _historicalDataAppService = historicalDataAppService;
        }

        public async Task<List<string>> GetContainerInvoiceHistory(GetInvCkdPartListHistoryInput input)
        {
            return await _historicalDataAppService.GetHistoricalDataById(input.Id, input.TableName, "Id");
        }

        public async Task<FileDto> GetHistoricalDataToExcel(GetInvCkdPartListHistoryExcelInput input)
        {
            var data = await _historicalDataAppService.GetHistoricalDataById(input.Id, input.TableName, "Id");
            return _calendarListExcelExporter.ExportToHistoricalFile(data);
        }

        public async Task<List<long?>> GetChangedRecords()
        {
            return await _historicalDataAppService.GetChangedRecordIds("InvCkdContainerInvoice");
        }

        //[AbpAuthorize(AppPermissions.Pages_Ckd_Intransit_ContainerInvoice_Edit)]
        //public async Task CreateOrEdit(CreateOrEditInvCkdContainerInvoiceDto input)
        //{
        //    if (input.Id == null) await Create(input);
        //    else await Update(input);
        //}

        ////CREATE
        //private async Task Create(CreateOrEditInvCkdContainerInvoiceDto input)
        //{
        //    var mainObj = ObjectMapper.Map<InvCkdContainerInvoice>(input);

        //    await CurrentUnitOfWork.GetDbContext<prodDbContext>().AddAsync(mainObj);
        //}

        //// EDIT
        //private async Task Update(CreateOrEditInvCkdContainerInvoiceDto input)
        //{
        //    using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant))
        //    {
        //        var mainObj = await _repo.GetAll()
        //        .FirstOrDefaultAsync(e => e.Id == input.Id);

        //        var mainObjToUpdate = ObjectMapper.Map(input, mainObj);
        //    }
        //}

        //[AbpAuthorize(AppPermissions.Pages_Ckd_Intransit_ContainerInvoice_Edit)]
        //public async Task Delete(EntityDto input)
        //{
        //    var mainObj = await _repo.FirstOrDefaultAsync(input.Id);
        //    _repo.HardDelete(mainObj);
        //    CurrentUnitOfWork.GetDbContext<prodDbContext>().Remove(mainObj);
        //}

        public async Task<PagedResultDto<InvCkdContainerInvoiceDto>> GetAll(GetInvCkdContainerInvoiceInput input)
        {
            string _sql = "Exec INV_CKD_CONTAINER_INVOICE_SEARCH @p_billNo,@p_cont, @p_renban, @p_invoice, @p_seal, @p_status, @p_supplier_no, @p_bill_date_from, @p_bill_date_to, @p_CkdPio, @p_OrderTypeCode";

            IEnumerable<InvCkdContainerInvoiceDto> result =
                await _dapperRepo.QueryAsync<InvCkdContainerInvoiceDto>(_sql, new
                {
                    @p_billNo = input.BillofladingNo,
                    @p_cont = input.ContainerNo,
                    @p_renban = input.Renban,
                    @p_invoice = input.InvoiceNo,
                    @p_seal = input.SealNo,
                    @p_status = input.Status,
                    @p_supplier_no = input.SupplierNo,
                    p_bill_date_from = input.BillDateFrom,
                    p_bill_date_to = input.BillDateTo,
                    @p_CkdPio = input.CkdPio,
                    p_OrderTypeCode = input.OrderTypeCode
                });

            var listResult = result.ToList();

            if (listResult.Count > 0)
            {
                listResult[0].GrandFob = listResult.Sum(e => e.Fob);
                listResult[0].GrandFreight = listResult.Sum(e => e.Freight);
                listResult[0].GrandInsurance = listResult.Sum(e => e.Insurance);
                listResult[0].GrandTax = listResult.Sum(e => e.Tax);
                listResult[0].GrandAmount = listResult.Sum(e => e.Amount);
                listResult[0].GrandTaxVn = (long)listResult.Sum(e => (decimal?)e.TaxVnd);
                listResult[0].GrandVatVn = (long)listResult.Sum(e => (decimal?)e.VatVnd);

            }

            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var totalCount = result.ToList().Count();

            return new PagedResultDto<InvCkdContainerInvoiceDto>(
                totalCount,
                pagedAndFiltered);

        }


        public async Task<FileDto> GetContainerInvoiceToExcel(GetInvCkdContainerInvoiceInputExcel input)
        {
            string _sql = "Exec INV_CKD_CONTAINER_INVOICE_SEARCH @p_billNo, @p_cont, @p_renban, @p_invoice, @p_seal, @p_status, @p_supplier_no, @p_bill_date_from, @p_bill_date_to, @p_CkdPio, @p_OrderTypeCode";

            IEnumerable<InvCkdContainerInvoiceDto> result =
                await _dapperRepo.QueryAsync<InvCkdContainerInvoiceDto>(_sql, new
                {
                    @p_billNo = input.BillofladingNo,
                    @p_cont = input.ContainerNo,
                    @p_renban = input.Renban,
                    @p_invoice = input.InvoiceNo,
                    @p_seal = input.SealNo,
                    @p_status = input.Status,
                    @p_supplier_no = input.SupplierNo,
                    p_bill_date_from = input.BillDateFrom,
                    p_bill_date_to = input.BillDateTo,
                    @p_CkdPio = input.CkdPio,
                    p_OrderTypeCode = input.OrderTypeCode
                });

            var exportToExcel = result.ToList();
            return _calendarListExcelExporter.ExportToFile(exportToExcel);
        }

        public async Task<FileDto> GetInvCkdContainerInvoiceCustomsToExcel(GetInvCkdContainerInvoiceInputExcel input)
        {
            string _sql = "Exec INV_CKD_CONTAINER_INVOICE_CUSTOMS_BY_SEARCH @p_billNo, @p_cont, @p_renban, @p_invoice, @p_seal, @p_status, @p_supplier_no, @p_bill_date_from, @p_bill_date_to, @p_CkdPio, @p_OrderTypeCode";  

            IEnumerable<InvCkdContainerInvoiceCustomsDto> result =
               await _dapperRepo.QueryAsync<InvCkdContainerInvoiceCustomsDto>(_sql, new
               {
                   p_billNo = input.BillofladingNo,
                   p_cont = input.ContainerNo,
                   p_renban = input.Renban,
                   p_invoice = input.InvoiceNo,
                   p_seal = input.SealNo,
                   p_status = input.Status,
                   p_supplier_no = input.SupplierNo,
                   p_bill_date_from = input.BillDateFrom,
                   p_bill_date_to = input.BillDateTo,
                   p_CkdPio = input.CkdPio,
                   p_OrderTypeCode = input.OrderTypeCode
               });

            var exportToExcel = result.ToList();
            return _calendarListExcelExporter.ExportCustomsToExcel(exportToExcel);
        }

        public async Task<List<MstInvContainerStatusDto>> GetContStatusList()
        {
            var listStatus = await (from x in _containerStatusRepo.GetAll().AsNoTracking()
                                    orderby x.Code
                                    select new MstInvContainerStatusDto
                                    {
                                        Code = x.Code,
                                        Description = x.Description
                                    }).ToListAsync();
            return listStatus;
        }

        public async Task<PagedResultDto<InvCkdContainerInvoiceViewCustomsDto>> GetInvCkdContainerInvoiceViewCustoms(GetViewCustomInput input)
        {
            string _sql = "Exec INV_CKD_CONTAINER_INVOICE_VIEW_CUSTOMS @p_id";

            IEnumerable<InvCkdContainerInvoiceViewCustomsDto> result =
                await _dapperRepo.QueryAsync<InvCkdContainerInvoiceViewCustomsDto>(_sql, new
                {
                    @p_id = input.Id,
                });

            var listResult = result.ToList();

            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var totalCount = result.ToList().Count();

            return new PagedResultDto<InvCkdContainerInvoiceViewCustomsDto>(
                totalCount,
                pagedAndFiltered);

        }


        public async Task<FileDto> GetInvCkdContainerInvoiceViewCustomsToExcel(GetViewCustomExcelInput input)
        {
            string _sql = "Exec INV_CKD_CONTAINER_INVOICE_VIEW_CUSTOMS @p_id";

            IEnumerable<InvCkdContainerInvoiceViewCustomsDto> result =
               await _dapperRepo.QueryAsync<InvCkdContainerInvoiceViewCustomsDto>(_sql, new
               {
                   @p_id = input.Id,
               });

            var exportToExcel = result.ToList();
            return _calendarListExcelExporter.ExportViewCustomsToExcel(exportToExcel);
        }
        // public async Task GenerateAsync()
        //  {
        //    await _dapperRepo.ExecuteAsync(InvCkdContainerInvoiceConsts.SP_MST_WPT_CALENDAR_GENERATE);
        // }

    }
}
