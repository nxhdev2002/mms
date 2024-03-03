using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Master.LogW.Dto
{

	public class MstLgwPlcSignalExportInput
	{		

		public virtual string ProdLine { get; set; }

		public virtual string Process { get; set; }


		public virtual string SignalCode { get; set; }

	}

}


