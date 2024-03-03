using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Inventory.CKD.Dto
{

    public class InvCkdBillDto : EntityDto<long?>
    {

        public virtual string BillofladingNo { get; set; }

        public virtual long? ShipmentId { get; set; }

        public virtual DateTime? BillDate { get; set; }

        public virtual string Status { get; set; }

        public virtual string StatusCode { get; set; }

        public virtual string IsActive { get; set; }

    }

    public class GetInvCkdBillInput : PagedAndSortedResultRequestDto
    {

        public virtual string BillofladingNo { get; set; }

        public virtual DateTime? BillDateFrom { get; set; }

        public virtual DateTime? BillDateTo { get; set; }

        public virtual string CkdPio { get; set; }

        public virtual string OrderTypeCode { get; set; }

    }
    public class GetInvCkdBillHistoryInput : PagedAndSortedResultRequestDto
    {
        public virtual long Id { get; set; }

        public virtual string TableName { get; set; }
    }
    public class GetInvCkdBillHistoryExcelInput
    {
        public virtual long Id { get; set; }

        public virtual string TableName { get; set; }
    }
    public class InvCkdBillExportInput
    {
        public virtual string BillofladingNo { get; set; }

        public virtual DateTime? BillDateFrom { get; set; }

        public virtual DateTime? BillDateTo { get; set; }

        public virtual string CkdPio { get; set; }

        public virtual string OrderTypeCode { get; set; }
    }
}





