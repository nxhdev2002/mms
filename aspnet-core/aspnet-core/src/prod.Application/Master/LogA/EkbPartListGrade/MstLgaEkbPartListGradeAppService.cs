using Abp.Application.Services.Dto;
using Abp.AspNetZeroCore.Net;
using Abp.Authorization;
using Abp.Dapper.Repositories;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.EntityFrameworkCore.Uow;
using Abp.Linq.Extensions;
using Abp.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using prod.Authorization;
using prod.Dto;
using prod.EntityFrameworkCore;
using prod.LogA.Pcs;
using prod.LogA.Pcs.Stock.Dto;
using prod.LogA.Sps;
using prod.LogA.Sps.Stock.Dto;
using prod.Master.LogA.Dto;
using prod.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace prod.Master.LogA
{
    //  [AbpAuthorize(AppPermissions.Pages_Master_LogA_EkbPartListGrade)]
    public class MstLgaEkbPartListGradeAppService : prodAppServiceBase, IMstLgaEkbPartListGradeAppService
    {
        private readonly IDapperRepository<MstLgaEkbPartListGrade, long> _dapperRepo;
        private readonly IRepository<MstLgaEkbPartListGrade, long> _repo;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly ITempFileCacheManager _tempFileCacheManager;

        public MstLgaEkbPartListGradeAppService(IRepository<MstLgaEkbPartListGrade, long> repo,
                                         IDapperRepository<MstLgaEkbPartListGrade, long> dapperRepo,
                                         IHostingEnvironment hostingEnvironment,
                                         ITempFileCacheManager tempFileCacheManager
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _hostingEnvironment = hostingEnvironment;
            _tempFileCacheManager = tempFileCacheManager;

        }

        //  [AbpAuthorize(AppPermissions.Pages_Master_LogA_EkbPartListGrade_Edit)]
        public async Task CreateOrEdit(CreateOrEditMstLgaEkbPartListGradeDto input)
        {
            if (input.Id == null) await Create(input);
            else await Update(input);
        }

        //CREATE
        private async Task Create(CreateOrEditMstLgaEkbPartListGradeDto input)
        {
            var mainObj = ObjectMapper.Map<MstLgaEkbPartListGrade>(input);

            await CurrentUnitOfWork.GetDbContext<prodDbContext>().AddAsync(mainObj);
        }

        // EDIT
        private async Task Update(CreateOrEditMstLgaEkbPartListGradeDto input)
        {
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                var mainObj = await _repo.GetAll()
                .FirstOrDefaultAsync(e => e.Id == input.Id);

                var mainObjToUpdate = ObjectMapper.Map(input, mainObj);
            }
        }

        //  [AbpAuthorize(AppPermissions.Pages_Master_LogA_EkbPartListGrade_Delete)]
        public async Task Delete(EntityDto input)
        {
            var mainObj = await _repo.FirstOrDefaultAsync(input.Id);
            _repo.HardDelete(mainObj);
            CurrentUnitOfWork.GetDbContext<prodDbContext>().Remove(mainObj);
        }

        public async Task<PagedResultDto<MstLgaEkbPartListGradeDto>> GetAll(GetMstLgaEkbPartListGradeInput input)
        {
            var filtered = _repo.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.PartNo), e => e.PartNo.Contains(input.PartNo))
                .WhereIf(!string.IsNullOrWhiteSpace(input.BackNo), e => e.BackNo.Contains(input.BackNo))
                .WhereIf(!string.IsNullOrWhiteSpace(input.ProdLine), e => e.ProdLine.Contains(input.ProdLine))
                .WhereIf(!string.IsNullOrWhiteSpace(input.SupplierNo), e => e.SupplierNo.Contains(input.SupplierNo))
                .WhereIf(!string.IsNullOrWhiteSpace(input.Model), e => e.Model.Contains(input.Model))
                .WhereIf(!string.IsNullOrWhiteSpace(input.ProcessCode), e => e.ProcessCode.Contains(input.ProcessCode));

            var pageAndFiltered = filtered.OrderBy(s => s.Id);

            var system = from o in pageAndFiltered
                         select new MstLgaEkbPartListGradeDto
                         {
                             Id = o.Id,
                             PartNo = o.PartNo,
                             PartNoNormanlized = o.PartNoNormanlized,
                             PartName = o.PartName,
                             BackNo = o.BackNo,
                             PartListId = o.PartListId,
                             ProdLine = o.ProdLine,
                             SupplierNo = o.SupplierNo,
                             Model = o.Model,
                             ProcessId = o.ProcessId,
                             ProcessCode = o.ProcessCode,
                             Grade = o.Grade,
                             UsageQty = o.UsageQty,
                             BoxQty = o.BoxQty,
                             Module = o.Module,
                             PcAddress = o.PcAddress,
                             PcSorting = o.PcSorting,
                             SpsAddress = o.SpsAddress,
                             SpsSorting = o.SpsSorting,
                             Remark = o.Remark,
                             IsActive = o.IsActive,
                         };

            var totalCount = await filtered.CountAsync();
            var paged = system.PageBy(input);

            return new PagedResultDto<MstLgaEkbPartListGradeDto>(
                totalCount,
                 await paged.ToListAsync()
            );
        }

        public async Task<FileDto> ExportEkbPartListGrade(string model)
        {
            string nameModel = ((model == "V") ? "Vios" : (model == "I" ? "Innova" : (model == "F" ? "Fortuner" : (model == "A" ? "Avanza" : ""))));
            string contentRootPath = "/Template/MstLgaEkbPartListGrade.xlsx";
            string webRootPath = _hostingEnvironment.WebRootPath + contentRootPath;
            string pathExcelTemp = webRootPath;
            string pathExcel = "/Download/";
            string nameExcel = "MstLgaEkbPartListGrade_" + nameModel + "_" + DateTime.Now.ToString("MMddyyyy-HHmmss") + ".xlsx";
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
                string _sqlTitle = "Exec MST_LGA_EKB_PART_LIST_GRADE_EXPORT_TITLE @model";
                var data1 = await Task.Run(() => _dapperRepo.QueryAsync<MstLgaEkbPartListGradeDto>(_sqlTitle, new { @model = model }));
                var lstTitle = from o in data1
                               select new MstLgaEkbPartListGradeDto
                               {
                                   Id = o.Id,
                                   PartNo = o.PartNo,
                                   PartName = o.PartName,
                                   PartListId = o.PartListId,
                                   BackNo = o.BackNo,
                                   ProdLine = o.ProdLine,
                                   Model = o.Model,
                                   Grade = o.Grade,
                                   UsageQty = o.UsageQty,
                                   Module = o.Module,
                                   BoxQty = o.BoxQty,
                                   PcAddress = o.PcAddress,
                                   PcSorting = o.PcSorting,
                                   SpsAddress = o.SpsAddress,
                                   SpsSorting = o.SpsSorting,
                                   ProcessCode = o.ProcessCode,
                                   Remark = o.Remark,
                                   IsActive = o.IsActive
                               };

                string _sqlContent = "Exec MST_LGA_EKB_PART_LIST_GRADE_EXPORT_CONTENT @model";
                var data2 = await Task.Run(() => _dapperRepo.QueryAsync<MstLgaEkbPartListGradeDto>(_sqlContent, new { @model = model }));
                var lstContend = from o in data2
                                 select new MstLgaEkbPartListGradeDto
                                 {
                                     Id = o.Id,
                                     PartNo = o.PartNo,
                                     PartName = o.PartName,
                                     PartListId = o.PartListId,
                                     BackNo = o.BackNo,
                                     ProdLine = o.ProdLine,
                                     Model = o.Model,
                                     Grade = o.Grade,
                                     UsageQty = o.UsageQty,
                                     Module = o.Module,
                                     BoxQty = o.BoxQty,
                                     PcAddress = o.PcAddress,
                                     PcSorting = o.PcSorting,
                                     SpsAddress = o.SpsAddress,
                                     SpsSorting = o.SpsSorting,
                                     ProcessCode = o.ProcessCode,
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


                MstLgaEkbPartListGradeDto[] arrFieldData = lstContend.ToArray();

                MstLgaEkbPartListGradeDto[] arrFieldTitle = lstTitle.ToArray();

                //Create Key pairs of Header and column index
                var listPairsHeader = new List<KeyValuePair<int, string>>();

                row = sheet.GetRow(2);
                rowName = sheet.GetRow(0);
                rowModel = sheet.GetRow(1);
                if (arrFieldData.Length > 0)
                {
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
                    int IT = CellReference.ConvertColStringToIndex("T");
                    int IU = CellReference.ConvertColStringToIndex("U");
                    int IV = CellReference.ConvertColStringToIndex("V");

                    int No_Number = 1;
                    foreach (MstLgaEkbPartListGradeDto lp in lstContend)
                    {

                        row = sheet.GetRow(rowIndex);
                        if (row == null)
                        {
                            row = sheet.CreateRow(rowIndex);
                        }

                        cell = row.CreateCell(IA, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(No_Number);
                        cell = row.CreateCell(IB, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(lp.PartNo);
                        cell = row.CreateCell(IC, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(lp.PartName);
                        cell = row.CreateCell(ID, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(lp.BackNo);
                        cell = row.CreateCell(IO, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(lp.Module);
                        cell = row.CreateCell(IP, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(lp.BoxQty.ToString());
                        cell = row.CreateCell(IQ, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(lp.PcAddress);
                        cell = row.CreateCell(IR, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(lp.PcSorting.ToString());
                        cell = row.CreateCell(IS, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(lp.SpsAddress);
                        cell = row.CreateCell(IT, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(lp.SpsSorting.ToString());
                        cell = row.CreateCell(IU, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(lp.ProcessCode);
                        cell = row.CreateCell(IV, CellType.String); cell.CellStyle = istyle; cell.SetCellValue(lp.Remark);



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

                }
                FileStream xfile = new FileStream(pathDownload, FileMode.Create, System.IO.FileAccess.Write);
                xlsxObject.Write(xfile);
                xfile.Dispose();

                MemoryStream downloadStream = new MemoryStream(File.ReadAllBytes(pathDownload));
                _tempFileCacheManager.SetFile(fileDto.FileToken, downloadStream.ToArray());
                File.Delete(pathDownload);
                downloadStream.Position = 0;

            }
            catch (Exception ex) { }

            return await Task.FromResult(fileDto);
        }


        //Import Data From Excel
        #region Import Data From Excel
        public async Task<List<MstLgaEkbPartListGradeImportDto>> ImportMstLgaEkbPartListGradeFromExcel(List<MstLgaEkbPartListGradeImportDto> ekbPartListGrades)
        {
            try
            {
                List<MstLgaEkbPartListGrade_T> ekbPartListGrade = new List<MstLgaEkbPartListGrade_T> { };
                foreach (var item in ekbPartListGrades)
                {
                    MstLgaEkbPartListGrade_T importData = new MstLgaEkbPartListGrade_T();
                    {
                        importData.Guid = item.Guid;
                        importData.PartNo = item.PartNo;
                        importData.PartName = item.PartName;
                        importData.SupplierNo = item.SupplierNo;
                        importData.BackNo = item.BackNo;
                        importData.Cfc = item.Cfc;
                        importData.BodyColor = item.BodyColor;
                        importData.Module = item.Module;
                        importData.BoxQty = item.BoxQty;
                        importData.ExporterBackNo = item.ExporterBackNo;
                        importData.PcAddress = item.PcAddress;
                        importData.PcSorting = item.PcSorting;
                        importData.SpsAddress = item.SpsAddress;
                        importData.SpsSorting = item.SpsSorting;
                        importData.ProcessCode = item.ProcessCode;
                        importData.Remark = item.Remark;
                        importData.Model = item.Model;
                        importData.Grade = item.Grade;
                        importData.UsageQty = item.UsageQty;
                    }
                    ekbPartListGrade.Add(importData);
                }
                CurrentUnitOfWork.GetDbContext<prodDbContext>().AddRangeAsync(ekbPartListGrade);
                return null;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(400, ex.Message);
            }
        }

        public async Task<List<LgaPcsStockImportDto>> ImportLgaPcsStockFromExcel(List<LgaPcsStockImportDto> pcsStocks)
        {
            try
            {
                List<LgaPcsStock_T> pcsStockT = new List<LgaPcsStock_T> { };
                foreach (var item in pcsStocks)
                {
                    LgaPcsStock_T importData = new LgaPcsStock_T();
                    {
                        importData.Guid = item.Guid;
                        importData.PartNo = item.PartNo;
                        importData.PartName = item.PartName;
                        importData.SupplierNo = item.SupplierNo;
                        importData.BackNo = item.BackNo;
                        importData.PcRackAddress = item.PcRackAddress;
                        importData.UsagePerHour = item.UsagePerHour;
                        importData.RackCapBox = item.RackCapBox;
                        importData.OutType = item.OutType;
                        importData.StockQty = item.StockQty;
                        importData.BoxQty = item.BoxQty;
                    }
                    pcsStockT.Add(importData);
                }
                CurrentUnitOfWork.GetDbContext<prodDbContext>().AddRangeAsync(pcsStockT);
                return null;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(400, ex.Message);
            }
        }

        public async Task<List<LgaSpsStockImportDto>> ImportLgaSpsStockFromExcel(List<LgaSpsStockImportDto> spsStocks)
        {
            try
            {
                List<LgaSpsStock_T> spsStockT = new List<LgaSpsStock_T> { };
                foreach (var item in spsStocks)
                {
                    LgaSpsStock_T importData = new LgaSpsStock_T();
                    {
                        importData.Guid = item.Guid;
                        importData.PartNo = item.PartNo;
                        importData.PartName = item.PartName;
                        importData.SupplierNo = item.SupplierNo;
                        importData.BackNo = item.BackNo;
                        importData.SpsRackAddress = item.SpsRackAddress;
                        importData.PcRackAddress = item.PcRackAddress;
                        importData.RackCapBox = item.RackCapBox;
                        importData.PcPicKingMember = item.PcPicKingMember;
                        importData.EkbQty = item.EkbQty;
                        importData.StockQty = item.StockQty;
                        importData.BoxQty = item.BoxQty;
                        importData.Process = item.Process;
                    }
                    spsStockT.Add(importData);
                }
                CurrentUnitOfWork.GetDbContext<prodDbContext>().AddRangeAsync(spsStockT);
                return null;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(400, ex.Message);
            }
        }






        //Merge Data From Temp To MstLgaEkbPartListGrade
        public async Task MergeMstLgaEkbPartListGrade(string v_Guid)
        {
            string _sql = "Exec MST_LGA_EKB_PARTLISTGRADE_MERGE @Guid";
            await _dapperRepo.QueryAsync<MstLgaEkbPartListGrade>(_sql, new { Guid = v_Guid });
        }

        #endregion

    }
}
