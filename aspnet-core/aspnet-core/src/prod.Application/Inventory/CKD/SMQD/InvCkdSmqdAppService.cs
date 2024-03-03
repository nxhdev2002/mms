using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Dapper.Repositories;
using Abp.Domain.Repositories;
using Abp.UI;
using FastMember;
using GemBox.Spreadsheet;
using prod.Authorization;
using prod.Dto;
using prod.Inventory.CKD.Dto;
using prod.Inventory.CKD.ProductionPlanMonthly.Dto;
using prod.Inventory.CKD.SMQD.Dto;
using prod.Inventory.CKD.SMQD.Exporting;
using prod.Inventory.DRM.StockPart.Dto;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using prod.Common;
using System.Web;

namespace prod.Inventory.CKD.SMQD
{
    [AbpAuthorize(AppPermissions.Pages_Ckd_SMQD_Management_View)]
    public class InvCkdSmqdAppService : prodAppServiceBase, IInvCkdSmqdAppService
    {
        private readonly IDapperRepository<InvCkdSmqd, long> _dapperRepo;
        private readonly IRepository<InvCkdSmqd, long> _repo;
        private readonly IInvCkdSmqdExcelExporter _calendarListExcelExporter;

        public InvCkdSmqdAppService(IRepository<InvCkdSmqd, long> repo,
                                    IDapperRepository<InvCkdSmqd, long> dapperRepo,
                                    IInvCkdSmqdExcelExporter calendarListExcelExporter
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
        }

        public async Task<PagedResultDto<InvCkdSmqdDto>> GetAll(GetInvCkdSmqdInput input)
        {
            string _sql = "Exec INV_CKD_SMQD_SEARCH @p_smqddate_from, @p_smqddate_to, @p_partno, @p_cfc, @p_supplierno, @p_lotno, @p_radio";

            IEnumerable<InvCkdSmqdDto> result = await _dapperRepo.QueryAsync<InvCkdSmqdDto>(_sql, new
            {
                p_smqddate_from = input.SmqdDateFrom,
                p_smqddate_to = input.SmqdDateTo,
                p_partno = input.PartNo,
                p_cfc = input.Cfc,
                p_supplierno = input.SupplierNo,
                p_lotno = input.LotNo,
                p_radio = input.Radio
            });

            var listResult = result.ToList();

            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var totalCount = result.ToList().Count();

            return new PagedResultDto<InvCkdSmqdDto>(
                totalCount,
                pagedAndFiltered);
        }

        public async Task<FileDto> GetInvCkdSmqdToExcel(GetInvCkdSmqdExportInput input)
        {

            string _sql = "Exec INV_CKD_SMQD_SEARCH @p_smqddate_from, @p_smqddate_to, @p_partno, @p_cfc, @p_supplierno, @p_lotno, @p_radio";

            IEnumerable<InvCkdSmqdDto> result = await _dapperRepo.QueryAsync<InvCkdSmqdDto>(_sql, new
            {
                p_smqddate_from = input.SmqdDateFrom,
                p_smqddate_to = input.SmqdDateTo,
                p_partno = input.PartNo,
                p_cfc = input.Cfc,
                p_supplierno = input.SupplierNo,
                p_lotno = input.LotNo,
                p_radio = input.Radio
            });
            var exportToExcel = result.ToList();
            return _calendarListExcelExporter.ExportToFile(exportToExcel);
        }

