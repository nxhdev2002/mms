using System.Collections.Generic;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Master.LogW.Dto;
using prod.Master.LogW.Exporting;
using prod.Storage;

namespace prod.Master.LogW.Exporting
{
    public class MstLgwAdoCallingLightExcelExporter : NpoiExcelExporterBase, IMstLgwAdoCallingLightExcelExporter
    {
        public MstLgwAdoCallingLightExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
        public FileDto ExportToFile(List<MstLgwAdoCallingLightDto> callinglight)
        {
                return CreateExcelPackage(
                    "MasterLogWCallingLight.xlsx",
                    excelPackage =>
                    {
                    var sheet = excelPackage.CreateSheet("CallingLight");
                    AddHeader(
                                sheet,
                                ("Code"),
								("LightName"),
								("ProdLine"),
								("Process"),
								("BlockCode"),
								("BlockDescription"),
								("Sorting"),
								("SignalId"),
								("SignalCode"),
                                ("IsActive")
							    );
                AddObjects(
                        sheet, callinglight,
                            _ => _.Code,
                            _ => _.LightName,
                            _ => _.ProdLine,
                            _ => _.Process,
                            _ => _.BlockCode,
                            _ => _.BlockDescription,
                            _ => _.Sorting,
                            _ => _.SignalId,
                            _ => _.SignalCode,
                            _ => _.IsActive
    

                            );
            });

        }
    }
}
