using Abp.Application.Services.Dto;
using prod.Master.Inv;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace prod.Master.Inventory.Dto
{
	public class MstGpsWbsCCMappingDto : EntityDto<long?>
	{
		public virtual string CostCenterFrom { get; set; }
		public virtual string WbsFrom { get; set; }
		public virtual string CostCenterTo { get; set; }
		public virtual string WbsTo { get; set; }
		public virtual DateTime? EffectiveFromDate { get; set; }
		public virtual DateTime? EffectiveFromTo { get; set; }
		public virtual string IsActive { get; set; }
	}

	public class CreateOrEditMstGpsWbsCCMappingDto : EntityDto<long?>
	{

		[StringLength(MstGpsWbsCCMappingConsts.MaxCostCenterFromLength)]
		public virtual string CostCenterFrom { get; set; }

		[StringLength(MstGpsWbsCCMappingConsts.MaxWbsFromLength)]
		public virtual string WbsFrom { get; set; }

		[StringLength(MstGpsWbsCCMappingConsts.MaxCostCenterToLength)]
		public virtual string CostCenterTo { get; set; }

		[StringLength(MstGpsWbsCCMappingConsts.MaxWbsToLength)]
		public virtual string WbsTo { get; set; }
		public virtual DateTime? EffectiveFromDate { get; set; }
		public virtual DateTime? EffectiveFromTo { get; set; }


		[StringLength(MstGpsWbsCCMappingConsts.MaxIsActiveLength)]
		public virtual string IsActive { get; set; }
	}

	public class GetMstGpsWbsCCMappingInput : PagedAndSortedResultRequestDto
	{

		public virtual string CostCenterFrom { get; set; }

		public virtual string WbsFrom { get; set; }


	}

	public class GetMstGpsWbsCCMappingExcelInput
	{

		public virtual string CostCenterFrom { get; set; }

		public virtual string WbsFrom { get; set; }


	}
	public class MstGpsWbsCCMappingImportDto : EntityDto<long?>
	{
		public virtual string CostCenterFrom { get; set; }

		public virtual string WbsFrom { get; set; }

		public virtual string CostCenterTo { get; set; }

		public virtual string WbsTo { get; set; }

		public virtual DateTime? EffectiveFromDate { get; set; }

		public virtual DateTime? EffectiveFromTo { get; set; }

		public virtual string IsActive { get; set; }

		public virtual long? CreatorUserId { get; set; }

		public virtual string Guid { get; set; }


	}

	public class MessageMstGpsWbsCCMappingDto
	{
		public string ExistCostCenterFromAndWbsFrom { get; set; }
	}

	public class MessageInvGpsExistMappingDto
	{
		public string ExistInInvGpsMapping { get; set; }
	}

	public class GetMstGpsWbsCCMappingHistoryInput : PagedAndSortedResultRequestDto
	{
		public virtual long Id { get; set; }
		public virtual string TableName { get; set; }
	}

	public class GetMstGpsWbsCCMappingHistoryExcelInput
	{
		public virtual long Id { get; set; }
		public virtual string TableName { get; set; }
	}
}
