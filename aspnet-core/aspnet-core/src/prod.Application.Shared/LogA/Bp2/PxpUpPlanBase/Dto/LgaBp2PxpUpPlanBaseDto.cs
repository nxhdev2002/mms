using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace prod.LogA.Bp2.Dto
{
    public class LgaBp2PxpUpPlanBaseDto : EntityDto<long?>
    {
        public virtual DateTime? WorkingDate { get; set; }

        public virtual string ProdLine { get; set; }

        public virtual int? Shift1 { get; set; }

        public virtual int? Shift2 { get; set; }

        public virtual int? Shift3 { get; set; }

        public virtual string IsActive { get; set; }

        public virtual string Shift { get; set; }

        public virtual int? StartNoInShift { get; set; }

        public virtual int? EndNoInShift { get; set; }



    }


}
