using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Dapper.Repositories;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.EntityFrameworkCore.Uow;
using Abp.Linq.Extensions;
using Abp.UI;
using FastMember;
using GemBox.Spreadsheet;
using Microsoft.EntityFrameworkCore;
using prod.Authorization;
using prod.Dto;
using prod.EntityFrameworkCore;
using prod.Inventory.CKD.Exporting;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using prod.Common;
using Abp.EntityFrameworkCore;
using System.Globalization;
using prod.Inventory.CKD.Dto;
using PayPalCheckoutSdk.Orders;
using Abp.AspNetZeroCore.Net;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using prod.Inventory.CKD.ShippingSchedule.Dto;
using prod.Storage;

namespace prod.Inventory.CKD
{
    [AbpAuthorize(AppPermissions.Pages_Ckd_Warehouse_ContainerRentalWHPlan_View)]
    public class InvCkdContainerRentalWHPlanAppService : prodAppServiceBase, IInvCkdContainerRentalWHPlanAppService
    {
        private readonly IDapperRepository<InvCkdContainerRentalWHPlan, long> _dapperRepo;
        private readonly IRepository<InvCkdContainerRentalWHPlan, long> _repo;
        private readonly IRepository<InvCkdContainerRentalWhRepack, long> _repackRepo;
        private readonly IInvCkdContainerRentalWHPlanExcelExporter _calendarListExcelExporter;
        private readonly IDbContextProvider<prodDbContext> _dbContextProvider;
        private readonly ITempFileCacheManager _tempFileCacheManager;
        public InvCkdContainerRentalWHPlanAppService(IRepository<InvCkdContainerRentalWHPlan, long> repo,
                                        IRepository<InvCkdContainerRentalWhRepack, long> repackRepo,
                                         IDapperRepository<InvCkdContainerRentalWHPlan, long> dapperRepo,
                                        IInvCkdContainerRentalWHPlanExcelExporter calendarListExcelExporter,
                                        IDbContextProvider<prodDbContext> dbContextProvider,
                                        ITempFileCacheManager tempFileCacheManager
            )
        {
            _repo = repo;
            _repackRepo = repackRepo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
            _dbContextProvider = dbContextProvider;
            _tempFileCacheManager = tempFileCacheManager;
        }

        [AbpAuthorize(AppPermissions.Pages_Ckd_Warehouse_ContainerRentalWHPlan_Edit)]
        public async Task CreateOrEdit(CreateOrEditInvCkdContainerRentalWHPlanDto input)
        {
            if (input.Id == null) await Create(input);
            else await Update(input);
        }

        //CREATE
        private async Task Create(CreateOrEditInvCkdContainerRentalWHPlanDto input)
        {
            var mainObj = ObjectMapper.Map<InvCkdContainerRentalWHPlan>(input);

            await CurrentUnitOfWork.GetDbContext<prodDbContext>().AddAsync(mainObj);
        }

        // EDIT
        private async Task Update(CreateOrEditInvCkdContainerRentalWHPlanDto input)
        {
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                var mainObj = await _repo.GetAll()
                .FirstOrDefaultAsync(e => e.Id == input.Id);

                var mainObjToUpdate = ObjectMapper.Map(input, mainObj);
            }
        }

        public async Task<int> ConFirmStatus(string p_container_id, string p_status)
        {
            try
            {
                string _sql = @"EXEC INV_CKD_CONTAINER_RETAL_WH_PLAN_UPDATE_STATUS @p_container_id, @p_status, @p_status_date, @p_user_id";
                var filtered = await _dapperRepo.ExecuteAsync(_sql, new
                {
                    p_container_id = p_container_id,
                    p_status = p_status,
                    p_status_date = DateTime.Now,
                    p_user_id = AbpSession.UserId
                });
                return filtered;
            }
            catch (Exception E)
            {
                return 0;
            }
        }
        public async Task<int> ConFirmStatusMultiCkb(string listIdStatus, string status)
        {
            try
            {
                string _sql = @"EXEC INV_CKD_CONTAINER_RETAL_WH_PLAN_UPDATE_STATUS @p_list_id_status,@p_status,@p_status_date, @p_user_id";
                var filtered = await _dapperRepo.ExecuteAsync(_sql, new
                {
                    p_list_id_status = listIdStatus,
                    p_status = status,
                    p_status_date = DateTime.Now,
                    p_user_id = AbpSession.UserId
                });
                return filtered;
            }
            catch (Exception E)
            {
                return 0;
            }
        }

