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
using prod.Master.Painting.BmpPartList.ImportDto;
using prod.Master.Painting.Dto;
using prod.Master.Painting.Exporting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace prod.Master.Painting
{
    //  [AbpAuthorize(AppPermissions.Pages_Master_Painting_BmpPartList)]
    public class MstPtsBmpPartListAppService : prodAppServiceBase, IMstPtsBmpPartListAppService
    {
        private readonly IDapperRepository<MstPtsBmpPartList, long> _dapperRepo;
        private readonly IRepository<MstPtsBmpPartList, long> _repo;
        private readonly IRepository<MstPtsBmpPartType, long> _repoPartType;
        private readonly IMstPtsBmpPartListExcelExporter _calendarListExcelExporter;

        public MstPtsBmpPartListAppService(IRepository<MstPtsBmpPartList, long> repo,
                                            IRepository<MstPtsBmpPartType, long> repoPartType,
                                         IDapperRepository<MstPtsBmpPartList, long> dapperRepo,
                                        IMstPtsBmpPartListExcelExporter calendarListExcelExporter
            )
        {
            _repo = repo;
            _repoPartType = repoPartType;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
        }

        //  [AbpAuthorize(AppPermissions.Pages_Master_Painting_BmpPartList_Edit)]
        public async Task CreateOrEdit(CreateOrEditMstPtsBmpPartListDto input)
        {
            if (input.Id == null) await Create(input);
            else await Update(input);
        }

        //CREATE
        private async Task Create(CreateOrEditMstPtsBmpPartListDto input)
        {
            var mainObj = ObjectMapper.Map<MstPtsBmpPartList>(input);

            await CurrentUnitOfWork.GetDbContext<prodDbContext>().AddAsync(mainObj);
        }

        // EDIT
        private async Task Update(CreateOrEditMstPtsBmpPartListDto input)
        {
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                var mainObj = await _repo.GetAll()
                .FirstOrDefaultAsync(e => e.Id == input.Id);

                var mainObjToUpdate = ObjectMapper.Map(input, mainObj);
            }
        }

        //  [AbpAuthorize(AppPermissions.Pages_Master_Painting_BmpPartList_Delete)]
        public async Task Delete(EntityDto input)
        {
            var mainObj = await _repo.FirstOrDefaultAsync(input.Id);
            _repo.HardDelete(mainObj);
            CurrentUnitOfWork.GetDbContext<prodDbContext>().Remove(mainObj);
        }

        public async Task<PagedResultDto<MstPtsBmpPartListDto>> GetAll(GetMstPtsBmpPartListInput input)
        {
            var partList = _repo.GetAll().AsNoTracking()
              .WhereIf(!string.IsNullOrWhiteSpace(input.Model), e => e.Model.Contains(input.Model))
             .WhereIf(!string.IsNullOrWhiteSpace(input.Cfc), e => e.Cfc.Contains(input.Cfc))
             .WhereIf(!string.IsNullOrWhiteSpace(input.Grade), e => e.Grade.Contains(input.Grade))
             .WhereIf(!string.IsNullOrWhiteSpace(input.ProdLine), e => e.ProdLine.Contains(input.ProdLine))
             .WhereIf(!string.IsNullOrWhiteSpace(input.PartTypeCode), e => e.PartTypeCode.Contains(input.PartTypeCode));

            var partType = _repoPartType.GetAll().AsNoTracking()
               .WhereIf(!string.IsNullOrWhiteSpace(input.IsBumper), e => e.IsBumper.Contains(input.IsBumper));


            var filtered = from pl in partList
                           join pt in partType
                           on pl.PartTypeId equals pt.Id
                           orderby pl.Id
                        select new MstPtsBmpPartListDto
                        {
                            Id = pl.Id,
                            Model = pl.Model,
                            Cfc = pl.Cfc,
                            Grade = pl.Grade,
                            BackNo = pl.BackNo,
                            ProdLine = pl.ProdLine,
                            PartTypeCode = pl.PartTypeCode,
                            PartTypeId = pl.PartTypeId,
                            Process = pl.Process,
                            PkProcess = pl.PkProcess,
                            IsPunch = pl.IsPunch,
                            SpecialColor = pl.SpecialColor,
                            SignalId = pl.SignalId,
                            SignalCode = pl.SignalCode,
                            Remark = pl.Remark,
                            IsActive = pl.IsActive,
                        };

            var totalCount = await filtered.CountAsync();
            var paged = filtered.PageBy(input);

            return new PagedResultDto<MstPtsBmpPartListDto>(
                totalCount,
                 await paged.ToListAsync()
            );
        }


        public async Task<FileDto> GetBmpPartListToExcel(GetMstPtsBmpPartListExcelInput input)
        {
            var partList = _repo.GetAll()
            .WhereIf(!string.IsNullOrWhiteSpace(input.Model), e => e.Model.Contains(input.Model))
           .WhereIf(!string.IsNullOrWhiteSpace(input.Cfc), e => e.Cfc.Contains(input.Cfc))
           .WhereIf(!string.IsNullOrWhiteSpace(input.Grade), e => e.Grade.Contains(input.Grade))
           .WhereIf(!string.IsNullOrWhiteSpace(input.ProdLine), e => e.ProdLine.Contains(input.ProdLine))
           .WhereIf(!string.IsNullOrWhiteSpace(input.PartTypeCode), e => e.PartTypeCode.Contains(input.PartTypeCode));

            var partType = _repoPartType.GetAll()
               .WhereIf(!string.IsNullOrWhiteSpace(input.IsBumper), e => e.IsBumper.Contains(input.IsBumper));


            var filtered = from pl in partList
                           join pt in partType
                           on pl.PartTypeId equals pt.Id
                           orderby pl.Id
                           select new MstPtsBmpPartListDto
                           {
                               Id = pl.Id,
                               Model = pl.Model,
                               Cfc = pl.Cfc,
                               Grade = pl.Grade,
                               BackNo = pl.BackNo,
                               ProdLine = pl.ProdLine,
                               PartTypeCode = pl.PartTypeCode,
                               PartTypeId = pl.PartTypeId,
                               Process = pl.Process,
                               PkProcess = pl.PkProcess,
                               IsPunch = pl.IsPunch,
                               SpecialColor = pl.SpecialColor,
                               SignalId = pl.SignalId,
                               SignalCode = pl.SignalCode,
                               Remark = pl.Remark,
                               IsActive = pl.IsActive,
                           };

            var exportToExcel = await filtered.ToListAsync();
            return _calendarListExcelExporter.ExportToFile(exportToExcel);
        }

        public async Task<List<ImportMstPtsBmpPartListDto>> ImportBmpPartListFromExcel(List<ImportMstPtsBmpPartListDto> bmppartlists)
        {
            try
            {
                List<MstPtsBmpPartList_T> MstPtsBmpPartList = new List<MstPtsBmpPartList_T> { };
                foreach (var item in bmppartlists)
                {
                    MstPtsBmpPartList_T importData = new MstPtsBmpPartList_T();
                    {
                        importData.Guid = item.Guid;
                        importData.Model = item.Model;
                        importData.Cfc = item.Cfc;
                        importData.Grade = item.Grade;
                        importData.BackNo = item.BackNo;
                        importData.ProdLine = item.ProdLine;
                        importData.PartTypeCode = item.PartTypeCode;
                        importData.PartTypeId = 0;
                        importData.Process = item.Process;
                        importData.PkProcess = item.PkProcess;
                        importData.IsPunch = item.IsPunch;
                        importData.SpecialColor = item.SpecialColor;
                        importData.SignalId = 0;
                        importData.SignalCode = item.SignalCode;
                        importData.Remark = item.Remark;
                        importData.IsActive = item.IsActive;
                    }
                    MstPtsBmpPartList.Add(importData);
                }
                CurrentUnitOfWork.GetDbContext<prodDbContext>().AddRangeAsync(MstPtsBmpPartList);
                return null;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(400, ex.Message);
            }
        }

        //Merge Data From Temp To bmppartlist
        public async Task MergeDataBmpPartList(string v_Guid)
        {
            string _sql = "Exec MST_PTS_BMP_PARTLIST_MERGE  @Guid";
            await _dapperRepo.QueryAsync<MstPtsBmpPartList>(_sql, new { Guid = v_Guid });
        }
    }
}
