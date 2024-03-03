using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Inventory.IHP.Dto;
using prod.Storage;
using System.Collections.Generic;

namespace prod.Inventory.IHP.IphMatCustomDeclare.Exporting
{
    public class InvIhpMatCustomDeclareExporter : NpoiExcelExporterBase, IInvIhpMatCustomDeclareExporter
    {
        public InvIhpMatCustomDeclareExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
        public FileDto CustomDeclareExportToFile(List<InvIphMatCustomDeclareDto> customdeclare)
        {
            return CreateExcelPackage(
                "InventoryIHPMatCustomDeclare.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("IHPMatCustomDeclare");
                    AddHeader(
                            sheet,
                            ("SOTK"),
                            ("NGAY_DK"),
                            ("VAN_DON"),
                            ("SOHANG"),
                            ("MA_NT"),
                            ("TYGIA_VND"),
                            ("TONGTGKB"),
                            ("PHI_BH"),
                            ("PHI_VC"),
                            ("SO_KIEN"),
                            ("SO_HDTM"),
                            ("NGAY_HDTM"),
                            ("TONGTG_HDTM"),
                            ("THUE_VAT"),
                            ("THUE_XNK"),
                            ("TongTienThue")            
                        );
                    AddObjects(
                    sheet, customdeclare,
                        _ => _.SOTK,
                        _ => _.NGAY_DK,
                        _ => _.VAN_DON,
                        _ => _.SOHANG,
                        _ => _.MA_NT,
                        _ => _.TYGIA_VND,
                        _ => _.TONGTGKB,
                        _ => _.PHI_BH,
                        _ => _.PHI_VC,
                        _ => _.SO_KIEN,
                        _ => _.SO_HDTM,
                        _ => _.NGAY_HDTM,
                        _ => _.TONGTG_HDTM,
                        _ => _.THUE_VAT,
                        _ => _.THUE_XNK,
                        _ => _.TongTienThue
                        );
                });
        }

        public FileDto CustomDeclareDetailsExportToFile(List<InvIphMatCustomDeclareDetailsDto> customdeclaredetails)
        {
            return CreateExcelPackage(
                "InventoryIHPMatCustomDeclareDetails.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("InvIhpMatCustomDeclareDetails");
                    AddHeader(
                            sheet,
                            ("STTHANG"),
                            ("PART_SPEC"),
                            ("LUONG"),
                            ("DGIA_KB"),
                            ("TRIGIA_KB"),
                            ("TRIGIA_TT"),
                            ("TS_XNK"),
                            ("TS_VAT"),
                            ("THUE_XNK"),
                            ("THUE_VAT")
                          
                        );
                    AddObjects(
                    sheet, customdeclaredetails,
                        _ => _.STTHANG,
                        _ => _.PART_SPEC,
                        _ => _.LUONG,
                         _ => _.DGIA_KB,
                        _ => _.TRIGIA_KB,
                        _ => _.TRIGIA_TT,
                        _ => _.TS_XNK,
                        _ => _.TS_VAT,
                         _ => _.THUE_XNK,
                        _ => _.THUE_VAT
                        );
                });

        }
    }
}

