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
using prod.HistoricalData;
using prod.Inventory.GPS.Dto;
using prod.Master.Inventory.Dto;
using prod.Master.Inventory.Exporting;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prod.Master.Inventory
{
	[AbpAuthorize(AppPermissions.Pages_Master_Gps_WbsCCMapping_View)]
	public class MstGpsWbsCCMappingAppService : prodAppServiceBase, IMstGpsWbsCCMappingAppService
	{
		private readonly IDapperRepository<MstGpsWbsCCMapping, long> _dapperRepo;
		private readonly IRepository<MstGpsWbsCCMapping, long> _repo;
		private readonly IMstGpsWbsCCMappingExcelExporter _mstGpsWbsCCMappingListExcelExporter;
		private readonly IHistoricalDataAppService _historicalDataAppService;
		public MstGpsWbsCCMappingAppService(IRepository<MstGpsWbsCCMapping, long> repo,
										 IDapperRepository<MstGpsWbsCCMapping, long> dapperRepo,
										IMstGpsWbsCCMappingExcelExporter mstGpsWbsCCMappingListExcelExporter,
										IHistoricalDataAppService historicalDataAppService
			)
		{
			_repo = repo;
			_dapperRepo = dapperRepo;
			_mstGpsWbsCCMappingListExcelExporter = mstGpsWbsCCMappingListExcelExporter;
			_historicalDataAppService = historicalDataAppService;
		}

		public async Task<List<string>> GetGpsWbsCCMappingHistory(GetMstGpsWbsCCMappingHistoryInput input)
		{
			return await _historicalDataAppService.GetHistoricalDataById(input.Id, input.TableName, "Id");
		}

		public async Task<FileDto> GetHistoricalDataToExcel(GetMstGpsWbsCCMappingHistoryExcelInput input)
		{
			var data = await _historicalDataAppService.GetHistoricalDataById(input.Id, input.TableName, "Id");
			return _mstGpsWbsCCMappingListExcelExporter.ExportToHistoricalFile(data);
		}

		public async Task<List<long?>> GetChangedRecords()
		{
			return await _historicalDataAppService.GetChangedRecordIds("MstGpsWbsCCMapping");
		}

		[AbpAuthorize(AppPermissions.Pages_Master_Gps_WbsCCMapping_CreateEdit)]
		public async Task CreateOrEdit(CreateOrEditMstGpsWbsCCMappingDto input)
		{
			if (input.Id == null) await Create(input);
			else await Update(input);
		}

		//CREATE
		private async Task Create(CreateOrEditMstGpsWbsCCMappingDto input)
		{
			var mainObj = ObjectMapper.Map<MstGpsWbsCCMapping>(input);

			await CurrentUnitOfWork.GetDbContext<prodDbContext>().AddAsync(mainObj);
		}

		// EDIT
		private async Task Update(CreateOrEditMstGpsWbsCCMappingDto input)
		{
			using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant))
			{
				var mainObj = await _repo.GetAll()
				.FirstOrDefaultAsync(e => e.Id == input.Id);

				var mainObjToUpdate = ObjectMapper.Map(input, mainObj);
			}
		}

		// DELETE
		[AbpAuthorize(AppPermissions.Pages_Master_Gps_WbsCCMapping_CreateEdit)]
		public async Task Delete(EntityDto input)
		{
			var mainObj = await _repo.FirstOrDefaultAsync(input.Id);
			_repo.HardDelete(mainObj);
			CurrentUnitOfWork.GetDbContext<prodDbContext>().Remove(mainObj);
		}

		//GET ALL
		public async Task<PagedResultDto<MstGpsWbsCCMappingDto>> GetAll(GetMstGpsWbsCCMappingInput input)
		{
			var filtered = _repo.GetAll()
				.WhereIf(!string.IsNullOrWhiteSpace(input.CostCenterFrom), e => e.CostCenterFrom.Contains(input.CostCenterFrom))
				.WhereIf(!string.IsNullOrWhiteSpace(input.WbsFrom), e => e.WbsFrom.Contains(input.WbsFrom));
			var pageAndFiltered = filtered.OrderBy(s => s.Id);


			var system = from o in pageAndFiltered
						 select new MstGpsWbsCCMappingDto
						 {
							 Id = o.Id,
							 CostCenterFrom = o.CostCenterFrom,
							 WbsFrom = o.WbsFrom,
							 CostCenterTo = o.CostCenterTo,
							 WbsTo = o.WbsTo,
							 EffectiveFromDate = o.EffectiveFromDate,
							 EffectiveFromTo = o.EffectiveFromTo,
							 IsActive = o.IsActive
						 };

			var totalCount = await filtered.CountAsync();
			var paged = system.PageBy(input);

			return new PagedResultDto<MstGpsWbsCCMappingDto>(
				totalCount,
				 await paged.ToListAsync()
			);
		}

		//EXPORT TO EXCEL
		public async Task<FileDto> GetMstGpsWbsCCMappingToExcel(GetMstGpsWbsCCMappingExcelInput input)
		{
			var query = from o in _repo.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.CostCenterFrom), e => e.CostCenterFrom.Contains(input.CostCenterFrom))
						.WhereIf(!string.IsNullOrWhiteSpace(input.WbsFrom), e => e.WbsFrom.Contains(input.WbsFrom))

						select new MstGpsWbsCCMappingDto
						{
							Id = o.Id,
							CostCenterFrom = o.CostCenterFrom,
							WbsFrom = o.WbsFrom,
							CostCenterTo = o.CostCenterTo,
							WbsTo = o.WbsTo,
							EffectiveFromDate = o.EffectiveFromDate,
							EffectiveFromTo = o.EffectiveFromTo,
							IsActive = o.IsActive
						};
			var exportToExcel = await query.ToListAsync();
			return _mstGpsWbsCCMappingListExcelExporter.ExportToFile(exportToExcel);
		}

		//IMPORT EXCEL
		public async Task<List<MstGpsWbsCCMappingImportDto>> ImportGpsWbsCCMappingFromExcel(byte[] fileBytes, string fileName)
		{
			try
			{
				List<MstGpsWbsCCMappingImportDto> listImport = new List<MstGpsWbsCCMappingImportDto>();
				using (var stream = new MemoryStream(fileBytes))
				{
					SpreadsheetInfo.SetLicense("EF21-1FW1-HWZF-CLQH");
					var xlWorkBook = ExcelFile.Load(stream);
					string strGUID = Guid.NewGuid().ToString("N");

					ExcelWorksheet v_worksheet_p1 = xlWorkBook.Worksheets[0];

					int row = 1;
					int col = 0;

					while (true)
					{
						if (v_worksheet_p1.Cells[row, col].Value == null) { break; }
						MstGpsWbsCCMappingImportDto listData = new MstGpsWbsCCMappingImportDto();
						listData.Guid = strGUID;
						listData.CostCenterFrom = v_worksheet_p1.Cells[row, col].Value != null ? v_worksheet_p1.Cells[row, col].Value.ToString() : null;
						listData.WbsFrom = v_worksheet_p1.Cells[row, col + 1].Value != null ? v_worksheet_p1.Cells[row, col + 1].Value.ToString() : null;
						listData.CostCenterTo = v_worksheet_p1.Cells[row, col + 2].Value != null ? v_worksheet_p1.Cells[row, col + 2].Value.ToString() : null;
						listData.WbsTo = v_worksheet_p1.Cells[row, col + 3].Value != null ? v_worksheet_p1.Cells[row, col + 3].Value.ToString() : null;
						listData.EffectiveFromDate = Convert.ToDateTime( v_worksheet_p1.Cells[row, col + 4].Value );
						listData.EffectiveFromTo = Convert.ToDateTime(v_worksheet_p1.Cells[row, col + 5].Value);
						listData.IsActive = v_worksheet_p1.Cells[row, col + 6].Value != null ? v_worksheet_p1.Cells[row, col + 6].Value.ToString() : null;
						listData.CreatorUserId = AbpSession.UserId;
						listImport.Add(listData);
						row++;
					}

					// import temp into db (bulkCopy)
					if (listImport.Count > 0)
					{
						IEnumerable<MstGpsWbsCCMappingImportDto> dataE = listImport.AsEnumerable();
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
									bulkCopy.DestinationTableName = "MstGpsWbsCCMapping_T";
									bulkCopy.ColumnMappings.Add("CostCenterFrom", "CostCenterFrom");
									bulkCopy.ColumnMappings.Add("WbsFrom", "WbsFrom");
									bulkCopy.ColumnMappings.Add("CostCenterTo", "CostCenterTo");
									bulkCopy.ColumnMappings.Add("WbsTo", "WbsTo");
									bulkCopy.ColumnMappings.Add("EffectiveFromDate", "EffectiveFromDate");
									bulkCopy.ColumnMappings.Add("EffectiveFromTo", "EffectiveFromTo");
									bulkCopy.ColumnMappings.Add("IsActive", "IsActive");
									bulkCopy.ColumnMappings.Add("Guid", "Guid");
									bulkCopy.ColumnMappings.Add("CreatorUserId", "CreatorUserId");
									bulkCopy.WriteToServer(table);
									tran.Commit();
								}
							}
							await conn.CloseAsync();
						}
					}
					/// merge vào bảng chính
					var _sqlMerge = "Exec [MST_GPS_WBS_CC_MAPPING_MERGE] @p_guid";
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

		public async Task<MessageMstGpsWbsCCMappingDto> CheckCostCenterFromAndWbsFrom(string CostCenterFrom, string WbsFrom, string CostCenterTo, string WbsTo)
		{
			string _sql = "Exec MST_GPS_WBS_CC_MAPPING_EXIST_COSTCENTERFROM_WBSFROM @p_costCenterFrom,@p_wbsFrom,@p_costCenterTo,@p_wbsTo ";
			IEnumerable<MessageMstGpsWbsCCMappingDto> result = await _dapperRepo.QueryAsync<MessageMstGpsWbsCCMappingDto>(_sql, new
			{
				p_costCenterFrom = CostCenterFrom,
				p_wbsFrom = WbsFrom,
				p_costCenterTo = CostCenterTo,
				p_wbsTo = WbsTo
			});

			return result.FirstOrDefault();
		}

		public async Task<MessageInvGpsExistMappingDto> CheckCostCenterFromAndWbsFromInInvGpsMapping(string CostCenterFrom, string WbsFrom)
		{
			string _sql = "Exec MST_GPS_WBS_CC_MAPPING_EXIST_COSTCENTERFROM_WBSFROM_IN_INVGPSMAPPING @p_costCenterFrom,@p_wbsFrom ";
			IEnumerable<MessageInvGpsExistMappingDto> result = await _dapperRepo.QueryAsync<MessageInvGpsExistMappingDto>(_sql, new
			{
				p_costCenterFrom = CostCenterFrom,
				p_wbsFrom = WbsFrom
			});

			return result.FirstOrDefault();
		}
	}
}
