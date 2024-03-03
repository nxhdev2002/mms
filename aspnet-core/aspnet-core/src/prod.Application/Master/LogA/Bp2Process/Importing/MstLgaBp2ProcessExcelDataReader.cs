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
using prod.Master.LogA.Bp2Process.ImportDto;

namespace prod.Master.LogA.Bp2Process.Importing
{
    public class MstLgaBp2ProcessExcelDataReader : NpoiExcelImporterBase<ImportMstLgaBp2ProcessDto>, IMstLgaBp2ProcessExcelDataReader
    {
        private readonly IMstLgaBp2ProcessAppService _MstLgaBp2Process;
        public MstLgaBp2ProcessExcelDataReader(IMstLgaBp2ProcessAppService MstLgaBp2Process,
            IDapperRepository<MstLgaBp2Process, long> dapperRepo)
        {
            _MstLgaBp2Process = MstLgaBp2Process;
        }

        public async Task<List<ImportMstLgaBp2ProcessDto>> GetMstLgaBp2ProcessFromExcel(byte[] fileBytes, string fileName)
        {
            try
            {
                List<ImportMstLgaBp2ProcessDto> rowList = new List<ImportMstLgaBp2ProcessDto>();
                var importData = new ImportMstLgaBp2ProcessDto();
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
                    IRow row = sheet.GetRow(3);
                    IRow rowWkd = sheet.GetRow(0);
                    string strGUID = Guid.NewGuid().ToString("N");

                    DataFormatter formatter = new DataFormatter();
                    for (int i = startRow; i <= endRow; i++)
                    {
                        importData = new ImportMstLgaBp2ProcessDto();
                        row = sheet.GetRow(i);
                        var v_Code = Convert.ToString(row.GetCell(1).ToString());
                        if (v_Code != null)
                        {
                            try
                            {
                                importData.Guid = strGUID;
                                importData.Code = v_Code;
                                importData.ProdLine = Convert.ToString(row.GetCell(2).ToString());
                                importData.Sorting = Convert.ToInt32(row.GetCell(3).ToString());
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
                    _MstLgaBp2Process.ImportMstLgaBp2ProcessFromExcel(rowList);
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
