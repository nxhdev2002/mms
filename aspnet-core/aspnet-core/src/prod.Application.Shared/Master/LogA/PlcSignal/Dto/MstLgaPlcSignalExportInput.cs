using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Master.LogA.Dto
{

	public class MstLgaPlcSignalExportInput
	{

		public virtual int? SignalIndex { get; set; }

		public virtual string SignalPattern { get; set; }

		public virtual string ProdLine { get; set; }

		public virtual string Process { get; set; }

		public virtual string SubProcess { get; set; }

		public virtual string SignalCode { get; set; }

		public virtual string SignalDescription { get; set; }

		public virtual string IsActive { get; set; }

	}

}

