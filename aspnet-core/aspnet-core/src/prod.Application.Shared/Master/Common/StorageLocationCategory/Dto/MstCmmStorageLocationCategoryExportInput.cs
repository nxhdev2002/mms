using System;
using System.Collections.Generic;
using System.Text;

namespace prod.Master.Common.StorageLocationCategory.Dto
{
    public class MstCmmStorageLocationCategoryExportInput
    {
        public virtual string Code { get; set; }
        public virtual string Name { get; set; }
        public virtual string AreaType { get; set; }
        public virtual string IsActive { get; set; }
    }
}
