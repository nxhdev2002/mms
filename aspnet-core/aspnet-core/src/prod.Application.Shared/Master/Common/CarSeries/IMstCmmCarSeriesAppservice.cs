using Abp.Application.Services;
using Abp.Application.Services.Dto;
using prod.Master.Common.CarSeries.Dto;
using prod.Master.Common.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace prod.Master.Common.CarSeries
{
    public interface IMstCmmCarSeriesAppservice : IApplicationService
    {
        Task<PagedResultDto<MstCmmCarSeriesDto>> GetAll(GetMstCmmCarSeriesInput input);

        //Task CreateOrEdit(CreateOrEditMstCmmCarSeriesDto input);

        //Task Delete(EntityDto input);
    }
}
