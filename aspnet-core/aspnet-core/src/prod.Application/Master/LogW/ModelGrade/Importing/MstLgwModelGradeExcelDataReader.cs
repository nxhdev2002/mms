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
using prod.Master.LogW.ModelGrade.ImportDto;
using GemBox.Spreadsheet;
using Twilio.TwiML.Voice;

namespace prod.Master.LogW.ModelGrade.Importing
{

    public class MstLgwModelGradeExcelDataReader : NpoiExcelImporterBase<ImportMstlgwModelGradeDto>, IMstLgwModelGradeExcelDataReader
    {
        private readonly IMstLgwModelGradeAppService _mstlgwmodelgrade;
        public MstLgwModelGradeExcelDataReader(IMstLgwModelGradeAppService mstlgwmodelgrade,
            IDapperRepository<MstLgwModelGrade, long> dapperRepo)
        {
            _mstlgwmodelgrade = mstlgwmodelgrade;
        }

        public async Task<List<ImportMstlgwModelGradeDto>> GetMstLgwModelgradeFromExcel(byte[] fileBytes, string fileName)
        {
            try
            {
                List<ImportMstlgwModelGradeDto> rowList = new List<ImportMstlgwModelGradeDto>();
                var importData = new ImportMstlgwModelGradeDto();
                ISheet sheet;

                using (var stream = new MemoryStream(fileBytes))
                {
                    SpreadsheetInfo.SetLicense("EF21-1FW1-HWZF-CLQH");
                    var xlWorkBook = ExcelFile.Load(stream);
                    var v_worksheet = xlWorkBook.Worksheets[0];


                    //Read Data
                    int startRow = 4;
                    int endRow = v_worksheet.Rows.Count;
                    string strGUID = Guid.NewGuid().ToString("N");

                    DataFormatter formatter = new DataFormatter();
                    for (int i = startRow; i <= endRow; i++)
                    {
                        importData = new ImportMstlgwModelGradeDto();
                        var v_Model = Convert.ToString(v_worksheet.Cells[i, 1].Value);
                        if (v_Model != null)
                        {
                            try
                            {
                                importData.Guid = strGUID;
                                importData.Model = v_Model;
                                importData.Grade  = Convert.ToString(v_worksheet.Cells[i, 2].Value);
                                importData.ModuleMkQty = Convert.ToInt32(v_worksheet.Cells[i, 3].Value);
                                importData.ModuleMkQty = Convert.ToInt32(v_worksheet.Cells[i, 4].Value);
                                importData.UpLeadtime = Convert.ToInt32(v_worksheet.Cells[i, 5].Value);
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
                    _mstlgwmodelgrade.ImportMstLgwModelgradeFromExcel(rowList);
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
