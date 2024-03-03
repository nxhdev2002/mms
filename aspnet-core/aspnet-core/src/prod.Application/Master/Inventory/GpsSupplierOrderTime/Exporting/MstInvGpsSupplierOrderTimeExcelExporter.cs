using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Master.Inv.Dto;
using prod.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prod.Master.Inv.Exporting
{
    public class MstInvGpsSupplierOrderTimeExcelExporter : NpoiExcelExporterBase, IMstInvGpsSupplierOrderTimeExcelExporter
    {
        public MstInvGpsSupplierOrderTimeExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
        public FileDto ExportToFile(List<MstInvGpsSupplierOrderTimeDto> mstinvgpssupplierordertime)
        {
            return CreateExcelPackage(
                "MasterInvMstInvGpsSupplierOrderTime.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("MstInvGpsSupplierOrderTime");
                    AddHeader(
                                sheet,
                                ("SupplierId"),
                                ("OrderSeq"),
                                ("OrderType"),
                                ("OrderTime"),
                                ("ReceivingDay"),
                                ("ReceiveTime"),
                                ("KeihenTime"),
                                ("KeihenDay"),
                                ("IsActive")
                               );
                    AddObjects(
                         sheet, mstinvgpssupplierordertime,
                                _ => _.SupplierId,
                                _ => _.OrderSeq,
                                _ => _.OrderType,
                                _ => _.OrderTime,
                                _ => _.ReceivingDay,
                                _ => _.ReceiveTime,
                                _ => _.KeihenTime,
                                _ => _.KeihenDay,
                                _ => _.IsActive
                                );

             
                });

        }
    }
}
