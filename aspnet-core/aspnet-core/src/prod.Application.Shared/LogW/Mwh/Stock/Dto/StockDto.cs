
using System;
namespace prod.LogW.Mwh.Dto
{
	public class StockDto
	{  
        public virtual int? Id { get; set; }

        public virtual string Renban { get; set; }

        public virtual string CaseNo { get; set; }

        public virtual string SupplierNo { get; set; }

        public virtual string TriggerCall { get; set; }

        public virtual int? MinModule { get; set; }

        public virtual int? MaxModule { get; set; }

        public virtual int? ModuleCapacity { get; set; }

        public virtual string ModuleSize { get; set; }

        public virtual string WhLoc { get; set; }

        public virtual string IsSmall { get; set; }

        public virtual int? MinMod { get; set; }

        public virtual int? MaxMod { get; set; }

        public virtual int? CaseOrder { get; set; }

        public virtual string CaseType { get; set; }

        public virtual string Remark { get; set; }

        public virtual string ProdLine { get; set; }

        public virtual string Cfc { get; set; }

        public virtual int? ScreenMode { get; set; }

        public virtual string IsActive { get; set; }

        public virtual DateTime? UpDate { get; set; }

        public virtual TimeSpan? UpTime { get; set; }

        public virtual string CasePrefix { get; set; }

        public virtual string Type { get; set; }

        public virtual int? Sort { get; set; }

        public virtual int? RowId { get; set; }

        public virtual string ColumnAlias { get; set; }

        public virtual string LaneNo { get; set; }

        public virtual string LaneName { get; set; }

        public virtual string ContNo { get; set; }

        public virtual DateTime? DevDate { get; set; }

        public virtual string ShowOnly { get; set; }

        public virtual string CallFlag { get; set; }

    }
}


