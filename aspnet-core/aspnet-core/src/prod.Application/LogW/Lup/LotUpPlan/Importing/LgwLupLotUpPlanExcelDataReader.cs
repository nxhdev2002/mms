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
using prod.LogW.Lup.Dto;
using prod.LogW.Pup;
using GemBox.Spreadsheet;
using Twilio.TwiML.Voice;
using NPOI.OpenXmlFormats.Spreadsheet;

namespace prod.LogW.Lup.LotUpPlan.Importing
{
 
    public class LgwLupLotUpPlanExcelDataReader : NpoiExcelImporterBase<ImportLotUpPlanDto>, ILgwLupLotUpPlanExcelDataReader
    {
        private readonly ILgwLupLotUpPlanAppService _lgwLotUpPlan;
        public LgwLupLotUpPlanExcelDataReader(ILgwLupLotUpPlanAppService lgwLotUpPlan,
            IDapperRepository<LgwPupPxPUpPlan, long> dapperRepo)
        {
            _lgwLotUpPlan = lgwLotUpPlan;
        }

        public async Task<List<ImportLotUpPlanDto>> GetLotUpPlanFromExcel(byte[] fileBytes, string fileName)
        {
            try
            {
                List<ImportLotUpPlanDto> rowList = new List<ImportLotUpPlanDto>();
                var importData = new ImportLotUpPlanDto();
                ISheet sheet;

                using (var stream = new MemoryStream(fileBytes))
                {
                    SpreadsheetInfo.SetLicense("EF21-1FW1-HWZF-CLQH");
                    var xlWorkBook = ExcelFile.Load(stream);
                    var v_worksheet = xlWorkBook.Worksheets[0];


                    //Read Data
                    int startRow = 4;
                    int endRow = v_worksheet.Rows.Count;
                    
                    DateTime? _workingDate = Convert.ToDateTime(v_worksheet.Cells[1, 2].Value);
                    string strGUID = Guid.NewGuid().ToString("N");

                    DataFormatter formatter = new DataFormatter();
                    for (int i = startRow; i <= endRow; i++)
                    {
                        try
                        {
                            importData = new ImportLotUpPlanDto();
                            var v_ProdLine = Convert.ToString(v_worksheet.Cells[i, 2].Value);
                            if (v_ProdLine != null && v_ProdLine != "")
                            {
                           
                                var v_lot_no = Convert.ToString(v_worksheet.Cells[i, 3].Value);
                                var v_shift = Convert.ToString(v_worksheet.Cells[i, 4].Value);
                               

                                importData.Guid = strGUID;
                                importData.WorkingDate = _workingDate;
                                importData.ProdLine = v_ProdLine;
                                importData.LotNo = v_lot_no;
                                importData.Shift = v_shift;
                                importData.UnpackingStartDatetime = Convert.ToDateTime(v_worksheet.Cells[i, 5].Value);
                                importData.UnpackingFinishDatetime = Convert.ToDateTime(v_worksheet.Cells[i, 6].Value);
                                importData.Tpm = Convert.ToString(v_worksheet.Cells[i, 7].Value);
                                importData.Remarks = Convert.ToString(v_worksheet.Cells[i, 8].Value);
                                
                                int check = rowList.Where(t => t.LotNo == v_lot_no && t.Shift == v_shift && t.ProdLine == v_ProdLine && t.WorkingDate == _workingDate).Count();
                                if (check == 0)
                                {
                                    rowList.Add(importData);
                                }
                                else
                                {
                                    importData.Exception = (i - 3).ToString();
                                    rowList.Add(importData);
                                    return rowList;

                                }                               
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
                    if (rowList.Count() > 0)
                    {
                        _lgwLotUpPlan.ImportPxPUpPlanFromExcel(rowList);
                    }

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
