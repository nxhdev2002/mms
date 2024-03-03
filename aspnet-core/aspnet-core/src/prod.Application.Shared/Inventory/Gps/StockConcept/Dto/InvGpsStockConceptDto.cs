using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Inventory.GPS.Dto
{

    public class InvGpsStockConceptDto : EntityDto<long?>
    {

        public virtual string Guid { get; set; }

        public virtual string RowNumber { get; set; }
        public virtual string SupplierCode { get; set; }

        public virtual DateTime? MonthStk { get; set; }
        public virtual string StkType { get; set; }

        public virtual decimal? Frequency { get; set; }
        public virtual decimal? StkFrequency { get; set; }


        public virtual decimal? MinStk1 { get; set; }

        public virtual decimal? MinStk2 { get; set; }

        public virtual decimal? MinStk3 { get; set; }

        public virtual decimal? MinStk4 { get; set; }

        public virtual decimal? MinStk5 { get; set; }

        public virtual decimal? MinStk6 { get; set; }

        public virtual decimal? MinStk7 { get; set; }

        public virtual decimal? MinStk8 { get; set; }

        public virtual decimal? MinStk9 { get; set; }

        public virtual decimal? MinStk10 { get; set; }

        public virtual decimal? MinStk11 { get; set; }

        public virtual decimal? MinStk12 { get; set; }

        public virtual decimal? MinStk13 { get; set; }

        public virtual decimal? MinStk14 { get; set; }

        public virtual decimal? MinStk15 { get; set; }

        public virtual decimal? MaxStk1 { get; set; }

        public virtual decimal? MaxStk2 { get; set; }

        public virtual decimal? MaxStk3 { get; set; }

        public virtual decimal? MaxStk4 { get; set; }

        public virtual decimal? MaxStk5 { get; set; }

        public virtual decimal? MinStkConcept { get; set; }

        public virtual decimal? MaxStkConcept { get; set; }

        public virtual string StkConcept { get; set; }

        public virtual int? StkConceptFrq { get; set; }

        public virtual string IsActive { get; set; }

    }

    public class CreateOrEditInvGpsStockConceptDto : EntityDto<long?>
    {

        [StringLength(InvGpsStockConceptConsts.MaxSupplierCodeLength)]
        public virtual string SupplierCode { get; set; }

        public virtual DateTime? MonthStk { get; set; }
        public virtual string StkType { get; set; }


        public virtual decimal? MinStk1 { get; set; }

        public virtual decimal? MinStk2 { get; set; }

        public virtual decimal? MinStk3 { get; set; }

        public virtual decimal? MinStk4 { get; set; }

        public virtual decimal? MinStk5 { get; set; }

        public virtual decimal? MinStk6 { get; set; }

        public virtual decimal? MinStk7 { get; set; }

        public virtual decimal? MinStk8 { get; set; }

        public virtual decimal? MinStk9 { get; set; }

        public virtual decimal? MinStk10 { get; set; }

        public virtual decimal? MinStk11 { get; set; }

        public virtual decimal? MinStk12 { get; set; }

        public virtual decimal? MinStk13 { get; set; }

        public virtual decimal? MinStk14 { get; set; }

        public virtual decimal? MinStk15 { get; set; }

        public virtual decimal? MaxStk1 { get; set; }

        public virtual decimal? MaxStk2 { get; set; }

        public virtual decimal? MaxStk3 { get; set; }

        public virtual decimal? MaxStk4 { get; set; }

        public virtual decimal? MaxStk5 { get; set; }

        public virtual decimal? MinStkConcept { get; set; }

        public virtual decimal? MaxStkConcept { get; set; }

        [StringLength(InvGpsStockConceptConsts.MaxStkConceptLength)]
        public virtual string StkConcept { get; set; }

        public virtual int? StkConceptFrq { get; set; }

        [StringLength(InvGpsStockConceptConsts.MaxIsActiveLength)]
        public virtual string IsActive { get; set; }
    }

    public class GetInvGpsStockConceptInput : PagedAndSortedResultRequestDto
    {

        public virtual string SupplierCode { get; set; }

        public virtual string StkConcept { get; set; }


    }

}


