using System;
using Abp.Dapper.Repositories;
using Abp.UI;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using prod.Master.LogA;
using prod.DataExporting.Excel.NPOI;
using prod.Welding.Andon.ImportDto;
using System.Linq;
using prod.Plan.Ccr.ProductionPlan.ImportDto;

namespace prod.Welding.Andon.Importing
{
    public class WldAdoWeldingPlanExcelDataReader : NpoiExcelImporterBase<ImportWldAdoWeldingPlanDto>, IWldAdoWeldingPlanExcelDataReader
    {

        private readonly IWldAdoWeldingPlanAppService _wldAdoWeldingPlan;
        private readonly IDapperRepository<MstLgaBp2PartList, long> _dapperRepo;

        public WldAdoWeldingPlanExcelDataReader(IWldAdoWeldingPlanAppService wldAdoWeldingPlan,
                                        IDapperRepository<MstLgaBp2PartList, long> dapperRepo)
        {
            _wldAdoWeldingPlan = wldAdoWeldingPlan;
            _dapperRepo = dapperRepo;
        }
        public async Task<List<ImportWldAdoWeldingPlanDto>> GetWldAdoWeldingPlanFromExcel(byte[] fileBytes, string fileName)
        {
            try
            {

                List<ImportWldAdoWeldingPlanDto> rowList = new List<ImportWldAdoWeldingPlanDto>();
                var importData = new ImportWldAdoWeldingPlanDto();

                List<ImportWldAdoEdInDto> rowListEd = new List<ImportWldAdoEdInDto>();
                var importDataED = new ImportWldAdoEdInDto();


                ISheet sheet;


                using (var stream = new MemoryStream(fileBytes))
                {
                    string nameSheet = "";
                    string strGUID = Guid.NewGuid().ToString("N");
                    stream.Position = 0;
                    string ck_Model = "";
                    string ck_Shift = "";
                    if (fileName.EndsWith("xlsx"))
                    {
                        XSSFWorkbook xlsxObject = new XSSFWorkbook(stream);
                        int CountSheetX = xlsxObject.Count;
                        //sheet
                        for (int s = 0; s < CountSheetX; s++)
                        {
                            sheet = xlsxObject.GetSheetAt(s); //get first sheet from workbook 
                            nameSheet = xlsxObject.GetSheetName(s);
                            ck_Shift = "";

                            //Read
                            if (nameSheet == "W1 IN" || nameSheet == "W2 IN" || nameSheet == "W3 IN")  // check sheet frame
                            {
                                try
                                {
                                    //check shift
                                    IRow rowCheckShift = sheet.GetRow(0);
                                    for (int w = 0; w < sheet.LastRowNum; w++)
                                    {
                                        if(ck_Model == "" && ck_Shift == "SHIFT3") { break; }
                                        rowCheckShift = sheet.GetRow(w);
                                        var checkShift = Convert.ToString(rowCheckShift.GetCell(1).ToString());
                                        var v_shift = (checkShift == "Shift: 1st") ? "SHIFT1" : (checkShift == "Shift: 2nd") ? "SHIFT2" : (checkShift == "Shift: 3rd") ? "SHIFT3" : "";
                                        if ((v_shift == "SHIFT1") || (v_shift == "SHIFT2") || (v_shift == "SHIFT3"))
                                        {
                                            IRow rowRqd = sheet.GetRow(4);
                                            IRow row = sheet.GetRow((w + 4));
                                            int startRow = w + 4;
                                            int endRow = sheet.LastRowNum;


                                            DateTime? _requestDate = DateTime.FromOADate((rowRqd.GetCell(3).NumericCellValue));

                                            for (int i = startRow; i <= endRow; i++)
                                            {
                                                try
                                                {
                                                    importData = new ImportWldAdoWeldingPlanDto();
                                                    row = sheet.GetRow(i);
                                                    var v_Model = Convert.ToString(row.GetCell(2).StringCellValue);

                                                    ck_Model = v_Model;
                                                    ck_Shift = v_shift;

                                                    if (v_Model != null && v_Model != "")
                                                    {
                                                        TimeSpan _planTime = DateTime.FromOADate((row.GetCell(9).NumericCellValue)).TimeOfDay;

                                                        importData.Guid = strGUID;
                                                        importData.Shift = v_shift;
                                                        importData.Model = v_Model;
                                                        importData.Welding = nameSheet;
                                                        importData.LotNo = Convert.ToString(row.GetCell(3).StringCellValue);
                                                        importData.NoInLot = Convert.ToInt32(row.GetCell(4).NumericCellValue);
                                                        importData.Grade = Convert.ToString(row.GetCell(5).StringCellValue);
                                                        importData.BodyNo = Convert.ToString(row.GetCell(6).StringCellValue);
                                                        importData.VinNo = Convert.ToString(row.GetCell(8).StringCellValue);
                                                        importData.PlanTime = Convert.ToString(_planTime.ToString());
                                                        importData.RequestDate = _requestDate;
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
                                                rowList.Add(importData);
                                            }
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    importData.Exception += ex.ToString();
                                    break;
                                }
                            }

                            //Read ED IN
                            else if (nameSheet == "ED IN")  // check sheet
                            {
                                 try
                                {
                                //check shift
                                IRow rowCheckShift = sheet.GetRow(0);
                                int a = sheet.LastRowNum;
                                for (int w = 0; w < a ; w++)
                                {
                                    if (ck_Model == "" && ck_Shift == "SHIFT3") { break; }
                                    rowCheckShift = sheet.GetRow(w);
                                    var checkShift = Convert.ToString(rowCheckShift.GetCell(1).ToString());
                                    var v_shift = (checkShift == "Shift: 1st") ? "SHIFT1" : (checkShift == "Shift: 2nd") ? "SHIFT2" : (checkShift == "Shift: 3rd") ? "SHIFT3" : "";
                                    if ((v_shift == "SHIFT1") || (v_shift == "SHIFT2") || (v_shift == "SHIFT3"))
                                    {
                                        IRow rowRqd = sheet.GetRow(4);
                                        IRow row = sheet.GetRow((w + 4));
                                        int startRow = w + 4;
                                        int endRow = sheet.LastRowNum;

                                        DateTime? _requestDate = DateTime.FromOADate((rowRqd.GetCell(3).NumericCellValue));

                                        for (int i = startRow; i <= endRow; i++)
                                        {
                                            try
                                            {
                                                importDataED = new ImportWldAdoEdInDto();
                                                row = sheet.GetRow(i);
                                                var v_Model = Convert.ToString(row.GetCell(2).StringCellValue);

                                                ck_Model = v_Model;
                                                ck_Shift = v_shift;

                                                    if (v_Model != null && v_Model != "")
                                                {
                                                    TimeSpan _planTime = DateTime.FromOADate((row.GetCell(9).NumericCellValue)).TimeOfDay;

                                                    importDataED.Guid = strGUID;
                                                    importDataED.Shift = v_shift;
                                                    importDataED.Model = v_Model;
                                                    importDataED.ProdLine = nameSheet;
                                                    importDataED.LotNo = Convert.ToString(row.GetCell(3).StringCellValue);
                                                    importDataED.NoInLot = Convert.ToInt32(row.GetCell(4).NumericCellValue);
                                                    //importData.Grade = Convert.ToString(row.GetCell(5).StringCellValue);
                                                    importDataED.BodyNo = Convert.ToString(row.GetCell(6).StringCellValue);
                                                    importDataED.Vin = Convert.ToString(row.GetCell(8).StringCellValue);
                                                    importDataED.PlanTime = Convert.ToString(_planTime.ToString());
                                                    importDataED.RequestDate = _requestDate;
                                                }
                                                else
                                                {
                                                    break;
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                importDataED.Exception += ex.ToString();
                                            }
                                            rowListEd.Add(importDataED);
                                        }
                                    }
                                }
                                }
                                catch (Exception ex)
                                {
                                    importData.Exception += ex.ToString();
                                    break;
                                }
                            }
                        }
                    }
                    else
                    {
                        HSSFWorkbook hssfworkbook = new HSSFWorkbook(stream);
                        int CountSheetH = hssfworkbook.Count;
                        for (int s1 = 0; s1 < CountSheetH; s1++)
                        {
                            sheet = hssfworkbook.GetSheetAt(s1); //get first sheet from workbook 
                            nameSheet = hssfworkbook.GetSheetName(s1);
                        }
                    }
                }

                //// return rowList;
                if (rowList.Count() > 0)
                {
                    await _wldAdoWeldingPlan.ImportWldAdoWeldingPlanFromExcel(rowList);
                }

                //// return rowListEd;
                if (rowListEd.Count() > 0)
                {
                    await _wldAdoWeldingPlan.ImportWldAdoEdInFromExcel(rowListEd);
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
