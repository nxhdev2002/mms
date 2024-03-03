using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Inventory.CKD.Dto
{

    public class InvCkdPhysicalConfirmLotDto : EntityDto<long?>
    {
        public virtual string ModelCode { get; set; }

        public virtual string ProdLine { get; set; }

        public virtual string Grade { get; set; }

        public virtual int? StartLot { get; set; }

        public virtual int? StartRun { get; set; }

        public virtual DateTime? StartProcessDate { get; set; }

        public virtual int? EndLot { get; set; }

        public virtual int? EndRun { get; set; }

        public virtual DateTime? EndProcessDate { get; set; }

        public virtual int PeriodId { get; set; }

    }


    public class GetInvCkdPhysicalConfirmLotInput : PagedAndSortedResultRequestDto
    {

        public virtual string ModelCode { get; set; }

        public virtual string ProdLine { get; set; }

        public virtual string Grade { get; set; }


    }

    public class InvCkdPhysicalConfirmLot_TDto 
    {
        public virtual long? ROW_NO { get; set; }
        public virtual string Guid { get; set; }

        public virtual string ModelCode { get; set; }

        public virtual string ProdLine { get; set; }

        public virtual string Grade { get; set; }

        public virtual int? StartLot { get; set; }

        public virtual int? StartRun { get; set; }

        public virtual DateTime? StartProcessDate { get; set; }

        public virtual int? EndLot { get; set; }

        public virtual int? EndRun { get; set; }

        public virtual DateTime? EndProcessDate { get; set; }

        public virtual int PeriodId { get; set; }

        public string ErrorDescription { get; set; }

    }

}


