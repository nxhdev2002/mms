using Abp.Dapper.Repositories;
using Abp.Domain.Repositories;
using prod.Inventory.Gps.Issuings.Dto;
using prod.Inventory.GPS.Exporting;
using prod.Inventory.GPS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using prod.Inventory.GPS.Dto;
using Abp.Authorization;
using prod.Authorization;
using Twilio.Rest.Api.V2010.Account;
using Abp.UI;
using FastMember;
using GemBox.Spreadsheet;
using NPOI.SS.UserModel;
using prod.Common;
using System.Data;
using System.IO;
using NPOI.Util;
using prod.Master.Common.Dto;
using prod.Inventory.CKD.Dto;
using prod.SapIF;
using prod.Inventory.IF.FQF3MM07.Dto;
using prod.Dto;
using prod.HistoricalData;

namespace prod.Inventory.Gps.Issuings
{
    [AbpAuthorize(AppPermissions.Pages_GPS_Issuings_View)]
    public class InvGpsIssuingsAppService : prodAppServiceBase, IInvGpsIssuingsAppService
    {
        private readonly IDapperRepository<InvGpsIssuing, long> _dapperRepo;
        private readonly IDapperRepository<SapIFLogging, long> _sapIFLoggingRepo;
        private readonly IHistoricalDataAppService _historicalDataAppService;
        private readonly IInvGpsIssuingsExcelExporter _calendarListExcelExporter;

        public InvGpsIssuingsAppService(
                                         IDapperRepository<InvGpsIssuing, long> dapperRepo,
                                         IDapperRepository<SapIFLogging, long> sapIFLoggingRepo,
                                         IHistoricalDataAppService historicalDataAppService,
                                         IInvGpsIssuingsExcelExporter calendarListExcelExporter

            )
        {
            _dapperRepo = dapperRepo;
            _sapIFLoggingRepo = sapIFLoggingRepo;
            _historicalDataAppService = historicalDataAppService;
            _calendarListExcelExporter = calendarListExcelExporter;
        }


        public async Task<List<string>> GetIssuingsHistory(GetInvCkdIssuingsHistoryInput input)
        {
            return await _historicalDataAppService.GetHistoricalDataById(input.Id, input.TableName, "Id");
        }

        public async Task<FileDto> GetHistoricalDataToExcel(GetInvCkdIssuingsHistoryExcelInput input)
        {
            var data = await _historicalDataAppService.GetHistoricalDataById(input.Id, input.TableName, "Id");
            return _calendarListExcelExporter.ExportToHistoricalFile(data);
        }

        public async Task<ChangedRecordIssuingsIdsDto> GetChangedRecords()
        {
            var listHeader = await _historicalDataAppService.GetChangedRecordIds("InvGpsIssuingHeader");
            var listDetail = await _historicalDataAppService.GetChangedRecordIds("InvGpsIssuingDetails");

            ChangedRecordIssuingsIdsDto result = new ChangedRecordIssuingsIdsDto();
            result.IssuingHeader = listHeader;
            result.IssuingDetail = listDetail;
            return result;
        }
        public async Task<PagedResultDto<InvGpsIssuingsHeaderDto>> GetAll(InvGpsIssuingsHeaderInput input)
        {
            string _sql = "Exec INV_GPS_ISSUING_HEADER_SEARCH @p_LoginId, @p_documentno, @p_documentdate ,@p_shop ,@p_team ,@p_costcenter, @p_partno, @p_lotno ,@p_issuedatefrom ,@p_issuedateto ,@p_requestdatefrom , @p_requestdateto, @p_status, @p_today, @p_per_member, @p_per_shop, @p_per_gps ";

            IEnumerable<InvGpsIssuingsHeaderDto> result = await _dapperRepo.QueryAsync<InvGpsIssuingsHeaderDto>(_sql, new
            {
                p_LoginId = AbpSession.UserId,
                p_documentno = input.DocumentNo,
                p_documentdate = input.DocumentDate,
                p_shop = input.Shop,
                p_team = input.Team,
                p_costcenter = input.CostCenter,
                p_partno = input.PartNo,
                p_lotno = input.LotNo,
                p_issuedatefrom = input.IssueFromDate,
                p_issuedateto = input.IssueFromDate,
                p_requestdatefrom = input.RequestFromDate,
                p_requestdateto = input.RequestToDate,
                p_status = input.Status,
                p_today = input.Today,
                p_per_member = input.PerMember,
                p_per_shop = input.PerShop,
                p_per_gps = input.PerGps

            });

            var listResult = result.ToList();

            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var totalCount = result.ToList().Count();
            var UserId = AbpSession.UserId;

            return new PagedResultDto<InvGpsIssuingsHeaderDto>(
               totalCount,
               pagedAndFiltered
        
               );
        }

