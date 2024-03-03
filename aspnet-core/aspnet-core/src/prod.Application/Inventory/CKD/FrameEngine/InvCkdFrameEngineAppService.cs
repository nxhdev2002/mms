using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Dapper.Repositories;
using Abp.Domain.Repositories;
using prod.Authorization;
using prod.Dto;
using prod.Inventory.CKD.Dto;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace prod.Inventory.CKD
{
    [AbpAuthorize(AppPermissions.Pages_Ckd_Intransit_FrameEngine_View)]
    public class InvCkdFrameEngineAppService : prodAppServiceBase, IInvCkdFrameEngineAppService
    {
        private readonly IDapperRepository<InvCkdFrameEngine, long> _dapperRepo;
        private readonly IRepository<InvCkdFrameEngine, long> _repo;
        private readonly IInvCkdFrameEngineExcelExporter _FrameEngineListExcelExporter;

        public InvCkdFrameEngineAppService(IRepository<InvCkdFrameEngine, long> repo,
                                         IDapperRepository<InvCkdFrameEngine, long> dapperRepo,
                                        IInvCkdFrameEngineExcelExporter FrameEngineListExcelExporter
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _FrameEngineListExcelExporter = FrameEngineListExcelExporter;
        }

        public async Task<PagedResultDto<InvCkdFrameEngineDto>> GetDataFrameEngine(GetInvCkdFrameEngineInput input)
        {

            string _sql = "Exec INV_CKD_FRAME_ENGINE_SEARCH @Billoflading_No, @Invoice_No, @Supplier_No, @Invoice_Date_From, @Invoice_Date_To, @FrameOrEngine, @Type, @p_OrderTypeCode";

            var filtered = await _dapperRepo.QueryAsync<InvCkdFrameEngineDto>(_sql, new
            {
                Billoflading_No = input.BillofladingNo,
                Invoice_No = input.InvoiceNo,
                Supplier_No = input.SupplierNo,
                Invoice_Date_From = input.InvoiceDateFrom,
                Invoice_Date_To = input.InvoiceDateTo,
                FrameOrEngine = input.FrameEngineNo,
                Type = input.Type,
                p_OrderTypeCode = input.OrderTypeCode
            });


            var results = from o in filtered
                          select new InvCkdFrameEngineDto
                          {
                              Id = o.Id,
                              SupplierNo = o.SupplierNo,
                              BillofladingNo = o.BillofladingNo,
                              InvoiceNo = o.InvoiceNo,
                              InvoiceDate = o.InvoiceDate,
                              LotNo = o.LotNo,
                              LotCode = o.LotCode,
                              InvoiceId = o.InvoiceId,
                              FrameNo = o.FrameNo,
                              EngineNo = o.EngineNo,
                              PartNo = o.PartNo,
                              Module = o.Module
                          };

            var totalCount = filtered.ToList().Count;

            return new PagedResultDto<InvCkdFrameEngineDto>(
                totalCount,
                results.ToList()
            );
        }



        public async Task<FileDto> GetFrameEngineToExcel(InvCkdFrameEngineExportInput input)
        {
            string _sql = "Exec INV_CKD_FRAME_ENGINE_SEARCH @Billoflading_No, @Invoice_No, @Supplier_No, @Invoice_Date_From, @Invoice_Date_To, @FrameOrEngine, @Type, @p_OrderTypeCode";

            var filtered = await _dapperRepo.QueryAsync<InvCkdFrameEngineDto>(_sql, new
            {
                Billoflading_No = input.BillofladingNo,
                Invoice_No = input.InvoiceNo,
                Supplier_No = input.SupplierNo,
                Invoice_Date_From = input.InvoiceDateFrom,
                Invoice_Date_To = input.InvoiceDateTo,
                FrameOrEngine = input.FrameEngineNo,
                Type = input.Type,
                p_OrderTypeCode = input.OrderTypeCode
            });

            var query = from o in filtered
                        select new InvCkdFrameEngineDto
                        {
                            Id = o.Id,
                            SupplierNo = o.SupplierNo,
                            BillofladingNo = o.BillofladingNo,
                            InvoiceNo = o.InvoiceNo,
                            InvoiceDate = o.InvoiceDate,
                            LotNo = o.LotNo,
                            LotCode = o.LotCode,
                            InvoiceId = o.InvoiceId,
                            FrameNo = o.FrameNo,
                            EngineNo = o.EngineNo,
                            PartNo = o.PartNo,
                            Module = o.Module
                        };

            var exportToExcel = query.ToList();
            return _FrameEngineListExcelExporter.ExportToFile(exportToExcel);
        }
    }
}
