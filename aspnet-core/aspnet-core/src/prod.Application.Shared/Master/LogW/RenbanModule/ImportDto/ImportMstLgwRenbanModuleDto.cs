using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace prod.Master.LogW.RenbanModule.ImportDto
{
	public class ImportMstLgwRenbanModuleDto
    {
	[StringLength(MstLgwRenbanModuleConsts.MaxGuidLength)]
	public virtual string Guid { get; set; }

	[StringLength(MstLgwRenbanModuleConsts.MaxRenbanLength)]
	public virtual string Renban { get; set; }

	[StringLength(MstLgwRenbanModuleConsts.MaxCaseNoLength)]
	public virtual string CaseNo { get; set; }

	[StringLength(MstLgwRenbanModuleConsts.MaxSupplierNoLength)]
	public virtual string SupplierNo { get; set; }

	public virtual int? MinModule { get; set; }

	public virtual int? MaxModule { get; set; }

	public virtual int? ModuleCapacity { get; set; }

	[StringLength(MstLgwRenbanModuleConsts.MaxModuleTypeLength)]
	public virtual string ModuleType { get; set; }

	[StringLength(MstLgwRenbanModuleConsts.MaxModuleSizeLength)]
	public virtual string ModuleSize { get; set; }

	[StringLength(MstLgwRenbanModuleConsts.MaxSortingTypeLength)]
	public virtual string SortingType { get; set; }

	public virtual int? MinMod { get; set; }

	public virtual int? MaxMod { get; set; }

	public virtual int? MonitorVisualize { get; set; }

	public virtual int? CaseOrder { get; set; }

	[StringLength(MstLgwRenbanModuleConsts.MaxCaseTypeLength)]
	public virtual string CaseType { get; set; }

	[StringLength(MstLgwRenbanModuleConsts.MaxProdLineLength)]
	public virtual string ProdLine { get; set; }

	[StringLength(MstLgwRenbanModuleConsts.MaxModelLength)]
	public virtual string Model { get; set; }

	[StringLength(MstLgwRenbanModuleConsts.MaxCfcLength)]
	public virtual string Cfc { get; set; }

	[StringLength(MstLgwRenbanModuleConsts.MaxWhLocLength)]
	public virtual string WhLoc { get; set; }

	[StringLength(MstLgwRenbanModuleConsts.MaxIsUsePxpDataLength)]
	public virtual string IsUsePxpData { get; set; }

	public virtual int? UpLeadtime { get; set; }


	[StringLength(MstLgwRenbanModuleConsts.MaxRemarkLength)]
	public virtual string Remark { get; set; }

	[StringLength(MstLgwRenbanModuleConsts.MaxIsActiveLength)]
	public virtual string IsActive { get; set; }
	public string Exception { get; set; }
	public bool CanBeImported()
	{
		return string.IsNullOrEmpty(Exception);
	}
	}
}
