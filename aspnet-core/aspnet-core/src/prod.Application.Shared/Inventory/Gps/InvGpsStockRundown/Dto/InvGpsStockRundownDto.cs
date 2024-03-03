using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Inventory.GPS.Dto
{

    public class InvGpsStockRundownDto : EntityDto<long?>
    {

        public virtual string PartNo { get; set; }

        public virtual string PartName { get; set; }

        public virtual string Uom { get; set; }

        //public virtual long? MaterialId { get; set; }

        //public virtual decimal? Qty { get; set; }

        //public virtual DateTime? WorkingDate { get; set; }

        //public virtual long? TransactionId { get; set; }

        //
        public virtual decimal? A1 { get; set; }
        public virtual decimal? A2 { get; set; }
        public virtual decimal? A3 { get; set; }
        public virtual decimal? A4 { get; set; }
        public virtual decimal? A5 { get; set; }
        public virtual decimal? A6 { get; set; }
        public virtual decimal? A7 { get; set; }
        public virtual decimal? A8 { get; set; }
        public virtual decimal? A9 { get; set; }
        public virtual decimal? A10 { get; set; }
        public virtual decimal? A11 { get; set; }
        public virtual decimal? A12 { get; set; }
        public virtual decimal? A13 { get; set; }
        public virtual decimal? A14 { get; set; }
        public virtual decimal? A15 { get; set; }
        public virtual decimal? A16 { get; set; }
        public virtual decimal? A17 { get; set; }
        public virtual decimal? A18 { get; set; }
        public virtual decimal? A19 { get; set; }
        public virtual decimal? A20 { get; set; }
        public virtual decimal? A21 { get; set; }
        public virtual decimal? A22 { get; set; }
        public virtual decimal? A23 { get; set; }
        public virtual decimal? A24 { get; set; }
        public virtual decimal? A25 { get; set; }
        public virtual decimal? A26 { get; set; }
        public virtual decimal? A27 { get; set; }
        public virtual decimal? A28 { get; set; }
        public virtual decimal? A29 { get; set; }
        public virtual decimal? A30 { get; set; }
        public virtual decimal? A31 { get; set; }
        public virtual decimal? A32 { get; set; }
        public virtual decimal? A33 { get; set; }
        public virtual decimal? A34 { get; set; }
        public virtual decimal? A35 { get; set; }
        public virtual decimal? A36 { get; set; }
        public virtual decimal? A37 { get; set; }
        public virtual decimal? A38 { get; set; }
        public virtual decimal? A39 { get; set; }
        public virtual decimal? A40 { get; set; }
        public virtual decimal? A41 { get; set; }
        public virtual decimal? A42 { get; set; }
        public virtual decimal? A43 { get; set; }
        public virtual decimal? A44 { get; set; }
        public virtual decimal? A45 { get; set; }
        public virtual decimal? A46 { get; set; }
        public virtual decimal? A47 { get; set; }
        public virtual decimal? A48 { get; set; }
        public virtual decimal? A49 { get; set; }
        public virtual decimal? A50 { get; set; }
        public virtual decimal? A51 { get; set; }
        public virtual decimal? A52 { get; set; }
        public virtual decimal? A53 { get; set; }
        public virtual decimal? A54 { get; set; }
        public virtual decimal? A55 { get; set; }
        public virtual decimal? A56 { get; set; }
        public virtual decimal? A57 { get; set; }
        public virtual decimal? A58 { get; set; }
        public virtual decimal? A59 { get; set; }
        public virtual decimal? A60 { get; set; }
        public virtual decimal? A61 { get; set; }

    }

    public class GetInvGpsStockRundownInput : PagedAndSortedResultRequestDto
    {
        public virtual string PartNo { get; set; }

    }

}


