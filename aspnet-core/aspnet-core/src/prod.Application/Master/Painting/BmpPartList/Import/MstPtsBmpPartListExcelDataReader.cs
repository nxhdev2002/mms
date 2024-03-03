using Abp.Dapper.Repositories;
using Abp.UI;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using prod.DataExporting.Excel.NPOI;
using prod.Master.Painting.BmpPartList.ImportDto;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GemBox.Spreadsheet;
using Twilio.TwiML.Voice;

namespace prod.Master.Painting.BmpPartList.Import
{
    public class MstPtsBmpPartListExcelDataReader : NpoiExcelImporterBase<ImportMstPtsBmpPartListDto>, IMstPtsBmpPartListExcelDataReader
    {
        private readonly IMstPtsBmpPartListAppService _ptsBmpPartList;
        private readonly IDapperRepository<MstPtsBmpPartList, long> _dapperRepo;

        public MstPtsBmpPartListExcelDataReader(IMstPtsBmpPartListAppService ptsBmpPartList,
                                        IDapperRepository<MstPtsBmpPartList, long> dapperRepo)
        {
            _ptsBmpPartList = ptsBmpPartList;
            _dapperRepo = dapperRepo;
        }
        public async Task<List<ImportMstPtsBmpPartListDto>> GetBmpPartListFromExcel(byte[] fileBytes, string fileName)
        {
            try
            {
                List<ImportMstPtsBmpPartListDto> rowList = new List<ImportMstPtsBmpPartListDto>();
                var importData = new ImportMstPtsBmpPartListDto();
                ISheet sheet;

                using (var stream = new MemoryStream(fileBytes))
                {
                    SpreadsheetInfo.SetLicense("EF21-1FW1-HWZF-CLQH");
                    var xlWorkBook = ExcelFile.Load(stream);
                    var v_worksheet = xlWorkBook.Worksheets[0];

                    //Read Data
                    int startRow = 1;
                    int endRow = v_worksheet.Rows.Count;
                    
                    DataFormatter formatter = new DataFormatter();
                    string strGUID = Guid.NewGuid().ToString("N");
                    for (int i = startRow; i <= endRow; i++)
                    {
                        importData = new ImportMstPtsBmpPartListDto();
                        var v_ProdLine = Convert.ToString(v_worksheet.Cells[i, 1].Value);
                        if (v_ProdLine != null)
                        {
                            try
                            {
                                importData.Guid = strGUID;
                                importData.ProdLine = v_ProdLine;
                                importData.Model = Convert.ToString(v_worksheet.Cells[i, 2].Value);
                                importData.Cfc = Convert.ToString(v_worksheet.Cells[i, 3].Value);
                                importData.Grade = Convert.ToString(v_worksheet.Cells[i, 4].Value);
                                importData.BackNo = Convert.ToString(v_worksheet.Cells[i, 5].Value);
                                importData.PartTypeCode = Convert.ToString(v_worksheet.Cells[i, 7].Value);
                                importData.Process = Convert.ToString(v_worksheet.Cells[i, 8].Value);
                                importData.PkProcess = Convert.ToString(v_worksheet.Cells[i, 9].Value);
                                importData.SpecialColor = Convert.ToString(v_worksheet.Cells[i, 10].Value);
                                importData.IsPunch = Convert.ToString(v_worksheet.Cells[i, 11].Value);
                                importData.SignalCode = Convert.ToString(v_worksheet.Cells[i, 12].Value);
                                importData.IsActive = Convert.ToString(v_worksheet.Cells[i, 13].Value);

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
                    _ptsBmpPartList.ImportBmpPartListFromExcel(rowList);

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