        [AbpAuthorize(AppPermissions.Pages_Ckd_Warehouse_ContainerRentalWHPlan_Edit)]
        public async Task Delete(EntityDto input)
        {

            var mainObj = await _repo.FirstOrDefaultAsync(input.Id);
            _repo.HardDelete(mainObj);
            CurrentUnitOfWork.GetDbContext<prodDbContext>().Remove(mainObj);

        }
        public async Task<PagedResultDto<InvCkdContainerRentalWHPlanDto>> GetAll(GetInvCkdContainerRentalWHPlanInput input)
        {
            string _sql = "Exec INV_CKD_CONTAINER_RENTAL_WH_PLAN @p_container_no, @p_renban, @p_invoice_no , @p_bill_of_lading_no , @p_supplier_no , @p_sealno , " +
                "@p_request_date_from , @p_request_date_to, @p_lotno ,@p_module_case_no , @p_partno";

            IEnumerable<InvCkdContainerRentalWHPlanDto> result = await _dapperRepo.QueryAsync<InvCkdContainerRentalWHPlanDto>(_sql, new
            {
                p_container_no = input.ContainerNo,
                p_renban = input.Renban,
                p_invoice_no = input.InvoiceNo,
                p_bill_of_lading_no = input.BillofladingNo,
                p_supplier_no = input.SupplierNo,
                p_sealno = input.SealNo,
                p_request_date_from = input.RequestDateFrom,
                p_request_date_to = input.RequestDateTo,
                p_lotno = input.LotNo,
                p_module_case_no = input.ModuleCaseNo,
                p_partno = input.PartNo,

            });

            var listResult = result.ToList();


            var totalCount = result.ToList().Count();
            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            return new PagedResultDto<InvCkdContainerRentalWHPlanDto>(
                totalCount,
                pagedAndFiltered);

        }
        public async Task<FileDto> GetContainerRentalWHPlanToExcel(GetInvCkdContainerRentalWHPlanInput input)
        {
            string _sql = "Exec INV_CKD_CONTAINER_RENTAL_WH_PLAN @p_container_no, @p_renban, @p_invoice_no , @p_bill_of_lading_no , @p_supplier_no , @p_sealno , " +
                "@p_request_date_from , @p_request_date_to,@p_lotno ,@p_module_case_no , @p_partno";
            IEnumerable<InvCkdContainerRentalWHPlanDto> result = await _dapperRepo.QueryAsync<InvCkdContainerRentalWHPlanDto>(_sql,
              new
              {
                  p_container_no = input.ContainerNo,
                  p_renban = input.Renban,
                  p_invoice_no = input.InvoiceNo,
                  p_bill_of_lading_no = input.BillofladingNo,
                  p_supplier_no = input.SupplierNo,
                  p_sealno = input.SealNo,
                  p_request_date_from = input.RequestDateFrom,
                  p_request_date_to = input.RequestDateTo,
                  p_lotno = input.LotNo,
                  p_module_case_no = input.ModuleCaseNo,
                  p_partno = input.PartNo,

              });
            var exportToFile = result.ToList();

            return _calendarListExcelExporter.ExportToFile(exportToFile);
        }

        public async Task<PagedResultDto<InvCkdContainerRentalWHPlanDto>> GetDataLateDate()
        {

            var getlatedate = _repo.GetAll().OrderByDescending(s => s.RequestDate).FirstOrDefault().RequestDate;
            var filtered = _repo.GetAll()
              .Where(e => e.RequestDate == getlatedate);


            var system = from o in filtered
                         select new InvCkdContainerRentalWHPlanDto
                         {
                             Id = o.Id,
                             ContainerNo = o.ContainerNo,
                             Renban = o.Renban,
                             RequestDate = o.RequestDate,
                             RequestTime = o.RequestTime,
                             InvoiceNo = o.InvoiceNo,
                             BillofladingNo = o.BillofladingNo,
                             SupplierNo = o.SupplierNo,
                             SealNo = o.SealNo,
                             ListcaseNo = o.ListcaseNo,
                             ListLotNo = o.ListLotNo,
                             CdDate = o.CdDate,
                             DevanningDate = o.DevanningDate,
                             DevanningTime = o.DevanningTime,
                             ActualDevanningDate = o.ActualDevanningDate,
                             GateInPlanTime = o.GateInPlanTime,
                             GateInActualDateTime = o.GateInActualDateTime,
                             Transport = o.Transport,
                             PlateId = o.PlateId,
                             Status = o.Status,
                             IsActive = o.IsActive,
                         };

            var totalCount = await filtered.CountAsync();

            return new PagedResultDto<InvCkdContainerRentalWHPlanDto>(
                totalCount,
                 await system.ToListAsync()
            );
        }

