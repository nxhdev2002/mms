using Abp.Application.Services.Dto;
using System;
namespace prod.Inventory.DRM.StockRundown.Dto
{
    public class InvDrmStockRundownDto : EntityDto<long?>
    {
        public virtual string MaterialCode { get; set; }

        public virtual string MaterialSpec { get; set; }

        public virtual long? DrmMaterialId { get; set; }

        public virtual string PartNo { get; set; }

        public virtual string PartName { get; set; }

        public virtual long? PartId { get; set; }

        public virtual long? MaterialId { get; set; }

        public virtual int? Qty { get; set; }

        public virtual DateTime? WorkingDate { get; set; }

        //
        public virtual int? A1 { get; set; }
        public virtual int? A2 { get; set; }
        public virtual int? A3 { get; set; }
        public virtual int? A4 { get; set; }
        public virtual int? A5 { get; set; }
        public virtual int? A6 { get; set; }
        public virtual int? A7 { get; set; }
        public virtual int? A8 { get; set; }
        public virtual int? A9 { get; set; }
        public virtual int? A10 { get; set; }
        public virtual int? A11 { get; set; }
        public virtual int? A12 { get; set; }
        public virtual int? A13 { get; set; }
        public virtual int? A14 { get; set; }
        public virtual int? A15 { get; set; }
        public virtual int? A16 { get; set; }
        public virtual int? A17 { get; set; }
        public virtual int? A18 { get; set; }
        public virtual int? A19 { get; set; }
        public virtual int? A20 { get; set; }
        public virtual int? A21 { get; set; }
        public virtual int? A22 { get; set; }
        public virtual int? A23 { get; set; }
        public virtual int? A24 { get; set; }
        public virtual int? A25 { get; set; }
        public virtual int? A26 { get; set; }
        public virtual int? A27 { get; set; }
        public virtual int? A28 { get; set; }
        public virtual int? A29 { get; set; }
        public virtual int? A30 { get; set; }
        public virtual int? A31 { get; set; }
        public virtual int? A32 { get; set; }
        public virtual int? A33 { get; set; }
        public virtual int? A34 { get; set; }
        public virtual int? A35 { get; set; }
        public virtual int? A36 { get; set; }
        public virtual int? A37 { get; set; }
        public virtual int? A38 { get; set; }
        public virtual int? A39 { get; set; }
        public virtual int? A40 { get; set; }
        public virtual int? A41 { get; set; }
        public virtual int? A42 { get; set; }
        public virtual int? A43 { get; set; }
        public virtual int? A44 { get; set; }
        public virtual int? A45 { get; set; }
        public virtual int? A46 { get; set; }
        public virtual int? A47 { get; set; }
        public virtual int? A48 { get; set; }
        public virtual int? A49 { get; set; }
        public virtual int? A50 { get; set; }
        public virtual int? A51 { get; set; }
        public virtual int? A52 { get; set; }
        public virtual int? A53 { get; set; }
        public virtual int? A54 { get; set; }
        public virtual int? A55 { get; set; }
        public virtual int? A56 { get; set; }
        public virtual int? A57 { get; set; }
        public virtual int? A58 { get; set; }
        public virtual int? A59 { get; set; }
        public virtual int? A60 { get; set; }
        public virtual int? A61 { get; set; }


    }

    public class GetInvDrmStockRundownInput : PagedAndSortedResultRequestDto
    {
        public virtual string MaterialCode { get; set; }

        public virtual string MaterialSpec { get; set; }
    }
}


