using NPOI.POIFS.Properties;
using NPOI.SS.UserModel;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Master.Inventory.LotPart.Dto;
using prod.Storage;
using Stripe;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prod.Master.Inventory.LotPart.Exporting
{
    public class MstInvLotPartExcelExporter : NpoiExcelExporterBase, IMstInvLotPartExcelExporter
    {
        public MstInvLotPartExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
        public FileDto ExportToFile(List<MstInvLotPartDto> invlotpart)
        {
            return CreateExcelPackage(
                "MasterInvInvLotPart.xlsx",
            excelPackage =>
            {
                var sheet = excelPackage.CreateSheet("InvLotPart");
                AddHeader(
                            sheet,
                            ("Part No"),
                            ("Source"),
                            ("Carfamily Code"),
                            ("Carfamily Name"),
                            ("Line Code"),
                            ("Color"),
                            ("Part Name"),
                            ("Is Active")
                        );
                AddObjects(
                    sheet, invlotpart,
                        _ => _.Part_No,
                        _ => _.Source,
                        _ => _.Carfamily_Code,
                        _ => _.Carfamily_Name,
                        _ => _.Line_Code,
                        _ => _.Color,
                        _ => _.Part_Name,
                        _ => _.Active
                        );
            });

        }
    }
}
