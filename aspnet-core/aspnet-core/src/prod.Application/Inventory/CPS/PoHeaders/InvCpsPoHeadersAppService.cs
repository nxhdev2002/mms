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
using NUglify.Helpers;
using prod.Authorization;
using prod.Dto;
using prod.Inventory.CPS.Dto;
using prod.Inventory.CPS.Exporting;
using prod.Inventory.Invoice.Dto;
using prod.Master.Cmm;
using prod.Master.Common.GradeColor.Dto;
using prod.Master.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace prod.Inventory.CPS
{
      [AbpAuthorize(AppPermissions.Pages_CpsLinkAge_PoHeaders_View)]
    public class InvCpsPoHeadersAppService : prodAppServiceBase, IInvCpsPoHeadersAppService
    {
        private readonly IDapperRepository<InvCpsPoLines, long> _invCpsPoLines;
        private readonly IDapperRepository<InvCpsPoHeaders, long> _invCpsPoHeaderRepo;
        private readonly IDapperRepository<MstInvCpsInventoryGroup, long> _cbxInventoryGroup;
        private readonly IDapperRepository<MstInvCpsSuppliers, long> _cbxSupplier;
        private readonly IRepository<InvCpsPoHeaders, long> _repo;
        private readonly IRepository<InvCpsPoLines, long> _repo2;
        private readonly IInvCpsPoHeadersExcelExporter _poHeaderExcelExporter;
       

        public InvCpsPoHeadersAppService(IRepository<InvCpsPoHeaders, long> repo,
                                        IRepository<InvCpsPoLines, long> repo2,
                                         IDapperRepository<InvCpsPoHeaders, long> invCpsPoHeaderRepo,
                                         IDapperRepository<InvCpsPoLines, long> invCpsPoLines,
                                          IDapperRepository<MstInvCpsInventoryGroup, long> cbxInventoryGroup,
                                           IDapperRepository<MstInvCpsSuppliers, long> cbxSupplier,
                                          IInvCpsPoHeadersExcelExporter poHeaderExcelExporter
            )
        {
            _repo = repo;
            _repo2 = repo2;
            _invCpsPoHeaderRepo = invCpsPoHeaderRepo;
            _cbxInventoryGroup = cbxInventoryGroup;
            _cbxSupplier = cbxSupplier;
            _invCpsPoLines = invCpsPoLines;
            _poHeaderExcelExporter = poHeaderExcelExporter;
 
        }
           
        public async Task<PagedResultDto<GridPoHeadersDto>> GetCpsPoHeaderSearch(GetInvCpsPoHeadersInput input)
        {
            string _sql = "Exec INV_CPS_PO_HEADERS_SEARCH @p_po_num, @p_invetory_group_id, @p_vendor_id, @p_from_date, @p_to_date, @p_part_no ,@p_part_name";

            IEnumerable<GridPoHeadersDto> result = await _invCpsPoHeaderRepo.QueryAsync<GridPoHeadersDto>(_sql,
                  new
                  {
                      p_po_num = input.PoNumber,
                      p_invetory_group_id = input.InventoryGroup,
                      p_vendor_id = input.Supplier,
                      p_from_date = input.POCreationFromDate,
                      p_to_date = input.POCreationToDate,
                      p_part_no = input.PartNo,
                      p_part_name = input.PartName,
                  });
            var listResult = result.ToList();

            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var totalCount = result.ToList().Count();

            return new PagedResultDto<GridPoHeadersDto>(
                totalCount,
                pagedAndFiltered);
        }
        public async Task<PagedResultDto<InvCpsPolinesDto>> GetCpsPoLine(GetInvCpsLine input)
        {
            string _sql = "Exec INV_CPS_PO_LINES_GETBYID @p_id";

            IEnumerable<InvCpsPolinesDto> result = await _invCpsPoLines.QueryAsync<InvCpsPolinesDto>(_sql,
                  new
                  {
                      p_id = input.PoHeaderId,
                  });
            var listResult = result.ToList();

            if (listResult.Count > 0)
            {
                listResult[0].TotalPrice = listResult.Sum(e => (e.UnitPrice * e.Quantity ));             
            }

            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var totalCount = result.ToList().Count();

            return new PagedResultDto<InvCpsPolinesDto>(
                totalCount,
                pagedAndFiltered);  
        }
        public async Task<List<CbxInventoryGroupDto>> GetCbxInventoryGroup()
        {
            string _sqlSearch = "Exec MST_INV_CPS_INVENTORY_GROUP_GETALL";

            IEnumerable<CbxInventoryGroupDto> _result = await _cbxInventoryGroup.QueryAsync<CbxInventoryGroupDto>(_sqlSearch);
            return _result.ToList();
        }

        public async Task<List<CbxSupplierDto>> GetCbxSupplier()
        {
            string _sqlSearch = "Exec MST_INV_CPS_SUPPLIERS_GETALL";

            IEnumerable<CbxSupplierDto> _result = await _cbxSupplier.QueryAsync<CbxSupplierDto>(_sqlSearch);
            return _result.ToList();
        }


        public async Task<FileDto> GetPoHeadersToExcel(GetInvCpsPoHeadersInput input)
        {
            string _sql = "Exec INV_CPS_PO_HEADERS_SEARCH @p_po_num, @p_invetory_group_id, @p_vendor_id, @p_from_date, @p_to_date, @p_part_no ,@p_part_name";

            IEnumerable<GridPoHeadersDto> result = await _invCpsPoHeaderRepo.QueryAsync<GridPoHeadersDto>(_sql,
              new
              {
                  p_po_num = input.PoNumber,
                  p_invetory_group_id = input.InventoryGroup,
                  p_vendor_id = input.Supplier,
                  p_from_date = input.POCreationFromDate,
                  p_to_date = input.POCreationToDate,
                  p_part_no = input.PartNo,
                  p_part_name = input.PartName,
              });
            var listPoHeaderResult = result.ToList();
            return _poHeaderExcelExporter.ExportToFile(listPoHeaderResult);
        }
        
        public async Task<FileDto> GetPoLinesToExcel(long? p_id)
        {

            string _sql = "Exec INV_CPS_PO_LINES_GETBYID @p_id";

            IEnumerable<InvCpsPoLinesDto> result = await _invCpsPoLines.QueryAsync<InvCpsPoLinesDto>(_sql,
                  new
                  {
                      p_id = p_id

                  });
            var exportToExcel = result.ToList();
            return _poHeaderExcelExporter.ExportToFilePoline(exportToExcel);
        }



    }
}