        [AbpAuthorize(AppPermissions.Pages_Ckd_Warehouse_ContainerRentalWHPlan_Import)]
        public async Task<List<InvCkdContainerRentalWHPlanTDto>> GetImportDataFromExcel(byte[] fileBytes, string fileName)
        {
            try
            {
                List<InvCkdContainerRentalWHPlanTDto> listImport = new List<InvCkdContainerRentalWHPlanTDto>();
                using (var stream = new MemoryStream(fileBytes))
                {
                    SpreadsheetInfo.SetLicense("EF21-1FW1-HWZF-CLQH");
                    var xlWorkBook = ExcelFile.Load(stream);
                    var v_worksheet = xlWorkBook.Worksheets[0];

                    DateTime dateTime = DateTime.Now;

                    string v_devanning_date = (v_worksheet.Cells[4, 2]).Value?.ToString() ?? "";
                    string strGUID = Guid.NewGuid().ToString("N");
                    for (int i = 8; i < v_worksheet.Rows.Count; i++)
                    {
                        var row = new InvCkdContainerRentalWHPlanTDto();
                        row.Guid = strGUID;
                        string v_rowNUmber = (v_worksheet.Cells[i, 0]).Value?.ToString() ?? "";
                        string v_time_devanning = (v_worksheet.Cells[i, 1]).Value?.ToString() ?? "";
                        string v_container = (v_worksheet.Cells[i, 2]).Value?.ToString() ?? "";
                        string v_seal = (v_worksheet.Cells[i, 3]).Value?.ToString() ?? "";
                        string v_transport = (v_worksheet.Cells[i, 4]).Value?.ToString() ?? "";
                        string v_renban = (v_worksheet.Cells[i, 5]).Value?.ToString() ?? "";
                        string v_case_no = (v_worksheet.Cells[i, 6]).Value?.ToString() ?? "";

                        if (v_container == "") break;
                        row.RowNumber = v_rowNUmber;

                        // validate devaning date
                        try
                        {
                            DateTime.Parse(v_devanning_date);
                            row.DevanningDate = DateTime.Parse(v_devanning_date);
                        }
                        catch
                        {
                            row.ErrorDescription = "Ngày nhận không hợp lệ!, ";
                        }

                        // validate length
                        if (v_container.Length > 15) row.ErrorDescription = "Độ dài Container No: " + v_container + " không hợp lệ! , ";
                        else row.ContainerNo = v_container;

                        if (v_seal.Length > 20) row.ErrorDescription += "Độ dài Seal No:" + v_seal + " không hợp lệ! , ";
                        else row.SealNo = v_seal;

                        if (v_transport.Length > 50) row.ErrorDescription += "Độ dài Transport:" + v_transport + " không hợp lệ! , ";
                        else row.Transport = v_transport;

                        if (v_renban.Length > 20) row.ErrorDescription += "Độ dài Renban không hợp lệ! , ";
                        else row.Renban = v_renban;

                        if (v_case_no.Length > 1000) row.ErrorDescription += "Độ dài List Case No không hợp lệ! , ";
                        else row.ListcaseNo = v_case_no;

                        try
                        {
                            row.DevanningTime = DateTime.Parse(v_time_devanning);
                        }
                        catch
                        {
                            row.ErrorDescription += "Giờ nhận không hợp lệ!, ";
                        }

                        try
                        {
                            int v_total_case = Int32.Parse((v_worksheet.Cells[i, 7]).Value?.ToString() ?? "0");
                            if (v_total_case < 0)
                            {
                                row.ErrorDescription += "Tổng Case nhận phải lớn hơn 0";
                            }
                        }
                        catch
                        {
                            row.ErrorDescription += "Tổng Case nhận phải là số!";
                        }

                        row.CreationTime = dateTime;
                        row.CreatorUserId = (int)AbpSession.UserId;
                        row.IsDeleted = 0;

                        listImport.Add(row);
                    }

                    // nếu có sheet 2(Bảo vệ) -> lấy thêm ngày plan Gate In
                    if (xlWorkBook.Worksheets.Count > 1)
                    {
                        var v_worksheet2 = xlWorkBook.Worksheets[1];
                        string v_plan_gate_in_date = (v_worksheet.Cells[4, 2]).Value?.ToString() ?? "";
                        try
                        {
                            DateTime.Parse(v_plan_gate_in_date);

                            for (int i = 8; i < v_worksheet2.Rows.Count; i++)
                            {
                                string v_container = (v_worksheet2.Cells[i, 2]).Value?.ToString() ?? "";
                                string v_seal = (v_worksheet2.Cells[i, 3]).Value?.ToString() ?? "";
                                string v_time_gatein = (v_worksheet.Cells[i, 1]).Value?.ToString() ?? "";
                                if (v_container == "") break;
                                try
                                {
                                    DateTime.Parse(v_time_gatein);
                                    if (listImport.Any(e => e.ContainerNo == v_container && e.SealNo == v_seal))
                                    {
                                        listImport.Where(e => e.ContainerNo == v_container && e.SealNo == v_seal).ToList()[0].GateInPlanDate = DateTime.Parse(v_plan_gate_in_date);
                                        listImport.Where(e => e.ContainerNo == v_container && e.SealNo == v_seal).ToList()[0].GateInPlanTime = DateTime.Parse(v_time_gatein);
                                    }
                                }
                                catch
                                {

                                }
                            }
                        }
                        catch
                        {

                        }
                    }
                }

                // import temp into db (bulkCopy)
                if (listImport.Count > 0)
                {
                    IEnumerable<InvCkdContainerRentalWHPlanTDto> dataE = listImport.AsEnumerable();
                    DataTable table = new DataTable();
                    using (var reader = ObjectReader.Create(dataE))
                    {
                        table.Load(reader);
                    }
                    string connectionString = Commons.getConnectionString();
                    using (Microsoft.Data.SqlClient.SqlConnection conn = new Microsoft.Data.SqlClient.SqlConnection(connectionString))
                    {
                        await conn.OpenAsync();

                        using (Microsoft.Data.SqlClient.SqlTransaction tran = conn.BeginTransaction(IsolationLevel.ReadCommitted))
                        {
                            using (var bulkCopy = new Microsoft.Data.SqlClient.SqlBulkCopy(conn, Microsoft.Data.SqlClient.SqlBulkCopyOptions.Default, tran))
                            {
                                bulkCopy.DestinationTableName = "InvCkdContainerRentalWHPlan_T";
                                bulkCopy.ColumnMappings.Add("Guid", "Guid");
                                bulkCopy.ColumnMappings.Add("ContainerNo", "ContainerNo");
                                bulkCopy.ColumnMappings.Add("Renban", "Renban");
                                bulkCopy.ColumnMappings.Add("SealNo", "SealNo");
                                bulkCopy.ColumnMappings.Add("Transport", "Transport");
                                bulkCopy.ColumnMappings.Add("ListcaseNo", "ListcaseNo");
                                bulkCopy.ColumnMappings.Add("DevanningDate", "DevanningDate");
                                bulkCopy.ColumnMappings.Add("DevanningTime", "DevanningTime");
                                bulkCopy.ColumnMappings.Add("GateInPlanDate", "GateInPlanDate");
                                bulkCopy.ColumnMappings.Add("GateInPlanTime", "GateInPlanTime");
                                bulkCopy.ColumnMappings.Add("CreationTime", "CreationTime");
                                bulkCopy.ColumnMappings.Add("CreatorUserId", "CreatorUserId");
                                bulkCopy.ColumnMappings.Add("LastModifierUserId", "LastModifierUserId");
                                bulkCopy.ColumnMappings.Add("IsDeleted", "IsDeleted");
                                bulkCopy.ColumnMappings.Add("DeleterUserId", "DeleterUserId");
                                bulkCopy.ColumnMappings.Add("RowNumber", "RowNumber");
                                bulkCopy.ColumnMappings.Add("ErrorDescription", "ErrorDescription");
                                bulkCopy.WriteToServer(table);
                                tran.Commit();
                            }
                        }
                        await conn.CloseAsync();
                    }
                }
                return listImport;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(400, ex.Message);
            }
        }

