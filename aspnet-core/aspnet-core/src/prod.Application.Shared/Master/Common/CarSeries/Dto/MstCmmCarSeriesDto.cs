using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace prod.Master.Common.CarSeries.Dto
{
    public class MstCmmCarSeriesDto : EntityDto<long?>
    {
        public virtual string Code { get; set; }

        public virtual string Name { get; set; }

    }
    public class CreateOrEditMstCmmCarSeriesDto : EntityDto<long?>
    {

        [StringLength(MstCmmCarSeriesConsts.MaxCodeLength)]
        public virtual string Code { get; set; }

        [StringLength(MstCmmCarSeriesConsts.MaxNameLength)]
        public virtual string Name { get; set; }
    }

    public class GetMstCmmCarSeriesInput : PagedAndSortedResultRequestDto
    {

        public virtual string Code { get; set; }

        public virtual string Name { get; set; }


    }
}
