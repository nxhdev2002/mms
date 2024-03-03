using System.Collections.Generic;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.LogW.Dto;
using prod.Storage;

namespace prod.LogW.Plc.Signal.Exporting
{
    public class LgwPlcSignalExcelExporter : NpoiExcelExporterBase, ILgwPlcSignalExcelExporter
    {
        public LgwPlcSignalExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }

        public FileDto ExportToFile(List<LgwPlcSignalDto> lgwplcsignal)
        {
            return CreateExcelPackage(
               "MasterPlcSignal.xlsx",
               excelPackage =>
               {
                   var sheet = excelPackage.CreateSheet("Signal");
                   AddHeader(
                               sheet,
                               ("SignalIndex"),
                               ("SignalPattern"),
                               ("SignalTime"),
                               ("ProdLine"),
                               ("Process"),
                               ("RefId"),
                               ("IsActive")
                                  );
                   AddObjects(
                        sheet, lgwplcsignal,
                               _ => _.SignalIndex,
                               _ => _.SignalPattern,
                               _ => _.SignalTime,
                               _ => _.ProdLine,
                               _ => _.Process,
                               _ => _.RefId,
                               _ => _.IsActive
                               );

                
               });
        }
    }
}
