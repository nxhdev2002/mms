using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Inventory.Tmss.Dto
{

    public class InvTmssDispatchPlanExportInput
    {

        public virtual string VehicleType { get; set; }

        public virtual string Model { get; set; }

        public virtual string MarketingCode { get; set; }

        public virtual string Vin { get; set; }


        public virtual DateTime? DlrDispatchPlanDateFrom { get; set; }

        public virtual DateTime? DlrDispatchPlanDateTo { get; set; }

        public virtual DateTime? DlrDispatchDateFrom { get; set; }

        public virtual DateTime? DlrDispatchDateTo { get; set; }

    }

}


