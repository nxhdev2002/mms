using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Dapper.Repositories;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.EntityFrameworkCore.Uow;
using Abp.Linq.Extensions;
using Abp.UI;
using Microsoft.EntityFrameworkCore;
using prod.Authorization;
using prod.Dto;
using prod.EntityFrameworkCore;
using prod.Master.Common.Dto;
using prod.Master.WorkingPattern.Dto;
using prod.Master.WorkingPattern.Exporting;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace prod.Master.WorkingPattern
{
    [AbpAuthorize(AppPermissions.Pages_WorkingPattern_PatternH_View)]
    public class MstWptPatternHAppService : prodAppServiceBase, IMstWptPatternHAppService
    {
        private readonly IDapperRepository<MstWptPatternH, long> _dapperRepo;
        private readonly IRepository<MstWptPatternH, long> _repo;
        private readonly IMstWptPatternHExcelExporter _calendarListExcelExporter;

        public MstWptPatternHAppService(IRepository<MstWptPatternH, long> repo,
                                         IDapperRepository<MstWptPatternH, long> dapperRepo,
                                        IMstWptPatternHExcelExporter calendarListExcelExporter
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
        }

        [AbpAuthorize(AppPermissions.Pages_WorkingPattern_PatternH_Edit)]
        public async Task CreateOrEdit(CreateOrEditMstWptPatternHDto input)
        {
            if (input.Id == null) await Create(input);
            else await Update(input);
        }

        //CREATE
        private async Task Create(CreateOrEditMstWptPatternHDto input)
        {
            if (input.StartDate <= input.EndDate)
            {
                var mainObj = ObjectMapper.Map<MstWptPatternH>(input);

                await CurrentUnitOfWork.GetDbContext<prodDbContext>().AddAsync(mainObj);
            }
            else
            {
                throw new UserFriendlyException(400, L("End Date phải lớn hơn Start Date"));
            }

        }

        // EDIT
        private async Task Update(CreateOrEditMstWptPatternHDto input)
        {
            if (input.StartDate <= input.EndDate)
            {
                using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant))
                {
                    var mainObj = await _repo.GetAll()
                    .FirstOrDefaultAsync(e => e.Id == input.Id);

                    var mainObjToUpdate = ObjectMapper.Map(input, mainObj);
                }
            }
            else
            {
                throw new UserFriendlyException(400, L("End Date phải lớn hơn Start Date"));
            }

        }

        [AbpAuthorize(AppPermissions.Pages_WorkingPattern_PatternH_Edit)]
        public async Task Delete(EntityDto input)
        {
            var mainObj = await _repo.FirstOrDefaultAsync(input.Id);
            _repo.HardDelete(mainObj);
            CurrentUnitOfWork.GetDbContext<prodDbContext>().Remove(mainObj);
        }

        public async Task<PagedResultDto<MstWptPatternHDto>> GetAll(GetMstWptPatternHInput input)
        {
            DateTime dateTime = DateTime.UtcNow.Date;

            var filtered = _repo.GetAll()
                 .WhereIf(input.Type != 0, t => t.Type == input.Type)
                //.WhereIf(!input.StartDate.HasValue, t => t.StartDate <= dateTime)
                .WhereIf(input.StartDate.HasValue, t => t.StartDate <= input.StartDate.Value.Date)
                .WhereIf(!string.IsNullOrWhiteSpace(input.IsActive), e => e.IsActive.Contains(input.IsActive));

            var pageAndFiltered = filtered.OrderBy(s => s.StartDate);

            var system = from o in pageAndFiltered
                         select new MstWptPatternHDto
                         {
                             Id = o.Id,
                             Type = o.Type,
                             StartDate = o.StartDate,
                             EndDate = o.EndDate,
                             Description = o.Description,
                             IsActive = o.IsActive,
                         };

            var totalCount = await filtered.CountAsync();
            var paged = system.PageBy(input);

            return new PagedResultDto<MstWptPatternHDto>(
                totalCount,
                 await paged.ToListAsync()
            );
        }


        public async Task<FileDto> GetPatternHToExcel(MstWptPatternHExportInput input)
        {
            DateTime dateTime = DateTime.UtcNow.Date;

            var filtered = _repo.GetAll()
                 .WhereIf(input.Type != 0, t => t.Type == input.Type)
                //.WhereIf(!input.StartDate.HasValue, t => t.StartDate <= dateTime)
                .WhereIf(input.StartDate.HasValue, t => t.StartDate <= input.StartDate.Value.Date)
                .WhereIf(!string.IsNullOrWhiteSpace(input.IsActive), e => e.IsActive.Contains(input.IsActive));

            var pageAndFiltered = filtered.OrderBy(s => s.StartDate);

            var query = from o in pageAndFiltered
                        select new MstWptPatternHDto
                        {
                            Id = o.Id,
                            Type = o.Type,
                            StartDate = o.StartDate,
                            EndDate = o.EndDate,
                            Description = o.Description,
                            IsActive = o.IsActive,
                        };
            var exportToExcel = await query.ToListAsync();
            return _calendarListExcelExporter.ExportToFile(exportToExcel);
        }


        //
        public async Task<PagedResultDto<GetMstCmmLookupByDomainCodeDto>> GetsByDomainCode(GetMstCmmLookupByDomainCodeInput input)
        {
            string _sql = "Exec  MST_CMM_LOOKUP_GET_BY_DOMAINCODE @DomainCode";

            var filtered = await _dapperRepo.QueryAsync<GetMstCmmLookupByDomainCodeDto>(_sql, new
            {
                DomainCode = input.DomainCode,
            });

            var pageAndFiltered = filtered.OrderBy(s => s.RowNo);


            var results = from d in pageAndFiltered
                          select new GetMstCmmLookupByDomainCodeDto
                          {
                              DomainCode = d.DomainCode,
                              ItemCode = d.ItemCode,
                              ItemValue = d.ItemValue,
                              RowNo = d.RowNo,
                          };

            var totalCount = filtered.ToList().Count;
            var paged = results.AsQueryable().PageBy(input);

            return new PagedResultDto<GetMstCmmLookupByDomainCodeDto>(
                totalCount,
                paged.ToList()
            );
        }
        // public async Task GenerateAsync()
        //  {
        //    await _dapperRepo.ExecuteAsync(MstWptPatternHConsts.SP_MST_WPT_CALENDAR_GENERATE);
        // }

    }
}
