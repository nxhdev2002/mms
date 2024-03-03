using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Inventory.CKD.Dto;
using prod.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prod.Inventory.CKD.PhysicalConfirmLot.ExportingErr
{
    public class InvCkdPhysicalConfirmLotExcelExporter : NpoiExcelExporterBase, IInvCkdPhysicalConfirmLotExcelExporter
    {
        public InvCkdPhysicalConfirmLotExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }

        public FileDto ExportToFileErr(List<InvCkdPhysicalConfirmLot_TDto> listimporterr)
        {
            return CreateExcelPackage(
                "InvCkdPhysicalConfirmLotErr.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("InvCkdPhysicalConfirmLotErr");
                    AddHeader(
                                sheet,
                                ("No"),
                                ("Model Code"),
                                ("Prod Line"),
                                ("Grade"),
                                ("Start Lot"),
                                ("Start Run"),
                                ("Start Process Date"),
                                ("End Lot"),
                                ("End Run"),
                                ("End Process Date"),
                                ("ErrorDescription")
                               );
                    AddObjects(
                         sheet, listimporterr,
                                _ => _.ROW_NO,
                                _ => _.ModelCode,
                                _ => _.ProdLine,
                                _ => _.Grade,
                                _ => _.StartLot,
                                _ => _.StartRun,
                                _ => _.StartProcessDate,
                                _ => _.EndLot,
                                _ => _.EndRun,
                                _ => _.EndProcessDate,
                                _ => _.ErrorDescription
                        );
                });
        }
    }
}
