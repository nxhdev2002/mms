using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Inventory.CKD.Dto
{
    public class InvCkdPartListExportInput
    {
        public virtual string PartNo { get; set; }
        public virtual string Cfc { get; set; }
        public virtual string Model { get; set; }
        public virtual string Grade { get; set; }
        public virtual string Shop { get; set; }
        public virtual string SupplierNo { get; set; }
        public virtual string OrderPattern { get; set; }
    }

    public class InvCkdPartListDetailsExportInput
    {
        public virtual string PartNo { get; set; }
        public virtual string Cfc { get; set; }
        public virtual string Model { get; set; }
        public virtual string Grade { get; set; }
        public virtual string Shop { get; set; }
        public virtual string SupplierNo { get; set; }
        public virtual string OrderPattern { get; set; }
        public virtual string IsActive { get; set; }
    }

}