        //Merge Data 
        public async Task MergeDataContainerRentalWHPlan(string v_Guid)
        {
            string _sql = "Exec INV_CKD_CONTAINER_RETAL_WH_PLAN_MERGE @Guid";
            await _dapperRepo.QueryAsync<InvCkdContainerRentalWHPlanDto>(_sql, new { Guid = v_Guid });
        }


        public async Task<PagedResultDto<InvCkdContainerRentalWHPlErrDto>> GetMessageErrorImport(string v_Guid)
        {
            var data = await _dapperRepo.QueryAsync<InvCkdContainerRentalWHPlErrDto>("Exec FRM_ADO_INVENTORY_CKD_WH_PLAN_GET_LIST_ERROR_IMPORT @Guid", new { @Guid = v_Guid });
            var rsData = from o in data
                         select new InvCkdContainerRentalWHPlErrDto
                         {
                             ROW_NO = o.ROW_NO,
                             DevanningDate = o.DevanningDate,
                             DevanningTime = o.DevanningTime,
                             ContainerNo = o.ContainerNo,
                             Renban = o.Renban,
                             SealNo = o.SealNo,
                             Transport = o.Transport,
                             ErrorDescription = o.ErrorDescription
                         };

            var totalCount = rsData.Count();
            return new PagedResultDto<InvCkdContainerRentalWHPlErrDto>(
                totalCount,
                    rsData.ToList()
            );
        }

