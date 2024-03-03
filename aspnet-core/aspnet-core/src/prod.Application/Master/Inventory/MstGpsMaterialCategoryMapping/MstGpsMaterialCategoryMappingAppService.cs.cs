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
using prod.Authorization;
using prod.Common;
using prod.Dto;
using prod.EntityFrameworkCore;
using prod.Master.Inventory.Dto;
using prod.Master.Inventory.Exporting;
using prod.Master.Inventory.GpsMaterialCategory.Dto;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System;
using System.Linq;
using System.Threading.Tasks;
using prod.HistoricalData;

namespace prod.Master.Inventory
{
	[AbpAuthorize(AppPermissions.Pages_Master_Gps_MaterialCategoryMapping_View)]
	public class MstGpsMaterialCategoryMappingAppService : prodAppServiceBase, IMstGpsMaterialCategoryMappingAppService
	{
		private readonly IDapperRepository<MstGpsMaterialCategoryMapping, long> _dapperRepo;
		private readonly IRepository<MstGpsMaterialCategoryMapping, long> _repo;
		private readonly IMstGpsMaterialCategoryMappingExcelExporter _calendarListExcelExporter;
        private readonly IHistoricalDataAppService _historicalDataAppService;

        public MstGpsMaterialCategoryMappingAppService(IRepository<MstGpsMaterialCategoryMapping, long> repo,
										 IDapperRepository<MstGpsMaterialCategoryMapping, long> dapperRepo,
										IMstGpsMaterialCategoryMappingExcelExporter calendarListExcelExporter,
                                        IHistoricalDataAppService historicalDataAppService
            )
		{
			_repo = repo;
			_dapperRepo = dapperRepo;
			_calendarListExcelExporter = calendarListExcelExporter;
            _historicalDataAppService = historicalDataAppService;
		}
        public async Task<List<string>> GetMaterialCategoryMappingHistory(GetMaterialCategoryMappingHistoryInput input)
        {
            return await _historicalDataAppService.GetHistoricalDataById(input.Id, input.TableName, "Id");
        }

        public async Task<FileDto> GetHistoricalDataToExcel(GetMaterialCategoryMappingHistoryExcelInput input)
        {
            var data = await _historicalDataAppService.GetHistoricalDataById(input.Id, input.TableName, "Id");
            return _calendarListExcelExporter.ExportToHistoricalFile(data);
        }

        public async Task<List<long?>> GetChangedRecords()
        {
            return await _historicalDataAppService.GetChangedRecordIds("MstGpsMaterialCategoryMapping");
        }


        [AbpAuthorize(AppPermissions.Pages_Master_Gps_MaterialCategory_CreateEdit)]
		public async Task CreateOrEdit(CreateOrEditMstGpsMaterialCategoryMappingDto input)
		{
			if (input.Id == null) await Create(input);
			else await Update(input);
		}
		//CREATE
		private async Task Create(CreateOrEditMstGpsMaterialCategoryMappingDto input)
		{
			var mainObj = ObjectMapper.Map<MstGpsMaterialCategoryMapping>(input);

			await CurrentUnitOfWork.GetDbContext<prodDbContext>().AddAsync(mainObj);
		}

