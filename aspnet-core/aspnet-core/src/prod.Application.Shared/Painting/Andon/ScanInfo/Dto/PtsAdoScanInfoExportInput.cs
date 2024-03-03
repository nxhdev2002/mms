using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Painting.Andon.Dto
{

	public class PtsAdoScanInfoExportInput
	{

		public virtual string ScanType { get; set; }

		public virtual string ScanLocation { get; set; }

	
	}

}


