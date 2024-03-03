using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Master.Common
{

    [Table("MstCmmExchangeRate_T1")]
    [Index(nameof(ExchangeDate), Name = "IX_MstCmmExchangeRate_T1_ExchangeDate")]
    [Index(nameof(IsActive), Name = "IX_MstCmmExchangeRate_T1_IsActive")]
    public class MstCmmExchangeRate_T1 : FullAuditedEntity<long>, IEntity<long>
    {

        public const int MaxMajorCurrencyLength = 3;

        public const int MaxMinorCurrencyLength = 3;

        public const int MaxIsDownloadedLength = 1;

        public const int MaxIsEmailReceivedLength = 1;

        public const int MaxIsDiffLength = 1;

        public const int MaxApprovedByLength = 50;

        public const int MaxStatusLength = 20;

        public const int MaxToolNameLength = 50;

        public const int MaxIsActiveLength = 1;

        public virtual DateTime ExchangeDate { get; set; }

        public virtual int Version { get; set; }

        [StringLength(MaxMajorCurrencyLength)]
        public virtual string MajorCurrency { get; set; }

        [StringLength(MaxMinorCurrencyLength)]
        public virtual string MinorCurrency { get; set; }

        public virtual decimal CeilingRate { get; set; }

        public virtual decimal SvbRate { get; set; }

        public virtual decimal FloorRate { get; set; }

        public virtual decimal BuyingOd { get; set; }

        public virtual decimal BuyingTt { get; set; }

        public virtual decimal SellingTtOd { get; set; }

        [StringLength(MaxIsDownloadedLength)]
        public virtual string IsDownloaded { get; set; }

        [StringLength(MaxIsEmailReceivedLength)]
        public virtual string IsEmailReceived { get; set; }

        [StringLength(MaxIsDiffLength)]
        public virtual string IsDiff { get; set; }

        public virtual DateTime DownloadDatetime { get; set; }

        public virtual DateTime EmailReceiveDatetime { get; set; }

        public virtual long CheckedBy { get; set; }

        public virtual DateTime CheckedDatetime { get; set; }

        [StringLength(MaxApprovedByLength)]
        public virtual string ApprovedBy { get; set; }

        public virtual DateTime ApprovedDatetime { get; set; }

        [StringLength(MaxStatusLength)]
        public virtual string Status { get; set; }

        [StringLength(MaxToolNameLength)]
        public virtual string ToolName { get; set; }

        [StringLength(MaxIsActiveLength)]
        public virtual string IsActive { get; set; }
    }
}

