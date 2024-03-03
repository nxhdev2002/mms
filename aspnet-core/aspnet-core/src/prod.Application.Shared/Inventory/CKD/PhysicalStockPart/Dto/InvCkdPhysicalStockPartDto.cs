using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Inventory.CKD.Dto
{

    public class InvCkdPhysicalStockPartDto : EntityDto<long?>
    {
        
        public virtual string PartNo { get; set; }
        public virtual string Cfc { get; set; }
        public virtual string SupplierNo { get; set; }
        public virtual string PartNoNormalized { get; set; }

        public virtual string PartName { get; set; }

        public virtual string PartNoNormalizedS4 { get; set; }

        public virtual string ColorSfx { get; set; }

        public virtual string LotNo { get; set; }

        public virtual long? PartListId { get; set; }

        public virtual long? MaterialId { get; set; }

        public virtual int? BeginQty { get; set; }

        public virtual int? ReceiveQty { get; set; }

        public virtual int? IssueQty { get; set; }

        public virtual int? CalculatorQty { get; set; }

        public virtual int? ActualQty { get; set; }

        public virtual long? PeriodId { get; set; }

        public virtual DateTime? LastCalDatetime { get; set; }
        public virtual string LastCalDatetime_DDMMYYYY
        {
            get
            {
                try
                {
                    return string.Format("{0:dd/MM/yyyy HH:mm:ss}", LastCalDatetime);
                }
                catch
                {
                    return "";
                }
            }
            set { }
        }

        public virtual int? Transtype { get; set; }

        public virtual string Remark { get; set; }

        public virtual string IsActive { get; set; }

        public virtual int? TotalBeginQty { get; set; }

        public virtual int? TotalReceiveQty { get; set; }

        public virtual int? TotalIssueQty { get; set; }

        public virtual int? TotalCalculatorQty { get; set; }

        public virtual int? TotalActualQty { get; set; }

        public virtual string ErrorMessage { get; set; }

        public virtual int? Diff { get; set; }


        //for export summary stock by part
        public virtual int? RegularReceiveQty { get; set; }
        public virtual int? RegularIssueQty { get; set; }
        public virtual int? SMQDPxPInQty { get; set; }
        public virtual int? SMQDPxPReturnQty { get; set; }
        public virtual int? SMQDInOtherQty { get; set; }
        public virtual int? SMQDOutOtherQty { get; set; }
        public virtual int? SMQDPxPOutQty { get; set; }

    }

    public class CreateOrEditInvCkdPhysicalStockPartDto : EntityDto<long?>
    {              

        [StringLength(12)]
        public virtual string PartNo { get; set; }

        [StringLength(12)]
        public virtual string PartNoNormalized { get; set; }

        [StringLength(300)]
        public virtual string PartName { get; set; }

        [StringLength(10)]
        public virtual string PartNoNormalizedS4 { get; set; }

        [StringLength(2)]
        public virtual string ColorSfx { get; set; }

        [StringLength(20)]
        public virtual string LotNo { get; set; }

        [StringLength(4)]
        public virtual string Cfc { get; set; }

        [StringLength(50)]
        public virtual string SupplierNo { get; set; }

        public virtual long? PartListId { get; set; }

        public virtual long? MaterialId { get; set; }

        public virtual int? BeginQty { get; set; }

        public virtual int? ReceiveQty { get; set; }

        public virtual int? IssueQty { get; set; }

        public virtual decimal? CalculatorQty { get; set; }

        public virtual int? ActualQty { get; set; }

        public virtual long? PeriodId { get; set; }

        public virtual DateTime? LastCalDatetime { get; set; }

        public virtual int? Transtype { get; set; }

        [StringLength(50)]
        public virtual string Remark { get; set; }

        [StringLength(1)]
        public virtual string IsActive { get; set; }

    }

    public class GetInvCkdPhysicalStockPartInput : PagedAndSortedResultRequestDto
    {

        public virtual string PartNo { get; set; }

        public virtual string Cfc { get; set; }

        public virtual string SupplierNo { get; set; }

        public virtual string ColorSfx { get; set; }

        public virtual long? PeriodId { get; set; }

        public virtual string LotNo { get; set; }

        public virtual int? p_mode { get; set; }

    }

    public class InvCkdPhysicalStockPartDto_T
    {
        public virtual string Guid { get; set; }
        public virtual string PartNo { get; set; }
        public virtual string Cfc { get; set; }
        public virtual string SupplierNo { get; set; }
        public virtual int? Qty { get; set; }
        public string ErrorDescription { get; set; }
        public virtual long? CreatorUserId { get; set; }

    }

    public class InvCkdPhysicalStockLotDto_T
    {
        public virtual string Guid { get; set; }
        public virtual string LotNo { get; set; }
        public virtual string SupplierNo { get; set; }
        public virtual string Shop { get; set; }
        public virtual int? Qty { get; set; }
        public string ErrorDescription { get; set; }
        public virtual long? CreatorUserId { get; set; }

    }


    public class InvCkdPhysicalStockErrDto
    {
        public virtual string Guid { get; set; }
        public virtual string PartNo { get; set; }
        public virtual string Cfc { get; set; }
        public virtual string SupplierNo { get; set; }
        public virtual string LotNo { get; set; }
        public virtual string Shop { get; set; }
        public virtual int? Qty { get; set; }
        public string ErrorDescription { get; set; }

    }
    public class UpdatePhysicalStockErrDto : EntityDto<long?>
    {
        public virtual int? ActualQty { get; set; }

    }


}

