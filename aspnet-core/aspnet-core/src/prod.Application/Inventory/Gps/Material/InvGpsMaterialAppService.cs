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
using NPOI.OpenXmlFormats.Dml.Diagram;
using NPOI.SS.Formula.Functions;
using NPOI.SS.UserModel;
using prod.Authorization;
using prod.Common;
using prod.Dto;
using prod.EntityFrameworkCore;
using prod.HistoricalData;
using prod.Inventory.CKD.Dto;
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
    [AbpAuthorize(AppPermissions.Pages_Gps_Master_Material_View)]
    public class InvGpsMaterialAppService : prodAppServiceBase, IInvGpsMaterialAppService
    {
        private readonly IDapperRepository<InvGpsMaterial, long> _dapperRepo;
        private readonly IRepository<InvGpsMaterial, long> _repo;
        private readonly IInvGpsMaterialExcelExporter _calendarListExcelExporter;
        private readonly IHistoricalDataAppService _historicalDataAppService;


        public InvGpsMaterialAppService(IRepository<InvGpsMaterial, long> repo,
                                         IDapperRepository<InvGpsMaterial, long> dapperRepo,
                                        IInvGpsMaterialExcelExporter calendarListExcelExporter,
                                        IHistoricalDataAppService historicalDataAppService
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
            _historicalDataAppService = historicalDataAppService;
        }

        public async Task<List<string>> GetGpsMaterialHistory(GetInvGpsMaterialHistoryInput input)
        {
            return await _historicalDataAppService.GetHistoricalDataById(input.Id, input.TableName, "Id");
        }

        public async Task<FileDto> GetHistoricalDataToExcel(GetInvGpsMaterialHistoryExcelInput input)
        {
            var data = await _historicalDataAppService.GetHistoricalDataById(input.Id, input.TableName, "Id");
            return _calendarListExcelExporter.ExportToHistoricalFile(data);
        }

        public async Task<List<long?>> GetChangedRecords()
        {
            return await _historicalDataAppService.GetChangedRecordIds("InvGpsMaterial");
        }

        [AbpAuthorize(AppPermissions.Pages_Gps_Master_Material_Edit)]
        public async Task CreateOrEdit(CreateOrEditInvGpsMaterialDto input)
        {
            if (input.Id == null) await Create(input);
            else await Update(input);
        }

        //CREATE
        private async Task Create(CreateOrEditInvGpsMaterialDto input)
        {
            var mainObj = ObjectMapper.Map<InvGpsMaterial>(input);

            await CurrentUnitOfWork.GetDbContext<prodDbContext>().AddAsync(mainObj);
        }

        // EDIT
        private async Task Update(CreateOrEditInvGpsMaterialDto input)
        {
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                var mainObj = await _repo.GetAll()
                .FirstOrDefaultAsync(e => e.Id == input.Id);

                var mainObjToUpdate = ObjectMapper.Map(input, mainObj);
            }
        }

        public async Task<PagedResultDto<InvGpsMaterialDto>> GetAll(GetInvGpsMaterialInput input)
        {
            string _sql = "Exec INV_GPS_MATERIAL_SEARCH @p_part_no, @p_supplier, @p_part_type, @p_is_exp_date, @p_part_group, @p_local_import, @p_is_active,@p_part_name,@p_category,@p_location ";

            IEnumerable<InvGpsMaterialDto> result = await _dapperRepo.QueryAsync<InvGpsMaterialDto>(_sql, new
            {
                p_part_no = input.PartNo,
                p_supplier = input.Supplier,
                p_part_type = input.PartType,
                p_is_exp_date = input.IsExpDate,
                p_part_group = input.PartGroup,
                p_local_import = input.LocalImport,
                p_is_active = input.IsActive,
                p_part_name = input.PartName,
                p_category = input.Category,
                p_location = input.Location,
            });

            var listResult = result.ToList();

            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var totalCount = result.ToList().Count();

            return new PagedResultDto<InvGpsMaterialDto>(
                totalCount,
                pagedAndFiltered);
        }

        public async Task<FileDto> GetMaterialToExcel(GetInvGpsMaterialInput input)
        {
            string _sql = "Exec INV_GPS_MATERIAL_SEARCH @p_part_no, @p_supplier, @p_part_type, @p_is_exp_date, @p_part_group, @p_local_import, @p_is_active,@p_part_name,@p_category,@p_location ";

            IEnumerable<InvGpsMaterialDto> result = await _dapperRepo.QueryAsync<InvGpsMaterialDto>(_sql, new
            {
                p_part_no = input.PartNo,
                p_supplier = input.Supplier,
                p_part_type = input.PartType,
                p_is_exp_date = input.IsExpDate,
                p_part_group = input.PartGroup,
                p_local_import = input.LocalImport,
                p_is_active = input.IsActive,
                p_part_name = input.PartName,
                p_category = input.Category,
                p_location = input.Location,
            });

            var listResult = result.ToList();

            return _calendarListExcelExporter.ExportToFile(listResult);
        }


        [AbpAuthorize(AppPermissions.Pages_Gps_Master_Material_Import)]
        public async Task<List<ImportInvGpsMaterialDto>> ImportInvGpsMaterialExcel(byte[] fileBytes, string fileName)
        {
            try
            {
                List<ImportInvGpsMaterialDto> listImport = new List<ImportInvGpsMaterialDto>();
                using (var stream = new MemoryStream(fileBytes))
                {
                    SpreadsheetInfo.SetLicense("EF21-1FW1-HWZF-CLQH");
                    var xlWorkBook = ExcelFile.Load(stream);
                    var v_worksheet = xlWorkBook.Worksheets[0];
                    DataFormatter formatter = new DataFormatter();
                    DateTime dateTime = DateTime.Now;

                    string strGUID = Guid.NewGuid().ToString("N");

                    for (int i = 7; i < v_worksheet.Rows.Count; i++)
                    {
                        string v_partNo = (v_worksheet.Cells[i, 1]).Value?.ToString().Replace(" ", "") ?? "";

                        if (v_partNo != "")
                        {

                            string v_partName = (v_worksheet.Cells[i, 2]).Value?.ToString() ?? "";
                            string v_partNameVn = (v_worksheet.Cells[i, 3]).Value?.ToString() ?? "";
                            string v_partType = v_worksheet.Cells[i, 4].Value?.ToString() ?? "";
                            string v_uses = v_worksheet.Cells[i, 5].Value?.ToString() ?? "";
                            string v_spec = v_worksheet.Cells[i, 6].Value?.ToString() ?? "";
                            string v_isExpDate = v_worksheet.Cells[i, 7].Value?.ToString() ?? "";
                            string v_partGroup = v_worksheet.Cells[i, 8].Value?.ToString() ?? "";
                            string v_price = v_worksheet.Cells[i, 9].Value?.ToString() ?? "";
                            string v_currency = v_worksheet.Cells[i, 10].Value?.ToString() ?? "";
                            string v_priceConvert = v_worksheet.Cells[i, 11].Value?.ToString() ?? "";                            
                            string v_uom = v_worksheet.Cells[i, 12].Value?.ToString() ?? "";
                            string v_supplierName = v_worksheet.Cells[i, 13].Value?.ToString() ?? "";
                            string v_supplier = v_worksheet.Cells[i, 14].Value?.ToString() ?? "";
                            string v_localImport = v_worksheet.Cells[i, 15].Value?.ToString() ?? "";
                            string v_leadTime = ((v_worksheet.Cells[i, 16]).Value?.ToString() ?? "0").Trim();
                            string v_leadTimeForecas =((v_worksheet.Cells[i, 17]).Value?.ToString() ?? "0").Trim();
                            string v_minLot = (v_worksheet.Cells[i, 18]).Value?.ToString() ?? "0";
                            string v_boxQty = (v_worksheet.Cells[i, 19]).Value?.ToString() ?? "0";
                            string v_remark = v_worksheet.Cells[i, 20].Value?.ToString() ?? "";
                            int v_palletL = Int32.Parse((v_worksheet.Cells[i, 21]).Value?.ToString() ?? "0");
                            int v_palletR = Int32.Parse((v_worksheet.Cells[i, 22]).Value?.ToString() ?? "0");
                            int v_palletH = Int32.Parse((v_worksheet.Cells[i, 23]).Value?.ToString() ?? "0");



                            var row = new ImportInvGpsMaterialDto();
                            row.Guid = strGUID;
                            row.CreatorUserId = AbpSession.UserId;
                            row.ErrorDescription = "";

                            if (!v_partNo.Contains(".00")) v_partNo += ".00";

                            row.ErrorDescription += (v_partNo.Length > 20) ? "Oracle part " + v_partNo + " dài quá 20 kí tự! " : "";
                            row.PartNo = (v_partNo.Length <= 20) ? v_partNo : row.PartNo;

                            row.ErrorDescription += (v_partName.Length > 300) ? "Oracle name " + v_partName + " dài quá 300 kí tự! " : "";
                            row.PartName = (v_partName.Length <= 300) ? v_partName.ToUpper() : row.PartName;

                            row.ErrorDescription += (v_partNameVn.Length > 300) ? "Tên Tiếng Việt (Tham khảo) " + v_partNameVn + " dài quá 300 kí tự! " : "";
                            row.PartNameVn = (v_partNameVn.Length <= 300) ? v_partNameVn : row.PartName;

                            row.ErrorDescription += (v_partType.Length > 50) ? "Loại hàng " + v_partType + " dài quá 50 kí tự! " : "";
                            row.PartType = (v_partType.Length <= 50) ? v_partType : row.PartType;

                            row.ErrorDescription += (v_uses.Length > 50) ? "Mục đích sử dụng " + v_uses + " dài quá 50 kí tự! " : "";
                            row.PurposeOfUse = (v_uses.Length <= 50) ? v_uses : row.PurposeOfUse;

                            row.ErrorDescription += (v_spec.Length > 1000) ? "Đặc tả hàng " + v_spec + " dài quá 1000 kí tự! " : "";
                            row.Spec = (v_spec.Length <= 1000) ? v_spec : row.Spec;

                            row.ErrorDescription += (v_isExpDate.Length > 1) ? "Kiểm soát hạn sử dụng " + v_isExpDate + " dài quá 1 kí tự! " : "";
                            row.HasExpiryDate = (v_isExpDate.Length <= 1) ? v_isExpDate : row.HasExpiryDate;

                            row.ErrorDescription += (v_partGroup.Length > 50) ? "Nhóm hàng " + v_partGroup + " dài quá 50 kí tự! " : "";
                            row.PartGroup = (v_partGroup.Length <= 50) ? v_partGroup : row.PartGroup;



                            row.ErrorDescription += (v_currency.Length > 50) ? "Tiền tệ " + v_currency + " dài quá 50 kí tự! " : "";
                            row.Currency = (v_currency.Length <= 50) ? v_currency : row.Currency;

                            row.ErrorDescription += (v_uom.Length > 50) ? "Đơn vị tính " + v_uom + " dài quá 50 kí tự! " : "";
                            row.UOM = (v_uom.Length <= 50) ? v_uom : row.Currency;



                            row.ErrorDescription += (v_supplierName.Length > 200) ? "Tên nhà cung cấp " + v_supplierName + " dài quá 200 kí tự! " : "";
                            row.SupplierName = (v_supplierName.Length <= 200) ? v_supplierName : row.SupplierName;

                            row.ErrorDescription += (v_supplier.Length > 50) ? "Tên NCC rút gọn " + v_supplier + " dài quá 50 kí tự! " : "";
                            row.SupplierNo = (v_supplier.Length <= 50) ? v_supplier : row.SupplierNo;


                            row.ErrorDescription += (v_localImport.Length > 50) ? "Hàng nội địa/ Nhập khẩu " + v_localImport + " dài quá 50 kí tự! " : "";
                            row.LocalImport = (v_localImport.Length <= 50) ? v_localImport : row.LocalImport;

                            row.ErrorDescription += (v_remark.Length > 4000) ? "Ghi chú " + v_remark + " dài quá 4000 kí tự! " : "";
                            row.Remark = (v_remark.Length <= 4000) ? v_remark : row.Remark;




                            v_priceConvert = v_priceConvert.Replace(",", "").Replace(" ", "");


                            //if (string.IsNullOrWhiteSpace(v_priceConvert))
                            //{
                            //    row.PriceConvert = 0;
                            //}
                            //else if (long.TryParse(v_priceConvert.Split('.')[0], out d_priceConverse))
                            //{
                            //    row.PriceConvert = d_priceConverse;
                            //}
                            //else
                            //{
                            //    row.ErrorDescription += "Đơn giá quy đổi " + v_priceConvert + " không phải là số! ";
                            //}

                            int d_priceConverse;
                            try
                            {   
                                row.ConvertPrice = int.Parse(v_priceConvert != "" ? v_priceConvert.Split('.')[0] : "0");
                            }
                            catch (Exception e)
                            {
                                row.ErrorDescription += "Đơn giá quy đổi " + v_priceConvert + " không phải là số! ";
                            }




                            try
                            {
                                row.Price = Convert.ToDecimal(v_price != "" ? v_price : 0);
                            }
                            catch (Exception e)
                            {
                                row.Price = 0;
                            }

                            try
                            {
                                row.BoxQty = Convert.ToDecimal(v_boxQty != "" ? v_boxQty : 0);
                            }
                            catch (Exception e)
                            {
                                row.BoxQty = 0;
                            }


                            if (string.IsNullOrWhiteSpace(v_leadTime))
                            {                               
                                row.LeadTime = 0;
                            }
                            else
                            {                                
                                row.LeadTime = int.Parse(v_leadTime);
                            }

                            if (string.IsNullOrWhiteSpace(v_leadTimeForecas))
                            {
                                row.LeadTimeForecast = 0;
                            }
                            else
                            {
                                row.LeadTimeForecast = int.Parse(v_leadTimeForecas);
                            }

                            try
                            {
                                row.MinLot = Convert.ToDecimal(v_minLot != "" ? v_minLot : 0);
                            }
                            catch (Exception e)
                            {
                                row.MinLot = 0;
                            }

                            try
                            {
                                row.PalletL = v_palletL;
                            }
                            catch (Exception ex)
                            {
                                row.ErrorDescription += "PalletL đóng gói " + v_palletL + " không phải là số! ";
                            }
                            try
                            {
                                row.PalletR = v_palletR;
                            }
                            catch (Exception ex)
                            {
                                row.ErrorDescription += "PalletR đóng gói " + v_palletR + " không phải là số! ";
                            }
                            try
                            {
                                row.PalletH = v_palletH;
                            }
                            catch (Exception ex)
                            {
                                row.ErrorDescription += "PalletH đóng gói " + v_palletL + " không phải là số! ";
                            }                           
                      

                            listImport.Add(row);
                        }
                    }

                }
                // import temp into db (bulkCopy)
                if (listImport.Count > 0)
                {
                    IEnumerable<ImportInvGpsMaterialDto> dataE = listImport.AsEnumerable();
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
                                bulkCopy.DestinationTableName = "InvGpsMaterial_T";
                                bulkCopy.ColumnMappings.Add("Guid", "Guid");
                                bulkCopy.ColumnMappings.Add("PartNo", "PartNo");
                                bulkCopy.ColumnMappings.Add("PartName", "PartName");
                                bulkCopy.ColumnMappings.Add("PartNameVn", "PartNameVn");
                                bulkCopy.ColumnMappings.Add("PartType", "PartType");
                                bulkCopy.ColumnMappings.Add("PurposeOfUse", "PurposeOfUse");
                                bulkCopy.ColumnMappings.Add("Spec", "Spec");
                                bulkCopy.ColumnMappings.Add("HasExpiryDate", "HasExpiryDate");
                                bulkCopy.ColumnMappings.Add("PartGroup", "PartGroup");
                                bulkCopy.ColumnMappings.Add("Price", "Price");
                                bulkCopy.ColumnMappings.Add("Currency", "Currency");
                                bulkCopy.ColumnMappings.Add("ConvertPrice", "ConvertPrice");
                                bulkCopy.ColumnMappings.Add("UOM", "UOM");
                                bulkCopy.ColumnMappings.Add("SupplierName", "SupplierName");
                                bulkCopy.ColumnMappings.Add("SupplierNo", "SupplierNo");
                                bulkCopy.ColumnMappings.Add("LocalImport", "LocalImport");
                                bulkCopy.ColumnMappings.Add("LeadTime", "LeadTime");
                                bulkCopy.ColumnMappings.Add("LeadTimeForecast", "LeadTimeForecast");
                                bulkCopy.ColumnMappings.Add("MinLot", "MinLot");
                                bulkCopy.ColumnMappings.Add("BoxQty", "BoxQty");
                                bulkCopy.ColumnMappings.Add("Remark", "Remark");
                                bulkCopy.ColumnMappings.Add("PalletL", "PalletL");
                                bulkCopy.ColumnMappings.Add("PalletR", "PalletR");
                                bulkCopy.ColumnMappings.Add("PalletH", "PalletH");
                                bulkCopy.ColumnMappings.Add("ErrorDescription", "ErrorDescription");
                                bulkCopy.ColumnMappings.Add("CreatorUserId", "CreatorUserId");

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


        public async Task MergeDataInvGpsMaterial(string v_Guid)
        {
            string _sql = "Exec INV_GPS_MATERIAL_MERGE @Guid";
            await _dapperRepo.QueryAsync<ImportInvGpsMaterialDto>(_sql, new { Guid = v_Guid });
        }

        // hiển thị lỗi import và export lỗi
        public async Task<PagedResultDto<ImportInvGpsMaterialDto>> GetMessageErrorImport(string v_Guid)
        {
            string _sql = "Exec INV_GPS_MATERIAL_GET_LIST_ERROR_IMPORT @Guid";

            IEnumerable<ImportInvGpsMaterialDto> result = await _dapperRepo.QueryAsync<ImportInvGpsMaterialDto>(_sql, new
            {
                Guid = v_Guid
            });

            var listResult = result.ToList();
            var totalCount = listResult.Count();

            return new PagedResultDto<ImportInvGpsMaterialDto>(
                totalCount,
               listResult
               );
        }

        public async Task<FileDto> GetListErrToExcel(string v_Guid)
        {
            string _sql = "Exec INV_GPS_MATERIAL_GET_LIST_ERROR_IMPORT @Guid";

            IEnumerable<ImportInvGpsMaterialDto> result = await _dapperRepo.QueryAsync<ImportInvGpsMaterialDto>(_sql, new
            {
                Guid = v_Guid
            });

            var exportToExcel = result.ToList();
            return _calendarListExcelExporter.ExportToFileErr(exportToExcel);
        }


        public async Task<bool> CheckExistPartInGpsMaterial(string PartNoNew, string PartNoOld)
        {
            string _sqlSearch = "Exec INV_GPS_MATERIAL_CHECK_PARTNO_EXIST @p_PartNoNew, @p_PartNoOld";

            IEnumerable<CheckExistDto> result = await _dapperRepo.QueryAsync<CheckExistDto>(_sqlSearch, new { p_PartNoNew = PartNoNew, p_PartNoOld = PartNoOld });

            var totalCount = result.ToList().Count();

            if (totalCount > 0) return true;
            else return false;
        }
    }
}
