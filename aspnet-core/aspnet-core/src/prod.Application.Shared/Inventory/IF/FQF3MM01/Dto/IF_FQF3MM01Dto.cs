using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace prod.Inventory.IF.FQF3MM01.Dto
{
    public class IF_FQF3MM01Dto : EntityDto<long?>
    {
        public virtual int? RecordId { get; set; }

        public virtual string Vin { get; set; }

        public virtual string Urn { get; set; }

        public virtual string SpecSheetNo { get; set; }

        public virtual string IdLine { get; set; }

        public virtual string Katashiki { get; set; }

        public virtual string SaleKatashiki { get; set; }

        public virtual string SaleSuffix { get; set; }

        public virtual string Spec200Digits { get; set; }

        public virtual string ProductionSuffix { get; set; }

        public virtual string LotCode { get; set; }

        public virtual string EnginePrefix { get; set; }

        public virtual string EngineNo { get; set; }

        public virtual string PlantCode { get; set; }

        public virtual string CurrentStatus { get; set; }

        public virtual string LineOffDatetime { get; set; }

        public virtual string InteriorColor { get; set; }

        public virtual string ExteriorColor { get; set; }

        public virtual string DestinationCode { get; set; }

        public virtual string EdOdno { get; set; }

        public virtual string CancelFlag { get; set; }

        public virtual string SmsCarFamilyCode { get; set; }

        public virtual string OrderType { get; set; }

        public virtual string KatashikiCode { get; set; }

        public virtual string EndOfRecord { get; set; }

    }

    public class GetIF_FQF3MM01Input : PagedAndSortedResultRequestDto
    {

        public virtual string LotCode { get; set; }

        public virtual string Vin { get; set; }

        public virtual DateTime? LineOffDatetimeFrom { get; set; }
        public virtual DateTime? LineOffDatetimeTo { get; set; }

        public virtual string SmsCarFamilyCode { get; set; }

    }

    public class GetIF_FQF3MM01_VALIDATE : EntityDto<long?>
    {
        public virtual int? RecordId { get; set; }
        public virtual string Vin { get; set; }
        public virtual string SpecSheetNo { get; set; }
        public virtual string IdLine { get; set; }
        public virtual string Katashiki { get; set; }
        public virtual string SaleKatashiki { get; set; }
        public virtual string SaleSuffix { get; set; }
        public virtual string Spec200Digits { get; set; }
        public virtual string ProductionSuffix { get; set; }
        public virtual string EnginePrefix { get; set; }
        public virtual string EngineNo { get; set; }
        public virtual string PlantCode { get; set; }
        public virtual string CurrentStatus { get; set; }
        public virtual string LineOffDatetime { get; set; }
        public virtual string InteriorColor { get; set; }
        public virtual string ExteriorColor { get; set; }
        public virtual string DestinationCode { get; set; }
        public virtual string EdOdno { get; set; }
        public virtual string CancelFlag { get; set; }
        public virtual string SmsCarFamilyCode { get; set; }
        public virtual string OrderType { get; set; }
        public virtual string EndOfRecord { get; set; }
        public virtual long? HeaderFwgId { get; set; }
        public virtual long? HeaderId { get; set; }
        public virtual string TrailerId { get; set; }
        public virtual DateTime? LineOffDate { get; set; }
        public virtual string ErrorDescription { get; set; }

    }

    public class GetIF_FQF3MM01_VALIDATEInput : PagedAndSortedResultRequestDto
    {
        public virtual DateTime? LineOffDatetimeFrom { get; set; }
        public virtual DateTime? LineOffDatetimeTo { get; set; }

    }


}
