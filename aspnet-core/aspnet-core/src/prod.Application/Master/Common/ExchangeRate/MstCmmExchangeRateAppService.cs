using Abp.Application.Services.Dto;
using Abp.AspNetZeroCore.Net;
using Abp.Authorization;
using Abp.Dapper.Repositories;
using Abp.Domain.Repositories;
using GemBox.Spreadsheet;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using prod.Authorization;
using prod.Common;
using prod.Dto;
using prod.HistoricalData;
using prod.Master.Common.Dto;
using prod.Master.Common.VehicleCBU.Exporting;
using prod.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace prod.Master.Common
{
    [AbpAuthorize(AppPermissions.Pages_Master_Common_ExchangeRate_View)]
    public class MstCmmExchangeRateAppService : prodAppServiceBase, IMstCmmExchangeRateAppService
    {
        private readonly IDapperRepository<MstCmmExchangeRate, long> _dapperRepo;
        private readonly IRepository<MstCmmExchangeRate, long> _repo;
        private readonly IHistoricalDataAppService _historicalDataAppService;
        private readonly ITempFileCacheManager _tempFileCacheManager;
        private readonly ILogger _logger;


        public MstCmmExchangeRateAppService(IRepository<MstCmmExchangeRate, long> repo,
                                         IDapperRepository<MstCmmExchangeRate, long> dapperRepo,
                                         IHistoricalDataAppService historicalDataAppService,
                                         ITempFileCacheManager tempFileCacheManager
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _historicalDataAppService = historicalDataAppService;
            _tempFileCacheManager = tempFileCacheManager;
        }

        public async Task<List<string>> GetMstCmmExchangeRateHistory(GetMstCmmExchangeRateHistoryInput input)
        {
            return await _historicalDataAppService.GetHistoricalDataById(input.Id, input.TableName, "Id");
        }

        public async Task<FileDto> GetHistoricalDataToExcel(GetMstCmmExchangeRateHistoryExcelInput input)
        {
            var data = await _historicalDataAppService.GetHistoricalDataById(input.Id, input.TableName, "Id");
            return ExportToHistoricalFile(data);
        }

        public FileDto ExportToHistoricalFile(List<string> data)
        {
            string fileName = "MstCmmExchangeRateHistorical.xlsx";
            var tempFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".xlsx");
            var file = new FileDto(fileName, MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet);

            var allHeaders = new List<string>();
            var rowDatas = new List<JObject>();
            var exceptCols = new List<string>()
        {
            "UpdateMask",
            "LastModificationTime",
        };

            try
            {
                SpreadsheetInfo.SetLicense("EF21-1FW1-HWZF-CLQH");
                var xlWorkBook = new ExcelFile();
                var workSheet = xlWorkBook.Worksheets.Add("Sheet1");

                foreach (var item in data)
                {
                    var json = JObject.Parse(item);
                    rowDatas.Add(json);
                    foreach (var prop in json.Properties())
                    {
                        if (!allHeaders.Contains(prop.Name) && !exceptCols.Contains(prop.Name))
                        {
                            allHeaders.Add(prop.Name);
                        }
                    }
                }

                var properties = allHeaders.Where(x => !exceptCols.Contains(x)).ToArray();


                /// Mapping Data
                MappingData(ref rowDatas, ref properties);

                Commons.FillHistoriesExcel(rowDatas, workSheet, 1, 0, properties, properties);
                xlWorkBook.Save(tempFile);
                using (var obj_stream = new MemoryStream(File.ReadAllBytes(tempFile)))
                {
                    _tempFileCacheManager.SetFile(file.FileToken, obj_stream.ToArray());
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"[EXCEPTION] While exporting {nameof(MstCmmExchangeRateAppService)} with error: {ex}");
            }
            finally
            {
                File.Delete(tempFile);
            }

            return file;
        }

        private void MappingData(ref List<JObject> rowDatas, ref string[] properties)
        {
            /// Mapping row data
            /// add new here
            var dataMapping = new Dictionary<string, Dictionary<string, string>>()
            {
                {
                    "Action", new Dictionary<string, string>() {
                        {"1", "Xoá"},
                        {"2", "Tạo mới"},
                        {"3", "Trước update"},
                        {"4", "Sau update"},
                    }
                    //"Header1", new Dictionary<string, string>()
                    //{
                    //    {"1", "Xoá"},
                    //    {"2", "Tạo mới"},
                    //    {"3", "Trước update"},
                    //    {"4", "Sau update"},
                    //},
                },
            };

            foreach (var header in dataMapping)
            {
                rowDatas.ConvertAll(x =>
                    x[header.Key] = dataMapping[header.Key][x[header.Key].ToString()]
                );
            }
        }

        public async Task<List<long?>> GetChangedRecords()
        {
            return await _historicalDataAppService.GetChangedRecordIds("MstCmmExchangeRate");
        }

        public async Task<PagedResultDto<MstCmmExchangeRateDto>> GetAll(GetMstCmmExchangeRateInput input)
        {
            string _sql = "Exec MST_CMM_EXCHANGE_RATE_SEARCH @p_ExchangeDateFrom, @p_ExchangeDateTo, @p_Status";

            IEnumerable<MstCmmExchangeRateDto> result = await _dapperRepo.QueryAsync<MstCmmExchangeRateDto>(_sql, new
            {
                p_ExchangeDateFrom = input.ExchangeDateFrom,
                p_ExchangeDateTo = input.ExchangeDateTo,
                p_Status = input.Status
            });

            var listResult = result.ToList();
            var totalCount = listResult.Count();

            return new PagedResultDto<MstCmmExchangeRateDto>(
                totalCount,
                listResult
               );
        }

        [AbpAuthorize(AppPermissions.Pages_Master_Common_ExchangeRate_Approve)]
        public async Task<int> ConFirmExchangeRate(int id, string status)
        {
            try
            {
                string _sql = @"EXEC MST_CMM_EXCHANGE_RATE_CONFIRM @p_id, @p_status, @p_confirm_date, @p_user_id";
                var filtered = await _dapperRepo.ExecuteAsync(_sql, new
                {
                    p_id = id,
                    p_status = status,
                    p_confirm_date = DateTime.Now,
                    p_user_id = AbpSession.UserId
                }); ;
                return filtered;
            }
            catch (Exception E)
            {
                return 0;
            }
        }

        public async Task<PagedResultDto<MstCmmExchangeRateDiffDto>> GetDataDiffExchangeRate(MstCmmExchangeRateDiffDtoInput input)
        {
            string _sql = "Exec MST_CMM_EXCHANGE_RATE_DIFF_GET @p_guid";

            IEnumerable<MstCmmExchangeRateDiffDto> result = await _dapperRepo.QueryAsync<MstCmmExchangeRateDiffDto>(_sql, new
            {
                p_guid = input.Guid
            });

            var listResult = result.ToList();
            var totalCount = listResult.Count();

            return new PagedResultDto<MstCmmExchangeRateDiffDto>(
                totalCount,
                listResult
               );

        }

    }
}
