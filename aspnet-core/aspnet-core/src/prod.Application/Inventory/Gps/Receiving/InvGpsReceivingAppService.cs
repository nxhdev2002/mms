using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Dapper.Repositories;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.EntityFrameworkCore.Uow;
using Abp.UI;
using FastMember;
using GemBox.Spreadsheet;
using Microsoft.EntityFrameworkCore;
using prod.Authorization;
using prod.Common;
using prod.Dto;
using prod.EntityFrameworkCore;
using prod.Inventory.GPS.Dto;
using prod.Inventory.GPS.Exporting;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace prod.Inventory.GPS
{
    [AbpAuthorize(AppPermissions.Pages_Gps_Receive_View)]
    public class InvGpsReceivingAppService : prodAppServiceBase, IInvGpsReceivingAppService
    {
        private readonly IDapperRepository<InvGpsReceiving, long> _dapperRepo;
        private readonly IRepository<InvGpsReceiving, long> _repo;
        private readonly IInvGpsReceivingExcelExporter _calendarListExcelExporter;

        public InvGpsReceivingAppService(IRepository<InvGpsReceiving, long> repo,
                                         IDapperRepository<InvGpsReceiving, long> dapperRepo,
                                        IInvGpsReceivingExcelExporter calendarListExcelExporter
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
        }

        [AbpAuthorize(AppPermissions.Pages_Gps_Receive_Edit)]
        public async Task CreateOrEdit(CreateOrEditInvGpsReceivingDto input)
        {
            if (input.Id == null) await Create(input);
            else await Update(input);
        }

        //CREATE
        private async Task Create(CreateOrEditInvGpsReceivingDto input)
        {
            await _dapperRepo.ExecuteAsync(@"
                  EXEC INV_GPS_RECEIVING_CREATE_DATA 
                                @p_LoginId,
                                @p_PartNo,
                                @p_PoNo,  
                                @p_BoxQty,
                                @p_Box,
                                @p_Qty,
                                @p_LotNo,
                                @p_ProdDate,
                                @p_ExpDate,
                                @p_ReceivedDate,
                                @p_Supplier,
                                @p_Dock
                                                                         
                ", new
            {
                p_LoginId = AbpSession.UserId,
                p_PartNo = input.PartNo,
                p_PoNo = input.PoNo,
                p_BoxQty = input.Boxqty,
                p_Box = input.Box,
                p_Qty = input.Qty,
                p_LotNo = input.LotNo,
                p_ProdDate = input.ProdDate,
                p_ExpDate = input.ExpDate,
                p_ReceivedDate = input.ReceivedDate,
                p_Supplier = input.Supplier,
                p_Dock = input.Dock,
                p_Id = input.Id,
            });
        }

        // EDIT
        private async Task Update(CreateOrEditInvGpsReceivingDto input)
        {
            await _dapperRepo.ExecuteAsync(@"
                  EXEC INV_GPS_RECEIVING_UPDATE_DATA 
                                @p_LoginId,
                                @p_PartNo,
                                @p_PoNo,  
                                @p_BoxQty,
                                @p_Box,
                                @p_Qty,
                                @p_LotNo,
                                @p_ProdDate,
                                @p_ExpDate,
                                @p_ReceivedDate,
                                @p_Supplier,
                                @p_Dock,
                                @p_Id
                                                                         
                ", new
            {
                p_LoginId = AbpSession.UserId,
                p_PartNo = input.PartNo,
                p_PoNo = input.PoNo,
                p_BoxQty = input.Boxqty,
                p_Box = input.Box,
                p_Qty = input.Qty,
                p_LotNo = input.LotNo,
                p_ProdDate = input.ProdDate,
                p_ExpDate = input.ExpDate,
                p_ReceivedDate = input.ReceivedDate,
                p_Supplier = input.Supplier,
                p_Dock = input.Dock,
                p_Id = input.Id,
            });
        }

        [AbpAuthorize(AppPermissions.Pages_Gps_Receive_Edit)]
        public async Task Delete(EntityDto input)
        {
            var mainObj = await _repo.FirstOrDefaultAsync(input.Id);
            CurrentUnitOfWork.GetDbContext<prodDbContext>().Remove(mainObj);
        }

        public async Task<PagedResultDto<InvGpsReceivingDto>> GetAll(GetInvGpsReceivingInput input)
        {
            string _sql = "Exec INV_GPS_RECEIVING_SEARCH  @p_pono, @p_partno, @p_supplier, @p_receivedDate_from, @p_receivedDate_to, @p_dock";

            IEnumerable<InvGpsReceivingDto> result = await _dapperRepo.QueryAsync<InvGpsReceivingDto>(_sql, new
            {
				p_pono = input.PoNo,
				p_partno = input.PartNo,
                p_supplier = input.Supplier,
                p_receivedDate_from = input.ReceivedDateFrom,
                p_receivedDate_to = input.ReceivedDateTo,
                p_dock = input.Dock
            });

            var listResult = result.ToList();

            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var totalCount = result.ToList().Count();

            return new PagedResultDto<InvGpsReceivingDto>(
               totalCount,
               pagedAndFiltered);
        }


        public async Task<FileDto> GetStockReceivingToExcel(GetInvGpsReceivingInput input)
        {
            string _sql = "Exec INV_GPS_RECEIVING_SEARCH @p_pono,@p_partno, @p_supplier, @p_receivedDate_from, @p_receivedDate_to, @p_dock";

            IEnumerable<InvGpsReceivingDto> result = await _dapperRepo.QueryAsync<InvGpsReceivingDto>(_sql, new
            {
				p_pono = input.PoNo,
				p_partno = input.PartNo,
                p_supplier = input.Supplier,
                p_receivedDate_from = input.ReceivedDateFrom,
                p_receivedDate_to = input.ReceivedDateTo,
                p_dock = input.Dock
            });

            var listResult = result.ToList();

            return _calendarListExcelExporter.ExportToFile(listResult);
        }

        //Import Receiving
        [AbpAuthorize(AppPermissions.Pages_Gps_Receive_Import)]
        public async Task<List<InvGpsReceivingImportDto>> ImportDataInvGpsReceiveFromExcel(byte[] fileBytes, string fileName)
        {
           
            try
            {

                List<InvGpsReceivingImportDto> listImport = new List<InvGpsReceivingImportDto>();
                using (var stream = new MemoryStream(fileBytes))
                {
                    SpreadsheetInfo.SetLicense("EF21-1FW1-HWZF-CLQH");
                    var xlWorkBook = ExcelFile.Load(stream);
                    var v_worksheet = xlWorkBook.Worksheets[0];

                    string strGUID = Guid.NewGuid().ToString("N");
                    string v_receiveDate = (v_worksheet.Cells[1, 1]).Value?.ToString() ?? "";
                    string v_supplier = (v_worksheet.Cells[1, 4]).Value?.ToString() ?? "";

                    for (int i = 3; i < v_worksheet.Rows.Count; i++)
                    {
						string v_poNo = (v_worksheet.Cells[i, 1]).Value?.ToString() ?? "";
						string v_partNo = (v_worksheet.Cells[i, 2]).Value?.ToString() ?? "";
                        string v_partName = (v_worksheet.Cells[i, 3]).Value?.ToString() ?? "";
                        string v_uom = (v_worksheet.Cells[i, 4]).Value?.ToString() ?? "";
                        string v_boxqty = (v_worksheet.Cells[i, 5]).Value?.ToString() ?? "";
                        string v_box = (v_worksheet.Cells[i, 6]).Value?.ToString() ?? "";
                        string v_qty = (v_worksheet.Cells[i, 7]).Value?.ToString() ?? "";
                        string v_lotNo = (v_worksheet.Cells[i, 8]).Value?.ToString() ?? "";
                        string v_workingDate = (v_worksheet.Cells[i,9]).Value?.ToString() ?? "";
                        string v_transactionDate = (v_worksheet.Cells[i, 10]).Value?.ToString() ?? "";
                        string v_dock = (v_worksheet.Cells[i, 11]).Value?.ToString() ?? "";
                        //  string v_shop = (v_worksheet.Cells[i, 10]).Value?.ToString() ?? "";

                        if (v_partNo != "")
                        {
                            var row = new InvGpsReceivingImportDto();
                            row.Guid = strGUID;
                            row.CreatorUserId = AbpSession.UserId;
                            row.ErrorDescription = "";
							row.PoNo = v_poNo;
                            row.Dock = v_dock;
							int dot = v_partNo.LastIndexOf('.');
                            if (dot != -1)
                            {
                                v_partNo = v_partNo;
                            }
                            if (v_partNo.Length > 14)
                            {
                                row.ErrorDescription += "PartNo " + v_partNo + " dài quá 14 kí tự! ";
                            }
                            else
                            {
                                row.PartNo = v_partNo;
                            }

                            row.PartName = v_partName;

                            if (v_uom.Length > 50)
                            {
                                row.ErrorDescription += "Đơn vị " + v_uom + " dài quá 50 kí tự! ";
                            }
                            else
                            {
                                row.Uom = v_uom;
                            }

                            try
                            {
                                if (string.IsNullOrEmpty(v_boxqty))
                                {
                                    row.BoxQty = null;
                                }
                                else
                                {
                                    row.BoxQty = Convert.ToInt32(v_boxqty);
                                    if (row.BoxQty < 0)
                                    {
                                        row.ErrorDescription += "Số lượng/box không được âm! ";
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                row.ErrorDescription += "Số lượng/box " + v_boxqty + " không phải là số! ";
                            }

                            try
                            {
                                if (string.IsNullOrEmpty(v_box))
                                {
                                    row.Box = null;
                                }
                                else
                                {
                                    row.Box = Convert.ToInt32(v_box);
                                    if (row.Box < 0)
                                    {
                                        row.ErrorDescription += "Số box không được âm! ";
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                row.ErrorDescription += "Số box " + v_box + " không phải là số! ";
                            }

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
                                        row.ErrorDescription += "Qty không được âm! ";
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                row.ErrorDescription += "Qty " + v_qty + " không phải là số! ";
                            }

                            if (row.BoxQty.HasValue && row.Box.HasValue && row.Qty.HasValue
                                && (row.Qty != row.Box * row.BoxQty))
                            {
                                row.ErrorDescription += "Tổng số lượng không khớp với số lượng/box và số box! ";
                            }

                            if (v_lotNo.Length > 50)
                            {
                                row.ErrorDescription += "Lot No " + v_lotNo + " dài quá 50 kí tự! ";
                            }
                            else
                            {
                                row.LotNo = v_lotNo;
                            }

                            try
                            {
                                if (string.IsNullOrEmpty(v_workingDate))
                                {
                                    row.ErrorDescription += "Ngày sản xuất không được để trống! ";
                                }
                                else
                                {
                                    row.ProdDate = DateTime.Parse(v_workingDate);
                                }
                            }
                            catch (Exception ex)
                            {
                                row.ErrorDescription += "Ngày sản xuất " + v_workingDate + " không đúng định dạng! ";
                            }

                            try
                            {
                                if (string.IsNullOrEmpty(v_transactionDate))
                                {
                                    row.ErrorDescription += "Ngày hết hạn không được để trống! ";
                                }
                                else
                                {
                                    row.ExpDate = DateTime.Parse(v_transactionDate);
                                    if (row.ProdDate.HasValue &&
                                        row.ProdDate > row.ExpDate)
                                    {
                                        row.ErrorDescription += "Ngày hết hạn phải sau Ngày sản xuất";
                                    }
                                    if (DateTime.Now > row.ExpDate)
                                    {
                                        row.ErrorDescription += "Sản phẩm đã hết hạn sản xuất! ";
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                row.ErrorDescription += "Ngày hết hạn " + v_transactionDate + " không đúng định dạng! ";
                            }

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

                            if (v_supplier.Length > 50)
                            {
                                row.ErrorDescription += "Supplier " + v_supplier + " dài quá 50 kí tự! ";
                            }
                            else
                            {
                                row.Supplier = v_supplier;
                            }

                            //if (v_shop.Length > 1)
                            //{
                            //    row.ErrorDescription += "Shop " + v_shop + " dài quá 1 kí tự! ";
                            //}
                            //else
                            //{
                            //    row.Shop = v_shop;
                            //}

                            listImport.Add(row);
                        }
                    }
                }
                // import temp into db (bulkCopy)
                if (listImport.Count > 0)
                {
                    IEnumerable<InvGpsReceivingImportDto> dataE = listImport.AsEnumerable();
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
                                bulkCopy.DestinationTableName = "InvGpsReceiving_T";
                                bulkCopy.ColumnMappings.Add("Guid", "Guid");
								bulkCopy.ColumnMappings.Add("PoNo", "PoNo");
								bulkCopy.ColumnMappings.Add("PartNo", "PartNo");
                                bulkCopy.ColumnMappings.Add("PartName", "PartName");
                                bulkCopy.ColumnMappings.Add("Uom", "Uom");
                                bulkCopy.ColumnMappings.Add("BoxQty", "BoxQty");
                                bulkCopy.ColumnMappings.Add("Box", "Box");
                                bulkCopy.ColumnMappings.Add("Qty", "Qty");
                                bulkCopy.ColumnMappings.Add("LotNo", "LotNo");
                                bulkCopy.ColumnMappings.Add("ProdDate", "ProdDate");
                                bulkCopy.ColumnMappings.Add("ExpDate", "ExpDate");
                                bulkCopy.ColumnMappings.Add("ReceivedDate", "ReceivedDate");
                                
                                bulkCopy.ColumnMappings.Add("Supplier", "Supplier");
                                bulkCopy.ColumnMappings.Add("Dock", "Dock");
                                //   bulkCopy.ColumnMappings.Add("Shop", "Shop");
                                bulkCopy.ColumnMappings.Add("ErrorDescription", "ErrorDescription");
                                bulkCopy.ColumnMappings.Add("CreatorUserId", "CreatorUserId");

                                bulkCopy.WriteToServer(table);
                                tran.Commit();
                            }
                        }
                        await conn.CloseAsync();
                    }
                }
                //SAO TỰ DƯNG THÊM MERGE VÀO ĐÂY ???? NANDE ???? WHY ????
                //insert GUID_USER 

                //string _sql = "Exec INV_GPS_RECEIVE_MERGE @Guid, @p_LoginId";
                //await _dapperRepo.QueryAsync<InvGpsReceivingImportDto>(_sql, new
                //{
                //    Guid = listImport[0].Guid,
                //    p_LoginId = AbpSession.UserId
                //});
                //
                //SAO TỰ DƯNG THÊM MERGE VÀO ĐÂY ???? NANDE ???? WHY ????
                return listImport;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(400, ex.Message);
            }
          
        }


        //Merge Data 
        public async Task MergeDataInvGpsReceiveStock(string v_Guid)
        {
            string _sql = "Exec INV_GPS_RECEIVE_MERGE @Guid, @p_LoginId";
            await _dapperRepo.QueryAsync<InvGpsReceivingImportDto>(_sql, new 
            {
                Guid = v_Guid, 
                p_LoginId = AbpSession.UserId
            });
        }

        // hiển thị lỗi import và export lỗi
        public async Task<PagedResultDto<InvGpsReceivingImportDto>> GetMessageErrorImport(string v_Guid)
        {
            string _sql = "Exec INV_GPS_RECEIVE_GET_LIST_ERROR_IMPORT @Guid";

            IEnumerable<InvGpsReceivingImportDto> result = await _dapperRepo.QueryAsync<InvGpsReceivingImportDto>(_sql, new
            {
                Guid = v_Guid
            });

            var listResult = result.ToList();
            var totalCount = listResult.Count();

            return new PagedResultDto<InvGpsReceivingImportDto>(
                totalCount,
               listResult
               );
        }

        public async Task<FileDto> GetListErrToExcel(string v_Guid)
        {
            string _sql = "Exec INV_GPS_RECEIVE_GET_LIST_ERROR_IMPORT @Guid";

            IEnumerable<InvGpsReceivingImportDto> result = await _dapperRepo.QueryAsync<InvGpsReceivingImportDto>(_sql, new
            {
                Guid = v_Guid
            });

            var exportToExcel = result.ToList();
            return _calendarListExcelExporter.ExportToFileErr(exportToExcel);
        }

        public async Task<CheckDto> spCheckPartNoMaterial(string PartNo)
        {
            string _sql = "Exec INV_GPS_RECEIVING_CHECK_EXIST_PARTNO @p_partNo";
            IEnumerable<CheckDto> result = await _dapperRepo.QueryAsync<CheckDto>(_sql, new
            {
                p_partNo = PartNo
            });

            return result.FirstOrDefault();
        }
    }
}
