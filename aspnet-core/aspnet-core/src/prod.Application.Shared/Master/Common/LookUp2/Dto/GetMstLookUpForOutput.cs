using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace prod.Master.Common.LookUp2.Dto
{
    public class GetMstLookUpForOutput : EntityDto<long?>
    {
        public CreateOrEditMstLookUpDto MstLookUpForEdit { get; set; }
    }
}
