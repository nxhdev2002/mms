using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Master.Inventory.Dto;
using prod.Storage;
using System;
using System.Collections.Generic;
using System.Linq;

namespace prod.Master.Inventory.Exporting
{
    public class MstInvDemDetDaysExcelExporter : NpoiExcelExporterBase, IMstInvDemDetDaysExcelExporter
    {
        public MstInvDemDetDaysExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
        public FileDto ExportToFile(List<MstInvDemDetDaysDto> mstinvdemdetdays)
        {
            var ins = new MstInvDemDetDaysDto();
            var properties = ins.GetType().GetProperties().Select(x => x.Name).ToArray();
            var funcList = new List<Func<MstInvDemDetDaysDto, object>>();
            foreach (var item in properties)
            {
                object x(MstInvDemDetDaysDto item) => item;
                funcList.Add(x);
            }

            var funcs = funcList.ToArray();

            return CreateExcelPackage(
                    "MasterDEMDETDAYS.xlsx",
                    excelPackage =>
                    {
                        var sheet = excelPackage.CreateSheet("MstInvDemDetDays");
                        AddHeader(
                                    sheet,
                                    ("Exp"),
                                    ("Carrier"),
                                    ("CombineDEMDET"),
                                    ("FreeDEM"),
                                    ("FreeDET"),
                                    ("CombineDEMDETFree")
                                       );
                        AddObjects(
                             sheet, mstinvdemdetdays,
                                    _ => _.Exp,
                                    _ => _.Carrier,
                                    _ => _.CombineDEMDET,
                                    _ => _.FreeDEM,
                                    _ => _.FreeDET,
                                    _ => _.CombineDEMDETFree

                                    );

                    });
        }
        public FileDto ExportToFileErr(List<MstInvDemDetDaysImportDto> demdetdays_err)
        {
            return CreateExcelPackage(
                "MstInvDemDetDaysError.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("DemDetDaysErr");
                    AddHeader(
                                sheet,
                                ("Exp"),
                                ("Carrier"),
                                ("CombineDEMDET"),
                                ("FreeDEM"),
                                ("FreeDET"),
                                ("CombineDEMDETFree"),
                                ("ErrorDescription")
                               );
                    AddObjects(
                         sheet, demdetdays_err,
                                _ => _.Exp,
                                _ => _.Carrier,
                                _ => _.CombineDEMDET,
                                _ => _.FreeDEM,
                                _ => _.FreeDET,
                                _ => _.CombineDEMDETFree,
                                _ => _.ErrorDescription
                                );

                });
        }
    }
}
