using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Inventory.IF.FQF3MM04.Dto
{

    public class IF_FQF3MM04Dto : EntityDto<long?>
    {

        public virtual int? RecordId { get; set; }

        public virtual string InvoiceNo { get; set; }

        public virtual string Renban { get; set; }

        public virtual string ContainerNo { get; set; }

        public virtual string ModuleNo { get; set; }

        public virtual DateTime? DevaningDate { get; set; }

        public virtual string Plant { get; set; }

        public virtual string CancelFlag { get; set; }

        public virtual string EndOfRecord { get; set; }

    }
    public class GetIF_FQF3MM04Input : PagedAndSortedResultRequestDto
    {
        public virtual DateTime? DevaningDateFrom { get; set; }

        public virtual DateTime? DevaningDateTo { get; set; }

        public virtual string InvoiceNo { get; set; }

        public virtual string Renban { get; set; }

        public virtual string ContainerNo { get; set; }

        public virtual string ModuleNo { get; set; }

    }


    public class GetIF_FQF3MM04_VALIDATE : EntityDto<long?>
    {
        public virtual int? RecordId { get; set; }

        public virtual string InvoiceNo { get; set; }

        public virtual string Renban { get; set; }

        public virtual string ContainerNo { get; set; }

        public virtual string ModuleNo { get; set; }

        public virtual DateTime? DevaningDate { get; set; }

        public virtual string Plant { get; set; }

        public virtual string CancelFlag { get; set; }

        public virtual string EndOfRecord { get; set; }
        public virtual long? HeaderFwgId { get; set; }
        public virtual long? HeaderId { get; set; }
        public virtual string TrailerId { get; set; }
        public virtual string ErrorDescription { get; set; }

    }


    public class GetIF_FQF3MM04_VALIDATEInput : PagedAndSortedResultRequestDto
    {
        public virtual DateTime? DevaningDateFrom { get; set; }

        public virtual DateTime? DevaningDateTo { get; set; }
    }

}


