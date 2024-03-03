using System.Collections.Generic;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.LogW.Ado.Dto;
using prod.LogW.Ado.Exporting;
using prod.Storage;

namespace prod.Master.Ado.Exporting
{
    public class LgwAdoCallingLightStatusExcelExporter : NpoiExcelExporterBase, ILgwAdoCallingLightStatusExcelExporter
    {
        public LgwAdoCallingLightStatusExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
        public FileDto ExportToFile(List<LgwAdoCallingLightStatusDto> callinglightstatus)
        {
            return CreateExcelPackage(
                "MasterAdoCallingLightStatus.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("CallingLightStatus");
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
                                    ("StartDate"),
                                    ("FinshDate"),
                                    ("Status"),
                                    ("WorkingDate"),
                                    ("Shift"),
                                    ("NoInDate"),
                                    ("NoInShift")
                                   );
                    AddObjects(
                         sheet, callinglightstatus,
                                _ => _.Code,
                                _ => _.LightName,
                                _ => _.ProdLine,
                                _ => _.Process,
                                _ => _.BlockCode,
                                _ => _.BlockDescription,
                                _ => _.Sorting,
                                _ => _.SignalId,
                                _ => _.SignalCode,
                                _ => _.StartDate,
                                _ => _.FinshDate,
                                _ => _.Status,
                                _ => _.WorkingDate,
                                _ => _.Shift,
                                _ => _.NoInDate,
                                _ => _.NoInShift

                                );
                });

        }
    }
}

