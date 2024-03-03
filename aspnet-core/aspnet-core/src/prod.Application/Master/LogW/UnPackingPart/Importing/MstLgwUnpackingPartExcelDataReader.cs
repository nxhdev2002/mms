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
using prod.Master.LogW.UnPackingPart.ImportDto;
using GemBox.Spreadsheet;
using Twilio.TwiML.Voice;

namespace prod.Master.LogW.UnPackingPart.Importing
{
    public class MstLgwUnpackingPartExcelDataReader : NpoiExcelImporterBase<ImportMstLgwUnpackingPartDto>, IMstLgwUnpackingPartExcelDataReader
    {
        private readonly IMstLgwUnpackingPartAppService _lgwUnpackingPart;
        public MstLgwUnpackingPartExcelDataReader(IMstLgwUnpackingPartAppService lgwUnpackingPart)
        {
            _lgwUnpackingPart = lgwUnpackingPart;
        }

        public async Task<List<ImportMstLgwUnpackingPartDto>> GetUnpackingPartFromExcel(byte[] fileBytes, string fileName)
        {
            try
            {
                List<ImportMstLgwUnpackingPartDto> rowList = new List<ImportMstLgwUnpackingPartDto>();
                var importData = new ImportMstLgwUnpackingPartDto();
                ISheet sheet;

                using (var stream = new MemoryStream(fileBytes))
                {
                    SpreadsheetInfo.SetLicense("EF21-1FW1-HWZF-CLQH");
                    var xlWorkBook = ExcelFile.Load(stream);
                    var v_worksheet = xlWorkBook.Worksheets[0];


                    //Read Data
                    int startRow = 2;
                    int endRow = v_worksheet.Rows.Count;
                    
                    string strGUID = Guid.NewGuid().ToString("N");

                    DataFormatter formatter = new DataFormatter();
                    for (int i = startRow; i <= endRow; i++)
                    {
                        importData = new ImportMstLgwUnpackingPartDto();
                        
                        var v_Cfc = Convert.ToString(v_worksheet.Cells[i, 1].Value);
                        if (v_Cfc != null)
                        {
                            try
                            {
                                importData.Guid = strGUID;
                                importData.Cfc = v_Cfc;
                                importData.PartNo = Convert.ToString(v_worksheet.Cells[i, 2].Value);
                                importData.PartName = Convert.ToString(v_worksheet.Cells[i, 3].Value);
                                importData.BackNo = Convert.ToString(v_worksheet.Cells[i, 4].Value);
                                importData.ModuleCode = Convert.ToString(v_worksheet.Cells[i, 5].Value);
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
                    _lgwUnpackingPart.ImportUnpackingPartFromExcel(rowList);
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
