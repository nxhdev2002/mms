using Abp.Application.Services;
using Abp.Dapper.Repositories;
using Abp.Domain.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using prod.Configuration;
using prod.SapIF.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.IO;
using System.Xml;
using prod.SapIF.Enum;
using Abp.EntityFrameworkCore.Uow;
using prod.EntityFrameworkCore;

namespace prod.SapIF
{
    public class SapDMAppService : ApplicationService, ISapDMAppService
    {
        private readonly IConfigurationRoot _appConfiguration;
        private readonly IDapperRepository<SapIFLogging, long> _dapper;
        private readonly IRepository<SapIFLogging, long> _loggingRepo;
        private readonly IRepository<SapIFFundCommitmentItemDM, long> _fundCommitmentItemDMRepo;

        public SapDMAppService(
            IWebHostEnvironment env,
            IDapperRepository<SapIFLogging, long> dapper,
            IRepository<SapIFLogging, long> loggingRepo,
            IRepository<SapIFFundCommitmentItemDM, long> fundCommitmentItemDMRepo)
        {
            _appConfiguration = env.GetAppConfiguration();
            _dapper = dapper;
            _loggingRepo = loggingRepo;
            _fundCommitmentItemDMRepo = fundCommitmentItemDMRepo;
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

        public async Task<SapIFResponseDto<FundCommitmentResponseDto>> SubmitFundCommitmentDMToSap()
        {
            var dbContext = CurrentUnitOfWork.GetDbContext<prodDbContext>();
            var sapIFResponse = new SapIFResponseDto<FundCommitmentResponseDto>();
            var headerDtos = await GetFundCommitmentRequestDM();
            if (headerDtos != null)
            {
                var listLegacyTransferId = new List<long>();
                var itemEntities = await GetFundCommitmentRequestDetailsDM();
                foreach (var headerDto in headerDtos)
                {
                    bool? isHeaderSuccess = null;
                    var requestXml = string.Empty;
                    var response = new FundCommitmentResponseDto();
                    var responseXml = string.Empty;
                    var exception = string.Empty;
                    if (string.IsNullOrWhiteSpace(headerDto.SqlMessage))
                    {
                        try
                        {
                            //request
                            var itemList = new List<FundCommitmentRequestDocumentItem>();
                            foreach (var itemEntity in itemEntities)
                            {
                                if (itemEntity.DocumentNo == headerDto.DocumentNo
                                    && itemEntity.FundCommitmentHeaderType == headerDto.FundCommitmentHeaderType
                                    && itemEntity.Action == headerDto.Action)
                                {
                                    var item = new FundCommitmentRequestDocumentItem();
                                    ObjectMapper.Map(itemEntity, item);
                                    itemList.Add(item);
                                }
                            }
                            var document = new FundCommitmentRequestDocument();
                            ObjectMapper.Map(headerDto, document);
                            document.Items = itemList;
                            var request = new FundCommitmentRequestDto()
                            {
                                Request = new FundCommitmentRequest() { Document = document }
                            };
                            var ns = new XmlQualifiedName("ns0", "http://toyota.com/vn/projectsystem/fund");
                            requestXml = Common.CommonXml.SerialSapXml(request, ns);
                            //response
                            var httpWebRequest = GetHttpWebRequest(_appConfiguration["SapIF:MMSFundCommitment"]);
                            var xmlBytes = System.Text.Encoding.UTF8.GetBytes(requestXml);
                            httpWebRequest.ContentLength = xmlBytes.Length;
                            var networkCredential = new NetworkCredential(_appConfiguration["SapIF:MMSFundCommitmentUser"], _appConfiguration["SapIF:MMSFundCommitmentPass"]);
                            var credentialCache = new CredentialCache();
                            credentialCache.Add(new Uri(_appConfiguration["SapIF:MMSFundCommitment"]), "Basic", networkCredential);
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
                                            response = Common.CommonXml.DeserialSapXml<FundCommitmentResponseDto>(responseXml);
                                            //                        
                                            if (response?.Response?.Documents != null)
                                            {
                                                foreach (var responseDocument in response.Response.Documents)
                                                {
                                                    var itemEntity = itemEntities.Find(o => o.DocumentNo.ToUpper() == responseDocument.DocumentNo?.ToUpper() && o.LineNo.ToUpper() == responseDocument.DocumentLineItemNo?.ToUpper());
                                                    //
                                                    if (itemEntity != null)
                                                    {
                                                        itemEntity.LatestSapTransferMessage = string.Concat(itemEntity.LatestSapTransferMessage, string.IsNullOrWhiteSpace(itemEntity.LatestSapTransferMessage) ? "" : "; ", responseDocument.Message);
                                                        //
                                                        if (responseDocument.MessageType == SapIFMessageType.S.ToString())
                                                        {
                                                            if (string.IsNullOrWhiteSpace(itemEntity.EarmarkedFundsDocument) || string.IsNullOrWhiteSpace(itemEntity.EarmarkedFundsDocumentItem))
                                                            {
                                                                itemEntity.EarmarkedFundsDocument = responseDocument.FundsCommitmentDocument;
                                                                itemEntity.EarmarkedFundsDocumentItem = responseDocument.FundsCommitmentDocumentLineItem;
                                                            }
                                                            //
                                                            if (!isHeaderSuccess.HasValue)
                                                            {
                                                                isHeaderSuccess = true;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            if (!isHeaderSuccess.HasValue || isHeaderSuccess.Value)
                                                            {
                                                                isHeaderSuccess = false;
                                                            }
                                                        }
                                                    }
                                                }
                                            }
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
                        //update item
                        foreach (var itemEntity in itemEntities)
                        {
                            if (itemEntity.DocumentNo == headerDto.DocumentNo
                                && itemEntity.FundCommitmentHeaderType == headerDto.FundCommitmentHeaderType
                                && itemEntity.Action == headerDto.Action)
                            {
                                if (isHeaderSuccess.HasValue && isHeaderSuccess.Value)
                                {
                                    itemEntity.MarkAsSapTransfer = false;
                                    itemEntity.LatestSapSuccessTransferDate = DateTime.Now;
                                }
                                else if (!string.IsNullOrWhiteSpace(exception))
                                {
                                    itemEntity.LatestSapTransferMessage = exception;
                                }
                                //
                                await _fundCommitmentItemDMRepo.UpdateAsync(itemEntity);
                            }
                            else if (headerDto.Id == 0 && !string.IsNullOrWhiteSpace(exception))
                            {
                                itemEntity.LatestSapTransferMessage = exception;
                                //
                                await _fundCommitmentItemDMRepo.UpdateAsync(itemEntity);
                            }
                        }
                        //legacy transfer list
                        if (isHeaderSuccess.HasValue && isHeaderSuccess.Value)
                        {
                            listLegacyTransferId.Add(headerDto.Id);
                        }
                    }
                    else
                    {
                        exception = headerDto.SqlMessage;
                    }
                    //
                    sapIFResponse.SapResponse.Add(new SapIFResponseSapDto<FundCommitmentResponseDto>()
                    {
                        Response = response,
                        Exception = exception
                    });
                    //logging
                    var log = new SapIFLogging()
                    {
                        Type = SapIFType.FundCommitmentDM.ToString(),
                        Request = requestXml,
                        Response = responseXml,
                        Exception = exception,
                        DataType = headerDto.DocumentType,
                        DataId = headerDto.Id
                    };
                    var logId = await _loggingRepo.InsertAndGetIdAsync(log);
                    if (response?.Response?.Documents != null)
                    {
                        var logResponseDetailsList = new List<SapIFLoggingResponseDetailsFundCommitment>();
                        foreach (var responseDocument in response.Response.Documents)
                        {
                            var logResponseDetails = new SapIFLoggingResponseDetailsFundCommitment();
                            ObjectMapper.Map(responseDocument, logResponseDetails);
                            logResponseDetails.LoggingId = logId;
                            logResponseDetailsList.Add(logResponseDetails);
                        }
                        if (logResponseDetailsList?.Count > 0)
                        {
                            await dbContext.AddRangeAsync(logResponseDetailsList);
                        }
                    }
                }
            }
            else
            {
                sapIFResponse.Exception = "Data not found";
            }
            //
            return sapIFResponse;
        }
        #endregion
        #region DB Query
        private async Task<List<SapIFFundCommitmentHeaderDto>> GetFundCommitmentRequestDM()
        {
            var documents = (await _dapper.QueryAsync<SapIFFundCommitmentHeaderDto>(
                "EXEC sp_SapIF_FundCommitment_GetRequestDM"
            )).ToList();
            //
            return documents;
        }
        private async Task<List<SapIFFundCommitmentItemDM>> GetFundCommitmentRequestDetailsDM()
        {
            var items = (await _dapper.QueryAsync<SapIFFundCommitmentItemDM>(
                "EXEC sp_SapIF_FundCommitment_GetRequestDetailsDM"
            )).ToList();
            //
            return items;
        }
        #endregion
    }
}
