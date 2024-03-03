using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace prod.Master.LogW.Dto
{
    public class MstLgwAdoCallingLightDto : EntityDto<long?>
    {

        public virtual string Code { get; set; }

        public virtual string LightName { get; set; }

        public virtual string ProdLine { get; set; }

        public virtual string Process { get; set; }

        public virtual string BlockCode { get; set; }

        public virtual string BlockDescription { get; set; }

        public virtual string Sorting { get; set; }

        public virtual int? SignalId { get; set; }

        public virtual string SignalCode { get; set; }

        public virtual string IsActive { get; set; }

    }

    public class CreateOrEditMstLgwAdoCallingLightDto : EntityDto<long?>
    {

        [StringLength(MstLgwAdoCallingLightConsts.MaxCodeLength)]
        public virtual string Code { get; set; }

        [StringLength(MstLgwAdoCallingLightConsts.MaxLightNameLength)]
        public virtual string LightName { get; set; }

        [StringLength(MstLgwAdoCallingLightConsts.MaxProdLineLength)]
        public virtual string ProdLine { get; set; }

        [StringLength(MstLgwAdoCallingLightConsts.MaxProcessLength)]
        public virtual string Process { get; set; }

        [StringLength(MstLgwAdoCallingLightConsts.MaxBlockCodeLength)]
        public virtual string BlockCode { get; set; }

        [StringLength(MstLgwAdoCallingLightConsts.MaxBlockDescriptionLength)]
        public virtual string BlockDescription { get; set; }

        [StringLength(MstLgwAdoCallingLightConsts.MaxSortingLength)]
        public virtual string Sorting { get; set; }

        public virtual int? SignalId { get; set; }

        [StringLength(MstLgwAdoCallingLightConsts.MaxSignalCodeLength)]
        public virtual string SignalCode { get; set; }

        [StringLength(MstLgwAdoCallingLightConsts.MaxIsActiveLength)]
        public virtual string IsActive { get; set; }
    }

    public class GetMstLgwAdoCallingLightInput : PagedAndSortedResultRequestDto
    {

        public virtual string Code { get; set; }

        public virtual string LightName { get; set; }

        public virtual string ProdLine { get; set; }

    }
}
