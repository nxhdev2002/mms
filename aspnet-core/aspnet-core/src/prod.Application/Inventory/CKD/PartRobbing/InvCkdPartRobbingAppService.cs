using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Dapper.Repositories;
using Abp.UI;
using FastMember;
using GemBox.Spreadsheet;
using prod.Authorization;
using prod.Dto;
using prod.Inventory.CKD.PartRobbing.Dto;
using prod.Inventory.CKD.PartRobbing.Exporting;
using prod.Storage;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using prod.Common;

namespace prod.Inventory.CKD.PartRobbing
{
    [AbpAuthorize(AppPermissions.Pages_Ckd_SMQD_PartRobbing_View)]
    public class InvCkdPartRobbingAppService : prodAppServiceBase, IInvCkdPartRobbingAppService
    {
        private readonly IDapperRepository<InvCkdPartRobbing, long> _dapperRepo;
        private readonly IDapperRepository<InvCkdPartRobbingDetails, long> _dapperRepoDetails;
        private readonly ITempFileCacheManager _tempFileCacheManager;
        private readonly IInvCkdPartRobbingExcelExporter _partRobbingExcelExporter;

        public InvCkdPartRobbingAppService(
                                         IDapperRepository<InvCkdPartRobbingDetails, long> dapperRepoDetails,
                                         IDapperRepository<InvCkdPartRobbing, long> dapperRepo,
                                         ITempFileCacheManager tempFileCacheManager,
                                         IInvCkdPartRobbingExcelExporter partRobbingExcelExporter

            )
        {
            _dapperRepoDetails = dapperRepoDetails;
            _dapperRepo = dapperRepo;
            _tempFileCacheManager = tempFileCacheManager;
            _partRobbingExcelExporter = partRobbingExcelExporter;

        }
        public async Task<PagedResultDto<InvCkdPartRobbingDto>> GetPartRobbingSearch(GetPartRobbingInput input)
        {
            string _sql = "Exec INV_CKD_PART_ROBBING_SEARCH @p_part_no, @p_cfc";

            IEnumerable<InvCkdPartRobbingDto> result = await _dapperRepo.QueryAsync<InvCkdPartRobbingDto>(_sql, new
            {
                p_part_no = input.PartNo,
                p_cfc = input.Cfc,
            });

            var listResult = result.ToList();

            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var totalCount = result.ToList().Count();

            return new PagedResultDto<InvCkdPartRobbingDto>(
                totalCount,
                pagedAndFiltered);
        }

        public async Task<FileDto> getPartRobbingToExcel(GetPartRobbingInput input)
        {
            string _sql = "Exec INV_CKD_PART_ROBBING_SEARCH @p_part_no, @p_cfc";

            IEnumerable<InvCkdPartRobbingDto> result = await _dapperRepo.QueryAsync<InvCkdPartRobbingDto>(_sql, new
            {
                p_part_no = input.PartNo,
                p_cfc = input.Cfc,
            });

            var exportToExcel = result.ToList();
            return _partRobbingExcelExporter.ExportToFile(exportToExcel);
        }
        //
        public async Task<PagedResultDto<InvCkdPartRobbingImportDto>> GetPartRobbingError(string v_Guid)
        {
            var data = await _dapperRepo.QueryAsync<InvCkdPartRobbingImportDto>("Exec INV_CKD_PART_ROBBING_GET_LIST_ERROR_IMPORT @Guid", new { @Guid = v_Guid });

            var totalCount = data.Count();
            return new PagedResultDto<InvCkdPartRobbingImportDto>(
                totalCount,
                 data.ToList()
            );
        }
        public async Task<FileDto> GetErrorPartRobbingToExcel(string v_Guid)
        {
            var data = await _dapperRepo.QueryAsync<InvCkdPartRobbingImportDto>("Exec INV_CKD_PART_ROBBING_GET_LIST_ERROR_IMPORT @Guid", new { @Guid = v_Guid });

            var exportToExcel = data.ToList();
            return _partRobbingExcelExporter.ExportToFileErr(exportToExcel);
        }


