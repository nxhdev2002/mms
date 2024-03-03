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
using prod.LogA.Lds.LotPlan.ImportDto;
using GemBox.Spreadsheet;

namespace prod.LogA.Lds.LotPlan.Importing
{
   
    public class LgaLdsLotPlanExcelDataReader : NpoiExcelImporterBase<ImportLgaLdsLotPlanDto>, ILgaLdsLotPlanExcelDataReader
    {
        private readonly ILgaLdsLotPlanAppService _LgaLdsLotPlan;
        public LgaLdsLotPlanExcelDataReader(ILgaLdsLotPlanAppService LgaLdsLotPlan,
            IDapperRepository<LgaLdsLotPlan, long> dapperRepo)
        {
            _LgaLdsLotPlan = LgaLdsLotPlan;
        }

        public async Task<List<ImportLgaLdsLotPlanDto>> GetLgaLdsLotPlanFromExcel(byte[] fileBytes, string fileName)
        {
            try
            {
                List<ImportLgaLdsLotPlanDto> rowList = new List<ImportLgaLdsLotPlanDto>();
                var importData = new ImportLgaLdsLotPlanDto();
                ISheet sheet;

                using (var stream = new MemoryStream(fileBytes))
                {
                    SpreadsheetInfo.SetLicense("EF21-1FW1-HWZF-CLQH");
                    var xlWorkBook = ExcelFile.Load(stream);
                    var v_worksheet = xlWorkBook.Worksheets[0];
                    //Read Data
                    int startRow = 4;
                    int endRow = v_worksheet.Rows.Count;

                    DateTime? _workingDate = Convert.ToDateTime((v_worksheet.Cells[1, 2].Value));
                    string strGUID = Guid.NewGuid().ToString("N");

                    DataFormatter formatter = new DataFormatter();
                    for (int i = startRow; i <= v_worksheet.Rows.Count; i++)
                    {

                        try
                        {
                            importData = new ImportLgaLdsLotPlanDto();
                            var v_Line = Convert.ToString(v_worksheet.Cells[i, 2].Value);
                            if (v_Line != "0")
                            {
                                    TimeSpan _planStartTime = Convert.ToDateTime((v_worksheet.Cells[i, 5].Value)).TimeOfDay;

                                    importData.Guid = strGUID;
                                    importData.WorkingDate = _workingDate;
                                    importData.ProdLine = v_Line;
                                    importData.Shift = Convert.ToString(v_worksheet.Cells[i, 3].Value);
                                    importData.SeqLineIn = Convert.ToInt32(v_worksheet.Cells[i, 4].Value);
                                    importData.PlanStartDatetime = Convert.ToDateTime(_planStartTime.ToString());
                                    importData.LotNo = Convert.ToString(v_worksheet.Cells[i, 6].Value);
                                    importData.LotNo2 = Convert.ToString(v_worksheet.Cells[i, 7].Value);
                                    importData.Trip = Convert.ToInt32(v_worksheet.Cells[i, 8].Value);
                                    importData.Dolly = Convert.ToString(v_worksheet.Cells[i, 9].Value);
                               
                                rowList.Add(importData);
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
                        _LgaLdsLotPlan.ImportLgaLdsLotPlanFromExcel(rowList);
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
