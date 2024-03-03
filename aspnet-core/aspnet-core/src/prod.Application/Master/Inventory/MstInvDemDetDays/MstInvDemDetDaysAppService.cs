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
using System.Collections.Generic;
using System.Data;
using System.IO;
using System;
using System.Linq;
using System.Threading.Tasks;
using prod.Inventory.CKD.Dto;


namespace prod.Master.Inventory
{
	[AbpAuthorize(AppPermissions.Pages_Master_CKD_DemDetDays_View)]
	public class MstInvDemDetDaysAppService : prodAppServiceBase, IMstInvDemDetDaysAppService
	{
		private readonly IDapperRepository<MstInvDemDetDays, long> _dapperRepo;
		private readonly IRepository<MstInvDemDetDays, long> _repo;
		private readonly IMstInvDemDetDaysExcelExporter _mstInvDemDetDaysExcelExporter;
		private readonly Abp.ObjectMapping.IObjectMapper _objectMapper;

		public MstInvDemDetDaysAppService(IRepository<MstInvDemDetDays, long> repo,
										 IDapperRepository<MstInvDemDetDays, long> dapperRepo,
										IMstInvDemDetDaysExcelExporter mstInvDemDetDaysExporter
										, Abp.ObjectMapping.IObjectMapper objectMapper)
		{
			_repo = repo;
			_dapperRepo = dapperRepo;
			_mstInvDemDetDaysExcelExporter = mstInvDemDetDaysExporter;
			_objectMapper = objectMapper;
		}
		[AbpAuthorize(AppPermissions.Pages_Master_CKD_DemDetFees_Edit)]
		public async Task CreateOrEdit(CreateOrEditMstInvDemDetDaysDto input)
		{
			if (input.Id == null) await Create(input);
			else await Update(input);
		}
		//CREATE
		private async Task Create(CreateOrEditMstInvDemDetDaysDto input)
		{
			var mainObj = ObjectMapper.Map<MstInvDemDetDays>(input);

			await CurrentUnitOfWork.GetDbContext<prodDbContext>().AddAsync(mainObj);
		}
		// EDIT
		private async Task Update(CreateOrEditMstInvDemDetDaysDto input)
		{
			using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant))
			{
				var mainObj = await _repo.GetAll()
				.FirstOrDefaultAsync(e => e.Id == input.Id);

				var mainObjToUpdate = ObjectMapper.Map(input, mainObj);
			}
		}
		public async Task Delete(EntityDto input)
		{
			var mainObj = await _repo.FirstOrDefaultAsync(input.Id);
			_repo.HardDelete(mainObj);
			CurrentUnitOfWork.GetDbContext<prodDbContext>().Remove(mainObj);
		}
        public async Task<PagedResultDto<MstInvDemDetDaysDto>> GetAll(GetMstInvDemDetDaysInput input)
        {
            string _sql = "Exec INV_CKD_MST_INV_DEM_DET_DAYS_SEARCH @p_Exp,@p_Carrier, @p_CombineDEMDET";

            IEnumerable<MstInvDemDetDaysDto> result = await _dapperRepo.QueryAsync<MstInvDemDetDaysDto>(_sql, new
            {
                p_Exp = input.Exp,
                p_Carrier = input.Carrier,
                p_CombineDEMDET = input.CombineDEMDET,
            });
            var listResult = result.ToList();



            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var totalCount = result.ToList().Count();

            return new PagedResultDto<MstInvDemDetDaysDto>(
                totalCount,
                pagedAndFiltered);
        }
        public async Task<FileDto> GetMstInvDemDetDaysToExcel(GetMstInvDemDetDaysInput input)
        {
            string _sql = "Exec INV_CKD_MST_INV_DEM_DET_DAYS_SEARCH @p_Exp,@p_Carrier, @p_CombineDEMDET";

            IEnumerable<MstInvDemDetDaysDto> result = await _dapperRepo.QueryAsync<MstInvDemDetDaysDto>(_sql, new
            {
                p_Exp = input.Exp,
                p_Carrier = input.Carrier,
                p_CombineDEMDET = input.CombineDEMDET,
            });

            var exportToExcel = result.ToList();
            return _mstInvDemDetDaysExcelExporter.ExportToFile(exportToExcel);
        }
        public async Task<List<GetExpDto>> GetListExp()
        {
            IEnumerable<GetExpDto> result = await _dapperRepo.QueryAsync<GetExpDto>("SELECT DISTINCT SupplierNo  AS Exp FROM MstInvSupplierList where SupplierNo <> ''");
            return result.ToList();
        }
        public async Task<List<GetCarrierDto>> GetListCarrier()
        {
            IEnumerable<GetCarrierDto> result = await _dapperRepo.QueryAsync<GetCarrierDto>("SELECT DISTINCT ShippingcompanyCode AS Carrier FROM InvCkdShipment where ShippingcompanyCode <> ''");
            return result.ToList();
        }
        public async Task<List<MstInvDemDetDaysImportDto>> ImportMstInvDemDetDaysFromExcel(byte[] fileBytes, string fileName)
		{
			try
			{
				List<MstInvDemDetDaysImportDto> listImport = new List<MstInvDemDetDaysImportDto>();
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
						MstInvDemDetDaysImportDto mstInvDemDetDaysImportDto = new MstInvDemDetDaysImportDto();
                        var errors = String.Empty;

                        mstInvDemDetDaysImportDto.Exp = v_worksheet_p1.Cells[row, col].Value != null ? v_worksheet_p1.Cells[row, col].Value.ToString() : null;
						mstInvDemDetDaysImportDto.Carrier = v_worksheet_p1.Cells[row, col + 1].Value != null ? v_worksheet_p1.Cells[row, col + 1].Value.ToString() : null;
						mstInvDemDetDaysImportDto.CombineDEMDET = v_worksheet_p1.Cells[row, col + 2].Value != null ? v_worksheet_p1.Cells[row, col + 2].Value.ToString() : null;

                        var FreeDEM = Convert.ToInt32(v_worksheet_p1.Cells[row, col + 3].Value != null ? v_worksheet_p1.Cells[row, col + 3].Value.ToString() : null);
                        if (FreeDEM >= 0)
                        {
                            mstInvDemDetDaysImportDto.FreeDEM = FreeDEM;
                        }
                        else
                        {
                            errors = "FreeDEM phải >=0";
                        }

                        var FreeDET = Convert.ToInt32(v_worksheet_p1.Cells[row, col + 4].Value != null ? v_worksheet_p1.Cells[row, col + 4].Value.ToString() : null);
                        if (FreeDET >= 0)
                        {
                            mstInvDemDetDaysImportDto.FreeDET = FreeDET;
                        }
                        else
                        {
                            errors = "FreeDET phải >=0";
                        }

                        var CombineDEMDETFree = Convert.ToInt32(v_worksheet_p1.Cells[row, col + 5].Value != null ? v_worksheet_p1.Cells[row, col + 5].Value.ToString() : null);
                        if (CombineDEMDETFree >= 0)
                        {
                            mstInvDemDetDaysImportDto.CombineDEMDETFree = CombineDEMDETFree;
                        }
                        else
                        {
                            errors = "CombineDEMDETFree phải >=0";
                        }
                        mstInvDemDetDaysImportDto.ErrorDescription = errors;
                        mstInvDemDetDaysImportDto.Guid = strGUID;
						listImport.Add(mstInvDemDetDaysImportDto);
						row++;
					}

					// import temp into db (bulkCopy)
					if (listImport.Count > 0)
					{
						IEnumerable<MstInvDemDetDaysImportDto> dataE = listImport.AsEnumerable();
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
									bulkCopy.DestinationTableName = "MstInvDemDetDays_T";
									bulkCopy.ColumnMappings.Add("Exp", "Exp");
									bulkCopy.ColumnMappings.Add("Carrier", "Carrier");
									bulkCopy.ColumnMappings.Add("CombineDEMDET", "CombineDEMDET");
									bulkCopy.ColumnMappings.Add("FreeDEM", "FreeDEM");
									bulkCopy.ColumnMappings.Add("FreeDET", "FreeDET");
									bulkCopy.ColumnMappings.Add("CombineDEMDETFree", "CombineDEMDETFree");
									bulkCopy.ColumnMappings.Add("Guid", "Guid");
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
        public async Task MergeDataMstInvDemDetDays(string strGUID)
        {

            string _merge = "Exec [MST_INV_DEM_DET_DAYS_MERGE] @p_guid";
            await _dapperRepo.QueryAsync<MstInvDemDetDaysImportDto>(_merge, new { p_guid = strGUID });
        }
        public async Task<PagedResultDto<MstInvDemDetDaysImportDto>> GetMessageErrorImport(string v_Guid)
        {
            string _sql = "Exec [MST_INV_DEMDET_DAYS_GET_LIST_ERROR_IMPORT] @Guid";

            IEnumerable<MstInvDemDetDaysImportDto> result = await _dapperRepo.QueryAsync<MstInvDemDetDaysImportDto>(_sql, new
            {
                Guid = v_Guid

            });

            var listResult = result.ToList();
            var totalCount = listResult.Count();

            return new PagedResultDto<MstInvDemDetDaysImportDto>(
                totalCount,
               listResult
               );
        }
        public async Task<FileDto> GetListErrDemDetToExcel(string v_Guid)
        {
            FileDto a = new FileDto();
            string _sql = "Exec MST_INV_DEMDET_DAYS_GET_LIST_ERROR_IMPORT @Guid";

            IEnumerable<MstInvDemDetDaysImportDto> result = await _dapperRepo.QueryAsync<MstInvDemDetDaysImportDto>(_sql, new
            {
                Guid = v_Guid
            });

            var exportToExcel = result.ToList();

            return _mstInvDemDetDaysExcelExporter.ExportToFileErr(exportToExcel); ;

        }
    }
}
