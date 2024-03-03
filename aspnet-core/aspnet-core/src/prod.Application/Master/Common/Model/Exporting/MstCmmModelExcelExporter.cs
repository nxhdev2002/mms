using System.Collections.Generic;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Master.Cmm.Exporting;
using prod.Master.Cmm.Dto;
using prod.Storage;
using prod.Master.Cmm.Dto;
namespace prod.Master.Cmm.Exporting
{
	public class MstCmmModelExcelExporter : NpoiExcelExporterBase, IMstCmmModelExcelExporter
	{
		public MstCmmModelExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
		public FileDto ExportToFile(List<MstCmmModelDto> model)
		{
			return CreateExcelPackage(
				"MasterCmmModel.xlsx",
				excelPackage =>
				{
					var sheet = excelPackage.CreateSheet("Model");
					AddHeader(
								sheet,
								("Code"),
								("Name"),
								("IsActive")
							   );
					AddObjects(
						 sheet, model,
								_ => _.Code,
								_ => _.Name,
								_ => _.IsActive
								);
				});

		}
        public FileDto ExportLotCodeToFile(List<MstCmmLotCodeGradeDto> lotcode)
        {
            return CreateExcelPackage(
                "MasterCommonLotCodeGrade.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("LotCodeGrade");
                    AddHeader(
                                sheet,
                                ("Model"),
                                ("LotCode"),
                                ("Cfc"),
                                ("Grade"),
                                ("GradeName"),
                                ("ModelCode"),
                                ("ModelVin")
                               );
                    AddObjects(
                         sheet, lotcode,
                                _ => _.Model,
                                _ => _.LotCode,
                                _ => _.Cfc,
                                _ => _.Grade,
                                _ => _.GradeName,
                                _ => _.ModelCode,
                                _ => _.ModelVin
                                );
                });

        }


    }
}
