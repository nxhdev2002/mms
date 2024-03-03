using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Inventory.CKD.Dto
{

    public class InvCkdContainerDeliveryGateInExportInput
    {

        public virtual string PlateNo { get; set; }

        public virtual string ContainerNo { get; set; }

        public virtual string Renban { get; set; }

        public virtual DateTime? DateIn { get; set; }

        public virtual string DriverName { get; set; }

        public virtual string Forwarder { get; set; }

        public virtual int? TimeIn { get; set; }

        public virtual int? TimeOut { get; set; }

        public virtual long? CkdReqId { get; set; }

        public virtual string CardNo { get; set; }

        public virtual string Mobile { get; set; }

        public virtual DateTime? CallTime { get; set; }

        public virtual DateTime? CancelTime { get; set; }

        public virtual DateTime? StartTime { get; set; }

        public virtual DateTime? FinishTime { get; set; }

        public virtual string IdNo { get; set; }

        public virtual string IsActive { get; set; }
        public virtual DateTime? DateFrom { get; set; }
        public virtual DateTime? DateTo { get; set; }

    }

}


