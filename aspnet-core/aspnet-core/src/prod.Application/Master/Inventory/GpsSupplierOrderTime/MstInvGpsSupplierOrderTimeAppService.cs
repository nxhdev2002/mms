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
using prod.Master.Inv.Dto;
using prod.Master.Inv.Exporting;
using FastMember;
using GemBox.Spreadsheet;
using prod.Inventory.CKD.Dto;
using System.Data;
using System.IO;
using prod.Common;
using prod.Master.Inventory.GpsSupplierOrderTime.Dto;

namespace prod.Master.Inv
{
    //  [AbpAuthorize(AppPermissions.Pages_Master_Inv_MstInvGpsSupplierOrderTime)]
    public class MstInvGpsSupplierOrderTimeAppService : prodAppServiceBase, IMstInvGpsSupplierOrderTimeAppService
    {
        private readonly IDapperRepository<MstInvGpsSupplierOrderTime, long> _dapperRepo;
        private readonly IRepository<MstInvGpsSupplierOrderTime, long> _repo;
        private readonly IMstInvGpsSupplierOrderTimeExcelExporter _calendarListExcelExporter;

        public MstInvGpsSupplierOrderTimeAppService(IRepository<MstInvGpsSupplierOrderTime, long> repo,
                                         IDapperRepository<MstInvGpsSupplierOrderTime, long> dapperRepo,
                                        IMstInvGpsSupplierOrderTimeExcelExporter calendarListExcelExporter
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
        }

        //  [AbpAuthorize(AppPermissions.Pages_Master_Inv_MstInvGpsSupplierOrderTime_Edit)]
        public async Task CreateOrEdit(CreateOrEditMstInvGpsSupplierOrderTimeDto input)
        {
            if (input.Id == null) await Create(input);
            else await Update(input);
        }

        //CREATE
        private async Task Create(CreateOrEditMstInvGpsSupplierOrderTimeDto input)
        {
            var mainObj = ObjectMapper.Map<MstInvGpsSupplierOrderTime>(input);

            await CurrentUnitOfWork.GetDbContext<prodDbContext>().AddAsync(mainObj);
        }

        // EDIT
        private async Task Update(CreateOrEditMstInvGpsSupplierOrderTimeDto input)
        {
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                var mainObj = await _repo.GetAll()
                .FirstOrDefaultAsync(e => e.Id == input.Id);

                var mainObjToUpdate = ObjectMapper.Map(input, mainObj);
            }
        }

        //  [AbpAuthorize(AppPermissions.Pages_Master_Inv_MstInvGpsSupplierOrderTime_Delete)]
        public async Task Delete(EntityDto input)
        {
            var mainObj = await _repo.FirstOrDefaultAsync(input.Id);
            _repo.HardDelete(mainObj);
            CurrentUnitOfWork.GetDbContext<prodDbContext>().Remove(mainObj);
        }
        public async Task<FileDto> GetMstInvGpsSupplierOrderTimeToExcel(MstInvGpsSupplierOrderTimeExportInput input)
        {
            var query = from o in _repo.GetAll().Where(e=>e.SupplierId==input.SupplierId)
                        select new MstInvGpsSupplierOrderTimeDto
                        {
                            Id = o.Id,
                            SupplierId = o.SupplierId,
                            OrderSeq = o.OrderSeq,
                            OrderType = o.OrderType,
                            OrderTime = o.OrderTime,
                            ReceivingDay = o.ReceivingDay,
                            ReceiveTime = o.ReceiveTime,
                            KeihenTime = o.KeihenTime,
                            KeihenDay = o.KeihenDay,
                            IsActive = o.IsActive,
                        };
            var exportToExcel = await query.ToListAsync();
            return _calendarListExcelExporter.ExportToFile(exportToExcel);
        }
    }
}
