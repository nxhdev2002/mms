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
using prod.Master.LogW.EciPart.ImportDto;
using GemBox.Spreadsheet;
using Twilio.TwiML.Voice;

namespace prod.Master.LogW.EciPart.Import
{
    public class MstLgwEciPartExcelDataReader : NpoiExcelImporterBase<ImportMstLgwEciPartDto>, IMstLgwEciPartExcelDataReader
    {
        private readonly IMstLgwEciPartAppService _lgwEciPart;
        private readonly IDapperRepository<MstLgwEciPart, long> _dapperRepo;

        public MstLgwEciPartExcelDataReader(IMstLgwEciPartAppService lgwEciPart,
                                        IDapperRepository<MstLgwEciPart, long> dapperRepo)
        {
            _lgwEciPart = lgwEciPart;
            _dapperRepo = dapperRepo;
        }

        public async Task<List<ImportMstLgwEciPartDto>> GetEciPartFromExcel(byte[] fileBytes, string fileName)
        {
            try
            {
                List<ImportMstLgwEciPartDto> rowList = new List<ImportMstLgwEciPartDto>();
                var importData = new ImportMstLgwEciPartDto();
                ISheet sheet;

                using (var stream = new MemoryStream(fileBytes))
                {
                    SpreadsheetInfo.SetLicense("EF21-1FW1-HWZF-CLQH");
                    var xlWorkBook = ExcelFile.Load(stream);
                    var v_worksheet = xlWorkBook.Worksheets[0];


                    //Read Data
                    int startRow = 1;
                    int endRow = v_worksheet.Rows.Count;
                    string strGUID = Guid.NewGuid().ToString("N");

                    DataFormatter formatter = new DataFormatter();
                    for (int i = startRow; i <= endRow; i++)
                    {
                        importData = new ImportMstLgwEciPartDto();
                        var v_ModuleNo = Convert.ToString(v_worksheet.Cells[i, 1].Value);
                        if (v_ModuleNo != null)
                        {
                            try
                            {
                                importData.Guid = strGUID;
                                importData.ModuleNo = v_ModuleNo;
                                importData.PartNo = Convert.ToString(v_worksheet.Cells[i, 2].Value);
                                importData.SupplierNo = Convert.ToString(v_worksheet.Cells[i, 3].Value);
                                importData.ModuleNoEci = Convert.ToString(v_worksheet.Cells[i, 4].Value);
                                importData.PartNoEci = Convert.ToString(v_worksheet.Cells[i, 5].Value);
                                importData.SupplierNoEci = Convert.ToString(v_worksheet.Cells[i, 6].Value);
                                importData.StartEciSeq = Convert.ToString(v_worksheet.Cells[i, 7].Value);
                                importData.StartEciRenban = Convert.ToString(v_worksheet.Cells[i, 8].Value);
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
                    _lgwEciPart.ImportEciPartFromExcel(rowList);
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
