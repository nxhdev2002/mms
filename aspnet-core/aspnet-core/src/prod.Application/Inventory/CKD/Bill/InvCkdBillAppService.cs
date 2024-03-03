    using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Dapper.Repositories;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.EntityFrameworkCore.Uow;
using Abp.Linq.Extensions;
using Microsoft.EntityFrameworkCore;
using prod.Authorization;
using prod.Dto;
using prod.EntityFrameworkCore;
using prod.HistoricalData;
using prod.Inventory.CKD.Dto;
using prod.Inventory.CKD.Exporting;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace prod.Inventory.CKD
{
    [AbpAuthorize(AppPermissions.Pages_Ckd_Intransit_Bill_View)]
    public class InvCkdBillAppService : prodAppServiceBase, IInvCkdBillAppService
    {
        private readonly IDapperRepository<InvCkdBill, long> _dapperRepo;
        private readonly IRepository<InvCkdBill, long> _repo;
        private readonly IInvCkdBillExcelExporter _invCkdBillListExcelExporter;
        private readonly IInvCkdBillExcelExporter _calendarListExcelExporter;
        private readonly IHistoricalDataAppService _historicalDataAppService;

        public InvCkdBillAppService(IRepository<InvCkdBill, long> repo,
                                        IDapperRepository<InvCkdBill, long> dapperRepo,
                                        IInvCkdBillExcelExporter invCkdBillListExcelExporter,
                                        IInvCkdBillExcelExporter calendarListExcelExporter,
                                        IHistoricalDataAppService historicalDataAppService
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _invCkdBillListExcelExporter = invCkdBillListExcelExporter;
            _historicalDataAppService = historicalDataAppService;
            _calendarListExcelExporter = calendarListExcelExporter;

        }

        public async Task<List<string>> GetBillHistory(GetInvCkdBillHistoryInput input)
        {
            return await _historicalDataAppService.GetHistoricalDataById(input.Id, input.TableName, "Id");
        }

        public async Task<FileDto> GetHistoricalDataToExcel(GetInvCkdBillHistoryExcelInput input)
        {

            var data = await _historicalDataAppService.GetHistoricalDataById(input.Id, input.TableName, "Id");
            return _calendarListExcelExporter.ExportToHistoricalFile(data);
        }

        public async Task<List<long?>> GetChangedRecords()
        {
            return await _historicalDataAppService.GetChangedRecordIds("InvCkdBill");
        }

        public async Task<PagedResultDto<InvCkdBillDto>> GetAll(GetInvCkdBillInput input)
        {
            string _sql = "Exec INV_CKD_BILL_SEARCH @p_BillofladingNo,@p_BillDateFrom,@p_BillDateTo,@p_CkdPio, @p_OrderTypeCode";

            var result = await _dapperRepo.QueryAsync<InvCkdBillDto>(_sql, new
            {
                p_BillofladingNo = input.BillofladingNo,
                p_BillDateFrom = input.BillDateFrom,
                p_BillDateTo = input.BillDateTo,             
                p_CkdPio = input.CkdPio,
                p_OrderTypeCode = input.OrderTypeCode
            });


            var listResult = result.ToList();

            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var totalCount = result.ToList().Count();

            return new PagedResultDto<InvCkdBillDto>(
                totalCount,
                pagedAndFiltered);
        }


        public async Task<FileDto> GetBillToExcel(InvCkdBillExportInput input)
        {
            string _sql = "Exec INV_CKD_BILL_SEARCH @p_BillofladingNo,@p_BillDateFrom,@p_BillDateTo,@p_CkdPio, @p_OrderTypeCode";

            var result = await _dapperRepo.QueryAsync<InvCkdBillDto>(_sql, new
            {
                p_BillofladingNo = input.BillofladingNo,
                p_BillDateFrom = input.BillDateFrom,
                p_BillDateTo = input.BillDateTo,
                p_CkdPio = input.CkdPio,
                p_OrderTypeCode = input.OrderTypeCode
            });

            var listResult = result.ToList();
            return _invCkdBillListExcelExporter.ExportToFile(listResult);
        }
        
    }
}
