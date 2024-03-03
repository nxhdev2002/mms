using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Master.Painting.Dto
{

    public class MstPtsInventoryStdExportInput
    {

        public virtual string Model { get; set; }

        public virtual int? InventoryStd { get; set; }

        public virtual string IsActive { get; set; }

    }

}
