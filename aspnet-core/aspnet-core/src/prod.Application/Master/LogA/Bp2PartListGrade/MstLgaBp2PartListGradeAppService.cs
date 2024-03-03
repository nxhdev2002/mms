using Abp.Application.Services.Dto;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Linq;
using Abp.Authorization;
using Abp.Collections.Extensions;
using Abp.Dapper.Repositories;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.EntityFrameworkCore.Uow;
//using DevExpress.Xpo;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using prod.Authorization;
using prod.Dto;
//using prod.EntityFrameworkCore;
using prod.Master.LogA.Bp2PartListGrade.Dto;
using prod.Master.LogA.Bp2PartListGrade.Exporting;
//using Microsoft.EntityFrameworkCore;
using prod.Master.LogA.Exporting;
using prod.EntityFrameworkCore;
using Abp.Linq.Extensions;
using prod.Master.LogA.ImportDto;
using Abp.UI;
using System;
using System.Collections.Generic;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using prod.Master.LogW.Dto;
using System.Security.Policy;
using Abp.AspNetZeroCore.Net;
using NPOI.HPSF;
using prod.Storage;

namespace prod.Master.LogA.Bp2PartListGrade
{
    //  [AbpAuthorize(AppPermissions.Pages_Master_LogA_Bp2PartListGrade)]
    public class MstLgaBp2PartListGradeAppService : prodAppServiceBase, IMstLgaBp2PartListGradeAppService
    {
        private readonly IDapperRepository<MstLgaBp2PartListGrade, long> _dapperRepo;
        private readonly IRepository<MstLgaBp2PartListGrade, long> _repo;
        private readonly IMstLgaBp2PartListGradeExcelExporter _calendarListExcelExporter;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly ITempFileCacheManager _tempFileCacheManager;

        public MstLgaBp2PartListGradeAppService(IRepository<MstLgaBp2PartListGrade, long> repo,
                                     IDapperRepository<MstLgaBp2PartListGrade, long> dapperRepo,
                                    IMstLgaBp2PartListGradeExcelExporter calendarListExcelExporter,
                                    IHostingEnvironment hostingEnvironment,
                                    ITempFileCacheManager tempFileCacheManager
        )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
            _hostingEnvironment = hostingEnvironment;
            _tempFileCacheManager = tempFileCacheManager;
        }

        //  [AbpAuthorize(AppPermissions.Pages_Master_LogA_Bp2PartListGrade_Edit)]
        public async Task CreateOrEdit(CreateOrEditMstLgaBp2PartListGradeDto input)
        {
            if (input.Id == null) await Create(input);
            else await Update(input);
        }

        //CREATE
        private async Task Create(CreateOrEditMstLgaBp2PartListGradeDto input)
        {
            var mainObj = ObjectMapper.Map<MstLgaBp2PartListGrade>(input);

            await CurrentUnitOfWork.GetDbContext<prodDbContext>().AddAsync(mainObj);
        }

