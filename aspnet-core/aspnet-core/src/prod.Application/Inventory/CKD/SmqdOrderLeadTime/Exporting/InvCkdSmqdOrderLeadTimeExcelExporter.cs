using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Inventory.CKD.SMQD.Dto;
using prod.Inventory.CKD.SmqdOrderLeadTime.Dto;
using prod.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prod.Inventory.CKD.SmqdOrderLeadTime.Exporting
{

    public class InvCkdSmqdOrderLeadTimeExcelExporter : NpoiExcelExporterBase, IInvCkdSmqdOrderLeadTimeExcelExporter
    {
        public InvCkdSmqdOrderLeadTimeExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
        public FileDto ExportToFile(List<InvCkdSmqdOrderLeadTimeDto> smqdorderleadtime)
        {
            return CreateExcelPackage(
                "InventoryCKDSmqdOrderLeadTime.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("SmqdOrderLeadTime");
                    AddHeader(
                                sheet,
                                ("SupplierNo"),
                                ("Cfc"),
                                ("ExpCode"),
                                ("Sea"),
                                ("Air")
                               );
                    AddObjects(
                         sheet, smqdorderleadtime,
                                _ => _.SupplierNo,
                                _ => _.Cfc,
                                _ => _.ExpCode,
                                _ => _.Sea,
                                _ => _.Air
                                );

                });

        }

        public FileDto ExportToFileErrOrderLeadTime(List<InvCkdSmqdOrderLeadImportDto> listimporterrorderleadtime)
        {
            return CreateExcelPackage(
                "InvCkdSmqdOrderLeadTimeImportErr.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("InvCkdSmqdListImportErr");
                    AddHeader(
                                sheet,
                                ("SupplierNo"),
                                ("Cfc"),
                                ("ExpCode"),
                                ("Sea"),
                                ("Air"),
                                ("ErrorDescription")
                               );
                    AddObjects(
                         sheet, listimporterrorderleadtime,
                                _ => _.SupplierNo,
                                _ => _.Cfc,
                                _ => _.ExpCode,
                                _ => _.Sea,
                                _ => _.Air,
                                _ => _.ErrorDescription
                                );
                });
        }
    }
}
