using System.Collections.Generic;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Inventory.CKD.Exporting;
using prod.Inventory.CKD.Dto;
using prod.Storage;

namespace prod.Inventory.CKD.Exporting
{
    public class InvCkdPhysicalStockPartExcelExporter : NpoiExcelExporterBase, IInvCkdPhysicalStockPartExcelExporter
    {
        public InvCkdPhysicalStockPartExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
        public FileDto ExportToFile(List<InvCkdPhysicalStockPartDto> physicalstockpart)
        {
            return CreateExcelPackage(
                "InventoryCKDPhysicalStockPart.xlsx",
                excelPackage =>
                {
                var sheet = excelPackage.CreateSheet("PhysicalStockPart");
                AddHeader(
                            sheet,
                                ("PartNo"),
                                ("Cfc"),
                                ("SupplierNo"),
								("PartNoNormalized"),
								("PartName"),
								("PartNoNormalizedS4"),
								("ColorSfx"),
								("LotNo"),
								("BeginQty"),
								("ReceiveQty"),
								("IssueQty"),
								("CalculatorQty"),
								("ActualQty"),
								("PeriodId"),
								("LastCalDatetime")
							   );
            AddObjects(
                 sheet, physicalstockpart,
                        _ => _.PartNo,
                        _ => _.Cfc,
                        _ => _.SupplierNo,
                        _ => _.PartNoNormalized,
                        _ => _.PartName,
                        _ => _.PartNoNormalizedS4,
                        _ => _.ColorSfx,
                        _ => _.LotNo,
                        _ => _.BeginQty,
                        _ => _.ReceiveQty,
                        _ => _.IssueQty,
                        _ => _.CalculatorQty,
                        _ => _.ActualQty,
                        _ => _.PeriodId,
                        _ => _.LastCalDatetime
                        );

        });

        }

        public FileDto ExportListErrToFile(List<InvCkdPhysicalStockErrDto> listerrphysicalstockpart)
        {
            return CreateExcelPackage(
                "InvCkdPhysicalStockPart_ListErr.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("PhysicalStockPart_ListErr");
                    AddHeader(
                                sheet,
                                ("PartNo"),
                                ("Cfc"),
                                ("SupplierNo"),
                                ("Qty"),
                                ("ErrorDescription")                              
                                );
                    AddObjects(
                         sheet, listerrphysicalstockpart,
                                _ => _.PartNo,
                                _ => _.Cfc,
                                _ => _.SupplierNo,
                                _ => _.Qty,
                                _ => _.ErrorDescription
                                );
                });

        }

        public FileDto ExportListLotErrToFile(List<InvCkdPhysicalStockErrDto> listerrphysicalstocklot)
        {
            return CreateExcelPackage(
                "InvCkdPhysicalStockLot_ListErr.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("PhysicalStockPartLot_ListErr");
                    AddHeader(
                                sheet,
                                ("LotNo"),
                                ("Shop"),
                                ("Qty"),
                                ("ErrorDescription")
                                );
                    AddObjects(
                         sheet, listerrphysicalstocklot,
                                _ => _.LotNo,
                                _ => _.Shop,
                                _ => _.Qty,
                                _ => _.ErrorDescription
                                );
                });

        }

        public FileDto ExportSummaryStockByPart(List<InvCkdPhysicalStockPartDto> listdata)
        {
            return CreateExcelPackage(
                "InvCKDPhysicalSummaryStockByPart.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("SummaryStockByPart");
                    AddHeader(
                        sheet,
                        ("Part No"),
                        ("Supplier No"),
                        ("Part No Normalized"),
                        ("Part Name"),
                        ("Part No Normalized S4"),
                        ("Color Sfx"),
                        ("Lot No"),
                        ("Begin Qty"),
                        ("Regular Receive Qty"),
                        ("Regular Issue Qty"),
                        ("PxP In Qty"),
                        ("PxP Return Qty"),
                        ("In Other Qty"),
                        ("PxP Out Qty"),
                        ("Out Other Qty"),
                        ("Calculator Qty"),
                        ("Actual Qty"),
                        ("Period Id"),
                        ("Last Cal Datetime")
                    );
                    AddObjects(
                        sheet, listdata,
                        _ => _.PartNo,
                        _ => _.SupplierNo,
                        _ => _.PartNoNormalized,
                        _ => _.PartName,
                        _ => _.PartNoNormalizedS4,
                        _ => _.ColorSfx,
                        _ => _.LotNo,
                        _ => _.BeginQty,
                        _ => _.RegularReceiveQty,
                        _ => _.RegularIssueQty,
                        _ => _.SMQDPxPInQty,
                        _ => _.SMQDPxPReturnQty,
                        _ => _.SMQDInOtherQty,
                        _ => _.SMQDPxPOutQty,
                        _ => _.SMQDOutOtherQty,
                        _ => _.CalculatorQty,
                        _ => _.ActualQty,
                        _ => _.PeriodId,
                        _ => _.LastCalDatetime
                    );
                });
        }
    }
}