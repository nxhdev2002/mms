using Microsoft.Extensions.Logging;
using prod.Common;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Master.Inv.Dto;
using prod.Master.Inventory.Exporting;
using prod.Master.Pio.Dto;
using prod.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace prod.Master.Pio.Exporting
{
    public class MstPioImpSupplierExcelExporter : NpoiExcelExporterBase, IMstPioImpSupplierExcelExporter
    {
        public MstPioImpSupplierExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
        public FileDto ExportToFile(List<MstPioImpSupplierDto> pioparttype)
        {
            return CreateExcelPackage(
                "MstPioImpSupplier.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("PIOPartType");
                    AddHeader(
                                sheet,
                                ("Supplier No"),
                                ("Supplier Name"),
                                ("Remarks"),
                                ("Supplier Type"),
                                ("Supplier Name Vn"),
                                ("Exporter"),
                                ("IsActive")

                               );
                    AddObjects(
                         sheet, pioparttype,
                                _ => _.SupplierNo,
                                _ => _.SupplierName,
                                _ => _.Remarks,
                                _ => _.SupplierType,
                                _ => _.SupplierNameVn,
                                _ => _.Exporter,
                                _ => _.IsActive

                                );

                    for (var i = 0; i < 8; i++)
                    {
                        sheet.AutoSizeColumn(i);
                    }
                });

        }
    }
}
