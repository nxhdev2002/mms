using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace prod.Master.LogA.Dto
{
    public class MstLgaBp2EcarDto : EntityDto<long?>
    {
        public virtual string Code { get; set; }
        public virtual string EcarName { get; set; }
        public virtual string ProdLine { get; set; }
        public virtual string EcarType { get; set; }
        public virtual int? Sorting { get; set; }
        public virtual string IsActive { get; set; }
    }

    public class CreateOrEditMstLgaBp2EcarDto : EntityDto<long?>
    {
        [StringLength(MstLgaBp2EcarConsts.MaxCodeLength)]
        public virtual string Code { get; set; }

        [StringLength(MstLgaBp2EcarConsts.MaxEcarNameLength)]
        public virtual string EcarName { get; set; }

        [StringLength(MstLgaBp2EcarConsts.MaxProdLineLength)]
        public virtual string ProdLine { get; set; }

        [StringLength(MstLgaBp2EcarConsts.MaxEcarTypeLength)]
        public virtual string EcarType { get; set; }

        public virtual int? Sorting { get; set; }

        [StringLength(MstLgaBp2EcarConsts.MaxIsActiveLength)]
        public virtual string IsActive { get; set; }
    }

    public class GetMstLgaBp2EcarInput : PagedAndSortedResultRequestDto
    {
        public virtual string Code { get; set; }
        public virtual string EcarName { get; set; }
        public virtual string ProdLine { get; set; }
    }

    public class GetMstLgaBp2EcarExcelInput
    {
        public virtual string Code { get; set; }
        public virtual string EcarName { get; set; }
        public virtual string ProdLine { get; set; }
    }
}
