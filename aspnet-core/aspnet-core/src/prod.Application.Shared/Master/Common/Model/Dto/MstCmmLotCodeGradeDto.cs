using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Master.Cmm.Dto
{
    public class MstCmmLotCodeGradeDto : EntityDto<long?>
    {
        public virtual string Model { get; set; }
        public virtual string LotCode { get; set; }
        public virtual string Cfc { get; set; }
        public virtual string Grade { get; set; }
        public virtual string GradeName { get; set; }
        public virtual string ModelCode { get; set; }
        public virtual string ModelVin { get; set; }
        public virtual string IsActive { get; set; }
    }

    public class GetMstCmmLotCodeGradeInput : PagedAndSortedResultRequestDto
    {
        public virtual string Cfc { get; set; }

        public virtual string Model { get; set; }

        public virtual string ModelVin { get; set; }

        public virtual string ModelCode { get; set; }
    }
}