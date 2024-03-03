using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Dapper.Repositories;
using Abp.Domain.Repositories;
using prod.Authorization;
using prod.Dto;
using prod.Inventory.CPS.Dto;
using prod.Inventory.CPS.InvCpsPrvShipment;
using prod.Inventory.CPS.InvCpsRcvShipmentHeader.Exproting;
using prod.Master.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace prod.Inventory.CPS
{
    [AbpAuthorize(AppPermissions.Pages_CpsLinkAge_RcvShipmentHeaders_View)]
    public class InvCpsRcvShipmentHeadersAppService : prodAppServiceBase, IInvCpsRcvShipmentHeadersAppServicecs
    {
        private readonly IDapperRepository<InvCpsRcvShipmentHeaders, long> _dapperRepo;
        private readonly IRepository<InvCpsRcvShipmentHeaders, long> _repo;
        private readonly IDapperRepository<InvCpsRcvShipmentLines, long> _invCpsRcvShipmentLinesRepo;
        private readonly IDapperRepository<MstInvCpsInventoryGroup, long> _mstInvCpsInventoryGroup;
        private readonly IDapperRepository<MstInvCpsSuppliers, long> _mstInvCpsSuppliers;
        private readonly IInvCpsRcvShipmentHeadersExcelExporter _calendarListExcelExporter;

        public InvCpsRcvShipmentHeadersAppService(IRepository<InvCpsRcvShipmentHeaders, long> repo,
                                         IDapperRepository<InvCpsRcvShipmentHeaders, long> dapperRepo,
                                         IDapperRepository<InvCpsRcvShipmentLines, long> invCpsRcvShipmentLinesRepo,
                                         IDapperRepository<MstInvCpsInventoryGroup, long> mstInvCpsInventoryGroup,
                                         IDapperRepository<MstInvCpsSuppliers, long> mstInvCpsSuppliers,
                                         IInvCpsRcvShipmentHeadersExcelExporter calendarListExcelExporter



            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _invCpsRcvShipmentLinesRepo = invCpsRcvShipmentLinesRepo;
            _mstInvCpsInventoryGroup = mstInvCpsInventoryGroup;
            _mstInvCpsSuppliers = mstInvCpsSuppliers;
            _calendarListExcelExporter = calendarListExcelExporter;
        }

        public async Task<PagedResultDto<InvCpsRcvShipmentHeadersDto>> GetAllInvCpsRcvShipmentHeaders(GetInvCpsRcvShipmentHeadersInput input)
        {
            string _sqlSearch = "Exec INV_CPS_RCVSHIPMENT_HEADERS_SEARCH  @p_receipt_num,@p_invetory_group_id,@p_vendor_id,@p_from_date,@p_to_date,@p_part_no,@p_part_name,@p_po_number ";

            IEnumerable<InvCpsRcvShipmentHeadersDto> result = await _dapperRepo.QueryAsync<InvCpsRcvShipmentHeadersDto>(_sqlSearch,
                  new
                  {
                      p_receipt_num = input.p_receipt_num,
                      p_invetory_group_id = input.p_invetory_group_id,
                      p_vendor_id = input.p_vendor_id,
                      p_from_date = input.p_from_date,
                      p_to_date = input.p_to_date,
                      p_part_no = input.p_part_no,
                      p_part_name = input.p_part_name,
                      p_po_number = input.p_po_number
                  });
            var listResult = result.ToList();

            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var totalCount = result.ToList().Count();

            return new PagedResultDto<InvCpsRcvShipmentHeadersDto>(
                totalCount,
                pagedAndFiltered);
        }


        public async Task<PagedResultDto<InvCpsRcvShipmentLineDto>> GetAllInvCpsRcvShipmentHeadersDetail(GetInvCpsRcvShipmentLineInput input)
        {
            string _sqlSearch = "Exec INV_CPS_RCVSHIPMENT_LINES_GETBYID  @p_id";

            IEnumerable<InvCpsRcvShipmentLineDto> result = await _invCpsRcvShipmentLinesRepo.QueryAsync<InvCpsRcvShipmentLineDto>(_sqlSearch,
                  new
                  {
                      p_id = input.p_id

                  });
            var listResult = result.ToList();

            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var totalCount = result.ToList().Count();

            return new PagedResultDto<InvCpsRcvShipmentLineDto>(
                totalCount,
                pagedAndFiltered);
        }

        public async Task<ListInvCpsRcvShipmentHeadersDto> GetShipmentHeaderData()
        {
            string _sqlSearch = "Exec MST_INV_CPS_INVENTORY_GROUP_GETALL ";

            IEnumerable<GetInvCpsInventoryGroup> MST_INV_CPS_INVENTORY_GROUP_GETALL = await _mstInvCpsInventoryGroup.QueryAsync<GetInvCpsInventoryGroup>(_sqlSearch);
            var listInvetory = MST_INV_CPS_INVENTORY_GROUP_GETALL.ToList();

            _sqlSearch = "Exec MST_INV_CPS_SUPPLIERS_GETALL ";
            IEnumerable<GetInvCpsSuppliers> MST_INV_CPS_SUPPLIERS_GETALL = await _mstInvCpsSuppliers.QueryAsync<GetInvCpsSuppliers>(_sqlSearch);

            var listSupplier = MST_INV_CPS_SUPPLIERS_GETALL.ToList();


            ListInvCpsRcvShipmentHeadersDto shipmentheader = new ListInvCpsRcvShipmentHeadersDto
            {
                ListInventoryGroup = listInvetory,
                ListSuppliers = listSupplier

            };

            return shipmentheader;
        }




        public async Task<FileDto> ShipmentHeader_ExportExcel(string p_receipt_num, long? p_invetory_group_id, long? p_vendor_id, DateTime? p_from_date, DateTime? p_to_date, string p_part_no, string p_part_name, string p_po_number)
        {

            string _sqlSearch = "Exec INV_CPS_RCVSHIPMENT_HEADERS_SEARCH  @p_receipt_num,@p_invetory_group_id,@p_vendor_id,@p_from_date,@p_to_date,@p_part_no,@p_part_name,@p_po_number  ";

            IEnumerable<InvCpsRcvShipmentHeadersDto> result = await _dapperRepo.QueryAsync<InvCpsRcvShipmentHeadersDto>(_sqlSearch,
                  new
                  {
                      p_receipt_num = p_receipt_num,
                      p_invetory_group_id = p_invetory_group_id,
                      p_vendor_id = p_vendor_id,
                      p_from_date = p_from_date,
                      p_to_date = p_to_date,
                      p_part_no = p_part_no,
                      p_part_name = p_part_name,
                      p_po_number = p_po_number

                  });
            var exportToExcel = result.ToList();
            return _calendarListExcelExporter.ExportToFile(exportToExcel);
        }


        public async Task<FileDto> ShipmentLines_ExportExcel(long? p_id)
        {

            string _sqlSearch = "Exec INV_CPS_RCVSHIPMENT_LINES_GETBYID  @p_id";

            IEnumerable<InvCpsRcvShipmentLineDto> result = await _invCpsRcvShipmentLinesRepo.QueryAsync<InvCpsRcvShipmentLineDto>(_sqlSearch,
                  new
                  {
                      p_id = p_id

                  });
            var exportToExcel = result.ToList();
            return _calendarListExcelExporter.ExportToFileLine(exportToExcel);
        }




    }
}

