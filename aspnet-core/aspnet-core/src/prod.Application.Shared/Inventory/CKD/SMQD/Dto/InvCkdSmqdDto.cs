using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace prod.Inventory.CKD.SMQD.Dto
{
    public class InvCkdSmqdDto : EntityDto<long?>
    {

        public virtual string RunNo { get; set; }

        public virtual string Cfc { get; set; }

        public virtual string LotNo { get; set; }

        public virtual string CheckModel { get; set; }

        public virtual string SupplierNo { get; set; }

        public virtual string PartNo { get; set; }

        public virtual string PartName { get; set; }

        public virtual string OrderNo { get; set; }

        public virtual int? Qty { get; set; }

        public virtual string Invoice { get; set; }

        public virtual DateTime? ReceivedDate { get; set; }

        public virtual int? EffectQty { get; set; }

        public virtual string ReasonCode { get; set; }

        public virtual string OrderStatus { get; set; }

        public virtual int? ReturnQty { get; set; }

        public virtual DateTime? ReturnDate { get; set; }

        public virtual string Remark { get; set; }

        public virtual DateTime? SmqdDate { get; set; }

    }

    public class GetInvCkdSmqdInput : PagedAndSortedResultRequestDto
    {
        public virtual DateTime? SmqdDateFrom { get; set; }

        public virtual DateTime? SmqdDateTo { get; set; }

        public virtual string PartNo { get; set; }

        public virtual string Cfc { get; set; }

        public virtual string SupplierNo { get; set; }

        public virtual string LotNo { get; set; }
        public virtual string Radio { get; set; }
    }

    public class GetInvCkdSmqdExportInput
    {
        public virtual DateTime? SmqdDateFrom { get; set; }

        public virtual DateTime? SmqdDateTo { get; set; }

        public virtual string PartNo { get; set; }

        public virtual string Cfc { get; set; }

        public virtual string SupplierNo { get; set; }

        public virtual string LotNo { get; set; }
        public virtual string Radio { get; set; }
    }

    public class InvCkdSmqdImportDto
    {
        public virtual long? ROW_NO { get; set; }

        public virtual string Guid { get; set; }

        public virtual DateTime? SmqdDate { get; set; }

        public virtual string RunNo { get; set; }

        public virtual string Cfc { get; set; }

        public virtual string LotNo { get; set; }

        public virtual string CheckModel { get; set; }

        public virtual string SupplierNo { get; set; }

        public virtual string PartNo { get; set; }

        public virtual string PartName { get; set; }

        public virtual int? Qty { get; set; }

        public virtual int? EffectQty { get; set; }

        public virtual string ReasonCode { get; set; }

        public virtual string OrderStatus { get; set; }

        public virtual int? ReturnQty { get; set; }

        public virtual DateTime? ReturnDate { get; set; }

        public virtual string Remark { get; set; }

        public string ErrorDescription { get; set; }
    }

    public class InvPxpReturnImportDto
    {
        public virtual string Guid { get; set; }
        public virtual DateTime? SmqdDate { get; set; }
        public virtual string RunNo { get; set; }
        public virtual string Cfc { get; set; }
        public virtual string LotNo { get; set; }
        public virtual string CheckModel { get; set; }
        public virtual string SupplierNo { get; set; }
        public virtual string PartNo { get; set; }
        public virtual string PartName { get; set; }
        public virtual int? Qty { get; set; }
        public virtual string OrderNo { get; set; }
        public virtual string Invoice { get; set; }
        public virtual DateTime? ReceivedDate { get; set; }
        public virtual int? EffectQty { get; set; }
        public virtual string ReasonCode { get; set; }
        public virtual string Reason { get; set; }
        public virtual string OrderStatus { get; set; }
        public virtual int? ReturnQty { get; set; }
        public virtual DateTime? ReturnDate { get; set; }
        public virtual string Remark { get; set; }
        public virtual string Type { get; set; }
        public string ErrorDescription { get; set; }

    }


    public class InvPxpOutImportDto
    {
        public virtual string Guid { get; set; }
        public virtual DateTime? SmqdDate { get; set; }
        public virtual string RunNo { get; set; }
        public virtual string Cfc { get; set; }
        public virtual string LotNo { get; set; }
        public virtual string CheckModel { get; set; }
        public virtual string SupplierNo { get; set; }
        public virtual string PartNo { get; set; }
        public virtual string PartName { get; set; }
        public virtual int? Qty { get; set; }
        public virtual string Remark { get; set; }
        public virtual string Type { get; set; }
        public string ErrorDescription { get; set; }

    }
}
