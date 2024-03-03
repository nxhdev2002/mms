using System.Collections.Generic;
using prod.Master.LogA.Exporting;
using prod.Storage;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Master.LogA.Bp2PartListGrade.Dto;

namespace prod.Master.LogA.Bp2PartListGrade.Exporting
{
    public class MstLgaBp2PartListGradeExcelExporter : NpoiExcelExporterBase, IMstLgaBp2PartListGradeExcelExporter
    {
        public MstLgaBp2PartListGradeExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
        public FileDto ExportToFile(List<MstLgaBp2PartListGradeDto> bp2partlistgrade)
        {
            return CreateExcelPackage(
                "MasterLogABp2PartListGrade.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("Bp2PartListGrade");
                    AddHeader(
                        sheet,
                        ("PartNo"),
                        ("PartName"),
                        ("PartListId"),
                        ("ProdLine"),
                        ("Model"),
                        ("Grade"),
                        ("UsageQty"),
                        ("PikLocType"),
                        ("PikAddress"),
                        ("PikAddressDisplay"),
                        ("DelLocType"),
                        ("DelAddress"),
                        ("DelAddressDisplay"),
                        ("Sorting"),
                        ("Remark"),
                        ("IsActive")                     
                        );

                    AddObjects(
                        sheet, bp2partlistgrade,
                        _ => _.PartNo,
                        _ => _.PartName,
                        _ => _.PartListId,
                        _ => _.ProdLine,
                        _ => _.Model,
                        _ => _.Grade,
                        _ => _.UsageQty,
                        _ => _.PikLocType,
                        _ => _.PikAddress,
                        _ => _.PikAddressDisplay,
                        _ => _.DelLocType,
                        _ => _.DelAddress,
                        _ => _.DelAddressDisplay,
                        _ => _.Sorting,
                         _ => _.Remark,
                        _ => _.DelLocType
                        );
                    

                }
                );
        }
    }
}
