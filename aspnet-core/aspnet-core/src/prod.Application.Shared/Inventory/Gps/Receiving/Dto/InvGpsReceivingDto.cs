using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Inventory.GPS.Dto
{

    public class InvGpsReceivingDto : EntityDto<long?>
    {
		public virtual string PoNo { get; set; }
		public virtual string PartNo { get; set; }

        public virtual string PartName { get; set; }

        public virtual string Uom { get; set; }

        public virtual int? Boxqty { get; set; }

        public virtual int? Box { get; set; }

        public virtual int? Qty { get; set; }

        public virtual string LotNo { get; set; }

        public virtual DateTime? ProdDate { get; set; }
        public virtual decimal? PoPrice { get; set; }

        public virtual string Shop { get; set; }
        public virtual string Gentani { get; set; }
        public virtual string FormatProdDate 
        {
            get
            {
                return ProdDate == null ? "" : string.Format("{0:dd/MM/yyyy}", ProdDate);
            }
            set { }
        }

        public virtual DateTime? ExpDate { get; set; }

        public virtual string FormatExpDate
        {
            get
            {
                return ExpDate == null ? "" : string.Format("{0:dd/MM/yyyy}", ExpDate);
            }
            set { }
        }

        public virtual DateTime? ReceivedDate { get; set; }

        public virtual string FormatReceivedDate
        {
            get
            {
                return ReceivedDate == null ? "" : string.Format("{0:dd/MM/yyyy}", ReceivedDate);
            }
            set { }
        }

        public virtual string Supplier { get; set; }

        public virtual string UserReceives { get; set; }

        public virtual string Dock { get; set; }

    }

    public class CreateOrEditInvGpsReceivingDto : EntityDto<long?>
    {
		[StringLength(InvGpsReceivingConsts.MaxPoNoLength)]
		public virtual string PoNo { get; set; }

		[StringLength(InvGpsReceivingConsts.MaxPartNoLength)]
        public virtual string PartNo { get; set; }

        [StringLength(InvGpsReceivingConsts.MaxPartNameLength)]
        public virtual string PartName { get; set; }

        [StringLength(InvGpsReceivingConsts.MaxOumLength)]
        public virtual string Uom { get; set; }

        public virtual int? Boxqty { get; set; }

        public virtual int? Box { get; set; }

        public virtual int? Qty { get; set; }

        [StringLength(InvGpsReceivingConsts.MaxLotNoLength)]
        public virtual string LotNo { get; set; }

        public virtual DateTime? ProdDate { get; set; }

        public virtual DateTime? ExpDate { get; set; }

        public virtual DateTime? ReceivedDate { get; set; }

        [StringLength(InvGpsReceivingConsts.MaxSupplierLength)]
        public virtual string Supplier { get; set; }

        [StringLength(50)]
        public virtual string Dock { get; set; }

        [StringLength(50)]
        public virtual string UserReceives { get; set; }

        [StringLength(10)]
        public virtual string Shop { get; set; }

        public virtual decimal? PoPrice { get; set; }
    }

    public class GetInvGpsReceivingInput : PagedAndSortedResultRequestDto
    {
		public virtual string PoNo { get; set; }
		public virtual string PartNo { get; set; }

        public virtual string Supplier { get; set; }

        public virtual DateTime? ReceivedDateFrom { get; set; }

        public virtual DateTime? ReceivedDateTo { get; set; }

        public virtual string Dock { get; set; }
    }

    public class InvGpsReceivingImportDto
    {
        public virtual long? ROW_NO { get; set; }

        public virtual string Guid { get; set; }

		public virtual string PoNo { get; set; }
		public virtual string PartNo { get; set; }

        public virtual string PartName { get; set; }

        public virtual string Uom { get; set; }

        public virtual int? BoxQty { get; set; }

        public virtual int? Box { get; set; }

        public virtual int? Qty { get; set; }

        public virtual string LotNo { get; set; }

        public virtual DateTime? ProdDate { get; set; }

        public virtual DateTime? ExpDate { get; set; }

        public virtual DateTime? ReceivedDate { get; set; }

        public virtual string Supplier { get; set; }

        public virtual string ErrorDescription { get; set; }

        public virtual string Shop { get; set; }

        public virtual long? CreatorUserId { get; set; }

        public virtual string Dock { get; set; }

    }

    public class InvGuidUserDto
    {
        public virtual string Guid { get; set; }
        public virtual long? UserId { get; set; }
    }
    public class CheckDto
    {
        public string ExistMaterial { get; set; }

        public string ExistDocumentNo { get; set; }
    }
}


