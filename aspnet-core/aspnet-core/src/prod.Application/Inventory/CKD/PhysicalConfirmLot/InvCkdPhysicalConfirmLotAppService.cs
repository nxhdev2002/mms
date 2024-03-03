using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Dapper.Repositories;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Abp.UI;
using FastMember;
using GemBox.Spreadsheet;
using Microsoft.EntityFrameworkCore;
using NPOI.SS.UserModel;
using prod.Authorization;
using prod.Dto;
using prod.Inventory.CKD.Dto;
using prod.Inventory.CKD.PhysicalConfirmLot.ExportingErr;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using prod.Common;

namespace prod.Inventory.CKD
{
    [AbpAuthorize(AppPermissions.Pages_Ckd_Physical_PhysicalConfirmLot_View)]
    public class InvCkdPhysicalConfirmLotAppService : prodAppServiceBase, IInvCkdPhysicalConfirmLotAppService
    {
        private readonly IDapperRepository<InvCkdPhysicalConfirmLot, long> _dapperRepo;
        private readonly IRepository<InvCkdPhysicalConfirmLot, long> _repo;
        private readonly IInvCkdPhysicalConfirmLotExcelExporter _calendarListExcelExporter;


        public InvCkdPhysicalConfirmLotAppService(IRepository<InvCkdPhysicalConfirmLot, long> repo,
                                         IDapperRepository<InvCkdPhysicalConfirmLot, long> dapperRepo,
                                         IInvCkdPhysicalConfirmLotExcelExporter calendarListExcelExporter
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
        }

        public async Task<PagedResultDto<InvCkdPhysicalConfirmLotDto>> GetAll(GetInvCkdPhysicalConfirmLotInput input)
        {
            string _sql = "Exec INV_CKD_PHYSICAL_CONFIRM_LOT_SEARCH @p_ModelCode, @p_ProdLine, @p_Grade";

            IEnumerable<InvCkdPhysicalConfirmLotDto> result = await _dapperRepo.QueryAsync<InvCkdPhysicalConfirmLotDto>(_sql, new
            {
                p_ModelCode = input.ModelCode,
                p_ProdLine = input.ProdLine,
                p_Grade = input.Grade,
            });

            var listResult = result.ToList();

            var totalCount = result.ToList().Count();
            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            return new PagedResultDto<InvCkdPhysicalConfirmLotDto>(
                totalCount,
                pagedAndFiltered);

        }

