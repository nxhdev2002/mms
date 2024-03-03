using Abp.Dapper.Repositories;
using Abp.UI;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using prod.DataExporting.Excel.NPOI;
using prod.Master.LogA;
using prod.Master.LogA.Bp2PartList.ImportDto;

namespace prod.Master.LogW.Bp2PartList.Import
{
    public class MstLgaBp2PartListExcelDataReader : NpoiExcelImporterBase<ImportMstLgaBp2PartListDto>, IMstLgaBp2PartListExcelDataReader
    {
        private readonly IMstLgaBp2PartListAppService _lgaBp2PartList;
        private readonly IDapperRepository<MstLgaBp2PartList, long> _dapperRepo;

        public MstLgaBp2PartListExcelDataReader(IMstLgaBp2PartListAppService lgaBp2PartList,
                                        IDapperRepository<MstLgaBp2PartList, long> dapperRepo)
        {
            _lgaBp2PartList = lgaBp2PartList;
            _dapperRepo = dapperRepo;
        }

        public async Task<List<ImportMstLgaBp2PartListDto>> GetBp2PartListFromExcel(byte[] fileBytes, string fileName)
        {
            try
            {
                List<ImportMstLgaBp2PartListDto> rowList = new List<ImportMstLgaBp2PartListDto>();
                var importData = new ImportMstLgaBp2PartListDto();
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
                    int startRow = 3;
                    int endRow = sheet.LastRowNum;
                    IRow row = sheet.GetRow(0);
                    string strGUID = Guid.NewGuid().ToString("N");

                    DataFormatter formatter = new DataFormatter();
                    for (int i = startRow; i <= endRow; i++)
                    {
                        importData = new ImportMstLgaBp2PartListDto();
                        row = sheet.GetRow(i);
                        var v_ProdLine = Convert.ToString(row.GetCell(0).StringCellValue);
                        if (v_ProdLine != null)
                        {
                            try
                            {
                                importData.Guid = strGUID;
                                importData.ProdLine = v_ProdLine;
                                importData.PartName = Convert.ToString(row.GetCell(2).StringCellValue);
                                importData.ShortName = Convert.ToString(row.GetCell(3).StringCellValue);
                                importData.PikProcess = Convert.ToString(row.GetCell(4).StringCellValue);
                                importData.PikSorting = Convert.ToInt32(row.GetCell(5).NumericCellValue);
                                importData.DelProcess = Convert.ToString(row.GetCell(6).StringCellValue);
                                importData.DelSorting = Convert.ToInt32(row.GetCell(7).NumericCellValue);
                                importData.Remark = Convert.ToString(row.GetCell(8).StringCellValue);
                            }
                            catch (Exception ex)
                            {
                                importData.Exception += ex.ToString();
                            }
                            rowList.Add(importData);
                        }
                    }
                }
                // return rowList;
                if (rowList.Count() > 0)
                {
                    _lgaBp2PartList.ImportBp2PartListFromExcel(rowList);
                }

                return rowList;
            }
            catch (Exception ex)
            {

                throw new UserFriendlyException(ex.ToString());
                // return ex;
            }
        }
    }
}
