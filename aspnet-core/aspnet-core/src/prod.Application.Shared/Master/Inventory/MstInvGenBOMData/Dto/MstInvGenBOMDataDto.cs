using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace prod.Master.Inventory.MstInvGenBOMData.Dto
{
    public class MstInvGenBOMDataDto : EntityDto<long?>
    {
        public string FileId { get; set; }
        public string DataFieldName { get; set; }
        public int? DataFieldLengh { get; set; }
        public string DataFieldType { get; set; }
        public string DataFieldDescription { get; set; }
    }
    public class GetInvGenBOMDataInput : PagedAndSortedResultRequestDto
    {
        public string DataFieldName { get; set; }
        public string DataFieldDescription { get; set; }
    }

}
