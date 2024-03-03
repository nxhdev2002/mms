using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Master.WorkingPattern.Dto
{

	public class MstWptWorkingTimeExportInput
	{

        public virtual int? ShiftNo { get; set; }

        public virtual string ShopName { get; set; }

    }

}


