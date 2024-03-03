using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Dapper.Repositories;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.EntityFrameworkCore.Uow;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using AutoMapper.Internal.Mappers;
using Dapper;
using Microsoft.EntityFrameworkCore;
using NUglify.Helpers;
using prod.Authorization;
using prod.Dto;
using prod.Master.Inv.Dto;
using prod.Master.Inv.Exporting;
using prod.Master.Inv;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using prod;
using prod.EntityFrameworkCore;
    
namespace prod.Master.Inv
{
    [AbpAuthorize(AppPermissions.Pages_Master_Gps_GpsTmvPic_View)]
    public class MstInvGpsTmvPicAppService : prodAppServiceBase, IMstInvGpsTmvPicAppService
    {
        private readonly IDapperRepository<MstInvGpsTmvPic, long> _dapperRepo;
        private readonly IRepository<MstInvGpsTmvPic, long> _repo;
        private readonly IMstInvGpsTmvPicExcelExporter _calendarListExcelExporter;

        public MstInvGpsTmvPicAppService(IRepository<MstInvGpsTmvPic, long> repo,
                                         IDapperRepository<MstInvGpsTmvPic, long> dapperRepo,
                                        IMstInvGpsTmvPicExcelExporter calendarListExcelExporter
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
        }

        [AbpAuthorize(AppPermissions.Pages_Master_Gps_GpsTmvPic_Edit)]
        public async Task CreateOrEdit(CreateOrEditMstInvGpsTmvPicDto input)
        {
            if (input.Id == null) await Create(input);
            else await Update(input);
        }

        //CREATE
        private async Task Create(CreateOrEditMstInvGpsTmvPicDto input)
        {
            var mainObj = ObjectMapper.Map<MstInvGpsTmvPic>(input);

            await CurrentUnitOfWork.GetDbContext<prodDbContext>().AddAsync(mainObj);
        }

        // EDIT
        private async Task Update(CreateOrEditMstInvGpsTmvPicDto input)
        {
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                var mainObj = await _repo.GetAll()
                .FirstOrDefaultAsync(e => e.Id == input.Id);

                var mainObjToUpdate = ObjectMapper.Map(input, mainObj);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_Master_Gps_GpsTmvPic_Edit)]
        public async Task Delete(EntityDto input)
        {
            var mainObj = await _repo.FirstOrDefaultAsync(input.Id);
            _repo.HardDelete(mainObj);
            CurrentUnitOfWork.GetDbContext<prodDbContext>().Remove(mainObj);
        }

        public async Task<PagedResultDto<MstInvGpsTmvPicDto>> GetAll(GetMstInvGpsTmvPicInput input)
        {
            var filtered = _repo.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.PicUserAccount), e => e.PicUserAccount.Contains(input.PicUserAccount))
                .WhereIf(!string.IsNullOrWhiteSpace(input.PicName), e => e.PicName.Contains(input.PicName))
                .WhereIf(!string.IsNullOrWhiteSpace(input.PicTelephone), e => e.PicTelephone.Contains(input.PicTelephone))
                .WhereIf(!string.IsNullOrWhiteSpace(input.PicEmail), e => e.PicEmail.Contains(input.PicEmail))
                .WhereIf(!string.IsNullOrWhiteSpace(input.IsMainPic), e => e.IsMainPic.Contains(input.IsMainPic))
                .WhereIf(!string.IsNullOrWhiteSpace(input.PicTelephone2), e => e.PicTelephone2.Contains(input.PicTelephone2))
                .WhereIf(!string.IsNullOrWhiteSpace(input.Suppliers), e => e.Suppliers.Contains(input.Suppliers))
                .WhereIf(!string.IsNullOrWhiteSpace(input.IsActive), e => e.IsActive.Contains(input.IsActive))
                ;
            var pageAndFiltered = filtered.OrderBy(s => s.Id);


            var system = from o in pageAndFiltered
                         select new MstInvGpsTmvPicDto
                         {
                             Id = o.Id,
                             PicUserAccount = o.PicUserAccount,
                             PicName = o.PicName,
                             PicTelephone = o.PicTelephone,
                             PicEmail = o.PicEmail,
                             IsMainPic = o.IsMainPic,
                             PicTelephone2 = o.PicTelephone2,
                             Suppliers = o.Suppliers,
                             IsActive = o.IsActive,
                         };

            var totalCount = await filtered.CountAsync();
            var paged = system.PageBy(input);

            return new PagedResultDto<MstInvGpsTmvPicDto>(
                totalCount,
                 await paged.ToListAsync()
            );
        }


        public async Task<FileDto> GetGpsTmvPicToExcel(MstInvGpsTmvPicExportInput input)
        {
            var query = from o in _repo.GetAll()
                        select new MstInvGpsTmvPicDto
                        {
                            Id = o.Id,
                            PicUserAccount = o.PicUserAccount,
                            PicName = o.PicName,
                            PicTelephone = o.PicTelephone,
                            PicEmail = o.PicEmail,
                            IsMainPic = o.IsMainPic,
                            PicTelephone2 = o.PicTelephone2,
                            Suppliers = o.Suppliers,
                            IsActive = o.IsActive,
                        };
            var exportToExcel = await query.ToListAsync();
            return _calendarListExcelExporter.ExportToFile(exportToExcel);
        }

        // public async Task GenerateAsync()
        //  {
        //    await _dapperRepo.ExecuteAsync(MstInvGpsTmvPicConsts.SP_MST_WPT_CALENDAR_GENERATE);
        // }

    }
}