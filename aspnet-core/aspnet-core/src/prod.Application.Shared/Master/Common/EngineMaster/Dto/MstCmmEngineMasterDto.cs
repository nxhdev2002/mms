using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Master.Cmm.Dto
{

    public class MstCmmEngineMasterDto : EntityDto<long?>
    {

        public virtual string MaterialCode { get; set; }

        public virtual string ClassType { get; set; }

        public virtual string ClassName { get; set; }

        public virtual string TransmissionType { get; set; }

        public virtual string EngineModel { get; set; }

        public virtual string EngineType { get; set; }

    }

    public class CreateOrEditMstCmmEngineMasterDto : EntityDto<long?>
    {

        [StringLength(MstCmmEngineMasterConsts.MaxMaterialCodeLength)]
        public virtual string MaterialCode { get; set; }

        [StringLength(MstCmmEngineMasterConsts.MaxClassTypeLength)]
        public virtual string ClassType { get; set; }

        [StringLength(MstCmmEngineMasterConsts.MaxClassNameLength)]
        public virtual string ClassName { get; set; }

        [StringLength(MstCmmEngineMasterConsts.MaxTransmissionTypeLength)]
        public virtual string TransmissionType { get; set; }

        [StringLength(MstCmmEngineMasterConsts.MaxEngineModelLength)]
        public virtual string EngineModel { get; set; }

        [StringLength(MstCmmEngineMasterConsts.MaxEngineTypeLength)]
        public virtual string EngineType { get; set; }
    }

    public class GetMstCmmEngineMasterInput : PagedAndSortedResultRequestDto
    {

        public virtual string MaterialCode { get; set; }


        public virtual string TransmissionType { get; set; }

        public virtual string EngineModel { get; set; }

        public virtual string EngineType { get; set; }

    }

    public class GetMstCmmEngineMasterHistoryInput : PagedAndSortedResultRequestDto
    {
        public virtual long Id { get; set; }
        public virtual string TableName { get; set; }
    }
    public class GetMstCmmEngineMasterHistoryExcelInput
    {
        public virtual long Id { get; set; }
        public virtual string TableName { get; set; }
    }
}