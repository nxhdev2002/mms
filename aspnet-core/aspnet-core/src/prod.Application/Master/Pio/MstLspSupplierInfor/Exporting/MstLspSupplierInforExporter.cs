using Abp.Application.Services;
using Abp.AspNetZeroCore.Net;
using GemBox.Spreadsheet;
using Microsoft.Extensions.Logging;
using prod.Common;
using prod.Dto;
using prod.Master.Inv.Dto;
using prod.Master.Pio.DTO;
using prod.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace prod.Master.Pio.Exporting
{
    public interface IMstLspSupplierInforExporter : IApplicationService
    {
        FileDto ExportToFile(List<MstLspSupplierInforDto> mstlspsupplierinfor);
    }
    public class MstLspSupplierInforExporter : IMstLspSupplierInforExporter
    {
        private readonly ITempFileCacheManager _tempFileCacheManager;
        private readonly ILogger<MstLspSupplierInforExporter> _logger;

        public MstLspSupplierInforExporter(ITempFileCacheManager tempFileCacheManager, ILogger<MstLspSupplierInforExporter> logger)
        {
            _tempFileCacheManager = tempFileCacheManager;
            _logger = logger;
        }

        public FileDto ExportToFile(List<MstLspSupplierInforDto> mstlspsupplierinfo)
        {
            string fileName = "MasterLspSupplierInfo.xlsx";
            var tempFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".xlsx");
            var file = new FileDto(fileName, MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet);

            try
            {
                SpreadsheetInfo.SetLicense("EF21-1FW1-HWZF-CLQH");
                var xlWorkBook = new ExcelFile();
                var workSheet = xlWorkBook.Worksheets.Add("Sheet1");

                var ins = new MstInvGpsSupplierInfoDto();
                var properties = ins.GetType().GetProperties()
                                            .Where(x => x.Name != "Id")
                                            .Select(x => x.Name)
                                            .ToArray();

                Commons.FillExcel2(mstlspsupplierinfo, workSheet, 1, 0, properties, properties);
                xlWorkBook.Save(tempFile);
                using (var obj_stream = new MemoryStream(File.ReadAllBytes(tempFile)))
                {
                    _tempFileCacheManager.SetFile(file.FileToken, obj_stream.ToArray());
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"[EXCEPTION] While exporting {nameof(MstLspSupplierInforExporter)} with error: {ex}");
            }
            finally
            {
                File.Delete(tempFile);
            }

            return file;
        }
    }
}
