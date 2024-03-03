using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Dapper.Repositories;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Microsoft.EntityFrameworkCore;
using prod.Assy.Andon.Dto;
using prod.Authorization;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace prod.Assy.Andon
{
    [AbpAuthorize(AppPermissions.Pages_ProdPlan_APlanShiftBase_View)]
    public class AsyAdoAPlanShiftAppService : prodAppServiceBase, IAsyAdoAPlanShiftAppService
    {
        private readonly IDapperRepository<AsyAdoAPlanShift, long> _dapperRepo;
        private readonly IRepository<AsyAdoAPlanShift, long> _repo;


        public AsyAdoAPlanShiftAppService(IRepository<AsyAdoAPlanShift, long> repo,
                                         IDapperRepository<AsyAdoAPlanShift, long> dapperRepo)
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
        }

        public async Task<PagedResultDto<AsyAdoAPlanShiftDto>> GetAll(GetAsyAdoAPlanShiftInput input)
        {
            var filtered = _repo.GetAll()
                .WhereIf(input.WorkingDate.HasValue, t => t.WorkingDate == input.WorkingDate.Value)
                .WhereIf(!string.IsNullOrWhiteSpace(input.ProdLine), e => e.ProdLine.Contains(input.ProdLine));

            var pageAndFiltered = filtered.OrderByDescending(s => s.WorkingDate);


            var system = from o in pageAndFiltered
                         select new AsyAdoAPlanShiftDto
                         {
                             Id = o.Id,
                             WorkingDate = o.WorkingDate,
                             ProdLine = o.ProdLine,
                             Shift1 = o.Shift1,
                             Shift2 = o.Shift2,
                             Shift3 = o.Shift3,
                             IsActive = o.IsActive,
                         };

            var totalCount = await filtered.CountAsync();
            var paged = system.PageBy(input);

            return new PagedResultDto<AsyAdoAPlanShiftDto>(
                totalCount,
                 await paged.ToListAsync()
            );
        }



    }
}
