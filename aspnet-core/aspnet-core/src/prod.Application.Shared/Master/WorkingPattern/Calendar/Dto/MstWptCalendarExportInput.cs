using System;
using System.Collections.Generic;
using Abp.Runtime.Validation;

namespace prod.Master.WorkingPattern.Dto
{
    public class MstWptCalendarExportInput
    {
		public virtual DateTime? WorkingDate { get; set; }

        public virtual DateTime? WorkingDateFrom { get; set; }

        public virtual DateTime? WorkingDateTo { get; set; }

        public virtual string WorkingType { get; set; }

		public virtual string WorkingStatus { get; set; }

		public virtual string SeasonType { get; set; }

		public virtual string DayOfWeek { get; set; }

		public virtual string IsActive { get; set; }


	}
}