        [AbpAuthorize(AppPermissions.Pages_Ckd_Physical_PhysicalConfirmLot_Import)]
        public async Task<List<InvCkdPhysicalConfirmLot_TDto>> ImportDataInvCkdPhysicalConfirmFromExcel(byte[] fileBytes, string fileName)
        {
            try
            {
                List<InvCkdPhysicalConfirmLot_TDto> listImport = new List<InvCkdPhysicalConfirmLot_TDto>();
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

                    for (int c = 0; c < v_worksheet.Rows.Count; c++)
                    {
                        string AW = (v_worksheet.Cells[c, 0]).Value?.ToString() ?? "";
                        if ((AW.Length > 0 ? AW.Substring(0, 1) : "") == "A")
                        {
                            for (int i = 4 + c; i < v_worksheet.Rows.Count; i++)
                            {
                                string v_ProdLine = (v_worksheet.Cells[i, 1]).Value?.ToString() ?? "";
                                string v_ModelCode = (v_worksheet.Cells[i, 0]).Value?.ToString() ?? "";
                                string v_Grade = (v_worksheet.Cells[i, 2]).Value?.ToString() ?? "";

                                    if (v_ProdLine != "" && v_ModelCode != "" && v_Grade != "")
                                    {
                                        var row = new InvCkdPhysicalConfirmLot_TDto();
                                        row.Guid = strGUID;
                                        row.ErrorDescription = "";
                                        row.ModelCode = v_ModelCode;
                                        row.ProdLine = "A";
                                        row.Grade = v_Grade;
                                        //check StartLot
                                        try
                                        {
                                            row.StartLot = Convert.ToInt32((v_worksheet.Cells[i, 3]).Value?.ToString() ?? "0");
                                            if (row.StartLot < 0)
                                            {
                                                row.ErrorDescription = "Start Lot phải là số dương! ";
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            row.ErrorDescription += "Start Lot phải là số! ";
                                        }
                                        //Check StartRun
                                        try
                                        {
                                            row.StartRun = Convert.ToInt32((v_worksheet.Cells[i, 4]).Value?.ToString() ?? "0");
                                            if (row.StartRun < 0)
                                            {
                                                row.ErrorDescription += "Start Run phải là số dương! ";
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            row.ErrorDescription += "Start Run phải là số! ";
                                        }
                                        // validate devaning date
                                        try
                                        {
                                            var a = (v_worksheet.Cells[2, 1]).Value?.ToString();
                                            row.StartProcessDate = DateTime.Parse((v_worksheet.Cells[2, 1]).Value?.ToString() ?? "");
                                        }
                                        catch
                                        {
                                            row.ErrorDescription += "Ngày nhận không hợp lệ! ";
                                        }
                                        //Check EndLot
                                        try
                                        {
                                            row.EndLot = Convert.ToInt32((v_worksheet.Cells[i, 6]).Value?.ToString() ?? "0");
                                            if (row.EndLot < 0)
                                            {
                                                row.ErrorDescription += "End Lot phải là số dương! ";
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            row.ErrorDescription += "End Lot phải là số! ";
                                        }
                                        //Check EndRun
                                        try
                                        {
                                            row.EndRun = Convert.ToInt32((v_worksheet.Cells[i, 7]).Value?.ToString() ?? "0");
                                            if (row.EndRun < 0)
                                            {
                                                row.ErrorDescription += "End Run phải là số dương! ";
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            row.ErrorDescription += "End Run phải là số! ";
                                        }
                                        // validate devaning date
                                        try
                                        {
                                            row.EndProcessDate = DateTime.Parse((v_worksheet.Cells[2, 6]).Value?.ToString() ?? "");
                                        }
                                        catch
                                        {
                                            row.ErrorDescription += "Ngày nhận không hợp lệ! ";
                                        }

                                        listImport.Add(row);
                                    }
                                
                                else
                                {
                                    break;
                                }

                                c = c + 1;

                            }
                        }
                        else if ((AW.Length > 0 ? AW.Substring(0, 1) : "") == "C")
                        {
                            for (int i = 4 + c; i < v_worksheet.Rows.Count; i++)
                            {
                                string v_ProdLine = (v_worksheet.Cells[i, 1]).Value?.ToString() ?? "";
                                string v_ModelCode = (v_worksheet.Cells[i, 0]).Value?.ToString() ?? "";
                                string v_Grade = (v_worksheet.Cells[i, 2]).Value?.ToString() ?? "";

                                    if (v_ProdLine != "" && v_ModelCode != "" && v_Grade != "")
                                    {
                                        var row = new InvCkdPhysicalConfirmLot_TDto();
                                        row.Guid = strGUID;
                                        row.ErrorDescription = "";
                                        row.ModelCode = v_ModelCode;
                                        row.ProdLine = "W";
                                        row.Grade = v_Grade;
                                        //check StartLot
                                        try
                                        {
                                            row.StartLot = Convert.ToInt32((v_worksheet.Cells[i, 3]).Value?.ToString() ?? "0");
                                            if (row.StartLot < 0)
                                            {
                                                row.ErrorDescription = "Start Lot phải là số dương! ";
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            row.ErrorDescription += "Start Lot phải là số! ";
                                        }
                                        //Check StartRun
                                        try
                                        {
                                            row.StartRun = Convert.ToInt32((v_worksheet.Cells[i, 4]).Value?.ToString() ?? "0");
                                            if (row.StartRun < 0)
                                            {
                                                row.ErrorDescription += "Start Run phải là số dương! ";
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            row.ErrorDescription += "Start Run phải là số! ";
                                        }
                                        // validate devaning date
                                        try
                                        {
                                            row.StartProcessDate = DateTime.Parse((v_worksheet.Cells[2, 1]).Value?.ToString() ?? "");
                                        }
                                        catch
                                        {
                                            row.ErrorDescription += "Ngày nhận không hợp lệ! ";
                                        }
                                        //Check EndLot
                                        try
                                        {
                                            row.EndLot = Convert.ToInt32((v_worksheet.Cells[i, 6]).Value?.ToString() ?? "0");
                                            if (row.EndLot < 0)
                                            {
                                                row.ErrorDescription += "End Lot phải là số dương! ";
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            row.ErrorDescription += "End Lot phải là số! ";
                                        }
                                        //Check EndRun
                                        try
                                        {
                                            row.EndRun = Convert.ToInt32((v_worksheet.Cells[i, 7]).Value?.ToString() ?? "0");
                                            if (row.EndRun < 0)
                                            {
                                                row.ErrorDescription += "End Run phải là số dương! ";
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            row.ErrorDescription += "End Run phải là số! ";
                                        }
                                        // validate devaning date
                                        try
                                        {
                                            row.EndProcessDate = DateTime.Parse((v_worksheet.Cells[2, 6]).Value?.ToString() ?? "");
                                        }
                                        catch
                                        {
                                            row.ErrorDescription += "Ngày nhận không hợp lệ! ";
                                        }

                                        listImport.Add(row);
                                    }
                                
                                else
                                {
                                    break;
                                }
                                c = c + 1;

                            }

                        }


                    }

                }
                // import temp into db (bulkCopy)
                if (listImport.Count > 0)
                {
                    IEnumerable<InvCkdPhysicalConfirmLot_TDto> dataE = listImport.AsEnumerable();
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
                                bulkCopy.DestinationTableName = "InvCkdPhysicalConfirmLot_T";
                                bulkCopy.ColumnMappings.Add("Guid", "Guid");
                                bulkCopy.ColumnMappings.Add("ModelCode", "ModelCode");
                                bulkCopy.ColumnMappings.Add("ProdLine", "ProdLine");
                                bulkCopy.ColumnMappings.Add("Grade", "Grade");
                                bulkCopy.ColumnMappings.Add("StartLot", "StartLot");
                                bulkCopy.ColumnMappings.Add("StartRun", "StartRun");
                                bulkCopy.ColumnMappings.Add("StartProcessDate", "StartProcessDate");
                                bulkCopy.ColumnMappings.Add("EndLot", "EndLot");
                                bulkCopy.ColumnMappings.Add("EndRun", "EndRun");
                                bulkCopy.ColumnMappings.Add("EndProcessDate", "EndProcessDate");
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
        public async Task MergeDataPhysicalConfirm(string v_Guid)
        {
            string _sql = "Exec INV_CKD_PHYSICAL_CONFIRM_LOT_MERGE @Guid";
            await _dapperRepo.QueryAsync<InvCkdPhysicalConfirmLotDto>(_sql, new { Guid = v_Guid });
        }


        // hiển thị lỗi import và export lỗi
        public async Task<PagedResultDto<InvCkdPhysicalConfirmLot_TDto>> GetMessageErrorImport(string v_Guid)
        {
            string _sql = "Exec INV_CKD_PHYSICAL_CONFIRM_LOT_GET_LIST_ERROR_IMPORT @Guid";

            IEnumerable<InvCkdPhysicalConfirmLot_TDto> result = await _dapperRepo.QueryAsync<InvCkdPhysicalConfirmLot_TDto>(_sql, new
            {
                Guid = v_Guid
            });

            var listResult = result.ToList();
            var totalCount = listResult.Count();

            return new PagedResultDto<InvCkdPhysicalConfirmLot_TDto>(
                totalCount,
               listResult
               );
        }

        public async Task<FileDto> GetListErrToExcel(string v_Guid)
        {
            string _sql = "Exec INV_CKD_PHYSICAL_CONFIRM_LOT_GET_LIST_ERROR_IMPORT @Guid";

            IEnumerable<InvCkdPhysicalConfirmLot_TDto> result = await _dapperRepo.QueryAsync<InvCkdPhysicalConfirmLot_TDto>(_sql, new
            {
                Guid = v_Guid
            });

            var exportToExcel = result.ToList();
            return _calendarListExcelExporter.ExportToFileErr(exportToExcel);
        }

    }
}
