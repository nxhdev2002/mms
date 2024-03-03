using Abp.Application.Services.Dto;
using Abp.AspNetZeroCore.Net;
using Abp.Authorization;
using Abp.Dapper.Repositories;
using GemBox.Spreadsheet;
using prod.Authorization;
using prod.Common;
using prod.Dto;
using prod.Inventory.CKD.RundownShippingSchedule.Dto;
using prod.Storage;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace prod.Inventory.CKD.RundownShippingSchedule
{
    [AbpAuthorize(AppPermissions.Pages_Ckd_Stock_Rundown_ShipingSchedule_View)]
    public class InvCkdRundownShippingScheduleAppService : prodAppServiceBase, IInvCkdRundownShippingScheduleAppService
    {
        private readonly IDapperRepository<InvCkdStockRundown, long> _dapperRepo;
        private readonly ITempFileCacheManager _tempFileCacheManager;

        public InvCkdRundownShippingScheduleAppService(
            IDapperRepository<InvCkdStockRundown, long> dapperRepo,
            ITempFileCacheManager tempFileCacheManager)
        {
            _dapperRepo = dapperRepo;
            _tempFileCacheManager = tempFileCacheManager;
        }

        public async Task<PagedResultDto<InvCkdRundownShippingScheduleDto>> GetAll(GetStockRundownShippingScheduleInput input)
        {
            string _sql = "Exec INV_CKD_STOCK_SHIP_SCHEDULE_RUNDOWN_SEARCH @p_PartNo, @p_CfcCode, @p_SupplierCode";

            IEnumerable<InvCkdRundownShippingScheduleDto> result = await _dapperRepo.QueryAsync<InvCkdRundownShippingScheduleDto>(_sql, new
            {
                p_PartNo = input.PartNo,
                p_CfcCode = input.Cfc,
                p_SupplierCode = input.SupplierCode
            });

            var listResult = result.ToList();

            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var totalCount = result.ToList().Count();

            return new PagedResultDto<InvCkdRundownShippingScheduleDto>(
                totalCount, pagedAndFiltered);

        }

        public async Task<int> CalculatorRundown()
        {
            Dapper.SqlMapper.Settings.CommandTimeout = 900;
            try
            {
                string _sql = "EXEC JOB_INV_CKD_STOCK_SHIP_SCHEDULE_RUNDOWN_CREATE";
                var filtered = await _dapperRepo.ExecuteAsync(_sql, new { });
                return filtered;
            }
            catch (Exception E)
            {
                return 0;
            }
        }

        public async Task<FileDto> GetExportShipScheduleRundownToExcel(GetStockRundownShippingScheduleExportInput input)
        {
            string exportfile = "InvCKDStockRDShippingSchedule.xlsx";
            var file = new FileDto(exportfile, MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet);
            SpreadsheetInfo.SetLicense("EF21-1FW1-HWZF-CLQH");
            string fileName = "temp_InventoryCKDStockRundown";
            string template = "wwwroot/Template";
            string path = "";
            path = Path.Combine(Directory.GetCurrentDirectory(), template, fileName);
            var xlWorkBook = ExcelFile.Load(path + ".xlsx");
            var workSheet = xlWorkBook.Worksheets[0];

            DateTime dt = DateTime.Now;
            int colstartRD = 3;
            int colendRD = 63;
            for (int i = colstartRD; i < colendRD; i++)
            {
                workSheet.Cells[0, i].Value = dt.ToString("d - MMM", new CultureInfo("en-US"));
                dt = dt.AddDays(1);
            }

            string _sql = "Exec INV_CKD_STOCK_SHIP_SCHEDULE_RUNDOWN_SEARCH @p_PartNo, @p_CfcCode, @p_SupplierCode";

            IEnumerable<InvCkdRundownShippingScheduleDto> result = await _dapperRepo.QueryAsync<InvCkdRundownShippingScheduleDto>(_sql, new
            {
                p_PartNo = input.PartNo,
                p_CfcCode = input.Cfc,
                p_SupplierCode = input.SupplierCode
            });

            List<InvCkdRundownShippingScheduleDto> listResult = result.ToList();

            List<string> listHeader = new List<string>();
            for (int i = 0; i < colendRD; i++)
            {
                listHeader.Add(workSheet.Cells[0, i].Value.ToString());
            }

            List<string> listExport = new List<string>();
            listExport.Add("PartNo");
            listExport.Add("Cfc");
            listExport.Add("SupplierNo");
            listExport.Add("A1");
            listExport.Add("A2");
            listExport.Add("A3");
            listExport.Add("A4");
            listExport.Add("A5");
            listExport.Add("A6");
            listExport.Add("A7");
            listExport.Add("A8");
            listExport.Add("A9");
            listExport.Add("A10");
            listExport.Add("A11");
            listExport.Add("A12");
            listExport.Add("A13");
            listExport.Add("A14");
            listExport.Add("A15");
            listExport.Add("A16");
            listExport.Add("A17");
            listExport.Add("A18");
            listExport.Add("A19");
            listExport.Add("A20");
            listExport.Add("A21");
            listExport.Add("A22");
            listExport.Add("A23");
            listExport.Add("A24");
            listExport.Add("A25");
            listExport.Add("A26");
            listExport.Add("A27");
            listExport.Add("A28");
            listExport.Add("A29");
            listExport.Add("A30");
            listExport.Add("A31");
            listExport.Add("A32");
            listExport.Add("A33");
            listExport.Add("A34");
            listExport.Add("A35");
            listExport.Add("A36");
            listExport.Add("A37");
            listExport.Add("A38");
            listExport.Add("A39");
            listExport.Add("A40");
            listExport.Add("A41");
            listExport.Add("A42");
            listExport.Add("A43");
            listExport.Add("A44");
            listExport.Add("A45");
            listExport.Add("A46");
            listExport.Add("A47");
            listExport.Add("A48");
            listExport.Add("A49");
            listExport.Add("A50");
            listExport.Add("A51");
            listExport.Add("A52");
            listExport.Add("A53");
            listExport.Add("A54");
            listExport.Add("A55");
            listExport.Add("A56");
            listExport.Add("A57");
            listExport.Add("A58");
            listExport.Add("A59");
            listExport.Add("A60");

            string[] properties = listExport.ToArray();
            string[] p_header = listHeader.ToArray();
            Commons.FillExcel2(listResult, workSheet, 1, 0, properties, p_header);

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
