using Abp.Application.Services;
using Abp.Application.Services.Dto;
using prod.Inventory.Gps.Issuings.Dto;
using prod.Inventory.GPS.Dto;
using prod.Master.Common.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace prod.Inventory.Gps.Issuings
{
    public interface IInvGpsIssuingsAppService : IApplicationService
    {
        Task<PagedResultDto<InvGpsIssuingsHeaderDto>> GetAll(InvGpsIssuingsHeaderInput input);
        Task<PagedResultDto<InvGpsIssuingsDetails>> GetAllDetails(InvGpsIssuingsDetailsInput input);
        Task<List<GetInvGgsIssuingsImport>> ImportDataInvGpsIssuingsListFromExcel(byte[] fileBytes, string fileName);
        Task<MessageDto> spCheckPartNoExistMaterial(string PartNo, string DocumentNo);
        Task UpdateGpsIssuingDetailQty(GetGpsIssuingDetailInput input);
        Task<MstCmmLookupDto> GetItemValue(string p_DomainCode, string p_ItemCode);
        Task<int> CreateDocumentRequest(string v_Shop, string v_CostCenters, string v_Project);
        Task<int> CreateItemRequest(InvGpsIssuingItemInsert input);
        Task<GetIssuingImportView> GetIssuingImportView(string v_costCenter);
        Task<PagedResultDto<LoggingResponseDetailsOnlBudgetCheckIssuingDto>> GetViewLoggingResponseDetailsOnlBudgetCheckIssuing(GetIF_OnlBudgetCheck_Input input);
        Task<PagedResultDto<GetIF_FundCommitmentItemDMIssuingExportDto>> GetViewFundCommmitmentItemDMIssuing(GetInv_Fundcmm_Item_Issuing_Input input);
    }
}
