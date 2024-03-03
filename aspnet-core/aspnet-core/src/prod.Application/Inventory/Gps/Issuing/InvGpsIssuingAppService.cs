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
using Microsoft.EntityFrameworkCore.Storage;
using Newtonsoft.Json;
using NPOI.SS.Formula.Functions;
using NPOI.SS.UserModel;
using prod;
using prod.Authorization;
using prod.Common;
using prod.Dto;
using prod.EntityFrameworkCore;
using prod.Inventory.CKD.Dto;
using prod.Inventory.GPS;
using prod.Inventory.GPS.Dto;
using prod.Inventory.GPS.Exporting;
using prod.Master.Common.Dto;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace vovina.Inventory.GPS
{
    [AbpAuthorize(AppPermissions.Pages_GPS_Issuing_View)]
    public class InvGpsIssuingAppService : prodAppServiceBase, IInvGpsIssuingAppService
    {
        private readonly IDapperRepository<InvGpsIssuing, long> _dapperRepo;
        private readonly IRepository<InvGpsIssuing, long> _repo;
        private readonly IInvGpsIssuingExcelExporter _calendarListExcelExporter;

        public InvGpsIssuingAppService(IRepository<InvGpsIssuing, long> repo,
                                         IDapperRepository<InvGpsIssuing, long> dapperRepo,
                                        IInvGpsIssuingExcelExporter calendarListExcelExporter
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
        }

        [AbpAuthorize(AppPermissions.Pages_GPS_Issuing_Edit)]
        public async Task CreateOrEdit(CreateOrEditInvGpsIssuingDto input)
        {
            if (input.Id == null) await Create(input);
            else await Update(input);
        }

        //CREATE
        private async Task Create(CreateOrEditInvGpsIssuingDto input)
        {
            var mainObj = ObjectMapper.Map<InvGpsIssuing>(input);

            await CurrentUnitOfWork.GetDbContext<prodDbContext>().AddAsync(mainObj);
        }

        // EDIT
        private async Task Update(CreateOrEditInvGpsIssuingDto input)
        {
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.SoftDelete))
            {
                var mainObj = await _repo.GetAll()
                .FirstOrDefaultAsync(e => e.Id == input.Id);
                
                var mainObjToUpdate = ObjectMapper.Map(input, mainObj);
            }
        }

        public async Task<GpsIssuingRequestCheckDto> GpsIssuingRequestCheck(string v_PartNo)
        {
            string _sql = "Exec INV_GPS_ISSUING_REQUEST_CHECK @p_partno";
                IEnumerable<GpsIssuingRequestCheckDto> result = await _dapperRepo.QueryAsync<GpsIssuingRequestCheckDto>(_sql, new { p_partno = v_PartNo });
            return result.FirstOrDefault();
        }

        [AbpAuthorize(AppPermissions.Pages_GPS_Issuing_Edit)]
        public async Task Delete(EntityDto input)
        {
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.SoftDelete))
            {
                var mainObj = await _repo.FirstOrDefaultAsync(input.Id);
                _repo.HardDelete(mainObj);
                CurrentUnitOfWork.GetDbContext<prodDbContext>().Remove(mainObj);
            }
       
        }


        public async Task<PagedResultDto<InvGpsIssuingDto>> GetAll(GetInvGpsIssuingInput input)
        {
            string _sql = "Exec INV_GPS_ISSUING_SEARCH @p_partno, @p_LotNo, @p_expDate_from ,@p_expDate_to ,@p_issueDate_from ,@p_issueDate_to , @p_today, @p_status, @p_isGentani ";

            IEnumerable<InvGpsIssuingDto> result = await _dapperRepo.QueryAsync<InvGpsIssuingDto>(_sql, new
            {
                p_partno = input.PartNo,
                p_LotNo = input.LotNo,
                p_expDate_from = input.ReqDateFrom,
                p_expDate_to = input.ReqDateTo,
                p_issueDate_from = input.IssueDateFrom,
                p_issueDate_to = input.IssueDateTo,
                p_today = input.Today,
                p_status = input.Status,
                p_isGentani = input.IsGentani
            });

            var listResult = result.ToList();

            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var totalCount = result.ToList().Count();

            return new PagedResultDto<InvGpsIssuingDto>(
               totalCount,
               pagedAndFiltered);
        }
        public async Task<FileDto> GetStockIssuingToExcel(GetInvGpsIssuingInput input)
    {
            string _sql = "Exec INV_GPS_ISSUING_SEARCH @p_partno, @p_LotNo, @p_expDate_from ,@p_expDate_to ,@p_issueDate_from ,@p_issueDate_to , @p_status, @p_isGentani ";

            IEnumerable<InvGpsIssuingDto> result = await _dapperRepo.QueryAsync<InvGpsIssuingDto>(_sql, new
            {
                p_partno = input.PartNo,
                p_LotNo = input.LotNo,
                p_expDate_from = input.ReqDateFrom,
                p_expDate_to = input.ReqDateTo,
                p_issueDate_from = input.IssueDateFrom,
                p_issueDate_to = input.IssueDateTo,
                p_status = input.Status,
                p_isGentani = input.IsGentani
            });

            var listResult = result.ToList();

        return _calendarListExcelExporter.ExportToFile(listResult);
    }




        public async Task<List<GetInvGesIssuingImport>> ImportDataInvGpsIssuingListFromExcel(byte[] fileBytes, string fileName)
        {
            try
            {
                List<GetInvGesIssuingImport> listImport = new List<GetInvGesIssuingImport>();
                using (var stream = new MemoryStream(fileBytes))
                {
                    SpreadsheetInfo.SetLicense("EF21-1FW1-HWZF-CLQH");
                    var xlWorkBook = ExcelFile.Load(stream);
                    var v_worksheet = xlWorkBook.Worksheets[0];
                    DataFormatter formatter = new DataFormatter();
                    DateTime dateTime = DateTime.Now;

                    string strGUID = Guid.NewGuid().ToString("N");
                    string _sql = @"EXEC CMM_IMPORT_USER_GUID_INSERT @Guid, @p_UserId";
                    _dapperRepo.ExecuteAsync(_sql, new
                    {
                        Guid = strGUID,
                        p_UserId = AbpSession.UserId
                    });

                    string v_receiveDate = (v_worksheet.Cells[1, 1]).Value?.ToString() ?? "";
                    string v_supplier = (v_worksheet.Cells[1, 4]).Value?.ToString() ?? "";

                    for (int i = 3; i < v_worksheet.Rows.Count; i++)
                    {

                        string v_partNo = (v_worksheet.Cells[i, 1]).Value?.ToString() ?? "";

                        if (v_partNo != "")
                        {

                            string v_PartName = (v_worksheet.Cells[i, 2]).Value?.ToString() ?? "";
                            string v_Oum = (v_worksheet.Cells[i, 3]).Value?.ToString() ?? "";
                            string v_BoxQty = "";
                            string v_Box = "";
                            string v_LotNo = "";
                            string v_ProdDate = "";
                            string v_ExpDate = "";
                            string v_Shop = "";
                            string v_QtyRequest = "";
                            string v_CosCenter = "";
                           

                            string checkType = (v_worksheet.Cells[2, 10]).Value?.ToString() ?? "";
                            string v_checkType = checkType.Length > 0 ? checkType.Substring(0, 4) : "";
                            if (v_checkType != "Shop")
                            {
                               v_QtyRequest = (v_worksheet.Cells[i, 4]).Value?.ToString() ?? "";
                               v_CosCenter = (v_worksheet.Cells[i, 5]).Value?.ToString() ?? "";
                                

                            }
                            else
                            {
                                v_BoxQty = (v_worksheet.Cells[i, 4]).Value?.ToString() ?? "";
                                v_Box = (v_worksheet.Cells[i, 5]).Value?.ToString() ?? "";
                                v_QtyRequest = (v_worksheet.Cells[i, 6]).Value?.ToString() ?? "";
                                v_LotNo = (v_worksheet.Cells[i, 7]).Value?.ToString() ?? "";
                                v_ProdDate = (v_worksheet.Cells[i, 8]).Value?.ToString() ?? "";
                                v_ExpDate = (v_worksheet.Cells[i, 9]).Value?.ToString() ?? "";
                                v_Shop = (v_worksheet.Cells[i, 10]).Value?.ToString() ?? "";
                            }
                           
                         
                            GetInvGesIssuingImport row = new GetInvGesIssuingImport();
                            row.Guid = strGUID;

                            //PartNo
                            int dot = v_partNo.LastIndexOf('.');
                            if (dot != -1)
                            {
                                v_partNo = v_partNo.Substring(0, dot);
                            }
                            if (v_partNo.Length > 12)
                            {
                                row.ErrorDescription += "PartNo " + v_partNo + " dài quá 12 kí tự! ";
                            }
                            else
                            {
                                row.PartNo = v_partNo;
                            }
                            //PartName
                            if (string.IsNullOrEmpty(v_PartName))
                            {
                                row.ErrorDescription += "PartName khác null!";
                            }
                            else
                            {
                                row.PartName = v_PartName;
                            }

                            if (string.IsNullOrEmpty(v_Oum))
                            {
                                row.ErrorDescription += "Đơn vị khác null!";
                            }
                            else
                            {
                                row.Uom = v_Oum;
                            }

                            if (v_checkType == "Shop")
                            {
                                //ReceivedDate
                                try
                                {
                                    if (string.IsNullOrEmpty(v_receiveDate))
                                    {
                                        row.ErrorDescription += "Ngày giao không được để trống! ";
                                    }
                                    else
                                    {
                                        row.ReceivedDate = DateTime.Parse(v_receiveDate);
                                        if (row.ProdDate.HasValue &&
                                            row.ProdDate > row.ReceivedDate)
                                        {
                                            row.ErrorDescription += " Ngày giao phải sau Ngày sản xuất";
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    row.ErrorDescription += "Ngày giao " + v_receiveDate + " không đúng định dạng! ";
                                }
                                //Supplier
                                if (v_supplier.Length > 50)
                                {
                                    row.ErrorDescription += "Supplier " + v_supplier + " dài quá 50 kí tự! ";
                                }
                                else
                                {
                                    row.Supplier = v_supplier;
                                }
                                //Boxqty
                                try
                                {
                                    if (string.IsNullOrEmpty(v_BoxQty))
                                    {
                                        row.ErrorDescription += "BoxQty không được để trống!";
                                    }
                                    else
                                    {

                                        if (Convert.ToInt32(v_BoxQty) < 0)
                                        {
                                            row.ErrorDescription += "BoxQty phải là số dương! ";
                                        }
                                        row.Boxqty = Convert.ToInt32(v_BoxQty);


                                    }
                                }
                                catch (Exception ex)
                                {
                                    row.ErrorDescription += "Box không phải là số! ";
                                }

                                //Box
                                try
                                {
                                    if (string.IsNullOrEmpty(v_Box))
                                    {
                                        row.ErrorDescription += "Box không được để trống!";
                                    }
                                    else
                                    {

                                        if (Convert.ToInt32(v_Box) < 0)
                                        {
                                            row.ErrorDescription += "Box phải là số dương! ";
                                        }
                                        row.Box = Convert.ToInt32(v_Box);


                                    }
                                }
                                catch (Exception ex)
                                {
                                    row.ErrorDescription += "Box không phải là số! ";
                                }

                                //LotNo
                                if (string.IsNullOrEmpty(v_LotNo))
                                {
                                    row.ErrorDescription += "LotNo khác null!";
                                }
                                else
                                {
                                    row.LotNo = v_LotNo;
                                }

                                //ProdDate
                                try
                                {
                                    if (string.IsNullOrEmpty(v_ProdDate))
                                    {
                                        row.ErrorDescription += "Ngày sản xuất không được để trống! ";
                                    }
                                    else
                                    {
                                        row.ProdDate = DateTime.Parse(v_ProdDate);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    row.ErrorDescription += "Ngày sản xuất " + v_ProdDate + " không đúng định dạng! ";
                                }

                                //ExpDate
                                try
                                {
                                    if (string.IsNullOrEmpty(v_ExpDate))
                                    {
                                        row.ErrorDescription += "Ngày hết hạn không được để trống! ";
                                    }
                                    else
                                    {
                                       
                                        if (row.ProdDate.HasValue &&
                                            row.ProdDate > row.ExpDate)
                                        {
                                            row.ErrorDescription += "Ngày hết hạn phải sau Ngày sản xuất";
                                        }
                                        else if (DateTime.Now > row.ExpDate)
                                        {
                                            row.ErrorDescription += "Sản phẩm đã hết hạn sản xuất! ";
                                        }
                                        else {
                                            row.ExpDate = DateTime.Parse(v_ExpDate);
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    row.ErrorDescription += "Ngày hết hạn " + v_ExpDate + " không đúng định dạng! ";
                                }


                                //LotNo
                                if (string.IsNullOrEmpty(v_Shop))
                                {
                                    row.ErrorDescription += "LotNo khác null!";
                                }
                                else
                                {
                                    row.LotNo = v_Shop;
                                }

                                //IsGentani
                                row.IsGentani = "Y";
                            }
                            //QtyRequest
                            try
                            {
                                if (string.IsNullOrEmpty(v_QtyRequest))
                                {
                                    row.ErrorDescription += "QtyRequest không được để trống!";
                                }
                                else
                                {

                                    if (Convert.ToInt32(v_QtyRequest) < 0)
                                    {
                                        row.ErrorDescription += "QtyRequest phải là số dương! ";
                                    }
                                    row.QtyRequest = Convert.ToInt32(v_QtyRequest);


                                }
                            }
                            catch (Exception ex)
                            {
                                row.ErrorDescription += "QtyRequest không phải là số! ";
                            }

                            if (v_checkType != "Shop")
                            {
                                //CostCenter
                                try
                                {
                                    if (string.IsNullOrEmpty(v_CosCenter))
                                    {
                                        row.ErrorDescription += "CosCenter không được để trống!";
                                    }
                                    else
                                    {
                                        row.CostCenter = v_CosCenter;


                                    }
                                }
                                catch (Exception ex)
                                {
                                    row.ErrorDescription += "CosCenter không phải là số! ";
                                }
                                //IsGentani 
                                row.IsGentani = "N";
                                //Request Date
                                try
                                {
                                    if (string.IsNullOrEmpty(v_receiveDate))
                                    {
                                        row.ErrorDescription += "Ngày giao không được để trống! ";
                                    }
                                    else
                                    {
                                        row.ReceivedDate = DateTime.Parse(v_receiveDate);
                                        if (row.ProdDate.HasValue &&
                                            row.ProdDate > row.ReceivedDate)
                                        {
                                            row.ErrorDescription += " Ngày giao phải sau Ngày sản xuất";
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    row.ErrorDescription += "Ngày giao " + v_receiveDate + " không đúng định dạng! ";
                                }
                            }

                            listImport.Add(row);
                

                        }

                    }
                    // import temp into db (bulkCopy)
                    if (listImport.Count > 0)
                    {
                        IEnumerable<GetInvGesIssuingImport> dataE = listImport.AsEnumerable();
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
                                    bulkCopy.DestinationTableName = "InvGpsIssuing_T";
                                    bulkCopy.ColumnMappings.Add("Guid", "Guid");
                                    bulkCopy.ColumnMappings.Add("PartNo", "PartNo");
                                    bulkCopy.ColumnMappings.Add("PartName", "PartName");
                                    bulkCopy.ColumnMappings.Add("Uom", "Uom");
                                    bulkCopy.ColumnMappings.Add("BoxQty", "BoxQty");
                                    bulkCopy.ColumnMappings.Add("Box", "Box"); 
                                    bulkCopy.ColumnMappings.Add("QtyRequest", "QtyRequest");
                                    bulkCopy.ColumnMappings.Add("QtyIssue", "QtyIssue");
                                    bulkCopy.ColumnMappings.Add("LotNo", "LotNo");
                                    bulkCopy.ColumnMappings.Add("ProdDate", "ProdDate");
                                    bulkCopy.ColumnMappings.Add("ExpDate", "ExpDate");
                                    bulkCopy.ColumnMappings.Add("IsGentani", "IsGentani");
                                    bulkCopy.ColumnMappings.Add("CostCenter", "CostCenter");
                                    bulkCopy.ColumnMappings.Add("Supplier", "Supplier");
                                    bulkCopy.ColumnMappings.Add("ReceivedDate", "ReceivedDate");                                   
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
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(400, ex.Message);
            }
        }


       
      
        public async Task MergeDataInvGpsIssuing(string v_Guid,string type)
        {
            if(type == "request")
            {
                string _sql = "Exec INV_GPS_ISSUE_REQUEST_MERGE @Guid";
                await _dapperRepo.QueryAsync<GetInvGesIssuingImport>(_sql, new { Guid = v_Guid });
            }
            else
            {
                string _sql = "Exec INV_GPS_ISSUE_GENTANI_MERGE @Guid";
                await _dapperRepo.QueryAsync<GetInvGesIssuingImport>(_sql, new { Guid = v_Guid });
            }
          
        }

        // hiển thị lỗi import và export lỗi
        public async Task<PagedResultDto<GetInvGesIssuingImport>> GetMessageErrorImport(string v_Guid)
        {
            string _sql = "Exec INV_GPS_ISSUE_GET_LIST_ERROR_IMPORT @Guid";

            IEnumerable<GetInvGesIssuingImport> result = await _dapperRepo.QueryAsync<GetInvGesIssuingImport>(_sql, new
            {
                Guid = v_Guid
            });

            var listResult = result.ToList();
            var totalCount = listResult.Count();

            return new PagedResultDto<GetInvGesIssuingImport>(
                totalCount,
               listResult
               );
        }



        public async Task<int> ConFirmStatusMultiCkb(string listIdStatus, string status)
        {
            try
            {
                string _sql = @"EXEC [INV_GPS_ISSUING_UPDATE_STATUS] @p_list_id_status,@p_status,@p_status_date, @p_user_id";
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

        /*public async Task<FileDto> GetStockIssuingToExcel(InvGpsStockIssuingExportInput input)
            {
                var query = from o in _repo.GetAll()
                            select new InvGpsStockIssuingDto
                            {
                                Id = o.Id,
                                PartNo = o.PartNo,
                                PartName = o.PartName,
                                Oum = o.Oum,
                                Boxqty = o.Boxqty,
                                Box = o.Box,
                                Qty = o.Qty,
                                LotNo = o.LotNo,
                                ProdDate = o.ProdDate,
                                ExpDate = o.ExpDate,
                                ReceivedDate = o.ReceivedDate,
                                Supplier = o.Supplier,
                                CostCenter = o.CostCenter,
                                QtyIssue = o.QtyIssue,
                                IsIssue = o.IsIssue,
                                Status = o.Status,
                                IsGentani = o.IsGentani,
                            };
                var exportToExcel = await query.ToListAsync();
                return _calendarListExcelExporter.ExportToFile(exportToExcel);
            }
    */
        // public async Task GenerateAsync()
        //  {
        //    await _dapperRepo.ExecuteAsync(InvGpsStockIssuingConsts.SP_MST_WPT_CALENDAR_GENERATE);
        // }

        public async Task<MstCmmLookupDto> GetItemValue(string p_DomainCode, string p_ItemCode)
        {

            string _sql = "Exec MST_CMN_LOOKUP_GET_ITEM_VALUE @DomainCode, @ItemCode";

            var filtered = await _dapperRepo.QueryAsync<MstCmmLookupDto>(_sql, new
            {
                DomainCode = p_DomainCode,
                ItemCode = p_ItemCode
            });

            return filtered.FirstOrDefault();
        }

        public async Task<int> CheckBudget(string listIdStatus)
        {
            try
            {
                string _sql = @"EXEC [INV_GPS_ISSUING_UPDATE_STATUS] @p_list_id_status,@p_status,@p_status_date, @p_user_id";
                var filtered = await _dapperRepo.ExecuteAsync(_sql, new
                {
                    p_list_id_status = listIdStatus,
                    p_status = "",
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


        public async Task TestApi()
        {
            using (var client = new HttpClient())
            {
                string ServerUrlEsignBase = "http://localhost:5500/";
                LoginTestDto dto = new LoginTestDto();
                dto.UserName = "admin";
                dto.TenancyName = "TMV";
                dto.Password = "123qwe";
                string url = ServerUrlEsignBase;
                var inputJson = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");
                var response = await client.PostAsync($"http://192.168.5.17:5500/api/TokenAuth/Login", inputJson);
                var x = response.Content.ReadAsStringAsync().Result;
                if (x != "")
                    throw new UserFriendlyException(x);

            }
        }

    }
}

