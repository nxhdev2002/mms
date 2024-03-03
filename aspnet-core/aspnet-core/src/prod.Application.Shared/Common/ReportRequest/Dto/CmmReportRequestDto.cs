using Abp.Application.Services.Dto;
using System;

namespace prod.Common.Dto
{
	public class CmmReportRequestDto : EntityDto<long?>
	{
        public virtual string ReportList { get; set; }
        public virtual string ReportParams { get; set; }
        public virtual string ReqStatus { get; set; }
        public virtual DateTime? ReqTime { get; set; }
        public virtual string IsActive { get; set; }

    }

	public class GetCmmReportRequestInput : PagedAndSortedResultRequestDto
	{
        public virtual string ReportList { get; set; }
        public virtual string ReqStatus { get; set; }
        public virtual DateTime? ReqTime { get; set; }
    }
}

