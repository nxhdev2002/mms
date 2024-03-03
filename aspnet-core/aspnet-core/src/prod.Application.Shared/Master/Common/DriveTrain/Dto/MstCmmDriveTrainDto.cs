using Abp.Application.Services.Dto;
using prod.Master.Common.CarSeries;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace prod.Master.Common.DriveTrain.Dto
{
    public class MstCmmDriveTrainDto : EntityDto<long?>
    {
        public virtual string Code { get; set; }

        public virtual string Name { get; set; }

    }
    public class CreateOrEditMstCmmDriveTrainDto : EntityDto<long?>
    {

        [StringLength(MstCmmDriveTrainConsts.MaxCodeLength)]
        public virtual string Code { get; set; }

        [StringLength(MstCmmDriveTrainConsts.MaxNameLength)]
        public virtual string Name { get; set; }
    }

    public class GetMstCmmDriveTrainInput : PagedAndSortedResultRequestDto
    {

        public virtual string Code { get; set; }

        public virtual string Name { get; set; }


    }
}
