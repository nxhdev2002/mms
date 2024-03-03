using System;
using System.Collections.Generic;
using System.Text;

namespace prod.Master.Common.Plant.Dto
{
    public class MstCmmPlantExportInput
    {
        public virtual string PlantCode { get; set; }

        public virtual string PlantName { get; set; }

        public virtual string BranchNo { get; set; }

        public virtual string AddressLanguageEn { get; set; }

        public virtual string AddressLanguageVn { get; set; }

        public virtual string IsActive { get; set; }
    }
}
