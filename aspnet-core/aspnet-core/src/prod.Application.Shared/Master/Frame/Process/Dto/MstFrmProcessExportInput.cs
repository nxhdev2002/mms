using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Master.Frame.Dto
{

	public class MstFrmProcessExportInput
	{

		public virtual int? ProcessSeq { get; set; }

		public virtual string ProcessCode { get; set; }

		public virtual string ProcessName { get; set; }

		public virtual string ProcessDesc { get; set; }

		public virtual int? ProcessGroup { get; set; }

		public virtual string GroupName { get; set; }

		public virtual int? ProcessSubgroup { get; set; }

		public virtual string SubgroupName { get; set; }

		public virtual string IsActive { get; set; }

	}

}


