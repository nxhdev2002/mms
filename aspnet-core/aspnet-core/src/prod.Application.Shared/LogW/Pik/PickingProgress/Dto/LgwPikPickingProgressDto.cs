using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.LogW.Pik.Dto
{

	public class LgwPikPickingProgressDto : EntityDto<long?>
	{

		public virtual string PickingTabletId { get; set; }

		public virtual string ProdLine { get; set; }

        public virtual string BodyNo { get; set; }

        public virtual string LotNo { get; set; }

        public virtual string ProcessCode { get; set; }

		public virtual string ProcessGroup { get; set; }

		public virtual int? SeqNo { get; set; }

		public virtual DateTime? WorkingDate { get; set; }
        public virtual DateTime? WorkingDateFrom { get; set; }
        public virtual DateTime? WorkingDateTo { get; set; }

        public virtual string Shift { get; set; }

		public virtual DateTime? TaktStartTime { get; set; }

		public virtual DateTime? StartTime { get; set; }

		public virtual DateTime? FinishTime { get; set; }

		public virtual string IsActive { get; set; }

	}


	public class GetLgwPikPickingProgressInput : PagedAndSortedResultRequestDto
	{
        public virtual DateTime? WorkingDateFrom { get; set; }
        public virtual DateTime? WorkingDateTo { get; set; }
        public virtual string ProdLine { get; set; }     
        public virtual string ProcessCode { get; set; }
	}

    public class GetLgwPikPickingProgressExportInput
    {
        public virtual DateTime? WorkingDateFrom { get; set; }
        public virtual DateTime? WorkingDateTo { get; set; }
        public virtual string ProdLine { get; set; }
        public virtual string ProcessCode { get; set; }
    }

}