        public async Task<PagedResultDto<InvCkdContainerRentalWHPlanDetails>> GetAllDetails(GetInvCkdContainerRentalWHPlanDetailsInput input)
        {
            Dapper.SqlMapper.Settings.CommandTimeout = 900;
            string _sql = "Exec INV_CKD_CONTAINER_RENTAL_WH_PLAN_DETAILS_SEARCH @p_container_no";

            IEnumerable<InvCkdContainerRentalWHPlanDetails> result = await _dapperRepo.QueryAsync<InvCkdContainerRentalWHPlanDetails>(_sql, new
            {
                p_container_no = input.ContainerNo,
            });

            var pagedAndFiltered = result.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
            var totalCount = result.ToList().Count();

            return new PagedResultDto<InvCkdContainerRentalWHPlanDetails>(
                totalCount,
                pagedAndFiltered
            );
        }

        public async Task<FileDto> GetListErrContainerRentalWHPlanToExcel(string v_Guid)
        {
            var data = await _dapperRepo.QueryAsync<InvCkdContainerRentalWHPlErrDto>("Exec FRM_ADO_INVENTORY_CKD_WH_PLAN_GET_LIST_ERROR_IMPORT @Guid", new { @Guid = v_Guid });
            var rsData = from o in data
                         select new InvCkdContainerRentalWHPlErrDto
                         {
                             ROW_NO = o.ROW_NO,
                             DevanningDate = o.DevanningDate,
                             DevanningTime = o.DevanningTime,
                             ContainerNo = o.ContainerNo,
                             Renban = o.Renban,
                             SealNo = o.SealNo,
                             Transport = o.Transport,
                             ErrorDescription = o.ErrorDescription
                         };
            var exportToExcel = rsData.ToList();
            return _calendarListExcelExporter.ExportToFileErr(exportToExcel);
        }

        public async Task<Boolean> RepackPart(List<InvCkdContainerPartRepackInput> input, string ModuleNo)
        {
            var tableObj = _dbContextProvider.GetDbContext().InvCkdContainerRentalWhRepacks;
            var currDate = DateTime.Today;
            //// INSERT TO REPACK DATA
            var partRepacks = from field in input
                       select new InvCkdContainerRentalWhRepack
                       {
                           Exp = field.Exp,
                           Module = field.Module,
                           PartNo = field.PartNo,
                           LotNo = field.LotNo,
                           Qty = field.Qty,
                           PackingDate = currDate,
                           RepackModuleNo = ModuleNo,
                           Status = false
                       };

            tableObj.AddRange(partRepacks);

            // input là list dto, dto có property Source: 1 là Source từ bảng InvCkdInvoiceDetailss, 2 là Source từ bảng InvCkdContainerRentalWhRepacks
            
            // lấy tương ứng với Source = 1, update lại RemainQty trong bảng InvCkdInvoiceDetailss
            var listChangedIds1 = input.Where(x => x.Source == 1).Select(x => x.Id).ToList();

            var partDb1 = await _dbContextProvider.GetDbContext().InvCkdInvoiceDetailss
                .Where(x => listChangedIds1.Contains(x.Id) && (x.RemainQty > 0 || x.RemainQty == null)).ToListAsync()
                ;

            // source = 2, update lại Qty trong bảng InvCkdContainerRentalWhRepacks
            var listChangedIds2 = input.Where(x => x.Source == 2).Select(x => x.Id).ToList();
            var partDb2 = await _dbContextProvider.GetDbContext().InvCkdContainerRentalWhRepacks
                .Where(x => listChangedIds2.Contains(x.Id)).ToListAsync()
                ;

            foreach (var item in partDb1)
            {
                item.RemainQty ??= item.UsageQty;
                item.RemainQty -= input.Where(x => x.Id == item.Id).First().Qty;
            }

            //foreach (var item in partDb2)
            //{
            //    item.RemainQty ??= item.Qty;
            //    item.RemainQty -= input.Where(x => x.Id == item.Id).First().Qty;
            //}

            await _dbContextProvider.GetDbContext().SaveChangesAsync();
            
            return true;
        }

