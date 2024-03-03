using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Painting.Andon.Dto
{

	public class PtsAdoScanInfoDto : EntityDto<long?>
	{

		public virtual string ScanType { get; set; }

		public virtual string ScanValue { get; set; }

		public virtual string ScanLocation { get; set; }

		public virtual DateTime? ScanTime { get; set; }

		public virtual string ScanBy { get; set; }

		public virtual string IsProcessed { get; set; }

	}

	public class CreateOrEditPtsAdoScanInfoDto : EntityDto<long?>
	{

		[Required]
		[StringLength(PtsAdoScanInfoConsts.MaxScanTypeLength)]
		public virtual string ScanType { get; set; }

		[StringLength(PtsAdoScanInfoConsts.MaxScanValueLength)]
		public virtual string ScanValue { get; set; }

		[StringLength(PtsAdoScanInfoConsts.MaxScanLocationLength)]
		public virtual string ScanLocation { get; set; }

		[Required]
		public virtual DateTime? ScanTime { get; set; }

		[StringLength(PtsAdoScanInfoConsts.MaxScanByLength)]
		public virtual string ScanBy { get; set; }

		[StringLength(PtsAdoScanInfoConsts.MaxIsProcessedLength)]
		public virtual string IsProcessed { get; set; }
	}

	public class GetPtsAdoScanInfoInput : PagedAndSortedResultRequestDto
	{

		public virtual string ScanType { get; set; }

		public virtual string ScanValue { get; set; }

		public virtual string ScanLocation { get; set; }

		public virtual DateTime? ScanTime { get; set; }

		public virtual string ScanBy { get; set; }

		public virtual string IsProcessed { get; set; }

	}

}

