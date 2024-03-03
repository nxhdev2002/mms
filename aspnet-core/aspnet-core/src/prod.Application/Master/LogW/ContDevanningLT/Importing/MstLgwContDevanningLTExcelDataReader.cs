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
using prod.Master.LogW;
using GemBox.Spreadsheet;
using Twilio.TwiML.Voice;

namespace prod.LogW.Lup.LotUpPlan.Importing
{

    public class MstLgwContDevanningLTExcelDataReader : NpoiExcelImporterBase<ImportContDevanningLTDto>, IMstLgwContDevanningLTExcelDataReader
    {
        private readonly IMstLgwContDevanningLTAppService _ContDevanningLT;
        public MstLgwContDevanningLTExcelDataReader(IMstLgwContDevanningLTAppService ContDevanningLT,
            IDapperRepository<MstLgwContDevanningLT, long> dapperRepo)
        {
            _ContDevanningLT = ContDevanningLT;
        }

        public async Task<List<ImportContDevanningLTDto>> GetContDevanningLTFromExcel(byte[] fileBytes, string fileName)
        {
            try
            {
                List<ImportContDevanningLTDto> rowList = new List<ImportContDevanningLTDto>();
                var importData = new ImportContDevanningLTDto();
                ISheet sheet;

                using (var stream = new MemoryStream(fileBytes))
                {
                    SpreadsheetInfo.SetLicense("EF21-1FW1-HWZF-CLQH");
                    var xlWorkBook = ExcelFile.Load(stream);
                    var v_worksheet = xlWorkBook.Worksheets[0];


                    //Read Data
                    int startRow = 3;
                    int endRow = v_worksheet.Rows.Count;
                    
                    string strGUID = Guid.NewGuid().ToString("N");

                    DataFormatter formatter = new DataFormatter();
                    for (int i = startRow; i <= endRow; i++)
                    {
                        importData = new ImportContDevanningLTDto();
                        var v_RenbanCode = Convert.ToString(v_worksheet.Cells[i, 1].Value);
                        if (v_RenbanCode != null)
                        {
                            try
                            {
                                importData.Guid = strGUID;
                                importData.RenbanCode = v_RenbanCode;
                                importData.Source = Convert.ToString(v_worksheet.Cells[i, 2].Value);
                                importData.DevLeadtime = Convert.ToInt32(v_worksheet.Cells[i, 3].Value);
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
                    _ContDevanningLT.ImportContDevanningLTFromExcel(rowList);
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
