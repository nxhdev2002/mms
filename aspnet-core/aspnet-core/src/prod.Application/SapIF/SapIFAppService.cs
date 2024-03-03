using Abp.EntityFrameworkCore.Uow;
using Abp.Application.Services;
using Abp.Dapper.Repositories;
using Abp.Domain.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using prod.EntityFrameworkCore;
using prod.SapIF.Dto;
using prod.SapIF.Enum;
using System;
using System.Net;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using prod.Configuration;
using System.IO;
using PayPalCheckoutSdk.Orders;
using Org.BouncyCastle.Crypto;
using prod.Inventory.CKD.Dto;

namespace prod.SapIF
{
    public class SapIFAppService : ApplicationService, ISapIFAppService
    {
        private readonly IWebHostEnvironment _env;
        private readonly IConfigurationRoot _appConfiguration;
        private readonly IDapperRepository<SapIFLogging, long> _dapper;
        private readonly IRepository<SapIFLogging, long> _loggingRepo;
        private readonly IRepository<SapIFLoggingResponseDetailsOnlineBudgetCheck, long> _loggingResponseDetailsOnlineBudgetCheckRepo;
        private readonly IRepository<SapIFLoggingResponseDetailsFundCommitment, long> _loggingResponseDetailsFundCommitmentRepo;
        private readonly IRepository<SapIFFundCommitmentHeader, long> _fundCommitmentHeaderRepo;
        private readonly IRepository<SapIFFundCommitmentItem, long> _fundCommitmentItemRepo;
        private readonly ISapDMAppService _ISapDMAppService;
        public SapIFAppService(
            IWebHostEnvironment env,
            IDapperRepository<SapIFLogging, long> dapper,
            IRepository<SapIFLogging, long> loggingRepo,
            IRepository<SapIFLoggingResponseDetailsOnlineBudgetCheck, long> loggingResponseDetailsOnlineBudgetCheckRepo,
            IRepository<SapIFLoggingResponseDetailsFundCommitment, long> loggingResponseDetailsFundCommitmentRepo,
            IRepository<SapIFFundCommitmentHeader, long> fundCommitmentHeaderRepo,
            IRepository<SapIFFundCommitmentItem, long> fundCommitmentItemRepo,
            ISapDMAppService ISapDMAppService
            )
        {
            _env = env;
            _appConfiguration = env.GetAppConfiguration();
            _dapper = dapper;
            _loggingRepo = loggingRepo;
            _loggingResponseDetailsOnlineBudgetCheckRepo = loggingResponseDetailsOnlineBudgetCheckRepo;
            _loggingResponseDetailsFundCommitmentRepo = loggingResponseDetailsFundCommitmentRepo;
            _fundCommitmentHeaderRepo = fundCommitmentHeaderRepo;
            _fundCommitmentItemRepo = fundCommitmentItemRepo;
            _ISapDMAppService = ISapDMAppService;
        }

