using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Master.LogW.Dto
{

	public class MstLgwLayoutDto : EntityDto<long?>
	{

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

		public virtual string IsActive { get; set; }

	}

	
	public class GetMstLgwLayoutInput : PagedAndSortedResultRequestDto
	{

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

		public virtual string IsActive { get; set; }

	}

}


