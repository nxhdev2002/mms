using System.Collections.Generic;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Frame.Andon.Exporting;
using prod.Frame.Andon.Dto;
using prod.Storage;
using prod.Frame.Andon.Dto;
namespace prod.Frame.Andon.Exporting
{
    public class FrmAdoFramePlanBMPVExcelExporter : NpoiExcelExporterBase, IFrmAdoFramePlanBMPVExcelExporter
    {
        public FrmAdoFramePlanBMPVExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
        public FileDto ExportToFile(List<FrmAdoFramePlanBMPVDto> frameplanBMPV)
        {
            return CreateExcelPackage(
                "FrameAndonFramePlanBMPV.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("FramePlanBMPV");
                    AddHeader(
                                sheet,
                                ("No"),
                                ("Model"),
                                ("LotNo"),
                                ("NoInLot"),
                                ("BodyNo"),
                                ("Color"),
                                ("VinNo"),
                                ("FrameId"),
                                ("Status"),
                                ("PlanMonth"),
                                ("PlanDate"),
                                ("Grade"),
                                ("Version"),
                                ("IsActive")
                               );
                    AddObjects(
                         sheet, frameplanBMPV,
                                _ => _.No,
                                _ => _.Model,
                                _ => _.LotNo,
                                _ => _.NoInLot,
                                _ => _.BodyNo,
                                _ => _.Color,
                                _ => _.VinNo,
                                _ => _.FrameId,
                                _ => _.Status,
                                _ => _.PlanMonth,
                                _ => _.PlanDate,
                                _ => _.Grade,
                                _ => _.Version,
                                _ => _.IsActive
                                );
                });

        }
        public FileDto ExportToFileErr(List<FrmAdoFramePlanBMPVDto> frameplanBMPV_Err)
        {
            return CreateExcelPackage(
                "FrameAndonFramePlanBMPV_ListError.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("frameplanBMPV_Err");
                    AddHeader(
                                sheet,
                                ("No"),
                                ("Model"),
                                ("LotNo"),
                                ("NoInLot"),
                                ("BodyNo"),
                                ("Color"),
                                ("VinNo"),
                                ("FrameId"),
                                ("Status"),
                                ("PlanMonth"),
                                ("PlanDate"),
                                ("Grade"),
                                ("Version"),
                                ("IsActive"),
                                ("MessagesError")
                               );
                    AddObjects(
                         sheet, frameplanBMPV_Err,
                                _ => _.No,
                                _ => _.Model,
                                _ => _.LotNo,
                                _ => _.NoInLot,
                                _ => _.BodyNo,
                                _ => _.Color,
                                _ => _.VinNo,
                                _ => _.FrameId,
                                _ => _.Status,
                                _ => _.PlanMonth,
                                _ => _.PlanDate,
                                _ => _.Grade,
                                _ => _.Version,
                                _ => _.IsActive,
                                _ => _.MessagesError
                                );
                });

        }
    }
}

