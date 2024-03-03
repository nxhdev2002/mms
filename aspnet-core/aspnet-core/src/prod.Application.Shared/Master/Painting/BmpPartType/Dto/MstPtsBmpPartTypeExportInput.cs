using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Master.Painting.Dto
{

	public class MstPtsBmpPartTypeExportInput
	{

		public virtual string PartType { get; set; }

		public virtual string PartTypeName { get; set; }

		public virtual string ProdLine { get; set; }

		public virtual long Sorting { get; set; }

		public virtual string IsActive { get; set; }

	}

}



