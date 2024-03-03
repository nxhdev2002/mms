using Abp.Dapper.Repositories;
using Abp.UI;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using prod.DataExporting.Excel.NPOI;
using prod.Master.LogA;
using prod.Master.LogA.Dto;
using prod.Master.LogA.EkbPartList;
using prod.Welding.Andon.ImportDto;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace prod.Master.LogA.Importing
{
    public class MstLgaEkbPartListExcelDataReader : NpoiExcelImporterBase<ImportWldAdoWeldingPlanDto>, IMstLgaEkbPartListExcelDataReader
    {

        private readonly IMstLgaEkbPartListAppService _mstLgaEkbPartList;
        private readonly IDapperRepository<MstLgaEkbPartList, long> _dapperRepo;

        public MstLgaEkbPartListExcelDataReader(IMstLgaEkbPartListAppService mstLgaEkbPartList,
                                        IDapperRepository<MstLgaEkbPartList, long> dapperRepo)
        {
            _mstLgaEkbPartList = mstLgaEkbPartList;
            _dapperRepo = dapperRepo;
        }
        public async Task<List<MstLgaEkbPartListImportDto>> GetMstLgaEkbPartListFromExcel(byte[] fileBytes, string fileName)
        {
            try
            {
                List<MstLgaEkbPartListImportDto> rowList = new List<MstLgaEkbPartListImportDto>();
                var importData = new MstLgaEkbPartListImportDto();
                ISheet sheet;

                using (var stream = new MemoryStream(fileBytes))
                {
                    stream.Position = 0;
                    if (fileName.EndsWith("xlsx"))
                    {
                        XSSFWorkbook xlsxObject = new XSSFWorkbook(stream);
                        sheet = xlsxObject.GetSheetAt(0); //get first sheet from workbook 
                    }
                    else
                    {
                        HSSFWorkbook hssfworkbook = new HSSFWorkbook(stream);
                        sheet = hssfworkbook.GetSheetAt(0); //get first sheet from workbook 
                    }


                    //Read Data
                    int startRow = 4;
                    int endRow = sheet.LastRowNum;
                    IRow row = sheet.GetRow(4);
                    IRow rowWkd = sheet.GetRow(1);
                    DateTime? _workingDate = DateTime.FromOADate((rowWkd.GetCell(2).NumericCellValue));
                    string strGUID = Guid.NewGuid().ToString("N");

                    DataFormatter formatter = new DataFormatter();
                    for (int i = startRow; i <= endRow; i++)
                    {
                        try
                        {
                            importData = new MstLgaEkbPartListImportDto();
                            row = sheet.GetRow(i);
                            var v_ProdLine = Convert.ToString(row.GetCell(2).StringCellValue);
                            if (v_ProdLine != null && v_ProdLine != "")
                            {
                                TimeSpan _unpackingStartTime = DateTime.FromOADate((row.GetCell(5).NumericCellValue)).TimeOfDay;
                                TimeSpan _unpackingFinishTime = DateTime.FromOADate((row.GetCell(6).NumericCellValue)).TimeOfDay;
                                var v_lot_no = Convert.ToString(row.GetCell(3).StringCellValue);
                                var v_shift = Convert.ToString(row.GetCell(4).NumericCellValue);


                                //importData.Guid = strGUID;
                                //importData.WorkingDate = _workingDate;
                                //importData.ProdLine = v_ProdLine;
                                //importData.LotNo = v_lot_no;
                                //importData.Shift = v_shift;
                                //importData.UnpackingStartDatetime = Convert.ToDateTime(_unpackingStartTime.ToString());
                                //importData.UnpackingFinishDatetime = Convert.ToDateTime(_unpackingFinishTime.ToString());
                                //importData.Tpm = Convert.ToString(row.GetCell(7).StringCellValue);
                                //importData.Remarks = Convert.ToString(row.GetCell(8).StringCellValue);

                                //int check = rowList.Where(t => t.LotNo == v_lot_no && t.Shift == v_shift && t.ProdLine == v_ProdLine && t.WorkingDate == _workingDate).Count();
                                //if (check == 0)
                                //{
                                //    rowList.Add(importData);
                                //}
                                //else
                                //{
                                //    importData.Exception = (i - 3).ToString();
                                //    rowList.Add(importData);
                                //    return rowList;

                                //}
                            }
                            else
                            {
                                break;
                            }

                        }
                        catch (Exception ex)
                        {
                            importData.Exception += ex.ToString();
                        }
                    }
                    // return rowList;
                    //if (rowList.Count() > 0)
                    //{
                    //    _lgwLotUpPlan.ImportPxPUpPlanFromExcel(rowList);
                    //}

                    return rowList;
                }
            }
            catch (Exception ex)
            {

                throw new UserFriendlyException(ex.ToString());
                // return ex;
            }
        }
    }
}
