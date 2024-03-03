using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Dapper.Repositories;
using prod.Authorization;
using prod.Dto;
using prod.Inventory.CKD;
using prod.Inventory.CKD.Dto;
using prod.Inventory.CKD.InvCkdRequest.Exporting;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prod.Inventory.Request
{
    [AbpAuthorize(AppPermissions.Pages_Ckd_Warehouse_ContainerRentalWHPlan_View)]
    public class InvCkdRequestAppService : prodAppServiceBase, IInvCkdRequestAppService
    {
        private readonly IDapperRepository<InvCkdRequestContentByLot, long> _iInvCkdRequestContentByLotRepo;
        private readonly IDapperRepository<InvCkdRequest, long> _invCkdRequestRepo;
        private readonly IDapperRepository<InvCkdRequestContentByPxP, long> _invCkdRequestContentByPxP;
        private readonly IInvCkdRequestExcelExporter _calendarListExcelExporter;

        public InvCkdRequestAppService(
                    IDapperRepository<InvCkdRequestContentByLot, long> iInvCkdRequestContentByLotRepo,
                    IDapperRepository<InvCkdRequest, long> invCkdRequestRepo,
                    IDapperRepository<InvCkdRequestContentByPxP, long> invCkdRequestContentByPxP,
                    IInvCkdRequestExcelExporter calendarListExcelExporter
          )
        {

            _iInvCkdRequestContentByLotRepo = iInvCkdRequestContentByLotRepo;
            _invCkdRequestRepo = invCkdRequestRepo;
            _invCkdRequestContentByPxP = invCkdRequestContentByPxP;
            _calendarListExcelExporter = calendarListExcelExporter;

        }


        public async Task<PagedResultDto<InvCkdRequestDto>> GetAllInvCkdRequestSearch(GetInvCkdRequestInput input)
        {
            string _sqlSearch = "Exec INV_CKD_REQUEST_SEARCH  @p_request_no, @p_request_date_from, @p_request_date_to, @p_status, @p_container_no, @p_reban, @p_invoice_no";

            IEnumerable<InvCkdRequestDto> result = await _invCkdRequestRepo.QueryAsync<InvCkdRequestDto>(_sqlSearch,
                  new
                  {
                      p_request_no = input.p_request_no,
                      p_request_date_from = input.request_date_from,
                      p_request_date_to = input.request_date_to,
                      p_status = input.p_status,
                      p_container_no = input.p_container_no,
                      p_reban = input.p_reban,
                      p_invoice_no = input.p_invoice_no,

                  });
            var listResult = result.ToList();

            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var totalCount = result.ToList().Count();

            return new PagedResultDto<InvCkdRequestDto>(
                totalCount,
                pagedAndFiltered);
        }

        public async Task<FileDto> GetRequestToExcel(GetInvCkdRequestInputExcel input)
        {
            string _sqlSearch = "Exec INV_CKD_REQUEST_SEARCH  @p_request_no, @p_request_date_from, @p_request_date_to, @p_status, @p_container_no, @p_reban, @p_invoice_no";

            IEnumerable<InvCkdRequestDto> result = await _invCkdRequestRepo.QueryAsync<InvCkdRequestDto>(_sqlSearch,
                  new
                  {
                      p_request_no = input.p_request_no,
                      p_request_date_from = input.request_date_from,
                      p_request_date_to = input.request_date_to,
                      p_status = input.p_status,
                      p_container_no = input.p_container_no,
                      p_reban = input.p_reban,
                      p_invoice_no = input.p_invoice_no

                  });
            var exportToExcel = result.ToList();
            return _calendarListExcelExporter.ExportToFileRequest(exportToExcel);
        }


        public async Task<PagedResultDto<InvCkdRequestContentByLotDto>> GetAllInvCkdByLot(GetInvCkdRequestDetailInput input)
        {
            string _sqlSearch = "Exec INV_CKD_REQUEST_CONTENT_BY_LOT_GETBYID @p_ckd_req_id";

            IEnumerable<InvCkdRequestContentByLotDto> result = await _iInvCkdRequestContentByLotRepo.QueryAsync<InvCkdRequestContentByLotDto>(_sqlSearch,
                  new
                  {
                      p_ckd_req_id = input.p_ckd_req_id

                  });
            var listResult = result.ToList();

            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var totalCount = result.ToList().Count();

            return new PagedResultDto<InvCkdRequestContentByLotDto>(
                totalCount,
                pagedAndFiltered);
        }

        public async Task<PagedResultDto<InvCkdRequestContentByPxPDto>> GetAllInvCkdByPxP(GetInvCkdRequestDetailInput input)
        {
            string _sqlSearch = "Exec INV_CKD_REQUEST_CONTENT_BY_PXP_GETBYID @p_ckd_req_id";

            IEnumerable<InvCkdRequestContentByPxPDto> result = await _iInvCkdRequestContentByLotRepo.QueryAsync<InvCkdRequestContentByPxPDto>(_sqlSearch,
                  new
                  {
                      p_ckd_req_id = input.p_ckd_req_id

                  });
            var listResult = result.ToList();

            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var totalCount = result.ToList().Count();

            return new PagedResultDto<InvCkdRequestContentByPxPDto>(
                totalCount,
                pagedAndFiltered);
        }


        public async Task<PagedResultDto<GetInvCkdDeliveryScheGetByReqByMakeDto>> GetAllDeliveryScheduleByMake(GetCkdRequestDetailInput input)
        {
            string _sqlSearch = "Exec INV_CKD_CONTAINER_DELIVERY_SCHE_GETBYREQID  @p_ckd_req_id";

            IEnumerable<GetInvCkdDeliveryScheGetByReqByMakeDto> result = await _iInvCkdRequestContentByLotRepo.QueryAsync<GetInvCkdDeliveryScheGetByReqByMakeDto>(_sqlSearch,
                  new
                  {
                      p_ckd_req_id = input.p_ckd_req_id,
                      
                  });
            var listResult = result.ToList();

            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var totalCount = result.ToList().Count();

            return new PagedResultDto<GetInvCkdDeliveryScheGetByReqByMakeDto>(
                totalCount,
                pagedAndFiltered);
        }


        public async Task<PagedResultDto<GetInvCkdDeliveryScheGetByReqByCallDto>> GetAllDeliveryScheduleByCall(GetCkdRequestDetailInput input)
        {
            string _sqlSearch = "Exec INV_CKD_CONTAINER_DELIVERY_SCHE_GETBYREQID_BYCALL  @p_ckd_req_id, @p_containerno ,@p_renban ,@p_invoiceno";

            IEnumerable<GetInvCkdDeliveryScheGetByReqByCallDto> result = await _iInvCkdRequestContentByLotRepo.QueryAsync<GetInvCkdDeliveryScheGetByReqByCallDto>(_sqlSearch,
                  new
                  {
                      p_ckd_req_id = input.p_ckd_req_id,
                      p_containerno = input.p_containerno,
                      p_renban = input.p_renban,
                      p_invoiceno = input.p_invoiceno

                  });
            var listResult = result.ToList();

            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var totalCount = result.ToList().Count();

            return new PagedResultDto<GetInvCkdDeliveryScheGetByReqByCallDto>(
                totalCount,
                pagedAndFiltered);
        }

        public async Task<FileDto> GetRequestByLotToExcel(GetInvCkdRequestDetailByInputExcel input)
        {
            string _sqlSearch = "Exec INV_CKD_REQUEST_CONTENT_BY_LOT_GETBYID @p_ckd_req_id";

            IEnumerable<InvCkdRequestContentByLotDto> result = await _iInvCkdRequestContentByLotRepo.QueryAsync<InvCkdRequestContentByLotDto>(_sqlSearch,
                  new
                  {
                      p_ckd_req_id = input.p_ckd_req_id

                  });
            var exportToExcel = result.ToList();
            return _calendarListExcelExporter.ExportToFileRequestByLot(exportToExcel);
        }

        public async Task<FileDto> GetRequestByPxpToExcel(GetInvCkdRequestDetailByInputExcel input)
        {
            string _sqlSearch = "Exec INV_CKD_REQUEST_CONTENT_BY_PXP_GETBYID @p_ckd_req_id";

            IEnumerable<InvCkdRequestContentByPxPDto> result = await _iInvCkdRequestContentByLotRepo.QueryAsync<InvCkdRequestContentByPxPDto>(_sqlSearch,
                  new
                  {
                      p_ckd_req_id = input.p_ckd_req_id

                  });
            var exportToExcel = result.ToList();
            return _calendarListExcelExporter.ExportToFileRequestByPxp(exportToExcel);
        }

        public async Task<FileDto> GetRequestByMakeToExcel(GetInvCkdRequestDetailByInputExcel input)
        {
            string _sqlSearch = "Exec INV_CKD_CONTAINER_DELIVERY_SCHE_GETBYREQID  @p_ckd_req_id";

            IEnumerable<GetInvCkdDeliveryScheGetByReqByMakeDto> result = await _iInvCkdRequestContentByLotRepo.QueryAsync<GetInvCkdDeliveryScheGetByReqByMakeDto>(_sqlSearch,
                  new
                  {
                      p_ckd_req_id = input.p_ckd_req_id

                  });
            var exportToExcel = result.ToList();
            return _calendarListExcelExporter.ExportToFileRequestByMake(exportToExcel);
        }

        public async Task<FileDto> GetRequestByCallToExcel(GetInvCkdRequestDetailInputExcel input)
        {
            string _sqlSearch = "Exec INV_CKD_CONTAINER_DELIVERY_SCHE_GETBYREQID_BYCALL  @p_ckd_req_id, @p_containerno ,@p_renban ,@p_invoiceno";

            IEnumerable<GetInvCkdDeliveryScheGetByReqByCallDto> result = await _iInvCkdRequestContentByLotRepo.QueryAsync<GetInvCkdDeliveryScheGetByReqByCallDto>(_sqlSearch,
                  new
                  {
                      p_ckd_req_id = input.p_ckd_req_id,
                      p_containerno = input.p_containerno,
                      p_renban = input.p_renban,
                      p_invoiceno = input.p_invoiceno

                  });
            var exportToExcel = result.ToList();
            return _calendarListExcelExporter.ExportToFileRequestByCall(exportToExcel);
        }
    }

}
