using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace prod.Master.Common.GradeColor.Dto
{
    public class MstCmmGradeColorDetailDto : EntityDto<long?>
    {
        public string Color { get; set; }
        public string ColorType { get; set; }
        public string NameEn { get; set; }
        public string NameVn { get; set; }

        public string NameColorType { get; set; }

    }
}
