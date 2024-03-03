using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Master.Inventory.Dto;
using prod.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prod.Master.Inventory.Exporting
{
    public class MstInvDemDetFeesExcelExporter : NpoiExcelExporterBase, IMstInvDemDetFeesExcelExporter
    {
        public MstInvDemDetFeesExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
        public FileDto ExportToFile(List<MstInvDemDetFeesDto> DemDetFees)
        {
            return CreateExcelPackage(
                "MstInvDemDetFees.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("DemDetFees");
                    AddHeader(
                                sheet,
                                ("Source"),
                                ("Carrier"),
                                ("Cont Type"),
                                ("Minday"),
                                ("MaxDay"),
                                ("EffectiveDateFrom"),
                                ("EffectiveDateTo"),
                                ("DemFee"),
                                ("DetFee"),
                                ("DemAndDetFee"),
                                ("Is Active")
                               );
                    AddObjects(
                         sheet, DemDetFees,
                                _ => _.Source,
                                _ => _.Carrier,
                                _ => _.ContType,
                                _ => _.MinDay,
                                _ => _.MaxDay,
                                _ => _.EffectiveDateFrom,
                                _ => _.EffectiveDateTo,
                                _ => _.DemFee,
                                _ => _.DetFee,
                                _ => _.DemAndDetFee,
                                _ => _.IsActive
                                );


                });

        }

        public FileDto ExportToFileErr(List<MstInvDemDetFeesImportDto> DemDetFees_err)
        {
            return CreateExcelPackage(
                "MstInvDemDetFeesErr.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("DemDetFeesErr");
                    AddHeader(
                                sheet,
                                ("Source"),
                                ("Carrier"),
                                ("Cont Type"),
                                ("NoOfDayOVF"),
                                ("DemFee"),
                                ("DetFee"),
                                ("DemAndDetFee"),
                                ("ErrorDescription")
                               );
                    AddObjects(
                         sheet, DemDetFees_err,
                                _ => _.Source,
                                _ => _.Carrier,
                                _ => _.ContType,
                                _ => _.NoOfDayOVF,
                                _ => _.DemFee,
                                _ => _.DetFee,
                                _ => _.DemAndDetFee,
                                _ => _.ErrorDescription
                                );


                });

        }
    }
}
