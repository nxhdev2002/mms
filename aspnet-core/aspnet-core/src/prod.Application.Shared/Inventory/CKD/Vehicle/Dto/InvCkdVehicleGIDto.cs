using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace prod.Inventory.CKD.Vehicle.Dto
{
    public class InvCkdVehicleGIDto
    {
        public virtual string VinNo { get; set; }

        public virtual string LotNo { get; set; }

        public virtual int? NoInLot { get; set; }

        public virtual string SequenceNo { get; set; }

        public virtual DateTime PdiDate { get; set; }

        public virtual string BodyNo { get; set; }

        public virtual string Cfc { get; set; }


    }
}
