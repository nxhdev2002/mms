using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Dapper.Repositories;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Abp.UI;
using FastMember;
using GemBox.Spreadsheet;
using Microsoft.EntityFrameworkCore;
using prod.Authorization;
using prod.Dto;
using prod.Inventory.DRM.Dto;
using prod.Inventory.DRM.Exporting;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using prod.Common;
using Abp.Domain.Uow;
using Abp.EntityFrameworkCore.Uow;
using prod.EntityFrameworkCore;
using prod.Inventory.GPS.Dto;
using prod.Inventory.GPS;
using prod.Inventory.CPS.SapAssetMaster.Dto;
using NPOI.SS.Formula.Functions;
using prod.Master.Common.Dto;
using prod.Inventory.CKD.Dto;
using prod.HistoricalData;
using Abp.Configuration;

namespace prod.Inventory.DRM
{
    [AbpAuthorize(AppPermissions.Pages_DMIHP_Mst_DRMPartList_View)]
    public class InvDrmPartListAppService : prodAppServiceBase, IInvDrmPartListAppService
    {
        private readonly IDapperRepository<InvDrmPartList, long> _dapperRepo;
        private readonly IRepository<InvDrmPartList, long> _repo;
        private readonly IInvDrmPartListExcelExporter _drmPartListExcelExporter;
        private readonly IHistoricalDataAppService _historicalDataAppService;

        public InvDrmPartListAppService(IRepository<InvDrmPartList, long> repo,
                                         IDapperRepository<InvDrmPartList, long> dapperRepo,
                                        IInvDrmPartListExcelExporter drmPartListExcelExporter,
                                        IHistoricalDataAppService historicalDataAppService
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _drmPartListExcelExporter = drmPartListExcelExporter;
            _historicalDataAppService = historicalDataAppService;
        }
        public async Task<List<string>> GetDrmPartListHistory(GetInvDrmPartListHistoryInput input)
        {
            return await _historicalDataAppService.GetHistoricalDataById(input.Id, input.TableName, "Id");
        }

        public async Task<FileDto> GetHistoricalDataToExcel(GetInvDrmPartListHistoryExcelInput input)
        {
            var data = await _historicalDataAppService.GetHistoricalDataById(input.Id, input.TableName, "Id");
            return _drmPartListExcelExporter.ExportToHistoricalFile(data);
        }

        public async Task<List<long?>> GetChangedRecords()
        {
            return await _historicalDataAppService.GetChangedRecordIds("InvDrmPartList");
        }



        [AbpAuthorize(AppPermissions.Pages_DMIHP_Mst_DRMPartList_Edit)]
        public async Task CreateOrEdit(CreateOrEditInvDrmPartListDto input)
        {
           if (input.Id == null) await Create(input);
           else await Update(input);
        }

        //CREATE
        private async Task Create(CreateOrEditInvDrmPartListDto input)
        {
            string _sql = "Exec INV_DRM_PART_LIST_INSERT @SupplierType, @SupplierCd, @Cfc, @MaterialCode, @MaterialSpec, " +
     "@FinMaterialNumber,@FinMaterialCode,@FinSpec,@FinPartSize,@FinPartPrice,@PartSpec,@SizeCode,@PartSize,@BoxQty,@PartCode,"+
     "@FirstDayProduct,@LastDayProduct,@Sourcing,@Cutting,@Packing,@SheetWeight,@YiledRation,@Model,@GradeName,@ModelCode,@MaterialId,"+
     "@AssetId,@MainAssetNumber,@AssetSubNumber,@WBS,@CostCenter,@ResponsibleCostCenter,@CostOfAsset,@CreateMaterial,@p_UserId";
            await _dapperRepo.ExecuteAsync(_sql, new
            {
                SupplierType = input.SupplierType,
                SupplierCd = input.SupplierCd,
                Cfc = input.Cfc,
                MaterialCode = input.MaterialCode,
                MaterialSpec = input.MaterialSpec,
                FinMaterialNumber = input.FinMaterialNumber,
                FinMaterialCode = input.FinMaterialCode,
                FinSpec = input.FinSpec,
                FinPartSize = input.FinPartSize,
                FinPartPrice = input.FinPartPrice,
                PartSpec = input.PartSpec,
                SizeCode = input.SizeCode,
                PartSize = input.PartSize,
                BoxQty = input.BoxQty,
                PartCode = input.PartCode,
                FirstDayProduct = input.FirstDayProduct,
                LastDayProduct = input.LastDayProduct,
                Sourcing = input.Sourcing,
                Cutting = input.Cutting,
                Packing = input.Packing,
                SheetWeight = input.SheetWeight,
                YiledRation = input.YiledRation,
                Model = input.Model,
                GradeName = input.GradeName,
                ModelCode = input.ModelCode,
                MaterialId = input.MaterialId,
                AssetId = input.AssetId,
                MainAssetNumber = input.MainAssetNumber,
                AssetSubNumber = input.AssetSubNumber,
                WBS = input.WBS,
                CostCenter = input.CostCenter,
                ResponsibleCostCenter = input.ResponsibleCostCenter,
                CostOfAsset = input.CostOfAsset,
                CreateMaterial = input.CreateMaterial,
                p_UserId = AbpSession.UserId
            });
          
        }

