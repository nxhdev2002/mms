using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace prod.Master.Inv.Dto
{
    public class MstInvGpsSupplierOrderTimeDto: EntityDto<long?>
    {
        public virtual long? SupplierId { get; set; }

        public virtual int? OrderSeq { get; set; }

        public virtual string OrderType { get; set; }

        public virtual TimeSpan? OrderTime { get; set; }

        public virtual int? ReceivingDay { get; set; }

        public virtual TimeSpan? ReceiveTime { get; set; }

        public virtual TimeSpan? KeihenTime { get; set; }

        public virtual int? KeihenDay { get; set; }

        public virtual string IsActive { get; set; }
    }

    public class CreateOrEditMstInvGpsSupplierOrderTimeDto : EntityDto<long?>
    {

        public virtual long? SupplierId { get; set; }

        public virtual int? OrderSeq { get; set; }

        [StringLength(MstInvGpsSupplierOrderTimeConsts.MaxOrderTypeLength)]
        public virtual string OrderType { get; set; }

        public virtual TimeSpan? OrderTime { get; set; }

        public virtual int? ReceivingDay { get; set; }

        public virtual TimeSpan? ReceiveTime { get; set; }

        public virtual TimeSpan? KeihenTime { get; set; }

        public virtual int? KeihenDay { get; set; }

        [StringLength(MstInvGpsSupplierOrderTimeConsts.MaxIsActiveLength)]
        public virtual string IsActive { get; set; }
    }

    public class GetMstInvGpsSupplierOrderTimeInput : PagedAndSortedResultRequestDto
    {

        public virtual long? SupplierId { get; set; }

    }
}
