

using Abp.Application.Services.Dto;
using Abp.AspNetZeroCore.Net;
using Abp.Authorization;
using Abp.Dapper.Repositories;
using Abp.Domain.Repositories;
using Abp.UI;
using FastMember;
using GemBox.Spreadsheet;
using NPOI.SS.UserModel;
using prod.Authorization;
using prod.Common;
using prod.Dto;
using prod.HistoricalData;
using prod.Inventory.CKD.Dto;
using prod.Inventory.CKD.Exporting;
using prod.Inventory.IHP.Dto;
using prod.Inventory.PIO.PartListInl;
using prod.Inventory.PIO.PartListInl.Dto;
using prod.Inventory.PIO.PartListInl.Export;
using prod.Storage;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace prod.Inventory.PIO
{
    [AbpAuthorize(AppPermissions.Pages_PIO_Master_PartListInl_View)]
    public class InvPioPartListInlAppService : prodAppServiceBase, IInvPioPartListInlAppService
    {
        private readonly IDapperRepository<InvPioPartListInl, long> _dapperRepo;
        private readonly IDapperRepository<InvPioPartGradeInl, long> _partGradeInl;
        private readonly IRepository<InvPioPartListInl, long> _repo;
        private readonly IInvPioPartListInlExcelExporter _calendarListExcelExporter;
        private readonly ITempFileCacheManager _tempFileCacheManager;
        private readonly IHistoricalDataAppService _historicalDataAppService;


        public InvPioPartListInlAppService(IRepository<InvPioPartListInl, long> repo,
                                         IDapperRepository<InvPioPartListInl, long> dapperRepo,
                                        IDapperRepository<InvPioPartGradeInl, long> partGradeInl,
                                        IInvPioPartListInlExcelExporter calendarListExcelExporter,
                                        ITempFileCacheManager tempFileCacheManager,
                                        IHistoricalDataAppService historicalDataAppService)
                                      
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _partGradeInl = partGradeInl;
            _calendarListExcelExporter = calendarListExcelExporter;
            _tempFileCacheManager = tempFileCacheManager;
            _historicalDataAppService = historicalDataAppService;

        }

        public async Task<PagedResultDto<InvPioPartListInlDto>> GetAll(GetInvPioPartListInlInput input)
        {
            string _sql = "Exec INV_PIO_PART_LIST_INL_SEARCH @p_part_no,@p_cfc, @p_model, @p_grade, @p_shop, @p_supplier_no,@p_order_pattern,@p_active";

            IEnumerable<InvPioPartListInlDto> result = await _dapperRepo.QueryAsync<InvPioPartListInlDto>(_sql, new
            {
                p_part_no = input.PartNo,
                p_cfc = input.Cfc,
                p_model = input.Model,
                p_grade = input.Grade,
                p_shop = input.Shop,
                p_supplier_no = input.SupplierNo,
                p_order_pattern = input.OrderPattern,
                p_active = input.IsActive,

            });

            var listResult = result.ToList();

            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var totalCount = result.ToList().Count();

            return new PagedResultDto<InvPioPartListInlDto>(
                totalCount,
                pagedAndFiltered);
        }

        public async Task<PagedResultDto<InvPioPartGradeInlDto>> GetPartGradeInl(GetInvPioPartListGradeInlInput input)
        {
            string _sqlSearch = "Exec [INV_PIO_PART_GRADE_INL] @p_part_id";

            IEnumerable<InvPioPartGradeInlDto> result = await _partGradeInl.QueryAsync<InvPioPartGradeInlDto>(_sqlSearch,
                  new
                  {
                      p_part_id = input.PartId

                  });
            var listResult = result.ToList();

            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var totalCount = result.ToList().Count();

            return new PagedResultDto<InvPioPartGradeInlDto>(
                totalCount,
                pagedAndFiltered);
        }


       [AbpAuthorize(AppPermissions.Pages_PIO_Master_PartListInl_Validate)]
        public async Task<PagedResultDto<ValidatePartListDto>> GetValidateInvPioPartList( PagedAndSortedResultRequestDto input)
        {
            string _sqlSearch = "Exec [INV_PIO_PART_LIST_INL_VALIDATE]";

            IEnumerable<ValidatePartListDto> result = await _partGradeInl.QueryAsync<ValidatePartListDto>(_sqlSearch, new { });
            var listResult = result.ToList();

            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var totalCount = result.ToList().Count();

            return new PagedResultDto<ValidatePartListDto>(
                totalCount,
                pagedAndFiltered);
        }

        public async Task<FileDto> GetValidateInvPioPartListExcel()
        {

            string _sql = "Exec [INV_PIO_PART_LIST_INL_VALIDATE]";

            IEnumerable<ValidatePartListDto> result = await _partGradeInl.QueryAsync<ValidatePartListDto>(_sql, new
            { });

            var exportToExcel = result.ToList();
            return _calendarListExcelExporter.ExportValidateToFile(exportToExcel);
        }
		public async Task<List<GetCfcPartListInlDto>> GetListCfcs()
		{
			IEnumerable<GetCfcPartListInlDto> result = await _dapperRepo.QueryAsync<GetCfcPartListInlDto>(@"exec [dbo].[MST_CMM_LOT_CODE_GRADE_CFC_GETS] ");
			return result.ToList();
		}

        public async Task<List<GetGradeDto>> GetListGrades()
        {
            IEnumerable<GetGradeDto> result = await _dapperRepo.QueryAsync<GetGradeDto>(@"exec [dbo].[MST_CMM_LOT_CODE_GRADE_GRADE_GETS] ");
            return result.ToList();
        }
        public async Task<List<GetSupplierPartListInlDto>> GetListSupplierNos()
		{
			IEnumerable<GetSupplierPartListInlDto> result = await _dapperRepo.QueryAsync<GetSupplierPartListInlDto>(@"exec [dbo].[MST_INV_PIO_SUPPLIER_LIST_GETS]");
			return result.ToList();
		}
		public async Task<List<GetPartNoPartListInlDto>> GetListPartNo()
		{
			IEnumerable<GetPartNoPartListInlDto> result = await _dapperRepo.QueryAsync<GetPartNoPartListInlDto>("SELECT DISTINCT PartNo FROM InvPioPartListInl");
			return result.ToList();
		}
		public async Task<PagedResultDto<CheckExistPartListInlDto>> CheckExistPartNo(string PartNo, string Cfc)
		{
			string _sqlSearch = "Exec INV_PIO_PARTLIST_INL_EXIST_PARTNO @partno, @cfc";

			IEnumerable<CheckExistPartListInlDto> result = await _partGradeInl.QueryAsync<CheckExistPartListInlDto>(_sqlSearch, new { partno = PartNo, cfc = Cfc });
			var listResult = result.ToList();

			var totalCount = result.ToList().Count();

			return new PagedResultDto<CheckExistPartListInlDto>(
				totalCount,
				result.ToList());
		}
		[AbpAuthorize(AppPermissions.Pages_PIO_Master_PartListInl_Add)]
		public async Task PartListInlInsert(GetPartListGradePartListInlDto input)
		{
			var result = (await _dapperRepo.QueryAsync<GetPartListInlId>(@"
                   exec [dbo].[INV_PIO_PART_LIST_INL_INSERT]
                                            @partNo,
                                            @partName,
                                            @cfc,
                                            @supplierNo ,
                                            @orderPattern,
                                            @p_UserId
                ", new
			{
				partNo = input.PartNo,
				partName = input.PartName,
				cfc = input.Cfc,
				supplierNo = input.SupplierNo,
				orderPattern = input.OrderPattern,
				p_UserId = AbpSession.UserId
			})).FirstOrDefault();

			if (result != null)
			{
				for (int i = 0; i < input.listGrade.Count; i++)
				{
					await _dapperRepo.ExecuteAsync(@"
                      exec [dbo].[INV_PIO_PART_GRADE_INL_INSERT]                                      
                                            @partId,
                                            @partNo,
                                            @cfc,
                                            @model,
                                            @grade,
                                            @usageQty,
                                            @shop,
                                            @bodyColor,
                                            @startLot,
                                            @endLot,
                                            @startRun,
                                            @endRun,
                                            @efFromPart,
                                            @efToPart,
                                            @orderPattern,
                                            @remark,
                                            @p_UserId
                                       
                    ", new
					{
						partId = result.PartListId,
						partNo = input.listGrade[i].PartNo,
						cfc = input.Cfc,
						model = input.listGrade[i].Model,
						grade = input.listGrade[i].Grade,
						usageQty = input.listGrade[i].UsageQty,
						shop = input.listGrade[i].Shop,
						bodyColor = input.listGrade[i].BodyColor,
						startLot = input.listGrade[i].StartLot,
						endLot = input.listGrade[i].EndLot,
						startRun = input.listGrade[i].StartRun,
						endRun = input.listGrade[i].EndRun,
						efFromPart = input.listGrade[i].EfFromPart,
						efToPart = input.listGrade[i].EfToPart,
						orderPattern = input.listGrade[i].OrderPattern,
						remark = input.listGrade[i].Remark,
						p_UserId = AbpSession.UserId
					});
				}
			}
			else
			{
				throw new UserFriendlyException(400, "PartNo is exist in Cfc");
			}
		}

        [AbpAuthorize(AppPermissions.Pages_PIO_Master_PartListInl_Add)]
        public async Task PartListInlEdit(GetPartListGradePartListInlDto input)
		{
			string listGradeSelected = "";
			await _dapperRepo.ExecuteAsync(@"
                   exec [dbo].[INV_PIO_PART_LIST_INL_EDIT]
                                            @id,
                                            @partNo,
                                            @partName,
                                            @cfc,
                                            @supplierNo ,
                                            @orderPattern,
                                            @p_UserId 
            ", new
			{
				id = input.Id,
				partNo = input.PartNo,
				partName = input.PartName,
				cfc = input.Cfc,
				supplierNo = input.SupplierNo,
				orderPattern = input.OrderPattern,
				p_UserId = AbpSession.UserId
			});

			for (int i = 0; i < input.listGrade.Count; i++)
			{
				listGradeSelected = listGradeSelected + "," + input.listGrade[i].Grade;
			}

			for (int i = 0; i < input.listGrade.Count; i++)
			{

				await _dapperRepo.ExecuteAsync(@"
                  exec [dbo].[INV_PIO_PART_GRADE_INL_EDITS]  
                                        @id,
                                        @partId,
                                        @partNo,
                                        @cfc,
                                        @model,
                                        @grade,
                                        @usageQty,
                                        @shop,
                                        @startLot,
                                        @endLot,
                                        @startRun,
                                        @endRun,
                                        @efFromPart,
                                        @efToPart,
                                        @orderPattern,
                                        @remark,
                                        @listGrade,
                                        @p_UserId
                ", new
				{
					id = input.listGrade[i].Id,
					partId = input.listGrade[i].PartId,
					partNo = input.listGrade[i].PartNo,
					cfc = input.Cfc,
					model = input.listGrade[i].Model,
					grade = input.listGrade[i].Grade,
					usageQty = input.listGrade[i].UsageQty,
					shop = input.listGrade[i].Shop,
					startLot = input.listGrade[i].StartLot,
					endLot = input.listGrade[i].EndLot,
					startRun = input.listGrade[i].StartRun,
					endRun = input.listGrade[i].EndRun,
					efFromPart = input.listGrade[i].EfFromPart,
					efToPart = input.listGrade[i].EfToPart,
					orderPattern = input.listGrade[i].OrderPattern,
					remark = input.listGrade[i].Remark,
					listGrade = listGradeSelected,
					p_UserId = AbpSession.UserId
				}); 
			}
		}

        public async Task<List<GetColorDto>> GetListColors(string cfc, string grade)
        {
            string _sql = "Exec MST_CMM_LOT_CODE_GRADE_COLOR_BY_GRADE @cfc, @grade";
            IEnumerable<GetColorDto> result = await _dapperRepo.QueryAsync<GetColorDto>(_sql, new
            {
                cfc = cfc,
                grade = grade
            });

            return result.ToList();
        }


       [AbpAuthorize(AppPermissions.Pages_PIO_Master_PartListInl_Add)]
        public async Task PartGradeDel(long? v_id)
        {
            await _dapperRepo.ExecuteAsync(@" exec [dbo].[INV_PIO_PART_GRADE_INL_DELETE] @id ,@p_UserId", new
            {
                id = v_id,
                p_UserId = AbpSession.UserId
            });
        }


        [AbpAuthorize(AppPermissions.Pages_PIO_Master_PartListInl_Import)]
        public async Task<List<ImportPioPartListDto>> ImportDataInvPioPartListFromExcel(byte[] fileBytes, string fileName)
        {
            try
            {
                List<ImportPioPartListDto> listImport = new List<ImportPioPartListDto>();
                using (var stream = new MemoryStream(fileBytes))
                {
                    SpreadsheetInfo.SetLicense("EF21-1FW1-HWZF-CLQH");
                    var xlWorkBook = ExcelFile.Load(stream);
                    var v_worksheet = xlWorkBook.Worksheets[0];
                    DataFormatter formatter = new DataFormatter();
                    DateTime dateTime = DateTime.Now;

                    //string v_devanning_date = (v_worksheet.Cells[4, 2]).Value?.ToString() ?? "";
                    string strGUID = Guid.NewGuid().ToString("N");

                    string _sql = @"EXEC CMM_IMPORT_USER_GUID_INSERT @Guid, @p_UserId";
                    await _dapperRepo.ExecuteAsync(_sql, new
                    {
                        Guid = strGUID,
                        p_UserId = AbpSession.UserId
                    });

                    int countGrade = 0;

                    //Đếm số lượng bản ghi Grade
                    for (int i = 9; i <= 500; i++)
                    {
                        if (!string.IsNullOrEmpty(Convert.ToString(v_worksheet.Cells[7, i].Value)))
                        {
                            countGrade++;
                        }
                        else { break; }

                    }

                    bool flag_HasRecord = false;
                    for (int i = 9; i < v_worksheet.Rows.Count; i++)
                    {

                        string v_partNo = (v_worksheet.Cells[i, 3]).Value?.ToString() ?? "";

                        if (v_partNo != "")
                        {
                            var v_remark = Convert.ToString(v_worksheet.Cells[i, 8 + countGrade + 1].Value);
                            string v_model = (v_worksheet.Cells[i, 0]).Value?.ToString() ?? "";
                            string v_shop = (v_worksheet.Cells[i, 1]).Value?.ToString() ?? "";
                            string v_idLine = (v_worksheet.Cells[i, 2]).Value?.ToString() ?? "";
                            string v_partName = (v_worksheet.Cells[i, 4]).Value?.ToString() ?? "";
                            string v_exporter = (v_worksheet.Cells[i, 5]).Value?.ToString() ?? "";
                            string v_exporterCode = (v_worksheet.Cells[i, 6]).Value?.ToString() ?? "";
                            string v_bodyColor = (v_worksheet.Cells[i, 7]).Value?.ToString() ?? "";
             
                            flag_HasRecord = false;
                            for (int j = 9; j < countGrade + 9; j++)
                            {
                                if (!string.IsNullOrEmpty(Convert.ToString(v_worksheet.Cells[i, j].Value)) && Convert.ToString(v_worksheet.Cells[i, j].Value) != "0")
                                {
                                    flag_HasRecord = true;
                                    var row = new ImportPioPartListDto();
                                    row.Guid = strGUID;
                                 
                                    row.Guid = strGUID;

                                    if (v_model.Length > 4) row.ErrorDescription = "Độ dài Model : " + v_model + " không hợp lệ! , ";
                                    else row.Cfc = v_model;

                                    if (v_shop.Length > 1) row.ErrorDescription += "Độ dài Shop : " + v_shop + " không hợp lệ! , ";
                                    else row.Shop = v_shop;

                                    if (v_idLine.Length > 10) row.ErrorDescription += "Độ dài IdLine : " + v_idLine + " không hợp lệ! , ";
                                    else row.IdLine = v_idLine;

                                    if (v_partNo.Length > 50) row.ErrorDescription += "Độ dài PartNo : " + v_partNo + " không hợp lệ! , ";
                                    else row.PartNo = v_partNo;

                                    if (v_partName.Length > 500) row.ErrorDescription += "Độ dài PartName : " + v_partName + " không hợp lệ! , ";
                                    else row.PartName = v_partName;

                                    if (v_exporter.Length > 50) row.ErrorDescription += "Độ dài SupplierNo : " + v_exporter + " không hợp lệ! , ";
                                    else row.SupplierNo = v_exporter;

                                    if (v_exporterCode.Length > 50) row.ErrorDescription += "Độ dài SupplierCd : " + v_exporterCode + " không hợp lệ! , ";
                                    else row.SupplierCd = v_exporterCode;

                                    if (v_bodyColor.Length > 100) row.ErrorDescription += "Độ dài BodyColor : " + v_bodyColor + " không hợp lệ! , ";
                                    else row.BodyColor = v_bodyColor;




                                    row.Grade = Convert.ToString(v_worksheet.Cells[7, j].Value);

                                    if (Convert.ToDecimal(v_worksheet.Cells[i, j].Value) < 0)
                                    {
                                        row.ErrorDescription += "Usage Qty không được âm";
                                    }
                                    else
                                    {
                                        row.UsageQty = Convert.ToDecimal(v_worksheet.Cells[i, j].Value);

                                    }

                                    row.Remark = v_remark;

                                    listImport.Add(row);
                                }


                            }
                            if (flag_HasRecord == false)
                            {
                                var row = new ImportPioPartListDto();
                                row.Guid = strGUID;
                             

                                if (v_shop.Length > 1) row.ErrorDescription += "Độ dài Shop : " + v_shop + " không hợp lệ! , ";
                                else row.Shop = v_shop;

                                if (v_idLine.Length > 10) row.ErrorDescription += "Độ dài IdLine : " + v_idLine + " không hợp lệ! , ";
                                else row.IdLine = v_idLine;

                                if (v_partNo.Length > 50) row.ErrorDescription += "Độ dài PartNo : " + v_partNo + " không hợp lệ! , ";

                                if (v_exporter.Length > 50) row.ErrorDescription += "Độ dài SupplierNo : " + v_exporter + " không hợp lệ! , ";
                                else row.SupplierNo = v_exporter;

                                if (v_exporterCode.Length > 50) row.ErrorDescription += "Độ dài SupplierCd : " + v_exporterCode + " không hợp lệ! , ";
                                else row.SupplierCd = v_exporterCode;

                                if (v_bodyColor.Length > 100) row.ErrorDescription += "Độ dài BodyColor : " + v_bodyColor + " không hợp lệ! , ";
                                else row.BodyColor = v_bodyColor;

                              

                                row.ErrorDescription = "";


                                row.Remark = v_remark;

                                listImport.Add(row);
                            }

                        }

                    }
                    // import temp into db (bulkCopy)
                    if (listImport.Count > 0)
                    {
                        IEnumerable<ImportPioPartListDto> dataE = listImport.AsEnumerable();
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
                                    bulkCopy.DestinationTableName = "InvPioPart_T";
                                    bulkCopy.ColumnMappings.Add("Guid", "Guid");
                                    bulkCopy.ColumnMappings.Add("Cfc", "Cfc");
                                    bulkCopy.ColumnMappings.Add("Shop", "Shop");
                                    bulkCopy.ColumnMappings.Add("IdLine", "IdLine");
                                    bulkCopy.ColumnMappings.Add("PartNo", "PartNo");
                                    bulkCopy.ColumnMappings.Add("PartName", "PartName");
                                    bulkCopy.ColumnMappings.Add("SupplierNo", "SupplierNo");
                                    bulkCopy.ColumnMappings.Add("SupplierCd", "SupplierCd");
                                    bulkCopy.ColumnMappings.Add("BodyColor", "BodyColor");
                                  
                                    bulkCopy.ColumnMappings.Add("Grade", "Grade");
                                    bulkCopy.ColumnMappings.Add("UsageQty", "UsageQty");

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
        public async Task MergeDataInvPioPartList(string v_Guid)
        {

            string _merge = "Exec [INV_PIO_PART_LIST_INL_MERGE] @Guid";
            await _dapperRepo.QueryAsync<ImportCkdPartListDto>(_merge, new { Guid = v_Guid });
        }
        public async Task<PagedResultDto<ImportCkdPartListDto>> GetMessageErrorImport(string v_Guid, string v_screen)
        {
            string _sql = "Exec [INV_PIO_PART_LIST_GET_LIST_ERROR_IMPORT] @Guid, @Screen";

            IEnumerable<ImportCkdPartListDto> result = await _dapperRepo.QueryAsync<ImportCkdPartListDto>(_sql, new
            {
                Guid = v_Guid,
                Screen = v_screen
            });

            var listResult = result.ToList();
            var totalCount = listResult.Count();

            return new PagedResultDto<ImportCkdPartListDto>(
                totalCount,
               listResult
               );
        }
        public async Task<FileDto> GetListErrPartListInlToExcel(string v_Guid, string v_Screen)
        {
            FileDto a = new FileDto();
            string _sql = "Exec INV_PIO_PART_LIST_GET_LIST_ERROR_IMPORT @Guid, @Screen";

            IEnumerable<ImportPioPartListDto> result = await _dapperRepo.QueryAsync<ImportPioPartListDto>(_sql, new
            {
                Guid = v_Guid,
                Screen = v_Screen
            });

            var exportToExcel = result.ToList();
            if (v_Screen == "PxP")
            {
                a = _calendarListExcelExporter.ExportToFileErr(exportToExcel);
            }
            else if (v_Screen == "L")
            {
                a = _calendarListExcelExporter.ExportToFileLotErr(exportToExcel);
            }

            return a;

        }



        public async Task<List<ImportPioPartListDto>> ImportDataInvPioPartListLotFromExcel(byte[] fileBytes, string fileName)
        {
            try
            {
                List<ImportPioPartListDto> listImport = new List<ImportPioPartListDto>();
                using (var stream = new MemoryStream(fileBytes))
                {
                    SpreadsheetInfo.SetLicense("EF21-1FW1-HWZF-CLQH");
                    var xlWorkBook = ExcelFile.Load(stream);
                    var v_worksheet = xlWorkBook.Worksheets[0];
                    DataFormatter formatter = new DataFormatter();
                    DateTime dateTime = DateTime.Now;

                    //string v_devanning_date = (v_worksheet.Cells[4, 2]).Value?.ToString() ?? "";
                    string strGUID = Guid.NewGuid().ToString("N");

                    string _sql = @"EXEC CMM_IMPORT_USER_GUID_INSERT @Guid, @p_UserId";
                    await _dapperRepo.ExecuteAsync(_sql, new
                    {
                        Guid = strGUID,
                        p_UserId = AbpSession.UserId
                    });

                    int countGrade = 0;

                    //Đếm số lượng bản ghi Grade
                    for (int i = 10; i <= 500; i++)
                    {
                        if (!string.IsNullOrEmpty(Convert.ToString(v_worksheet.Cells[0, i].Value)))
                        {
                            countGrade++;
                        }
                        else { break; }

                    }

                    bool flag_HasRecord = false;
                    for (int i = 2; i < v_worksheet.Rows.Count; i++)
                    {

                        string v_partNo = (v_worksheet.Cells[i, 4]).Value?.ToString().Replace("-", "") ?? "";

                        if (v_partNo != "")
                        {

                            string v_orderPattern = (v_worksheet.Cells[i, 0]).Value?.ToString() ?? "";
                            string v_model = (v_worksheet.Cells[i, 1]).Value?.ToString() ?? "";
                            string v_shop = (v_worksheet.Cells[i, 3]).Value?.ToString() ?? "";
                            string v_partName = (v_worksheet.Cells[i, 5]).Value?.ToString() ?? "";
                            string v_supplierNo = (v_worksheet.Cells[i, 8]).Value?.ToString() ?? "";

                            flag_HasRecord = false;
                            for (int j = 10; j < countGrade + 10; j++)
                            {
                                if (!string.IsNullOrEmpty(Convert.ToString(v_worksheet.Cells[i, j].Value)) && Convert.ToString(v_worksheet.Cells[i, j].Value) != "0")
                                {
                                    flag_HasRecord = true;
                                    var row = new ImportPioPartListDto();
                                    row.Guid = strGUID;
                                    //decimal eUsageQty = 0;
                                    //decimal.TryParse(Convert.ToString(v_worksheet.Cells[i, j].Value), out eUsageQty);

                                    row.Guid = strGUID;

                                    if (v_orderPattern.Length > 50) row.ErrorDescription = "Độ dài Order Pattern : " + v_orderPattern + " không hợp lệ! , ";
                                    else row.OrderPattern = v_orderPattern.ToUpper();


                                    if (v_model.Length > 4) row.ErrorDescription = "Độ dài Model : " + v_model + " không hợp lệ! , ";
                                    else row.Cfc = v_model;

                                    if (v_shop.Length > 1) row.ErrorDescription = "Độ dài Shop : " + v_shop + " không hợp lệ! , ";
                                    else row.Shop = v_shop;

                                    if (v_partNo.Length > 50) row.ErrorDescription = "Độ dài PartNo : " + v_partNo + " không hợp lệ! , ";
                                    else row.PartNo = v_partNo;

                                    if (v_partName.Length > 500) row.ErrorDescription = "Độ dài PartName : " + v_partName + " không hợp lệ! , ";
                                    else row.PartName = v_partName;


                                    if (v_supplierNo.Length > 100) row.ErrorDescription = "Độ dài Source : " + v_supplierNo + " không hợp lệ! , ";
                                    else row.SupplierNo = v_supplierNo;



                                    row.Grade = Convert.ToString(v_worksheet.Cells[0, j].Value);

                                    if (Convert.ToDecimal(v_worksheet.Cells[i, j].Value) < 0)
                                    {
                                        row.ErrorDescription = "Usage Qty không được âm";
                                    }
                                    else
                                    {
                                        row.UsageQty = Convert.ToDecimal(v_worksheet.Cells[i, j].Value);

                                    }


                                    listImport.Add(row);


                                }
                            }
                            if (flag_HasRecord == false)
                            {
                                var row = new ImportPioPartListDto();
                                row.Guid = strGUID;
                                if (v_orderPattern.Length > 50) row.ErrorDescription = "Độ dài Order Pattern : " + v_orderPattern + " không hợp lệ! , ";
                                else row.OrderPattern = v_orderPattern.ToUpper();

                                if (v_model.Length > 50) row.ErrorDescription = "Độ dài Model : " + v_model + " không hợp lệ! , ";
                                else row.Cfc = v_model;

                                if (v_shop.Length > 1) row.ErrorDescription = "Độ dài Shop : " + v_shop + " không hợp lệ! , ";
                                else row.Shop = v_shop;


                                if (v_partNo.Length > 50) row.ErrorDescription = "Độ dài PartNo : " + v_partNo + " không hợp lệ! , ";
                                else row.PartNo = v_partNo;

                                if (v_partName.Length > 500) row.ErrorDescription = "Độ dài PartName : " + v_partName + " không hợp lệ! , ";
                                else row.PartName = v_partName;

                                if (v_supplierNo.Length > 100) row.ErrorDescription = "Độ dài Source : " + v_supplierNo + " không hợp lệ! , ";
                                else row.SupplierNo = v_supplierNo;


                                listImport.Add(row);
                            }

                        }

                    }
                    if (listImport.Count > 0)
                    {
                        IEnumerable<ImportPioPartListDto> dataE = listImport.AsEnumerable();
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
                                    bulkCopy.DestinationTableName = "InvPioPart_T";
                                    bulkCopy.ColumnMappings.Add("Guid", "Guid");
                                    bulkCopy.ColumnMappings.Add("OrderPattern", "OrderPattern");
                                    bulkCopy.ColumnMappings.Add("Cfc", "Cfc");
                                    bulkCopy.ColumnMappings.Add("Shop", "Shop");
                                    bulkCopy.ColumnMappings.Add("PartNo", "PartNo");
                                    bulkCopy.ColumnMappings.Add("PartName", "PartName");
                                    bulkCopy.ColumnMappings.Add("SupplierNo", "SupplierNo");

                                    bulkCopy.ColumnMappings.Add("Grade", "Grade");
                                    bulkCopy.ColumnMappings.Add("UsageQty", "UsageQty");

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



        // Excel
        public async Task<FileDto> GetPioPartExportToFile(InvCkdPartListExportInput input)
        {
            var file = new FileDto("PIOPartList.xlsx", MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet);
            SpreadsheetInfo.SetLicense("EF21-1FW1-HWZF-CLQH");
            string fileName = "temp_PIOPartListInl";
            string template = "wwwroot/Template";
            string path = "";
            path = Path.Combine(Directory.GetCurrentDirectory(), template, fileName);
            var xlWorkBook = ExcelFile.Load(path + ".xlsx");
            var workSheet = xlWorkBook.Worksheets[0];

            List<string> listExport = new List<string>();
            listExport.Add("Id");
            listExport.Add("Cfc");
            listExport.Add("Model");
            listExport.Add("Shop");
            listExport.Add("IdLine");
            listExport.Add("PartNo");
            listExport.Add("PartName");
            listExport.Add("SupplierNo");
            listExport.Add("SupplierCd");
            listExport.Add("BodyColor");
            listExport.Add("StartLot");
            listExport.Add("EndLot");
            listExport.Add("StartRun");
            listExport.Add("EndRun");
            listExport.Add("Blank");
            listExport.Add("Remark");
            listExport.Add("Value");

            string[] properties = listExport.ToArray();
            string _sql = "Exec [INV_PIO_PART_LIST_INL_EXCEL] @part_no,@cfc, @p_model, @p_grade, @p_shop,@supplierNo,@p_order_pattern";
            var listDataHeader = (await _dapperRepo.QueryAsync<ExportPioPartListDto>(_sql, new
            {
                part_no = input.PartNo,
                cfc = input.Cfc,
                p_model = input.Model,
                p_grade = input.Grade,
                p_shop = input.Shop,
                supplierNo = input.SupplierNo,
                p_order_pattern = input.OrderPattern,
            })).ToList();

            _sql = "Exec [INV_PIO_PART_GRADE_BY_GRADE] @p_part_no";
            var listGrade = (await _partGradeInl.QueryAsync<ExportCkdPartListGradeDto>(_sql, new { p_part_no = input.PartNo })).ToList();


            DataTable table = new DataTable();
            using (var reader = ObjectReader.Create(listDataHeader, properties))
            {
                table.Load(reader);
            }
            var style = new CellStyle();
            style.Borders.SetBorders(MultipleBorders.All, SpreadsheetColor.FromArgb(0, 0, 0), GemBox.Spreadsheet.LineStyle.Thin);
            style.FillPattern.SetSolid(SpreadsheetColor.FromArgb(255, 255, 102));

            var styleheader = new CellStyle();
            styleheader.Borders.SetBorders(MultipleBorders.All, SpreadsheetColor.FromArgb(0, 0, 0), GemBox.Spreadsheet.LineStyle.Thin);
            styleheader.FillPattern.SetSolid(SpreadsheetColor.FromArgb(89, 89, 89));
            styleheader.Font.Color = SpreadsheetColor.FromName(ColorName.White);
            styleheader.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            styleheader.VerticalAlignment = VerticalAlignmentStyle.Center;

            for (int i = 0; i < listGrade.Count; i++)
            {
                table.Columns.Add(listGrade[i].Grade, typeof(int));
       
                workSheet.Cells[0, 18 + i].Value = listGrade[i].Grade;
                workSheet.Cells[0, 18 + i].Style = styleheader;
            }
            workSheet.Cells[0, 18 + listGrade.Count].Value = "Remark";
            workSheet.Cells[0, 18 + listGrade.Count].Style = styleheader;




            for (int i = 0; i < table.Rows.Count; i++)
            {
                DataRow dr = table.Rows[i];

           /*     foreach (string x in dr["Value"].ToString().Split(","))
                {
                    if (x != null && x != "")
                        dr[x.Split("-")[0]] = x.Split("-")[1] == "" ? 0 : int.Parse(x.Split("-")[1]);
                }*/

                //set style excel with Gembox 
                for (int column_index = 0; column_index <= (18 + listGrade.Count); column_index++)
                {
                    if (column_index == 17) { continue; }
                    var cell = workSheet.Cells[i + 2, column_index];
                    cell.Style = style;
                }

            }

            if (table.Rows.Count > 0)
            {
                table.Columns.Remove("Value");
                table.Columns["Remark"].SetOrdinal(table.Columns.Count - 1);
                InsertDataTableOptions ins = new InsertDataTableOptions(2, 0);
                workSheet.InsertDataTable(table, ins);
            }


            var tempFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".xlsx");
            xlWorkBook.Save(tempFile);
            MemoryStream obj_stream = new MemoryStream();
            obj_stream = new MemoryStream(File.ReadAllBytes(tempFile));
            _tempFileCacheManager.SetFile(file.FileToken, obj_stream.ToArray());
            File.Delete(tempFile);
            obj_stream.Position = 0;
            return file;
        }


        public async Task<FileDto> GetPioPartGroupBodyColorExportToFile(InvCkdPartListExportInput input)
        {
            var file = new FileDto("PIOPartListGroupBodyColor.xlsx", MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet);
            SpreadsheetInfo.SetLicense("EF21-1FW1-HWZF-CLQH");
            string fileName = "temp_PIOPartListInl";
            string template = "wwwroot/Template";
            string path = "";
            path = Path.Combine(Directory.GetCurrentDirectory(), template, fileName);
            var xlWorkBook = ExcelFile.Load(path + ".xlsx");
            var workSheet = xlWorkBook.Worksheets[0];

            List<string> listExport = new List<string>();
            listExport.Add("Id");
            listExport.Add("Cfc");
            listExport.Add("Model");
            listExport.Add("Shop");
            listExport.Add("IdLine");
            listExport.Add("PartNo");
            listExport.Add("PartName");
            listExport.Add("SupplierNo");
            listExport.Add("SupplierCd");
            listExport.Add("BodyColor");
            listExport.Add("StartLot");
            listExport.Add("EndLot");
            listExport.Add("StartRun");
            listExport.Add("EndRun");
            listExport.Add("Blank");
            listExport.Add("Remark");
            listExport.Add("Value");

            string[] properties = listExport.ToArray();
            string _sql = "Exec [INV_PIO_PART_LIST_GROUP_BODY_COLOR_SEARCH] @part_no,@cfc,@p_model, @p_grade, @p_shop,@supplierNo,@p_order_pattern";
            var listDataHeader = (await _dapperRepo.QueryAsync<ExportPioPartListDto>(_sql, new
            {
                part_no = input.PartNo,
                cfc = input.Cfc,
                p_model = input.Model,
                p_grade = input.Grade,
                p_shop = input.Shop,
                supplierNo = input.SupplierNo,
                p_order_pattern = input.OrderPattern,
            })).ToList();

            _sql = "Exec INV_PIO_PART_GRADE_BY_GRADE @p_part_no";
            var listGrade = (await _partGradeInl.QueryAsync<ExportCkdPartListGradeDto>(_sql, new { p_part_no = input.PartNo })).ToList();


            DataTable table = new DataTable();
            using (var reader = ObjectReader.Create(listDataHeader, properties))
            {
                table.Load(reader);
            }
            var style = new CellStyle();
            style.Borders.SetBorders(MultipleBorders.All, SpreadsheetColor.FromArgb(0, 0, 0), GemBox.Spreadsheet.LineStyle.Thin);
            style.FillPattern.SetSolid(SpreadsheetColor.FromArgb(255, 255, 102));

            var styleheader = new CellStyle();
            styleheader.Borders.SetBorders(MultipleBorders.All, SpreadsheetColor.FromArgb(0, 0, 0), GemBox.Spreadsheet.LineStyle.Thin);
            styleheader.FillPattern.SetSolid(SpreadsheetColor.FromArgb(89, 89, 89));
            styleheader.Font.Color = SpreadsheetColor.FromName(ColorName.White);
            styleheader.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            styleheader.VerticalAlignment = VerticalAlignmentStyle.Center;

            for (int i = 0; i < listGrade.Count; i++)
            {
                table.Columns.Add(listGrade[i].Grade, typeof(int));
                workSheet.Cells[0, 18 + i].Value = listGrade[i].Grade;
                workSheet.Cells[0, 18 + i].Style = styleheader;
            }
            workSheet.Cells[0, 18 + listGrade.Count].Value = "Remark";
            workSheet.Cells[0, 18 + listGrade.Count].Style = styleheader;



            for (int i = 0; i < table.Rows.Count; i++)
            {
                DataRow dr = table.Rows[i];

             /*   foreach (string x in dr["Value"].ToString().Split(","))
                {
                    if (x != null && x != "")
                        dr[x.Split("-")[0]] = x.Split("-")[1] == "" ? 0 : int.Parse(x.Split("-")[1]);
                }*/

                //set style excel with Gembox 
                for (int column_index = 0; column_index <= (18 + listGrade.Count); column_index++)
                {
                    if (column_index == 17) { continue; }
                    var cell = workSheet.Cells[i + 2, column_index];
                    cell.Style = style;
                }

            }

            if (table.Rows.Count > 0)
            {
                table.Columns.Remove("Value");
                table.Columns["Remark"].SetOrdinal(table.Columns.Count - 1);
                InsertDataTableOptions ins = new InsertDataTableOptions(2, 0);
                workSheet.InsertDataTable(table, ins);
            }


            var tempFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".xlsx");
            xlWorkBook.Save(tempFile);
            MemoryStream obj_stream = new MemoryStream();
            obj_stream = new MemoryStream(File.ReadAllBytes(tempFile));
            _tempFileCacheManager.SetFile(file.FileToken, obj_stream.ToArray());
            File.Delete(tempFile);
            obj_stream.Position = 0;
            return file;
        }


        [AbpAuthorize(AppPermissions.Pages_PIO_Master_PartListInl_Add)]
        public async Task PartGradeEdit(InvCkdPartGradeDto input)
        {
            await _dapperRepo.ExecuteAsync(@"
                  exec [dbo].[INV_PIO_PART_GRADE_EDIT]
                                @id,
                                @usageQty,
                                @shop,
                                @grade,
                                @bodyColor,
                                @startLot,
                                @endLot,
                                @startRun,
                                @endRun,
                                @efFromPart,
                                @efToPart,
                                @orderPattern,
                                @remark,
                                @p_UserId
                                                                         
                ", new 
            {
                id = input.Id,
                usageQty = input.UsageQty,
                shop = input.Shop,
                grade = input.Grade,
                bodyColor = input.BodyColor,
                startLot = input.StartLot,
                endLot = input.EndLot,
                startRun = input.StartRun,
                endRun = input.EndRun,
                efFromPart = input.EfFromPart,
                efToPart = input.EfToPart,
                orderPattern = input.OrderPattern,
                remark = input.Remark,
                p_UserId = AbpSession.UserId
            });
        }

        public async Task<List<string>> GetPartListInlHistory(GetInvCkdPartListHistoryInput input)
        {
            return await _historicalDataAppService.GetHistoricalDataById(input.Id, input.TableName, "Id");
        }

        public async Task<FileDto> GetHistoricalDataToExcel(GetInvCkdPartListHistoryExcelInput input)
        {
            var data = await _historicalDataAppService.GetHistoricalDataById(input.Id, input.TableName, "Id");
            return _calendarListExcelExporter.ExportToHistoricalFile(data, input.TableName);
        }
        public async Task<ChangedRecordIdsDto> GetChangedRecords()
        {
            var listPartNo = await _historicalDataAppService.GetChangedRecordIds("InvPioPartListInl");
            var listPartGrade = await _historicalDataAppService.GetChangedRecordIds("InvPioPartGradeInl");

            ChangedRecordIdsDto result = new ChangedRecordIdsDto();
            result.PartList = listPartNo;
            result.PartGrade = listPartGrade;
            return result;
        }


        public async Task<List<GetGradeDto>> GetListGradesByCfc(string cfc)
        {
            IEnumerable<GetGradeDto> result = await _dapperRepo.QueryAsync<GetGradeDto>("Exec MST_CMM_LOT_CODE_GRADE_GET_BY_CFC @p_Cfc", new { p_Cfc = cfc });
            return result.ToList();
        }


        [AbpAuthorize(AppPermissions.Pages_PIO_Master_PartListInl_Add)]
        public async Task EciPartGrade(long? v_id, string v_StartLot, string v_StartRun, string v_Grade, int? v_usageQty)
        {
            await _dapperRepo.ExecuteAsync(@" exec [dbo].[INV_PIO_PART_GRADE_ECI] @id, @p_StartLot, @p_StartRun, @p_Grade , @p_UserId, @p_usageQty ", new
            {
                id = v_id,
                p_StartLot = v_StartLot,
                p_StartRun = v_StartRun,
                p_Grade = v_Grade,
                p_UserId = AbpSession.UserId,
                p_usageQty = v_usageQty
            });
        }

    }
}
