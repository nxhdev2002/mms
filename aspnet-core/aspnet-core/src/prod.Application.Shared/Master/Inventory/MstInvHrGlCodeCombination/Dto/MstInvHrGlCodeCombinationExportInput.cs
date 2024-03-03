using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Master.Inventory.Dto
{

    public class MstInvHrGlCodeCombinationExportInput
    {
        public virtual string AccountType { get; set; }

        public virtual string EnabledFlag { get; set; }

        public virtual string Segment1 { get; set; }

        public virtual string Segment2 { get; set; }

        public virtual string Segment3 { get; set; }

        public virtual string Segment4 { get; set; }

        public virtual string Segment5 { get; set; }

        public virtual string Segment6 { get; set; }

        public virtual string Concatenatedsegments { get; set; }

        public virtual string IsActive { get; set; }

    }

}
