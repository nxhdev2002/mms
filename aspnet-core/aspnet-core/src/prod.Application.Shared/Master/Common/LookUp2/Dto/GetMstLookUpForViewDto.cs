using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace prod.Master.Common.LookUp2.Dto
{
    public class GetMstLookUpForViewDto : EntityDto<long>
    {

        public virtual string DomainCode { get; set; }

        public virtual string ItemCode { get; set; }

        public virtual string ItemValue { get; set; }

        public virtual int? ItemOrder { get; set; }

        public virtual string Description { get; set; }

        public virtual string IsUse { get; set; }

        public virtual string IsRestrict { get; set; }
    }
}
