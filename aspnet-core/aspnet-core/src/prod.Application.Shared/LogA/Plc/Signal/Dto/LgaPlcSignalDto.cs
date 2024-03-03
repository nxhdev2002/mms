using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace prod.LogA.Plc.Signal.Dto
{
    public class LgaPlcSignalDto : EntityDto<long?>
    {
        public virtual int? SignalIndex { get; set; }

        public virtual string SignalPattern { get; set; }

        public virtual DateTime? SignalTime { get; set; }

        public virtual string ProdLine { get; set; }

        public virtual string Process { get; set; }

        public virtual long? RefId { get; set; }

        public virtual string IsActive { get; set; }
    }
    public class GetLgaPlcSignalInput : PagedAndSortedResultRequestDto
    {
        public virtual DateTime? SignalTimeFrom { get; set; }
        public virtual DateTime? SignalTimeTo { get; set; }

        public virtual string SignalPattern { get; set; }

        public virtual string ProdLine { get; set; }

        public virtual string Process { get; set; }

    }
    public class CreateOrEditLgaPlcSignalDto : EntityDto<long?>
    {

        public virtual int? SignalIndex { get; set; }

        public virtual string SignalPattern { get; set; }

        public virtual DateTime? SignalTime { get; set; }
        public virtual string ProdLine { get; set; }

        public virtual string Process { get; set; }

        public virtual long? RefId { get; set; }

        public virtual string IsActive { get; set; }
    }
    public class GetLgaPlcSignalExportInput
    {
        public virtual DateTime? SignalTimeFrom { get; set; }
        public virtual DateTime? SignalTimeTo { get; set; }

        public virtual string SignalPattern { get; set; }

        public virtual string ProdLine { get; set; }

        public virtual string Process { get; set; }

    }
}
