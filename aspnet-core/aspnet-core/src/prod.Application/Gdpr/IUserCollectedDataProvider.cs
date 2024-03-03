using System.Collections.Generic;
using System.Threading.Tasks;
using Abp;
using prod.Dto;

namespace prod.Gdpr
{
    public interface IUserCollectedDataProvider
    {
        Task<List<FileDto>> GetFiles(UserIdentifier user);
    }
}
