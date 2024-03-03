using Abp.Application.Services.Dto;
using System;
namespace prod.Inventory.IF.Dto
{

    public class IF_FQF3MM_LV2Dto : EntityDto<long?>
    {

        public virtual string FileId { get; set; }

        public virtual string FileDescription { get; set; }

        public virtual string Content { get; set; }

        public virtual DateTime? InterfaceDate { get; set; }

        public virtual string Status { get; set; }

        public virtual DateTime? StatusDateTime { get; set; }

        public virtual string FilePath { get; set; }

    }

    public class GetIF_FQF3MM_LV2Input : PagedAndSortedResultRequestDto
    {

        public virtual string FileDescription { get; set; }

        public virtual DateTime? InterfaceDate { get; set; }

        public virtual string Status { get; set; }

    }

}