using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Dapper.Repositories;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.EntityFrameworkCore.Uow;
using Abp.Linq.Extensions;
using Microsoft.EntityFrameworkCore;
using prod.Authorization;
using prod.Dto;
using prod.EntityFrameworkCore;
using prod.Master.Inventory.Exporting;
using prod.Master.Pio.Dto;
using prod.Master.Pio.Exporting;
using System.Linq;
using System.Threading.Tasks;

namespace prod.Master.Pio
{
    [AbpAuthorize(AppPermissions.Pages_Master_MstPioImpSupplier_View)]
    public class MstPioImpSupplierAppService : prodAppServiceBase, IMstPioImpSupplierAppService
    {
        //private readonly IDapperRepository<MstPioImpSupplier, long> _dapperRepo;
        private readonly IRepository<MstPioImpSupplier, long> _repo;
        private readonly IMstPioImpSupplierExcelExporter _mstlspSupplierinfoExporter;
        private readonly Abp.ObjectMapping.IObjectMapper _objectMapper;

        public MstPioImpSupplierAppService(IRepository<MstPioImpSupplier, long> repo,
                                         //IDapperRepository<MstPioImpSupplier, long> dapperRepo
                                        IMstPioImpSupplierExcelExporter mstlspSupplierinfoExporter
                                        , Abp.ObjectMapping.IObjectMapper objectMapper)
        {
            _repo = repo;
            //_dapperRepo = dapperRepo;
            _mstlspSupplierinfoExporter = mstlspSupplierinfoExporter;
            _objectMapper = objectMapper;
        }

        [AbpAuthorize(AppPermissions.Pages_Master_MstPioImpSupplier_Edit)]
        public async Task CreateOrEdit(CreateOrEditMstPioImpSupplierDto input)
        {
            if (input.Id == null) await Create(input);
            else await Update(input);
        }
        //CREATE
        private async Task Create(CreateOrEditMstPioImpSupplierDto input)
        {
            var mainObj = ObjectMapper.Map<MstPioImpSupplier>(input);

            await CurrentUnitOfWork.GetDbContext<prodDbContext>().AddAsync(mainObj);
        }
        // EDIT
        private async Task Update(CreateOrEditMstPioImpSupplierDto input)
        {
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                var mainObj = await _repo.GetAll()
                .FirstOrDefaultAsync(e => e.Id == input.Id);

                var mainObjToUpdate = ObjectMapper.Map(input, mainObj);
            }
        }
        [AbpAuthorize(AppPermissions.Pages_PIO_Master_Supplier_Info_Delete)]
        public async Task Delete(EntityDto input)
        {
            var mainObj = await _repo.FirstOrDefaultAsync(input.Id);
            _repo.HardDelete(mainObj);
            CurrentUnitOfWork.GetDbContext<prodDbContext>().Remove(mainObj);
        }



        public async Task<PagedResultDto<MstPioImpSupplierDto>> GetAll(GetMstPioImpSupplierInput input)
        {
            var filtered = _repo.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.SupplierNo), e => e.SupplierNo.Contains(input.SupplierNo))
                .WhereIf(!string.IsNullOrWhiteSpace(input.SupplierName), e => e.SupplierName.Contains(input.SupplierName))
                .WhereIf(!string.IsNullOrWhiteSpace(input.SupplierType), e => e.SupplierType.Contains(input.SupplierType))
                ;
            var pageAndFiltered = filtered.OrderBy(s => s.Id);
            var system = _objectMapper.ProjectTo<MstPioImpSupplierDto>(pageAndFiltered);


            var totalCount = await filtered.CountAsync();
            var paged = system.PageBy(input);

            return new PagedResultDto<MstPioImpSupplierDto>(
                totalCount,
                 await paged.ToListAsync()
            );
        }

        public async Task<FileDto> GetMstLspSupplierInfoToExcel(GetMstPioImpSupplierExportInput input)
        {

            var filtered = _repo.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.SupplierNo), e => e.SupplierNo.Contains(input.SupplierNo))
                .WhereIf(!string.IsNullOrWhiteSpace(input.SupplierName), e => e.SupplierName.Contains(input.SupplierName))
                .WhereIf(!string.IsNullOrWhiteSpace(input.SupplierType), e => e.SupplierType.Contains(input.SupplierType))
                ;
            var pageAndFiltered = filtered.OrderBy(s => s.Id);

            var query = _objectMapper.ProjectTo<MstPioImpSupplierDto>(pageAndFiltered);

            var exportToExcel = await query.ToListAsync();
            return _mstlspSupplierinfoExporter.ExportToFile(exportToExcel);
        }
    }
}
