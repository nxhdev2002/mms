using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.LogA.Plc.Exporting;
using prod.LogA.Plc.Signal.Dto;
using prod.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prod.LogA.Plc.Exporting
{
    public class LgaPlcSignalExcelExporter : NpoiExcelExporterBase, ILgaPlcSignalExcelExporter
    {
        public LgaPlcSignalExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }

        public FileDto ExportToFile(List<LgaPlcSignalDto> lgaplcsignal)
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
                        sheet, lgaplcsignal,
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
