using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Dapper.Repositories;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Microsoft.EntityFrameworkCore;
using prod.Authorization;
using prod.Dto;
using prod.Master.Cmm;
using prod.Master.Common.Dto;
using prod.Master.Common.Exporting;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace prod.Master.Common
{
    [AbpAuthorize(AppPermissions.Pages_Master_Cmm_ProductGroup_View)]
    public class MstCmmProductGroupAppService : prodAppServiceBase, IMstCmmProductGroupAppService
    {
        private readonly IDapperRepository<MstCmmProductGroup, long> _dapperRepo;
        private readonly IRepository<MstCmmProductGroup, long> _repo;
        private readonly IMstCmmProductGroupExcelExporter _calendarListExcelExporter;

        public MstCmmProductGroupAppService(IRepository<MstCmmProductGroup, long> repo,
                                         IDapperRepository<MstCmmProductGroup, long> dapperRepo,
                                        IMstCmmProductGroupExcelExporter calendarListExcelExporter
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
        }

        //[AbpAuthorize(AppPermissions.Pages_InvtSetup_ProductGroup_Edit)]
        //public async Task CreateOrEdit(CreateOrEditMstCmmProductGroupDto input)
        //{
        //	if (input.Id == null) await Create(input);
        //	else await Update(input);
        //}

        ////CREATE
        //private async Task Create(CreateOrEditMstCmmProductGroupDto input)
        //{
        //	var mainObj = ObjectMapper.Map<MstCmmProductGroup>(input);

        //	await CurrentUnitOfWork.GetDbContext<prodDbContext>().AddAsync(mainObj);
        //}

        //// EDIT
        //private async Task Update(CreateOrEditMstCmmProductGroupDto input)
        //{
        //	using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant))
        //	{
        //		var mainObj = await _repo.GetAll()
        //		.FirstOrDefaultAsync(e => e.Id == input.Id);

        //		var mainObjToUpdate = ObjectMapper.Map(input, mainObj);
        //	}
        //}

        //[AbpAuthorize(AppPermissions.Pages_InvtSetup_ProductGroup_Delete)]
        //public async Task Delete(EntityDto input)
        //{
        //          var mainObj = await _repo.FirstOrDefaultAsync(input.Id);
        //          _repo.HardDelete(mainObj);
        //          CurrentUnitOfWork.GetDbContext<prodDbContext>().Remove(mainObj);
        //      }

        public async Task<PagedResultDto<MstCmmProductGroupDto>> GetAll(GetMstCmmProductGroupInput input)
        {
            var filtered = _repo.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.Code), e => e.Code.Contains(input.Code))
                .WhereIf(!string.IsNullOrWhiteSpace(input.Description), e => e.Description.Contains(input.Description))
                ;
            var pageAndFiltered = filtered.OrderBy(s => s.Id);


            var system = from o in pageAndFiltered
                         select new MstCmmProductGroupDto
                         {
                             Id = o.Id,
                             Code = o.Code,
                             Description = o.Description,
                         };

            var totalCount = await filtered.CountAsync();
            var paged = system.PageBy(input);

            return new PagedResultDto<MstCmmProductGroupDto>(
                totalCount,
                 await paged.ToListAsync()
            );
        }

        public async Task<FileDto> GetProductGroupToExcel(MstCmmProductGroupExportInput input)
        {
            var filtered = _repo.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.Code), e => e.Code.Contains(input.Code))
                .WhereIf(!string.IsNullOrWhiteSpace(input.Description), e => e.Description.Contains(input.Description));

            var pageAndFiltered = filtered.OrderBy(s => s.Id);
            var system = from o in pageAndFiltered
                         select new MstCmmProductGroupDto
                         {
                             Id = o.Id,
                             Code = o.Code,
                             Description = o.Description,
                         };
            var exportToExcel = await system.ToListAsync();
            return _calendarListExcelExporter.ExportToFile(exportToExcel);
        }

        // public async Task GenerateAsync()
        //  {
        //    await _dapperRepo.ExecuteAsync(MstCmmProductGroupConsts.SP_MST_WPT_CALENDAR_GENERATE);
        // }

    }
}

