using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using prod.DataExporting.Excel.NPOI;
using prod.Master.LogA.Bp2Process.ImportDto;
using prod.Master.LogA.Bp2Process;
using Abp.Dapper.Repositories;
using prod.Master.LogA.ImportDto;
using NPOI.SS.UserModel;
using System.IO;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using Abp.UI;
using prod.Master.LogA.Bp2PartListGrade;
using NPOI.POIFS.Properties;

namespace prod.Master.LogA.Importing
{
    public class MstLgaBp2PartListGradeExcelDataReader : NpoiExcelImporterBase<ImportMstLgaBp2ProcessDto>, IMstLgaBp2PartListGradeExcelDataReader
    {
        private readonly IMstLgaBp2PartListGradeAppService _MstLgaBp2PartListGrade;
        public MstLgaBp2PartListGradeExcelDataReader(IMstLgaBp2PartListGradeAppService MstLgaBp2PartListGrade)
        {
            _MstLgaBp2PartListGrade = MstLgaBp2PartListGrade;
        }
        public async Task<List<ImportMstLgaBp2PartListGradeDto>> GetMstLgaBp2PartListGradeFromExcel (byte[] fileBytes, string fileName)
        {
            try {
                List<ImportMstLgaBp2PartListGradeDto> rowList = new List<ImportMstLgaBp2PartListGradeDto>();
               
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
                    IRow rowGrade = sheet.GetRow(2);
                    IRow rowModel = sheet.GetRow(1);

                    string strGUID = Guid.NewGuid().ToString("N");
                    var v_Model = Convert.ToString(rowModel.GetCell(4));

                    DataFormatter formatter = new DataFormatter();
                    for (int i = startRow; i <= endRow; i++)
                    {

                        //importData = new ImportMstLgaBp2PartListGradeDto();
                        row = sheet.GetRow(i);
                        var v_PartNo = Convert.ToString(row.GetCell(1));
                        if (v_PartNo != null && v_PartNo != "")
                        {
                            //
                            try
                            {
                                var v_PartName = Convert.ToString(row.GetCell(2).StringCellValue);
                                var v_PikLocType = Convert.ToString(row.GetCell(15).StringCellValue);
                                var v_PikAddress = Convert.ToString(row.GetCell(16).StringCellValue);
                                var v_DelAddress = Convert.ToString(row.GetCell(17).StringCellValue);
                                var v_Remark = Convert.ToString(row.GetCell(18).StringCellValue);

                                for (int j = 4; j < 14; j++)
                                {
                                    if (!string.IsNullOrEmpty(Convert.ToString(rowGrade.GetCell(j).StringCellValue)))
                                    {
                                        ImportMstLgaBp2PartListGradeDto importData = new ImportMstLgaBp2PartListGradeDto();
                                        try
                                        {
                                            int eUsageQty = 0;
                                            int.TryParse(Convert.ToString(row.GetCell(j)), out eUsageQty);
                                            if (eUsageQty > 0)
                                            {
                                                importData.Guid = strGUID;
                                                importData.PartNo = v_PartNo;
                                                importData.PartName = v_PartName;
                                                importData.PikLocType = v_PikLocType;
                                                importData.PikAddress = v_PikAddress;
                                                importData.DelAddress = v_DelAddress;
                                                importData.Remark = v_Remark;
                                                importData.Model = v_Model;

                                                importData.Grade = Convert.ToString(rowGrade.GetCell(j).StringCellValue);
                                                importData.UsageQty = Convert.ToInt32(row.GetCell(j).NumericCellValue);
                                                importData.Sorting = Convert.ToInt32(row.GetCell(0).NumericCellValue);
                                                rowList.Add(importData);
                                            }
                                        }
                                        catch
                                        {
                                            importData.Exception += "USAGE_QTY COLUMN " + (j + 1) + " should not be blank!";
                                        }

                                    }

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
                // return rowList;
                if (rowList.Count() > 0)
                {
                    _MstLgaBp2PartListGrade.ImportMstLgaBp2PartListGradeFromExcel(rowList);
                }

                return rowList;
            }
            catch (Exception ex) {
                throw new UserFriendlyException(ex.ToString());
                // return ex;
            }
        }

    }
}
