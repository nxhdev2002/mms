using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Dapper.Repositories;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.EntityFrameworkCore.Uow;
using Abp.Linq.Extensions;
using Abp.Runtime.Session;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using prod.Authorization;
using prod.Common;
using prod.Dto;
using prod.Frame.Andon;
using prod.Inv.D125;
using prod.Inv.D125.Dto;
using prod.Inv.D125.Exporting;
using prod.Configuration.Host;
using prod.Configuration.Host.Dto;
using prod.Authorization.Users;

namespace prod.Inv.Stock
{
    //  [AbpAuthorize(AppPermissions.Pages_Inv_D125_Stock)]
    public class InvStockAppService : prodAppServiceBase, IInvStockAppService
    {
        private readonly IDapperRepository<InvStock, long> _dapperRepo;
        private readonly IRepository<InvStock, long> _repo;
        private readonly IInvStockExcelExporter _calendarListExcelExporter;
        private readonly IHostSettingsAppService _emailSettingService;
        private readonly IUserEmailer _userEmailer;

        public InvStockAppService(IRepository<InvStock, long> repo,
                                         IDapperRepository<InvStock, long> dapperRepo,
                                        IInvStockExcelExporter calendarListExcelExporter,
                                        IHostSettingsAppService emailSettingService,
                                        IUserEmailer userEmailer
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
            _emailSettingService = emailSettingService;
            _userEmailer = userEmailer;
        }

        public async Task<PagedResultDto<InvStockDto>> GetAll(GetInvStockInput input)
        {
            var filtered = _repo.GetAll()
                .WhereIf(input.PeriodId != 0, e => e.PeriodId == input.PeriodId)
                .WhereIf(!string.IsNullOrWhiteSpace(input.PartNo), e => e.PartNo.Contains(input.PartNo))
                .WhereIf(!string.IsNullOrWhiteSpace(input.Source), e => e.Source.Contains(input.Source))
                .WhereIf(!string.IsNullOrWhiteSpace(input.CarFamilyCode), e => e.CarFamilyCode.Contains(input.CarFamilyCode))
                .WhereIf(!string.IsNullOrWhiteSpace(input.LotNo), e => e.LotNo.Contains(input.LotNo))
                .WhereIf(!string.IsNullOrWhiteSpace(input.InStockByLot), e => e.InStockByLot.Contains(input.InStockByLot));

            var pageAndFiltered = filtered.OrderBy(s => s.Id);

            var system = from o in pageAndFiltered
                         select new InvStockDto
                         {
                             Id = o.Id,
                             PeriodId = o.PeriodId,
                             PartNo = o.PartNo,
                             Source = o.Source,
                             CarFamilyCode = o.CarFamilyCode,
                             LotNo = o.LotNo,
                             Quantity = o.Quantity,
                             CustomsDeclareNo = o.CustomsDeclareNo,
                             DeclareDate = o.DeclareDate,
                             DcType = o.DcType,
                             InStockByLot = o.InStockByLot,
                             Cif = o.Cif,
                             Tax = o.Tax,
                             Inland = o.Inland,
                             Cost = o.Cost,
                             Amount = o.Amount,
                             CifVn = o.CifVn,
                             TaxVn = o.TaxVn,
                             InlandVn = o.InlandVn,
                             CostVn = o.CostVn,
                             AmountVn = o.AmountVn,
                         };

            var totalCount = await filtered.CountAsync();
            var paged = system.PageBy(input);

            return new PagedResultDto<InvStockDto>(
                totalCount,
                 await paged.ToListAsync()
            );
        }


