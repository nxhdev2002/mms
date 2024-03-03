using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Inventory.CKD.Dto
{

    public class InvCkdStockReceivingDto : EntityDto<long?>
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
        public virtual int? GrandTotal { get; set; }
        public virtual string ErrDes { get; set; }


    }

    public class CreateOrEditInvCkdStockReceivingDto : EntityDto<long?>
    {

        [StringLength(InvCkdStockReceivingConsts.MaxPartNoLength)]
        public virtual string PartNo { get; set; }

        [StringLength(InvCkdStockReceivingConsts.MaxPartNoNormalizedLength)]
        public virtual string PartNoNormalized { get; set; }

        [StringLength(InvCkdStockReceivingConsts.MaxPartNameLength)]
        public virtual string PartName { get; set; }

        [StringLength(InvCkdStockReceivingConsts.MaxPartNoNormalizedS4Length)]
        public virtual string PartNoNormalizedS4 { get; set; }

        [StringLength(InvCkdStockReceivingConsts.MaxColorSfxLength)]
        public virtual string ColorSfx { get; set; }

        public virtual long? PartListId { get; set; }

        public virtual long? PartListGradeId { get; set; }

        public virtual long? MaterialId { get; set; }

        public virtual int? Qty { get; set; }

        public virtual DateTime? TransactionDatetime { get; set; }

        public virtual long? ReferenceId { get; set; }

        public virtual DateTime? WorkingDate { get; set; }

        public virtual long? PeriodId { get; set; }

        [StringLength(InvCkdStockReceivingConsts.MaxIsActiveLength)]
        public virtual string IsActive { get; set; }
    }

    public class GetInvCkdStockReceivingInput : PagedAndSortedResultRequestDto
    {
       public virtual string PartNo { get; set; }
        public virtual string ColorSfx { get; set; }
        public virtual DateTime? WorkingDateFrom { get; set; }
        public virtual DateTime? WorkingDateTo { get; set; }
        public virtual string SupplierNo { get; set; }
        public virtual string ContainerNo { get; set; }
        public virtual string InvoiceNo { get; set; }
        public virtual string Cfc { get; set; }


    }

    public class InvCkdStockReceivingValidateDto : EntityDto<long?>
    {
        public virtual string PartNo { get; set; }

        public virtual string SupplierNo { get; set; }

        public virtual string Cfc { get; set; }

        public virtual string ContainerNo { get; set; }

        public virtual string Renban { get; set; }

        public virtual DateTime? ReceiveDate { get; set; }

        public virtual string Status { get; set; }

        public virtual DateTime? CdDate { get; set; }

        public virtual string ErrDesc { get; set; }
    }

}


