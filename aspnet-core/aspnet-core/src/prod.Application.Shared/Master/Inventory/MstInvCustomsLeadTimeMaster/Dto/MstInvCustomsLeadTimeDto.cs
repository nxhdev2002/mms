using Abp.Application.Services.Dto;
using prod.Master.Inv;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace prod.Master.Inventory.Dto
{
    public class MstInvCustomsLeadTimeDto : EntityDto<long?>
    {

        public virtual string Exporter { get; set; }

        public virtual string Carrier { get; set; }

        public virtual int Leadtime { get; set; }


    }
    public class CreateOrEditMstInvCustomsLeadTimeDto : EntityDto<long?>
    {
        [StringLength(50)]
        public virtual string Exporter { get; set; }

        [StringLength(50)]
        public virtual string Carrier { get; set; }

        public virtual int Leadtime { get; set; }
    }



    public class GetMstInvCustomsLeadTimeInput : PagedAndSortedResultRequestDto
    {

        public virtual string Exporter { get; set; }

        public virtual string Carrier { get; set; }

    }
    public class MstInvCustomsLeadTimeImportDto : EntityDto<long?>
    {

        public virtual string Exporter { get; set; }

        public virtual string Carrier { get; set; }

        public virtual int Leadtime { get; set; }

        public virtual string Guid { get; set; }
    }
}
