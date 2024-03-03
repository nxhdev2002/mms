using Abp.Application.Services;
using prod.Dto;
using prod.Master.Common.DriveTrain.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prod.Master.Common.DriveTrain.Exporting
{
    public interface IMstCmmDriveTrainExcelExporter : IApplicationService
    {
        FileDto ExportToFile(List<MstCmmDriveTrainDto> mstcmmdrivetrain);
    }
}
