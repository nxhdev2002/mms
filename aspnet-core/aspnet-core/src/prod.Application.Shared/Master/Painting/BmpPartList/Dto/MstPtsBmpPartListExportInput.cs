using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Master.Painting.Dto
{

	public class MstPtsBmpPartListExportInput
	{

		public virtual string Model { get; set; }

		public virtual string Cfc { get; set; }

		public virtual string Grade { get; set; }

		public virtual string BackNo { get; set; }

		public virtual string ProdLine { get; set; }

		public virtual string PartTypeCode { get; set; }

		public virtual long? PartTypeId { get; set; }

		public virtual string Process { get; set; }

		public virtual string PkProcess { get; set; }

		public virtual string IsPunch { get; set; }

		public virtual string SpecialColor { get; set; }

		public virtual long? SignalId { get; set; }

		public virtual string SignalCode { get; set; }

		public virtual string Remark { get; set; }

		public virtual string IsActive { get; set; }

	}

}