        [AbpAuthorize(AppPermissions.Pages_Ckd_SMQD_PartRobbing_Import)]
        public async Task<List<InvCkdPartRobbingImportDto>> ImportPartRobbingFromExcel(byte[] fileBytes, string fileName)
        {
            try
            {
                List<InvCkdPartRobbingImportDto> listImport = new List<InvCkdPartRobbingImportDto>();
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

                    int startrow = 10;

                    for (int i = startrow; i < v_worksheet.Rows.Count; i++)
                    {
                        string v_partNo = (v_worksheet.Cells[i, 5]).Value?.ToString() ?? "";

                        if (v_partNo != "")
                        {
                            string v_model = (v_worksheet.Cells[i, 3]).Value?.ToString() ?? "";
                            string v_source = (v_worksheet.Cells[i, 4]).Value?.ToString() ?? "";
                            string v_partname = (v_worksheet.Cells[i, 6]).Value?.ToString() ?? "";
                            string v_robbing = (v_worksheet.Cells[i, 7]).Value?.ToString() ?? "";
                            string v_unitqty = (v_worksheet.Cells[i, 8]).Value?.ToString() ?? "";
                            string v_effectvhe = (v_worksheet.Cells[i, 9]).Value?.ToString() ?? "";
                            string v_case = (v_worksheet.Cells[i, 10]).Value?.ToString() ?? "";
                            string v_box = (v_worksheet.Cells[i, 11]).Value?.ToString() ?? "";
                            string v_detailmodel = (v_worksheet.Cells[i, 12]).Value?.ToString() ?? "";
                            string v_shop = (v_worksheet.Cells[i, 13]).Value?.ToString() ?? "";
                            //List<string> result = v_detailmodel.Split(',').ToList();

                            var row = new InvCkdPartRobbingImportDto();
                            row.Guid = strGUID;
                            row.PartNo = v_partNo != null ? v_partNo.Replace("-","") : "";
                            row.PartNoNormalized = v_partNo.Replace("-", "");
                            row.PartName = v_partname;
                            row.Cfc = v_model;
                            row.SupplierNo = v_source;
                            row.RobbingQty = v_robbing != null ? Convert.ToInt32(v_robbing) : 0;
                            row.UnitQty = v_unitqty != null ? Convert.ToInt32(v_unitqty) : 0;
                            row.EffectVehQty = v_effectvhe != null ? Convert.ToInt32(v_effectvhe) : 0;
                            row.Case = v_case;
                            row.Box = v_box;
                            row.DetailModel = v_detailmodel;
                            row.Shop = v_shop;
                            row.ErrorDescription = "";

                            listImport.Add(row);


                        }

                    }
                    // import temp into db (bulkCopy)
                    if (listImport.Count > 0)
                    {
                        IEnumerable<InvCkdPartRobbingImportDto> dataE = listImport.AsEnumerable();
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
                                    bulkCopy.DestinationTableName = "InvCkdPartRobbing_T";
                                    bulkCopy.ColumnMappings.Add("Guid", "Guid");
                                    bulkCopy.ColumnMappings.Add("Cfc", "Cfc");
                                    bulkCopy.ColumnMappings.Add("SupplierNo", "SupplierNo");
                                    bulkCopy.ColumnMappings.Add("PartNo", "PartNo");
                                    bulkCopy.ColumnMappings.Add("PartNoNormalized", "PartNoNormalized");
                                    bulkCopy.ColumnMappings.Add("PartName", "PartName");
                                    bulkCopy.ColumnMappings.Add("RobbingQty", "RobbingQty");
                                    bulkCopy.ColumnMappings.Add("UnitQty", "UnitQty");
                                    bulkCopy.ColumnMappings.Add("EffectVehQty", "EffectVehQty");
                                    bulkCopy.ColumnMappings.Add("Case", "Case");
                                    bulkCopy.ColumnMappings.Add("Box", "Box");
                                    bulkCopy.ColumnMappings.Add("DetailModel", "DetailModel");
                                    bulkCopy.ColumnMappings.Add("Shop", "Shop");
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
        public async Task MergeDataInvCkdPartRobbing(string v_Guid)
        {
            string _sql = "Exec INV_CKD_PART_ROBBING_MERGE @Guid";
            await _dapperRepo.QueryAsync<InvCkdPartRobbingImportDto>(_sql, new { Guid = v_Guid });
        }

    }
}
