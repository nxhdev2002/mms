using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Master.Inventory;
using prod.Storage;
using System.Collections.Generic;
using prod.Master.Inventory.Exporting;
using prod.Master.Inventory.MstInvGenBOMData.Dto;
using prod.Inventory.CPS.Dto;

namespace vovina.Master.Inventory.Exporting
{
    public class MstInvGenBOMDatasExcelExporter : NpoiExcelExporterBase, IMstInvGenBOMDatasExcelExporter
    {
        public MstInvGenBOMDatasExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
        public FileDto ExportToFile(List<MstInvGenBOMDataDto> mstinvgenbomdatas)
        {
            return CreateExcelPackage(
    "MstInvGenBOMDatas.xlsx",
    excelPackage =>
    {
        var sheet = excelPackage.CreateSheet("InvGenBOMDatas");
        AddHeader(
                    sheet,
                        ("Fileid"),
                        ("DataField Name"),
                        ("DataField Lengh"),
                        ("DataField Type"),
                        ("Data Field Description")
        );
        AddObjects(
             sheet, mstinvgenbomdatas,
                    _ => _.FileId,
                    _ => _.DataFieldName,
                    _ => _.DataFieldLengh,
                    _ => _.DataFieldType,
                     _ => _.DataFieldDescription



                    );
    });
        }
    }
}
