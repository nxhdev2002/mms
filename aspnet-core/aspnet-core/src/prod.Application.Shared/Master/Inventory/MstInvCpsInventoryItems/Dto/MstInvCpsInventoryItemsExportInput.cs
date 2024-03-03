using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Master.Inventory.Dto
{

    public class MstInvCpsInventoryItemsExportInput
    {

        public virtual string PartNo { get; set; }

        public virtual string PartName { get; set; }

        public virtual string Color { get; set; }

        public virtual string Puom { get; set; }

    }

}