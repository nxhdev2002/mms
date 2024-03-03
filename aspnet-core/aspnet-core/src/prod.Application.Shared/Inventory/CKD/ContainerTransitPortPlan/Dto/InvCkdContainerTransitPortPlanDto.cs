using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Inventory.CKD.Dto
{

    public class InvCkdContainerTransitPortPlanDto : EntityDto<long?>
    {
        public virtual long? ROW_NO { get; set; }
        public virtual string ContainerNo { get; set; }

        public virtual string RowNumber { get; set; }

        public virtual string Guid { get; set; }

        public virtual string Renban { get; set; }

        public virtual DateTime? RequestDate { get; set; }

        public virtual TimeSpan? RequestTime { get; set; }

        public virtual DateTime? ReceiveDate { get; set; }

        public virtual string InvoiceNo { get; set; }

        public virtual string BillOfLadingNo { get; set; }

        public virtual string SupplierNo { get; set; }

        public virtual string SealNo { get; set; }

        public virtual string ListCaseNo { get; set; }

        public virtual string ListLotNo { get; set; }

        public virtual DateTime? CdDate { get; set; }

        public virtual string Transport { get; set; }

        public virtual string Status { get; set; }

        public virtual string PortCode { get; set; }

        public virtual string PortName { get; set; }

        public virtual string Remarks { get; set; }

        public virtual string IsActive { get; set; }

        public virtual string ErrorDescription { get; set; }

        public virtual string Custums1 { get; set; }

        public virtual string Custums2 { get; set; }

        public virtual string LotNo { get; set; }
        public virtual string ModuleCaseNo { get; set; }
        public virtual string PartNo { get; set; }

    }

    public class CreateOrEditInvCkdContainerTransitPortPlanDto : EntityDto<long?>
    {

        [StringLength(InvCkdContainerTransitPortPlanConsts.MaxContainerNoLength)]
        public virtual string ContainerNo { get; set; }

        [StringLength(InvCkdContainerTransitPortPlanConsts.MaxRenbanLength)]
        public virtual string Renban { get; set; }

        public virtual DateTime? RequestDate { get; set; }

        public virtual TimeSpan? RequestTime { get; set; }

        [StringLength(InvCkdContainerTransitPortPlanConsts.MaxInvoiceNoLength)]
        public virtual string InvoiceNo { get; set; }

        [StringLength(InvCkdContainerTransitPortPlanConsts.MaxBillOfLadingNoLength)]
        public virtual string BillOfLadingNo { get; set; }

        [StringLength(InvCkdContainerTransitPortPlanConsts.MaxSupplierNoLength)]
        public virtual string SupplierNo { get; set; }

        [StringLength(InvCkdContainerTransitPortPlanConsts.MaxSealNoLength)]
        public virtual string SealNo { get; set; }

        [StringLength(InvCkdContainerTransitPortPlanConsts.MaxListCaseNoLength)]
        public virtual string ListCaseNo { get; set; }

        [StringLength(InvCkdContainerTransitPortPlanConsts.MaxListLotNoLength)]
        public virtual string ListLotNo { get; set; }

        public virtual DateTime? CdDate { get; set; }

        [StringLength(InvCkdContainerTransitPortPlanConsts.MaxTransportLength)]
        public virtual string Transport { get; set; }

        [StringLength(InvCkdContainerTransitPortPlanConsts.MaxStatusLength)]
        public virtual string Status { get; set; }

        [StringLength(InvCkdContainerTransitPortPlanConsts.MaxPortCodeLength)]
        public virtual string PortCode { get; set; }

        [StringLength(InvCkdContainerTransitPortPlanConsts.MaxPortNameLength)]
        public virtual string PortName { get; set; }

        [StringLength(InvCkdContainerTransitPortPlanConsts.MaxRemarksLength)]
        public virtual string Remarks { get; set; }

        [StringLength(InvCkdContainerTransitPortPlanConsts.MaxIsActiveLength)]
        public virtual string IsActive { get; set; }
    }

    public class GetInvCkdContainerTransitPortPlanInput : PagedAndSortedResultRequestDto
    {

        public virtual string ContainerNo { get; set; }
        public virtual string Renban { get; set; }
        public virtual string SupplierNo { get; set; }
        public virtual string InvoiceNo { get; set; }
        public virtual string BillOfLadingNo { get; set; }   
        public virtual string SealNo { get; set; }
        public virtual DateTime? RequestDateFrom { get; set; }
        public virtual DateTime? RequestDateTo { get; set; }
        public virtual DateTime? ReceiveDateFrom { get; set; }
        public virtual DateTime? ReceiveDateTo { get; set; }
        public virtual string Status { get; set; }
        public virtual string OrderTypeCode { get; set; }
        public virtual string LotNo { get; set; }
        public virtual string ModuleCaseNo { get; set; }
        public virtual string PartNo { get; set; }
    }

  

}


