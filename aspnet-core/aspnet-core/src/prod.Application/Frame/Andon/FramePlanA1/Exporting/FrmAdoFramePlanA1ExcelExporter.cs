using System.Collections.Generic;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Frame.Andon.Exporting;
using prod.Frame.Andon.Dto;
using prod.Storage;
using prod.Frame.Andon.Dto;
namespace prod.Frame.Andon.Exporting
{
	public class FrmAdoFramePlanA1ExcelExporter : NpoiExcelExporterBase, IFrmAdoFramePlanA1ExcelExporter
	{
		public FrmAdoFramePlanA1ExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
		public FileDto ExportToFile(List<FrmAdoFramePlanA1Dto> frameplana1)
		{
			return CreateExcelPackage(
				"FrameAndonFramePlanA1.xlsx",
				excelPackage =>
				{
					var sheet = excelPackage.CreateSheet("FramePlanA1");
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
						 sheet, frameplana1,
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

        public FileDto ExportToFileErr(List<FrmAdoFramePlanA1Dto> frameplana1_Err)
        {
            return CreateExcelPackage(
                "FrameAndonFramePlanA1_ListErr.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("FramePlanA1_Err");
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
                         sheet, frameplana1_Err,
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

