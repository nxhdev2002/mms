using Abp.Application.Services;
using prod.Dto;
using prod.Inventory.CKD.Dto;
using prod.Inventory.PIO.PartListInl.Dto;
using prod.Inventory.PIO.PartListOff.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prod.Inventory.PIO.PartListOff.Export
{
    public  interface IInvPioPartListOffExcelExporter : IApplicationService
    {

        FileDto ExportValidateToFile(List<ValidatePartListDto> invckdpartlistvalidate);

        FileDto ExportToFileErr(List<ImportPioPartListOffDto> invckdpartlist_err);

       
    }
}
