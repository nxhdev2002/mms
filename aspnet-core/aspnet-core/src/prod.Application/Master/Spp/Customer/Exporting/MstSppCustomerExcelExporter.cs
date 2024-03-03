using System.Collections.Generic;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Master.Spp.Exporting;
using prod.Master.Spp.Dto;
using prod.Storage;
using prod.Master.Spp.Dto;
using NPOI.SS.Formula.Functions;
using NPOI.SS.UserModel;
using PayPalCheckoutSdk.Orders;
using Stripe;

namespace prod.Master.Spp.Exporting
{
    public class MstSppCustomerExcelExporter : NpoiExcelExporterBase, IMstSppCustomerExcelExporter
    {
        public MstSppCustomerExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
        public FileDto ExportToFile(List<MstSppCustomerDto> customer)
        {
            return CreateExcelPackage(
                "MasterSppCustomer.xlsx",
                excelPackage =>
                {
                var sheet = excelPackage.CreateSheet("Customer");
                AddHeader(
                            sheet,
                            ("Code"),
								("Name"),
								("Rep"),
								("FromMonth"),
								("FromYear"),
								("ToMonth"),
								("ToYear"),
								("FromPeriodId"),
								("ToPeriodId"),
								("IsNew"),
								("OraCustomerId")
							   );
            AddObjects(
                 sheet, customer,
                        _ => _.Code,
                        _ => _.Name,
                        _ => _.Rep,
                        _ => _.FromMonth,
                        _ => _.FromYear,
                        _ => _.ToMonth,
                        _ => _.ToYear,
                        _ => _.FromPeriodId,
                        _ => _.ToPeriodId,
                        _ => _.IsNew,
                        _ => _.OraCustomerId

                        );

            for (var i = 0; i < 8; i++)
            {
                sheet.AutoSizeColumn(i);
            }
        });

        }
}
}
