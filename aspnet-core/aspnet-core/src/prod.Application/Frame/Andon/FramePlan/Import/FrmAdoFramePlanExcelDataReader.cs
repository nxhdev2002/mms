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
using prod.Frame.Andon.FramePlanA1;
using GemBox.Spreadsheet;
using Twilio.TwiML.Voice;

namespace prod.Frame.Andon.Importing
{
    public class FrmAdoFramePlanExcelDataReader : NpoiExcelImporterBase<ImportFrmAdoFramePlanDto>, IFrmAdoFramePlanExcelDataReader
    {
        private readonly IFrmAdoFramePlanAppService _frmAdoFramePlan;
        private readonly IDapperRepository<FrmAdoFramePlan, long> _dapperRepo;
        public FrmAdoFramePlanExcelDataReader(IFrmAdoFramePlanAppService frmAdoFramePlan,
            IDapperRepository<FrmAdoFramePlan, long> dapperRepo)
        {
            _frmAdoFramePlan = frmAdoFramePlan;
            _dapperRepo = dapperRepo;
        }

        public async Task<List<ImportFrmAdoFramePlanDto>> GetFramePlanFromExcel(byte[] fileBytes, string fileName)
        {
            try
            {
                List<ImportFrmAdoFramePlanDto> rowList = new List<ImportFrmAdoFramePlanDto>();
                var importData = new ImportFrmAdoFramePlanDto();
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
                                importData = new ImportFrmAdoFramePlanDto();

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
                    if (rowList.Count() > 0)
                    {
                        await _frmAdoFramePlan.ImportFramePlan_V2(rowList);
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