        // EDIT
        private async Task Update(CreateOrEditInvDrmPartListDto input)
        {
            string _sql = "Exec INV_DRM_PART_LIST_EDIT @p_Id, @SupplierType, @SupplierCd, @Cfc, @MaterialCode, @MaterialSpec, " +
     "@FinMaterialNumber,@FinMaterialCode,@FinSpec,@FinPartSize,@FinPartPrice,@PartSpec,@SizeCode,@PartSize,@BoxQty,@PartCode," +
     "@FirstDayProduct,@LastDayProduct,@Sourcing,@Cutting,@Packing,@SheetWeight,@YiledRation,@Model,@GradeName,@ModelCode,@MaterialId," +
     "@AssetId,@MainAssetNumber,@AssetSubNumber,@WBS,@CostCenter,@ResponsibleCostCenter,@CostOfAsset,@CreateMaterial,@p_UserId";
            await _dapperRepo.QueryAsync<GetDrmPartListId>(_sql, new
            {
                p_Id = input.Id,
                SupplierType = input.SupplierType,
                SupplierCd = input.SupplierCd,
                Cfc = input.Cfc,
                MaterialCode = input.MaterialCode,
                MaterialSpec = input.MaterialSpec,
                FinMaterialNumber = input.FinMaterialNumber,
                FinMaterialCode = input.FinMaterialCode,
                FinSpec = input.FinSpec,
                FinPartSize = input.FinPartSize,
                FinPartPrice = input.FinPartPrice,
                PartSpec = input.PartSpec,
                SizeCode = input.SizeCode,
                PartSize = input.PartSize,
                BoxQty = input.BoxQty,
                PartCode = input.PartCode,
                FirstDayProduct = input.FirstDayProduct,
                LastDayProduct = input.LastDayProduct,
                Sourcing = input.Sourcing,
                Cutting = input.Cutting,
                Packing = input.Packing,
                SheetWeight = input.SheetWeight,
                YiledRation = input.YiledRation,
                Model = input.Model,
                GradeName = input.GradeName,
                ModelCode = input.ModelCode,
                MaterialId = input.MaterialId,
                AssetId = input.AssetId,
                MainAssetNumber = input.MainAssetNumber,
                AssetSubNumber = input.AssetSubNumber,
                WBS = input.WBS,
                CostCenter = input.CostCenter,
                ResponsibleCostCenter = input.ResponsibleCostCenter,
                CostOfAsset = input.CostOfAsset,
                CreateMaterial = input.CreateMaterial,
                p_UserId = AbpSession.UserId
            });
        }
        public async Task<PagedResultDto<InvDrmPartListDto>> GetAll(GetInvDrmPartListInput input)
        {
            string _sql = "Exec INV_DRM_PART_LIST_SEARCH @SupplierType,@SupplierCd, @Cfc, @MaterialCode, @MaterialSpec";
            IEnumerable<InvDrmPartListDto> result = await _dapperRepo.QueryAsync<InvDrmPartListDto>(_sql, new
            {
                SupplierType = input.SupplierType,
                SupplierCd = input.SupplierCd,
                Cfc = input.Cfc,
                MaterialCode = input.MaterialCode,
                MaterialSpec = input.MaterialSpec,
            });
            var listResult = result.ToList();
            //var listResult = result.ToList();

            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var totalCount = result.ToList().Count();
            return new PagedResultDto<InvDrmPartListDto>(
                totalCount,
                pagedAndFiltered);
        }

