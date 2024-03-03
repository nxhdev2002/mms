using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Dapper.Repositories;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.EntityFrameworkCore.Uow;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using AutoMapper.Internal.Mappers;
using Dapper;
using Microsoft.EntityFrameworkCore;
using NUglify.Helpers;
using prod.Authorization;
using prod.Dto;
using prod.Inventory.CKD.Dto;
using prod.Inventory.CKD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using prod;
using prod.Inventory.CKD.Exporting;
using prod.EntityFrameworkCore;
using System.IO;
using GemBox.Spreadsheet;
using System.Data;
using FastMember;
using prod.Common;
using MimeKit.Encodings;

namespace prod.Inventory.CKD
{
      [AbpAuthorize(AppPermissions.Pages_Ckd_Intransit_ContainerTransitPortPlan_View)]
    public class InvCkdContainerTransitPortPlanAppService : prodAppServiceBase, IInvCkdContainerTransitPortPlanAppService
    {
        private readonly IDapperRepository<InvCkdContainerTransitPortPlan, long> _dapperRepo;
        private readonly IRepository<InvCkdContainerTransitPortPlan, long> _repo;
        private readonly IInvCkdContainerTransitPortPlanExcelExporter _calendarListExcelExporter;

        public InvCkdContainerTransitPortPlanAppService(IRepository<InvCkdContainerTransitPortPlan, long> repo,
                                         IDapperRepository<InvCkdContainerTransitPortPlan, long> dapperRepo,
                                        IInvCkdContainerTransitPortPlanExcelExporter calendarListExcelExporter
            )
        {
            _repo = repo;
            _dapperRepo = dapperRepo;
            _calendarListExcelExporter = calendarListExcelExporter;
        }

          [AbpAuthorize(AppPermissions.Pages_Ckd_Intransit_ContainerTransitPortPlan_Edit)]
        public async Task CreateOrEdit(CreateOrEditInvCkdContainerTransitPortPlanDto input)
        {
            if (input.Id == null) await Create(input);
            else await Update(input);
        }

        //CREATE
        private async Task Create(CreateOrEditInvCkdContainerTransitPortPlanDto input)
        {
            var mainObj = ObjectMapper.Map<InvCkdContainerTransitPortPlan>(input);

            await CurrentUnitOfWork.GetDbContext<prodDbContext>().AddAsync(mainObj);
        }

