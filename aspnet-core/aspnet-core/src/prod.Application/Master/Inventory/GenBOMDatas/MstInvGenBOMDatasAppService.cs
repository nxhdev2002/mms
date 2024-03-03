using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Dapper.Repositories;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Microsoft.EntityFrameworkCore;
using prod.Authorization;
using prod.Dto;
using prod.Master.Inventory.MstInvGenBOMData;
using prod.Master.Inventory.MstInvGenBOMData.Dto;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using vovina.Master.Inventory.Exporting;
namespace prod.Master.Inventory.GenBOMDatas
{
    [AbpAuthorize(AppPermissions.Pages_InvtSetup_MstInvGenBOMData_View)]
    public class MstInvGenBOMDataSAppService : prodAppServiceBase, IMstInvGenBOMDataAppService
    {
        private readonly IDapperRepository<MstInvGenBOMDatas, long> _dapperRepo;
        private readonly IRepository<MstInvGenBOMDatas, long> _repo;
        private readonly IMstInvGenBOMDatasExcelExporter _calendarListExcelExporter;

        public MstInvGenBOMDataSAppService(IRepository<MstInvGenBOMDatas, long> repo,
            IDapperRepository<MstInvGenBOMDatas, long> dapperRepo,
            IMstInvGenBOMDatasExcelExporter calendarListExcelExporter)
        {
            _repo = repo;
            _calendarListExcelExporter = calendarListExcelExporter;
            _calendarListExcelExporter = calendarListExcelExporter;
        }

        public async Task<PagedResultDto<MstInvGenBOMDataDto>> GetAll(GetInvGenBOMDataInput input)
        {
            var filtered = _repo.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.DataFieldName), e => e.DataFieldName.Contains(input.DataFieldName))
                .WhereIf(!string.IsNullOrWhiteSpace(input.DataFieldDescription), e => e.DataFieldDescription.Contains(input.DataFieldDescription));
            var pageAndFiltered = filtered.OrderBy(s => s.CreationTime);


            var system = from o in pageAndFiltered
                         select new MstInvGenBOMDataDto
                         {
                             Id = o.Id,
                             FileId = o.FileId,
                             DataFieldName = o.DataFieldName,
                             DataFieldLengh = o.DataFieldLengh,
                             DataFieldType = o.DataFieldType,
                             DataFieldDescription = o.DataFieldDescription
                         };

            var totalCount = await filtered.CountAsync();
            var paged = system.PageBy(input);

            return new PagedResultDto<MstInvGenBOMDataDto>(
                totalCount,
                 await paged.ToListAsync()
            );
        }
        public async Task<FileDto> GetGenBOMDatasToExcel(GetInvGenBOMDataInput input)
        {
            var filtered = _repo.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.DataFieldName), e => e.DataFieldName.Contains(input.DataFieldName))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DataFieldDescription), e => e.DataFieldDescription.Contains(input.DataFieldDescription));
            var pageAndFiltered = filtered.OrderBy(s => s.Id);
            var system = from o in pageAndFiltered
                         select new MstInvGenBOMDataDto
                        {
                            Id = o.Id,
                            FileId = o.FileId,
                            DataFieldName = o.DataFieldName,
                            DataFieldLengh = o.DataFieldLengh,
                            DataFieldType = o.DataFieldType,
                            DataFieldDescription = o.DataFieldDescription
                        };
            var exportToExcel = await system.ToListAsync();
            return _calendarListExcelExporter.ExportToFile(exportToExcel);
        }
    }

}
