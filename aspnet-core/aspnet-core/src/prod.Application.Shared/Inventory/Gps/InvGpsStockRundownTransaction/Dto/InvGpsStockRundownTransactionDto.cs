using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Inventory.GPS.Dto
{

    public class InvGpsStockRundownTransactionDto : EntityDto<long?>
    {

        public virtual string PartNo { get; set; }

        public virtual string PartName { get; set; }

        public virtual long? PartId { get; set; }

        public virtual long? MaterialId { get; set; }

        public virtual decimal? Qty { get; set; }

        public virtual DateTime? WorkingDate { get; set; }

        public virtual DateTime? TransactionDate { get; set; }

        public virtual long? TransactionId { get; set; }

    }


    public class GetInvGpsStockRundownTransactionInput : PagedAndSortedResultRequestDto
    {

        public virtual string PartNo { get; set; }

        public virtual string PartName { get; set; }


    }

}


