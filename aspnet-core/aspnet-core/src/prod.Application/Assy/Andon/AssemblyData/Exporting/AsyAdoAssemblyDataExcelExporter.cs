using System.Collections.Generic;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Storage;
using prod.Assy.Andon.Dto;

namespace prod.Assy.Andon.Exporting
{
    public class AsyAdoAssemblyDataExcelExporter : NpoiExcelExporterBase, IAsyAdoAssemblyDataExcelExporter
    {
        public AsyAdoAssemblyDataExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
        public FileDto ExportToFile(List<AsyAdoAssemblyDataDto> assemblydata)
        {
            return CreateExcelPackage(
                "AssemblyAndonAssemblyData.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("AssemblyData");
                    AddHeader(
                                sheet,
                                ("WorkingDate"),
                                ("Shift"),
                                ("NoInDate"),
                                ("Line"),
                                ("Process"),
                                ("Model"),
                                ("Body"),
                                ("SeqNo"),
                                ("Grade"),
                                ("LotNo"),
                                ("NoInLot"),
                                ("LotNoIndex"),
                                 ("NoInShift"),
                                ("Color"),
                                ("CreateDate")

                               );
                    AddObjects(
                         sheet, assemblydata,
                                _ => _.WorkingDate,
                                _ => _.Shift,
                                _ => _.NoInDate,
                                _ => _.Line,
                                _ => _.Process,
                                _ => _.Model,
                                _ => _.Body,
                                _ => _.SeqNo,
                                _ => _.Grade,
                                _ => _.LotNo,
                                _ => _.NoInLot,
                                _ => _.LotNoIndex,
                                 _ => _.NoInShift,
                                _ => _.Color,
                                _ => _.CreateDate
                                );

               
                });

        }
    }
}
