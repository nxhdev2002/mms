using Abp.Dapper.Repositories;
using Abp.UI;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using prod.DataExporting.Excel.NPOI;
using prod.Frame.Andon.Dto;
using prod.Frame.Andon.FramePlanA1;
using GemBox.Spreadsheet;
using Twilio.TwiML.Voice;

namespace prod.Frame.Andon.Importing
{
    public class FrmAdoFramePlanA1ExcelDataReader : NpoiExcelImporterBase<ImportFrmAdoFramePlanA1Dto>, IFrmAdoFramePlanA1ExcelDataReader
    {
        private readonly IFrmAdoFramePlanA1AppService _frmAdoFramePlanA1;
        private readonly IDapperRepository<FrmAdoFramePlanA1, long> _dapperRepo;
        public FrmAdoFramePlanA1ExcelDataReader(IFrmAdoFramePlanA1AppService frmAdoFramePlanA1,
            IDapperRepository<FrmAdoFramePlanA1, long> dapperRepo)
        {
            _frmAdoFramePlanA1 = frmAdoFramePlanA1;
            _dapperRepo = dapperRepo;
        }

        public async Task<List<ImportFrmAdoFramePlanA1Dto>> GetFramePlanA1FromExcel(byte[] fileBytes, string fileName)
        {
            try
            {
                List<ImportFrmAdoFramePlanA1Dto> rowList = new List<ImportFrmAdoFramePlanA1Dto>();
                var importData = new ImportFrmAdoFramePlanA1Dto();
                ISheet sheet;


                using (var stream = new MemoryStream(fileBytes))
                {
                    SpreadsheetInfo.SetLicense("EF21-1FW1-HWZF-CLQH");
                    var xlWorkBook = ExcelFile.Load(stream);
                    var v_worksheet = xlWorkBook.Worksheets[0];
                    {
                        //Read Data
                        int startRow = 6;
                        int endRow = v_worksheet.Rows.Count;

                        DateTime? _plandate = Convert.ToDateTime(v_worksheet.Cells[2, 2].Value);
                        string strGUID = Guid.NewGuid().ToString("N");

                        for (int i = startRow; i <= v_worksheet.Rows.Count; i++)
                        {
                            try
                            {
                                importData = new ImportFrmAdoFramePlanA1Dto();

                                string v_BodyNo = Convert.ToString(v_worksheet.Cells[i, 4].Value);
                                if (v_BodyNo != null && v_BodyNo != "")
                                {

                                    importData.Guid = strGUID;
                                    importData.No = Convert.ToInt32(v_worksheet.Cells[i, 0].Value);
                                    importData.Model = Convert.ToString(v_worksheet.Cells[i, 1].Value);
                                    importData.LotNo = Convert.ToString(v_worksheet.Cells[i, 2].Value);
                                    importData.NoInLot = Convert.ToInt32(v_worksheet.Cells[i, 3].Value);
                                    importData.BodyNo = v_BodyNo;
                                    importData.VinNo = Convert.ToString(v_worksheet.Cells[i, 6].Value);
                                    importData.PlanDate = _plandate;

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
                    }
                    // return rowList;
                    if (rowList.Count > 0)
                    {
                        _frmAdoFramePlanA1.ImportFramePlanA1_V2(rowList);
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
