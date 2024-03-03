using Abp.Application.Services.Dto;
using System;

namespace prod.LogW.Mwh
{

	public class GetPxpModuleInputAndonLayoutOutput : EntityDto<long?>
	{
		public virtual long? LocationId { get; set; }
		public virtual string ZoneCd { get; set; }
		public virtual string AreaCd { get; set; }
		public virtual int? RowId { get; set; }
		public virtual int? ColumnId { get; set; }
		public virtual string RowName { get; set; }
		public virtual string ColumnName { get; set; }
		public virtual string LocationCd { get; set; }
		public virtual string LocationName { get; set; }
		public virtual string LocationTitle { get; set; }
		public virtual string IsDisabled { get; set; }
		public virtual string SubScreenNo { get; set; }
		public virtual string ScreenArea { get; set; }
		public virtual string CellName { get; set; }
		public virtual string CellType { get; set; }
		public virtual string IsColDisabled { get; set; }

	}

	public class GetPxpModuleInputAndonDataOutput : EntityDto<long?>
	{
		public virtual long? LocId { get; set; }
		public virtual string CaseNo { get; set; }
		public virtual string SupplierNo { get; set; } 
		public virtual string ContainerNo { get; set; }
		public virtual string Renban { get; set; }
		public virtual DateTime? DevanningDate { get; set; }
		public virtual int? RowId { get; set; }
		public virtual int? ColumnId { get; set; }
		public virtual string RowName { get; set; }
		public virtual string ColumnName { get; set; } 
		public virtual string LocationName { get; set; } 
		public virtual string LocationNameAlias { get; set; }

		public virtual string IsActive { get; set; }
		public virtual string IsDisabled { get; set; }
		public virtual string ZoneCd { get; set; }
		public virtual string AreaCd { get; set; }
		public virtual string CallFlag { get; set; }
		public virtual string ModuleSize { get; set; }
		public virtual string IsUnpacked { get; set; }

		public virtual int? MinModule { get; set; }
		public virtual int? MaxModule { get; set; }

		public virtual string Remarks { get; set; }
		public virtual string UpTable { get; set; }
		public virtual int? SubScreenNo { get; set; }
		public virtual string ScreenArea { get; set; }
		public virtual string MinFlag { get; set; }
		public virtual string IsEci { get; set; }
		public virtual string IsDeleted { get; set; }
	}


	public class GetPxpModuleCaseListOutput : EntityDto<long?>
	{
		public virtual string Renban { get; set; }
		public virtual string CaseNo { get; set; }
		public virtual string SupplierNo { get; set; }
		public virtual int? MinModule { get; set; }
		public virtual int? MaxModule { get; set; }
		public virtual int? ModuleCapacity { get; set; }
		public virtual string ModuleType { get; set; }
		public virtual string ModuleSize { get; set; }
		public virtual string SortingType { get; set; }
		public virtual int? MinMod { get; set; }
		public virtual int? MaxMod { get; set; }
		public virtual int? MonitorVisualize { get; set; }
		public virtual int? CaseOrder { get; set; }
		public virtual string CaseType { get; set; }
		public virtual string ProdLine { get; set; }
		public virtual string Model { get; set; }
		public virtual string Cfc { get; set; }
		public virtual string WhLoc { get; set; }
		public virtual string IsUsePxpData { get; set; }
		public virtual int? UpLeadtime { get; set; }
		public virtual string Remark { get; set; }
		public virtual string IsActive { get; set; }
	}

	public class GetPxpModuleSuggestionListOutput : EntityDto<long?>
	{ 
		public virtual string CaseNo { get; set; }
		public virtual string Renban { get; set; }  
		public virtual string SupplierNo { get; set; }
		public virtual string CasePrefix { get; set; }
		public virtual string Renban2 { get; set; } 

	}

	public class GetPxpModuleLotCodeOutput
	{
		public virtual string Model { get; set; }
		public virtual string LotCode { get; set; }
		public virtual string Cfc { get; set; }
		public virtual string Grade { get; set; }
		public virtual string GradeName { get; set; }
		public virtual string ModelCode { get; set; }
		public virtual string ModelVin { get; set; }

	}

	public class GetPxpModuleCaseNoLocationOutput
	{
		public virtual string CaseNo { get; set; }
		public virtual string LotNo { get; set; }
		public virtual string Grade { get; set; }
		public virtual string Model { get; set; }
		public virtual int? CaseQty { get; set; }
		public virtual string ContainerNo { get; set; }
		public virtual string SupplierNo { get; set; }
		public virtual string OrderType { get; set; }
		public virtual string CasePrefix { get; set; }
		public virtual string ProdLine { get; set; }
		public virtual long? PxpCaseId { get; set; }
		public virtual long? ContScheduleId { get; set; }
		public virtual string Status { get; set; }
		public virtual DateTime? FinishDevanningDate { get; set; } 
		public virtual string DelayFlag { get; set; } 

	}

	public class GetPxpModuleContRenbanOutput
	{

		public virtual string SupplierNo { get; set; }
		public virtual string ContainerNo { get; set; }
		public virtual string Renban { get; set; }
		public virtual DateTime? DevanningDate { get; set; } 
		public virtual int? ColumnId { get; set; } 
		public virtual string ColumnName { get; set; } 
		public virtual string LocationNameAlias { get; set; }

	}

}

