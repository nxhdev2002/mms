using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Inventory.GPS.Dto
{

    public class InvGpsKanbanExportInput
    {
        public virtual string BackNo { get; set; }

        public virtual string PartNo { get; set; }
    }

}


