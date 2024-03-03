using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace prod.Master.Common.GradeColor.Dto
{
    public  class MstCmmGradeColorDetailInput : PagedAndSortedResultRequestDto
    {
        public int? gradeId { get; set; }
    }
}
