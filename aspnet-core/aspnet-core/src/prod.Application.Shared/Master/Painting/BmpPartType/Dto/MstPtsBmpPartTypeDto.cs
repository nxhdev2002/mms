using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace prod.Master.Painting.BmpPartType.Dto
{
    public class MstPtsBmpPartTypeDto : EntityDto<long?>
    {
        public virtual string PartType { get; set; }

        public virtual string PartTypeName { get; set; }

        public virtual string ProdLine { get; set; }
        public virtual long? Sorting { get; set; }

        public virtual string IsActive { get; set; }
    }

    public class CreateOrEditMstPtsBmpPartTypeDto : EntityDto<long?>
    {
        [StringLength(MstPtsBmpPartTypeConsts.MaxPartTypeLength)]
		public virtual string PartType { get; set; }
        [StringLength(MstPtsBmpPartTypeConsts.MaxPartTypeNameLength)]
		public virtual string PartTypeName { get; set; }
        [StringLength(MstPtsBmpPartTypeConsts.MaxProdLineLength)]
		public virtual string ProdLine { get; set; }
        public virtual long? Sorting { get; set; }
        [StringLength(MstPtsBmpPartTypeConsts.MaxIsActiveLength)]
		public virtual string IsActive { get; set; }
    }

    public class GetMstPtsBmpPartTypeInput : PagedAndSortedResultRequestDto
    {
        public virtual string PartType { get; set; }
        public virtual string PartTypeName { get; set; }
        public virtual string ProdLine { get; set; }
        public virtual string IsActive { get; set; }
    }
}