        [AbpAuthorize(AppPermissions.Pages_Ckd_SMQD_Management_Import)]
        public async Task<List<InvCkdSmqdImportDto>> ImportInvCkdSmqdFromExcel(byte[] fileBytes, string fileName)
        {
            try
            {
                List<InvCkdSmqdImportDto> listImport = new List<InvCkdSmqdImportDto>();
                using (var stream = new MemoryStream(fileBytes))
                {
                    SpreadsheetInfo.SetLicense("EF21-1FW1-HWZF-CLQH");
                    var xlWorkBook = ExcelFile.Load(stream);
                    var v_worksheet = xlWorkBook.Worksheets[0];

                    string strGUID = Guid.NewGuid().ToString("N");

                    string _sql = @"EXEC CMM_IMPORT_USER_GUID_INSERT @Guid, @p_UserId";
                    _dapperRepo.ExecuteAsync(_sql, new
                    {
                        Guid = strGUID,
                        p_UserId = AbpSession.UserId
                    });

                    for (int i = 8; i < v_worksheet.Rows.Count; i++)
                    {
                        string v_runno = (v_worksheet.Cells[i, 10]).Value?.ToString() ?? "";

                        if (v_runno != "")
                        {
                            string v_smqddate = (v_worksheet.Cells[i, 9]).Value?.ToString() ?? "";
                            string v_model = (v_worksheet.Cells[i, 11]).Value?.ToString() ?? "";
                            string v_lotno = (v_worksheet.Cells[i, 12]).Value?.ToString() ?? "";
                            string v_checkmodel = (v_worksheet.Cells[i, 13]).Value?.ToString() ?? "";
                            string v_source = (v_worksheet.Cells[i, 14]).Value?.ToString() ?? "";
                            string v_partno = (v_worksheet.Cells[i, 15]).Value?.ToString() ?? "";
                            string v_partname = (v_worksheet.Cells[i, 16]).Value?.ToString() ?? "";
                            string v_qty = (v_worksheet.Cells[i, 17]).Value?.ToString() ?? "";
                            string v_effectedqty = (v_worksheet.Cells[i, 18]).Value?.ToString() ?? "";
                            string v_reasoncode = (v_worksheet.Cells[i, 20]).Value?.ToString() ?? "";
                            string v_orderstatus = (v_worksheet.Cells[i, 22]).Value?.ToString() ?? "";
                            string v_returnqty = (v_worksheet.Cells[i, 24]).Value?.ToString() ?? "";
                            string v_returndate = (v_worksheet.Cells[i, 25]).Value?.ToString() ?? "";
                            string v_remark = (v_worksheet.Cells[i, 26]).Value?.ToString() ?? "";

                            var row = new InvCkdSmqdImportDto();
                            row.Guid = strGUID;
                            row.ErrorDescription = "";

                            //check Smqd Date
                            try
                            {
                                if(string.IsNullOrEmpty(v_smqddate))
                                {
                                    row.ErrorDescription = "Smqd Date không được để trống!";
                                }
                                else
                                {
                                    row.SmqdDate = DateTime.Parse(v_smqddate);
                                }
                            }
                            catch (Exception e)
                            {
                                row.ErrorDescription = "Smqd Date " + v_smqddate + " không đúng định dạng! ";
                            }

                            row.RunNo = v_runno;
                            row.Cfc = v_model;

                            //Check Lot No
                            if (v_lotno.Length > 10)
                            {
                                row.ErrorDescription += "Lot No " + v_lotno + " dài quá 10 kí tự! ";
                            }
                            else
                            {
                                row.LotNo = v_lotno;
                            }

                            row.CheckModel = v_checkmodel;

                            //Check Supplier No
                            if (v_source.Length > 20)
                            {
                                row.ErrorDescription += "Supplier No " + v_source + " dài quá 20 kí tự! ";
                            }
                            else
                            {
                                row.SupplierNo = v_source;
                            }

                            row.PartNo = v_partno.Replace("-","");
                            row.PartName = v_partname;

                            //Check Qty
                            try
                            {
                                if (string.IsNullOrEmpty(v_qty))
                                {
                                    row.Qty = null;
                                }
                                else
                                {
                                    row.Qty = Convert.ToInt32(v_qty);
                                    if (row.Qty < 0)
                                    {
                                        row.ErrorDescription += "Qty phải là số dương! ";
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                row.ErrorDescription += "Qty " + v_qty + " không phải là số! ";
                            }
                            //Check Effected Qty
                            try
                            {
                                if (string.IsNullOrEmpty(v_effectedqty))
                                {
                                    row.EffectQty = null;
                                }
                                else
                                {
                                    row.EffectQty = Convert.ToInt32(v_effectedqty);
                                    if (row.EffectQty < 0)
                                    {
                                        row.ErrorDescription += "Effected Qty phải là số dương! ";
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                row.ErrorDescription += "Effected Qty " + v_effectedqty + " không phải là số! ";
                            }

                            row.ReasonCode = v_reasoncode;
                            row.OrderStatus = v_orderstatus;

                            //Check Return Qty
                            try
                            {
                                if (string.IsNullOrEmpty(v_returnqty))
                                {
                                    row.ReturnQty = null;
                                }
                                else
                                {
                                    row.ReturnQty = Convert.ToInt32(v_returnqty);
                                    if (row.ReturnQty < 0)
                                    {
                                        row.ErrorDescription += "Return Qty phải là số dương! ";
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                row.ErrorDescription += "Return Qty " + v_returnqty + " không phải là số! ";
                            }
                            //Check Return Date
                            try
                            {
                                if (string.IsNullOrEmpty(v_returndate))
                                {
                                    row.ReturnDate = null;
                                }
                                else
                                {
                                    row.ReturnDate = DateTime.Parse(v_returndate);
                                    if (row.SmqdDate.HasValue && row.ReturnDate < row.SmqdDate)
                                    {
                                        row.ErrorDescription += "Return Date phải sau Smqd Date! ";
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                row.ErrorDescription += "Return Date " + v_returndate + " không đúng định dạng! ";
                            }

                            row.Remark = v_remark;

                            listImport.Add(row);
                        }
                    }
                    // import temp into db (bulkCopy)
                    if (listImport.Count > 0)
                    {
                        IEnumerable<InvCkdSmqdImportDto> dataE = listImport.AsEnumerable();
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
                                    bulkCopy.DestinationTableName = "InvCkdSmqd_T";
                                    bulkCopy.ColumnMappings.Add("Guid", "Guid");
                                    bulkCopy.ColumnMappings.Add("SmqdDate", "SmqdDate");
                                    bulkCopy.ColumnMappings.Add("RunNo", "RunNo");
                                    bulkCopy.ColumnMappings.Add("Cfc", "Cfc");
                                    bulkCopy.ColumnMappings.Add("LotNo", "LotNo");
                                    bulkCopy.ColumnMappings.Add("CheckModel", "CheckModel");
                                    bulkCopy.ColumnMappings.Add("SupplierNo", "SupplierNo");
                                    bulkCopy.ColumnMappings.Add("PartNo", "PartNo");
                                    bulkCopy.ColumnMappings.Add("PartName", "PartName");
                                    bulkCopy.ColumnMappings.Add("Qty", "Qty");
                                    bulkCopy.ColumnMappings.Add("EffectQty", "EffectQty");
                                    bulkCopy.ColumnMappings.Add("ReasonCode", "ReasonCode");
                                    bulkCopy.ColumnMappings.Add("OrderStatus", "OrderStatus");
                                    bulkCopy.ColumnMappings.Add("ReturnQty", "ReturnQty");
                                    bulkCopy.ColumnMappings.Add("ReturnDate", "ReturnDate");
                                    bulkCopy.ColumnMappings.Add("Remark", "Remark");
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
        //Merge Data 
        public async Task MergeDataInvCkdSmqd(string v_Guid, string v_Type)
        {
            string _sql = "Exec INV_CKD_SMQD_MERGE @Guid, @Type";
            await _dapperRepo.QueryAsync<InvCkdSmqdImportDto>(_sql, new { Guid = v_Guid, Type = v_Type });
        }

        // PXP REturn
        /*   public async Task<List<InvPxpReturnImportDto>> ImportPXPReturnFromExcel(byte[] fileBytes, string fileName)
           {
               try
               {
                   List<InvPxpReturnImportDto> listImport = new List<InvPxpReturnImportDto>();
                   using (var stream = new MemoryStream(fileBytes))
                   {
                       SpreadsheetInfo.SetLicense("EF21-1FW1-HWZF-CLQH");
                       var xlWorkBook = ExcelFile.Load(stream);
                       var v_worksheet = xlWorkBook.Worksheets[0];

                       string strGUID = Guid.NewGuid().ToString("N");

                       string _sql = @"EXEC CMM_IMPORT_USER_GUID_INSERT @Guid, @p_UserId";
                       _dapperRepo.ExecuteAsync(_sql, new
                       {
                           Guid = strGUID,
                           p_UserId = AbpSession.UserId
                       });

                       for (int i = 6; i < v_worksheet.Rows.Count; i++)
                       {
                           string v_runno = (v_worksheet.Cells[i, 1]).Value?.ToString() ?? "";

                           if (v_runno != "")
                           {
                               string v_smqddate = (v_worksheet.Cells[i, 0]).Value?.ToString() ?? "";
                               string v_model = (v_worksheet.Cells[i, 2]).Value?.ToString() ?? "";
                               string v_lotno = (v_worksheet.Cells[i, 3]).Value?.ToString() ?? "";
                               string v_checkmodel = (v_worksheet.Cells[i, 4]).Value?.ToString() ?? "";
                               string v_source = (v_worksheet.Cells[i, 5]).Value?.ToString() ?? "";
                               string v_partno = (v_worksheet.Cells[i, 6]).Value?.ToString() ?? "";
                               string v_partname = (v_worksheet.Cells[i, 7]).Value?.ToString() ?? "";
                               string v_qty = (v_worksheet.Cells[i, 8]).Value?.ToString() ?? "";
                               string v_returnqty = (v_worksheet.Cells[i, 11]).Value?.ToString() ?? "";
                               string v_returndate = (v_worksheet.Cells[i, 12]).Value?.ToString() ?? "";
                               string v_remark = (v_worksheet.Cells[i, 13]).Value?.ToString() ?? "";

                               var row = new InvPxpReturnImportDto();
                               row.Guid = strGUID;
                               row.ErrorDescription = "";

                               //check Smqd Date
                               try
                               {
                                   if(string.IsNullOrEmpty(v_smqddate))
                                   {
                                       row.ErrorDescription = "Smqd Date không được để trống!";
                                   }
                                   else
                                   {
                                       row.SmqdDate = DateTime.Parse(v_smqddate);
                                   }
                               }
                               catch (Exception e)
                               {
                                   row.ErrorDescription = "Smqd Date " + v_smqddate + " không đúng định dạng! ";
                               }

                               row.RunNo = v_runno;
                               row.Cfc = v_model;

                               //Check Lot No
                               if (v_lotno.Length > 10)
                               {
                                   row.ErrorDescription += "Lot No " + v_lotno + " dài quá 10 kí tự! ";
                               }
                               else
                               {
                                   row.LotNo = v_lotno;
                               }

                               row.CheckModel = v_checkmodel;

                               //Check Supplier No
                               if (v_source.Length > 20)
                               {
                                   row.ErrorDescription += "Supplier No " + v_source + " dài quá 20 kí tự! ";
                               }
                               else
                               {
                                   row.SupplierNo = v_source;
                               }

                               row.PartNo = v_partno.Replace("-","");
                               row.PartName = v_partname;

                               //Check Qty
                               try
                               {
                                   if (string.IsNullOrEmpty(v_qty))
                                   {
                                       row.Qty = null;
                                   }
                                   else
                                   {
                                       row.Qty = Convert.ToInt32(v_qty);
                                       if (row.Qty < 0)
                                       {
                                           row.ErrorDescription += "Qty phải là số dương! ";
                                       }
                                   }
                               }
                               catch (Exception ex)
                               {
                                   row.ErrorDescription += "Qty " + v_qty + " không phải là số! ";
                               }


                               //Check Return Qty
                               try
                               {
                                   if (string.IsNullOrEmpty(v_returnqty))
                                   {
                                       row.ReturnQty = null;
                                   }
                                   else
                                   {
                                       row.ReturnQty = Convert.ToInt32(v_returnqty);
                                       if (row.ReturnQty < 0)
                                       {
                                           row.ErrorDescription += "Return Qty phải là số dương! ";
                                       }
                                   }
                               }
                               catch (Exception ex)
                               {
                                   row.ErrorDescription += "Return Qty " + v_returnqty + " không phải là số! ";
                               }
                               //Check Return Date
                               try
                               {
                                   if (string.IsNullOrEmpty(v_returndate))
                                   {
                                       row.ReturnDate = null;
                                   }
                                   else
                                   {
                                       row.ReturnDate = DateTime.Parse(v_returndate);
                                       if (row.SmqdDate.HasValue && row.ReturnDate < row.SmqdDate)
                                       {
                                           row.ErrorDescription += "Return Date phải sau Smqd Date! ";
                                       }
                                   }
                               }
                               catch (Exception ex)
                               {
                                   row.ErrorDescription += "Return Date " + v_returndate + " không đúng định dạng! ";
                               }

                               row.Remark = v_remark;

                               listImport.Add(row);
                           }
                       }
                       // import temp into db (bulkCopy)
                       if (listImport.Count > 0)
                       {
                           IEnumerable<InvPxpReturnImportDto> dataE = listImport.AsEnumerable();
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
                                       bulkCopy.DestinationTableName = "InvCkdSmqd_T";
                                       bulkCopy.ColumnMappings.Add("Guid", "Guid");
                                       bulkCopy.ColumnMappings.Add("SmqdDate", "SmqdDate");
                                       bulkCopy.ColumnMappings.Add("RunNo", "RunNo");
                                       bulkCopy.ColumnMappings.Add("Cfc", "Cfc");
                                       bulkCopy.ColumnMappings.Add("LotNo", "LotNo");
                                       bulkCopy.ColumnMappings.Add("CheckModel", "CheckModel");
                                       bulkCopy.ColumnMappings.Add("SupplierNo", "SupplierNo");
                                       bulkCopy.ColumnMappings.Add("PartNo", "PartNo");
                                       bulkCopy.ColumnMappings.Add("PartName", "PartName");
                                       bulkCopy.ColumnMappings.Add("Qty", "Qty");
                                       bulkCopy.ColumnMappings.Add("EffectQty", "EffectQty");
                                       bulkCopy.ColumnMappings.Add("ReasonCode", "ReasonCode");
                                       bulkCopy.ColumnMappings.Add("OrderStatus", "OrderStatus");
                                       bulkCopy.ColumnMappings.Add("ReturnQty", "ReturnQty");
                                       bulkCopy.ColumnMappings.Add("ReturnDate", "ReturnDate");
                                       bulkCopy.ColumnMappings.Add("Remark", "Remark");
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
           }*/
        // PXP IN
        /*         public async Task<List<InvPxpReturnImportDto>> ImportPXPINFromExcel(byte[] fileBytes, string fileName)



                {
                    try
                    {
                        List<InvPxpReturnImportDto> listImport = new List<InvPxpReturnImportDto>();
                        using (var stream = new MemoryStream(fileBytes))
                        {
                            SpreadsheetInfo.SetLicense("EF21-1FW1-HWZF-CLQH");
                            var xlWorkBook = ExcelFile.Load(stream);
                            var v_worksheet = xlWorkBook.Worksheets[0];

                            string strGUID = Guid.NewGuid().ToString("N");

                            string _sql = @"EXEC CMM_IMPORT_USER_GUID_INSERT @Guid, @p_UserId";
                            _dapperRepo.ExecuteAsync(_sql, new
                            {
                                Guid = strGUID,
                                p_UserId = AbpSession.UserId
                            });

                            for (int i = 4; i < v_worksheet.Rows.Count; i++)
                            {
                                string v_runno = (v_worksheet.Cells[i, 1]).Value?.ToString() ?? "";

                                if (v_runno != "")
                                {
                                    string v_smqddate = (v_worksheet.Cells[i, 0]).Value?.ToString() ?? "";
                                    string v_model = (v_worksheet.Cells[i, 2]).Value?.ToString() ?? "";
                                    string v_lotno = (v_worksheet.Cells[i, 3]).Value?.ToString() ?? "";
                                    string v_checkmodel = (v_worksheet.Cells[i, 4]).Value?.ToString() ?? "";
                                    string v_source = (v_worksheet.Cells[i, 5]).Value?.ToString() ?? "";
                                    string v_partno = (v_worksheet.Cells[i, 6]).Value?.ToString() ?? "";
                                    string v_partname = (v_worksheet.Cells[i, 7]).Value?.ToString() ?? "";
                                    string v_orderno = (v_worksheet.Cells[i, 8]).Value?.ToString() ?? "";
                                    string v_qty = (v_worksheet.Cells[i, 9]).Value?.ToString() ?? "";
                                    string v_invoice = (v_worksheet.Cells[i, 11]).Value?.ToString() ?? "";
                                    string v_returndate = (v_worksheet.Cells[i, 12]).Value?.ToString() ?? "";
                                    string v_reasoncode = (v_worksheet.Cells[i, 14]).Value?.ToString() ?? "";

                                    var row = new InvPxpReturnImportDto();
                                    row.Guid = strGUID;
                                    row.ErrorDescription = "";

                                    //check Smqd Date
                                    try
                                    {
                                        if(string.IsNullOrEmpty(v_smqddate))
                                        {
                                            row.ErrorDescription = "Smqd Date không được để trống!";
                                        }
                                        else
                                        {
                                            row.SmqdDate = DateTime.Parse(v_smqddate);
                                        }
                                    }
                                    catch (Exception e)
                                    {
                                        row.ErrorDescription = "Smqd Date " + v_smqddate + " không đúng định dạng! ";
                                    }

                                    row.RunNo = v_runno;
                                    row.Cfc = v_model;

                                    //Check Lot No
                                    if (v_lotno.Length > 10)
                                    {
                                        row.ErrorDescription += "Lot No " + v_lotno + " dài quá 10 kí tự! ";
                                    }
                                    else
                                    {
                                        row.LotNo = v_lotno;
                                    }

                                    row.CheckModel = v_checkmodel;

                                    //Check Supplier No
                                    if (v_source.Length > 20)
                                    {
                                        row.ErrorDescription += "Supplier No " + v_source + " dài quá 20 kí tự! ";
                                    }
                                    else
                                    {
                                        row.SupplierNo = v_source;
                                    }

                                    row.PartNo = v_partno.Replace("-","");
                                    row.PartName = v_partname;

                                    //Check Qty
                                    try
                                    {
                                        if (string.IsNullOrEmpty(v_qty))
                                        {
                                            row.Qty = null;
                                        }
                                        else
                                        {
                                            row.Qty = Convert.ToInt32(v_qty);
                                            if (row.Qty < 0)
                                            {
                                                row.ErrorDescription += "Qty phải là số dương! ";
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        row.ErrorDescription += "Qty " + v_qty + " không phải là số! ";
                                    }



                                    //Check Return Date
                                    try
                                    {
                                        if (string.IsNullOrEmpty(v_returndate))
                                        {
                                            row.ReturnDate = null;
                                        }
                                        else
                                        {
                                            row.ReturnDate = DateTime.Parse(v_returndate);
                                            if (row.SmqdDate.HasValue && row.ReturnDate < row.SmqdDate)
                                            {
                                                row.ErrorDescription += "Return Date phải sau Smqd Date! ";
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        row.ErrorDescription += "Return Date " + v_returndate + " không đúng định dạng! ";
                                    }


                                    listImport.Add(row);
                                }
                            }
                            // import temp into db (bulkCopy)
                            if (listImport.Count > 0)
                            {
                                IEnumerable<InvPxpReturnImportDto> dataE = listImport.AsEnumerable();
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
                                            bulkCopy.DestinationTableName = "InvCkdSmqd_T";
                                            bulkCopy.ColumnMappings.Add("Guid", "Guid");
                                            bulkCopy.ColumnMappings.Add("SmqdDate", "SmqdDate");
                                            bulkCopy.ColumnMappings.Add("RunNo", "RunNo");
                                            bulkCopy.ColumnMappings.Add("Cfc", "Cfc");
                                            bulkCopy.ColumnMappings.Add("LotNo", "LotNo");
                                            bulkCopy.ColumnMappings.Add("CheckModel", "CheckModel");
                                            bulkCopy.ColumnMappings.Add("SupplierNo", "SupplierNo");
                                            bulkCopy.ColumnMappings.Add("PartNo", "PartNo");
                                            bulkCopy.ColumnMappings.Add("PartName", "PartName");
                                            bulkCopy.ColumnMappings.Add("Qty", "Qty");
                                            bulkCopy.ColumnMappings.Add("EffectQty", "EffectQty");
                                            bulkCopy.ColumnMappings.Add("ReasonCode", "ReasonCode");
                                            bulkCopy.ColumnMappings.Add("Reason", "Reason");
                                            bulkCopy.ColumnMappings.Add("OrderStatus", "OrderStatus");
                                            bulkCopy.ColumnMappings.Add("ReturnQty", "ReturnQty");
                                            bulkCopy.ColumnMappings.Add("ReturnDate", "ReturnDate");
                                            bulkCopy.ColumnMappings.Add("Remark", "Remark");
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
        */
        //Merge Data 
        /*    public async Task MergeDataInvCkdSmqdPxpReturn(string v_Guid)
            {
                string _sql = "Exec INV_CKD_SMQD_PXP_RETURN_MERGE @Guid";
                await _dapperRepo.QueryAsync<InvCkdSmqdImportDto>(_sql, new { Guid = v_Guid });
            }
    */


        // hiển thị lỗi import và export lỗi
        public async Task<PagedResultDto<InvCkdSmqdImportDto>> GetMessageErrorImport(string v_Guid)
        {
            string _sql = "Exec INV_CKD_SMQD_GET_LIST_ERROR_IMPORT @Guid";

            IEnumerable<InvCkdSmqdImportDto> result = await _dapperRepo.QueryAsync<InvCkdSmqdImportDto>(_sql, new
            {
                Guid = v_Guid

            });

            var listResult = result.ToList();
            var totalCount = listResult.Count();

            return new PagedResultDto<InvCkdSmqdImportDto>(
                totalCount,
               listResult
               );
        }

        /*        public async Task<FileDto> GetListErrToExcel(string v_Guid, string v_Type)
                {
                    string _sql = "Exec INV_CKD_SMQD_GET_LIST_ERROR_IMPORT @Guid,@Type";

                    IEnumerable<InvCkdSmqdImportDto> result = await _dapperRepo.QueryAsync<InvCkdSmqdImportDto>(_sql, new
                    {
                        Guid = v_Guid,
                         Type = v_Type
                    });

                    var exportToExcel = result.ToList();
                    return _calendarListExcelExporter.ExportToFileErr(exportToExcel);
                }*/




        public async Task<List<InvPxpOutImportDto>> ImportPXPOtherOutFromExcel(byte[] fileBytes, string fileName)
        {
            try
            {
                List<InvPxpOutImportDto> listImport = new List<InvPxpOutImportDto>();
                using (var stream = new MemoryStream(fileBytes))
                {
                    SpreadsheetInfo.SetLicense("EF21-1FW1-HWZF-CLQH");
                    var xlWorkBook = ExcelFile.Load(stream);
                    var v_worksheet = xlWorkBook.Worksheets[0];

                    string strGUID = Guid.NewGuid().ToString("N");

                    string _sql = @"EXEC CMM_IMPORT_USER_GUID_INSERT @Guid, @p_UserId";
                    _dapperRepo.ExecuteAsync(_sql, new
                    {
                        Guid = strGUID,
                        p_UserId = AbpSession.UserId
                    });

                    for (int i = 6; i < v_worksheet.Rows.Count; i++)
                    {
                        string v_runno = (v_worksheet.Cells[i, 1]).Value?.ToString() ?? "";

                        if (v_runno != "")
                        {
                            string v_smqddate = (v_worksheet.Cells[i, 0]).Value?.ToString() ?? "";
                            string v_model = (v_worksheet.Cells[i, 2]).Value?.ToString() ?? "";
                            string v_lotno = (v_worksheet.Cells[i, 3]).Value?.ToString() ?? "";
                            string v_checkmodel = (v_worksheet.Cells[i, 4]).Value?.ToString() ?? "";
                            string v_source = (v_worksheet.Cells[i, 5]).Value?.ToString() ?? "";
                            string v_partno = (v_worksheet.Cells[i, 6]).Value?.ToString() ?? "";
                            string v_partname = (v_worksheet.Cells[i, 7]).Value?.ToString() ?? "";
                            string v_qty = (v_worksheet.Cells[i, 8]).Value?.ToString() ?? "";
                            string v_remark = (v_worksheet.Cells[i, 9]).Value?.ToString() ?? "";
                            var row = new InvPxpOutImportDto();
                            row.Guid = strGUID;
                            row.ErrorDescription = "";
                            row.Type = "6";

                            //check Smqd Date
                            try
                            {
                                if (string.IsNullOrEmpty(v_smqddate))
                                {
                                    row.ErrorDescription = "Smqd Date không được để trống!";
                                }
                                else
                                {
                                    row.SmqdDate = DateTime.Parse(v_smqddate);
                                }
                            }
                            catch (Exception e)
                            {
                                row.ErrorDescription = "Smqd Date " + v_smqddate + " không đúng định dạng! ";
                            }

            
                            // Check Run No
                            if (v_runno.Length > 50)
                            {
                                row.ErrorDescription += "RunNo No " + v_runno + " dài quá 50 kí tự! ";
                            }
                            else
                            {
                                row.RunNo = v_runno;
                            }

                            row.Cfc = v_model;

                            //Check Lot No
                            if (v_lotno.Length > 10)
                            {
                                row.ErrorDescription += "Lot No " + v_lotno + " dài quá 10 kí tự! ";
                            }
                            else
                            {
                                row.LotNo = v_lotno;
                            }

                            row.CheckModel = v_checkmodel;

                            //Check Supplier No
                            if (v_source.Length > 20)
                            {
                                row.ErrorDescription += "Supplier No " + v_source + " dài quá 20 kí tự! ";
                            }
                            else
                            {
                                row.SupplierNo = v_source;
                            }


                            //Check Part No
                            if (v_partno.Length > 20)
                            {
                                row.ErrorDescription += "Part No " + v_partno + " dài quá 50 kí tự! ";
                            }
                            else
                            {
                                row.PartNo = v_partno.Replace("-", "");
                            }
                    
                            row.PartName = v_partname;

                            //Check Qty
                            try
                            {
                                if (string.IsNullOrEmpty(v_qty))
                                {
                                    row.Qty = null;
                                }
                                else
                                {
                                    row.Qty = Convert.ToInt32(v_qty);
                                    if (row.Qty < 0)
                                    {
                                        row.ErrorDescription += "Qty phải là số dương! ";
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                row.ErrorDescription += "Qty " + v_qty + " không phải là số! ";
                            }

                            row.Remark = v_remark;

                         

                            listImport.Add(row);
                        }
                    }
                    // import temp into db (bulkCopy)
                    if (listImport.Count > 0)
                    {
                        IEnumerable<InvPxpOutImportDto> dataE = listImport.AsEnumerable();
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
                                    bulkCopy.DestinationTableName = "InvCkdSmqd_T";
                                    bulkCopy.ColumnMappings.Add("Guid", "Guid");
                                    bulkCopy.ColumnMappings.Add("SmqdDate", "SmqdDate");
                                    bulkCopy.ColumnMappings.Add("RunNo", "RunNo");
                                    bulkCopy.ColumnMappings.Add("Cfc", "Cfc");
                                    bulkCopy.ColumnMappings.Add("LotNo", "LotNo");
                                    bulkCopy.ColumnMappings.Add("CheckModel", "CheckModel");
                                    bulkCopy.ColumnMappings.Add("SupplierNo", "SupplierNo");
                                    bulkCopy.ColumnMappings.Add("PartNo", "PartNo");
                                    bulkCopy.ColumnMappings.Add("PartName", "PartName");
                                    bulkCopy.ColumnMappings.Add("Qty", "Qty");
                                    bulkCopy.ColumnMappings.Add("Remark", "Remark");
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

        //Merge Data Pxp Out or Other Out
        public async Task MergeDataSmqdPxpOtherOut(string v_Guid, string v_Type)
        {
            string _sql = "Exec INV_CKD_SMQD_PXP_OUT_MERGE @Guid, @Type ";
            await _dapperRepo.QueryAsync<InvCkdSmqdImportDto>(_sql, new { Guid = v_Guid, Type = v_Type });
        }

        public async Task<List<InvPxpReturnImportDto>> ImportPXPReturnExcel(byte[] fileBytes, string fileName)
        {
            try
            {
                List<InvPxpReturnImportDto> listImport = new List<InvPxpReturnImportDto>();
                using (var stream = new MemoryStream(fileBytes))
                {
                    SpreadsheetInfo.SetLicense("EF21-1FW1-HWZF-CLQH");
                    var xlWorkBook = ExcelFile.Load(stream);
                    var v_worksheet = xlWorkBook.Worksheets[0];

                    string strGUID = Guid.NewGuid().ToString("N");

                    string _sql = @"EXEC CMM_IMPORT_USER_GUID_INSERT @Guid, @p_UserId";
                    _dapperRepo.ExecuteAsync(_sql, new
                    {
                        Guid = strGUID,
                        p_UserId = AbpSession.UserId
                    });

                    for (int i = 5; i < v_worksheet.Rows.Count; i++)
                    {
                        string v_runno = (v_worksheet.Cells[i, 1]).Value?.ToString() ?? "";

                        if (v_runno != "")
                        {
                            string v_smqddate = (v_worksheet.Cells[i, 0]).Value?.ToString() ?? "";
                            string v_model = (v_worksheet.Cells[i, 2]).Value?.ToString() ?? "";
                            string v_lotno = (v_worksheet.Cells[i, 3]).Value?.ToString() ?? "";
                            string v_checkmodel = (v_worksheet.Cells[i, 4]).Value?.ToString() ?? "";
                            string v_source = (v_worksheet.Cells[i, 5]).Value?.ToString() ?? "";
                            string v_partno = (v_worksheet.Cells[i, 6]).Value?.ToString() ?? "";
                            string v_partname = (v_worksheet.Cells[i, 7]).Value?.ToString() ?? "";
                            string v_qty = (v_worksheet.Cells[i, 8]).Value?.ToString() ?? "";
                            string v_returnqty = (v_worksheet.Cells[i, 11]).Value?.ToString() ?? "";
                            string v_returndate = (v_worksheet.Cells[i, 12]).Value?.ToString() ?? "";
                            string v_remark = (v_worksheet.Cells[i, 13]).Value?.ToString() ?? "";
                            var row = new InvPxpReturnImportDto();
                            row.Guid = strGUID;
                            row.ErrorDescription = "";

                            //check Smqd Date
                            try
                            {
                                if (string.IsNullOrEmpty(v_smqddate))
                                {
                                    row.ErrorDescription = "Smqd Date không được để trống!";
                                }
                                else
                                {
                                    row.SmqdDate = DateTime.Parse(v_smqddate);
                                }
                            }
                            catch (Exception e)
                            {
                                row.ErrorDescription = "Smqd Date " + v_smqddate + " không đúng định dạng! ";
                            }


                            // Check Run No
                            if (v_runno.Length > 50)
                            {
                                row.ErrorDescription += "RunNo No " + v_runno + " dài quá 50 kí tự! ";
                            }
                            else
                            {
                                row.RunNo = v_runno;
                            }

                            row.Cfc = v_model;

                            //Check Lot No
                            if (v_lotno.Length > 10)
                            {
                                row.ErrorDescription += "Lot No " + v_lotno + " dài quá 10 kí tự! ";
                            }
                            else
                            {
                                row.LotNo = v_lotno;
                            }

                            row.CheckModel = v_checkmodel;

                            //Check Supplier No
                            if (v_source.Length > 20)
                            {
                                row.ErrorDescription += "Supplier No " + v_source + " dài quá 20 kí tự! ";
                            }
                            else
                            {
                                row.SupplierNo = v_source;
                            }


                            //Check Part No
                            if (v_partno.Length > 20)
                            {
                                row.ErrorDescription += "Part No " + v_partno + " dài quá 50 kí tự! ";
                            }
                            else
                            {
                                row.PartNo = v_partno.Replace("-", "");
                            }

                            row.PartName = v_partname;

                            //Check Qty
                            try
                            {
                                if (string.IsNullOrEmpty(v_qty))
                                {
                                    row.Qty = null;
                                }
                                else
                                {
                                    row.Qty = Convert.ToInt32(v_qty);
                                    if (row.Qty < 0)
                                    {
                                        row.ErrorDescription += "Qty phải là số dương! ";
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                row.ErrorDescription += "Qty " + v_qty + " không phải là số! ";
                            }


                            // Check Return Qty

                            try
                            {
                                if (string.IsNullOrEmpty(v_returnqty))
                                {
                                    row.ReturnQty = null;
                                }
                                else
                                {
                                    row.ReturnQty = Convert.ToInt32(v_returnqty);
                                    if (row.ReturnQty < 0)
                                    {
                                        row.ErrorDescription += "Returning Qty phải là số dương! ";
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                row.ErrorDescription += "Returning Qty " + v_returnqty + " không phải là số! ";
                            }


                            //check Return  Date
                            try
                            {
                                if (string.IsNullOrEmpty(v_returndate))
                                {
                                    row.ErrorDescription = "Returning Date không được để trống!";
                                }
                                else
                                {
                                    row.ReturnDate = DateTime.Parse(v_returndate);
                                }
                            }
                            catch (Exception e)
                            {
                                row.ErrorDescription = "Returning Date " + v_returndate + " không đúng định dạng! ";
                            }



                            listImport.Add(row);
                        }
                    }
                    // import temp into db (bulkCopy)
                    if (listImport.Count > 0)
                    {
                        IEnumerable<InvPxpReturnImportDto> dataE = listImport.AsEnumerable();
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
                                    bulkCopy.DestinationTableName = "InvCkdSmqd_T";
                                    bulkCopy.ColumnMappings.Add("Guid", "Guid");
                                    bulkCopy.ColumnMappings.Add("SmqdDate", "SmqdDate");
                                    bulkCopy.ColumnMappings.Add("RunNo", "RunNo");
                                    bulkCopy.ColumnMappings.Add("Cfc", "Cfc");
                                    bulkCopy.ColumnMappings.Add("LotNo", "LotNo");
                                    bulkCopy.ColumnMappings.Add("CheckModel", "CheckModel");
                                    bulkCopy.ColumnMappings.Add("SupplierNo", "SupplierNo");
                                    bulkCopy.ColumnMappings.Add("PartNo", "PartNo");
                                    bulkCopy.ColumnMappings.Add("PartName", "PartName");
                                    bulkCopy.ColumnMappings.Add("Qty", "Qty");
                                    bulkCopy.ColumnMappings.Add("ReturnQty", "ReturnQty");
                                    bulkCopy.ColumnMappings.Add("ReturnDate", "ReturnDate");
                                    bulkCopy.ColumnMappings.Add("Remark", "Remark");
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

        //Merge Data PXP Return
        public async Task MergeDataSmqdPxpReturn(string v_Guid, string v_Type)
        {
            string _sql = "Exec INV_CKD_SMQD_PXP_RETURN_DATA_MERGE @Guid, @Type ";
            await _dapperRepo.QueryAsync<InvCkdSmqdImportDto>(_sql, new { Guid = v_Guid, Type = v_Type });
        }



        public async Task<List<InvPxpReturnImportDto>> ImportPXPInExcel(byte[] fileBytes, string fileName)
        {
            try
            {
                List<InvPxpReturnImportDto> listImport = new List<InvPxpReturnImportDto>();
                using (var stream = new MemoryStream(fileBytes))
                {
                    SpreadsheetInfo.SetLicense("EF21-1FW1-HWZF-CLQH");
                    var xlWorkBook = ExcelFile.Load(stream);
                    var v_worksheet = xlWorkBook.Worksheets[0];

                    string strGUID = Guid.NewGuid().ToString("N");

                    string _sql = @"EXEC CMM_IMPORT_USER_GUID_INSERT @Guid, @p_UserId";
                    _dapperRepo.ExecuteAsync(_sql, new
                    {
                        Guid = strGUID,
                        p_UserId = AbpSession.UserId
                    });

                    for (int i = 4; i < v_worksheet.Rows.Count; i++)
                    {
                        string v_runno = (v_worksheet.Cells[i, 1]).Value?.ToString() ?? "";

                        if (v_runno != "")
                        {
                            string v_smqddate = (v_worksheet.Cells[i, 0]).Value?.ToString() ?? "";

                            string v_model = (v_worksheet.Cells[i, 2]).Value?.ToString() ?? "";

                            string v_source = (v_worksheet.Cells[i, 3]).Value?.ToString() ?? "";
                            string v_partno = (v_worksheet.Cells[i, 4]).Value?.ToString() ?? "";
                            string v_partname = (v_worksheet.Cells[i, 5]).Value?.ToString() ?? "";
                            string v_OrderNo = (v_worksheet.Cells[i, 6]).Value?.ToString() ?? "";
                            string v_qty = (v_worksheet.Cells[i, 7]).Value?.ToString() ?? "";
                            string v_invoice = (v_worksheet.Cells[i, 16]).Value?.ToString() ?? "";
                            string v_ReceivedDate = (v_worksheet.Cells[i, 17]).Value?.ToString() ?? "";
                            string v_reason = (v_worksheet.Cells[i, 19]).Value?.ToString() ?? "";

                            var row = new InvPxpReturnImportDto();
                            row.Guid = strGUID;
                            row.ErrorDescription = "";

                            //check Smqd Date
                            try
                            {
                                if (string.IsNullOrEmpty(v_smqddate))
                                {
                                    row.ErrorDescription = "Smqd Date không được để trống!";
                                }
                                else
                                {
                                    row.SmqdDate = DateTime.Parse(v_smqddate);
                                }
                            }
                            catch (Exception e)
                            {
                                row.ErrorDescription = "Smqd Date " + v_smqddate + " không đúng định dạng! ";
                            }


                            // Check Run No
                            if (v_runno.Length > 50)
                            {
                                row.ErrorDescription += "RunNo No " + v_runno + " dài quá 50 kí tự! ";
                            }
                            else
                            {
                                row.RunNo = v_runno;
                            }

                            row.Cfc = v_model;





                            //Check Supplier No
                            if (v_source.Length > 20)
                            {
                                row.ErrorDescription += "Supplier No " + v_source + " dài quá 20 kí tự! ";
                            }
                            else
                            {
                                row.SupplierNo = v_source;
                            }



                            //Check Part No
                            if (v_partno.Length > 20)
                            {
                                row.ErrorDescription += "Part No " + v_partno + " dài quá 50 kí tự! ";
                            }
                            else
                            {
                                row.PartNo = v_partno.Replace("-", "");
                            }

                            row.PartName = v_partname;

                            //Check Qty
                            try
                            {
                                if (string.IsNullOrEmpty(v_qty))
                                {
                                    row.Qty = null;
                                }
                                else
                                {
                                    row.Qty = Convert.ToInt32(v_qty);
                                    if (row.Qty < 0)
                                    {
                                        row.ErrorDescription += "Qty phải là số dương! ";
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                row.ErrorDescription += "Qty " + v_qty + " không phải là số! ";
                            }
                            row.Reason = v_reason;

                            //Check Order No
                            if (v_OrderNo.Length > 50)
                            {
                                row.ErrorDescription += "Order No " + v_source + " dài quá 50 kí tự! ";
                            }
                            else
                            {
                                row.OrderNo = v_OrderNo;
                            }

                            //Check Order No
                            if (v_invoice.Length > 50)
                            {
                                row.ErrorDescription += "Invoice " + v_invoice + " dài quá 50 kí tự! ";
                            }
                            else
                            {
                                row.Invoice  = v_invoice;
                            }

                            //Check Order No
                            if (v_invoice.Length > 50)
                            {
                                row.ErrorDescription += "Invoice " + v_invoice + " dài quá 50 kí tự! ";
                            }
                            else
                            {
                                row.Invoice = v_invoice;
                            }

                     
                            try
                            {
                                if (string.IsNullOrEmpty(v_ReceivedDate))
                                {
                                    row.ReceivedDate = null;
                                }
                                else
                                {
                                    row.ReceivedDate = DateTime.Parse(v_ReceivedDate);
                                }
                            }
                            catch (Exception e)
                            {
                                row.ErrorDescription = "Received Date " + v_ReceivedDate + " không đúng định dạng! ";
                            }

                            listImport.Add(row);
                        }
                    }
                    // import temp into db (bulkCopy)
                    if (listImport.Count > 0)
                    {
                        IEnumerable<InvPxpReturnImportDto> dataE = listImport.AsEnumerable();
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
                                    bulkCopy.DestinationTableName = "InvCkdSmqd_T";
                                    bulkCopy.ColumnMappings.Add("Guid", "Guid");
                                    bulkCopy.ColumnMappings.Add("SmqdDate", "SmqdDate");
                                    bulkCopy.ColumnMappings.Add("RunNo", "RunNo");
                                    bulkCopy.ColumnMappings.Add("Cfc", "Cfc");
                                    bulkCopy.ColumnMappings.Add("OrderNo", "OrderNo");
                                    bulkCopy.ColumnMappings.Add("Invoice", "Invoice");
                                    bulkCopy.ColumnMappings.Add("ReceivedDate", "ReceivedDate");
                                    bulkCopy.ColumnMappings.Add("SupplierNo", "SupplierNo");
                                    bulkCopy.ColumnMappings.Add("PartNo", "PartNo");
                                    bulkCopy.ColumnMappings.Add("PartName", "PartName");
                                    bulkCopy.ColumnMappings.Add("Qty", "Qty");
                                    bulkCopy.ColumnMappings.Add("ReasonCode", "ReasonCode");
                                    bulkCopy.ColumnMappings.Add("Reason", "Reason");
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


        public async Task MergeDataSmqdPxpIn(string v_Guid, string v_Type)
        {
            string _sql = "Exec [INV_CKD_SMQD_PXP_IN_MERGE] @Guid, @Type ";
            await _dapperRepo.QueryAsync<InvCkdSmqdImportDto>(_sql, new { Guid = v_Guid, Type = v_Type });
        }


        public async Task<FileDto> GetListErrExcel(string v_Guid)
        {
            string _sql = "Exec INV_CKD_SMQD_GET_LIST_ERROR_IMPORT @Guid";

            IEnumerable<InvCkdSmqdImportDto> result = await _dapperRepo.QueryAsync<InvCkdSmqdImportDto>(_sql, new
            {
                Guid = v_Guid
              
            });

            var exportToExcel = result.ToList();
            return _calendarListExcelExporter.ExportToFileErr(exportToExcel);
        }

    }
}
