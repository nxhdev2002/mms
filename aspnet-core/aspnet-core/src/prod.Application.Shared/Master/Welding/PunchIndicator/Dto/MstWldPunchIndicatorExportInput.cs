using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Master.Welding.Dto
{

	public class MstWldPunchIndicatorExportInput
	{

		public virtual string Grade { get; set; }

		public virtual string Indicator { get; set; }

		public virtual string IsActive { get; set; }

	}

}


