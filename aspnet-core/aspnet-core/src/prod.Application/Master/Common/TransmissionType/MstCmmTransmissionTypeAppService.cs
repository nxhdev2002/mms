using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Dapper.Repositories;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.EntityFrameworkCore.Uow;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using Dapper;
using Microsoft.EntityFrameworkCore;
using NUglify.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using prod.Authorization;
using prod.Dto;
using prod.EntityFrameworkCore;
using prod.Master.Common.Dto;
using prod.Master.Common.Exporting;

namespace prod.Master.Common
{
    [AbpAuthorize(AppPermissions.Pages_InvtSetup_TransmissionType_View)]
    public class MstCmmTransmissionTypeAppService : prodAppServiceBase, IMstCmmTransmissionTypeAppService
    {

        private readonly IRepository<MstCmmTransmissionType, long> _repo;
        private readonly IMstCmmTransmissionTypeExcelExporter _transmissiontypeListExcelExporter;

        public MstCmmTransmissionTypeAppService(IRepository<MstCmmTransmissionType, long> repo,
                                        IMstCmmTransmissionTypeExcelExporter transmissiontypeListExcelExporter
            )
        {
            _repo = repo;
            _transmissiontypeListExcelExporter = transmissiontypeListExcelExporter;
        }

        //[AbpAuthorize(AppPermissions.Pages_InvtSetup_TransmissionType_Edit)]
        //public async Task CreateOrEdit(CreateOrEditMstCmmTransmissionTypeDto input)
        //{
        //    if (input.Id == null) await Create(input);
        //    else await Update(input);
        //}

        ////CREATE
        //private async Task Create(CreateOrEditMstCmmTransmissionTypeDto input)
        //{
        //    var mainObj = ObjectMapper.Map<MstCmmTransmissionType>(input);

        //    await CurrentUnitOfWork.GetDbContext<prodDbContext>().AddAsync(mainObj);
        //}

        //// EDIT
        //private async Task Update(CreateOrEditMstCmmTransmissionTypeDto input)
        //{
        //    using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant))
        //    {
        //        var mainObj = await _repo.GetAll()
        //        .FirstOrDefaultAsync(e => e.Id == input.Id);

        //        var mainObjToUpdate = ObjectMapper.Map(input, mainObj);
        //    }
        //}

        //[AbpAuthorize(AppPermissions.Pages_InvtSetup_TransmissionType_Delete)]
        //public async Task Delete(EntityDto input)
        //{
        //    var mainObj = await _repo.FirstOrDefaultAsync(input.Id);
        //    _repo.HardDelete(mainObj);
        //    CurrentUnitOfWork.GetDbContext<prodDbContext>().Remove(mainObj);
        //}

        public async Task<PagedResultDto<MstCmmTransmissionTypeDto>> GetAll(GetMstCmmTransmissionTypeInput input)
        {
            var filtered = _repo.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.Code), e => e.Code.Contains(input.Code))
                .WhereIf(!string.IsNullOrWhiteSpace(input.Name), e => e.Name.Contains(input.Name))
              
                ;
            var pageAndFiltered = filtered.OrderBy(s => s.Id);


            var system = from o in pageAndFiltered
                         select new MstCmmTransmissionTypeDto
                         {
                             Id = o.Id,
                             Code = o.Code,
                             Name = o.Name,
                             IsActive = o.IsActive,
                         };

            var totalCount = await filtered.CountAsync();
            var paged = system.PageBy(input);

            return new PagedResultDto<MstCmmTransmissionTypeDto>(
                totalCount,
                 await paged.ToListAsync()
            );
        }


        public async Task<FileDto> GetTransmissionTypeToExcel(MstCmmTransmissionTypeExportInput input)
        {
            var filtered = _repo.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.Code), e => e.Code.Contains(input.Code))
                .WhereIf(!string.IsNullOrWhiteSpace(input.Name), e => e.Name.Contains(input.Name))
                ;
            var pageAndFiltered = filtered.OrderBy(s => s.Id);


            var query = from o in pageAndFiltered
                         select new MstCmmTransmissionTypeDto
                        {
                            Id = o.Id,
                            Code = o.Code,
                            Name = o.Name,
                            IsActive = o.IsActive,
                        };
            var exportToExcel = await query.ToListAsync();
            return _transmissiontypeListExcelExporter.ExportToFile(exportToExcel);
        }


    }
}
