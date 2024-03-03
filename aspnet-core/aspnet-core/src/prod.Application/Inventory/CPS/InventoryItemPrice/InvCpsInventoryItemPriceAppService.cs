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
    [AbpAuthorize(AppPermissions.Pages_CpsLinkAge_InventoryItemPrice_View)]
    public class InvCpsInventoryItemPriceAppService : prodAppServiceBase, IInvCpsInventoryItemPriceAppService
    {
        private readonly IDapperRepository<InvCpsInventoryItemPrice, long> _invCpsInventoryItemPrice;
        private readonly IInvCpsInventoryItemPriceExcelExporter _listExcelExporter;



        public InvCpsInventoryItemPriceAppService(IRepository<InvCpsInventoryItemPrice, long> repo,
                                         IDapperRepository<InvCpsInventoryItemPrice, long> invCpsInventoryItemPrice,
                                         IInvCpsInventoryItemPriceExcelExporter listExcelExporter

            )
        {
            _invCpsInventoryItemPrice = invCpsInventoryItemPrice;
            _listExcelExporter = listExcelExporter;
        }

        public async Task<PagedResultDto<InvCpsInventoryItemPriceDto>> GetCpsInventoryItemPriceSearch(GetInvCpsInventoryItemPriceInput input)
        {
            string _sql = "Exec INV_CPS_INVENTORY_ITEM_PRICE_SEARCH @p_partNo, @p_partName, @p_effectiveFrom, @p_effectiveTo";

            IEnumerable<InvCpsInventoryItemPriceDto> result = await _invCpsInventoryItemPrice.QueryAsync<InvCpsInventoryItemPriceDto>(_sql,
                  new
                  {
                      p_partNo = input.PartNo,
                      p_partName = input.PartName,
                      p_effectiveFrom = input.EffectiveFrom,
                      p_effectiveTo = input.EffectiveTo
                  });
            var listResult = result.ToList();

            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var totalCount = result.ToList().Count();

            return new PagedResultDto<InvCpsInventoryItemPriceDto>(
                totalCount,
                pagedAndFiltered);
        }

        public async Task<FileDto> GetCpsInventoryItemPriceToExcel(GetInvCpsInventoryItemPriceExportInput input)
        {
            string _sql = "Exec INV_CPS_INVENTORY_ITEM_PRICE_SEARCH @p_partNo, @p_partName, @p_effectiveFrom, @p_effectiveTo";

            IEnumerable<InvCpsInventoryItemPriceDto> result = await _invCpsInventoryItemPrice.QueryAsync<InvCpsInventoryItemPriceDto>(_sql,
                  new
                  {
                      p_partNo = input.PartNo,
                      p_partName = input.PartName,
                      p_effectiveFrom = input.EffectiveFrom,
                      p_effectiveTo = input.EffectiveTo
                  });
            var exportToExcel = result.ToList();
            return _listExcelExporter.ExportToFile(exportToExcel);
        }
    }
}
