using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Linq.Extensions;
using Abp.Collections.Extensions;
using Abp.Dapper.Repositories;
using Abp.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using prod.Authorization;
using prod.Dto;
using prod.Master.Cmm;
using prod.Master.Common.DriveTrain.Dto;
using prod.Master.Common.DriveTrain.Exporting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using prod.Master.Common.Dto;

namespace prod.Master.Common.DriveTrain
{
    [AbpAuthorize(AppPermissions.Pages_InvtSetup_DriveTrain_View)]
    public class MstCmmDriveTrainAppService : prodAppServiceBase, IMstCmmDriveTrainAppservice
    {
        private readonly IDapperRepository<MstCmmDriveTrain, long> _dapperRepo;
        private readonly IRepository<MstCmmDriveTrain, long> _repo;
        private readonly IMstCmmDriveTrainExcelExporter _calendarListExcelExporter;

        public MstCmmDriveTrainAppService(IRepository<MstCmmDriveTrain, long> repo,
                                         IDapperRepository<MstCmmDriveTrain, long> dapperRepo,
                                       IMstCmmDriveTrainExcelExporter calendarListExcelExporter
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
        }



        //[AbpAuthorize(AppPermissions.Pages_InvtSetup_ProductType_Edit)]
        //public async Task CreateOrEdit(CreateOrEditMstCmmDriveTrainDto input)
        //{
        //	if (input.Id == null) await Create(input);
        //	else await Update(input);
        //}

        ////CREATE
        //private async Task Create(CreateOrEditMstCmmDriveTrainDto input)
        //{
        //	var mainObj = ObjectMapper.Map<MstCmmDriveTrain>(input);

        //	await CurrentUnitOfWork.GetDbContext<prodDbContext>().AddAsync(mainObj);
        //}

        //// EDIT
        //private async Task Update(CreateOrEditMstCmmDriveTrainDto input)
        //{
        //	using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant))
        //	{
        //		var mainObj = await _repo.GetAll()
        //		.FirstOrDefaultAsync(e => e.Id == input.Id);

        //		var mainObjToUpdate = ObjectMapper.Map(input, mainObj);
        //	}
        //}

        //[AbpAuthorize(AppPermissions.Pages_InvtSetup_ProductType_Delete)]
        //public async Task Delete(EntityDto input)
        //{
        //          var mainObj = await _repo.FirstOrDefaultAsync(input.Id);
        //          _repo.HardDelete(mainObj);
        //          CurrentUnitOfWork.GetDbContext<prodDbContext>().Remove(mainObj);
        //      }

        public async Task<PagedResultDto<MstCmmDriveTrainDto>> GetAll(GetMstCmmDriveTrainInput input)
        {
            var filtered = _repo.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.Code), e => e.Code.Contains(input.Code))
                .WhereIf(!string.IsNullOrWhiteSpace(input.Name), e => e.Name.Contains(input.Name));
            var pageAndFiltered = filtered.OrderBy(s => s.Id);


            var system = from o in pageAndFiltered
                         select new MstCmmDriveTrainDto
                         {
                             Id = o.Id,
                             Code = o.Code,
                             Name = o.Name,
                         };

            var totalCount = await filtered.CountAsync();
            var paged = system.PageBy(input);

            return new PagedResultDto<MstCmmDriveTrainDto>(
                totalCount,
                 await paged.ToListAsync()
            );
        }

        public async Task<FileDto> GetDriveTrainToExcel(MstCmmDriveTrainExportInput input)
        {
            var filtered = _repo.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Name), e => e.Name.Contains(input.Name))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Code), e => e.Code.Contains(input.Code));
            var pageAndFiltered = filtered.OrderBy(s => s.Id);
            var system = from o in pageAndFiltered
                         select new MstCmmDriveTrainDto
                         {
                             Id = o.Id,
                             Code = o.Code,
                             Name = o.Name,

                         };
            var exportToExcel = await system.ToListAsync();
            return _calendarListExcelExporter.ExportToFile(exportToExcel);
        }

        // public async Task GenerateAsync()
        //  {
        //    await _dapperRepo.ExecuteAsync(MstCmmDriveTrainConsts.SP_MST_WPT_CALENDAR_GENERATE);
        // }

    }
}
