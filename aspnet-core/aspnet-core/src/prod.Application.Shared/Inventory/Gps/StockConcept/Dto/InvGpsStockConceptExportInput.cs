using System;
using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Inventory.GPS.Dto
{

    public class InvGpsStockConceptExportInput
    {

        public virtual string SupplierCode { get; set; }

       
        public virtual string StkConcept { get; set; }

   

    }

}


