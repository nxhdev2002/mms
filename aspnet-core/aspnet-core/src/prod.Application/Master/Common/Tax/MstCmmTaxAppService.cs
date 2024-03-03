using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Dapper.Repositories;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Microsoft.EntityFrameworkCore;
using prod.Authorization;
using prod.Dto;
using prod.Master.Cmm.Dto;
using prod.Master.Common.Tax.Exporting;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace prod.Master.Cmm
{
    [AbpAuthorize(AppPermissions.Pages_Master_Cmm_Tax_View)]
    public class MstCmmTaxAppService : prodAppServiceBase, IMstCmmTaxAppService
    {
        private readonly IDapperRepository<MstCmmTax, long> _dapperRepo;
        private readonly IRepository<MstCmmTax, long> _repo;
        private readonly IMstCmmTaxExcelExporter _calendarListExcelExporter;

        public MstCmmTaxAppService(IRepository<MstCmmTax, long> repo,
                                         IDapperRepository<MstCmmTax, long> dapperRepo,
                                        IMstCmmTaxExcelExporter calendarListExcelExporter
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
        }

        public async Task<PagedResultDto<MstCmmTaxDto>> GetAll(GetMstCmmTaxInput input)
        {
            var filtered = _repo.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.Code), e => e.Code.Contains(input.Code))
                .WhereIf(!string.IsNullOrWhiteSpace(input.Description), e => e.Description.Contains(input.Description))
                .WhereIf(!string.IsNullOrWhiteSpace(input.Rate), e => e.Rate.Contains(input.Rate))
                ;
            var pageAndFiltered = filtered.OrderBy(s => s.Id);


            var system = from o in pageAndFiltered
                         select new MstCmmTaxDto
                         {
                             Id = o.Id,
                             Code = o.Code,
                             Description = o.Description,
                             Rate = o.Rate,
                         };

            var totalCount = await filtered.CountAsync();
            var paged = system.PageBy(input);

            return new PagedResultDto<MstCmmTaxDto>(
                totalCount,
                 await paged.ToListAsync()
            );
        }


        public async Task<FileDto> GetTaxToExcel(MstCmmTaxExportInput input)
        {
            var filtered = _repo.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.Code), e => e.Code.Contains(input.Code))
                .WhereIf(!string.IsNullOrWhiteSpace(input.Description), e => e.Description.Contains(input.Description))
                .WhereIf(!string.IsNullOrWhiteSpace(input.Rate), e => e.Rate.Contains(input.Rate));
            var pageAndFiltered = filtered.OrderBy(s => s.Id);
            var system = from o in pageAndFiltered
                         select new MstCmmTaxDto
                        {
                            Id = o.Id,
                            Code = o.Code,
                            Description = o.Description,
                            Rate = o.Rate,
                        };
            var exportToExcel = await system.ToListAsync();
            return _calendarListExcelExporter.ExportToFile(exportToExcel);
        }

        // public async Task GenerateAsync()
        //  {
        //    await _dapperRepo.ExecuteAsync(MstCmmTaxConsts.SP_MST_WPT_CALENDAR_GENERATE);
        // }

    }
}

