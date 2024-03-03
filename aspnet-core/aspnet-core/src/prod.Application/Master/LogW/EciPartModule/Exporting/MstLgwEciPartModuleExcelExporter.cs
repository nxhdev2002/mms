using System.Collections.Generic;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Storage;
using prod.Master.LogW.Dto;

namespace prod.Master.LogW.Exporting
{
    public class MstLgwEciPartModuleExcelExporter : NpoiExcelExporterBase, IMstLgwEciPartModuleExcelExporter
    {
        public MstLgwEciPartModuleExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
        public FileDto ExportToFile(List<MstLgwEciPartModuleDto> ecipartmodule)
        {
            return CreateExcelPackage(
                "MasterLogWEciPartModule.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("EciPartModule");
                    AddHeader(
                                sheet,
                                "PartNo",
                                "CaseNo",
                                "SupplierNo",
                                "ContainerNo",
                                "Renban",
                                "CasePrefix",
                                "ChkEciPartId",
                                "EciType",
                                "IsActive"
                               );
                    AddObjects(
                         sheet, ecipartmodule,
                                _ => _.PartNo,
                                _ => _.CaseNo,
                                _ => _.SupplierNo,
                                _ => _.ContainerNo,
                                _ => _.Renban,
                                _ => _.CasePrefix,
                                _ => _.ChkEciPartId,
                                _ => _.EciType,
                                _ => _.IsActive
                                );

                  
                });

        }
    }
}
