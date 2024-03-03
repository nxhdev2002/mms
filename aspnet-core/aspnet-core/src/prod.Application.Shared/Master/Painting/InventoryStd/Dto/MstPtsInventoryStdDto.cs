using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Master.Painting.Dto
{

    public class MstPtsInventoryStdDto : EntityDto<long?>
    {

        public virtual string Model { get; set; }

        public virtual int? InventoryStd { get; set; }

        public virtual string IsActive { get; set; }

    }

    public class CreateOrEditMstPtsInventoryStdDto : EntityDto<long?>
    {

        [StringLength(MstPtsInventoryStdConsts.MaxModelLength)]
        public virtual string Model { get; set; }

        public virtual int? InventoryStd { get; set; }

        [StringLength(MstPtsInventoryStdConsts.MaxIsActiveLength)]
        public virtual string IsActive { get; set; }
    }

    public class GetMstPtsInventoryStdInput : PagedAndSortedResultRequestDto
    {

        public virtual string Model { get; set; }

        public virtual int? InventoryStd { get; set; }

        public virtual string IsActive { get; set; }

    }

}
