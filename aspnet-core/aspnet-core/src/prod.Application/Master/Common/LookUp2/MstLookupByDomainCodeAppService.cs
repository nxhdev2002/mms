using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Dapper.Repositories;
using Abp.Domain.Repositories;
using Abp.EntityFrameworkCore.Uow;
using Abp.Linq.Extensions;
using Abp.ObjectMapping;
using Abp.UI;
using AutoMapper.Internal.Mappers;
using Microsoft.EntityFrameworkCore;
using prod.Authorization;
using prod.EntityFrameworkCore;
using prod.Master.Common.Exporting;
using prod.Master.Common.LookUp2.Dto;
using prod.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prod.Master.Common.LookUp2
{
/*    //  [AbpAuthorize(AppPermissions.Pages_Master_Common_Lookup)]*/
    public  class MstLookupByDomainCodeAppService : prodAppServiceBase, IMstLookupByDomainCodeAppService
    {
        private readonly IDapperRepository<MstCmmLookup, long> _dapperRepo;
        private readonly IRepository<MstCmmLookup, long> _repo;
        private readonly IMstCmmLookupExcelExporter _calendarListExcelExporter;


        public MstLookupByDomainCodeAppService
            (
                                      IRepository<MstCmmLookup, long> repo,
                                      IDapperRepository<MstCmmLookup, long> dapperRepo,
                                      IMstCmmLookupExcelExporter calendarListExcelExporter
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;

        }
        
        public async Task<PagedResultDto<GetMstLookUpForViewDto>> GetAll(GetMstLookUpInput input)
        {
            var query = from o in _repo.GetAll()
                         .Where(e => string.IsNullOrWhiteSpace(input.filterText))
                    
                        orderby o.Id ascending
                        select new GetMstLookUpForViewDto
                        {
                            Id = o.Id,
                            DomainCode = o.DomainCode,
                            ItemCode = o.ItemCode,
                            ItemValue = o.ItemValue,
                            ItemOrder = o.ItemOrder,
                            Description = o.Description,
                            IsUse = o.IsUse,
                            IsRestrict = o.IsRestrict,
                        };
            var totalCount = await query.CountAsync();
            var pagedAndFilteredVehicleProductAppliedPosition = query.PageBy(input);

            return new PagedResultDto<GetMstLookUpForViewDto>(
                totalCount,
                await pagedAndFilteredVehicleProductAppliedPosition.ToListAsync());
        }
      /*  //  [AbpAuthorize(AppPermissions.Pages_Master_Common_Lookup_Edit)]*/
        public async Task CreateOrEdit(CreateOrEditMstLookUpDto input)
        {
            if (input.Id == null)
            {
                await Create(input);
            }
            else
            {
                await Update(input);
            }
        }

  
        protected virtual async Task Create(CreateOrEditMstLookUpDto input)
        {

            var mainObj = ObjectMapper.Map<MstCmmLookup>(input);

            await _repo.InsertAsync(mainObj);
        }

        protected virtual async Task Update(CreateOrEditMstLookUpDto input)
        {
            var mainObj = await _repo.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, mainObj);

        }

        public async Task Delete(EntityDto<long> input)
        {
            var result = await _repo.GetAll().FirstOrDefaultAsync(e => e.Id == input.Id); 
            _repo.HardDelete(result); 
            await _repo.DeleteAsync(result);

            /*
              var mainObj = await _repo.FirstOrDefaultAsync(input.Id);
                                    _repo.HardDelete(mainObj);
                CurrentUnitOfWork.GetDbContext<prodDbContext>().Remove(mainObj);
            */
        }
        public async Task<GetMstLookUpForOutput> GetMstLookUpEdit(EntityDto<long> input)
        {
            var mainObj = await _repo.FirstOrDefaultAsync(input.Id);

            var ouput = new GetMstLookUpForOutput { MstLookUpForEdit = ObjectMapper.Map<CreateOrEditMstLookUpDto>(mainObj) };

            return ouput;
        }
    }
}
