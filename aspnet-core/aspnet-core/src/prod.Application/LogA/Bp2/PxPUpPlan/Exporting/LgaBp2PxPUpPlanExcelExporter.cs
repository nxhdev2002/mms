using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.LogA.Bp2.Dto;
using prod.LogA.Bp2.Exporting;
using prod.LogA.Bp2.PxPUpPlan.Dto;
using prod.Storage;

namespace prod.LogA.Bp2.PxPUpPlan.Exporting
{
    public class LgaBp2PxPUpPlanExcelExporter : NpoiExcelExporterBase, ILgaBp2PxPUpPlanExcelExporter
    {
        public LgaBp2PxPUpPlanExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
        public FileDto ExportToFile(List<LgaBp2PxPUpPlanDto> pxpupplan)
        {
            return CreateExcelPackage(
                "LogABp2PxPUpPlan.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("PxPUpPlan");
                    AddHeader(
                                sheet,
                                ("ProdLine"),
                                ("NoOfALineIn"),
                                ("UnpackingTime"),
                                ("UnpackingDate"),
                                ("CaseNo"),
                                ("SupplierNo"),
                                ("Model"),
                                ("TotalNoInShift"),
                                ("UnpackingDatetime"),
                                ("WorkingDate"),
                                ("Shift"),
                                ("UpTable"),
                                ("UpLt"),
                                ("UnpackingStartDatetime"),
                                ("UnpackingFinishDatetime"),
                                ("UnpackingSecond"),
                                ("UnpackingBy"),
                                ("DelaySecond"),
                                ("TimeOffSecond"),
                                ("StartPauseTime"),
                                ("EndPauseTime"),
                                ("DelayConfirmFlag"),
                                ("FinishConfirmFlag"),
                                ("DelayConfirmSecond"),
                                ("TimeOffConfirmSecond"),
                                ("WhLocation"),
                                ("InvoiceDate"),
                                ("Remarks"),
                                ("IsNewPart"),
                                ("IsActive")
                               );
                    AddObjects(
                         sheet, pxpupplan,
                                _ => _.ProdLine,
                                _ => _.NoOfALineIn,
                                _ => _.UnpackingTime,
                                _ => _.UnpackingDate,
                                _ => _.CaseNo,
                                _ => _.SupplierNo,
                                _ => _.Model,
                                _ => _.TotalNoInShift,
                                _ => _.UnpackingDatetime,
                                _ => _.WorkingDate,
                                _ => _.Shift,
                                _ => _.UpTable,
                                _ => _.UpLt,
                                _ => _.UnpackingStartDatetime,
                                _ => _.UnpackingFinishDatetime,
                                _ => _.UnpackingSecond,
                                _ => _.UnpackingBy,
                                _ => _.DelaySecond,
                                _ => _.TimeOffSecond,
                                _ => _.StartPauseTime,
                                _ => _.EndPauseTime,
                                _ => _.DelayConfirmFlag,
                                _ => _.FinishConfirmFlag,
                                _ => _.DelayConfirmSecond,
                                _ => _.TimeOffConfirmSecond,
                                _ => _.WhLocation,
                                _ => _.InvoiceDate,
                                _ => _.Remarks,
                                _ => _.IsNewPart,
                                _ => _.IsActive
                                );
                });

        }
    }
}

