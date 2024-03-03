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
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using prod.Authorization;
using prod.Common;
using prod.Dto;
using prod.EntityFrameworkCore;
using prod.HistoricalData;
using prod.Inventory.CKD.Dto;
using prod.Inventory.Gps.PartListByCategory.Dto;
using prod.Inventory.GPS;
using prod.Inventory.PIO.PartList.Dto;
using prod.Inventory.PIO.PartList.Exporting;
using prod.Inventory.PIO.PartListOff.Dto;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace prod.Inventory.PIO.PartList
{
    [AbpAuthorize(AppPermissions.Pages_PIO_Master_PartList_View)]
    public class InvPioPartListAppService : prodAppServiceBase, IInvPioPartListAppService
    {
        private readonly IDapperRepository<InvPioPartList, long> _dapperRepo;
        private readonly IRepository<InvPioPartList, long> _repo;
        private readonly IInvPioPartListExcelExporter _calendarListExcelExporter;
        private readonly IHistoricalDataAppService _historicalDataAppService;
        public InvPioPartListAppService(IRepository<InvPioPartList, long> repo,
                                         IDapperRepository<InvPioPartList, long> dapperRepo,
                                        IInvPioPartListExcelExporter calendarListExcelExporter,
                                        IHistoricalDataAppService historicalDataAppService 
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
            _historicalDataAppService = historicalDataAppService;
        }

        public async Task<List<string>> GetPartListHistory(GetInvPioPartListHistoryInput input)
        {
            return await _historicalDataAppService.GetHistoricalDataById(input.Id, input.TableName, "Id");
        }

        public async Task<ChangedRecordIdsInvPioPartListDto> GetChangedRecords()
        {
            var listPartNo = await _historicalDataAppService.GetChangedRecordIds("InvPioPartList");

            ChangedRecordIdsInvPioPartListDto result = new()
            {
                PartList = listPartNo
            };
            return result;
        }

        [AbpAuthorize(AppPermissions.Pages_PIO_Master_PartList_CreateEdit)]
        public async Task CreateOrEdit(CreateOrEditInvPioPartListDto input)

        {
            if (input.Id == null)
            {
                await PIOPartListAdd(input);
            }
            else
            {
                await PIOPartListEdit(input);
            }
        }
        [AbpAuthorize(AppPermissions.Pages_PIO_Master_PartList_CreateEdit)]

        public async Task PIOPartListAdd(CreateOrEditInvPioPartListDto input)
        { 

            string _sql = @"EXEC INV_PIO_PART_LIST_INSERT @FullModel,
                                @ProdSfx,
                                @MktCode,
                                @PartNo,
                                @PartName,
                                @PartType,
                                @PioType,
                                @BoxSize,
                                @StartDate,
                                @EndDate,
                                @Supplier,
                                @Remark,
                                @Model,
                                @Cfc,
                                @Grade,
                                @MaterialId,
                                @p_UserId";
            await _dapperRepo.ExecuteAsync(_sql,new
            {
                FullModel = input.FullModel,
                ProdSfx = input.ProdSfx,
                MktCode = input.MktCode,
                PartNo = input.PartNo,
                PartName = input.PartName,
                PartType = input.PartType,
                PioType = input.PioType,
                BoxSize = input.BoxSize,
                StartDate = input.StartDate,
                EndDate = input.EndDate,
                Supplier = input.Supplier,
                Remark = input.Remark,
                Model = input.Model,
                Cfc = input.Cfc,
                Grade = input.Grade,
                MaterialId = input.MaterialId,
                p_UserId = AbpSession.UserId

            });
        
        }
        
        [AbpAuthorize(AppPermissions.Pages_PIO_Master_PartList_CreateEdit)]

        public async Task PIOPartListEdit(CreateOrEditInvPioPartListDto input)
        {

                string _sql = @"EXEC INV_PIO_PART_LIST_EDIT 
                                @p_Id,
                                @FullModel,
                                @ProdSfx,
                                @MktCode,
                                @PartNo,
                                @PartName,
                                @PartType,
                                @PioType,
                                @BoxSize,
                                @StartDate,
                                @EndDate,
                                @Supplier,
                                @Remark,
                                @Model,
                                @Cfc,
                                @Grade,
                                @MaterialId,
                                @p_UserId,
                                @IsActive";
                await _dapperRepo.ExecuteAsync(_sql, new
                 
                {
                    p_Id = input.Id,
                    FullModel = input.FullModel,
                    ProdSfx = input.ProdSfx,
                    MktCode = input.MktCode,
                    PartNo = input.PartNo,
                    PartName = input.PartName,
                    PartType = input.PartType,
                    PioType = input.PioType,
                    BoxSize = input.BoxSize,
                    StartDate = input.StartDate,
                    EndDate = input.EndDate,
                    Supplier = input.Supplier,
                    Remark = input.Remark,
                    Model = input.Model,
                    Cfc = input.Cfc,
                    Grade = input.Grade,
                    MaterialId = input.MaterialId,
                    p_UserId = AbpSession.UserId,
                    IsActive = input.IsActive

                });

        }




        /* public async Task CreateOrEdit(CreateOrEditInvPioPartListDto input)
         {
             if (input.Id == null) await Create(input);
             else await Update(input);
         }

         //CREATE
         private async Task Create(CreateOrEditInvPioPartListDto input)
         {
             var mainObj = ObjectMapper.Map<InvPioPartList>(input);

             await CurrentUnitOfWork.GetDbContext<prodDbContext>().AddAsync(mainObj);
         }

         // EDIT
         private async Task Update(CreateOrEditInvPioPartListDto input)
         {
             using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant))
             {
                 var mainObj = await _repo.GetAll()
                 .FirstOrDefaultAsync(e => e.Id == input.Id);

                 var mainObjToUpdate = ObjectMapper.Map(input, mainObj);
             }
         }*/

        [AbpAuthorize(AppPermissions.Pages_PIO_Master_PartList_CreateEdit)]
        public async Task Delete(EntityDto input)
        {
            var mainObj = await _repo.FirstOrDefaultAsync(input.Id);
            _repo.HardDelete(mainObj);
            CurrentUnitOfWork.GetDbContext<prodDbContext>().Remove(mainObj);
        }



        public async Task<PagedResultDto<InvPioPartListDto>> GetAll(GetInvPioPartListInput input)
        {

            string _sql = "Exec INV_PIO_PART_LIST_SEARCH @p_model,@p_marketing_code,@p_part_no,@p_is_active";

            IEnumerable<InvPioPartListDto> result = await _dapperRepo.QueryAsync<InvPioPartListDto>(_sql, new
            {
                p_model = input.FullModel,
                p_marketing_code = input.MktCode,
                p_part_no = input.PartNo,
                p_is_active = input.IsActive,
            });

            var listResult = result.ToList();

            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var totalCount = result.ToList().Count();

            return new PagedResultDto<InvPioPartListDto>(
                totalCount,
                 pagedAndFiltered);
        }


        public async Task<FileDto> GetPioPartListToExcel(InvPioPartListExportInput input)
        {
            string _sql = "Exec INV_PIO_PART_LIST_SEARCH @p_model,@p_marketing_code,@p_part_no,@p_is_active";

            IEnumerable<InvPioPartListDto> result = await _dapperRepo.QueryAsync<InvPioPartListDto>(_sql, new
            {
                p_model = input.FullModel,
                p_marketing_code = input.MktCode,
                p_part_no = input.PartNo,
                p_is_active = input.IsActive,
            });
            var exportToExcel = result.ToList();
            return _calendarListExcelExporter.ExportToFile(exportToExcel);
        }

        public async Task<List<InvPioPartListImportDto>> ImportPartListFromExcel(byte[] fileBytes, string fileName)
        {
            try
            {
                using (var stream = new MemoryStream(fileBytes))
                {
                    SpreadsheetInfo.SetLicense("EF21-1FW1-HWZF-CLQH");
                    var xlWorkBook = ExcelFile.Load(stream);
                    string strGUID = Guid.NewGuid().ToString("N");

                    ExcelWorksheet v_worksheet_p1 = null;

                    v_worksheet_p1 = xlWorkBook.Worksheets[0];

                    if (v_worksheet_p1 == null)
                    {
                        throw new UserFriendlyException(400, "Invalid template");
                    }



                    int row, col;
                    var listResult = new List<InvPioPartListImportDto>();
                    /// lấy danh sách các marketing code
                    /// start L7
                    var cr = new CellReference("L7");
                    row = cr.Row;
                    col = cr.Col;
                    var marketingCodeList = new List<string>();
                    while (true)
                    {
                        var cell = v_worksheet_p1.Cells[row, col];
                        if (cell.Value == null)
                        {
                            break;
                        }
                        marketingCodeList.Add(cell.Value.ToString());
                        col++;
                    }

                    /// đọc dữ liệu từ file excel
                    /// row = 8, col = 1
                    var dateFormat = "dd/MM/yyyy HH:mm:ss";
                    row = 8;
                    col = 1;
                    while (true)
                    {
                        if (v_worksheet_p1.Cells[row, col].Value == null)
                        {
                            break;
                        }   
                        var type = v_worksheet_p1.Cells[row, col].Value?.ToString();
                        var model = v_worksheet_p1.Cells[row, col + 2].Value?.ToString();
                        var partNo = v_worksheet_p1.Cells[row, col + 3].Value?.ToString();
                        var partName = v_worksheet_p1.Cells[row, col + 4].Value?.ToString();
                        var eciPart = v_worksheet_p1.Cells[row, col + 5].Value?.ToString();
                        var boxSize = int.Parse(v_worksheet_p1.Cells[row, col + 6].Value?.ToString());
                        var errors = String.Empty;
                        DateTime? startDate = null;
                        DateTime? endDate = null;
                        if (v_worksheet_p1.Cells[row, col + 7].Value != null)
                        {
                            startDate = DateTime.ParseExact(v_worksheet_p1.Cells[row, col + 7].Value?.ToString(), dateFormat, CultureInfo.InvariantCulture);
                        }
                        if (v_worksheet_p1.Cells[row, col + 8].Value != null)
                        {
                            endDate = DateTime.ParseExact(v_worksheet_p1.Cells[row, col + 8].Value?.ToString(), dateFormat, CultureInfo.InvariantCulture);
                        }


                        // validate boxqty > 0, startdate < enddate
                        if (boxSize <= 0)
                        {
                            errors += "BoxSize phải > 0, ";
                        }
                        if (startDate != null && endDate != null)
                        {
                            if (startDate > endDate)
                            {
                                errors += "StartDate phải nhỏ hơn EndDate, ";
                            }
                        }


                        // đọc tiếp thông tin marketing code từ marketingCodeList, bắt đầu từ col = 11
                        var startMktCodeCol = 11;
                        foreach (var marketingCode in marketingCodeList)
                        {
                            var record = new InvPioPartListImportDto();
                            record.Type = type;
                            record.Model = model;
                            record.PartNo = partNo;
                            record.PartName = partName;
                            record.ECIPart = eciPart;
                            record.BoxSize = boxSize;
                            record.StartDate = startDate;
                            record.EndDate = endDate;
                            record.Guid = strGUID;
                            record.ErrorDescription = errors;

                            var cell = v_worksheet_p1.Cells[row, startMktCodeCol];
                            record.MktCode = marketingCode;
                            if (cell.Value != null)
                            {
                                record.Qty = int.Parse(cell.Value.ToString());
                                listResult.Add(record);
                            }
                            startMktCodeCol++;
                        }
                        row++;
                    }

                    // import temp into db (bulkCopy)
                    if (listResult.Count > 0)
                    {
                        IEnumerable<InvPioPartListImportDto> dataE = listResult.AsEnumerable();
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
                                    bulkCopy.DestinationTableName = "InvPioPartList_T";
                                    bulkCopy.ColumnMappings.Add("Type", "Type");
                                    bulkCopy.ColumnMappings.Add("Model", "Model");
                                    bulkCopy.ColumnMappings.Add("PartNo", "PartNo");
                                    bulkCopy.ColumnMappings.Add("PartName", "PartName");
                                    bulkCopy.ColumnMappings.Add("ECIPart", "ECIPart");
                                    bulkCopy.ColumnMappings.Add("BoxSize", "BoxSize");
                                    bulkCopy.ColumnMappings.Add("StartDate", "StartDate");
                                    bulkCopy.ColumnMappings.Add("EndDate", "EndDate");
                                    bulkCopy.ColumnMappings.Add("MktCode", "MktCode");
                                    bulkCopy.ColumnMappings.Add("Qty", "Qty");
                                    bulkCopy.ColumnMappings.Add("Guid", "Guid");
                                    bulkCopy.ColumnMappings.Add("ErrorDescription", "ErrorDescription");
                                    bulkCopy.WriteToServer(table);
                                    tran.Commit();
                                }
                            }
                            await conn.CloseAsync();
                        }
                    }
                    


                    return listResult;
                }
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(400, ex.Message);
            }
        }

        public async Task MergeDataInvPioPartList(string v_Guid)
        {

            string _merge = "Exec [INV_PIO_PART_LIST_MERGE] @Guid";
            await _dapperRepo.QueryAsync<InvPioPartListImportDto>(_merge, new { Guid = v_Guid });
        }

        public async Task<PagedResultDto<InvPioPartListImportDto>> GetMessageErrorImport(string v_Guid)
        {
            string _sql = "Exec [INV_PIO_PART_LIST_ERROR_IMPORT] @Guid";

            IEnumerable<InvPioPartListImportDto> result = await _dapperRepo.QueryAsync<InvPioPartListImportDto>(_sql, new
            {
                Guid = v_Guid

            });

            var listResult = result.ToList();
            var totalCount = listResult.Count();

            return new PagedResultDto<InvPioPartListImportDto>(
                totalCount,
               listResult
               );
        }


        public async Task<FileDto> GetListErrPartListOffToExcel(string v_Guid)
        {
            FileDto a = new FileDto();
            string _sql = "Exec INV_PIO_PART_LIST_ERROR_IMPORT @Guid";

            IEnumerable<InvPioPartListImportDto> result = await _dapperRepo.QueryAsync<InvPioPartListImportDto>(_sql, new
            {
                Guid = v_Guid
            });

            var exportToExcel = result.ToList();

            return _calendarListExcelExporter.ExportToFileErr(exportToExcel); ;

        }

        // public async Task GenerateAsync()
        //  {
        //    await _dapperRepo.ExecuteAsync(InvPioPartListConsts.SP_MST_WPT_CALENDAR_GENERATE);
        // }

    }
}