        public async Task<FileDto> GetStockToExcel(GetInvStockẼportInput input)
        {

            var filtered = _repo.GetAll()
                .WhereIf(input.PeriodId != 0, e => e.PeriodId == input.PeriodId)
                .WhereIf(!string.IsNullOrWhiteSpace(input.PartNo), e => e.PartNo.Contains(input.PartNo))
                .WhereIf(!string.IsNullOrWhiteSpace(input.Source), e => e.Source.Contains(input.Source))
                .WhereIf(!string.IsNullOrWhiteSpace(input.CarFamilyCode), e => e.CarFamilyCode.Contains(input.CarFamilyCode))
                .WhereIf(!string.IsNullOrWhiteSpace(input.LotNo), e => e.LotNo.Contains(input.LotNo))
                .WhereIf(!string.IsNullOrWhiteSpace(input.InStockByLot), e => e.InStockByLot.Contains(input.InStockByLot));

            var query = from o in filtered
                        select new InvStockDto
                        {
                            Id = o.Id,
                            PeriodId = o.PeriodId,
                            PartNo = o.PartNo,
                            Source = o.Source,
                            CarFamilyCode = o.CarFamilyCode,
                            LotNo = o.LotNo,
                            Quantity = o.Quantity,
                            CustomsDeclareNo = o.CustomsDeclareNo,
                            DeclareDate = o.DeclareDate,
                            DcType = o.DcType,
                            InStockByLot = o.InStockByLot,
                            Cif = o.Cif,
                            Tax = o.Tax,
                            Inland = o.Inland,
                            Cost = o.Cost,
                            Amount = o.Amount,
                            CifVn = o.CifVn,
                            TaxVn = o.TaxVn,
                            InlandVn = o.InlandVn,
                            CostVn = o.CostVn,
                            AmountVn = o.AmountVn,
                        };
            var exportToExcel = await query.ToListAsync();
            return _calendarListExcelExporter.ExportToFile(exportToExcel);
        }



        public async Task<PagedResultDto<InvPeriodDto>> GetIdInvPeriod()
        {
            string _sql = "Exec  INV_PERIOD_GETID";

            var data = await _dapperRepo.QueryAsync<InvPeriodDto>(_sql, new {});

            var results = from d in data
                          select new InvPeriodDto
                          {
                            Id = d.Id,
                            Description = d.Description,
                            Status = d.Status
                          };

            var totalCount = data.ToList().Count;
            return new PagedResultDto<InvPeriodDto>(
                totalCount,
                results.ToList()
            );
        }

        public async Task<PagedResultDto<InvPeriodDto>> ReportRequest(string ckbImportbyCont, 
                                                                    string ckbStock, 
                                                                    string ckbOutWipStock, 
                                                                    string ckbOutLineOff, 
                                                                    string ckbDetails, 
                                                                    string ckbNotificationEmail,
                                                                    int txtPid,
                                                                    int txtPid_d125,
                                                                    string txtGrade,
                                                                    string txtFrame)
        {
            var user = await UserManager.FindByIdAsync(AbpSession.GetUserId().ToString());
            var userId = user.Id;
            var userName = user.UserName;


            var DmrPeriod = await _dapperRepo.QueryAsync<InvPeriodDto>("Exec INV_PERIOD_GETBYID @id", new { id = txtPid });
            var D125Period = await _dapperRepo.QueryAsync<InvPeriodDto>("Exec INV_PERIOD_GETBYID @id", new { id = txtPid_d125 });
            var v_DmrPeriod = DmrPeriod.Count() > 0 ? DmrPeriod.ToList()[0].Description : "";
            var v_D125Period = D125Period.Count() > 0 ? D125Period.ToList()[0].Description : "";

            var dataListInput = ckbImportbyCont.ToString() + ";" + ckbStock.ToString() + ";" + ckbOutWipStock.ToString() + ";" + ckbOutLineOff.ToString() + ";" + ckbDetails.ToString() + ";" +
                                    v_DmrPeriod + ";" + v_D125Period + ";" + txtGrade + ";" + txtFrame;

            var reportlist = (ckbImportbyCont == "true" ? "IMPORT_BY_CONT" : "")
                            + (ckbStock == "true" ? ",STOCK" : "")
                            + (ckbOutWipStock == "true" ? ",OUT_WIP_STOCK" : "")
                            + (ckbOutLineOff == "true" ? ",OUT_LINEOFF" : "")
                            + (ckbDetails == "true" ? ",IN_DETAILS" : "");

            var v_params = "Period_Daily :" + txtPid.ToString() + ";"
                         + "Period_D125 :" + txtPid_d125.ToString() + ";"
                         + "Grade_List :" + txtGrade + ";"
                         + "Frame_List :" + txtFrame ;

            //call store procedure
            string _sql = "Exec CMM_REPORT_REQUEST_SUBMIT @p_report_list,@p_params,@p_creator_user_id,@p_request_id";
            await _dapperRepo.QueryAsync<InvStock>(_sql, new
            {
                p_report_list = reportlist,
                p_params = v_params,
                p_creator_user_id = userId,
                p_request_id = 0
            });

            //check send mail
            if (ckbNotificationEmail == "true")
            {
                await _userEmailer.SendEmailReportRequetsAsync(userName, dataListInput, "", "helpdesk@toyotavn.com.vn", "", "Report Request");
            }
            return null;

        }
    }
}