        #region Sap IF
        private static HttpWebRequest GetHttpWebRequest(string url)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.ContentType = "text/xml; encoding='utf-8'";
            httpWebRequest.Method = "POST";
            httpWebRequest.PreAuthenticate = true;
            return httpWebRequest;
        }
        public async Task<OnlineBudgetCheckResponseDto> SapOnlineBudgetCheck(OnlineBudgetCheckRequest document)
        {
            var response = new OnlineBudgetCheckResponseDto();
            var request = new OnlineBudgetCheckRequestDto();
            var requestXml = string.Empty;
            var responseXml = string.Empty;
            var exception = string.Empty;
            try
            {
                //request                
                request.Request = document;
                var ns = new XmlQualifiedName("ns0", "http://toyota.com/vn/projectsystem/budget");
                requestXml =  Common.CommonXml.SerialSapXml(request, ns);
                //response                
                var httpWebRequest = GetHttpWebRequest(_appConfiguration["SapIF:MMSBudgetCheck"]);
                var xmlBytes = System.Text.Encoding.UTF8.GetBytes(requestXml);
                httpWebRequest.ContentLength = xmlBytes.Length;
                var networkCredential = new NetworkCredential(_appConfiguration["SapIF:MMSBudgetCheckUser"], _appConfiguration["SapIF:MMSBudgetCheckPass"]);
                var credentialCache = new CredentialCache();
                credentialCache.Add(new Uri(_appConfiguration["SapIF:MMSBudgetCheck"]), "Basic", networkCredential);
                httpWebRequest.Credentials = credentialCache;
                //ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;
                using (var requestStream = httpWebRequest.GetRequestStream())
                {
                    requestStream.Write(xmlBytes, 0, xmlBytes.Length);
                    using (var httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse())
                    {
                        if (httpWebResponse.StatusCode == HttpStatusCode.OK)
                        {
                            using (var responseStream = httpWebResponse.GetResponseStream())
                            {
                                responseXml = new StreamReader(responseStream).ReadToEnd();
                                response = Common.CommonXml.DeserialSapXml<OnlineBudgetCheckResponseDto>(responseXml);
                            }
                        }
                        else
                        {
                            exception = httpWebResponse.StatusDescription;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                 exception = string.Concat(ex.Message, Environment.NewLine, ex.StackTrace);
            }
            //logging
            var log = new SapIFLogging()
            {
                Type = SapIFType.OnlineBudgetCheck.ToString(),
                Request = requestXml,
                Response = responseXml,
                Exception = exception
            };
            var logId = await _loggingRepo.InsertAndGetIdAsync(log);
            if (response?.Response != null)
            {
                for(int i = 0; i < document.ListItemId.Count; i++)
                {
                    var logResponseDetails = new SapIFLoggingResponseDetailsOnlineBudgetCheck();
                    logResponseDetails.ItemId = document.ListItemId[i];
                    if (response.Response.AvailableBudget != null)
                    {
                        ObjectMapper.Map(response.Response.AvailableBudget, logResponseDetails);
                    }
                    if (response.Response.DataValidation != null)
                    {
                        ObjectMapper.Map(response.Response.DataValidation, logResponseDetails);
                    }
                    logResponseDetails.LoggingId = logId;
                    await _loggingResponseDetailsOnlineBudgetCheckRepo.InsertAsync(logResponseDetails);
                }
              
            }
            //
            return (response != null) ? response: new OnlineBudgetCheckResponseDto();
        }

        public async Task SubmitFundCommitmentRequest(string type, List<long> listId, string v_action)
        {
            var ids = string.Join(",", listId);
            string _merge = "EXEC sp_SapIF_FundCommitmentDM_GetRequest_MMS @p_type, @p_ids, @p_is_action, @p_user_id";
            await _dapper.QueryAsync<ImportCkdPartListDto>(_merge, new
            {
                @p_type = type,
                @p_ids = ids,
                @p_is_action = v_action,
                @p_user_id = AbpSession.UserId
            });           
            await _ISapDMAppService.SubmitFundCommitmentDMToSap();

           
        }


        public async Task<SapIFResponseDto<FundCommitmentResponseDto>> SubmitFundCommitmentToSap(string type, List<long> listId, string v_action, bool? isClosed = null)
        {
            var dbContext = CurrentUnitOfWork.GetDbContext<prodDbContext>();
            var sapIFResponse = new SapIFResponseDto<FundCommitmentResponseDto>();
            var ids = string.Join(",", listId);
            var headerDtos = await GetFundCommitmentRequest(type, v_action, ids, isClosed);
            if (headerDtos != null)
            {
                var listLegacyTransferId = new List<long>();
                var itemEntities = await GetFundCommitmentRequestDetails(type, ids);
                //foreach (var headerDto in headerDtos)
                //{
                //    bool? isHeaderSuccess = null;
                //    var requestXml = string.Empty;
                //    var response = new FundCommitmentResponseDto();
                //    var responseXml = string.Empty;
                //    var exception = string.Empty;
                //    if (string.IsNullOrWhiteSpace(headerDto.SqlMessage))
                //    {
                //        try
                //        {
                //            //request
                //            var itemList = new List<FundCommitmentRequestDocumentItem>();
                //            foreach (var itemEntity in itemEntities)
                //            {
                //                if (itemEntity.DocumentNo == headerDto.DocumentNo && itemEntity.Action == headerDto.Action)
                //                {
                //                    var item = new FundCommitmentRequestDocumentItem();
                //                    ObjectMapper.Map(itemEntity, item);
                //                    itemList.Add(item);
                //                }
                //            }
                //            var document = new FundCommitmentRequestDocument();
                //            ObjectMapper.Map(headerDto, document);
                //            document.Items = itemList;
                //            var request = new FundCommitmentRequestDto()
                //            {
                //                Request = new FundCommitmentRequest() { Document = document }
                //            };
                //            var ns = new XmlQualifiedName("ns0", "http://toyota.com/vn/projectsystem/fund");
                //            requestXml = Common.CommonXml.SerialSapXml(request, ns);
                //            //response
                //            var httpWebRequest = GetHttpWebRequest(_appConfiguration["SapIF:MMSFundCommitment"]);
                //            var xmlBytes = System.Text.Encoding.UTF8.GetBytes(requestXml);
                //            httpWebRequest.ContentLength = xmlBytes.Length;
                //            var networkCredential = new NetworkCredential(_appConfiguration["SapIF:MMSFundCommitmentUser"], _appConfiguration["SapIF:MMSFundCommitmentPass"]);
                //            var credentialCache = new CredentialCache();
                //            credentialCache.Add(new Uri(_appConfiguration["SapIF:MMSFundCommitment"]), "Basic", networkCredential);
                //            httpWebRequest.Credentials = credentialCache;
                //            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;
                //            using (var requestStream = httpWebRequest.GetRequestStream())
                //            {
                //                requestStream.Write(xmlBytes, 0, xmlBytes.Length);
                //                using (var httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse())
                //                {
                //                    if (httpWebResponse.StatusCode == HttpStatusCode.OK)
                //                    {
                //                        using (var responseStream = httpWebResponse.GetResponseStream())
                //                        {
                //                            responseXml = new StreamReader(responseStream).ReadToEnd();
                //                            response = Common.CommonXml.DeserialSapXml<FundCommitmentResponseDto>(responseXml);
                //                            //                        
                //                            if (response?.Response?.Documents != null)
                //                            {
                //                                foreach (var responseDocument in response.Response.Documents)
                //                                {
                //                                    var itemEntity = itemEntities.Find(o => o.DocumentNo.ToUpper() == responseDocument.DocumentNo?.ToUpper() && o.LineNo.ToUpper() == responseDocument.DocumentLineItemNo?.ToUpper());
                //                                    //
                //                                    if (itemEntity != null)
                //                                    {
                //                                        itemEntity.LatestSapTransferMessage = string.Concat(itemEntity.LatestSapTransferMessage, string.IsNullOrWhiteSpace(itemEntity.LatestSapTransferMessage) ? "" : "; ", responseDocument.Message);
                //                                        //
                //                                        if (responseDocument.MessageType == SapIFMessageType.S.ToString())
                //                                        {
                //                                            if (string.IsNullOrWhiteSpace(itemEntity.EarmarkedFundsDocument) || string.IsNullOrWhiteSpace(itemEntity.EarmarkedFundsDocumentItem))
                //                                            {
                //                                                itemEntity.EarmarkedFundsDocument = responseDocument.FundsCommitmentDocument;
                //                                                itemEntity.EarmarkedFundsDocumentItem = responseDocument.FundsCommitmentDocumentLineItem;
                //                                            }
                //                                            //
                //                                            if (isHeaderSuccess == null)
                //                                            {
                //                                                isHeaderSuccess = true;
                //                                            }
                //                                        }
                //                                        else
                //                                        {
                //                                            if (isHeaderSuccess == null || isHeaderSuccess.Value)
                //                                            {
                //                                                isHeaderSuccess = false;
                //                                            }
                //                                        }
                //                                    }
                //                                }
                //                            }
                //                        }
                //                    }
                //                    else
                //                    {
                //                        exception = httpWebResponse.StatusDescription;
                //                    }
                //                }
                //            }
                //        }
                //        catch (Exception ex)
                //        {
                //            exception = string.Concat(ex.Message, Environment.NewLine, ex.StackTrace);
                //        }
                //        //update item
                //        foreach (var itemEntity in itemEntities)
                //        {
                //            if (itemEntity.DocumentNo == headerDto.DocumentNo && itemEntity.Action == headerDto.Action)
                //            {
                //                if (isHeaderSuccess.Value)
                //                {
                //                    itemEntity.MarkAsSapTransfer = false;
                //                    itemEntity.LatestSapSuccessTransferDate = DateTime.Now;
                //                }
                //                else if (!string.IsNullOrWhiteSpace(exception))
                //                {
                //                    itemEntity.LatestSapTransferMessage = exception;
                //                }
                //                //
                //                await _fundCommitmentItemRepo.UpdateAsync(itemEntity);
                //            }
                //            else if (headerDto.Id == 0 && !string.IsNullOrWhiteSpace(exception))
                //            {
                //                itemEntity.LatestSapTransferMessage = exception;
                //                //
                //                await _fundCommitmentItemRepo.UpdateAsync(itemEntity);
                //            }
                //        }
                //        //legacy transfer list
                //        if (isHeaderSuccess.Value)
                //        {
                //            listLegacyTransferId.Add(headerDto.Id);
                //        }
                //    }
                //    else
                //    {
                //        exception = headerDto.SqlMessage;
                //    }
                //    //
                //    sapIFResponse.SapResponse.Add(new SapIFResponseSapDto<FundCommitmentResponseDto>()
                //    {
                //        Response = response,
                //        Exception = exception
                //    });
                //    //logging
                //    var log = new SapIFLogging()
                //    {
                //        Type = SapIFType.FundCommitment.ToString(),
                //        Request = requestXml,
                //        Response = responseXml,
                //        Exception = exception,
                //        DataType = type,
                //        DataId = headerDto.Id
                //    };
                //    var logId = await _loggingRepo.InsertAndGetIdAsync(log);
                //    if (response?.Response?.Documents != null)
                //    {
                //        for (int i = 0; i < listId.Count; i++)
                //        {
                //            var logResponseDetailsList = new List<SapIFLoggingResponseDetailsFundCommitment>();
                //            foreach (var responseDocument in response.Response.Documents)
                //            {
                //                var logResponseDetails = new SapIFLoggingResponseDetailsFundCommitment();
                //                ObjectMapper.Map(responseDocument, logResponseDetails);
                //                logResponseDetails.LoggingId = logId;
                //                logResponseDetails.ItemId = listId[i];
                //                logResponseDetailsList.Add(logResponseDetails);
                //            }
                //            if (logResponseDetailsList?.Count > 0)
                //            {
                //                await dbContext.AddRangeAsync(logResponseDetailsList);
                //            }
                //        }

                //    }
                //}
                ////legacy transfer
                //if (listLegacyTransferId.Count > 0)
                //{
                //    await dbContext.SaveChangesAsync();
                //    sapIFResponse.LegacyResponse = await LegacyTransfer(type, listLegacyTransferId);
                //}
            }
            else
            {
                sapIFResponse.Exception = "Data not found";
            }
            //
            return sapIFResponse;
        }
        #endregion
        #region DB Store
        private async Task<List<SapIFFundCommitmentHeaderDto>> GetFundCommitmentRequest(string type, string action, string ids, bool? isClosed)
        {
            var documents = (await _dapper.QueryAsync<SapIFFundCommitmentHeaderDto>(
                "EXEC sp_SapIF_FundCommitmentDM_GetRequest_MMS @p_type, @p_ids, @p_is_action, @p_user_id",
                new
                {
                    @p_type = type,
                    @p_ids = ids,
                    @p_is_action = action,
                    @p_user_id = AbpSession.UserId
                }
            )).ToList();
            //
            return documents;
        }

        private async Task<List<SapIFFundCommitmentItem>> GetFundCommitmentRequestDetails(string type, string ids)
        {
            var items = (await _dapper.QueryAsync<SapIFFundCommitmentItem>(
                "EXEC sp_SapIF_FundCommitmentDM_GetRequestDetails_MMS @p_type, @p_ids",
                new
                {
                    @p_type = type,
                    @p_ids = ids
                }
            )).ToList();
            //
            return items;
        }

        private async Task<string> LegacyTransfer(string type, List<long> listId)
        {
            var response = (await _dapper.QueryAsync<string>(
                "EXEC sp_SapIF_FundCommitment_LegacyTransfer_MMS @p_type, @p_ids, @p_user_id",
                new
                {
                    @p_type = type,
                    @p_ids = string.Join(",", listId),
                    @p_user_id = AbpSession.UserId
                }
            )).FirstOrDefault();
            //
            return response;
        }
        #endregion
    }
}
