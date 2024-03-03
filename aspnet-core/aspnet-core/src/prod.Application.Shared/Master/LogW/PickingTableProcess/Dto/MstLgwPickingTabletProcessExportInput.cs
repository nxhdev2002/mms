using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Master.LogW.Dto
{

	public class MstLgwPickingTabletProcessExportInput
	{

		public virtual string PickingTabletId { get; set; }

		public virtual string PickingPosition { get; set; }

		public virtual string Process { get; set; }	

	}

}


