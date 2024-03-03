using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.LogW.Mwh.Dto
{

	public class LgwMwhRobbingLaneDto : EntityDto<long?>
	{

		public virtual string LaneNo { get; set; }

		public virtual string LaneName { get; set; }

		public virtual string ContNo { get; set; }

		public virtual string Renban { get; set; }

		public virtual string SupplierNo { get; set; }

		public virtual string ShowOnly { get; set; }

		public virtual string IsActive { get; set; }

	}

	public class GetLgwMwhRobbingLaneInput : PagedAndSortedResultRequestDto
	{
		public virtual string LaneNo { get; set; }
		public virtual string LaneName { get; set; }

	}

    public class GetLgwMwhRobbingLaneExportInput
    {
        public virtual string LaneNo { get; set; }
        public virtual string LaneName { get; set; }

    }
}


