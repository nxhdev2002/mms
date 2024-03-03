using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Master.CKD.Dto
{

    public class MstCkdCustomsLeadtimeDto : EntityDto<long?>
    {

        public virtual string SupplierNo { get; set; }

        public virtual int Leadtime { get; set; }



    }

    public class CreateOrEditMstCkdCustomsLeadtimeDto : EntityDto<long?>
    {

        [StringLength(MstCkdCustomsLeadtimeConsts.MaxSupplierNoLength)]
        public virtual string SupplierNo { get; set; }

        public virtual int Leadtime { get; set; }
    }

    public class GetMstCkdCustomsLeadtimeInput : PagedAndSortedResultRequestDto
    {

        public virtual string SupplierNo { get; set; }

        public virtual int Leadtime { get; set; }

    }

}


