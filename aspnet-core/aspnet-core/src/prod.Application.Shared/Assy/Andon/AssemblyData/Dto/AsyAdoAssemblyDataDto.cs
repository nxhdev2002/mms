using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Assy.Andon.Dto
{

	public class AsyAdoAssemblyDataDto : EntityDto<long?>
	{
		public virtual DateTime? WorkingDate { get; set; }
        public virtual DateTime? WorkingDateFrom { get; set; }
        public virtual DateTime? WorkingDateTo { get; set; }
        public virtual string Shift { get; set; }
        public virtual int? NoInDate { get; set; }
		public virtual string Line { get; set; }
		public virtual string Process { get; set; }
		public virtual string Model { get; set; }
		public virtual string Body { get; set; }
		public virtual string SeqNo { get; set; }
		public virtual string Grade { get; set; }
		public virtual string LotNo { get; set; }
		public virtual int? NoInLot { get; set; }
		public virtual int? LotNoIndex { get; set; }
        public virtual int? NoInShift { get; set; }
        public virtual string Color { get; set; }
		public virtual DateTime? CreateDate { get; set; }
	}

	public class GetAsyAdoAssemblyDataInput : PagedAndSortedResultRequestDto
	{
		public virtual DateTime? WorkingDateFrom { get; set; }
        public virtual DateTime? WorkingDateTo { get; set; }
        public virtual string Line { get; set; }
        public virtual string Shift { get; set; }
        public virtual string SeqNo { get; set; }
        public virtual string Process { get; set; }
		public virtual string Body { get; set; }
		public virtual string LotNo { get; set; }
        public virtual int? NoInShift { get; set; }

    }
}


