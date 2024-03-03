using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
using prod.LogW.Plc.Signal;

namespace prod.LogW.Dto
{
    public class LgwPlcSignalDto : EntityDto<long?>
    {
        public virtual int? SignalIndex { get; set; }

        public virtual string SignalPattern { get; set; }

        public virtual DateTime? SignalTime { get; set; }

        public virtual string ProdLine { get; set; }

        public virtual string Process { get; set; }

        public virtual long? RefId { get; set; }

        public virtual string IsActive { get; set; }
    }
    public class GetLgwPlcSignalInput : PagedAndSortedResultRequestDto
    {
        public virtual DateTime? SignalTimeFrom { get; set; }
        public virtual DateTime? SignalTimeTo { get; set; }

        public virtual string SignalPattern { get; set; }

        public virtual string ProdLine { get; set; }

        public virtual string Process { get; set; }
        public virtual long? RefId { get; set; }

    }
    public class CreateOrEditLgwPlcSignalDto : EntityDto<long?>
    {
        public virtual int? SignalIndex { get; set; }

        [StringLength(LgwPlcSignalConsts.MaxSignalPatternLength)]
        public virtual string SignalPattern { get; set; }

        public virtual DateTime? SignalTime { get; set; }

        [StringLength(LgwPlcSignalConsts.MaxProdLineLength)]
        public virtual string ProdLine { get; set; }

        [StringLength(LgwPlcSignalConsts.MaxProcessLength)]
        public virtual string Process { get; set; }

        public virtual long? RefId { get; set; }

        [StringLength(LgwPlcSignalConsts.MaxIsActiveLength)]
        public virtual string IsActive { get; set; }
    }
    public class GetLgwPlcSignalExportInput
    {
        public virtual DateTime? SignalTimeFrom { get; set; }
        public virtual DateTime? SignalTimeTo { get; set; }
        public virtual string SignalPattern { get; set; }
        public virtual string ProdLine { get; set; }
        public virtual string Process { get; set; }
        public virtual long? RefId { get; set; }

    }
}
