using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Dapper.Repositories;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Microsoft.EntityFrameworkCore;
using prod.Authorization;
using prod.Dto;
using prod.Inventory.CKD.Dto;
using prod.Inventory.CKD.Invoice.Dto;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace prod.Inventory.CKD
{
    [AbpAuthorize(AppPermissions.Pages_Ckd_Intransit_Shipment_View)]
    public class InvCkdShipmentAppService : prodAppServiceBase, IInvCkdShipmentAppService
    {
        private readonly IDapperRepository<InvCkdShipment, long> _dapperRepo;
        private readonly IRepository<InvCkdShipment, long> _repo;
        private readonly IInvCkdShipmentExcelExporter _calendarListExcelExporter;

        public InvCkdShipmentAppService(IRepository<InvCkdShipment, long> repo,
                                         IDapperRepository<InvCkdShipment, long> dapperRepo,
                                        IInvCkdShipmentExcelExporter calendarListExcelExporter
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
        }

        public async Task<PagedResultDto<InvCkdShipmentDto>> GetAll(GetInvCkdShipmentInput input)
        {
            string _sql = "Exec INV_CKD_SHIPMENT_SEARCH @p_ShipmentNo,@p_ShippingcompanyCode,@p_SupplierNo,@p_FromPort,@p_ToPort,@p_ShipmentDate,@p_CkdPio, @p_OrderTypeCode";

            var result = await _dapperRepo.QueryAsync<InvCkdShipmentDto>(_sql, new
            {
                p_ShipmentNo = input.ShipmentNo,
                p_ShippingcompanyCode = input.ShippingcompanyCode,
                p_SupplierNo = input.SupplierNo,
                p_FromPort = input.FromPort,
                p_ToPort = input.ToPort,
                p_ShipmentDate = input.ShipmentDate,
                p_CkdPio = input.CkdPio,
                p_OrderTypeCode = input.OrderTypeCode
            });


            var listResult = result.ToList();

            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var totalCount = result.ToList().Count();

            return new PagedResultDto<InvCkdShipmentDto>(
                totalCount,
                pagedAndFiltered);
        }


        public async Task<FileDto> GetShipmentToExcel(InvCkdShipmentExportInput input)
        {
            string _sql = "Exec INV_CKD_SHIPMENT_SEARCH @p_ShipmentNo,@p_ShippingcompanyCode,@p_SupplierNo,@p_FromPort,@p_ToPort,@p_ShipmentDate,@p_CkdPio, @p_OrderTypeCode";

            var result = await _dapperRepo.QueryAsync<InvCkdShipmentDto>(_sql, new
            {
                p_ShipmentNo = input.ShipmentNo,
                p_ShippingcompanyCode = input.ShippingcompanyCode,
                p_SupplierNo = input.SupplierNo,
                p_FromPort = input.FromPort,
                p_ToPort = input.ToPort,
                p_ShipmentDate = input.ShipmentDate,
                p_CkdPio = input.CkdPio,
                p_OrderTypeCode = input.OrderTypeCode
            });

            var listResult = result.ToList();
            return _calendarListExcelExporter.ExportToFile(listResult);
        }

    }
}
