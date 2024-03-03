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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using prod.Authorization;
using prod.Dto;
using prod.EntityFrameworkCore;
using prod.Inventory.GPS.Dto;
using prod.Inventory.GPS.Exporting;
using prod.Inventory.Gps.PartList.Dto;

namespace prod.Inventory.GPS
{
    [AbpAuthorize(AppPermissions.Pages_Gps_Warehouse_GpsStock_View)]
    public class InvGpsStockAppService : prodAppServiceBase, IInvGpsStockAppService
    {
        private readonly IDapperRepository<InvGpsStock, long> _dapperRepo;
        private readonly IRepository<InvGpsStock, long> _repo;
        private readonly IInvGpsStockExcelExporter _calendarListExcelExporter;

        public InvGpsStockAppService(IRepository<InvGpsStock, long> repo,
                                         IDapperRepository<InvGpsStock, long> dapperRepo,
                                        IInvGpsStockExcelExporter calendarListExcelExporter
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
        }

        public async Task<PagedResultDto<InvGpsStockDto>> GetAll(GetInvGpsStockInput input)
        {
            string _sql = "Exec INV_GPS_STOCK_SEARCH @p_part_no, @p_working_date_from, @p_working_date_to,@p_supplier_no,@p_vin_no,@p_cfc,@p_lot_no,@p_no_in_lot";

            IEnumerable<InvGpsStockDto> result = await _dapperRepo.QueryAsync<InvGpsStockDto>(_sql, new
            {
                p_part_no = input.PartNo,
                p_working_date_from = input.WorkingDateFrom,
                p_working_date_to = input.WorkingDateTo,
                p_supplier_no = input.SupplierNo,
                p_vin_no = input.VinNo,
                p_cfc = input.Cfc,
                p_lot_no = input.LotNo,
                p_no_in_lot = input.NoInLot,
            });

            var listResult = result.ToList();

            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var totalCount = result.ToList().Count();

            return new PagedResultDto<InvGpsStockDto>(
                totalCount,
                pagedAndFiltered);
        }


        public async Task<FileDto> GetGpsStockToExcel(InvGpsStockExportInput input)
        {
            string _sql = "Exec INV_GPS_STOCK_SEARCH @p_part_no, @p_working_date_from, @p_working_date_to,@p_supplier_no,@p_vin_no,@p_cfc,@p_lot_no,@p_no_in_lot";

            IEnumerable<InvGpsStockDto> result = await _dapperRepo.QueryAsync<InvGpsStockDto>(_sql, new
            {
                p_part_no = input.PartNo,
                p_working_date_from = input.WorkingDateFrom,
                p_working_date_to = input.WorkingDateTo,
                p_supplier_no = input.SupplierNo,
                p_vin_no = input.VinNo,
                p_cfc = input.Cfc,
                p_lot_no = input.LotNo,
                p_no_in_lot = input.NoInLot,
            });
            var exportToExcel = result.ToList();
            return _calendarListExcelExporter.ExportToFile(exportToExcel);
        }
    }
}
