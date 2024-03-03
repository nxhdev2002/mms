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
using prod.Assy.Andon.Dto;
using prod.Assy.Andon.Exporting;

namespace prod.Assy.Andon
{
    //  [AbpAuthorize(AppPermissions.Pages_Assy_Andon_TotalDelayFinalAsakai)]
    public class AsyAdoTotalDelayFinalAsakaiAppService : prodAppServiceBase, IAsyAdoTotalDelayFinalAsakaiAppService
    {
        private readonly IDapperRepository<AsyAdoTotalDelayFinalAsakai, long> _dapperRepo;
        private readonly IRepository<AsyAdoTotalDelayFinalAsakai, long> _repo;
        private readonly IAsyAdoTotalDelayFinalAsakaiExcelExporter _calendarListExcelExporter;

        public AsyAdoTotalDelayFinalAsakaiAppService(IRepository<AsyAdoTotalDelayFinalAsakai, long> repo,
                                         IDapperRepository<AsyAdoTotalDelayFinalAsakai, long> dapperRepo,
                                        IAsyAdoTotalDelayFinalAsakaiExcelExporter calendarListExcelExporter
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
        }


        public async Task<PagedResultDto<AsyAdoTotalDelayFinalAsakaiDto>> GetAll(GetAsyAdoTotalDelayFinalAsakaiInput input)
        {
            var filtered = _repo.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.BodyNo), e => e.BodyNo.Contains(input.BodyNo))
                .WhereIf(!string.IsNullOrWhiteSpace(input.LotNo), e => e.LotNo.Contains(input.LotNo));
            var pageAndFiltered = filtered.OrderBy(s => s.Id);


            var system = from o in pageAndFiltered
                         select new AsyAdoTotalDelayFinalAsakaiDto
                         {
                             Id = o.Id,
                             BodyNo = o.BodyNo,
                             LotNo = o.LotNo,
                             Color = o.Color,
                             TotalDelayLeadTime = o.TotalDelayLeadTime,
                             DispatchPlanDatetime = o.DispatchPlanDatetime,
                             CurrLocation = o.CurrLocation,
                             Location = o.Location,
                             WDelayWithLeadTime = o.WDelayWithLeadTime,
                             TDelayWithLeadTime = o.TDelayWithLeadTime,
                             ADelayWithLeadTime = o.ADelayWithLeadTime,
                             IDelayWithLeadTime = o.IDelayWithLeadTime,
                             FOutDelay = o.FOutDelay,
                             DelayFlag = o.DelayFlag,
                         };

            var totalCount = await filtered.CountAsync();
            var paged = system.PageBy(input);

            return new PagedResultDto<AsyAdoTotalDelayFinalAsakaiDto>(
                totalCount,
                 await paged.ToListAsync()
            );
        }


        public async Task<FileDto> GetTotalDelayFinalAsakaiToExcel(GetAsyAdoTotalDelayFinalAsakaiExportInput input)
        {
            var filtered = _repo.GetAll()
              .WhereIf(!string.IsNullOrWhiteSpace(input.BodyNo), e => e.BodyNo.Contains(input.BodyNo))
              .WhereIf(!string.IsNullOrWhiteSpace(input.LotNo), e => e.LotNo.Contains(input.LotNo));

            var pageAndFiltered = filtered.OrderBy(s => s.Id);

            var query = from o in pageAndFiltered
                        select new AsyAdoTotalDelayFinalAsakaiDto
                        {
                            Id = o.Id,
                            BodyNo = o.BodyNo,
                            LotNo = o.LotNo,
                            Color = o.Color,
                            TotalDelayLeadTime = o.TotalDelayLeadTime,
                            DispatchPlanDatetime = o.DispatchPlanDatetime,
                            CurrLocation = o.CurrLocation,
                            Location = o.Location,
                            WDelayWithLeadTime = o.WDelayWithLeadTime,
                            TDelayWithLeadTime = o.TDelayWithLeadTime,
                            ADelayWithLeadTime = o.ADelayWithLeadTime,
                            IDelayWithLeadTime = o.IDelayWithLeadTime,
                            FOutDelay = o.FOutDelay,
                            DelayFlag = o.DelayFlag,
                        };
            var exportToExcel = await query.ToListAsync();
            return _calendarListExcelExporter.ExportToFile(exportToExcel);
        }

    }
}
