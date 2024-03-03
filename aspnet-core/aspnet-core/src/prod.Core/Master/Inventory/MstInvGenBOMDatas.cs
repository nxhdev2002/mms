using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace prod.Master.Inventory
{
    [Table("MstInvGenBOMData")]
    public  class MstInvGenBOMDatas : FullAuditedEntity<long>, IEntity<long>
    {
        public string FileId { get; set; }
        public string DataFieldName { get; set; }
        public int? DataFieldLengh { get; set; }
        public string DataFieldType { get; set; }
        public string DataFieldDescription { get; set; }
    }
}
