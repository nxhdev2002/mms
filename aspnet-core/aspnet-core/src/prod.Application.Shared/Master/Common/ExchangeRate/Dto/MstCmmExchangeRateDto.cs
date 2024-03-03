using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Master.Common.Dto
{
    public class MstCmmExchangeRateDto : EntityDto<long?>
    {
        public virtual string Guid { get; set; }
        public virtual long? IdT { get; set; }

        public virtual DateTime? ExchangeDate { get; set; }

        public virtual int? Version { get; set; }

        public virtual string MajorCurrency { get; set; }

        public virtual string MinorCurrency { get; set; }

        public virtual decimal? CeilingRate { get; set; }

        public virtual decimal? SvbRate { get; set; }

        public virtual decimal? FloorRate { get; set; }

        public virtual decimal? BuyingOd { get; set; }

        public virtual decimal? BuyingTt { get; set; }

        public virtual decimal? SellingTtOd { get; set; }

        public virtual string IsDownloaded { get; set; }

        public virtual string IsEmailReceived { get; set; }

        public virtual string IsDiff { get; set; }

        public virtual DateTime? DownloadDatetime { get; set; }

        public virtual DateTime? EmailReceiveDatetime { get; set; }

        public virtual long? CheckedBy { get; set; }

        public virtual DateTime? CheckedDatetime { get; set; }

        public virtual string ApprovedBy { get; set; }

        public virtual DateTime? ApprovedDatetime { get; set; }

        public virtual string Status { get; set; }

        public virtual string ToolName { get; set; }

        public virtual string IsActive { get; set; }

    }


    public class GetMstCmmExchangeRateInput : PagedAndSortedResultRequestDto
    {
        public virtual DateTime? ExchangeDateFrom { get; set; }

        public virtual DateTime? ExchangeDateTo { get; set; }

        public virtual string Status { get; set; }

    }

    public class MstCmmExchangeRateDiffDto 
    {
        public virtual DateTime? ExchangeDate { get; set; }

        public virtual int? Version { get; set; }

        public virtual string MajorCurrency { get; set; }

        public virtual string MinorCurrency { get; set; }

        public virtual string CeilingRateNew { get; set; }

        public virtual string SvbRateNew { get; set; }

        public virtual string FloorRateNew { get; set; }

        public virtual string BuyingOdNew { get; set; }

        public virtual string BuyingTtNew { get; set; }

        public virtual string AgvRateNew { get; set; }

        public virtual string SellingTtOdNew { get; set; }

    }

    public class MstCmmExchangeRateDiffDtoInput 
    {
        public virtual string Guid { get; set; }
    }

    public class GetMstCmmExchangeRateHistoryInput : PagedAndSortedResultRequestDto
    {
        public virtual long Id { get; set; }
        public virtual string TableName { get; set; }
    }

    public class GetMstCmmExchangeRateHistoryExcelInput
    {
        public virtual long Id { get; set; }

        public virtual string TableName { get; set; }
    }

}
