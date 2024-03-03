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
using NPOI.SS.UserModel;
using prod.Authorization;
using prod.Dto;
using prod.EntityFrameworkCore;
using prod.Inventory.DRM.Dto;
using prod.Inventory.DRM.Exporting;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using prod.Common;

namespace prod.Inventory.DRM
{
    [AbpAuthorize(AppPermissions.Pages_DMIHP_GR_ImportPlan_View)]
    public class InvDrmImportPlanAppService : prodAppServiceBase, IInvDrmImportPlanAppService
    {
        private readonly IDapperRepository<InvDrmImportPlan, long> _dapperRepo;
        private readonly IRepository<InvDrmImportPlan, long> _repo;
        private readonly IInvDrmImportPlanExcelExporter _calendarListExcelExporter;

        public InvDrmImportPlanAppService(IRepository<InvDrmImportPlan, long> repo,
                                         IDapperRepository<InvDrmImportPlan, long> dapperRepo,
                                        IInvDrmImportPlanExcelExporter calendarListExcelExporter
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
        }

        public async Task CreateOrEdit(CreateOrEditInvDrmImportPlanDto input)
        {
            if (input.Id == null) await Create(input);
            else await Update(input);
        }

        //CREATE
        private async Task Create(CreateOrEditInvDrmImportPlanDto input)
        {
            var mainObj = ObjectMapper.Map<InvDrmImportPlan>(input);

            await CurrentUnitOfWork.GetDbContext<prodDbContext>().AddAsync(mainObj);
        }

        // EDIT
        private async Task Update(CreateOrEditInvDrmImportPlanDto input)
        {
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                var mainObj = await _repo.GetAll()
                .FirstOrDefaultAsync(e => e.Id == input.Id);

                if (mainObj != null)
                {
                    mainObj.Ata = input.Ata;

                    await _repo.UpdateAsync(mainObj);
                    await CurrentUnitOfWork.SaveChangesAsync();
                }
            }
        }


