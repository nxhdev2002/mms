using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Master.Spp.Dto
{

    public class MstSppCustomerDto : EntityDto<long?>
    {

        public virtual string Code { get; set; }

        public virtual string Name { get; set; }

        public virtual string Rep { get; set; }

        public virtual int FromMonth { get; set; }

        public virtual int FromYear { get; set; }

        public virtual int ToMonth { get; set; }

        public virtual int ToYear { get; set; }

        public virtual long FromPeriodId { get; set; }

        public virtual long ToPeriodId { get; set; }

        public virtual string IsNew { get; set; }

        public virtual long OraCustomerId { get; set; }

    }

    public class CreateOrEditMstSppCustomerDto : EntityDto<long?>
    {

        [StringLength(MstSppCustomerConsts.MaxCodeLength)]
        public virtual string Code { get; set; }

        [StringLength(MstSppCustomerConsts.MaxNameLength)]
        public virtual string Name { get; set; }

        [StringLength(MstSppCustomerConsts.MaxRepLength)]
        public virtual string Rep { get; set; }

        public virtual int FromMonth { get; set; }

        public virtual int FromYear { get; set; }

        public virtual int ToMonth { get; set; }

        public virtual int ToYear { get; set; }

        public virtual long FromPeriodId { get; set; }

        public virtual long ToPeriodId { get; set; }

        [StringLength(MstSppCustomerConsts.MaxIsNewLength)]
        public virtual string IsNew { get; set; }

        public virtual long OraCustomerId { get; set; }
    }

    public class GetMstSppCustomerInput : PagedAndSortedResultRequestDto
    {

        public virtual string Code { get; set; }

        public virtual DateTime? FromMonthYear { get; set; }
        public virtual DateTime? ToMonthYear { get; set; }


    }

}