        public async Task<PagedResultDto<InvCkdContainerRentalWhRepack>> GetAllRepack(GetInvCkdContainerRentalRepackInput input)
        {
            var data = await _repackRepo.GetAll().OrderBy(x => x.Status).ToListAsync();

            var pagedAndFiltered = data.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();
            var totalCount = data.ToList().Count();

            return new PagedResultDto<InvCkdContainerRentalWhRepack>(
                totalCount,
                pagedAndFiltered
            );
        }

        public async Task<FileDto> GetAllRepackToExcel()
        {

            string contentRootPath = "/Template/InvCkdContainerRentalWHRepack_Template.xlsx";
            string webRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot") + contentRootPath;
            string pathExcelTemp = webRootPath;
            string pathExcel = "/Download/";
            string nameExcel = "InvCkdContainerRentalWHRepack_Template" + DateTime.Now.ToString("MMddyyyy-HHmmss") + ".xlsx";
            string pathDownload = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot") + pathExcel + nameExcel;
            var fileDto = new FileDto(nameExcel, MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet);


            FileInfo finfo = new FileInfo(pathDownload);
            if (finfo.Exists) { try { finfo.Delete(); } catch (Exception ex) { } }

            XSSFWorkbook xlsxObject = null;     //XLSX
            ISheet sheet = null;
            IRow row = null;

            ICell cell = null;
            CellRangeAddress range = null;


            CellReference cr = null;
            using (FileStream file = new FileStream(pathExcelTemp, FileMode.Open, FileAccess.Read))
            {
                xlsxObject = new XSSFWorkbook(file);
            }

            // Lấy Object Sheet  
            sheet = xlsxObject.GetSheetAt(0);
            if (sheet == null) { return null; }

            IFont boldFont = xlsxObject.CreateFont();
            boldFont.IsBold = true;

            ICellStyle istyle = xlsxObject.CreateCellStyle();
            istyle.FillPattern = FillPattern.SolidForeground;
            istyle.FillForegroundColor = IndexedColors.White.Index;
            istyle.BorderBottom = BorderStyle.Thin;
            istyle.BorderTop = BorderStyle.Thin;
            istyle.BorderLeft = BorderStyle.Thin;
            istyle.BorderRight = BorderStyle.Thin;


            try
            {

                var Result = await _dbContextProvider.GetDbContext().InvCkdContainerRentalWhRepacks
                    .Where(x => x.Status == false)
                    .GroupBy(x => new { x.Exp, x.RepackModuleNo, x.Module })
                    .Select(x => x.First())
                    .ToListAsync();

                int r = 2;

                if (Result.Count > 0)
                {

                    for (int i = 0; i < Result.Count; i++)
                    {
                        row = sheet.CreateRow(r);
                        // Exp
                        cell = row.CreateCell(0);
                        cell.CellStyle = istyle;
                        cell.SetCellValue(Result[i].Exp);

                        cell = row.CreateCell(1);
                        cell.CellStyle = istyle;
                        cell.SetCellValue(Result[i].Module);

                        cell = row.CreateCell(2);
                        cell.CellStyle = istyle;
                        cell.SetCellValue(Result[i].RepackModuleNo);

                        // fill style
                        for (int j = 3; j <= 8; j++)
                        {
                            cell = row.CreateCell(j);
                            cell.CellStyle = istyle;
                        }

                        r++;

                    }

                }

                FileStream xfile = new FileStream(pathDownload, FileMode.Create, System.IO.FileAccess.Write);
                xlsxObject.Write(xfile);
                xfile.Dispose();


                MemoryStream downloadStream = new MemoryStream(File.ReadAllBytes(pathDownload));
                _tempFileCacheManager.SetFile(fileDto.FileToken, downloadStream.ToArray());
                File.Delete(pathDownload);
                downloadStream.Position = 0;

            }
            catch (Exception ex)
            {

            }
            return await Task.FromResult(fileDto);

        }

