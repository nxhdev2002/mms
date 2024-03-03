using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Dapper.Repositories;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.EntityFrameworkCore.Repositories;
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
using prod.Inventory.PIO.PartList.Dto;
using prod.Master.Inv;
using prod.Master.Inventory.Dto;
using prod.Master.Inventory.Exporting;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twilio.Rest.Api.V2010.Account;

namespace prod.Master.Inventory
{
    [AbpAuthorize(AppPermissions.Pages_Master_CKD_DemDetFees_View)]
    public class MstInvDemDetFeesAppService : prodAppServiceBase, IMstInvDemDetFeesAppService
    {
        private readonly IDapperRepository<MstInvDemDetFees, long> _dapperRepo;
        private readonly IRepository<MstInvDemDetFees, long> _repo;
        private readonly IMstInvDemDetFeesExcelExporter _demdetfeesExcelExporter;
        private readonly Abp.ObjectMapping.IObjectMapper _objectMapper;

        public MstInvDemDetFeesAppService(IRepository<MstInvDemDetFees, long> repo,
                                         IDapperRepository<MstInvDemDetFees, long> dapperRepo,
                                         IMstInvDemDetFeesExcelExporter demdetfeesExcelExporter,
                                         Abp.ObjectMapping.IObjectMapper objectMapper)
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _demdetfeesExcelExporter = demdetfeesExcelExporter;
            _objectMapper = objectMapper;
        }


        [AbpAuthorize(AppPermissions.Pages_Master_CKD_DemDetFees_Edit)]
        public async Task CreateOrEdit(CreateOrEditMstInvDemDetFeesDto input)
        {
            if (input.Id == null) await Create(input);
            else await Update(input);
        }

        //CREATE
        private async Task Create(CreateOrEditMstInvDemDetFeesDto input)
        {
            var mainObj = ObjectMapper.Map<MstInvDemDetFees>(input);

            await CurrentUnitOfWork.GetDbContext<prodDbContext>().AddAsync(mainObj);
        }

        // EDIT
        private async Task Update(CreateOrEditMstInvDemDetFeesDto input)
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

        public async Task<PagedResultDto<MstInvDemDetFeesDto>> GetAll(GetMstInvDemDetFeesInput input)
        {
            string _sql = "Exec INV_CKD_MST_INV_DEM_DET_FEES_SEARCH @p_Source,@p_Carrier";

            IEnumerable<MstInvDemDetFeesDto> result = await _dapperRepo.QueryAsync<MstInvDemDetFeesDto>(_sql, new
            {
                p_Source = input.Source,
                p_Carrier = input.Carrier,
            });
            var listResult = result.ToList();

            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();


            var totalCount = result.ToList().Count();

            return new PagedResultDto<MstInvDemDetFeesDto>(
                totalCount,
                pagedAndFiltered);
        }

