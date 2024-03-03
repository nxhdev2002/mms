using prod.DataExporting.Excel.NPOI;
using prod.Dto;
using prod.Inventory.CPS.SapAssetMaster.Dto;
using prod.Inventory.SPP.Shipping.Dto;
using prod.Inventory.SPP.Shipping.Exporting;
using prod.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prod.Inventory.CPS.SapAssetMaster.Exporting
{
    public class InvCpsSapAssetMasterExcelExporter : NpoiExcelExporterBase, IInvCpsSapAssetMasterExcelExporter
    {
        public InvCpsSapAssetMasterExcelExporter(ITempFileCacheManager tempFileCacheManager) : base(tempFileCacheManager) { }
        public FileDto ExportToFile(List<InvCpsSapAssetMasterDto> InvCpsSapAssetMaster)
        {
            return CreateExcelPackage(
                "InvCpsSapAssetMaster.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet("InvCpsSapAssetMaster");
                    AddHeader(
                                sheet,                        
                                     ("StatusRemark"),
                                     ("CompanyCode"),
                                     ("FixedAssetNumber"),
                                     ("SubAssetNumber"),
                                     ("AssetDescription"),
                                     ("AdditionalAssetDescription"),
                                     ("AssetClass"),
                                     ("AssetClassDescription"),
                                     ("SerialNumber"),
                                     ("WBS"),
                                     ("CostCenter"),
                                     ("ResponsibleCostCenter"),
                                     ("DeactivationDate"),
                                     ("AcquisitionLock"),
                                     ("CostOfAsset"),
                                     ("LineItem"),
                                     ("Ordering")
                               );
                    AddObjects(
                         sheet, InvCpsSapAssetMaster,                                  
                                     _ => _.StatusRemark,
                                     _ => _.CompanyCode,
                                     _ => _.FixedAssetNumber,
                                     _ => _.SubAssetNumber,
                                     _ => _.AssetDescription,
                                     _ => _.AdditionalAssetDescription,
                                     _ => _.AssetClass,
                                     _ => _.AssetClassDescription,
                                     _ => _.SerialNumber,
                                     _ => _.WBS,
                                     _ => _.CostCenter,
                                     _ => _.ResponsibleCostCenter,
                                     _ => _.DeactivationDate,
                                     _ => _.AcquisitionLock,
                                     _ => _.CostOfAsset,
                                     _ => _.LineItem,
                                     _ => _.Ordering
                                );
                });
        }
    }
}
