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

        public class MstGpsCostCenterExcelExporter : NpoiExcelExporterBase, IMstGpsCostCenterExcelExporter
    {
            public MstGpsCostCenterExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
            public FileDto ExportToFile(List<MstGpsCostCenterDto> mstGpsCostCenter)
            {
                return CreateExcelPackage(
                    "MstGpsCostCenter.xlsx",
                    excelPackage =>
                    {
                        var sheet = excelPackage.CreateSheet("MstGpsCostCenter");
                        AddHeader(
                                    sheet,
                                        ("Group"),
                                        ("SubGroup"),
                                        ("Division"),
                                        ("Dept"),
                                        ("Shop"),
                                        ("Team"),
										("Cost Center Category"),
										("Budget Owner"),
                                        ("Cost Center Group"),
                                        ("Cost Center Current"),
                                        ("Cost Center"),
										("Is Direct Cost Center")
								   );
                        AddObjects(
                             sheet, mstGpsCostCenter,
                                        _ => _.Group,
                                        _ => _.SubGroup,
                                        _ => _.Division,
                                        _ => _.Dept,
                                        _ => _.Shop,
                                        _ => _.Team,
										_ => _.CostCenterCategory,
										_ => _.BudgetOwner,
                                        _ => _.CostCenterGroup,
                                        _ => _.CostCenterCurrent,
                                        _ => _.CostCenter,
										_ => _.IsDirectCostCenter
									);

                        for (var i = 0; i < 8; i++)
                        {
                            sheet.AutoSizeColumn(i);
                        }
                    });
            }
        }
    }
