using Abp.Application.Services;
using prod.Dto;
using prod.Logging.Dto;

namespace prod.Logging
{
    public interface IWebLogAppService : IApplicationService
    {
        GetLatestWebLogsOutput GetLatestWebLogs();

        FileDto DownloadWebLogs();
    }
}
