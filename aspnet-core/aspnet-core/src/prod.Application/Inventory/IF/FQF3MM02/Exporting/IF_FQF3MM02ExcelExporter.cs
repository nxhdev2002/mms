using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Inventory.IF.FQF3MM02.Dto;
using prod.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prod.Inventory.IF.FQF3MM02.Exporting
{

    public class IF_FQF3MM02ExcelExporter : NpoiExcelExporterBase, IIF_FQF3MM02ExcelExporter
    {
        public IF_FQF3MM02ExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
        public FileDto ExportToFile(List<IF_FQF3MM02Dto> fqf3mm02)
        {
            return CreateExcelPackage(
                "IFIFFQF3MM02.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("FQF3MM02");
                    AddHeader(
                                sheet,
                                ("Record Id(M)"),
                                ("Company Code(M)"),
                                ("Plant Code(M)"),
                                ("Maru Code(M)"),
                                ("Receiving Stock Line(O)"),
                                ("Production Date(O)"),
                                ("Posting Date(M)"),
                                ("Part Code(M)"),
                                ("Quantity(M)"),
                                ("Spoiled Parts Quantity(O)"),
                                ("Spoiled Material Quantity1(O)"),
                                ("Material Code(M)"),
                                ("Free Shot Quantity(O)"),
                                ("Recycled Quantity(O)"),
                                ("Cost Center(M)"),
                                ("Normal Cancel Flag(M)"),
                                ("Grgi No(M)"),
                                ("Grgi Type(O)"),
                                ("Material DocType(M)"),
                                ("Material Quantity(O)"),
                                ("Spoiled Material Quantity 2(O)"),
                                ("Related Part Receive No(M)"),
                                ("Related Gr Type(M)"),
                                ("Related Gr Transaction Type(M)"),
                                ("In House Part Quantity Receive(M)"),
                                ("Related Part Issue No(O)"),
                                ("Related Gi Type(O)"),
                                ("Related Gi Transaction Type(O)"),
                                ("Related In House Part Quantity Issued(O)"),
                                ("Related Spoiled Part Quantity Issued(O)"),
                                ("Wip(O)"),
                                ("Production Id(O)"),
                                ("Final Price(M)"),
                                ("Wbs(O)"),
                                ("Earmarked Fund(O)"),
                                ("Earmarked Fund Item(O)"),
                                ("Psms Code(M)"),
                                ("Gi Uom(M)"),
                                ("Ending Of Record(M)")
                               );
                    AddObjects(
                         sheet, fqf3mm02,
                                _ => _.RecordId,
                                _ => _.CompanyCode,
                                _ => _.PlantCode,
                                _ => _.MaruCode,
                                _ => _.ReceivingStockLine,
                                _ => _.ProductionDate,
                                _ => _.PostingDate,
                                _ => _.PartCode,
                                _ => _.Quantity,
                                _ => _.SpoiledPartsQuantity,
                                _ => _.SpoiledMaterialQuantity1,
                                _ => _.MaterialCode,
                                _ => _.FreeShotQuantity,
                                _ => _.RecycledQuantity,
                                _ => _.CostCenter,
                                _ => _.NormalCancelFlag,
                                _ => _.GrgiNo,
                                _ => _.GrgiType,
                                _ => _.MaterialDocType,
                                _ => _.MaterialQuantity,
                                _ => _.SpoiledMaterialQuantity2,
                                _ => _.RelatedPartReceiveNo,
                                _ => _.RelatedGrType,
                                _ => _.RelatedGrTransactionType,
                                _ => _.InHousePartQuantityReceive,
                                _ => _.RelatedPartIssueNo,
                                _ => _.RelatedGiType,
                                _ => _.RelatedGiTransactionType,
                                _ => _.RelatedInHousePartQuantityIssued,
                                _ => _.RelatedSpoiledPartQuantityIssued,
                                _ => _.Wip,
                                _ => _.ProductionId,
                                _ => _.FinalPrice,
                                _ => _.Wbs,
                                _ => _.EarmarkedFund,
                                _ => _.EarmarkedFundItem,
                                _ => _.PsmsCode,
                                _ => _.GiUom,
                                _ => _.EndingOfRecord
                                );
                });




        }


        public FileDto ExportValidateToFile(List<GetIF_FQF3MM02_VALIDATE> fqf3mm02)
        {
            return CreateExcelPackage(
                "VALIDATEFFQF3MM02.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("VALIDATE_FQF3MM02");
                    AddHeader(
                                sheet,
                                ("ErrorDescription"),
                                ("Record Id(M)"),
                                ("Company Code(M)"),
                                ("Plant Code(M)"),
                                ("Maru Code(M)"),
                                ("Posting Date(M)"),
                                ("Part Code(M)"),
                                ("Quantity(M)"),
                                ("Material Code(M)"),
                                ("Cost Center(M)"),
                                ("Normal Cancel Flag(M)"),
                                ("Grgi No(M)"),
                                ("Material DocType(M)"),
                                ("Related Part Receive No(M)"),
                                ("Related Gr Type(M)"),
                                ("Related Gr Transaction Type(M)"),
                                ("In House Part Quantity Receive(M)"),
                                ("Final Price(M)"),
                                ("Psms Code(M)"),
                                ("Gi Uom(M)"),
                                ("Ending Of Record(M)"),
                                ("HeaderFwgId"),
                                ("HeaderId"),
                                ("TrailerId")
                             
                               );
                    AddObjects(
                         sheet, fqf3mm02,
                                _ => _.ErrorDescription,
                                _ => _.RecordId,
                                _ => _.CompanyCode,
                                _ => _.PlantCode,
                                _ => _.MaruCode,
                                _ => _.PostingDate,
                                _ => _.PartCode,
                                _ => _.Quantity,
                                _ => _.MaterialCode,
                                _ => _.CostCenter,
                                _ => _.NormalCancelFlag,
                                _ => _.GrgiNo,
                                _ => _.MaterialDocType,
                                _ => _.RelatedPartReceiveNo,
                                _ => _.RelatedGrType,
                                _ => _.RelatedGrTransactionType,
                                _ => _.InHousePartQuantityReceive,
                                _ => _.FinalPrice,
                                _ => _.PsmsCode,
                                _ => _.GiUom,
                                _ => _.EndingOfRecord,
                                _ => _.HeaderFwgId,
                                _ => _.HeaderId,
                                _ => _.TrailerId
                          
                                );
                });




        }


    }
}
