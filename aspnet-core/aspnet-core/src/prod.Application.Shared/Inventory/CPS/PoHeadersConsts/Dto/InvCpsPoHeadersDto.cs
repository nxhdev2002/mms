using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Inventory.CPS.Dto
{

    public class InvCpsPoHeadersDto : EntityDto<long?>
    {

        public virtual string TypeLookupCode { get; set; }

        public virtual string PoNumber { get; set; }

        public virtual long? VendorId { get; set; }

        public virtual string CurrencyCode { get; set; }

        public virtual string AuthorizationStatus { get; set; }

        public virtual decimal? TotalPrice { get; set; }

        public virtual decimal? TotalPriceUsd { get; set; }

        public virtual DateTime? ApprovedDate { get; set; }

        public virtual string Comments { get; set; }

        public virtual DateTime? SubmitDate { get; set; }

        public virtual string IsActive { get; set; }

    }

    public class GridPoHeadersDto : EntityDto<long?>
    {


        public virtual string PoNumber { get; set; }

        public virtual DateTime? CreationTime { get; set; }

        public virtual string Comments { get; set; }

        public virtual string TypeLookupCode { get; set; }

        public virtual string Productgroupname { get; set; }


        public virtual string SupplierName { get; set; }

        public virtual decimal? TotalPrice { get; set; }



    }


    public class GetInvCpsPoHeadersInput : PagedAndSortedResultRequestDto
    {


        public virtual string PoNumber { get; set; }

        public virtual long? InventoryGroup { get; set; }


        public virtual long? Supplier { get; set; } 

        public virtual DateTime? POCreationFromDate { get; set; }

        
        public virtual DateTime? POCreationToDate { get; set; }

        public virtual string PartNo { get; set; }

        public virtual string PartName { get; set; }

    }

    public class InvCpsPolinesDto : EntityDto<long?>
    {
        public virtual string PartNo { get; set; }

        public virtual string Color { get; set; }

        public virtual string ItemDescription { get; set; }

        public virtual string UnitMeasLookupCode { get; set; }

        public virtual decimal ListPricePerUnit { get; set; }

        public virtual decimal UnitPrice { get; set; }

        public virtual decimal Quantity { get; set; }

        public virtual decimal  TotalPrice { get; set; }

    }

    public class CbxInventoryGroupDto : EntityDto<long?>
    {
        public virtual long? Id { get; set; }

        public virtual string Productgroupcode { get; set; }

        public virtual string Productgroupname { get; set; }

    }
    public class CbxSupplierDto : EntityDto<long?>
    {
        public virtual long? Id { get; set; }

        public virtual string SupplierName { get; set; }

        public virtual string SupplierNumber { get; set; }

    }


    public class GetInvCpsLine : PagedAndSortedResultRequestDto
    {
        public virtual long? PoHeaderId { get; set; }
    

    }

}

