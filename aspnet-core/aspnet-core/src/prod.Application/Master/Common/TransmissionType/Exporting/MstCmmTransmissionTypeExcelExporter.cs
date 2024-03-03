using System.Collections.Generic;
using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Master.Common.Exporting;
using prod.Master.Common.Dto;
using prod.Storage;
using prod.Master.Common.Dto;
namespace prod.Master.Common.Exporting
{
    public class MstCmmTransmissionTypeExcelExporter : NpoiExcelExporterBase, IMstCmmTransmissionTypeExcelExporter
    {
        public MstCmmTransmissionTypeExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
        public FileDto ExportToFile(List<MstCmmTransmissionTypeDto> transmissiontype)
        {
            return CreateExcelPackage(
                "MasterCommonTransmissionType.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("TransmissionType");
                    AddHeader(
                                sheet,
                                ("Code"),
                                ("Name"),
                                ("IsActive")

                               );
                    AddObjects(
                         sheet, transmissiontype,
                                _ => _.Code,
                                _ => _.Name,
                                _ => _.IsActive

                                );

              
                });

        }
    }
}
