using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace prod.Master.Inventory.LotPart.Dto
{
    public class MstInvLotPartDto : EntityDto<long?>
    {
        public virtual string Part_No { get; set; }

        public virtual string Source { get; set; }

        public virtual string Carfamily_Code { get; set; }

        public virtual string Carfamily_Name { get; set; }

        public virtual string Line_Code { get; set; }

        public virtual string Color { get; set; }

        public virtual string Part_Name { get; set; }

        public virtual string Active { get; set; }
    }

    public class GetMstInvLotPartInput : PagedAndSortedResultRequestDto
    {

        public virtual string PartNo { get; set; }

        public virtual string Source { get; set; }

        public virtual string CarfamilyCode { get; set; }


    }

    public class GetMstInvLotPartExportInput
    {

        public virtual string PartNo { get; set; }

        public virtual string Source { get; set; }

        public virtual string CarfamilyCode { get; set; }

    }
}
