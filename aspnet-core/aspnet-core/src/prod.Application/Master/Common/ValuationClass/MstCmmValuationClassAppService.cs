using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Dapper.Repositories;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Microsoft.EntityFrameworkCore;
using prod.Authorization;
using prod.Dto;
using prod.Master.Common.Dto;
using prod.Master.Common.Exporting;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace prod.Master.Common
{
    [AbpAuthorize(AppPermissions.Pages_InvtSetup_ValuationClass_View)]
    public class MstCmmValuationClassAppService : prodAppServiceBase, IMstCmmValuationClassAppService
    {
        private readonly IDapperRepository<MstCmmValuationClass, long> _dapperRepo;
        private readonly IRepository<MstCmmValuationClass, long> _repo;
        private readonly IMstCmmValuationClassExcelExporter _calendarListExcelExporter;

        public MstCmmValuationClassAppService(IRepository<MstCmmValuationClass, long> repo,
                                         IDapperRepository<MstCmmValuationClass, long> dapperRepo,
                                        IMstCmmValuationClassExcelExporter calendarListExcelExporter
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
        }

        public async Task<PagedResultDto<MstCmmValuationClassDto>> GetAll(GetMstCmmValuationClassInput input)
        {
            var filtered = _repo.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.Name), e => e.Name.Contains(input.Name))
                .WhereIf(!string.IsNullOrWhiteSpace(input.BsAccount), e => e.BsAccount.Contains(input.BsAccount))
                ;
            var pageAndFiltered = filtered.OrderBy(s => s.Id);


            var system = from o in pageAndFiltered
                         select new MstCmmValuationClassDto
                         {
                             Id = o.Id,
                             Code = o.Code,
                             Name = o.Name,
                             BsAccount = o.BsAccount,
                             BsAccountDescription = o.BsAccountDescription,
                         };

            var totalCount = await filtered.CountAsync();
            var paged = system.PageBy(input);

            return new PagedResultDto<MstCmmValuationClassDto>(
                totalCount,
                 await paged.ToListAsync()
            );
        }


        public async Task<FileDto> GetValuationClassToExcel(MstCmmValuationClassExportInput input)
        {
            var filtered = _repo.GetAll()
             .WhereIf(!string.IsNullOrWhiteSpace(input.Name), e => e.Name.Contains(input.Name))
             .WhereIf(!string.IsNullOrWhiteSpace(input.BsAccount), e => e.BsAccount.Contains(input.BsAccount))
             ;
            var pageAndFiltered = filtered.OrderBy(s => s.Id);

            var system = from o in pageAndFiltered
                         select new MstCmmValuationClassDto
                         {
                            Id = o.Id,
                            Code = o.Code,
                            Name = o.Name,
                            BsAccount = o.BsAccount,
                            BsAccountDescription = o.BsAccountDescription,

                        };
            var exportToExcel = await system.ToListAsync();
            return _calendarListExcelExporter.ExportToFile(exportToExcel);
        }


    }
}