        public async Task<FileDto> GetDrmPartListToExcel(GetInvDrmPartListExportInput input)
        {
            string _sql = "Exec INV_DRM_PART_LIST_SEARCH @SupplierType,@SupplierCd, @Cfc, @MaterialCode, @MaterialSpec";
            IEnumerable<InvDrmPartListDto> result = await _dapperRepo.QueryAsync<InvDrmPartListDto>(_sql, new
            {
                SupplierType = input.SupplierType,
                SupplierCd = input.SupplierCd,
                Cfc = input.Cfc,
                MaterialCode = input.MaterialCode,
                MaterialSpec = input.MaterialSpec,
            });
            var exportToExcel = result.ToList();
            return _drmPartListExcelExporter.ExportToFile(exportToExcel);
        }
        public async Task<PagedResultDto<InvDrmPartListDto>> GetViewAsAsset(long? Id)
        {
            string _sql = "Exec VIEW_AS_ASSET_DRM_PART_LIST @p_id_asset";


            IEnumerable<InvDrmPartListDto> result = await _dapperRepo.QueryAsync<InvDrmPartListDto>(_sql, new
            {
                p_id_asset = Id
            });

            var listResult = result.ToList();
            var totalCount = listResult.Count();

            return new PagedResultDto<InvDrmPartListDto>(
                totalCount,
                listResult);
        }


