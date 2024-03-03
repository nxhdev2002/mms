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
    [AbpAuthorize(AppPermissions.Pages_Ckd_Intransit_ContainerIntransit_View)]
    public class InvCkdContainerIntransitAppService : prodAppServiceBase, IInvCkdContainerIntransitAppService
    {
        private readonly IDapperRepository<InvCkdContainerIntransit, long> _dapperRepo;
        private readonly IRepository<InvCkdContainerIntransit, long> _repo;
        private readonly IInvCkdContainerIntransitExcelExporter _calendarListExcelExporter;

        public InvCkdContainerIntransitAppService(IRepository<InvCkdContainerIntransit, long> repo,
                                         IDapperRepository<InvCkdContainerIntransit, long> dapperRepo,
                                        IInvCkdContainerIntransitExcelExporter calendarListExcelExporter
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
        }

        public async Task<PagedResultDto<InvCkdContainerIntransitDto>> GetAll(GetInvCkdContainerIntransitInput input)
        {
            string _sql = "Exec INV_CKD_CONTAINER_INTRANSIT_SEARCH @p_container_no, @p_renban, @p_supplier_no, @p_shipping_date, @p_port_date, @p_trans_date, @p_tmv_date";

            IEnumerable<InvCkdContainerIntransitDto> result = await _dapperRepo.QueryAsync<InvCkdContainerIntransitDto>(_sql, new
            {
                p_container_no = input.ContainerNo,
                p_renban = input.Renban,
                p_supplier_no = input.SupplierNo,
                p_shipping_date = input.ShippingDate,
                p_port_date = input.PortDate,
                p_trans_date = input.TransDate,
                p_tmv_date = input.TmvDate
            });

            var listResult = result.ToList();

            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var totalCount = result.ToList().Count();

            return new PagedResultDto<InvCkdContainerIntransitDto>(
                totalCount,
                pagedAndFiltered);
        }


        public async Task<FileDto> GetContainerIntransitToExcel(InvCkdContainerIntransitExportInput input)
        {
            string _sql = "Exec INV_CKD_CONTAINER_INTRANSIT_SEARCH @p_container_no, @p_renban, @p_supplier_no, @p_shipping_date, @p_port_date, @p_trans_date, @p_tmv_date";

            IEnumerable<InvCkdContainerIntransitDto> result = await _dapperRepo.QueryAsync<InvCkdContainerIntransitDto>(_sql, new
            {
                p_container_no = input.ContainerNo,
                p_renban = input.Renban,
                p_supplier_no = input.SupplierNo,
                p_shipping_date = input.ShippingDate,
                p_port_date = input.PortDate,
                p_trans_date = input.TransDate,
                p_tmv_date = input.TmvDate
            });

            var exportToExcel = result.ToList();
            return _calendarListExcelExporter.ExportToFile(exportToExcel);
        }
    }
}