        public async Task<PagedResultDto<InvGpsIssuingsDetails>> GetAllDetails(InvGpsIssuingsDetailsInput input)
        {
            string _sql = "Exec INV_GPS_ISSUING_DETAILS_SEARCH @p_issuingheaderId, @p_partno, @p_lotno ,@p_issuedatefrom ,@p_issuedateto ,@p_requestdatefrom , @p_requestdateto, @p_per_member, @p_per_shop, @p_per_gps";

            IEnumerable<InvGpsIssuingsDetails> result = await _dapperRepo.QueryAsync<InvGpsIssuingsDetails>(_sql, new
            {
                p_issuingheaderId = input.IssuingHeaderId,
                p_partno = input.PartNo,
                p_lotno = input.LotNo,
                p_issuedatefrom = input.IssueFromDate,
                p_issuedateto = input.IssueFromDate,
                p_requestdatefrom = input.RequestFromDate,
                p_requestdateto = input.RequestToDate,
                p_per_member = input.PerMember,
                p_per_shop = input.PerShop,
                p_per_gps = input.PerGps

            });

            var listResult = result.ToList();

            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var totalCount = result.ToList().Count();

            return new PagedResultDto<InvGpsIssuingsDetails>(
               totalCount,
               pagedAndFiltered);
        }

        public async Task<int> CreateDocumentRequest(string v_Shop, string v_CostCenters, string v_Project)
        {
            try
            {
                string _sql = "Exec INV_GPS_ISSUING_DOCUMNET_REQUEST @p_LoginId, @p_Shop, @p_CostCenter, @v_Project";

                var filtered = await _dapperRepo.ExecuteAsync(_sql, new
                {
                    p_LoginId = AbpSession.UserId,
                    p_Shop = v_Shop,
                    p_CostCenter = v_CostCenters,
                    v_Project = v_Project
                });
                return filtered;
            }
            catch (Exception E)
            {
                return 0;
            }
        }

        public async Task<int> CreateItemRequest(InvGpsIssuingItemInsert input)
        {
            try
            {
                var v_partno = (input.PartNo.Length == 11 ? input.PartNo.ToString() + ".00" : input.PartNo);
                string _sql = "Exec INV_GPS_ISSUING_REQUEST @p_LoginId, @p_PartNo ,@p_QtyRequest, @p_IssuingHeaderId, @p_DocumentDate, @p_DocumentNo, @p_Shop , @p_Wbs, @p_WbsMapping, @p_CostCenter, @p_CostCenterMapping, @p_GlAccount, @p_IsBudgetCheck";

                var filtered = await _dapperRepo.ExecuteAsync(_sql, new
                {
                    p_LoginId = AbpSession.UserId,
                    p_PartNo = v_partno,
                    p_QtyRequest = input.QtyRequest,
                    p_IssuingHeaderId = input.IssuingHeaderId,
                    p_DocumentDate = input.DocumentDate,
                    p_DocumentNo = input.DocumentNo,
                    p_Shop = input.Shop,
                    p_Wbs = input.Wbs,
                    p_WbsMapping = input.WbsMapping,                   
                    p_CostCenter = input.CostCenter,
                    p_CostCenterMapping =  input.CostCenterMapping,
                    p_GlAccount =   input.GlAccount,
                    p_IsBudgetCheck = input.IsBudgetCheck

                });
                return filtered;
            }
            catch (Exception E)
            {
                return 0;
            }

        }

