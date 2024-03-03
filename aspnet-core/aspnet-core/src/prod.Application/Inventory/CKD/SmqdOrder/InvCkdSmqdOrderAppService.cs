using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Dapper.Repositories;
using Abp.UI;
using FastMember;
using GemBox.Spreadsheet;
using prod.Authorization;
using prod.Dto;
using prod.Inventory.CKD.Dto;
using prod.Inventory.CKD.Exporting;
using prod.Inventory.CKD.PartRobbing.Dto;
using prod.Inventory.CKD.SmqdOrder.Exporting;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using prod.Common;
using Twilio.TwiML.Voice;
using static Azure.Core.HttpHeader;
using Abp.Domain.Uow;
using Abp.EntityFrameworkCore.Uow;
using prod.EntityFrameworkCore;
using Abp.Domain.Repositories;
using prod.Inventory.DRM;
using NPOI.SS.Formula.Functions;
using Microsoft.EntityFrameworkCore;
using prod.Inventory.DRM.Dto;

namespace prod.Inventory.CKD
{
    [AbpAuthorize(AppPermissions.Pages_Ckd_SMQD_SmqdOrder_View)]
    public class InvCkdSmqdOrderAppService : prodAppServiceBase, IInvCkdSmqdOrderAppService
    {
        private readonly IDapperRepository<InvCkdSmqdOrder, long> _dapperRepo;
        private readonly IRepository<InvCkdSmqdOrder, long> _repo;
        private readonly IInvCkdSmqdOrdergExcelExporter _ExcelExporter;

        public InvCkdSmqdOrderAppService(IDapperRepository<InvCkdSmqdOrder, long> dapperRepo,
                                            IRepository<InvCkdSmqdOrder, long> repo,
                                        IInvCkdSmqdOrdergExcelExporter ExcelExporter
                                         )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _ExcelExporter = ExcelExporter;
        }
        

        public async Task<PagedResultDto<InvCkdSmqdOrderDto>> GetAll(GetInvCkdSmqdOrderInput input)
        {
            string _sql = "Exec INV_CKD_SMQD_ORDER_SEARCH @p_shop, @p_Smqd_date_from, @p_Smqd_date_to, @p_part_no , @p_cfc, @p_supplier_no, @p_order_no, @p_order_date_from, @p_order_date_to, @p_invoice_no";

            IEnumerable<InvCkdSmqdOrderDto> result = await _dapperRepo.QueryAsync<InvCkdSmqdOrderDto>(_sql, new
            {
                p_shop = input.Shop,
                p_Smqd_date_from = input.SmqdDateFrom,
                p_Smqd_date_to = input.SmqdDateTo,
                p_part_no = input.PartNo,
                p_cfc = input.Cfc,
                p_supplier_no = input.SupplierNo,
                p_order_no = input.OrderNo,
                p_order_date_from = input.OrderDateFrom,
                p_order_date_to = input.OrderDateTo,
                p_invoice_no = input.InvoiceNo
            });

            var listResult = result.ToList();

            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var totalCount = result.ToList().Count();

            return new PagedResultDto<InvCkdSmqdOrderDto>(
                totalCount,
                pagedAndFiltered);
        }
        [AbpAuthorize(AppPermissions.Pages_Ckd_SMQD_SmqdOrder_CreateEdit)]
        public async System.Threading.Tasks.Task CreateOrEdit(CreateOrEditInvCkdSmqdOrderDto input)
        {
            if (input.Id == null) await Create(input);
            else await Update(input);
        }

        //CREATE
        private async System.Threading.Tasks.Task Create(CreateOrEditInvCkdSmqdOrderDto input)
        {
            var mainObj = ObjectMapper.Map<InvCkdSmqdOrder>(input);
            await CurrentUnitOfWork.GetDbContext<prodDbContext>().AddAsync(mainObj);
        }

