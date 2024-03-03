using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace prod.Master.Inventory.Dto
{
    public class MstInvDemDetFeesDto : EntityDto<long?>
    {
        public virtual string Source { get; set; }

        public virtual string Carrier { get; set; }

        public virtual int ContType { get; set; }

        public virtual int NoOfDayOVF { get; set; }

        public virtual decimal? DemFee { get; set; }

        public virtual decimal? DetFee { get; set; }

        public virtual decimal? DemAndDetFee { get; set; }
        public virtual int? MinDay { get; set; }
        public virtual int? MaxDay { get; set; }
        public virtual DateTime? EffectiveDateFrom { get; set; }
        public virtual DateTime? EffectiveDateTo { get; set; }
        public virtual string IsActive { get; set; }
        public virtual string IsMax { get; set; }
    }


    public class GetMstInvDemDetFeesInput : PagedAndSortedResultRequestDto
    {

        public virtual string Source { get; set; }

        public virtual string Carrier { get; set; }


    }
    public class CreateOrEditMstInvDemDetFeesDto : EntityDto<long?>
    {

        public const int MaxSourceLength = 50;
        public const int MaxCarrierLength = 50;
        public const int MaxIsActiveLength = 1;

        [StringLength(MaxSourceLength)]
        public virtual string Source { get; set; }

        [StringLength(MaxCarrierLength)]
        public virtual string Carrier { get; set; }
        public virtual int? ContType { get; set; }
        public virtual int? NoOfDayOVF { get; set; }
        public virtual decimal? DemFee { get; set; }
        public virtual decimal? DetFee { get; set; }
        public virtual decimal? DemAndDetFee { get; set; }
        public virtual string IsMax { get; set; }
        public virtual DateTime? EffectiveDateFrom { get; set; }
        public virtual DateTime? EffectiveDateTo { get; set; }

        [StringLength(MaxIsActiveLength)]
        public virtual string IsActive { get; set; }
        public virtual int? MinDay { get; set; }
        public virtual int? MaxDay { get; set; }



    }
    public class MstInvDemDetFeesImportDto : EntityDto<long?>
    {
        public virtual string Source { get; set; }

        public virtual string Carrier { get; set; }

        public virtual int ContType { get; set; }

        public virtual int NoOfDayOVF { get; set; }

        public virtual decimal? DemFee { get; set; }

        public virtual decimal? DetFee { get; set; }

        public virtual decimal? DemAndDetFee { get; set; }
        public virtual string IsMax { get; set; }
        public virtual string Guid { get; set; }
        public virtual string ErrorDescription { get; set; }
        public virtual int MinDay { get; set; }
        public virtual int MaxDay { get; set; }
    }
    public class GetSourceDto
    {
        public virtual string Source { get; set; }
    }
    public class GetCarrieerDto
    {
        public virtual string Carrier { get; set; }
    }

}