        public async Task<FileDto> GetMstInvDemDetFeesToExcel(GetMstInvDemDetFeesInput input)
        {
            string _sql = "Exec INV_CKD_MST_INV_DEM_DET_FEES_SEARCH @p_Source,@p_Carrier";

            IEnumerable<MstInvDemDetFeesDto> result = await _dapperRepo.QueryAsync<MstInvDemDetFeesDto>(_sql, new
            {
                p_Source = input.Source,
                p_Carrier = input.Carrier,
            });
            var exportToExcel = result.ToList();
            return _demdetfeesExcelExporter.ExportToFile(exportToExcel);
        }
        public async Task<List<GetSourceDto>> GetListSource()
        {
            IEnumerable<GetSourceDto> result = await _dapperRepo.QueryAsync<GetSourceDto>("SELECT DISTINCT SupplierNo  AS Source FROM MstInvSupplierList where SupplierNo <> ''");
            return result.ToList();
        }
        public async Task<List<GetCarrieerDto>> GetListCarrier()
        {
            IEnumerable<GetCarrieerDto> result = await _dapperRepo.QueryAsync<GetCarrieerDto>("SELECT DISTINCT ShippingcompanyCode AS Carrier FROM InvCkdShipment where ShippingcompanyCode <> ''");
            return result.ToList();
        }
        public async Task<List<MstInvDemDetFeesImportDto>> ImportMstInvDemDetFeesFromExcel(byte[] fileBytes, string fileName)
        {
            try
            {
                List<MstInvDemDetFeesImportDto> listImport = new List<MstInvDemDetFeesImportDto>();
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
                        MstInvDemDetFeesImportDto mstInvDemDetFeesImportDto = new MstInvDemDetFeesImportDto();
                        mstInvDemDetFeesImportDto.Source = v_worksheet_p1.Cells[row, col].Value != null ? v_worksheet_p1.Cells[row, col].Value.ToString() : null;
                        mstInvDemDetFeesImportDto.Carrier = v_worksheet_p1.Cells[row, col + 1].Value != null ? v_worksheet_p1.Cells[row, col + 1].Value.ToString() : null;
                        var errors = String.Empty;

                        var ContType = Convert.ToInt32(v_worksheet_p1.Cells[row, col + 2].Value != null ? v_worksheet_p1.Cells[row, col + 2].Value.ToString() : null);
                        if (ContType > 0)
                        {
                            mstInvDemDetFeesImportDto.ContType = ContType;
                        }
                        else
                        {
                            errors = "ContType phải >=0";
                        }

                        if (v_worksheet_p1.Cells[row, col + 3].Value != null)
                        {
                            if (v_worksheet_p1.Cells[row, col + 3].Value.ToString().Contains(">="))
                            {

                                mstInvDemDetFeesImportDto.NoOfDayOVF = Convert.ToInt32(v_worksheet_p1.Cells[row, col + 3].Value != null ? v_worksheet_p1.Cells[row, col + 3].Value.ToString().Replace(">=", "") : null);
                                mstInvDemDetFeesImportDto.IsMax = "Y";
                            }
                            else
                            {
                                mstInvDemDetFeesImportDto.NoOfDayOVF = Convert.ToInt32(v_worksheet_p1.Cells[row, col + 3].Value != null ? v_worksheet_p1.Cells[row, col + 3].Value.ToString() : null);
                                mstInvDemDetFeesImportDto.IsMax = "N";
                            }
                        }

                        var DemFee = Convert.ToDecimal(v_worksheet_p1.Cells[row, col + 4].Value != null ? v_worksheet_p1.Cells[row, col + 4].Value.ToString() : null);
                        if (DemFee >= 0)
                        {
                            mstInvDemDetFeesImportDto.DemFee = DemFee;
                        }
                        else
                        {
                            errors = "DemFee phải >=0";
                        }

                        var DetFee = Convert.ToDecimal(v_worksheet_p1.Cells[row, col + 5].Value != null ? v_worksheet_p1.Cells[row, col + 5].Value.ToString() : null);
                        if (DetFee >= 0)
                        {
                            mstInvDemDetFeesImportDto.DetFee = DetFee;
                        }
                        else
                        {
                            errors = "DetFee phải >=0";
                        }

                        var DemAndDetFee = Convert.ToDecimal(v_worksheet_p1.Cells[row, col + 6].Value != null ? v_worksheet_p1.Cells[row, col + 6].Value.ToString() : null);
                        if (DemAndDetFee >= 0)
                        {
                            mstInvDemDetFeesImportDto.DemAndDetFee = DemAndDetFee;
                        }
                        else
                        {
                            errors = "DemAndDetFee phải >=0";
                        }

                        mstInvDemDetFeesImportDto.ErrorDescription = errors;
                        mstInvDemDetFeesImportDto.Guid = strGUID;
                        listImport.Add(mstInvDemDetFeesImportDto);
                        row++;
                    }

                    // import temp into db (bulkCopy)
                    var importCandidates = listImport.GroupBy(x => new { x.Source, x.Carrier, x.ContType, x.DemFee, x.DetFee, x.DemAndDetFee })
                                            .Select(g => new MstInvDemDetFeesImportDto
                                            {
                                                Source = g.Key.Source,
                                                Carrier = g.Key.Carrier,
                                                ContType = g.Key.ContType,
                                                DemFee = g.Key.DemFee,
                                                DetFee = g.Key.DetFee,
                                                DemAndDetFee = g.Key.DemAndDetFee,
                                                MaxDay = g.Max(x => x.NoOfDayOVF),
                                                MinDay = g.Min(x => x.NoOfDayOVF),
                                                Guid = strGUID
                                            }).ToList();


                    if (listImport.Count > 0)
                    {
                        IEnumerable<MstInvDemDetFeesImportDto> dataE = importCandidates.AsEnumerable();
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
                                    bulkCopy.DestinationTableName = "MstInvDemDetFees_T";
                                    bulkCopy.ColumnMappings.Add("Source", "Source");
                                    bulkCopy.ColumnMappings.Add("Carrier", "Carrier");
                                    bulkCopy.ColumnMappings.Add("ContType", "ContType");
                                    bulkCopy.ColumnMappings.Add("DemFee", "DemFee");
                                    bulkCopy.ColumnMappings.Add("DetFee", "DetFee");
                                    bulkCopy.ColumnMappings.Add("DemAndDetFee", "DemAndDetFee");
                                    bulkCopy.ColumnMappings.Add("MinDay", "MinDay");
                                    bulkCopy.ColumnMappings.Add("MaxDay", "MaxDay");
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

        public async Task MergeDataMstInvDemDetFees(string strGUID)
        {

            string _merge = "Exec [MST_INV_DEM_DET_FEES_MERGE] @p_Guid";
            await _dapperRepo.QueryAsync<MstInvDemDetFeesImportDto>(_merge, new { p_Guid = strGUID });
        }
        public async Task<PagedResultDto<MstInvDemDetFeesImportDto>> GetMessageErrorImport(string v_Guid)
        {
            string _sql = "Exec [MST_INV_DEMDET_FEES_GET_LIST_ERROR_IMPORT] @Guid";

            IEnumerable<MstInvDemDetFeesImportDto> result = await _dapperRepo.QueryAsync<MstInvDemDetFeesImportDto>(_sql, new
            {
                Guid = v_Guid

            });

            var listResult = result.ToList();
            var totalCount = listResult.Count();

            return new PagedResultDto<MstInvDemDetFeesImportDto>(
                totalCount,
               listResult
               );
        }


        public async Task<FileDto> GetListErrDemDetToExcel(string v_Guid)
        {
            FileDto a = new FileDto();
            string _sql = "Exec MST_INV_DEMDET_FEES_GET_LIST_ERROR_IMPORT @Guid";

            IEnumerable<MstInvDemDetFeesImportDto> result = await _dapperRepo.QueryAsync<MstInvDemDetFeesImportDto>(_sql, new
            {
                Guid = v_Guid
            });

            var exportToExcel = result.ToList();

            return _demdetfeesExcelExporter.ExportToFileErr(exportToExcel); ;

        }
    }
}
