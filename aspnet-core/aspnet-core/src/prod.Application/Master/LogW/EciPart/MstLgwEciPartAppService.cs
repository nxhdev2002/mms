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
using prod.Master.LogW.Dto;
using prod.Master.LogW.EciPart.ImportDto;
using prod.Master.LogW.Exporting;

namespace prod.Master.LogW.EciPart
{
    //  [AbpAuthorize(AppPermissions.Pages_Master_LogW_EciPart)]
    public class MstLgwEciPartAppService : prodAppServiceBase, IMstLgwEciPartAppService
    {
        private readonly IDapperRepository<MstLgwEciPart, long> _dapperRepo;
        private readonly IRepository<MstLgwEciPart, long> _repo;
        private readonly IMstLgwEciPartExcelExporter _calendarListExcelExporter;

        public MstLgwEciPartAppService(IRepository<MstLgwEciPart, long> repo,
                                         IDapperRepository<MstLgwEciPart, long> dapperRepo,
                                        IMstLgwEciPartExcelExporter calendarListExcelExporter
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
        }

        //  [AbpAuthorize(AppPermissions.Pages_Master_LogW_EciPart_Edit)]
        public async Task CreateOrEdit(CreateOrEditMstLgwEciPartDto input)
        {
            if (input.Id == null) await Create(input);
            else await Update(input);
        }

        //CREATE
        private async Task Create(CreateOrEditMstLgwEciPartDto input)
        {
            var mainObj = ObjectMapper.Map<MstLgwEciPart>(input);

            await CurrentUnitOfWork.GetDbContext<prodDbContext>().AddAsync(mainObj);
        }

        // EDIT
        private async Task Update(CreateOrEditMstLgwEciPartDto input)
        {
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                var mainObj = await _repo.GetAll()
                .FirstOrDefaultAsync(e => e.Id == input.Id);

                var mainObjToUpdate = ObjectMapper.Map(input, mainObj);
            }
        }

        //  [AbpAuthorize(AppPermissions.Pages_Master_LogW_EciPart_Delete)]
        public async Task Delete(EntityDto input)
        {
            var mainObj = await _repo.FirstOrDefaultAsync(input.Id);
            _repo.HardDelete(mainObj);
            CurrentUnitOfWork.GetDbContext<prodDbContext>().Remove(mainObj);
        }

        public async Task<PagedResultDto<MstLgwEciPartDto>> GetAll(GetMstLgwEciPartInput input)
        {
            var filtered = _repo.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.ModuleNo), e => e.ModuleNo.Contains(input.ModuleNo))
                .WhereIf(!string.IsNullOrWhiteSpace(input.PartNo), e => e.PartNo.Contains(input.PartNo))
                .WhereIf(!string.IsNullOrWhiteSpace(input.SupplierNo), e => e.SupplierNo.Contains(input.SupplierNo))
                .WhereIf(!string.IsNullOrWhiteSpace(input.ModuleNoEci), e => e.ModuleNoEci.Contains(input.ModuleNoEci))
                .WhereIf(!string.IsNullOrWhiteSpace(input.PartNoEci), e => e.PartNoEci.Contains(input.PartNoEci))
                ;
            var pageAndFiltered = filtered.OrderBy(s => s.Id);


            var system = from o in pageAndFiltered
                         select new MstLgwEciPartDto
                         {
                             Id = o.Id,
                             ModuleNo = o.ModuleNo,
                             PartNo = o.PartNo,
                             SupplierNo = o.SupplierNo,
                             ModuleNoEci = o.ModuleNoEci,
                             PartNoEci = o.PartNoEci,
                             SupplierNoEci = o.SupplierNoEci,
                             StartEciSeq = o.StartEciSeq,
                             StartEciRenban = o.StartEciRenban,
                             StartEciModule = o.StartEciModule,
                             IsActive = o.IsActive,
                         };

            var totalCount = await filtered.CountAsync();
            var paged = system.PageBy(input);

            return new PagedResultDto<MstLgwEciPartDto>(
                totalCount,
                 await paged.ToListAsync()
            );
        }


        public async Task<FileDto> GetEciPartToExcel(MstLgwEciPartExportInput input)
        {
            var filtered = _repo.GetAll()
               .WhereIf(!string.IsNullOrWhiteSpace(input.ModuleNo), e => e.ModuleNo.Contains(input.ModuleNo))
               .WhereIf(!string.IsNullOrWhiteSpace(input.PartNo), e => e.PartNo.Contains(input.PartNo))
               .WhereIf(!string.IsNullOrWhiteSpace(input.SupplierNo), e => e.SupplierNo.Contains(input.SupplierNo))
               .WhereIf(!string.IsNullOrWhiteSpace(input.ModuleNoEci), e => e.ModuleNoEci.Contains(input.ModuleNoEci))
               .WhereIf(!string.IsNullOrWhiteSpace(input.PartNoEci), e => e.PartNoEci.Contains(input.PartNoEci))
               ;
            var pageAndFiltered = filtered.OrderBy(s => s.Id);


            var query = from o in pageAndFiltered
                         select new MstLgwEciPartDto
                        {
                            Id = o.Id,
                            ModuleNo = o.ModuleNo,
                            PartNo = o.PartNo,
                            SupplierNo = o.SupplierNo,
                            ModuleNoEci = o.ModuleNoEci,
                            PartNoEci = o.PartNoEci,
                            SupplierNoEci = o.SupplierNoEci,
                            StartEciSeq = o.StartEciSeq,
                            StartEciRenban = o.StartEciRenban,
                            StartEciModule = o.StartEciModule,
                            IsActive = o.IsActive,
                        };
            var exportToExcel = await query.ToListAsync();
            return _calendarListExcelExporter.ExportToFile(exportToExcel);
        }


        //Import Data From Excel
        public Task<List<ImportMstLgwEciPartDto>> ImportEciPartFromExcel(List<ImportMstLgwEciPartDto> eicParts)
        {
            try
            {
                List<MstLgwEciPart_T> eicPartList = new List<MstLgwEciPart_T> { };
                foreach (var item in eicParts)
                {
                    MstLgwEciPart_T importData = new MstLgwEciPart_T();
                    {
                        importData.Guid = item.Guid;
                        importData.ModuleNo = item.ModuleNo;
                        importData.PartNo = item.PartNo;
                        importData.SupplierNo = item.SupplierNo;
                        importData.ModuleNoEci = item.ModuleNoEci;
                        importData.PartNoEci = item.PartNoEci;
                        importData.SupplierNoEci = item.SupplierNoEci;
                        importData.StartEciSeq = item.StartEciSeq;
                        importData.StartEciRenban = item.StartEciRenban;
                    }
                    eicPartList.Add(importData);
                }
                CurrentUnitOfWork.GetDbContext<prodDbContext>().AddRangeAsync(eicPartList);
                return null;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(400, ex.Message);
            }
        }


        //Merge Data From Temp To EciPart
        public async Task MergeDataEciPart(string v_Guid)
        {
            string _sql = "Exec MST_LGW_ECI_PART_MERGE @Guid";
            await _dapperRepo.QueryAsync<MstLgwEciPart>(_sql, new { Guid = v_Guid });
        }
    }
}
