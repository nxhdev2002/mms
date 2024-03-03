using Abp.Application.Services;
using Abp.Application.Services.Dto;
using prod.Master.Common.DriveTrain.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace prod.Master.Common.DriveTrain
{
    public interface IMstCmmDriveTrainAppservice : IApplicationService
    {
        Task<PagedResultDto<MstCmmDriveTrainDto>> GetAll(GetMstCmmDriveTrainInput input);

        //Task CreateOrEdit(CreateOrEditMstCmmDriveTrainDto input);

        //Task Delete(EntityDto input);
    }
}
