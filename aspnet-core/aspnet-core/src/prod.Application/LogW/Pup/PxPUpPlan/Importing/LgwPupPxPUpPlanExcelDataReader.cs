using Abp.Dapper.Repositories;
using Abp.UI;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using prod.DataExporting.Excel.NPOI;
using prod.Frame.Andon.Dto;
using prod.LogW.Pup.ImportDto;
using GemBox.Spreadsheet;
using Twilio.TwiML.Voice;

namespace prod.LogW.Pup.Importing
{
    public class LgwPupPxPUpPlanExcelDataReader : NpoiExcelImporterBase<ImportPxPUpPlanDto>, IPxPUpPlanExcelDataReader
    {
        private readonly ILgwPupPxPUpPlanAppService _lgwPupPxPUpPlan;
        private readonly ILgwPupPxPUpPlanBaseAppService _lgwPupPxPUpPlanBase;
        public LgwPupPxPUpPlanExcelDataReader(ILgwPupPxPUpPlanAppService lgwPupPxPUpPlan,
                                              ILgwPupPxPUpPlanBaseAppService lgwPupPxPUpPlanBase,
            IDapperRepository<LgwPupPxPUpPlan, long> dapperRepo)
        {
            _lgwPupPxPUpPlan = lgwPupPxPUpPlan;
            _lgwPupPxPUpPlanBase = lgwPupPxPUpPlanBase;
        }

        public async Task<List<ImportPxPUpPlanDto>> GetPxPUpPlanFromExcel(byte[] fileBytes, string fileName)
        {
            try
            {
                List<ImportPxPUpPlanBaseDto> rowListBase = new List<ImportPxPUpPlanBaseDto>();
                List<ImportPxPUpPlanDto> rowList = new List<ImportPxPUpPlanDto>();
                var importDataBase = new ImportPxPUpPlanBaseDto();
                var importData = new ImportPxPUpPlanDto();
                ISheet sheet;

                using (var stream = new MemoryStream(fileBytes))
                {
                    SpreadsheetInfo.SetLicense("EF21-1FW1-HWZF-CLQH");
                    var xlWorkBook = ExcelFile.Load(stream);
                    var v_worksheet = xlWorkBook.Worksheets[0];


                    //Read Data
                    int startRowBase = 2;
                    int endRowBase = 4;

                    int startRow = 8;
                    int endRow = v_worksheet.Rows.Count;

                    DateTime? _workingDate = Convert.ToDateTime(v_worksheet.Cells[1, 1].Value);
                    string strGUID = Guid.NewGuid().ToString("N");


             //LgwPupPxPUpPlanBase
                    for (int i = startRowBase; i <= endRowBase; i++)
                    {
                        try
                        {
                            importDataBase = new ImportPxPUpPlanBaseDto();
                            var v_ProdLine = Convert.ToString(v_worksheet.Cells[i, 1].Value);
                            if (v_ProdLine != null && v_ProdLine != "")
                            {
                                importDataBase.Guid = strGUID;
                                importDataBase.WorkingDate = _workingDate;
                                importDataBase.ProdLine = v_ProdLine;
                                importDataBase.Shift1 = Convert.ToInt32(v_worksheet.Cells[i, 2].Value);
                                importDataBase.Shift2 = Convert.ToInt32(v_worksheet.Cells[i, 3].Value);
                                importDataBase.Shift3 = Convert.ToInt32(v_worksheet.Cells[i, 4].Value);

                                rowListBase.Add(importDataBase);
                            }
                            else
                            {
                                break;
                            }
                        }
                        catch (Exception ex)
                        {
                            importDataBase.Exception += ex.ToString();
                        }
                    }


            //LgwPupPxPUpPlan
                    for (int i = startRow; i <= endRow; i++)
                    {
                        try
                        {
                            importData = new ImportPxPUpPlanDto();
                            var v_ProdLine = Convert.ToString(v_worksheet.Cells[i, 2].Value);
                            if (v_ProdLine != null && v_ProdLine != "")
                            {
                                 
                                importData.Guid = strGUID;
                                importData.WorkingDate = _workingDate;
                                importData.ProdLine = v_ProdLine;
                                importData.SeqLineIn = Convert.ToInt32(v_worksheet.Cells[i, 2].Value);
                                importData.UnpackingStartDatetime = Convert.ToDateTime(v_worksheet.Cells[i, 3].Value);
                                importData.SupplierNo = Convert.ToString(v_worksheet.Cells[i, 4].Value);
                                importData.CaseNo = Convert.ToString(v_worksheet.Cells[i, 5].Value);
                                importData.UpTable = Convert.ToString(v_worksheet.Cells[i, 6].Value);
                                importData.IsNoPxpData = Convert.ToString(v_worksheet.Cells[i, 7].Value);

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
                    if (rowList.Count() > 0 )
                    {
                         _lgwPupPxPUpPlan.ImportPxPUpPlanFromExcel(rowList);
                    }

                    if (rowListBase.Count() > 0)
                    {
                        _lgwPupPxPUpPlanBase.ImportPxPUpPlanBaseFromExcel(rowListBase);;
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
