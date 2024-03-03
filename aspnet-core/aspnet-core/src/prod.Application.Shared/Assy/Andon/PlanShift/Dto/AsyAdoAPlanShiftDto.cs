using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Assy.Andon.Dto
{

    public class AsyAdoAPlanShiftDto : EntityDto<long?>
    {

        public virtual DateTime? WorkingDate { get; set; }

        public virtual string ProdLine { get; set; }

        public virtual int? Shift1 { get; set; }

        public virtual int? Shift2 { get; set; }

        public virtual int? Shift3 { get; set; }

        public virtual string IsActive { get; set; }

    }
      

    public class GetAsyAdoAPlanShiftInput : PagedAndSortedResultRequestDto
    {

        public virtual DateTime? WorkingDate { get; set; }

        public virtual string ProdLine { get; set; }

        public virtual int? Shift1 { get; set; }

        public virtual int? Shift2 { get; set; }

        public virtual int? Shift3 { get; set; }

        public virtual string IsActive { get; set; }

    }

}


