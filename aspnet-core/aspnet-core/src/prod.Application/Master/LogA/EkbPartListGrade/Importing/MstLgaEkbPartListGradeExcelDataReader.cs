using Abp.UI;
using GemBox.Spreadsheet;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using prod.DataExporting.Excel.NPOI;
using prod.LogA.Pcs.Stock.Dto;
using prod.LogA.Sps.Stock.Dto;
using prod.Master.LogA.Dto;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;


namespace prod.Master.LogA.Importing
{
    public class MstLgaEkbPartListGradeExcelDataReader : NpoiExcelImporterBase<MstLgaEkbPartListGradeImportDto>, IMstLgaEkbPartListGradeExcelDataReader
    {
        private readonly IMstLgaEkbPartListGradeAppService _MstLgaEkbPartListGrade;
        public MstLgaEkbPartListGradeExcelDataReader(IMstLgaEkbPartListGradeAppService MstLgaEkbPartListGrade)
        {
            _MstLgaEkbPartListGrade = MstLgaEkbPartListGrade;
        }
        public async Task<List<MstLgaEkbPartListGradeImportDto>> GetMstLgaEkbPartListGradeFromExcel(byte[] fileBytes, string fileName)
        {
            try
            {
                List<MstLgaEkbPartListGradeImportDto> rowList = new List<MstLgaEkbPartListGradeImportDto>();
                List<LgaPcsStockImportDto> rowListPcStock = new List<LgaPcsStockImportDto>();
                List<LgaSpsStockImportDto> rowListSpsStock = new List<LgaSpsStockImportDto>();

                ISheet sheet;
                CommonFunction fn = new CommonFunction();

                using (var stream = new MemoryStream(fileBytes))
                {
                    string nameSheet = "";
                    string strGUID = Guid.NewGuid().ToString("N");
                    stream.Position = 0;

                    if (fileName.EndsWith("xlsx"))
                    {
                        SpreadsheetInfo.SetLicense("EF21-1FW1-HWZF-CLQH");
                        var xlWorkBook = ExcelFile.Load(stream);


                        //sheet
                        foreach (ExcelWorksheet worksheet in xlWorkBook.Worksheets)
                        {
                            nameSheet = worksheet.Name;

                            //Read
                            if (nameSheet == "PartList")  // check sheet frame
                            {
                                //Read Data
                                int startRow = 3;
                                int endRow = worksheet.Rows.Count;

                                //string strGUID = Guid.NewGuid().ToString("N");
                                var v_Model = Convert.ToString(worksheet.Cells[1, 7].Value);

                                DataFormatter formatter = new DataFormatter();
                                for (int i = startRow; i <= endRow; i++)
                                {
                                    var v_PartNo = Convert.ToString(worksheet.Cells[i,1].Value);
                                    if (v_PartNo != null && v_PartNo != "")
                                    {
                                        //
                                        try
                                        { 
                                            var v_PartName = Convert.ToString(worksheet.Cells[i, 2].Value);
                                            var v_SupplierNo = Convert.ToString(worksheet.Cells[i, 3].Value);
                                            var v_BackNo = Convert.ToString(worksheet.Cells[i, 4].Value);
                                            var v_Cfc = Convert.ToString(worksheet.Cells[i, 5].Value);
                                            var v_BodyColor = Convert.ToString(worksheet.Cells[i, 6].Value);
                                            var v_Module = Convert.ToString(worksheet.Cells[i, 17].Value);
                                            var v_BoxQuantity = Convert.ToInt32(worksheet.Cells[i, 18].Value);
                                            var v_ExporterBackNo = Convert.ToString(worksheet.Cells[i, 19].Value);
                                            var v_PcAddress = Convert.ToString(worksheet.Cells[i, 20].Value);
                                            var v_PcSorting = Convert.ToInt32(worksheet.Cells[i, 21].Value);
                                            var v_SpsAddress = Convert.ToString(worksheet.Cells[i, 22].Value);
                                            var v_SpsSorting = Convert.ToInt32(worksheet.Cells[i, 23].Value);
                                            var v_ProcessCode = Convert.ToString(worksheet.Cells[i, 24].Value);
                                            var v_Remark = Convert.ToString(worksheet.Cells[i, 25].Value);

                                            for (int j = 7; j < 17; j++)
                                            {
                                                if (!string.IsNullOrEmpty(Convert.ToString(worksheet.Cells[2, j].Value)))
                                                {
                                                    MstLgaEkbPartListGradeImportDto importData = new MstLgaEkbPartListGradeImportDto();
                                                    try
                                                    {
                                                        int eUsageQty = 0;
                                                        int.TryParse(Convert.ToString(worksheet.Cells[i, j].Value), out eUsageQty);
                                                        if (eUsageQty > 0)
                                                        {

                                                            importData.Guid = strGUID;
                                                            importData.PartNo = v_PartNo;
                                                            importData.PartName = v_PartName;
                                                            importData.SupplierNo = v_SupplierNo;
                                                            importData.Cfc = v_Cfc;
                                                            importData.BackNo = v_BackNo;
                                                            importData.BodyColor = v_BodyColor;
                                                            importData.Module = v_Module;
                                                            importData.BoxQty = v_BoxQuantity;
                                                            importData.ExporterBackNo = v_ExporterBackNo;
                                                            importData.PcAddress = v_PcAddress;
                                                            importData.PcSorting = v_PcSorting;
                                                            importData.SpsAddress = v_SpsAddress;
                                                            importData.SpsSorting = v_SpsSorting;
                                                            importData.ProcessCode = v_ProcessCode;
                                                            importData.Remark = v_Remark;
                                                            importData.Model = v_Model;

                                                            importData.Grade = Convert.ToString(worksheet.Cells[2, j].Value);
                                                            importData.UsageQty = Convert.ToInt32(worksheet.Cells[i, j].Value);

                                                            rowList.Add(importData);
                                                        }
                                                    }
                                                    catch
                                                    {
                                                        importData.Exception += "USAGE_QTY COLUMN " + (j + 1) + " should not be blank!";
                                                    }

                                                }

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
                            else if (nameSheet == "PcStock")
                            {
                                //Read Data
                                int startRow = 3;
                                int endRow = worksheet.Rows.Count;
             

                                DataFormatter formatter = new DataFormatter();
                                for (int i = startRow; i <= endRow; i++)
                                {
                                    LgaPcsStockImportDto iDataPcStock = new LgaPcsStockImportDto();
                      
                                    var v_PartNo = Convert.ToString(worksheet.Cells[i, 1].Value);
                                    if (v_PartNo != null && v_PartNo != "")
                                    {
                                        try
                                        {
                                            iDataPcStock.Guid = strGUID;
                                            iDataPcStock.PartNo = v_PartNo;
                                            iDataPcStock.PartName = Convert.ToString(worksheet.Cells[i, 2].Value);
                                            iDataPcStock.SupplierNo = Convert.ToString(worksheet.Cells[i, 3].Value);
                                            iDataPcStock.BackNo = Convert.ToString(worksheet.Cells[i, 4].Value);
                                            iDataPcStock.PcRackAddress = Convert.ToString(worksheet.Cells[i, 5].Value);
                                            iDataPcStock.UsagePerHour = Convert.ToInt32(worksheet.Cells[i, 6].Value);
                                            iDataPcStock.RackCapBox = Convert.ToInt32(worksheet.Cells[i, 7].Value);
                                            iDataPcStock.OutType = Convert.ToString(worksheet.Cells[i, 8].Value);
                                            iDataPcStock.StockQty = Convert.ToInt32(worksheet.Cells[i, 9].Value);
                                            iDataPcStock.BoxQty = Convert.ToInt32(worksheet.Cells[i, 10].Value);

                                        }
                                        catch (Exception ex)
                                        {
                                            iDataPcStock.Exception += ex.ToString();
                                        }
                                        rowListPcStock.Add(iDataPcStock);
                                    }
                                }
                            }
                            else if (nameSheet == "SpsStock")
                            {
                                //Read Data
                                int startRow = 3;
                                int endRow = worksheet.Rows.Count;


                                DataFormatter formatter = new DataFormatter();
                                for (int i = startRow; i <= endRow; i++)
                                {
                                    LgaSpsStockImportDto iDataSpsStock = new LgaSpsStockImportDto();
                                    var v_PartNo = Convert.ToString(worksheet.Cells[i, 1].Value);
                                    if (v_PartNo != null && v_PartNo != "")
                                    {
                                        try
                                        {
                                            iDataSpsStock.Guid = strGUID;
                                            iDataSpsStock.PartNo = v_PartNo;
                                            iDataSpsStock.PartName = Convert.ToString(worksheet.Cells[i, 2].Value);
                                            iDataSpsStock.SupplierNo = Convert.ToString(worksheet.Cells[i, 3].Value);
                                            iDataSpsStock.BackNo = Convert.ToString(worksheet.Cells[i, 4].Value);
                                            iDataSpsStock.SpsRackAddress = Convert.ToString(worksheet.Cells[i, 5].Value);
                                            iDataSpsStock.PcRackAddress = Convert.ToString(worksheet.Cells[i, 6].Value);
                                            iDataSpsStock.RackCapBox = Convert.ToInt32(worksheet.Cells[i, 7].Value);
                                            iDataSpsStock.PcPicKingMember = Convert.ToString(worksheet.Cells[i, 8].Value);                                  
                                            iDataSpsStock.EkbQty = Convert.ToInt32(worksheet.Cells[i, 9].Value);
                                            iDataSpsStock.StockQty = Convert.ToInt32(worksheet.Cells[i, 10].Value);
                                            iDataSpsStock.BoxQty = Convert.ToInt32(worksheet.Cells[i, 11].Value);

                                            iDataSpsStock.Process = Convert.ToString(worksheet.Cells[i, 12].Value);
                                        }
                                        catch (Exception ex)
                                        {
                                            iDataSpsStock.Exception += ex.ToString();
                                        }
                                        rowListSpsStock.Add(iDataSpsStock);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        HSSFWorkbook hssfworkbook = new HSSFWorkbook(stream);
                        sheet = hssfworkbook.GetSheetAt(0); //get first sheet from workbook 
                    }

                }
                // return rowList;
                if (rowList.Count() > 0)
                {
                    _MstLgaEkbPartListGrade.ImportMstLgaEkbPartListGradeFromExcel(rowList);
                }
                if (rowListPcStock.Count() > 0)
                {
                    _MstLgaEkbPartListGrade.ImportLgaPcsStockFromExcel(rowListPcStock);
                }
                if (rowListSpsStock.Count() > 0)
                {
                    _MstLgaEkbPartListGrade.ImportLgaSpsStockFromExcel(rowListSpsStock);
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
