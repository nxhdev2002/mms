using Abp.Application.Services.Dto;
using Abp.AspNetZeroCore.Net;
using Abp.Authorization;
using Abp.Dapper.Repositories;
using prod.Authorization;
using prod.Dto;
using prod.Inventory.IF;
using prod.Inventory.IF.Dto;
using prod.Storage;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace prod.IF.IF
{
    [AbpAuthorize(AppPermissions.Pages_Interface_IF_FQF3MM_LV2_View)]
    public class IF_FQF3MM_LV2AppService : prodAppServiceBase, IIF_FQF3MM_LV2AppService
    {
        private readonly IDapperRepository<IF_FQF3MM_LV2, long> _dapperRepo;
        private readonly ITempFileCacheManager _tempFileCacheManager;

        public IF_FQF3MM_LV2AppService(IDapperRepository<IF_FQF3MM_LV2, long> dapperRepo,
                                        ITempFileCacheManager tempFileCacheManager)
        {
            _dapperRepo = dapperRepo;
            _tempFileCacheManager = tempFileCacheManager;
        }


        public async Task<PagedResultDto<IF_FQF3MM_LV2Dto>> GetAll(GetIF_FQF3MM_LV2Input input)
        {

            string _sql = "Exec INV_INTERFACE_FQF3MM_LV2_SEARCH @p_FileDescription, @p_InterfaceDate, @p_Status";

            IEnumerable<IF_FQF3MM_LV2Dto> result = await _dapperRepo.QueryAsync<IF_FQF3MM_LV2Dto>(_sql, new
            {
                p_FileDescription = input.FileDescription,
                p_InterfaceDate = input.InterfaceDate,
                p_Status = input.Status,
            });

            var listResult = result.ToList();

            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var totalCount = result.ToList().Count();

            return new PagedResultDto<IF_FQF3MM_LV2Dto>(
                totalCount,
                pagedAndFiltered);
        }

        public async Task<PagedResultDto<IF_FQF3MM_LV2Dto>> GetFQF3MMLV2byId(int id)
        {
            Dapper.SqlMapper.Settings.CommandTimeout = 900;
            string _sql = "Exec INV_INTERFACE_FQF3MM_LV2_SEARCH_BY_ID @p_Id";

            IEnumerable<IF_FQF3MM_LV2Dto> result = await _dapperRepo.QueryAsync<IF_FQF3MM_LV2Dto>(_sql, new
            {
                p_Id = id

            });

            var listResult = result.ToList();

            var totalCount = result.ToList().Count();

            return new PagedResultDto<IF_FQF3MM_LV2Dto>(
                totalCount,
                listResult);
        }

        public async Task<FileDto> GetExportToText(int id)
        {
            Dapper.SqlMapper.Settings.CommandTimeout = 900;
            string _sql = "Exec INV_INTERFACE_FQF3MM_LV2_SEARCH_BY_ID @p_Id";

            IEnumerable<IF_FQF3MM_LV2Dto> _result = await _dapperRepo.QueryAsync<IF_FQF3MM_LV2Dto>(_sql, new
            {
                p_Id = id

            });

            var result = _result.ToList();
            var title = "Null";

            var authors1 = result[0].Content;
            if (authors1.Length > 16)
            {
                title = authors1.Substring(11, 16);
            }
            string fileName = title + ".txt";

            string pathExcel = "/Download/";
            string pathDownload = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot") + pathExcel + fileName;
            var fileDto = new FileDto(fileName, MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet);

            string[] authors = new string[] { authors1.ToString() };
            File.WriteAllLines(pathDownload, authors);
            // Read a file
            //  string readText = File.ReadAllText(fullPath);

            MemoryStream downloadStream = new MemoryStream(File.ReadAllBytes(pathDownload));
            _tempFileCacheManager.SetFile(fileDto.FileToken, downloadStream.ToArray());
            File.Delete(pathDownload);

            return await Task.FromResult(fileDto);
        }
    }
}
