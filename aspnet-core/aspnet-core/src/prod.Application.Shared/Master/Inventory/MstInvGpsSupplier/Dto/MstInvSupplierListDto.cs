using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Master.Inventory.Dto
{

    public class MstInvSupplierListDto : EntityDto<long?>
    {

        public virtual string SupplierNo { get; set; }

        public virtual string SupplierName { get; set; }

        public virtual string Remarks { get; set; }

        public virtual string SupplierType { get; set; }

        public virtual string SupplierNameVn { get; set; }

        public virtual string Exporter { get; set; }

    }


    public class GetMstInvSupplierListInput : PagedAndSortedResultRequestDto
    {

        public virtual string SupplierNo { get; set; }

        public virtual string SupplierName { get; set; }

        public virtual string Remarks { get; set; }

        public virtual string SupplierType { get; set; }

        public virtual string SupplierNameVn { get; set; }

        public virtual string Exporter { get; set; }

    }
     public class MstInvSupplierListInput 
    {

        public virtual string SupplierNo { get; set; }

        public virtual string SupplierName { get; set; }

        public virtual string Remarks { get; set; }

        public virtual string SupplierType { get; set; }

        public virtual string SupplierNameVn { get; set; }

        public virtual string Exporter { get; set; }

    }

}


