using Abp.Application.Services.Dto;
using System;

namespace prod.Inventory.CKD.ReceivingPhysicalStock.Dto
{
    public class InvCkdReceivingPhysicalStockDto : EntityDto<long?>
    {

        public virtual string PartNo { get; set; }

        public virtual string SupplierNo { get; set; }

        public virtual string Cfc { get; set; }

        public virtual string ContainerNo { get; set; }

        public virtual string InvoiceNo { get; set; }

        public virtual string PartNoNormalized { get; set; }

        public virtual string PartName { get; set; }

        public virtual string PartNoNormalizedS4 { get; set; }

        public virtual string ColorSfx { get; set; }

        public virtual long? PartListId { get; set; }

        public virtual long? PartListGradeId { get; set; }

        public virtual long? MaterialId { get; set; }

        public virtual int? Qty { get; set; }

        public virtual DateTime? TransactionDatetime { get; set; }
        public virtual string TransactionDatetime_DDMMYYYY
        {
            get
            {
                try
                {
                    return string.Format("{0:dd/MM/yyyy HH:mm:ss}", TransactionDatetime);
                }
                catch
                {
                    return "";
                }
            }
            set { }
        }

        public virtual long? ReferenceId { get; set; }

        public virtual DateTime? WorkingDate { get; set; }

        public virtual long? PeriodId { get; set; }

        public virtual string IsActive { get; set; }

        public virtual string LotNo { get; set; }

        public virtual long? Carid { get; set; }

        public virtual int? GrandTotal { get; set; }

    }
    public class InvCkdReceivingPhysicalStockInputDto : PagedAndSortedResultRequestDto
    {
        public virtual string PartNo { get; set; }

        public virtual string Cfc { get; set; }

        public virtual DateTime? WorkingDateFrom { get; set; }

        public virtual DateTime? WorkingDateTo { get; set; }

        public virtual string SupplierNo { get; set; }

        public virtual string LotNo { get; set; }

        public virtual int? p_mode { get; set; }

        public virtual int? PeriodId { get; set; }
    }

    public class InvCkdReceivingPhysStockDetailsDataDto
    {
        public virtual string PartNo { get; set; }

        public virtual string Cfc { get; set; }

        public virtual string SupplierNo { get; set; }

        public virtual string LotNo { get; set; }

        public virtual string ModuleNo { get; set; }

        public virtual string ContainerNo { get; set; }

        public virtual string InvoiceNo { get; set; }

        public virtual int? Out_Qty { get; set; }

        public virtual string PartName { get; set; }
    }
}
