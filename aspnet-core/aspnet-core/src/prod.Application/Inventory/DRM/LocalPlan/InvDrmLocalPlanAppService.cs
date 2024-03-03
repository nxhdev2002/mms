using Abp.Application.Services.Dto;
using Abp.AspNetZeroCore.Net;
using Abp.Authorization;
using Abp.Dapper.Repositories;
using FastMember;
using GemBox.Spreadsheet;
using prod.Authorization;
using prod.Dto;
using prod.Inventory.DRM.Dto;
using prod.Storage;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using prod.Common;

namespace prod.Inventory.DRM
{
    [AbpAuthorize(AppPermissions.Pages_DMIHP_GR_LocalPlan_View)]
    public class InvDrmLocalPlanAppService : prodAppServiceBase, IInvDrmLocalPlanAppService
    {
        private readonly IDapperRepository<InvDrmLocalPlan, long> _dapperRepo;
        private readonly ITempFileCacheManager _tempFileCacheManager;

        public InvDrmLocalPlanAppService(IDapperRepository<InvDrmLocalPlan, long> dapperRepo,
                                        ITempFileCacheManager tempFileCacheManager)
        {
            _dapperRepo = dapperRepo;
            _tempFileCacheManager = tempFileCacheManager;
        }

        public async Task<PagedResultDto<InvDrmLocalPlanDto>> GetAll(GetInvDrmLocalPlanInput input)
        {
            string _sql = "Exec INV_DRM_LOCAL_PLAN_SEARCH @p_SupplierNo, @p_ShipmentNo, @p_Cfc, @p_PartCode, @p_MaterialCode";

            IEnumerable<InvDrmLocalPlanDto> result = await _dapperRepo.QueryAsync<InvDrmLocalPlanDto>(_sql, new
            {
                p_SupplierNo = input.SupplierNo,
                p_ShipmentNo = input.ShipmentNo,
                p_Cfc = input.Cfc,
                p_PartCode = input.PartCode,
                p_MaterialCode = input.MaterialCode,
            });

            var listResult = result.ToList();

            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var totalCount = result.ToList().Count();

            return new PagedResultDto<InvDrmLocalPlanDto>(
                totalCount,
                pagedAndFiltered);
        }


        public async Task<FileDto> GetInvDrmLocalPlanExportToFile(GetInvDrmLocalPlanExportInput input)
        {
            var file = new FileDto("InvDrmLocalPlan.xlsx", MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet);
            SpreadsheetInfo.SetLicense("EF21-1FW1-HWZF-CLQH");
            string fileName = "temp_InvDrmLocalPlan";
            string template = "wwwroot/Template";
            string path = "";
            path = Path.Combine(Directory.GetCurrentDirectory(), template, fileName);
            var xlWorkBook = ExcelFile.Load(path + ".xlsx");
            var workSheet = xlWorkBook.Worksheets[0];

            List<string> listExport = new List<string>();
            listExport.Add("SupplierNo");
            listExport.Add("DeliveryDate");
            listExport.Add("ShipmentNo");
            listExport.Add("Cfc");
            listExport.Add("PartCode");
            listExport.Add("MaterialCode");
            listExport.Add("MaterialSpec");
            listExport.Add("Qty");
            listExport.Add("DeliveryMonth");
            listExport.Add("DelayDelivery");
            listExport.Add("Remark");

            string[] properties = listExport.ToArray();

            List<string> listHeader = new List<string>();
            listHeader.Add("Supplier No");
            listHeader.Add("Delivery Date");
            listHeader.Add("Shipment No");
            listHeader.Add("Cfc");
            listHeader.Add("Part Code");
            listHeader.Add("Material Code");
            listHeader.Add("Material Spec");
            listHeader.Add("Qty");
            listHeader.Add("Delivery Month");
            listHeader.Add("Delay Delivery");
            listHeader.Add("Remark");

            string[] p_header = listHeader.ToArray();

            string _sql = "Exec INV_DRM_LOCAL_PLAN_SEARCH @p_SupplierNo, @p_ShipmentNo, @p_Cfc, @p_PartCode, @p_MaterialCode";

            IEnumerable<InvDrmLocalPlanDto> result = await _dapperRepo.QueryAsync<InvDrmLocalPlanDto>(_sql, new
            {
                p_SupplierNo = input.SupplierNo,
                p_ShipmentNo = input.ShipmentNo,
                p_Cfc = input.Cfc,
                p_PartCode = input.PartCode,
                p_MaterialCode = input.MaterialCode,
            });

            var listDataHeader = result.ToList();

            Commons.FillExcel2(listDataHeader, workSheet, 1, 0, properties, p_header);

            var tempFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".xlsx");
            xlWorkBook.Save(tempFile);
            MemoryStream obj_stream = new MemoryStream();
            obj_stream = new MemoryStream(System.IO.File.ReadAllBytes(tempFile));
            _tempFileCacheManager.SetFile(file.FileToken, obj_stream.ToArray());
            System.IO.File.Delete(tempFile);
            obj_stream.Position = 0;
            return file;
        }

    }
}
