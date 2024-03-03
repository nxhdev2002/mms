using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Inventory.CKD.Dto
{

	public class InvCkdStockPartDto : EntityDto<long?>
	{

		public virtual string PartNo { get; set; }

        public virtual string Cfc { get; set; }

        public virtual string SupplierNo { get; set; }

        public virtual string PartNoNormalized { get; set; }

		public virtual string PartName { get; set; }

		public virtual string PartNoNormalizedS4 { get; set; }

		public virtual string ColorSfx { get; set; }

		public virtual long? PartListId { get; set; }

		public virtual long? MaterialId { get; set; }

		public virtual int? Qty { get; set; }

		public virtual DateTime? WorkingDate { get; set; }

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

        public virtual int? GrandTotal { get; set; }

        public virtual string IsActive { get; set; }

        public virtual bool IsDeleted { get; set; }


    }

	public class CreateOrEditInvCkdStockPartDto : EntityDto<long?>
	{

		[StringLength(InvCkdStockPartConsts.MaxPartNoLength)]
		public virtual string PartNo { get; set; }

		[StringLength(InvCkdStockPartConsts.MaxPartNoNormalizedLength)]
		public virtual string PartNoNormalized { get; set; }

		[StringLength(InvCkdStockPartConsts.MaxPartNameLength)]
		public virtual string PartName { get; set; }

		[StringLength(InvCkdStockPartConsts.MaxPartNoNormalizedS4Length)]
		public virtual string PartNoNormalizedS4 { get; set; }

		[StringLength(InvCkdStockPartConsts.MaxColorSfxLength)]
		public virtual string ColorSfx { get; set; }

		public virtual long? PartListId { get; set; }

		public virtual long? MaterialId { get; set; }

		public virtual int? Qty { get; set; }

		public virtual DateTime? WorkingDate { get; set; }

		public virtual long? PeriodId { get; set; }

		public virtual DateTime? LastCalDatetime { get; set; }

		[StringLength(InvCkdStockPartConsts.MaxIsActiveLength)]
		public virtual string IsActive { get; set; }
	}

	public class GetInvCkdStockPartInput : PagedAndSortedResultRequestDto
	{
        public virtual string PartNo { get; set; }

        public virtual string Cfc { get; set; }

        public virtual string SupplierNo { get; set; }

        public virtual string ColorSfx { get; set; }

        public virtual DateTime? WorkingDate { get; set; }

		public virtual bool NegativeStock { get; set; }
    }

    public class GetCheckStockPart : PagedAndSortedResultRequestDto
    {
    }

    public class InvCkdStockPartByMaterialDto : EntityDto<long?>
    {

        public virtual string PartNo { get; set; }

        public virtual string Description { get; set; }

        public virtual string ValuationType { get; set; }

        public virtual long? Qty { get; set; }

        public virtual DateTime? WorkingDate { get; set; }

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

        public virtual long? MaterialId { get; set; }

    }
}