        // EDIT
        private async Task Update(CreateOrEditInvCkdContainerTransitPortPlanDto input)
        {
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.SoftDelete))
            {
                var mainObj = await _repo.GetAll()
                .FirstOrDefaultAsync(e => e.Id == input.Id);

                var mainObjToUpdate = ObjectMapper.Map(input, mainObj);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_Ckd_Intransit_ContainerTransitPortPlan_Edit)]
        public async Task Delete(EntityDto input)
        {
            using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.SoftDelete))
            {
                var mainObj = await _repo.FirstOrDefaultAsync(input.Id);
                _repo.HardDelete(mainObj);
                CurrentUnitOfWork.GetDbContext<prodDbContext>().Remove(mainObj);
            }

        }

        public async Task<int> ConFirmStatus(string p_container_id, string p_status)
		{
			try
			{
				string _sql = @"EXEC INV_CKD_CONTAINER_TRANSIT_PORT_PLAN_UPDATE_STATUS @p_container_id, @p_status, @p_status_date, @p_user_id";
				var filtered = await _dapperRepo.ExecuteAsync(_sql, new
				{
					p_container_id = p_container_id,
					p_status = p_status,
					p_status_date = DateTime.Now,
					p_user_id = AbpSession.UserId
				});
				return filtered;
			}
			catch (Exception E)
			{
				return 0;
			}
		}
		public async Task<int> ConFirmStatusMultiCkb(string listIdStatus, string status)
		{
			try
			{
				string _sql = @"EXEC INV_CKD_CONTAINER_TRANSIT_PORT_PLAN_UPDATE_STATUS @p_list_id_status,@p_status,@p_status_date, @p_user_id";
				var filtered = await _dapperRepo.ExecuteAsync(_sql, new
				{
					p_list_id_status = listIdStatus,
					p_status = status,
					p_status_date = DateTime.Now,
					p_user_id = AbpSession.UserId
				});
				return filtered;
			}
			catch (Exception E)
			{
				return 0;
			}
		}

		

		public async Task<PagedResultDto<InvCkdContainerTransitPortPlanDto>> GetAll(GetInvCkdContainerTransitPortPlanInput input)
        {

            string _sql = "Exec INV_CKD_CONTAINER_TRANSIT_PORT_PLAN_SEARCH @p_container_no, @p_renban, @p_supplier_no, @p_invoice_no, @p_bill_of_lading_no, @p_sealno, " +
                "@p_request_date_from, @p_request_date_to, @p_receive_date_from, @p_receive_date_to, @p_status, @p_OrderTypeCode,@p_lotno,@p_module_case_no ,@p_partno";

            IEnumerable<InvCkdContainerTransitPortPlanDto> result = await _dapperRepo.QueryAsync<InvCkdContainerTransitPortPlanDto>(_sql, new
            {
                p_container_no = input.ContainerNo,
                p_renban = input.Renban,
                p_supplier_no = input.SupplierNo,
                p_invoice_no = input.InvoiceNo,
                p_bill_of_lading_no = input.BillOfLadingNo,
                p_sealno = input.SealNo,
                p_request_date_from = input.RequestDateFrom,
                p_request_date_to = input.RequestDateTo,
                p_receive_date_from = input.ReceiveDateFrom,
                p_receive_date_to = input.ReceiveDateTo,
                p_status = input.Status,
                p_OrderTypeCode = input.OrderTypeCode,
                p_lotno = input.LotNo,
                @p_module_case_no = input.ModuleCaseNo,
                @p_partno = input.PartNo,
            });

            var listResult = result.ToList();

            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var totalCount = result.ToList().Count();

            return new PagedResultDto<InvCkdContainerTransitPortPlanDto>(
                totalCount,
                pagedAndFiltered);
        }


        public async Task<PagedResultDto<InvCkdContainerTransitPortPlanDto>> GetDataLateDate()
        {

            var getlatedate = _repo.GetAll().OrderByDescending(s => s.RequestDate).FirstOrDefault().RequestDate;
            var filtered = _repo.GetAll()
              .Where(e => e.RequestDate == getlatedate);


            var system = from o in filtered
                         select new InvCkdContainerTransitPortPlanDto
                         {
                             Id = o.Id,
                             ContainerNo = o.ContainerNo,
                             Renban = o.Renban,
                             RequestDate = o.RequestDate,
                             RequestTime = o.RequestTime,
                             InvoiceNo = o.InvoiceNo,
                             BillOfLadingNo = o.BillOfLadingNo,
                             SupplierNo = o.SupplierNo,
                             SealNo = o.SealNo,
                             ListCaseNo = o.ListCaseNo,
                             ListLotNo = o.ListLotNo,
                             CdDate = o.CdDate,
                             Transport = o.Transport,
                             Status = o.Status,
                             PortCode = o.PortCode,
                             PortName = o.PortName,
                             Remarks = o.Remarks,
                             IsActive = o.IsActive,
                         };

            var totalCount = await filtered.CountAsync();

            return new PagedResultDto<InvCkdContainerTransitPortPlanDto>(
                totalCount,
                 await system.ToListAsync()
            );
        }




        public async Task<FileDto> GetContainerTransitPortPlanToExcel(GetInvCkdContainerTransitPortPlanInput input)
        {
            string _sql = "Exec INV_CKD_CONTAINER_TRANSIT_PORT_PLAN_SEARCH @p_container_no, @p_renban, @p_supplier_no, @p_invoice_no, @p_bill_of_lading_no, @p_sealno, " +
               "@p_request_date_from, @p_request_date_to, @p_receive_date_from, @p_receive_date_to, @p_status, @p_OrderTypeCode,@p_lotno,@p_module_case_no ,@p_partno";

            IEnumerable<InvCkdContainerTransitPortPlanDto> result = await _dapperRepo.QueryAsync<InvCkdContainerTransitPortPlanDto>(_sql, new
            {
                p_container_no = input.ContainerNo,
                p_renban = input.Renban,
                p_supplier_no = input.SupplierNo,
                p_invoice_no = input.InvoiceNo,
                p_bill_of_lading_no = input.BillOfLadingNo,
                p_sealno = input.SealNo,
                p_request_date_from = input.RequestDateFrom,
                p_request_date_to = input.RequestDateTo,
                p_receive_date_from = input.ReceiveDateFrom,
                p_receive_date_to = input.ReceiveDateTo,
                p_status = input.Status,
                p_OrderTypeCode = input.OrderTypeCode,
                p_lotno = input.LotNo,
                p_module_case_no = input.ModuleCaseNo,
                @p_partno = input.PartNo,
            });

            var listResult = result.ToList();
            return _calendarListExcelExporter.ExportToFile(listResult);
        }

        [AbpAuthorize(AppPermissions.Pages_Ckd_Intransit_ContainerTransitPortPlan_Import)]
        public async Task<List<InvCkdContainerTransitPortPlanDto>> GetImportDataFromExcel(byte[] fileBytes, string fileName)
        {
            try
            {
                List<InvCkdContainerTransitPortPlanDto> listImport = new List<InvCkdContainerTransitPortPlanDto>();
                using (var stream = new MemoryStream(fileBytes))
                {
                    SpreadsheetInfo.SetLicense("EF21-1FW1-HWZF-CLQH");
                    var xlWorkBook = ExcelFile.Load(stream);
                    var v_worksheet = xlWorkBook.Worksheets[0];

                    DateTime dateTime = DateTime.Now;

                    string v_devanning_date = (v_worksheet.Cells[4, 2]).Value?.ToString() ?? "";
                    string strGUID = Guid.NewGuid().ToString("N");
                    string _sql = @"EXEC CMM_IMPORT_USER_GUID_INSERT @Guid, @p_UserId";
                    _dapperRepo.ExecuteAsync(_sql, new
                    {
                        Guid = strGUID,
                        p_UserId = AbpSession.UserId
                    });
                    string v_transport1 = (v_worksheet.Cells[8, 1]).Value?.ToString() ?? "";
                    string v_transport = v_transport1 != "" ? v_transport1.Replace("To ","") : "";
                    for (int i = 15; i < v_worksheet.Rows.Count; i++)
                    {                      
                        var row = new InvCkdContainerTransitPortPlanDto();

                        row.Guid = strGUID;
                        string v_request_date = (v_worksheet.Cells[i, 1]).Value?.ToString() ?? "";
                        string v_request_time = (v_worksheet.Cells[i, 2]).Value?.ToString() ?? "";
                        string v_container_no = (v_worksheet.Cells[i, 3]).Value?.ToString() ?? "";
                        string v_renban = (v_worksheet.Cells[i, 4]).Value?.ToString() ?? "";
                        string v_bill_of_landing_no = (v_worksheet.Cells[i, 5]).Value?.ToString() ?? "";
                     //   string v_lot_no = (v_worksheet.Cells[i, 6]).Value?.ToString() ?? "";
                     //   string v_list_case_no = (v_worksheet.Cells[i, 7]).Value?.ToString() ?? "";
                        string v_source = (v_worksheet.Cells[i, 6]).Value?.ToString() ?? "";
                        string v_custums1 = (v_worksheet.Cells[i, 7]).Value?.ToString() ?? "";
                        string v_custums2 = (v_worksheet.Cells[i, 8]).Value?.ToString() ?? "";
                        string v_invoice_no = (v_worksheet.Cells[i, 9]).Value?.ToString() ?? "";
                       // string v_cd_date = (v_worksheet.Cells[i, 11]).Value?.ToString() ?? "";
                        string v_seal_no = (v_worksheet.Cells[i, 10]).Value?.ToString() ?? "";
                        string v_remarks = (v_worksheet.Cells[i, 11]).Value?.ToString() ?? "";
                       
                        if (v_container_no == "") break;

                        try
                        {
                            var v_time = v_request_time != "" ? v_request_time.Substring(11, 8) : "";
                            row.RequestTime = TimeSpan.Parse(v_time);  
                            
                        } 
                        catch {
                            row.ErrorDescription += "RequestTime không hợp lệ! ";
                        }

                        try
                        {
                            DateTime.Parse(v_request_date);
                            row.RequestDate = DateTime.Parse(v_request_date);
                        } 
                        catch
                        {
                            row.ErrorDescription += "RequestDate không hợp lệ! ";
                        }

                        //try
                        //{
                        //    if(v_cd_date != "")
                        //    {
                        //        DateTime.Parse(v_cd_date);
                        //        row.CdDate = DateTime.Parse(v_cd_date);
                        //    } 
                        //} 
                        //catch
                        //{
                        //    row.ErrorDescription += "CdDate không hợp lệ! ";
                        //}

                        if (v_container_no.Length > 15) row.ErrorDescription += "Độ dài Container No: "+ v_container_no + " không hợp lệ! ";
                        else row.ContainerNo = v_container_no;

                        if (v_renban.Length > 20) row.ErrorDescription += "Độ dài Renban không hợp lệ! ";
                        else row.Renban = v_renban;

                        //if (v_lot_no.Length > 1000) row.ErrorDescription += "Độ dài List Lot No: "+ v_lot_no + " không hợp lệ! ";
                        //row.ListLotNo = v_lot_no;

                        //if (v_list_case_no.Length > 1000) row.ErrorDescription += "Độ dài List Case No: "+ v_list_case_no + " không hợp lệ! ";
                        //row.ListCaseNo = v_list_case_no;

                        if (v_source.Length > 50) row.ErrorDescription += "Độ dài Supplier No: "+ v_source + " không hợp lệ! ";
                        row.SupplierNo = v_source;

                        if (v_custums1.Length > 100) row.ErrorDescription += "Độ dài Custums VP : " + v_custums1 + " không hợp lệ! ";
                        row.Custums1 = v_custums1;

                        if (v_custums2.Length > 100) row.ErrorDescription += "Độ dài Custums HP & Trucking: " + v_custums2 + " không hợp lệ! ";
                        row.Custums2 = v_custums2;

                        if (v_transport.Length > 50) row.ErrorDescription += "Độ dài Transport:" + v_transport + " không hợp lệ! ";
                        else row.Transport = v_transport;

                        if (v_invoice_no.Length > 200) row.ErrorDescription += "Độ dài Invoice No: "+ v_invoice_no + " không hợp lệ! ";
                        row.InvoiceNo = v_invoice_no;

                        if (v_bill_of_landing_no.Length > 200) row.ErrorDescription += "Độ dài Bill Of Lading No: "+ v_bill_of_landing_no + " không hợp lệ! ";
                        else row.BillOfLadingNo = v_bill_of_landing_no;

                        if (v_seal_no.Length > 20) row.ErrorDescription += "Độ dài Seal No:" + v_seal_no + " không hợp lệ! ";
                        else row.SealNo = v_seal_no;

                        if (v_remarks.Length > 200) row.ErrorDescription += "Độ dài Remark: "+ v_remarks + " không hợp lệ! ";
                        row.Remarks = v_remarks;

                        listImport.Add(row);
                    }

                   
                }

                // import temp into db (bulkCopy)
                if (listImport.Count > 0)
                {
                    IEnumerable<InvCkdContainerTransitPortPlanDto> dataE = listImport.AsEnumerable();
                    DataTable table = new DataTable();
                    using (var reader = ObjectReader.Create(dataE))
                    {
                        table.Load(reader);
                    }
                    string connectionString = Commons.getConnectionString();
                    using (Microsoft.Data.SqlClient.SqlConnection conn = new Microsoft.Data.SqlClient.SqlConnection(connectionString))
                    {
                        await conn.OpenAsync();

                        using (Microsoft.Data.SqlClient.SqlTransaction tran = conn.BeginTransaction(IsolationLevel.ReadCommitted))
                        {
                            using (var bulkCopy = new Microsoft.Data.SqlClient.SqlBulkCopy(conn, Microsoft.Data.SqlClient.SqlBulkCopyOptions.Default, tran))
                            {
                                bulkCopy.DestinationTableName = "InvCkdContainerTransitPortPlan_T";
                                bulkCopy.ColumnMappings.Add("Guid", "Guid");
                                bulkCopy.ColumnMappings.Add("RequestDate", "RequestDate");
                                bulkCopy.ColumnMappings.Add("ContainerNo", "ContainerNo");
                                bulkCopy.ColumnMappings.Add("Renban", "Renban");
                                bulkCopy.ColumnMappings.Add("BillOfLadingNo", "BillOfLadingNo");
                              //  bulkCopy.ColumnMappings.Add("ListLotNo", "ListLotNo");
                              //  bulkCopy.ColumnMappings.Add("ListCaseNo", "ListCaseNo");
                                bulkCopy.ColumnMappings.Add("SupplierNo", "SupplierNo");
                                bulkCopy.ColumnMappings.Add("Transport", "Transport");
                                bulkCopy.ColumnMappings.Add("Custums1", "Custums1");
                                bulkCopy.ColumnMappings.Add("Custums2", "Custums2");
                                bulkCopy.ColumnMappings.Add("InvoiceNo", "InvoiceNo");
                              //  bulkCopy.ColumnMappings.Add("CdDate", "CdDate");
                                bulkCopy.ColumnMappings.Add("SealNo", "SealNo");
                                bulkCopy.ColumnMappings.Add("Remarks", "Remarks");
                                bulkCopy.ColumnMappings.Add("RequestTime", "RequestTime");
                                bulkCopy.ColumnMappings.Add("ErrorDescription", "ErrorDescription");
                                bulkCopy.WriteToServer(table);
                                tran.Commit();
                            }
                        }
                        await conn.CloseAsync();
                    }
                }
                return listImport;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(400, ex.Message);
            }
        }

        public async Task MergeDataContainerTransitPortPlan(string v_Guid)
        {
            string _sql = "Exec INV_CKD_CONTAINER_TRANSIT_PORT_PLAN_MERGE @Guid";
            await _dapperRepo.QueryAsync<InvCkdContainerTransitPortPlanDto>(_sql, new { Guid = v_Guid });

        }


        public async Task<PagedResultDto<InvCkdContainerTransitPortPlanDto>> GetMessageErrorImportPortPlan(string v_Guid)
        {
            var data = await _dapperRepo.QueryAsync<InvCkdContainerTransitPortPlanDto>("Exec FRM_ADO_INVENTORY_CKD_PORT_PLAN_GET_LIST_ERROR_IMPORT @Guid", new { @Guid = v_Guid });
            var rsData = from o in data
                         select new InvCkdContainerTransitPortPlanDto
                         {
                             ROW_NO = o.ROW_NO,
                             ContainerNo = o.ContainerNo,
                             Renban = o.Renban,
                             RequestDate = o.RequestDate,
                             RequestTime = o.RequestTime,
                             SealNo = o.SealNo,
                             InvoiceNo = o.InvoiceNo,
                             Transport = o.Transport,
                             Custums1 = o.Custums1,
                             Custums2 = o.Custums2,
                             Remarks = o.Remarks,
                             ErrorDescription = o.ErrorDescription
                         };

            var totalCount = rsData.Count();
            return new PagedResultDto<InvCkdContainerTransitPortPlanDto>(
                totalCount,
                    rsData.ToList()
            );

        }
            public async Task<FileDto> GetListErrContainerTransitPortPlanToExcel(string v_Guid)
            {
                var data = await _dapperRepo.QueryAsync<InvCkdContainerTransitPortPlanDto>("Exec FRM_ADO_INVENTORY_CKD_PORT_PLAN_GET_LIST_ERROR_IMPORT @Guid", new { @Guid = v_Guid });
                var rsData = from o in data
                             select new InvCkdContainerTransitPortPlanDto
                             {
                                 ROW_NO = o.ROW_NO,
                                 ContainerNo = o.ContainerNo,
                                 Renban = o.Renban,
                                 RequestDate = o.RequestDate,
                                 RequestTime = o.RequestTime,
                                 SealNo = o.SealNo,
                                 Transport = o.Transport,
                                 Remarks = o.Remarks,
                                 ErrorDescription = o.ErrorDescription
                             };
                var exportToExcel = rsData.ToList();
                return _calendarListExcelExporter.ExportToFileErr(exportToExcel);
            }


    }
}
