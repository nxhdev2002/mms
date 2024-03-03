using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.LogW.Ado.Dto
{

	public class LgwAdoCallingLightStatusExportInput
	{

		public virtual string Code { get; set; }

		public virtual string LightName { get; set; }

		public virtual string ProdLine { get; set; }

		public virtual string Status { get; set; }

	}

}


