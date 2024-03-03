using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.LogW.Pik.Dto
{

	public class LgwPikPickingSignalDto : EntityDto<long?>
	{

		public virtual string PickingTabletId { get; set; }

		public virtual int? TabletProcessId { get; set; }

		public virtual int? PickingProgressId { get; set; }

		public virtual DateTime? FirstSignalTime { get; set; }

		public virtual DateTime? LastSignalTime { get; set; }

		public virtual string IsCompleted { get; set; }

		public virtual string IsActive { get; set; }

	}

	public class GetLgwPikPickingSignalInput : PagedAndSortedResultRequestDto
	{
		public virtual string PickingTabletId { get; set; }
		public virtual DateTime? FirstSignalTime { get; set; }
		public virtual DateTime? LastSignalTime { get; set; }	
	}

    public class GetLgwPikPickingSignalExportInput
    {
        public virtual string PickingTabletId { get; set; }
        public virtual DateTime? FirstSignalTime { get; set; }
        public virtual DateTime? LastSignalTime { get; set; }
    }
}