        // EDIT
        private async System.Threading.Tasks.Task Update(CreateOrEditInvCkdSmqdOrderDto input)
        {
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                var mainObj = await _repo.GetAll().FirstOrDefaultAsync(e => e.Id == input.Id);
                if (mainObj != null)
                {
                    mainObj.Shop = input.Shop;
                    mainObj.SmqdDate= input.SmqdDate;
                    mainObj.RunNo = input.RunNo;
                    mainObj.Cfc= input.Cfc;
                    mainObj.SupplierNo= input.SupplierNo;
                    mainObj.PartNo= input.PartNo;
                    mainObj.PartName= input.PartName;
                    mainObj.OrderNo= input.OrderNo;
                    mainObj.OrderQty= input.OrderQty;
                    mainObj.OrderDate= input.OrderDate;
                    mainObj.Transport= input.Transport;
                    mainObj.ReasonCode= input.ReasonCode;
                    mainObj.EtaRequest= input.EtaRequest;
                    mainObj.ActualEtaPort= input.ActualEtaPort;
                    mainObj.EtaExpReply= input.EtaExpReply;
                    mainObj.InvoiceNo= input.InvoiceNo;
                    mainObj.ReceiveDate= input.ReceiveDate;
                    mainObj.ReceiveQty= input.ReceiveQty;
                    mainObj.Remark= input.Remark;
                    mainObj.OrderType= input.OrderType;

                }

                //var mainObjToUpdate = ObjectMapper.Map(input, mainObj);
            }
        }

        public async Task<FileDto> GetSmqdOrderListToExcel(GetInvCkdSmqdOrderExportInput input)
        {
            string _sql = "Exec INV_CKD_SMQD_ORDER_SEARCH @p_shop, @p_Smqd_date_from, @p_Smqd_date_to, @p_part_no , @p_cfc, @p_supplier_no, @p_order_no, @p_order_date_from, @p_order_date_to, @p_invoice_no";

            IEnumerable<InvCkdSmqdOrderDto> result = await _dapperRepo.QueryAsync<InvCkdSmqdOrderDto>(_sql, new
            {
                p_shop = input.Shop,
                p_Smqd_date_from = input.SmqdDateFrom,
                p_Smqd_date_to = input.SmqdDateTo,
                p_part_no = input.PartNo,
                p_cfc = input.Cfc,
                p_supplier_no = input.SupplierNo,
                p_order_no = input.OrderNo,
                p_order_date_from = input.OrderDateFrom,
                p_order_date_to = input.OrderDateTo,
                p_invoice_no = input.InvoiceNo
            });

            var listResult = result.ToList();
            return _ExcelExporter.ExportToFile(listResult);
        }

        [AbpAuthorize(AppPermissions.Pages_Ckd_SMQD_SmqdOrder_Import)]
        public async Task<List<InvCkdSmqdOrderImportDto>> ImportSmqdOrderNormalFromExcel(byte[] fileBytes, string fileName)
        {
            try
            {
                List<InvCkdSmqdOrderImportDto> listImport = new List<InvCkdSmqdOrderImportDto>();
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

                    int startrow = 8;

                    for (int i = startrow; i < v_worksheet.Rows.Count; i++)
                    {
                        string v_partNo = (v_worksheet.Cells[i, 9]).Value?.ToString() ?? "";

                        if (v_partNo.Trim() != "")
                        {
                            string v_shop = (v_worksheet.Cells[i, 4]).Value?.ToString() ?? "";
                            string v_smqd_Date = (v_worksheet.Cells[i, 5]).Value?.ToString();
                            string v_runno = (v_worksheet.Cells[i, 6]).Value?.ToString() ?? "";
                            string v_cfc = (v_worksheet.Cells[i, 7]).Value?.ToString() ?? "";
                            string v_source = (v_worksheet.Cells[i, 8]).Value?.ToString() ?? "";
                            string v_partname = (v_worksheet.Cells[i, 10]).Value?.ToString() ?? "";
                            string v_order_no = (v_worksheet.Cells[i, 11]).Value?.ToString() ?? "";
                            string v_order_qty = (v_worksheet.Cells[i, 12]).Value?.ToString() ?? "";     // > 0
                            string v_order_date = (v_worksheet.Cells[i, 13]).Value?.ToString() ?? "";
                            string v_transport = (v_worksheet.Cells[i, 14]).Value?.ToString() ?? "";
                            string v_reason_code = (v_worksheet.Cells[i, 15]).Value?.ToString() ?? "";
                            string v_eta_request = (v_worksheet.Cells[i, 16]).Value?.ToString() ?? "";
                            string v_actual_port = (v_worksheet.Cells[i, 17]).Value?.ToString() ?? "";
                            string v_eta_exp_rep = (v_worksheet.Cells[i, 19]).Value?.ToString() ?? "";
                            string v_invoice = (v_worksheet.Cells[i, 21]).Value?.ToString() ?? "";
                            string v_received_date = (v_worksheet.Cells[i, 22]).Value?.ToString() ?? "";
                            string v_qty_receive = (v_worksheet.Cells[i, 23]).Value?.ToString() ?? "";  // > 0              
                            string v_remark = (v_worksheet.Cells[i, 24]).Value?.ToString() ?? "";

                            //List<string> result = v_detailmodel.Split(',').ToList();
                            var row = new InvCkdSmqdOrderImportDto();
                            row.Guid = strGUID;

                            if (v_shop.Length > 1) row.ErrorDescription += "Độ dài Shop : " + v_shop + " không hợp lệ! , ";
                            else row.Shop = v_shop;

                            if (v_partNo.Length > 50) row.ErrorDescription += "Độ dài Part No : " + v_partNo + " không hợp lệ! , ";
                            else row.PartNo = v_partNo.Replace("-", "");

                            if (v_runno.Length > 50) row.ErrorDescription += "Độ dài Run No : " + v_runno + " không hợp lệ! , ";
                            else row.RunNo = v_runno;

                            if (v_cfc.Length > 4) row.ErrorDescription += "Độ dài Cfc : " + v_cfc + " không hợp lệ! , ";
                            else row.Cfc = v_cfc;

                            if (v_source.Length > 50) row.ErrorDescription += "Độ dài Supplier No : " + v_source + " không hợp lệ! , ";
                            else row.SupplierNo = v_source;

                            if (v_partname.Length > 200) row.ErrorDescription += "Độ dài Part Name : " + v_partname + " không hợp lệ! , ";
                            else row.PartName = v_partname;

                            if (v_order_no.Length > 50) row.ErrorDescription += "Độ dài Order No : " + v_order_no + " không hợp lệ! , ";
                            else row.OrderNo = v_order_no;
                            
                            if (v_transport.Length > 50) row.ErrorDescription += "Độ dài Transport : " + v_transport + " không hợp lệ! , ";
                            else row.Transport = v_transport;

                            if (v_reason_code.Length > 50) row.ErrorDescription += "Độ dài Reason Code : " + v_reason_code + " không hợp lệ! , ";
                            else row.ReasonCode = v_reason_code;

                            if (v_actual_port.Length > 50) row.ErrorDescription += "Độ dài Actual Eta Port : " + v_actual_port + " không hợp lệ! , ";
                            else row.ActualEtaPort = v_actual_port;

                            if (v_invoice.Length > 50) row.ErrorDescription += "Độ dài Invoice No : " + v_invoice + " không hợp lệ! , ";
                            else row.InvoiceNo = v_invoice;

                            if (v_remark.Length > 500) row.ErrorDescription += "Độ dài Remark : " + v_remark + " không hợp lệ! , ";
                            else row.Remark = v_remark;


                            #region Check Format Date
                            try
                            {
                                if (v_smqd_Date != "")
                                {
                                    row.SmqdDate = DateTime.Parse(v_smqd_Date);
                                }
                            }
                            catch (Exception e)
                            {
                                row.ErrorDescription += "Order Date " + v_smqd_Date + " không đúng định dạng! ,";
                            }

                            try
                            {   
                                if (v_order_date != "")
                                {
                                    row.OrderDate = DateTime.Parse(v_order_date);
                                }
                            }
                            catch (Exception e)
                            {
                                row.ErrorDescription += "Order Date " + v_order_date + " không đúng định dạng! ,";
                            }

                            try
                            {
                                if (v_eta_request != "")
                                {
                                    row.EtaRequest = DateTime.Parse(v_eta_request);
                                }
                            }
                            catch (Exception e)
                            {
                                row.ErrorDescription += "Eta Request " + v_eta_request + " không đúng định dạng! ,";
                            }

                            try
                            {
                                if (v_eta_exp_rep != "")
                                {
                                    row.EtaExpReply = DateTime.Parse(v_eta_exp_rep);
                                }
                            }
                            catch (Exception e)
                            {
                                row.ErrorDescription += "Order Date " + v_eta_exp_rep + " không đúng định dạng! ,";
                            }

                            try
                            {
                                if (v_received_date != "")
                                {
                                    row.ReceiveDate = DateTime.Parse(v_received_date);
                                }
                            }
                            catch (Exception e)
                            {
                                row.ErrorDescription += "Receive Date " + v_received_date + " không đúng định dạng! ,";
                            }

                            #endregion Check Format Date

                            #region Check Date
                            
                            if (row.OrderDate < row.SmqdDate)
                            {
                                row.ErrorDescription += "Order Date phải lớn hơn Smqd Date! ";
                            }

                            if (row.ReceiveDate < row.OrderDate)
                            {
                                row.ErrorDescription += "Receive Date phải lớn hơn Order Date! ";
                            }
                            
                            #endregion Check Date

                            #region Check Data    
                            try
                            {
                                row.OrderQty = v_order_qty != "" ? int.Parse(v_order_qty) : null;
                                if (row.OrderQty <= 0)
                                {
                                    throw new Exception();
                                }
                            }
                            catch (Exception e)
                            {
                                row.ErrorDescription += "Order Qty phải là số nguyên và lớn hơn 0! ,";
                            }

                            try
                            {
                                row.ReceiveQty = v_qty_receive != "" ? int.Parse( v_qty_receive) : null;
                                if (row.ReceiveQty <= 0)
                                {
                                    throw new Exception();
                                }
                            }
                            catch (Exception e)
                            {
                                row.ErrorDescription += "Receive Qty phải là số nguyên và lớn hơn 0! ,";
                            }

                            #endregion

                            listImport.Add(row);
                        }
                        else
                        {
                            break;
                        }

                    }
                    // import temp into db (bulkCopy)
                    if (listImport.Count > 0)
                    {
                        IEnumerable<InvCkdSmqdOrderImportDto> dataE = listImport.AsEnumerable();
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
                                    bulkCopy.DestinationTableName = "InvCkdSmqdOrder_T";
                                    bulkCopy.ColumnMappings.Add("Guid", "Guid");
                                    bulkCopy.ColumnMappings.Add("Shop", "Shop");
                                    bulkCopy.ColumnMappings.Add("SmqdDate", "SmqdDate");
                                    bulkCopy.ColumnMappings.Add("RunNo", "RunNo");
                                    bulkCopy.ColumnMappings.Add("Cfc", "Cfc");
                                    bulkCopy.ColumnMappings.Add("SupplierNo", "SupplierNo");
                                    bulkCopy.ColumnMappings.Add("PartNo", "PartNo");
                                    bulkCopy.ColumnMappings.Add("PartName", "PartName");
                                    bulkCopy.ColumnMappings.Add("OrderNo", "OrderNo");
                                    bulkCopy.ColumnMappings.Add("OrderQty", "OrderQty");
                                    bulkCopy.ColumnMappings.Add("OrderDate", "OrderDate");
                                    bulkCopy.ColumnMappings.Add("Transport", "Transport");
                                    bulkCopy.ColumnMappings.Add("ReasonCode", "ReasonCode");
                                    bulkCopy.ColumnMappings.Add("EtaRequest", "EtaRequest");
                                    bulkCopy.ColumnMappings.Add("ActualEtaPort", "ActualEtaPort");
                                    bulkCopy.ColumnMappings.Add("EtaExpReply", "EtaExpReply");
                                    bulkCopy.ColumnMappings.Add("InvoiceNo", "InvoiceNo");
                                    bulkCopy.ColumnMappings.Add("ReceiveDate", "ReceiveDate");
                                    bulkCopy.ColumnMappings.Add("ReceiveQty", "ReceiveQty");
                                    bulkCopy.ColumnMappings.Add("Remark", "Remark");
                                    bulkCopy.ColumnMappings.Add("OrderType", "OrderType");

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

        public async System.Threading.Tasks.Task MergeDataSmqdOrderNormal(string v_Guid, int v_OrderType)
        {
            string _sql = "Exec INV_CKD_SMQD_ORDER_MERGE @Guid, @OrderType";
            await _dapperRepo.QueryAsync<InvCkdSmqdOrderImportDto>(_sql, new 
            {
                Guid = v_Guid,
                OrderType = v_OrderType
            });
        }

        public async Task<PagedResultDto<InvCkdSmqdOrderImportDto>> GetMessageErrorImport(string v_Guid)
        {
            string _sql = "Exec INV_CKD_SMQD_ORDER_GET_LIST_ERROR_IMPORT @Guid";

            IEnumerable<InvCkdSmqdOrderImportDto> result = await _dapperRepo.QueryAsync<InvCkdSmqdOrderImportDto>(_sql, new
            {
                Guid = v_Guid
            });

            var listResult = result.ToList();
            var totalCount = listResult.Count();

            return new PagedResultDto<InvCkdSmqdOrderImportDto>(
                totalCount,
                listResult
               );
        }

        public async Task<FileDto> GetSmqdOrderListErrToExcel(string v_Guid)
        {
            string _sql = "Exec INV_CKD_SMQD_ORDER_GET_LIST_ERROR_IMPORT @Guid";

            IEnumerable<InvCkdSmqdOrderImportDto> result = await _dapperRepo.QueryAsync<InvCkdSmqdOrderImportDto>(_sql, new
            {
                Guid = v_Guid
            });

            var listResult = result.ToList();
            return _ExcelExporter.ExportToFileErr(listResult);
        }

    }
}

