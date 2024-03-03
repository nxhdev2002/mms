using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Inventory.CKD.Dto;
using prod.Inventory.CKD.Exporting;
using prod.Storage;
using prod.Storage;

namespace prod.Inventory.CKD
{
    public class InvCkdBillExcelExporter : NpoiExcelExporterBase, IInvCkdBillExcelExporter
    {
        public InvCkdBillExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
        public FileDto ExportToFile(List<InvCkdBillDto> bill)
        {
            return CreateExcelPackage(
                "InventoryCKDBill.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("Bill");
                    AddHeader(
                                sheet,
                                ("Bill Of Lading No"),
                                ("Shipment Id"),
                                ("Bill Date"),
                                ("Status")
                               );
                    AddObjects(
                         sheet, bill,
                                _ => _.BillofladingNo,
                                _ => _.ShipmentId,
                                _ => _.BillDate,
                                _ => _.Status
                                );
                });
        }

        public FileDto ExportToHistoricalFile(List<string> data)
        {
            var allHeaders = new List<string>();
            var rowDatas = new List<object>();
            var exceptCols = new List<string>()
            {
                "UpdateMask",
                "ActionId",
            };

            var status = new Dictionary<string, string>()
            {
                {"0", "NEW"},
                {"1", "PENDING"},
                {"2", "PAID"}
            };

            // get all headers
            foreach (var item in data)
            {
                var json = JObject.Parse(item);
                rowDatas.Add(json);
                foreach (var prop in json.Properties())
                {
                    if (!allHeaders.Contains(prop.Name) && !exceptCols.Contains(prop.Name))
                    {
                        allHeaders.Add(prop.Name);
                    }
                }
            }

            // generate function (lấy value từ object thành value cell)
            var funcList = new Func<object, object>[allHeaders.Count];

            for (int i = 0; i < allHeaders.Count; i++)
            {
                var header = allHeaders[i];
                if (header == "Status")
                {
                    funcList[i] = _ =>
                    {
                        if (((JObject)_)[header] != null)
                        {
                            return status[((JObject)_)[header].ToString()];
                        }
                        else
                        {
                            return "";
                        }
                    };
                } else {
                    funcList[i] = _ => ((JObject)_)[header] ?? "";
                }
            }

            return CreateExcelPackage(
                "InventoryCKDCBillOfLandingHistorical.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("BillOfLanding");
                    AddHeader(
                              sheet,
                              allHeaders.ToArray()
                    );
                    AddObjects(
                              sheet, rowDatas,
                              funcList
                    );
                });
        }
    }


}