        // EDIT
        private async Task Update(CreateOrEditMstLgaBp2PartListGradeDto input)
        {
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                var mainObj = await _repo.GetAll()
                .FirstOrDefaultAsync(e => e.Id == input.Id);

                var mainObjToUpdate = ObjectMapper.Map(input, mainObj);
            }
        }

        //DELETE

        //  [AbpAuthorize(AppPermissions.Pages_Master_LogA_Bp2PartListGrade_Delete)]
        public async Task Delete(EntityDto input)
        {
            var mainObj = await _repo.FirstOrDefaultAsync(input.Id);
            _repo.HardDelete(mainObj);
            CurrentUnitOfWork.GetDbContext<prodDbContext>().Remove(mainObj);
        }

        public async Task<PagedResultDto<MstLgaBp2PartListGradeDto>> GetAll(GetMstLgaBp2PartListGradeInput input)
        {
            var filtered = _repo.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.PartNo), e => e.PartNo.Contains(input.PartNo))
                .WhereIf(!string.IsNullOrWhiteSpace(input.ProdLine), e => e.ProdLine.Contains(input.ProdLine))
                .WhereIf(!string.IsNullOrWhiteSpace(input.Grade), e => e.Grade.Contains(input.Grade))
                .WhereIf(!string.IsNullOrWhiteSpace(input.Model), e => e.Model.Contains(input.Model))
                .WhereIf(!string.IsNullOrWhiteSpace(input.PartName), e => e.PartName.Contains(input.PartName));

            var pageAndFiltered = filtered.OrderBy(s => s.ProdLine).ThenBy(x => x.Model).ThenBy(x => x.Grade).ThenBy(x => x.Sorting);

            var system = from o in pageAndFiltered
                         select new MstLgaBp2PartListGradeDto
                         {
                             Id = o.Id,
                             PartNo = o.PartNo,
                             PartName = o.PartName,
                             PartListId = o.PartListId,
                             ProdLine = o.ProdLine,
                             Model = o.Model,
                             Grade = o.Grade,
                             UsageQty = o.UsageQty,
                             PikLocType = o.PikLocType,
                             PikAddress = o.PikAddress,
                             PikAddressDisplay = o.PikAddressDisplay,
                             DelLocType = o.PikLocType,
                             DelAddress = o.DelAddress,
                             DelAddressDisplay = o.DelAddressDisplay,
                             Sorting = o.Sorting,
                             Remark = o.Remark,
                             IsActive = o.IsActive,
                         };
            var totalCount = await filtered.CountAsync();
            var paged = system.PageBy(input);

            return new PagedResultDto<MstLgaBp2PartListGradeDto>(
             totalCount,
              await paged.ToListAsync()
         );
        }

        public async Task<FileDto> GetBp2PartListGradeToExcel(GetMstLgaBp2PartListGradeExportInput input)
        {
            var query = from o in _repo.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Model), e => e.Model.Contains(input.Model))
                        select new MstLgaBp2PartListGradeDto
                        {
                            Id = o.Id,
                            PartNo = o.PartNo,
                            PartName = o.PartName,
                            PartListId = o.PartListId,
                            ProdLine = o.ProdLine,
                            Model = o.Model,
                            Grade = o.Grade,
                            UsageQty = o.UsageQty,
                            PikLocType = o.PikLocType,
                            PikAddress = o.PikAddress,
                            PikAddressDisplay = o.PikAddressDisplay,
                            DelLocType = o.PikLocType,
                            DelAddress = o.DelAddress,
                            DelAddressDisplay = o.DelAddressDisplay,
                            Sorting = o.Sorting,
                            Remark = o.Remark,
                            IsActive = o.IsActive,
                        };
            var exportToExcel = await query.ToListAsync();
            return _calendarListExcelExporter.ExportToFile(exportToExcel);
        }


        public async Task<FileDto> ExportPartListGrade(string model)
        {
            string contentRootPath = "/Template/MstLgaBp2PartListGrade.xlsx";
            string webRootPath = _hostingEnvironment.WebRootPath + contentRootPath;
            string pathExcelTemp = webRootPath;
            string pathExcel = "/Download/";
            string nameExcel = "MstLgaBp2PartListGrade_" + DateTime.Now.ToString("MMddyyyy-HHmmss") + ".xlsx";
            string pathDownload = _hostingEnvironment.WebRootPath + pathExcel + nameExcel;
            var fileDto = new FileDto(nameExcel, MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet);

            FileInfo finfo = new FileInfo(pathDownload);
            if (finfo.Exists) { try { finfo.Delete(); } catch (Exception ex) { } }

            XSSFWorkbook xlsxObject = null;     //XLSX
            ISheet sheet = null;
            IRow row;
            IRow rowTitle;
            IRow rowName;
            IRow rowModel;
            ICell cell;

            // Lấy Object Excel (giữa xls và xlsx)
            using (FileStream file = new FileStream(pathExcelTemp, FileMode.Open, FileAccess.Read))
            {
                xlsxObject = new XSSFWorkbook(file);
            }

            // Lấy Object Sheet  
            sheet = xlsxObject.GetSheetAt(0);
            if (sheet == null) { return null; }

            ICellStyle istyle = xlsxObject.CreateCellStyle();
            istyle.FillPattern = FillPattern.SolidForeground;
            istyle.FillForegroundColor = IndexedColors.White.Index;
            istyle.BorderBottom = BorderStyle.Thin;
            istyle.BorderTop = BorderStyle.Thin;
            istyle.BorderLeft = BorderStyle.Thin;
            istyle.BorderRight = BorderStyle.Thin;



            int rowIndex = 3; //starting row to fill in     

            try
            {
                string _sqlTitle = "Exec MST_LGA_BP2_PART_LIST_GRADE_EXPORT_TITLE @model";
                var data1 = await Task.Run(() => _dapperRepo.QueryAsync<MstLgaBp2PartListGradeDto>(_sqlTitle, new { @model = model })) ;
                var lstTitle = from o in data1
                                     select new MstLgaBp2PartListGradeDto
                                     {
                                         Id = o.Id,
                                         PartNo = o.PartNo,
                                         PartName = o.PartName,
                                         PartListId = o.PartListId,
                                         ProdLine = o.ProdLine,
                                         Model = o.Model,
                                         Grade = o.Grade,
                                         UsageQty = o.UsageQty,
                                         PikLocType = o.PikLocType,
                                         PikAddress = o.PikAddress,
                                         PikAddressDisplay = o.PikAddressDisplay,
                                         DelLocType = o.PikLocType,
                                         DelAddress = o.DelAddress,
                                         DelAddressDisplay = o.DelAddressDisplay,
                                         Sorting = o.Sorting,
                                         Remark = o.Remark,
                                         IsActive = o.IsActive
                                     };

                string _sqlContent = "Exec MST_LGA_BP2_PART_LIST_GRADE_EXPORT_CONTENT @model";
                var data2 = await Task.Run(() => _dapperRepo.QueryAsync<MstLgaBp2PartListGradeDto>(_sqlContent, new { @model = model }));
                var lstContend = from o in data2
                               select new MstLgaBp2PartListGradeDto
                               {
                                   Id = o.Id,
                                   PartNo = o.PartNo,
                                   PartName = o.PartName,
                                   PartListId = o.PartListId,
                                   ProdLine = o.ProdLine,
                                   Model = o.Model,
                                   Grade = o.Grade,
                                   UsageQty = o.UsageQty,
                                   PikLocType = o.PikLocType,
                                   PikAddress = o.PikAddress,
                                   PikAddressDisplay = o.PikAddressDisplay,
                                   DelLocType = o.PikLocType,
                                   DelAddress = o.DelAddress,
                                   DelAddressDisplay = o.DelAddressDisplay,
                                   Sorting = o.Sorting,
                                   Remark = o.Remark,
                                   IsActive = o.IsActive,
                                   A1 = o.A1,
                                   A2 = o.A2,
                                   A3 = o.A3,
                                   A4 = o.A4,
                                   A5 = o.A5,
                                   A6 = o.A6,
                                   A7 = o.A7,
                                   A8 = o.A8,
                                   A9 = o.A9,
                                   A10 = o.A10,
                               };


                 MstLgaBp2PartListGradeDto[] arrFieldData = lstContend.ToArray();

                MstLgaBp2PartListGradeDto[] arrFieldTitle = lstTitle.ToArray();

                //Create Key pairs of Header and column index
                var listPairsHeader = new List<KeyValuePair<int, string>>();

                row = sheet.GetRow(2);
                rowName = sheet.GetRow(0);
                rowModel = sheet.GetRow(1);

                string v_model = ((arrFieldData[0].Model == "V") ? "Vios" : (arrFieldData[0].Model == "I" ? "Innova" : (arrFieldData[0].Model == "F" ? "Fortuner" : (arrFieldData[0].Model == "A" ? "Avanza" : ""))));

                //set name 
                cell = rowName.CreateCell(1, CellType.String);
                cell.CellStyle = istyle;
                cell.SetCellValue("List of  " + v_model + " Big part 2x2");

                //set Model 
                cell = rowModel.CreateCell(4, CellType.String);
                cell.CellStyle = istyle;
                cell.SetCellValue(v_model);

                for (int i = 0; i < arrFieldTitle.Length; i++)
                {
                    int j = 3;
                    cell = row.CreateCell(j + i + 1, CellType.String); cell.CellStyle = istyle;
                    var sFieldObj = lstTitle.Where(f => f.Grade == arrFieldTitle[i].Grade).FirstOrDefault();
                    if (sFieldObj != null)
                    {
                        cell.SetCellValue(sFieldObj.Grade ?? "");
                    }
                    else cell.SetCellValue(arrFieldTitle[i].Grade);
                }


                int IA = CellReference.ConvertColStringToIndex("A");
                int IB = CellReference.ConvertColStringToIndex("B");
                int IC = CellReference.ConvertColStringToIndex("C");
                int ID = CellReference.ConvertColStringToIndex("D");
                int IE = CellReference.ConvertColStringToIndex("E");
                int IF = CellReference.ConvertColStringToIndex("F");
                int IG = CellReference.ConvertColStringToIndex("G");
                int IH = CellReference.ConvertColStringToIndex("H");
                int II = CellReference.ConvertColStringToIndex("I");
                int IJ = CellReference.ConvertColStringToIndex("J");
                int IK = CellReference.ConvertColStringToIndex("K");
                int IL = CellReference.ConvertColStringToIndex("L");
                int IM = CellReference.ConvertColStringToIndex("M");
                int IN = CellReference.ConvertColStringToIndex("N");
                int IO = CellReference.ConvertColStringToIndex("O");
                int IP = CellReference.ConvertColStringToIndex("P");
                int IQ = CellReference.ConvertColStringToIndex("Q");
                int IR = CellReference.ConvertColStringToIndex("R");
                int IS = CellReference.ConvertColStringToIndex("S");
           

                int No_Number = 1;
                foreach (MstLgaBp2PartListGradeDto lp in lstContend)
                {

                    row = sheet.GetRow(rowIndex);
                    if (row == null)
                    {
                        row = sheet.CreateRow(rowIndex);
                    }

                    cell = row.CreateCell(IA, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(No_Number);
                    cell = row.CreateCell(IB, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(lp.PartNo);
                    cell = row.CreateCell(IC, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(lp.PartName);
                    cell = row.CreateCell(ID, CellType.String); cell.CellStyle = istyle; cell.SetCellValue("");
                    cell = row.CreateCell(IO, CellType.String); cell.CellStyle = istyle; cell.SetCellValue("");
                    cell = row.CreateCell(IP, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(lp.PikLocType);
                    cell = row.CreateCell(IQ, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(lp.PikAddress);
                    cell = row.CreateCell(IR, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(lp.DelAddress);
                    cell = row.CreateCell(IS, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(lp.Remark);
                   


                    //NEW
                    if (arrFieldTitle.Length == 1)
                    {
                        cell = row.CreateCell(IE, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(lp.A1);
                    }
                    else if (arrFieldTitle.Length == 2)
                    {
                        cell = row.CreateCell(IE, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(lp.A1);
                        cell = row.CreateCell(IF, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(lp.A2);
                    }
                    else if (arrFieldTitle.Length == 3)
                    {
                        cell = row.CreateCell(IE, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(lp.A1);
                        cell = row.CreateCell(IF, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(lp.A2);
                        cell = row.CreateCell(IG, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(lp.A3);
                    }
                    else if (arrFieldTitle.Length == 4)
                    {
                        cell = row.CreateCell(IE, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(lp.A1);
                        cell = row.CreateCell(IF, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(lp.A2);
                        cell = row.CreateCell(IG, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(lp.A3);
                        cell = row.CreateCell(IH, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(lp.A4);
                    }
                    else if (arrFieldTitle.Length == 5)
                    {

                        cell = row.CreateCell(IE, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(lp.A1);
                        cell = row.CreateCell(IF, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(lp.A2);
                        cell = row.CreateCell(IG, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(lp.A3);
                        cell = row.CreateCell(IH, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(lp.A4);
                        cell = row.CreateCell(II, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(lp.A5);
                    }
                    else if (arrFieldTitle.Length == 6)
                    {
                        cell = row.CreateCell(IE, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(lp.A1);
                        cell = row.CreateCell(IF, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(lp.A2);
                        cell = row.CreateCell(IG, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(lp.A3);
                        cell = row.CreateCell(IH, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(lp.A4);
                        cell = row.CreateCell(II, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(lp.A5);
                        cell = row.CreateCell(IJ, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(lp.A6);
                    }
                    else if (arrFieldTitle.Length == 7)
                    {
                        cell = row.CreateCell(IE, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(lp.A1);
                        cell = row.CreateCell(IF, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(lp.A2);
                        cell = row.CreateCell(IG, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(lp.A3);
                        cell = row.CreateCell(IH, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(lp.A4);
                        cell = row.CreateCell(II, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(lp.A5);
                        cell = row.CreateCell(IJ, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(lp.A6);
                        cell = row.CreateCell(IK, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(lp.A7);
                    }
                    else if (arrFieldTitle.Length == 8)
                    {
                        cell = row.CreateCell(IE, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(lp.A1);
                        cell = row.CreateCell(IF, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(lp.A2);
                        cell = row.CreateCell(IG, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(lp.A3);
                        cell = row.CreateCell(IH, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(lp.A4);
                        cell = row.CreateCell(II, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(lp.A5);
                        cell = row.CreateCell(IJ, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(lp.A6);
                        cell = row.CreateCell(IK, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(lp.A7);
                        cell = row.CreateCell(IL, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(lp.A8);
                    }
                    else if (arrFieldTitle.Length == 9)
                    {
                        cell = row.CreateCell(IE, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(lp.A1);
                        cell = row.CreateCell(IF, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(lp.A2);
                        cell = row.CreateCell(IG, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(lp.A3);
                        cell = row.CreateCell(IH, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(lp.A4);
                        cell = row.CreateCell(II, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(lp.A5);
                        cell = row.CreateCell(IJ, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(lp.A6);
                        cell = row.CreateCell(IK, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(lp.A7);
                        cell = row.CreateCell(IL, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(lp.A8);
                        cell = row.CreateCell(IM, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(lp.A9);

                    }
                    else if (arrFieldTitle.Length == 10)
                    {
                        cell = row.CreateCell(IE, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(lp.A1);
                        cell = row.CreateCell(IF, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(lp.A2);
                        cell = row.CreateCell(IG, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(lp.A3);
                        cell = row.CreateCell(IH, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(lp.A4);
                        cell = row.CreateCell(II, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(lp.A5);
                        cell = row.CreateCell(IJ, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(lp.A6);
                        cell = row.CreateCell(IK, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(lp.A7);
                        cell = row.CreateCell(IL, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(lp.A8);
                        cell = row.CreateCell(IM, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(lp.A9);
                        cell = row.CreateCell(IN, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(lp.A10);
                    }

                    rowIndex++;
                    No_Number++;
                }

                FileStream xfile = new FileStream(pathDownload, FileMode.Create, System.IO.FileAccess.Write);
                xlsxObject.Write(xfile);
                xfile.Dispose();

                MemoryStream downloadStream = new MemoryStream(File.ReadAllBytes(pathDownload));
                _tempFileCacheManager.SetFile(fileDto.FileToken, downloadStream.ToArray());
                File.Delete(pathDownload);
                downloadStream.Position = 0;
            }
            catch (Exception ex){}

            return await Task.FromResult(fileDto);
        }






            //Import Data From Excel
            #region Import Data From Excel
            public async Task<List<ImportMstLgaBp2PartListGradeDto>> ImportMstLgaBp2PartListGradeFromExcel(List<ImportMstLgaBp2PartListGradeDto> LotUpPlans)
        {
            try
            {
                List<MstLgaBp2PartListGrade_T> PartListGrade = new List<MstLgaBp2PartListGrade_T> { };
                foreach (var item in LotUpPlans)
                {
                    MstLgaBp2PartListGrade_T importData = new MstLgaBp2PartListGrade_T();
                    {

                        importData.Guid = item.Guid;
                        importData.PartListId = 0;
                        importData.PartNo = item.PartNo;
                        importData.PartName = item.PartName;
                        importData.PikLocType = item.PikLocType;
                        importData.PikAddress = item.PikAddress;
                        importData.DelAddress = item.DelAddress;
                        importData.Remark = item.Remark;
                        importData.Model = item.Model;

                        importData.Grade = item.Grade;
                        importData.UsageQty = item.UsageQty;
                        importData.Sorting = item.Sorting;


                    }
                    PartListGrade.Add(importData);
                }
                CurrentUnitOfWork.GetDbContext<prodDbContext>().AddRangeAsync(PartListGrade);
                return null;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(400, ex.Message);
            }
        }


        //Merge Data From Temp To MstLgaBp2PartListGrade
        public async Task MergeMstLgaBp2PartListGrade(string v_Guid)
        {
            string _sql = "Exec MST_LGA_BP2_PARTLISTGRADE_MERGE @Guid";
            await _dapperRepo.QueryAsync<MstLgaBp2PartListGrade>(_sql, new { Guid = v_Guid });
        }

        #endregion

    }

}
