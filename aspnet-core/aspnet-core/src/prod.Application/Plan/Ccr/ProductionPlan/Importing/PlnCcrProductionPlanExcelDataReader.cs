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
using prod.Plan.Ccr;
using prod.Plan.Ccr.ProductionPlan;
using prod.Plan.Ccr.ProductionPlan.ImportDto;

namespace prod.LogW.Pup.Importing
{
    public class PlnCcrProductionPlanExcelDataReader : NpoiExcelImporterBase<ImportPlnCcrProductionPlanDto>, IPlnCcrProductionPlanExcelDataReader
    {
        private readonly IPlnCcrProductionPlanAppService _plnCcrProductionPlan;

        public PlnCcrProductionPlanExcelDataReader(IPlnCcrProductionPlanAppService plnCcrProductionPlan,
            IDapperRepository<PlnCcrProductionPlan, long> dapperRepo)
        {
            _plnCcrProductionPlan = plnCcrProductionPlan;
        }

        public async Task<List<ImportPlnCcrProductionPlanDto>> GetPlnCcrProductionPlanFromExcel(byte[] fileBytes, string fileName)
        {
            try
            {
                List<ImportPlnCcrProductionPlanDto> rowMkList = new List<ImportPlnCcrProductionPlanDto>();
                List<ImportPlnCcrProductionPlanDto> rowFPList = new List<ImportPlnCcrProductionPlanDto>();
                List<ImportPlnCcrProductionPlanDto> rowW1List = new List<ImportPlnCcrProductionPlanDto>();
                List<ImportPlnCcrProductionPlanDto> rowW2List = new List<ImportPlnCcrProductionPlanDto>();
                List<ImportPlnCcrProductionPlanDto> rowW3List = new List<ImportPlnCcrProductionPlanDto>();
                List<ImportPlnCcrProductionPlanDto> rowA1List = new List<ImportPlnCcrProductionPlanDto>();
                List<ImportPlnCcrProductionPlanDto> rowA2List = new List<ImportPlnCcrProductionPlanDto>();
                var importDataMk = new ImportPlnCcrProductionPlanDto();
                var importDataFP = new ImportPlnCcrProductionPlanDto();
                var importDataW1 = new ImportPlnCcrProductionPlanDto();
                var importDataW2 = new ImportPlnCcrProductionPlanDto();
                var importDataW3 = new ImportPlnCcrProductionPlanDto();
                var importDataA1 = new ImportPlnCcrProductionPlanDto();
                var importDataA2 = new ImportPlnCcrProductionPlanDto();

                ISheet sheet;


                using (var stream = new MemoryStream(fileBytes))
                {
                    string nameSheet = "";
                    string strGUID = Guid.NewGuid().ToString("N");
                    stream.Position = 0;
                    if (fileName.EndsWith("xlsx"))
                    {
                        XSSFWorkbook xlsxObject = new XSSFWorkbook(stream);
                        int CountSheetX = xlsxObject.Count;
                        //sheet
                        for(int s = 0 ; s < CountSheetX; s++)
                        {
                            sheet = xlsxObject.GetSheetAt(s); //get first sheet from workbook 
                            nameSheet = xlsxObject.GetSheetName(s);
                           

                            //Read Frame
                            if (nameSheet == "Frame in")  // check sheet frame
                            {
                                int startRow = 6;
                                int endRow = sheet.LastRowNum;
                                IRow row = sheet.GetRow(6);

                             //5x5 Making
                                for (int i = startRow; i <= endRow; i++)
                                {
                                    try
                                    {
                                        importDataMk = new ImportPlnCcrProductionPlanDto();
                                        row = sheet.GetRow(i);
                                        var v_LotNo = Convert.ToString(row.GetCell(15).StringCellValue);
                                        if (v_LotNo != null && v_LotNo != "")
                                        {
                                            importDataMk.Guid = "mk"+ strGUID ;
                                            importDataMk.PlanSequence = (i - 5);
                                            importDataMk.Shop = "F";
                                            importDataMk.LotNo = v_LotNo;
                                            importDataMk.NoInLot = Convert.ToString(row.GetCell(17).NumericCellValue);
                                            importDataMk.Model = Convert.ToString(row.GetCell(18).StringCellValue);
                                            importDataMk.Body = Convert.ToString(row.GetCell(19).StringCellValue);
                                            importDataMk.SupplierNo = "TMI";
                                            importDataMk.UseLotNo = Convert.ToString(row.GetCell(21).StringCellValue);
                                        }
                                        else
                                        {
                                            break;
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        importDataMk.Exception += ex.ToString();
                                    }
                                    rowMkList.Add(importDataMk);
                                }

                            //Frame in Plan
                                for (int i = startRow; i <= endRow; i++)
                                {
                                    try
                                    {
                                        importDataFP = new ImportPlnCcrProductionPlanDto();
                                        row = sheet.GetRow(i);
                                        DateTime? _dateInF = (row.GetCell(34) != null) ? DateTime.FromOADate(row.GetCell(34).NumericCellValue) : null;
                                        var v_LotNo = Convert.ToString(row.GetCell(30).StringCellValue);
                                        if (v_LotNo != null && v_LotNo != "")
                                        {
                                            importDataFP.Guid = "fp" + strGUID;
                                            importDataFP.LotNo = v_LotNo;
                                            importDataFP.NoInLot = Convert.ToString(row.GetCell(31).NumericCellValue);
                                            importDataFP.Grade = Convert.ToString(row.GetCell(32).StringCellValue);
                                            importDataFP.Body = Convert.ToString(row.GetCell(33).StringCellValue);
                                            importDataFP.DateIn = _dateInF;
                                        }
                                        else
                                        {
                                            break;
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        importDataFP.Exception += ex.ToString();
                                    }
                                    rowFPList.Add(importDataFP);
                                }
                            }

                            //Read W1 Plan
                            else if (nameSheet == "W1 PLAN")  // check sheet W1 PLAN
                            {
                                string nameWell = nameSheet.Substring(0, 2);
                                int startRow = 4;
                                int endRow = sheet.LastRowNum;
                                IRow row = sheet.GetRow(4);
                               

                                //W1 IN PLAN
                                for (int i = startRow; i <= endRow; i++)
                                {
                                    try
                                    {
                                        importDataW1 = new ImportPlnCcrProductionPlanDto();
                                        row = sheet.GetRow(i);
                                        DateTime? _dateIn = (row.GetCell(7) != null) ? DateTime.FromOADate(row.GetCell(7).NumericCellValue) : null;
                                        TimeSpan? _timeIn = (row.GetCell(8) != null) ? (DateTime.FromOADate(row.GetCell(8).NumericCellValue).TimeOfDay) : null;
                                        var v_LotNo = Convert.ToString(row.GetCell(3));
                                        if (v_LotNo != null && v_LotNo != "")
                                        {
                                            importDataW1.Guid = strGUID;
                                            importDataW1.PlanSequence = Convert.ToInt32(row.GetCell(1).NumericCellValue);
                                            importDataW1.Shop = nameWell;
                                            importDataW1.Model = Convert.ToString(row.GetCell(2).StringCellValue);
                                            importDataW1.LotNo = v_LotNo;
                                            importDataW1.NoInLot = Convert.ToString(row.GetCell(4).NumericCellValue);
                                            importDataW1.Grade = Convert.ToString(row.GetCell(5).StringCellValue);
                                            importDataW1.Body = Convert.ToString(row.GetCell(6).StringCellValue);
                                            importDataW1.DateIn = _dateIn;
                                            importDataW1.TimeIn = _timeIn;
                                            importDataW1.SupplierNo2 = "TMT";
                                            importDataW1.UseLotNo2 = Convert.ToString(row.GetCell(10).StringCellValue);
                                        }
                                        else
                                        {
                                            break;
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        importDataW1.Exception += ex.ToString();
                                    }
                                    rowW1List.Add(importDataW1);
                                }

                                
                            }

                            //Read W2 Plan
                            else if (nameSheet == "W2 PLAN")  // check sheet W2 PLAN
                            {
                                string nameWell = nameSheet.Substring(0, 2);
                                int startRow = 4;
                                int endRow = sheet.LastRowNum;
                                IRow row = sheet.GetRow(4);

                                //WELL IN PLAN
                                for (int i = startRow; i <= endRow; i++)
                                {
                                    try
                                    {
                                        importDataW2 = new ImportPlnCcrProductionPlanDto();
                                        row = sheet.GetRow(i);
                                        DateTime? _dateIn = (row.GetCell(7) != null) ? DateTime.FromOADate(row.GetCell(7).NumericCellValue) : null;
                                        TimeSpan? _timeIn = (row.GetCell(8) != null) ? (DateTime.FromOADate(row.GetCell(8).NumericCellValue).TimeOfDay) : null;
                                        var v_LotNo = Convert.ToString(row.GetCell(3));
                                        if (v_LotNo != null && v_LotNo != "")
                                        {
                                            importDataW2.Guid = strGUID;
                                            importDataW2.PlanSequence = Convert.ToInt32(row.GetCell(1).NumericCellValue);
                                            importDataW2.Shop = nameWell;
                                            importDataW2.Model = Convert.ToString(row.GetCell(2).StringCellValue);
                                            importDataW2.LotNo = v_LotNo;
                                            importDataW2.NoInLot = Convert.ToString(row.GetCell(4).NumericCellValue);
                                            importDataW2.Grade = Convert.ToString(row.GetCell(5).StringCellValue);
                                            importDataW2.Body = Convert.ToString(row.GetCell(6).StringCellValue);
                                            importDataW2.DateIn = _dateIn;
                                            importDataW2.TimeIn = _timeIn;
                                            importDataW2.SupplierNo = "TMI";
                                            importDataW2.UseLotNo = Convert.ToString(row.GetCell(10).StringCellValue);
                                        }
                                        else
                                        {
                                            break;
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        importDataW2.Exception += ex.ToString();
                                    }
                                    rowW2List.Add(importDataW2);
                                }


                            }


                            //Read W3 Plan
                            else if (nameSheet == "W3 PLAN")  // check sheet W3 PLAN
                            {
                                string nameWell = nameSheet.Substring(0, 2);
                                int startRow = 4;
                                int endRow = sheet.LastRowNum;
                                IRow row = sheet.GetRow(4);

                                //WELL IN PLAN
                                for (int i = startRow; i <= endRow; i++)
                                {
                                    try
                                    {
                                        importDataW3 = new ImportPlnCcrProductionPlanDto();
                                        row = sheet.GetRow(i);
                                        DateTime? _dateIn = (row.GetCell(7) != null) ? DateTime.FromOADate(row.GetCell(7).NumericCellValue) : null;
                                        TimeSpan? _timeIn = (row.GetCell(8) != null) ? (DateTime.FromOADate(row.GetCell(8).NumericCellValue).TimeOfDay) : null;
                                        var v_LotNo = Convert.ToString(row.GetCell(3));
                                        if (v_LotNo != null && v_LotNo != "")
                                        {
                                            importDataW3.Guid = strGUID;
                                            importDataW3.PlanSequence = Convert.ToInt32(row.GetCell(1).NumericCellValue);
                                            importDataW3.Shop = nameWell;
                                            importDataW3.Model = Convert.ToString(row.GetCell(2).StringCellValue);
                                            importDataW3.LotNo = v_LotNo;
                                            importDataW3.NoInLot = Convert.ToString(row.GetCell(4).NumericCellValue);
                                            importDataW3.Grade = Convert.ToString(row.GetCell(5).StringCellValue);
                                            importDataW3.Body = Convert.ToString(row.GetCell(6).StringCellValue);
                                            importDataW3.DateIn = _dateIn;
                                            importDataW3.TimeIn = _timeIn;
                                            importDataW3.SupplierNo = "TMI";
                                            importDataW3.UseLotNo = Convert.ToString(row.GetCell(10).StringCellValue);
                                        }
                                        else
                                        {
                                            break;
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        importDataW3.Exception += ex.ToString();
                                    }
                                    rowW3List.Add(importDataW3);
                                }


                            }

                            //Read A1 Plan
                            else if (nameSheet == "A1 PLAN")  // check sheet A1 PLAN
                            {
                                int startRow = 4;
                                int endRow = sheet.LastRowNum;
                                IRow row = sheet.GetRow(4);
                                IRow rowH = sheet.GetRow(3);

                                //A1 IN PLAN
                                for (int i = startRow; i <= endRow; i++)
                                {
                                    try
                                    {
                                        importDataA1 = new ImportPlnCcrProductionPlanDto();
                                        row = sheet.GetRow(i);
                                        DateTime? _dateIn = (row.GetCell(7) != null) ? DateTime.FromOADate(row.GetCell(7).NumericCellValue) : null;
                                        TimeSpan? _timeIn = (row.GetCell(8) != null) ? (DateTime.FromOADate(row.GetCell(8).NumericCellValue).TimeOfDay) : null;
                                        var v_LotNo = Convert.ToString(row.GetCell(3).StringCellValue);
                                        if (v_LotNo != null && v_LotNo != "")
                                        {

                                            importDataA1.Guid = strGUID;
                                            importDataA1.PlanSequence = Convert.ToInt32(row.GetCell(1).NumericCellValue);
                                            importDataA1.Shop = "A1";
                                            importDataA1.Model = Convert.ToString(row.GetCell(2).StringCellValue);
                                            importDataA1.LotNo = v_LotNo;
                                            importDataA1.NoInLot = Convert.ToString(row.GetCell(4).NumericCellValue);
                                            importDataA1.Grade = Convert.ToString(row.GetCell(5).StringCellValue);
                                            importDataA1.Body = Convert.ToString(row.GetCell(6).StringCellValue);
                                            importDataA1.DateIn = _dateIn;
                                            importDataA1.TimeIn = _timeIn;
                                            importDataA1.SupplierNo2 = (Convert.ToString(rowH.GetCell(10).StringCellValue) != "") ? "TMT" : "";
                                            importDataA1.SupplierNo = (Convert.ToString(rowH.GetCell(11).StringCellValue) != "") ? "TMI" : "";
                                            importDataA1.UseLotNo2 = (Convert.ToString(rowH.GetCell(10).StringCellValue) != "") ? Convert.ToString(row.GetCell(10).StringCellValue) : "";
                                            importDataA1.UseLotNo = (Convert.ToString(rowH.GetCell(11).StringCellValue) != "") ? Convert.ToString(row.GetCell(11).StringCellValue) : "";
                                        }
                                        else
                                        {
                                            break;
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        importDataA1.Exception += ex.ToString();
                                    }
                                    rowA1List.Add(importDataA1);
                                }


                            }

                            //Read A2 Plan
                            else if (nameSheet == "A2 PLAN")  // check sheet A2 PLAN
                            {
                                string nameA = nameSheet.Substring(0, 2);
                                int startRow = 4;
                                int endRow = sheet.LastRowNum;
                                IRow row = sheet.GetRow(4);
                                IRow rowH = sheet.GetRow(3);
                               
                                //A2 IN PLAN
                                for (int i = startRow; i <= endRow; i++)
                                {
                                    try
                                    {
                                        importDataA2 = new ImportPlnCcrProductionPlanDto();
                                        row = sheet.GetRow(i);
                                        DateTime? _dateIn = (row.GetCell(7) != null) ? DateTime.FromOADate(row.GetCell(7).NumericCellValue) : null;
                                        TimeSpan? _timeIn = (row.GetCell(8) != null) ? (DateTime.FromOADate(row.GetCell(8).NumericCellValue).TimeOfDay) : null;
                                        var v_LotNo = Convert.ToString(row.GetCell(3).StringCellValue);
                                        if (v_LotNo != null && v_LotNo != "")
                                        {
                                            importDataA2.Guid = strGUID;
                                            importDataA2.PlanSequence = Convert.ToInt32(row.GetCell(1).NumericCellValue);
                                            importDataA2.Shop = nameA;
                                            importDataA2.Model = Convert.ToString(row.GetCell(2).StringCellValue);
                                            importDataA2.LotNo = v_LotNo;
                                            importDataA2.NoInLot = Convert.ToString(row.GetCell(4).NumericCellValue);
                                            importDataA2.Grade = Convert.ToString(row.GetCell(5).StringCellValue);
                                            importDataA2.Body = Convert.ToString(row.GetCell(6).StringCellValue);
                                            importDataA2.DateIn = _dateIn;
                                            importDataA2.TimeIn = _timeIn;
                                            importDataA2.SupplierNo2 = (Convert.ToString(rowH.GetCell(10).StringCellValue) != "") ? "TMT" : "";
                                            importDataA2.SupplierNo = (Convert.ToString(rowH.GetCell(11).StringCellValue) != "") ? "TMI" : "";
                                            importDataA2.UseLotNo2 = (Convert.ToString(rowH.GetCell(10).StringCellValue) != "") ? Convert.ToString(row.GetCell(10).StringCellValue) : "";
                                            importDataA2.UseLotNo = (Convert.ToString(rowH.GetCell(11).StringCellValue) != "") ? Convert.ToString(row.GetCell(11).StringCellValue) : "";
                                        }
                                        else
                                        {
                                            break;
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        importDataA2.Exception += ex.ToString();
                                    }
                                    rowA2List.Add(importDataA2);
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

                // return rowMkList;
                if (rowMkList.Count() > 0)
                {
                    _plnCcrProductionPlan.ImportPlnCcrProductionPlanFMKFromExcel(rowMkList);
                }

                // return rowFpList;
                if (rowFPList.Count() > 0)
                {
                    _plnCcrProductionPlan.ImportPlnCcrProductionPlanFPFromExcel(rowFPList);
                }

                // return W1 in Plan;
                if (rowW1List.Count() > 0)
                {
                    _plnCcrProductionPlan.ImportPlnCcrProductionPlanW1FromExcel(rowW1List);
                }

                // return W2 in Plan;
                if (rowW2List.Count() > 0)
                {
                    _plnCcrProductionPlan.ImportPlnCcrProductionPlanW2FromExcel(rowW2List);
                }

                // return W3 in Plan;
                if (rowW3List.Count() > 0)
                {
                    _plnCcrProductionPlan.ImportPlnCcrProductionPlanW3FromExcel(rowW3List);
                }

                // return A1 in Plan;
                if (rowA1List.Count() > 0)
                {
                    _plnCcrProductionPlan.ImportPlnCcrProductionPlanA1FromExcel(rowA1List);
                }

                // return A2 in Plan;
                if (rowA2List.Count() > 0)
                {
                    _plnCcrProductionPlan.ImportPlnCcrProductionPlanA2FromExcel(rowA2List);
                }

                return rowMkList;
            }
            catch (Exception ex)
            {

                throw new UserFriendlyException(ex.ToString());
                // return ex;
            }
        }

    }
}
