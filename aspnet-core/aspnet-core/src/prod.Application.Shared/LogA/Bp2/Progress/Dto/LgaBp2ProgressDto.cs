using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.LogA.Bp2.Dto
{

	public class LgaBp2ProgressDto : EntityDto<long?>
	{
        public virtual long? ProcessId { get; set; }

        public virtual int? EcarId { get; set; }

        public virtual string ProdLine { get; set; }

		public virtual DateTime? WorkingDate { get; set; }

		public virtual string Shift { get; set; }

		public virtual int? NoInShift { get; set; }
        
        public virtual DateTime? NewtaktDatetime { get; set; }

		public virtual DateTime? StartDatetime { get; set; }

		public virtual DateTime? FinishDatetime { get; set; }

		public virtual string Status { get; set; }

		public virtual string IsActive { get; set; }

	}

	public class GetLgaBp2ProgressInput : PagedAndSortedResultRequestDto
	{

		public virtual string ProdLine { get; set; }

        public virtual DateTime? WorkingDateFrom { get; set; }
        public virtual DateTime? WorkingDateTo { get; set; }

        public virtual string Shift { get; set; }

	}

    public class GetLgaBp2ProgressExportInput
    {

        public virtual string ProdLine { get; set; }

        public virtual DateTime? WorkingDateFrom { get; set; }
        public virtual DateTime? WorkingDateTo { get; set; }

        public virtual string Shift { get; set; }

    }

}

