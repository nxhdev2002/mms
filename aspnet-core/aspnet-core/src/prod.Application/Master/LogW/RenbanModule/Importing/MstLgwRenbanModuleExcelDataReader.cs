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
using prod.Master.LogW.RenbanModule.ImportDto;
using GemBox.Spreadsheet;
using Twilio.TwiML.Voice;

namespace prod.Master.LogW.RenbanModule.Importing
{

    public class MstLgwRenbanModuleExcelDataReader : NpoiExcelImporterBase<ImportMstLgwRenbanModuleDto>, IMstLgwRenbanModuleExcelDataReader
    {
        private readonly IMstLgwRenbanModuleAppService _mstlgwrenbanmodule;
        public MstLgwRenbanModuleExcelDataReader(IMstLgwRenbanModuleAppService mstlgwrenbanmodule,
            IDapperRepository<MstLgwRenbanModule, long> dapperRepo)
        {
            _mstlgwrenbanmodule = mstlgwrenbanmodule;
        }

        public async Task<List<ImportMstLgwRenbanModuleDto>> GetMstLgwRenbanModuleFromExcel(byte[] fileBytes, string fileName)
        {
            try
            {
                List<ImportMstLgwRenbanModuleDto> rowList = new List<ImportMstLgwRenbanModuleDto>();
                var importData = new ImportMstLgwRenbanModuleDto();
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
                        importData = new ImportMstLgwRenbanModuleDto();
                        var v_caseNo = Convert.ToString(v_worksheet.Cells[i, 1].Value);
                        if (v_caseNo != null)
                        {
                            try
                            {
                                importData.Guid = strGUID;
                                importData.CaseNo = v_caseNo;
                                importData.SupplierNo = Convert.ToString(v_worksheet.Cells[i, 2].Value);
                                importData.Renban = Convert.ToString(v_worksheet.Cells[i, 3].Value);
                                importData.ProdLine = Convert.ToString(v_worksheet.Cells[i, 4].Value);
                                importData.Model = Convert.ToString(v_worksheet.Cells[i, 5].Value);
                                importData.Cfc = Convert.ToString(v_worksheet.Cells[i, 6].Value);
                                importData.ModuleType = Convert.ToString(v_worksheet.Cells[i, 7].Value);
                                importData.ModuleSize = Convert.ToString(v_worksheet.Cells[i, 8].Value);
                                importData.SortingType = Convert.ToString(v_worksheet.Cells[i, 9].Value);
                                importData.WhLoc = Convert.ToString(v_worksheet.Cells[i, 10].Value);
                                importData.MinModule = Convert.ToInt32(v_worksheet.Cells[i, 11].Value);
                                importData.MaxModule = Convert.ToInt32(v_worksheet.Cells[i, 12].Value);
                                importData.ModuleCapacity = Convert.ToInt32(v_worksheet.Cells[i, 13].Value);
                                importData.IsActive = Convert.ToString(v_worksheet.Cells[i, 14].Value);
                                importData.MonitorVisualize = Convert.ToInt32(v_worksheet.Cells[i, 15].Value);
                                importData.CaseOrder = Convert.ToInt32(v_worksheet.Cells[i, 16].Value);
                                importData.IsUsePxpData = Convert.ToString(v_worksheet.Cells[i, 17].Value);
                                importData.UpLeadtime = Convert.ToInt32(v_worksheet.Cells[i, 18].Value);

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
                    await _mstlgwrenbanmodule.ImportMstLgwRenbanModuleFromExcel(rowList);
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
