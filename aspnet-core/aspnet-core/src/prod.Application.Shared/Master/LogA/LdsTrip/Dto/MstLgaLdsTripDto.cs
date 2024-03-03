using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Master.LogA.Dto
{

	public class MstLgaLdsTripDto : EntityDto<long?>
	{

		public virtual string ProdLine { get; set; }

		public virtual int? DeliveryNo { get; set; }

		public virtual string DeliveryName { get; set; }

        public virtual string Model { get; set; }

        public virtual int? TripNumber { get; set; }

		public virtual string DollyName { get; set; }

		public virtual int? TaktTime { get; set; }

		public virtual string IsActive { get; set; }

	}

	public class CreateOrEditMstLgaLdsTripDto : EntityDto<long?>
	{

		[StringLength(MstLgaLdsTripConsts.MaxProdLineLength)]
		public virtual string ProdLine { get; set; }

		public virtual int? DeliveryNo { get; set; }

		[StringLength(MstLgaLdsTripConsts.MaxDeliveryNameLength)]
		public virtual string DeliveryName { get; set; }

        [StringLength(MstLgaLdsTripConsts.MaxModelLength)]
        public virtual string Model { get; set; }

        public virtual int? TripNumber { get; set; }

		[StringLength(MstLgaLdsTripConsts.MaxDollyNameLength)]
		public virtual string DollyName { get; set; }

		public virtual int? TaktTime { get; set; }

		[StringLength(MstLgaLdsTripConsts.MaxIsActiveLength)]
		public virtual string IsActive { get; set; }
	}

	public class GetMstLgaLdsTripInput : PagedAndSortedResultRequestDto
	{

		public virtual string ProdLine { get; set; }

		public virtual string DeliveryName { get; set; }

		public virtual string DollyName { get; set; }

	}
    public class GetMstLgaLdsTripExcelInput
    {

        public virtual string ProdLine { get; set; }

        public virtual string DeliveryName { get; set; }

        public virtual string DollyName { get; set; }

    }

}