        [AbpAuthorize(AppPermissions.Pages_DMIHP_Mst_DRMPartList_Import)]
        public async Task<List<InvDrmIhpPartImportDto>> ImportInvDRMIHPPartFromExcel(byte[] fileBytes, string fileName)
        {
            try
            {
                List<InvDrmIhpPartImportDto> listImport = new List<InvDrmIhpPartImportDto>();
                using (var stream = new MemoryStream(fileBytes))
                {
                    SpreadsheetInfo.SetLicense("EF21-1FW1-HWZF-CLQH");
                    var xlWorkBook = ExcelFile.Load(stream);
                    string nameSheet = "";
                    string strGUID = Guid.NewGuid().ToString("N");

                    string _sql = @"EXEC CMM_IMPORT_USER_GUID_INSERT @Guid, @p_UserId";
                    _dapperRepo.ExecuteAsync(_sql, new
                    {
                        Guid = strGUID,
                        p_UserId = AbpSession.UserId
                    });

                    int startrowDRM = 8;
                    int startrowIHP = 2;
                    int countGrade = 0;
                    int startcol = 13;
                    //sheet
                    foreach (ExcelWorksheet worksheet in xlWorkBook.Worksheets)
                    {
                        nameSheet = worksheet.Name;

                        //Read
                        if (nameSheet == "DRM")  // check sheet DRM
                        {
                            for (int i = startrowDRM; i < worksheet.Rows.Count; i++)
                            {
                                string v_suppliertype = (worksheet.Cells[i, 1]).Value?.ToString() ?? "";
                                if (v_suppliertype != "")
                                {
                                    string v_supplierdrm = (worksheet.Cells[i, 2]).Value?.ToString() ?? "";
                                    string v_modeldrm = (worksheet.Cells[i, 3]).Value?.ToString() ?? "";
                                    string v_materialcode = (worksheet.Cells[i, 4]).Value?.ToString() ?? "";
                                    string v_materialspec = (worksheet.Cells[i, 5]).Value?.ToString() ?? "";
                                    string v_boxqty = (worksheet.Cells[i, 6]).Value?.ToString() ?? "";
                                    string v_partcode = (worksheet.Cells[i, 13]).Value?.ToString() ?? "";
                                    string v_partspec = (worksheet.Cells[i, 14]).Value?.ToString() ?? "";
                                    string v_partsize = (worksheet.Cells[i, 15]).Value?.ToString() ?? "";
                                    string v_firtday = (worksheet.Cells[i, 17]).Value?.ToString() ?? "";
                                    string v_lastday = (worksheet.Cells[i, 18]).Value?.ToString() ?? "";
                                    string v_assetid = (worksheet.Cells[i, 19]).Value?.ToString() ?? "";
                                    string v_mainassetnumber = (worksheet.Cells[i, 20]).Value?.ToString() ?? "";
                                    string v_wbs = (worksheet.Cells[i, 21]).Value?.ToString() ?? "";
                                    string v_costcenter = (worksheet.Cells[i, 22]).Value?.ToString() ?? "";
                                    string v_responsiblecostcenter = (worksheet.Cells[i, 22]).Value?.ToString() ?? "";
                                    string v_costofasset = (worksheet.Cells[i, 23]).Value?.ToString() ?? "";

                                    var row = new InvDrmIhpPartImportDto();
                                    row.Guid = strGUID;
                                    row.ErrorDescription = "";
                                    row.DrmOrIhp = "DRM";
                                    row.SupplierType = v_suppliertype;
                                    row.SupplierDrm = v_supplierdrm;
                                    if (v_modeldrm.Length > 4) { row.ErrorDescription = "Cfc " + v_modeldrm + " dài quá 4 kí tự! "; }
                                    else { row.Cfc = v_modeldrm; }
                                    row.MaterialCode = v_materialcode;
                                    row.MaterialSpec = v_materialspec;
                                    try
                                    {
                                        row.BoxQty = Convert.ToInt32(v_boxqty);
                                        if (row.BoxQty < 0)
                                        {
                                            row.ErrorDescription += "Box Qty " + v_boxqty + " phải là số dương! ";
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        row.ErrorDescription += "Box Qty " + v_boxqty + " không phải là số! ";
                                    }
                                    row.PartCode = v_partcode;
                                    row.Spec = v_partspec;
                                    row.Size = v_partsize;
                                    try
                                    {
                                        row.FirstDayProduct = DateTime.Parse(v_firtday);
                                    }
                                    catch (Exception e)
                                    {
                                        row.ErrorDescription += "FirstDayProduct " + v_firtday + " không đúng định dạng! ";
                                    }
                                    try
                                    {
                                        if (string.IsNullOrEmpty(v_lastday))
                                        {
                                            row.LastDayProduct = null;
                                        }
                                        else
                                        {
                                            row.LastDayProduct = DateTime.Parse(v_lastday);
                                            if (row.FirstDayProduct.HasValue &&
                                                row.FirstDayProduct > row.LastDayProduct)
                                            {
                                                row.ErrorDescription += "LastDayProduct " + v_lastday + "phải sau FirstDayProduct! ";
                                            }
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        row.ErrorDescription += "LastDayProduct " + v_lastday + " không đúng định dạng! ";
                                    }

                                    listImport.Add(row);
                                }
                            }
                        }


                        if (nameSheet == "IHP")  // check sheet IHP
                        {

                            //count grade
                            for (int i = startcol; i <= 500; i++)
                            {
                                if (!string.IsNullOrEmpty(Convert.ToString(worksheet.Cells[1, i].Value)))
                                {
                                    countGrade++;
                                }
                                else { break; }

                            }

                            for (int i = startrowIHP; i < worksheet.Rows.Count; i++)
                            {
                                string v_partcode = (worksheet.Cells[i, 1]).Value?.ToString() ?? "";

                                if (v_partcode != "")
                                {
                                    string v_modelihp = (worksheet.Cells[i, 0]).Value?.ToString() ?? "";
                                    string v_partno = (worksheet.Cells[i, 2]).Value?.ToString() ?? "";
                                    string v_partname = (worksheet.Cells[i, 3]).Value?.ToString() ?? "";
                                    string v_materialcode = (worksheet.Cells[i, 4]).Value?.ToString() ?? "";
                                    string v_partsize = (worksheet.Cells[i, 5]).Value?.ToString() ?? "";
                                    string v_partspec = (worksheet.Cells[i, 6]).Value?.ToString() ?? "";
                                    string v_sourcing = (worksheet.Cells[i, 7]).Value?.ToString() ?? "";
                                    string v_cutting = (worksheet.Cells[i, 8]).Value?.ToString().Trim() ?? "";
                                    string v_packing = (worksheet.Cells[i, 9]).Value?.ToString() ?? "";
                                    string v_sheetweight = (worksheet.Cells[i, 10]).Value?.ToString() ?? "";
                                    string v_panelweight = (worksheet.Cells[i, 11]).Value?.ToString() ?? "";
                                    string v_yiledration = (worksheet.Cells[i, 12]).Value?.ToString() ?? "";

                                    for (int j = startcol; j < countGrade + startcol; j++)
                                    {
                                        if (!string.IsNullOrEmpty(Convert.ToString(worksheet.Cells[i, j].Value))
                                            && Convert.ToString(worksheet.Cells[i, j].Value) != "0")
                                        {
                                            string v_cfc = v_modelihp != "" ? (v_modelihp.Substring(v_modelihp.IndexOf("(") + 1, 4)) : "";
                                            var row = new InvDrmIhpPartImportDto();

                                            row.Guid = strGUID;
                                            row.ErrorDescription = "";
                                            row.DrmOrIhp = "IHP";
                                            row.CfcIhp = v_cfc;
                                            row.PartNo = v_partno;
                                            row.PartName = v_partname;
                                            row.MaterialCode = v_materialcode;
                                            row.Size = v_partsize;
                                            row.Spec = v_partspec;
                                            row.Sourcing = v_sourcing;
                                            row.Cutting = v_cutting;
                                            try
                                            {
                                                row.Packing = Convert.ToInt32(v_packing);
                                                if (row.Packing < 0)
                                                {
                                                    row.ErrorDescription += "Packing " + v_packing + " phải là số dương! ";
                                                }
                                            }
                                            catch (Exception e)
                                            {
                                                row.ErrorDescription += "Packing " + v_packing + " không phải là số! ";
                                            }

                                            try
                                            {
                                                row.SheetWeight = Convert.ToDecimal(v_sheetweight != "" ? v_sheetweight : 0);
                                            }
                                            catch (Exception e)
                                            {
                                                row.SheetWeight = 0;
                                            }

                                            try
                                            {
                                                row.PanelWeight = Convert.ToDecimal(v_panelweight != "" ? v_panelweight : 0);
                                            }
                                            catch (Exception e)
                                            {
                                                row.PanelWeight = 0;
                                            }

                                            try
                                            {
                                                row.YiledRation = Convert.ToDecimal(v_yiledration != "" ? v_yiledration : 0);
                                            }
                                            catch (Exception e)
                                            {
                                                row.YiledRation = 0;
                                            }
                                            string v_grade = Convert.ToString(worksheet.Cells[1, j].Value);
                                            if (v_grade.Length > 2) { row.ErrorDescription = "Grade " + v_grade + " dài quá 2 kí tự! "; }
                                            else { row.Grade = v_grade; }
                                            try
                                            {
                                                row.UsageQty = Convert.ToInt32(worksheet.Cells[i, j].Value);
                                                if (row.UsageQty < 0)
                                                {
                                                    row.ErrorDescription += "Usage Qty phải là số dương! ";
                                                }
                                            }
                                            catch (Exception e)
                                            {
                                                row.ErrorDescription += "Usage Qty phải là số! ";
                                            }

                                            listImport.Add(row);

                                        }
                                    }
                                }
                            }

                        }
                        // import temp into db (bulkCopy)
                        if (listImport.Count > 0)
                        {
                            IEnumerable<InvDrmIhpPartImportDto> dataE = listImport.AsEnumerable();
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
                                        bulkCopy.DestinationTableName = "InvDrmIhpPartList_T";
                                        bulkCopy.ColumnMappings.Add("Guid", "Guid");
                                        bulkCopy.ColumnMappings.Add("SupplierType", "SupplierType");
                                        bulkCopy.ColumnMappings.Add("SupplierDrm", "SupplierDrm");
                                        bulkCopy.ColumnMappings.Add("Cfc", "Cfc");
                                        bulkCopy.ColumnMappings.Add("MaterialCode", "MaterialCode");
                                        bulkCopy.ColumnMappings.Add("MaterialSpec", "MaterialSpec");
                                        bulkCopy.ColumnMappings.Add("BoxQty", "BoxQty");
                                        bulkCopy.ColumnMappings.Add("SupplierIhp", "SupplierIhp");
                                        bulkCopy.ColumnMappings.Add("CfcIhp", "CfcIhp");
                                        bulkCopy.ColumnMappings.Add("PartNo", "PartNo");
                                        bulkCopy.ColumnMappings.Add("PartName", "PartName");
                                        bulkCopy.ColumnMappings.Add("PartCode", "PartCode");
                                        bulkCopy.ColumnMappings.Add("Spec", "Spec");
                                        bulkCopy.ColumnMappings.Add("Size", "Size");
                                        bulkCopy.ColumnMappings.Add("FirstDayProduct", "FirstDayProduct");
                                        bulkCopy.ColumnMappings.Add("LastDayProduct", "LastDayProduct");
                                        bulkCopy.ColumnMappings.Add("Grade", "Grade");
                                        bulkCopy.ColumnMappings.Add("UsageQty", "UsageQty");
                                        bulkCopy.ColumnMappings.Add("Sourcing", "Sourcing");
                                        bulkCopy.ColumnMappings.Add("Cutting", "Cutting");
                                        bulkCopy.ColumnMappings.Add("Packing", "Packing");
                                        bulkCopy.ColumnMappings.Add("SheetWeight", "SheetWeight");
                                        bulkCopy.ColumnMappings.Add("PanelWeight", "PanelWeight");
                                        bulkCopy.ColumnMappings.Add("YiledRation", "YiledRation");
                                        bulkCopy.ColumnMappings.Add("ErrorDescription", "ErrorDescription");
                                        bulkCopy.ColumnMappings.Add("DrmOrIhp", "DrmOrIhp");

                                        bulkCopy.WriteToServer(table);
                                        tran.Commit();
                                    }
                                }
                                await conn.CloseAsync();
                            }
                        }

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
        public async Task MergeDataInvDrmIhpPart(string v_Guid)
        {
            string _sql = "Exec INV_DRM_IHP_PART_LIST_MERGE @Guid";
            await _dapperRepo.QueryAsync<InvDrmIhpPartImportDto>(_sql, new { Guid = v_Guid });
        }



        // hiển thị lỗi import và export lỗi
        public async Task<PagedResultDto<InvDrmIhpPartImportDto>> GetMessageErrorImport(string v_Guid)
        {
            string _sql = "Exec INV_DIRECT_MATERIAL_GET_LIST_ERROR_IMPORT @Guid, @DrmOrIhp";

            IEnumerable<InvDrmIhpPartImportDto> result = await _dapperRepo.QueryAsync<InvDrmIhpPartImportDto>(_sql, new
            {
                Guid = v_Guid,
                @DrmOrIhp = ""
            });

            var listResult = result.ToList();
            var totalCount = listResult.Count();

            return new PagedResultDto<InvDrmIhpPartImportDto>(
                totalCount,
               listResult
               );
        }

        public async Task<FileDto> GetListErrToExcel(string v_Guid)
        {
            var drm = await _dapperRepo.QueryAsync<InvDrmIhpPartImportDto>("Exec INV_DIRECT_MATERIAL_GET_LIST_ERROR_IMPORT @Guid, @DrmOrIhp", new { @Guid = v_Guid, @DrmOrIhp = "DRM" });
            var ihp = await _dapperRepo.QueryAsync<InvDrmIhpPartImportDto>("Exec INV_DIRECT_MATERIAL_GET_LIST_ERROR_IMPORT @Guid, @DrmOrIhp", new { @Guid = v_Guid, @DrmOrIhp = "IHP" });

            var drmExportToExcel = drm.ToList();
            var ihpExportToExcel = ihp.ToList();

            return _drmPartListExcelExporter.ExportToFileErr(drmExportToExcel, ihpExportToExcel);
        }

        public async Task<List<InvCpsSapAssetMasterDto>> GetListSapAsset(long? p_id )
        {
            string _sql = "Exec INV_DIRECT_MATERIAL_GET_LIST_SAP_ASSET @p_id";

            IEnumerable<InvCpsSapAssetMasterDto> result = await _dapperRepo.QueryAsync<InvCpsSapAssetMasterDto>(_sql, new
            {
                p_id = p_id
            });
            return result.ToList();
        }

        public async Task AddAssetDRMPartList(InvCpsSapAssetInput input)
        {
            string _sql = "Exec INV_DIRECT_MATERIAL_ADD_ASSET @p_CurrentLineItemId, @p_AssetId, @p_MainAssetNumber, @p_AssetSubNumber, @p_WBS, @p_CostCenter, @p_ResponsibleCostCenter, @p_CostOfAsset, @p_DRMPartListId, @p_UserId";

            await _dapperRepo.ExecuteAsync(_sql, new 
            {
                p_CurrentLineItemId = input.CurrentLineItemId,
                p_AssetId = input.Id,
                p_MainAssetNumber = input.LineItem,
                p_AssetSubNumber = input.SubAssetNumber,
                p_WBS = input.WBS,
                p_CostCenter = input.CostCenter,
                p_ResponsibleCostCenter = input.ResponsibleCostCenter,
                p_CostOfAsset = input.CostOfAsset,
                p_DRMPartListId = input.DrmPartListId,
                p_UserId = AbpSession.UserId
            });
        }
    }
}
