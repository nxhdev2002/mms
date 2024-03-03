using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Master.WorkingPattern.Dto
{

	public class MstWptDailyWorkingTimeExportInput
	{

        public virtual int? ShiftNo { get; set; }

        public virtual string ShopName { get; set; }

        public virtual int? WorkingType { get; set; }

        public virtual DateTime? WorkingDateFrom { get; set; }

        public virtual DateTime? WorkingDateTo { get; set; }

    }

}

