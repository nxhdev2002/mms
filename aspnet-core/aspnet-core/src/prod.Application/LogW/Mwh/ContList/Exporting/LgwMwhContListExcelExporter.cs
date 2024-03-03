using System.Collections.Generic;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.LogW.Mwh.Dto;
using prod.Storage;
using prod.LogW.Mwh.Dto;

namespace prod.LogW.Mwh.ContList.Exporting
{
    public class LgwMwhContListExcelExporter : NpoiExcelExporterBase, ILgwMwhContListExcelExporter
    {
        public LgwMwhContListExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
        public FileDto ExportToFile(List<LgwMwhContListDto> contlist)
        {
            return CreateExcelPackage(
                "LogWMwhContList.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("ContList");
                    AddHeader(
                                sheet,
                                "ContainerNo",
                                "Renban",
                                "SupplierNo",
                                "DevanningDate",
                                "StartDevanningDate",
                                "FinishDevanningDate",
                                "Status",
                                "ContScheduleId",
                                "Shop",
                                "IsActive"
                               );
                    AddObjects(
                         sheet, contlist,
                                _ => _.ContainerNo,
                                _ => _.Renban,
                                _ => _.SupplierNo,
                                _ => _.DevanningDate,
                                _ => _.StartDevanningDate,
                                _ => _.FinishDevanningDate,
                                _ => _.Status,
                                _ => _.ContScheduleId,
                                _ => _.Shop,
                                _ => _.IsActive
                                );

                 
                });

        }
    }
}
