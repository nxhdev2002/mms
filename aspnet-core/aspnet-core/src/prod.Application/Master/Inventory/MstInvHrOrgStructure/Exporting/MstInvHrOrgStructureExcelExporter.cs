using System.Collections.Generic;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Master.Inventory.Exporting;
using prod.Master.Inventory.Dto;
using prod.Storage;
using prod.Master.Inventory.Dto;
namespace prod.Master.Inventory.Exporting
{
    public class MstInvHrOrgStructureExcelExporter : NpoiExcelExporterBase, IMstInvHrOrgStructureExcelExporter
    {
        public MstInvHrOrgStructureExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
        public FileDto ExportToFile(List<MstInvHrOrgStructureDto> hrorgstructure)
        {
            return CreateExcelPackage(
                "MasterInventoryHrOrgStructure.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("HrOrgStructure");
                    AddHeader(
                                sheet,
                                //("Hrid"),
                                ("Code"),
                                ("Name"),
                                ("Description"),
                                ("Published"),
                                ("Orgstructuretypename"),
                                ("Orgstructuretypecode"),
                                ("Parentid"),
                                ("IsActive")

                               );
                    AddObjects(
                         sheet, hrorgstructure,
                                //_ => _.Hrid,
                                _ => _.Code,
                                _ => _.Name,
                                _ => _.Description,
                                _ => _.Published,
                                _ => _.Orgstructuretypename,
                                _ => _.Orgstructuretypecode,
                                _ => _.Parentid,
                                _ => _.IsActive

                                );
                });

        }
    }
}