        public async Task<List<GetInvGgsIssuingsImport>> ImportDataInvGpsIssuingsListFromExcel(byte[] fileBytes, string fileName)
        {
            try
            {
                List<GetInvGgsIssuingsImport> listImport = new List<GetInvGgsIssuingsImport>();
                List<string> partNos = new List<string>();
                using (var stream = new MemoryStream(fileBytes))
                {
                    SpreadsheetInfo.SetLicense("EF21-1FW1-HWZF-CLQH");
                    var xlWorkBook = ExcelFile.Load(stream);
                    var v_worksheet = xlWorkBook.Worksheets[0];
                    DataFormatter formatter = new DataFormatter();
                    DateTime dateTime = DateTime.Now;
                    string strGUID = Guid.NewGuid().ToString("N");
                    
                    for (int i = 3; i < v_worksheet.Rows.Count; i++)
                    {

                        string v_PartNo = (v_worksheet.Cells[i, 1]).Value?.ToString() ?? "";

                        if (v_PartNo != "")
                        {


                            string v_QtyRequest = (v_worksheet.Cells[i, 2]).Value?.ToString() ?? "";

                            GetInvGgsIssuingsImport row = new GetInvGgsIssuingsImport();
                            row.Guid = strGUID;

                            //PartNo
                            if (v_PartNo.Length > 14)
                            {
                                row.ErrorDescription += "PartNo " + v_PartNo + " dài quá 14 kí tự! ";
                            }
                            else
                            {
                                row.PartNo = v_PartNo;
                            }

                            /// check if partno is exist in list partno
                            if (partNos.Contains(v_PartNo))
                            {
                                row.ErrorDescription += "PartNo: " + v_PartNo + " bị trùng ở dòng số " + (i + 1);
                            } else
                            {
                                partNos.Add(v_PartNo);
                            }

                            //QtyRequest
                            try
                            {
                                if (string.IsNullOrEmpty(v_QtyRequest))
                                {
                                    row.ErrorDescription += "QtyRequest không được để trống!";
                                }
                                else
                                {

                                    if (Convert.ToInt32(v_QtyRequest) < 0)
                                    {
                                        row.ErrorDescription += "QtyRequest phải là số dương! ";
                                    }
                                    row.QtyRequest = Convert.ToInt32(v_QtyRequest);


                                }
                            }
                            catch (Exception ex)
                            {
                                row.ErrorDescription += "QtyRequest không phải là số! ";
                            }
                            listImport.Add(row);


                        }

                    }
                    // import temp into db (bulkCopy)
                    if (listImport.Count > 0)
                    {
                        IEnumerable<GetInvGgsIssuingsImport> dataE = listImport.AsEnumerable();
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
                                    bulkCopy.DestinationTableName = "InvGpsIssuing_T";
                                    bulkCopy.ColumnMappings.Add("Guid", "Guid");
                                    bulkCopy.ColumnMappings.Add("PartNo", "PartNo");
                                    bulkCopy.ColumnMappings.Add("QtyRequest", "QtyRequest");
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
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(400, ex.Message);
            }
        }

        public async Task MergeDataInvGpsIssuing(string v_Guid, string v_CostCenter)
        {
               string _sql = "Exec INV_GPS_ISSUINGS_REQUEST_MERGE @p_Guid, @p_UserId, @p_CostCenter";
               await _dapperRepo.QueryAsync<GetInvGgsIssuingsImport>(_sql, new 
               { 
                   p_Guid = v_Guid , 
                   p_UserId  = AbpSession.UserId,
                   p_CostCenter = v_CostCenter
               });    
        }

        public async Task<PagedResultDto<GetInvGesIssuingImport>> GetMessageErrorImport(string v_Guid)
        {
            string _sql = "Exec INV_GPS_ISSUE_GET_LIST_ERROR_IMPORT @Guid";

            IEnumerable<GetInvGesIssuingImport> result = await _dapperRepo.QueryAsync<GetInvGesIssuingImport>(_sql, new
            {
                Guid = v_Guid
            });

            var listResult = result.ToList();
            var totalCount = listResult.Count();

            return new PagedResultDto<GetInvGesIssuingImport>(
                totalCount,
               listResult
               );
        }

        public async Task GpsIssuingRequestCreate(GetGpsIssuingRequetCreateInput input)
        {
            string _sql = "Exec INV_GPS_ISSUING_REQUEST_CREATE @p_Id, @p_qtyRequest, @p_budgetCheckMes, @p_fundCommitmentMes";

            IEnumerable<GetInvGesIssuingImport> result = await _dapperRepo.QueryAsync<GetInvGesIssuingImport>(_sql, new
            {
                p_Id = input.HeaderId,
                p_qtyRequest = input.QtyRequest,
                p_budgetCheckMes = input.BudgetCheckMessage ?? null,
                p_fundCommitmentMes = input.FundCommitmentMessage ?? null,
            });

        }

        public async Task UpdateGpsIssuingDetailQty(GetGpsIssuingDetailInput input)
        {
            string _sql = "Exec INV_GPS_ISSUING_DETAILS_QTY_UPDATE  @p_DetailsId, @p_qtyRequest, @p_qtyReject, @p_qtyIssue, @p_stausItem, @p_budgetCheckMes, @p_fundCommitmentMes, @p_QtyRemain";

            IEnumerable<GetInvGesIssuingImport> result = await _dapperRepo.QueryAsync<GetInvGesIssuingImport>(_sql, new
            {
                p_detailsId = input.IdDetails,
                p_qtyRequest = input.QtyRequest ?? null,
                p_qtyReject = input.QtyReject ?? null,
                p_qtyIssue = input.QtyIssue ?? null,
                p_stausItem = input.StatusItem ?? null,
                p_budgetCheckMes = input.BudgetCheckMessage ?? null,
                p_fundCommitmentMes = input.FundCommitmentMessage ?? null,
                p_QtyRemain = input.QtyRemain ?? null
            });
        }

        public async Task GpsIssuingChangeStatus(int v_IssuingHeaderId, string v_Status)
        {
            string _sql = "Exec INV_GPS_ISSUING_CHANGE_STATUS @p_issuingHeaderId, @p_status";

            IEnumerable<GetInvGesIssuingImport> result = await _dapperRepo.QueryAsync<GetInvGesIssuingImport>(_sql, new
            {
                p_issuingHeaderId = v_IssuingHeaderId,
                p_status = v_Status
            });
        }

        public async Task<MessageDto> spCheckPartNoExistMaterial(string PartNo,string DocumentNo)
        {
            string _sql = "Exec INV_GPS_ISSUING_CHECK_EXIST_PART_IN_MATERIAL @p_partNo,@p_documentNo ";
            IEnumerable<MessageDto> result = await _dapperRepo.QueryAsync<MessageDto>(_sql, new
            {
                p_partNo = PartNo,
                p_documentNo = DocumentNo
            });

            return result.FirstOrDefault();
        }




        public async Task<List<GetCostCenter>> GetListCostCenter()
        {
            string _sql = "Exec INV_GPS_COSTCENTER_BY_USER @p_UserId ";

            IEnumerable<GetCostCenter> result = await _dapperRepo.QueryAsync<GetCostCenter>(_sql, new
            {
                p_UserId = AbpSession.UserId
            });

            return result.ToList();


        }

        public async Task<List<GetPartNo>> GetListPartNo()
        {
            IEnumerable<GetPartNo> result = await _dapperRepo.QueryAsync<GetPartNo>("SELECT DISTINCT PartNo FROM InvGpsMaterial");
            return result.ToList();
        }

        public async Task<GetIssuingImportView> GetIssuingImportView(string v_costCenter)
        {
            IEnumerable<GetIssuingImportView> result = await _dapperRepo.QueryAsync<GetIssuingImportView>(@"
            exec [INV_GPS_ISSUING_IMPORT_VIEW_DEFAULT] @p_LoginId, @v_CostCenter", new
            {
                p_LoginId = AbpSession.UserId,
                v_CostCenter = v_costCenter
            });

            return result.FirstOrDefault();
        }

        public async Task<MstCmmLookupDto> GetItemValue(string p_DomainCode, string p_ItemCode)
        {
            string _sql = "Exec MST_CMN_LOOKUP_GET_ITEM_VALUE @DomainCode, @ItemCode";
            var filtered = await _dapperRepo.QueryAsync<MstCmmLookupDto>(_sql, new
            {
                DomainCode = p_DomainCode,
                ItemCode = p_ItemCode
            });

            return filtered.FirstOrDefault();
        }


        public async Task<PagedResultDto<LoggingResponseDetailsOnlBudgetCheckIssuingDto>> GetViewLoggingResponseDetailsOnlBudgetCheckIssuing(GetIF_OnlBudgetCheck_Input input)
        {
            string _sql = "Exec INV_LOGGING_RESPONSE_ONL_BUDGET_CHECK_BY_DOCUMENTNO @p_DocumentNo";

            IEnumerable<LoggingResponseDetailsOnlBudgetCheckIssuingDto> result = await _dapperRepo.QueryAsync<LoggingResponseDetailsOnlBudgetCheckIssuingDto>(_sql, new {
                p_DocumentNo = input.p_DocumentNo
            });

            var listResult = result.ToList();

            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var totalCount = result.ToList().Count();

            return new PagedResultDto<LoggingResponseDetailsOnlBudgetCheckIssuingDto>(
                totalCount,
                 pagedAndFiltered
            );
        }

        public async Task<PagedResultDto<GetIF_FundCommitmentItemDMIssuingExportDto>> GetViewFundCommmitmentItemDMIssuing(GetInv_Fundcmm_Item_Issuing_Input input)
        {
            string _sql = "Exec [INV_FUND_COMMITMENT_ITEM_DM_ISSUING_SEARCH] @p_id";

            IEnumerable<GetIF_FundCommitmentItemDMIssuingExportDto> result = await _dapperRepo.QueryAsync<GetIF_FundCommitmentItemDMIssuingExportDto>(_sql, new
            {
                p_id = input.IdFund
            });

            var listResult = result.ToList();

            var pagedAndFiltered = listResult.Skip(input.SkipCount).Take(input.MaxResultCount).ToList();

            var totalCount = result.ToList().Count();

            return new PagedResultDto<GetIF_FundCommitmentItemDMIssuingExportDto>(
                totalCount,
                 pagedAndFiltered
            );
        }

        public async Task<GetRequestBudgetCheckIssuing> GetViewRequestBudgetCheck(long logingId)
        {
            IEnumerable<GetRequestBudgetCheckIssuing> result = await _sapIFLoggingRepo.QueryAsync<GetRequestBudgetCheckIssuing>(@"
                exec [dbo].[INV_VIEW_REQUEST_BUDGET_CHECK] @id_Logging
            ", new
            {
                id_Logging = logingId,
            });

            return result.FirstOrDefault();
        }

        public async Task<GetRequestBudgetCheckIssuing> GetViewRequestFuncommitment(long logingId)
        {
            IEnumerable<GetRequestBudgetCheckIssuing> result = await _sapIFLoggingRepo.QueryAsync<GetRequestBudgetCheckIssuing>(@"
                exec [dbo].[INV_VIEW_REQUEST_FUN_COMMITMENT] @id_Logging
            ", new
            {
                id_Logging = logingId,
            });

            return result.FirstOrDefault();
        }


        public async Task<GetNewItemRequestValidateDto> GetNewItemRequestValidate(GetNewItemRequestValidateInputDto input)
        {
            string _sql = "Exec INV_GPS_ISSUING_NEW_ITEM_REQUEST_VALIDATES @p_DocumentNo, @p_PartNo, @p_CostCenter, @p_Shop, @p_UserId";
            return (await _dapperRepo.QueryAsync<GetNewItemRequestValidateDto>(_sql, new
            {
                p_DocumentNo = input.DocumentNo,
                p_PartNo = input.PartNo,
                p_CostCenter = input.CostCenter,
                p_Shop = input.Shop,
                p_UserId = AbpSession.UserId
            })).FirstOrDefault();
        }
    }
}