		// EDIT
		private async Task Update(CreateOrEditMstGpsMaterialCategoryMappingDto input)
		{
			using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant))
			{
				var mainObj = await _repo.GetAll()
				.FirstOrDefaultAsync(e => e.Id == input.Id);

				var mainObjToUpdate = ObjectMapper.Map(input, mainObj);
			}
		}

		// DELETE
		[AbpAuthorize(AppPermissions.Pages_Master_Gps_MaterialCategoryMapping_CreateEdit)]
		public async Task Delete(EntityDto input)
		{
			var mainObj = await _repo.FirstOrDefaultAsync(input.Id);
			_repo.HardDelete(mainObj);
			CurrentUnitOfWork.GetDbContext<prodDbContext>().Remove(mainObj);
		}

		public async Task<PagedResultDto<MstGpsMaterialCategoryMappingDto>> GetAll(GetMstGpsMaterialCategoryMappingInput input)
		{
			var filtered = _repo.GetAll()
				.WhereIf(!string.IsNullOrWhiteSpace(input.YVCategory), e => e.YVCategory.Contains(input.YVCategory)) 
				.WhereIf(!string.IsNullOrWhiteSpace(input.GLExpenseAccount), e => e.GLExpenseAccount.Contains(input.GLExpenseAccount));
            var pageAndFiltered = filtered.OrderBy(s => s.Id);


			var system = from o in pageAndFiltered
						 select new MstGpsMaterialCategoryMappingDto
						 {
							 Id = o.Id,
							 YVCategory = o.YVCategory,
							 GLExpenseAccount = o.GLExpenseAccount,
							 GLLevel5InWBS = o.GLLevel5InWBS,
							 GLAccountDescription = o.GLAccountDescription,
							 Definition = o.Definition,
							 FixedVariableCost = o.FixedVariableCost,
							 Example = o.Example,
                             AccountType = o.AccountType,
                             PostingKey = o.PostingKey,
                             PartType = o.PartType,
                             DocumentType = o.DocumentType,
                             IsAsset = o.IsAsset,
                             RevertCancel = o.RevertCancel,
                             IsActive = o.IsActive							 
						 };

			var totalCount = await filtered.CountAsync();
			var paged = system.PageBy(input);

			return new PagedResultDto<MstGpsMaterialCategoryMappingDto>(
				totalCount,
				 await paged.ToListAsync()
			);
		}
		public async Task<FileDto> GetMaterialCategoryMappingToExcel(GetMstGpsMaterialCategoryMappingExcelInput input)
		{
			var query = from o in _repo.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.YVCategory), e => e.YVCategory.Contains(input.YVCategory))
						.WhereIf(!string.IsNullOrWhiteSpace(input.GLExpenseAccount), e => e.GLExpenseAccount.Contains(input.GLExpenseAccount))
            
						select new MstGpsMaterialCategoryMappingDto
						{
                            Id = o.Id,
                            YVCategory = o.YVCategory,
                            GLExpenseAccount = o.GLExpenseAccount,
                            GLLevel5InWBS = o.GLLevel5InWBS,
                            GLAccountDescription = o.GLAccountDescription,
                            Definition = o.Definition,
                            FixedVariableCost = o.FixedVariableCost,
                            Example = o.Example,
                            AccountType = o.AccountType,
                            PostingKey = o.PostingKey,
                            PartType = o.PartType,
                            DocumentType = o.DocumentType,
                            IsAsset = o.IsAsset,
                            RevertCancel = o.RevertCancel,
                            IsActive = o.IsActive
                        };
			var exportToExcel = await query.ToListAsync();
			return _calendarListExcelExporter.ExportToFile(exportToExcel);
		}
        public async Task<List<MstGpsMaterialCategoryMappingImportDto>> ImportMaterialCategoryMappingFromExcel(byte[] fileBytes, string fileName)
        {
            try
            {
                List<MstGpsMaterialCategoryMappingImportDto> listImport = new List<MstGpsMaterialCategoryMappingImportDto>();
                using (var stream = new MemoryStream(fileBytes))
                {
                    SpreadsheetInfo.SetLicense("EF21-1FW1-HWZF-CLQH");
                    var xlWorkBook = ExcelFile.Load(stream);
                    string strGUID = Guid.NewGuid().ToString("N");

                    ExcelWorksheet v_worksheet_p1 = xlWorkBook.Worksheets[0];

                    int row = 2;
                    int col = 1;

                    while (true)
                    {
                        if (v_worksheet_p1.Cells[row, col].Value == null) { break; }
                        MstGpsMaterialCategoryMappingImportDto listData = new MstGpsMaterialCategoryMappingImportDto();
                        listData.Guid = strGUID;
                        listData.YVCategory = v_worksheet_p1.Cells[row, col ].Value != null ? v_worksheet_p1.Cells[row, col].Value.ToString() : null;
                        listData.GLExpenseAccount = v_worksheet_p1.Cells[row, col + 1].Value != null ? v_worksheet_p1.Cells[row, col + 1].Value.ToString() : null;
                        listData.GLLevel5InWBS = v_worksheet_p1.Cells[row, col + 2].Value != null ? v_worksheet_p1.Cells[row, col + 2].Value.ToString() : null;
                        listData.GLAccountDescription = v_worksheet_p1.Cells[row, col + 3].Value != null ? v_worksheet_p1.Cells[row, col + 3].Value.ToString() : null;
                        listData.Definition = v_worksheet_p1.Cells[row, col + 4].Value != null ? v_worksheet_p1.Cells[row, col + 4].Value.ToString() : null;
                        listData.FixedVariableCost = v_worksheet_p1.Cells[row, col + 5].Value != null ? v_worksheet_p1.Cells[row, col + 5].Value.ToString() : null;
                        listData.Example = v_worksheet_p1.Cells[row, col + 6].Value != null ? v_worksheet_p1.Cells[row, col + 6].Value.ToString() : null;
                        listData.AccountType = v_worksheet_p1.Cells[row, col + 7].Value != null ? v_worksheet_p1.Cells[row, col + 7].Value.ToString() : null;
                        listData.PostingKey = v_worksheet_p1.Cells[row, col + 8].Value != null ? v_worksheet_p1.Cells[row, col + 8].Value.ToString() : null;
                        listData.PartType = v_worksheet_p1.Cells[row, col + 9].Value != null ? v_worksheet_p1.Cells[row, col + 9].Value.ToString() : null;
                        listData.DocumentType = v_worksheet_p1.Cells[row, col + 10].Value != null ? v_worksheet_p1.Cells[row, col + 10].Value.ToString() : null;
                        listData.IsAsset = v_worksheet_p1.Cells[row, col + 11].Value != null ? v_worksheet_p1.Cells[row, col + 11].Value.ToString() : null;
                        listData.RevertCancel = v_worksheet_p1.Cells[row, col + 12].Value != null ? v_worksheet_p1.Cells[row, col + 12].Value.ToString() : null;
                        listData.IsActive = v_worksheet_p1.Cells[row, col + 13].Value != null ? v_worksheet_p1.Cells[row, col + 13].Value.ToString() : null;
                        listData.CreatorUserId = AbpSession.UserId;
                        listImport.Add(listData);
                        row++;
                    }

                    // import temp into db (bulkCopy)
                    if (listImport.Count > 0)
                    {
                        IEnumerable<MstGpsMaterialCategoryMappingImportDto> dataE = listImport.AsEnumerable();
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
                                    bulkCopy.DestinationTableName = "MstGpsMaterialCategoryMapping_T";
                                    bulkCopy.ColumnMappings.Add("YVCategory", "YVCategory");
                                    bulkCopy.ColumnMappings.Add("GLExpenseAccount", "GLExpenseAccount");
                                    bulkCopy.ColumnMappings.Add("GLLevel5InWBS", "GLLevel5InWBS");
                                    bulkCopy.ColumnMappings.Add("GLAccountDescription", "GLAccountDescription");
                                    bulkCopy.ColumnMappings.Add("Definition", "Definition");
                                    bulkCopy.ColumnMappings.Add("FixedVariableCost", "FixedVariableCost");
                                    bulkCopy.ColumnMappings.Add("Example", "Example");
                                    bulkCopy.ColumnMappings.Add("IsActive", "IsActive");
                                    bulkCopy.ColumnMappings.Add("Guid", "Guid");
                                    bulkCopy.ColumnMappings.Add("PostingKey", "PostingKey");
                                    bulkCopy.ColumnMappings.Add("PartType", "PartType");
                                    bulkCopy.ColumnMappings.Add("RevertCancel", "RevertCancel");
                                    bulkCopy.ColumnMappings.Add("AccountType", "AccountType");
                                    bulkCopy.ColumnMappings.Add("DocumentType", "DocumentType");
                                    bulkCopy.ColumnMappings.Add("IsAsset", "IsAsset");
                                    bulkCopy.ColumnMappings.Add("CreatorUserId", "CreatorUserId");
                                    bulkCopy.WriteToServer(table);
                                    tran.Commit();
                                }
                            }
                            await conn.CloseAsync();
                        }
                    }
                    /// merge vào bảng chính
                    var _sqlMerge = "Exec [MST_GPS_MATERIAL_CATEGORY_MAPPING_MERGE] @p_guid";
                    _dapperRepo.Execute(_sqlMerge, new
                    {
                        p_guid = strGUID,
                    });


                    return listImport;
                }

            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(400, ex.Message);
            }
        }


        public async Task<List<ListCategoryDto>> GetListCategory()
        {
            string _sql = "Exec INV_GPS_GET_LIST_CATEGORY";
            IEnumerable<ListCategoryDto> result = await _dapperRepo.QueryAsync<ListCategoryDto>(_sql);
            return result.ToList();
        }
    }
}