        public async Task<List<InvCkdContainerRentalWHPlanDetailsImportDto>> ImportRepackTransferFromExcel(byte[] fileBytes, string fileName)
        {
            try
            {
                List<InvCkdContainerRentalWHPlanDetailsImportDto> listImport = new();
                using (var stream = new MemoryStream(fileBytes))
                {
                    SpreadsheetInfo.SetLicense("EF21-1FW1-HWZF-CLQH");
                    var xlWorkBook = ExcelFile.Load(stream);
                    string strGUID = Guid.NewGuid().ToString("N");

                    ExcelWorksheet v_worksheet_p1 = xlWorkBook.Worksheets[0];

                    var dateFormat = "dd/MM/yyyy hh:mm:ss";
                    int row, col;

                    row = 2;
                    col = 0; 
                    while (true)
                    {
                        if (v_worksheet_p1.Cells[row, col].Value == null)
                        {
                            break;
                        }

                        var rowObj = new InvCkdContainerRentalWHPlanDetailsImportDto();
                        rowObj.Guid = strGUID;
                        rowObj.Exp = v_worksheet_p1.Cells[row, col].Value?.ToString();
                        rowObj.Module = v_worksheet_p1.Cells[row, col + 1].Value?.ToString() ?? "";
                        rowObj.RepackModule = v_worksheet_p1.Cells[row, col + 2].Value?.ToString() ?? "";
                        rowObj.Container = v_worksheet_p1.Cells[row, col + 3].Value?.ToString() ?? "";
                        rowObj.WHCurrrent = v_worksheet_p1.Cells[row, col + 4].Value?.ToString() ?? "";
                        rowObj.WHNew = v_worksheet_p1.Cells[row, col + 5].Value?.ToString() ?? "";
                        rowObj.Shift = Convert.ToInt32(v_worksheet_p1.Cells[row, col + 7].Value.ToString());


                        /// datetime
                        DateTime receiveDate;
                        if (!DateTime.TryParse(v_worksheet_p1.Cells[row, col + 6].Value?.ToString(), out receiveDate))
                        {
                            throw new UserFriendlyException(400, "Ngày nhận không hợp lệ!");

                        }
                        var receiveTime = Convert.ToDateTime(v_worksheet_p1.Cells[row, col + 8].Value?.ToString()).TimeOfDay;
                        receiveDate = receiveDate.AddHours(receiveTime.Hours).AddMinutes(receiveTime.Minutes);
                        /// 


                        rowObj.ReceiveDateTime = receiveDate;

                        if (rowObj.RepackModule.Length > 6)
                        {
                            rowObj.ErrorDescription = "Độ dài Module No: " + rowObj.RepackModule + " không hợp lệ! , ";
                        }

                        listImport.Add(rowObj);
                        row++;
                    }
                }


                // set tất cả bản ghi có cùng repackModuleNo thành dữ liệu từ listImport
                foreach (var item in listImport)
                {
                    var importData = _dbContextProvider.GetDbContext().InvCkdContainerRentalWhRepacks
                        .Where(x => x.Module == item.Module)
                        .Where(x => x.RepackModuleNo == item.RepackModule)
                        //.Where(x => x.Status == false)
                        .ToList();

                   // update import data tự theo listImport
                   foreach (var importItem in importData)
                    {
                        importItem.Container = item.Container;
                        importItem.WHCurrent = item.WHCurrrent;
                        importItem.WHNew = item.WHNew;
                        importItem.Shift = item.Shift;
                        importItem.ReceiveDateTime = item.ReceiveDateTime;
                        importItem.Status = true;
                    }
                }

                await _dbContextProvider.GetDbContext().SaveChangesAsync();


                return listImport;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(400, ex.Message);
            }
        }
    }
}
