using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Inventory.CKD.Dto
{

    public class InvCkdContainerRentalWHPlanExportInput
    {

        public virtual string ContainerNo { get; set; }

        public virtual string Renban { get; set; }

        public virtual DateTime? RequestDate { get; set; }

        public virtual DateTime? RequestTime { get; set; }

        public virtual string InvoiceNo { get; set; }

        public virtual string BillofladingNo { get; set; }

        public virtual string SupplierNo { get; set; }

        public virtual string SealNo { get; set; }

		public virtual DateTime? RequestDateFrom { get; set; }

		public virtual DateTime? RequestDateTo { get; set; }

	}

}


