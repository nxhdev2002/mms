using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Dapper.Repositories;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.EntityFrameworkCore.Uow;
using Abp.Linq.Extensions;
using Microsoft.EntityFrameworkCore;
using prod.Authorization;
using prod.Dto;
using prod.EntityFrameworkCore;
using prod.Master.LogA.Dto;
using prod.Master.LogA.Exporting;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace prod.Master.LogA
{
    //  [AbpAuthorize(AppPermissions.Pages_Master_LogA_EkbUser)]
    public class MstLgaEkbUserAppService : prodAppServiceBase, IMstLgaEkbUserAppService
    {
        private readonly IDapperRepository<MstLgaEkbUser, long> _dapperRepo;
        private readonly IRepository<MstLgaEkbUser, long> _repo;
        private readonly IMstLgaEkbUserExcelExporter _calendarListExcelExporter;

        public MstLgaEkbUserAppService(IRepository<MstLgaEkbUser, long> repo,
                                         IDapperRepository<MstLgaEkbUser, long> dapperRepo,
                                        IMstLgaEkbUserExcelExporter calendarListExcelExporter
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
        }

        //  [AbpAuthorize(AppPermissions.Pages_Master_LogA_EkbUser_Edit)]
        public async Task CreateOrEdit(CreateOrEditMstLgaEkbUserDto input)
        {
            if (input.Id == null) await Create(input);
            else await Update(input);
        }

        //CREATE
        private async Task Create(CreateOrEditMstLgaEkbUserDto input)
        {
            var mainObj = ObjectMapper.Map<MstLgaEkbUser>(input);

            await CurrentUnitOfWork.GetDbContext<prodDbContext>().AddAsync(mainObj);
        }

        // EDIT
        private async Task Update(CreateOrEditMstLgaEkbUserDto input)
        {
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                var mainObj = await _repo.GetAll()
                .FirstOrDefaultAsync(e => e.Id == input.Id);

                var mainObjToUpdate = ObjectMapper.Map(input, mainObj);
            }
        }

        //  [AbpAuthorize(AppPermissions.Pages_Master_LogA_EkbUser_Delete)]
        public async Task Delete(EntityDto input)
        {
            var mainObj = await _repo.FirstOrDefaultAsync(input.Id);
            _repo.HardDelete(mainObj);
            CurrentUnitOfWork.GetDbContext<prodDbContext>().Remove(mainObj);
        }

        public async Task<PagedResultDto<MstLgaEkbUserDto>> GetAll(GetMstLgaEkbUserInput input)
        {
            var filtered = _repo.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.UserCode), e => e.UserCode.Contains(input.UserCode))
                .WhereIf(!string.IsNullOrWhiteSpace(input.UserName), e => e.UserName.Contains(input.UserName))
                .WhereIf(!string.IsNullOrWhiteSpace(input.ProcessCode), e => e.ProcessCode.Contains(input.ProcessCode))
                .WhereIf(!string.IsNullOrWhiteSpace(input.ProcessGroup), e => e.ProcessGroup.Contains(input.ProcessGroup))
                .WhereIf(!string.IsNullOrWhiteSpace(input.ProcessSubgroup), e => e.ProcessSubgroup.Contains(input.ProcessSubgroup))
                .WhereIf(!string.IsNullOrWhiteSpace(input.ProdLine), e => e.ProdLine.Contains(input.ProdLine))
                .WhereIf(!string.IsNullOrWhiteSpace(input.UserType), e => e.UserType.Contains(input.UserType))
                .WhereIf(!string.IsNullOrWhiteSpace(input.IsActive), e => e.IsActive.Contains(input.IsActive))
                ;
            var pageAndFiltered = filtered.OrderBy(s => s.Id);


            var system = from o in pageAndFiltered
                         select new MstLgaEkbUserDto
                         {
                             Id = o.Id,
                             UserCode = o.UserCode,
                             UserName = o.UserName,
                             ProcessId = o.ProcessId,
                             ProcessCode = o.ProcessCode,
                             ProcessGroup = o.ProcessGroup,
                             ProcessSubgroup = o.ProcessSubgroup,
                             ProdLine = o.ProdLine,
                             UserType = o.UserType,
                             LeadTime = o.LeadTime,
                             Sortingg = o.Sortingg,
                             IsActive = o.IsActive,
                         };

            var totalCount = await filtered.CountAsync();
            var paged = system.PageBy(input);

            return new PagedResultDto<MstLgaEkbUserDto>(
                totalCount,
                 await paged.ToListAsync()
            );
        }


        public async Task<FileDto> GetEkbUserToExcel(MstLgaEkbUserExportInput input)
        {
            var query = from o in _repo.GetAll()
                        select new MstLgaEkbUserDto
                        {
                            Id = o.Id,
                            UserCode = o.UserCode,
                            UserName = o.UserName,
                            ProcessId = o.ProcessId,
                            ProcessCode = o.ProcessCode,
                            ProcessGroup = o.ProcessGroup,
                            ProcessSubgroup = o.ProcessSubgroup,
                            ProdLine = o.ProdLine,
                            UserType = o.UserType,
                            LeadTime = o.LeadTime,
                            Sortingg = o.Sortingg,
                            IsActive = o.IsActive,
                        };
            var exportToExcel = await query.ToListAsync();
            return _calendarListExcelExporter.ExportToFile(exportToExcel);
        }

        // public async Task GenerateAsync()
        //  {
        //    await _dapperRepo.ExecuteAsync(MstLgaEkbUserConsts.SP_MST_WPT_CALENDAR_GENERATE);
        // }

    }
}
