using System.Threading.Tasks;
using Abp.Application.Services;
using prod.Install.Dto;

namespace prod.Install
{
    public interface IInstallAppService : IApplicationService
    {
        Task Setup(InstallDto input);

        AppSettingsJsonDto GetAppSettingsJson();

        CheckDatabaseOutput CheckDatabase();
    }
}