        [AbpAuthorize(AppPermissions.Pages_DMIHP_GR_ImportPlan_Confirm)]
        public async Task<int> ConFirmStatus(string p_container_id, string p_status)
        {
            try
            {
                string _sql = @"EXEC INV_DRM_IMPORT_PLAN_UPDATE_STATUS @p_container_id, @p_status, @p_status_date, @p_user_id";
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


        [AbpAuthorize(AppPermissions.Pages_DMIHP_GR_ImportPlan_Confirm)]
        public async Task<int> ConFirmStatusMultiCkb(string listIdStatus, string status)
        {
            try
            {
                string _sql = @"EXEC INV_DRM_IMPORT_PLAN_UPDATE_STATUS @p_list_id_status,@p_status,@p_status_date, @p_user_id";
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

        public async Task<PagedResultDto<InvDrmImportPlanDto>> GetAll(GetInvDrmImportPlanInput input)
        {
            var filtered = _repo.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.SupplierNo), e => e.SupplierNo.Contains(input.SupplierNo))
                .WhereIf(input.Etd.HasValue, t => t.Etd == input.Etd.Value.Date)
                .WhereIf(input.Eta.HasValue, t => t.Eta == input.Eta.Value.Date)
                .WhereIf(!string.IsNullOrWhiteSpace(input.Cfc), e => e.Cfc.Contains(input.Cfc))
                .WhereIf(!string.IsNullOrWhiteSpace(input.PartCode), e => e.PartCode.Contains(input.PartCode))
                .WhereIf(!string.IsNullOrWhiteSpace(input.PartNo), e => e.PartNo.Contains(input.PartNo))


                ;
            var pageAndFiltered = filtered.OrderByDescending  (s => s.Etd);


            var system = from o in pageAndFiltered
                         select new InvDrmImportPlanDto
                         {
                             Id = o.Id,
                             SupplierNo = o.SupplierNo,
                             Etd = o.Etd,
                             Eta = o.Eta,
                             ShipmentNo = o.ShipmentNo,
                             Cfc = o.Cfc,
                             PartCode = o.PartCode,
                             PartNo = o.PartNo,
                             PartName = o.PartName,
                             Qty = o.Qty,
                             PackingMonth = o.PackingMonth,
                             DelayEtd = o.DelayEtd,
                             DelayEta = o.DelayEta,
                             Remark = o.Remark,
                             Ata = o.Ata,
                             Status = o.Status,
                         };

            var totalCount = await filtered.CountAsync();
            var paged = system.PageBy(input);

            return new PagedResultDto<InvDrmImportPlanDto>(
                totalCount,
                 await paged.ToListAsync()
            );
        }

        public async Task<FileDto> GetDrmImportPlanToExcel(GetDrmImportPlanExportInput input)
        {
            var query = from o in _repo.GetAll()
                        select new InvDrmImportPlanDto
                        {
                            Id = o.Id,
                            SupplierNo = o.SupplierNo,
                            Etd = o.Etd,
                            Eta = o.Eta,
                            ShipmentNo = o.ShipmentNo,
                            Cfc = o.Cfc,
                            PartCode = o.PartCode,
                            PartNo = o.PartNo,
                            PartName = o.PartName,
                            Qty = o.Qty,
                            PackingMonth = o.PackingMonth,
                            DelayEtd = o.DelayEtd,
                            DelayEta = o.DelayEta,
                            Remark = o.Remark,
                            Ata = o.Ata,
                        };
            var exportToExcel = await query.ToListAsync();
            return _calendarListExcelExporter.ExportToFile(exportToExcel);
        }

        [AbpAuthorize(AppPermissions.Pages_DMIHP_GR_ImportPlan_Import)]
        public async Task<List<InvDrmImportPlanImportDto>> ImportDataInvDrmImportPlanFromExcel(byte[] fileBytes, string fileName)
        {
            try
            {
                List<InvDrmImportPlanImportDto> listImport = new List<InvDrmImportPlanImportDto>();
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

                    for (int i = 8; i < v_worksheet.Rows.Count; i++)
                    {
                        string v_partNo = (v_worksheet.Cells[i, 7]).Value?.ToString().Replace(" ", "") ?? "";

                        if (v_partNo != "")
                        {
                            var row = new InvDrmImportPlanImportDto();
                            row.Guid = strGUID;
                            row.ErrorDescription = "";

                            if (v_partNo.Length > 12)
                            {
                                row.ErrorDescription += "PartNo " + v_partNo + " dài quá 12 kí tự! ";
                            }
                            else
                            {
                                row.PartNo = v_partNo;
                            }

                            string v_exp = (v_worksheet.Cells[i, 1]).Value?.ToString() ?? "";
                            if (v_exp.Length > 10)
                            {
                                row.ErrorDescription += "EXP " + v_exp + " dài quá 10 kí tự! ";
                            }
                            else
                            {
                                row.SupplierNo = v_exp;
                            }

                            string v_etd = (v_worksheet.Cells[i, 2]).Value?.ToString() ?? "";
                            try
                            {
                                if(string.IsNullOrEmpty(v_etd))
                                {
                                    row.ErrorDescription += "ETD không được để trống! ";
                                }
                                else
                                {
                                    row.Etd = DateTime.Parse(v_etd);
                                }
                            }catch(Exception ex)
                            {
                                row.ErrorDescription += "ETD " + v_etd + " không đúng định dạng! ";
                            }

                            string v_eta = (v_worksheet.Cells[i, 3]).Value?.ToString() ?? "";
                            try
                            {
                                if (string.IsNullOrEmpty(v_eta))
                                {
                                    row.ErrorDescription += "ETA không được để trống! ";
                                }
                                else
                                {
                                    row.Eta = DateTime.Parse(v_eta);
                                    if (row.Etd.HasValue &&
                                        row.Etd > row.Eta)
                                    {
                                        row.ErrorDescription += " ETA phải sau ETD";
                                    }    

                                }
                            }
                            catch (Exception ex)
                            {
                                row.ErrorDescription += "ETA " + v_eta + " không đúng định dạng! ";
                            }

                            string v_shipmentno = v_worksheet.Cells[i, 4].Value?.ToString() ?? "";
                            if (v_shipmentno.Length > 10)
                            {
                                row.ErrorDescription += "Shipment no " + v_shipmentno + " dài quá 10 kí tự! ";
                            }
                            else
                            {
                                row.ShipmentNo = v_shipmentno;
                            }

                            string v_model = v_worksheet.Cells[i, 5].Value?.ToString() ?? "";
                            if (v_model.Length > 4)
                            {
                                row.ErrorDescription += "Cfc " + v_model + " dài quá 4 kí tự! ";
                            }
                            else
                            {
                                row.Cfc = v_model;
                            }

                            if (v_worksheet.Cells[i, 6].Value?.ToString().Length > 60)
                            {
                                row.ErrorDescription += "" + v_worksheet.Cells[i, 6].Value?.ToString() + "vượt quá độ dài";
                            }
                            else
                            {
                                row.PartCode = v_worksheet.Cells[i, 6].Value?.ToString() ?? "";
                            }

                            if (v_worksheet.Cells[i, 8].Value?.ToString().Length > 300)
                            {
                                row.ErrorDescription += "" + v_worksheet.Cells[i, 8].Value?.ToString() + "vượt quá độ dài";
                            }
                            else
                            {
                                row.PartName = v_worksheet.Cells[i, 8].Value?.ToString().Replace(" ", "") ?? "";
                            }

                            string v_qty = v_worksheet.Cells[i, 9].Value?.ToString() ?? "";
                            try
                            {
                                if(string.IsNullOrEmpty(v_qty))
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

                            //Hiện tại Packing month trong Template có dạng ETA.Mar.20 
                            string v_packingmonth = v_worksheet.Cells[i, 10].Value?.ToString() ?? "";
                            try
                            {
                                if (string.IsNullOrEmpty(v_packingmonth))
                                {
                                    row.ErrorDescription += "Packing Month không được để trống! ";
                                }
                                else
                                {
                                    row.PackingMonth = DateTime.Parse(v_packingmonth.Replace("ETA", "1"));
                                }
                            }
                            catch (Exception ex)
                            {
                                row.ErrorDescription += "Packing Month " + v_packingmonth + " không đúng định dạng! ";
                            }

                            string v_delayetd = v_worksheet.Cells[i, 12].Value?.ToString() ?? "";
                            try
                            {
                                if (string.IsNullOrEmpty(v_delayetd))
                                {
                                    row.DelayEtd = null;
                                }
                                else
                                {
                                    row.DelayEtd = DateTime.Parse(v_delayetd);
                                }
                            }
                            catch (Exception ex)
                            {
                                row.ErrorDescription += "Delay Shipment ETD " + v_delayetd + " không đúng định dạng! ";
                            }

                            string v_delayeta = v_worksheet.Cells[i, 13].Value?.ToString() ?? "";
                            try
                            {
                                if (string.IsNullOrEmpty(v_delayeta))
                                {
                                    row.DelayEta = null;
                                }
                                else
                                {
                                    row.DelayEta = DateTime.Parse(v_delayeta);
                                }
                            }
                            catch (Exception ex)
                            {
                                row.ErrorDescription += "Delay Shipment ETA " + v_delayeta + " không đúng định dạng! ";
                            }

                            row.Remark = v_worksheet.Cells[i, 14].Value?.ToString() ?? "";

                            string v_atatmv = v_worksheet.Cells[i, 16].Value?.ToString() ?? "";
                            try
                            {
                                if (string.IsNullOrEmpty(v_atatmv))
                                {
                                    row.Ata = null;
                                }
                                else
                                {
                                    row.Ata = DateTime.Parse(v_atatmv);
                                }
                            }
                            catch (Exception ex)
                            {
                                row.ErrorDescription += "ATA TMV " + v_atatmv + " không đúng định dạng! ";
                            }

                            listImport.Add(row);
                        }
                    }

                }
                // import temp into db (bulkCopy)
                if (listImport.Count > 0)
                {
                    IEnumerable<InvDrmImportPlanImportDto> dataE = listImport.AsEnumerable();
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
                                bulkCopy.DestinationTableName = "InvDrmImportPlan_T";
                                bulkCopy.ColumnMappings.Add("Guid", "Guid");
                                bulkCopy.ColumnMappings.Add("SupplierNo", "SupplierNo");
                                bulkCopy.ColumnMappings.Add("Etd", "Etd");
                                bulkCopy.ColumnMappings.Add("Eta", "Eta");
                                bulkCopy.ColumnMappings.Add("ShipmentNo", "ShipmentNo");
                                bulkCopy.ColumnMappings.Add("Cfc", "Cfc");
                                bulkCopy.ColumnMappings.Add("PartCode", "PartCode");
                                bulkCopy.ColumnMappings.Add("PartNo", "PartNo");
                                bulkCopy.ColumnMappings.Add("PartName", "PartName");
                                bulkCopy.ColumnMappings.Add("Qty", "Qty");
                                bulkCopy.ColumnMappings.Add("PackingMonth", "PackingMonth");
                                bulkCopy.ColumnMappings.Add("DelayEtd", "DelayEtd");
                                bulkCopy.ColumnMappings.Add("DelayEta", "DelayEta");
                                bulkCopy.ColumnMappings.Add("Remark", "Remark");
                                bulkCopy.ColumnMappings.Add("Ata", "Ata");
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

        public async Task MergeDataInvDrmImportPlan(string v_Guid)
        {

            string _merge = "Exec INV_DRM_IMPORT_PLAN_MERGE @Guid";
            await _dapperRepo.QueryAsync<InvDrmImportPlanImportDto>(_merge, new { Guid = v_Guid });
        }
        public async Task<PagedResultDto<InvDrmImportPlanImportDto>> GetMessageErrorImport(string v_Guid)
        {
            string _sql = "Exec INV_DRM_IMPORT_PLAN_GET_LIST_ERROR_IMPORT @Guid";

            IEnumerable<InvDrmImportPlanImportDto> result = await _dapperRepo.QueryAsync<InvDrmImportPlanImportDto>(_sql, new
            {
                Guid = v_Guid
            });

            var listResult = result.ToList();
            var totalCount = listResult.Count();

            return new PagedResultDto<InvDrmImportPlanImportDto>(
                totalCount,
               listResult
               );
        }
        public async Task<FileDto> GetListErrImportPlanToExcel(string v_Guid)
        {
            string _sql = "Exec INV_DRM_IMPORT_PLAN_GET_LIST_ERROR_IMPORT @Guid";
            IEnumerable<InvDrmImportPlanImportDto> result = await _dapperRepo.QueryAsync<InvDrmImportPlanImportDto>(_sql, new
            {
                Guid = v_Guid
            });

            var exportToExcel = result.ToList();
            return _calendarListExcelExporter.ExportToFileErr(exportToExcel);

        }

    }
}
