using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace prod.Inventory.CKD.SmqdOrderLeadTime.Dto
{
    public class InvCkdSmqdOrderLeadTimeDto : EntityDto<long?>
    {

        public virtual string SupplierNo { get; set; }

        public virtual string Cfc { get; set; }

        public virtual string ExpCode { get; set; }

        public virtual int? Sea { get; set; }

        public virtual int? Air { get; set; }

    }

    public class GetInvCkdSmqdOrderLeadTimeInput : PagedAndSortedResultRequestDto
    {

        public virtual string SupplierNo { get; set; }

        public virtual string ExpCode { get; set; }


    }

    public class GetInvCkdSmqdOrderLeadTimeExportInput
    {

        public virtual string SupplierNo { get; set; }

        public virtual string ExpCode { get; set; }
    }

    public class InvCkdSmqdOrderLeadImportDto
    {
        public virtual long? ROW_NO { get; set; }
        public virtual string Guid { get; set; }

        public string ErrorDescription { get; set; }
        public virtual string SupplierNo { get; set; }

        public virtual string Cfc { get; set; }

        public virtual string ExpCode { get; set; }

        public virtual int? Sea { get; set; }

        public virtual int? Air { get; set; }
    }

}
