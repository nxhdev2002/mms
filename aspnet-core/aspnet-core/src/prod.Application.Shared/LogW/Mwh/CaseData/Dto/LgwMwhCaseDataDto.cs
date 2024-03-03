using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.LogW.Mwh.Dto
{

	public class LgwMwhCaseDataDto : EntityDto<long?>
	{
        public virtual string CaseNo { get; set; }

		public virtual string LotNo { get; set; }

		public virtual string Grade { get; set; }

		public virtual string Model { get; set; }

		public virtual int? CaseQty { get; set; }

		public virtual string ContainerNo { get; set; }

		public virtual string Renban { get; set; }

		public virtual string SupplierNo { get; set; }

		public virtual string OrderType { get; set; }

		public virtual string CasePrefix { get; set; }

		public virtual string ProdLine { get; set; }

		public virtual long? PxpCaseId { get; set; }

		public virtual long? ContScheduleId { get; set; }

		public virtual string Status { get; set; }

		public virtual DateTime? DevanningDate { get; set; }

		public virtual DateTime? StartDevanningDate { get; set; }

		public virtual DateTime? FinishDevanningDate { get; set; }

		public virtual string ZoneCd { get; set; }

		public virtual string AreaCd { get; set; }

		public virtual long? LocId { get; set; }

		public virtual string LocCd { get; set; }

		public virtual DateTime? LocDate { get; set; }

		public virtual string LocBy { get; set; }

		public virtual string Shop { get; set; }

		public virtual string IsBigpart { get; set; }

		public virtual string IsActive { get; set; }

		public virtual string Loc { get; set; }

		public virtual string LocDetails { get; set; }

	}

	public class GetLgwMwhCaseDataInput : PagedAndSortedResultRequestDto
	{

        public virtual DateTime? DevanningDateFrom { get; set; }

        public virtual DateTime? DevanningDateTo { get; set; }

        public virtual string CaseNo { get; set; }

		public virtual string Renban { get; set; }		

		public virtual string ZoneCd { get; set; }

		public virtual string AreaCd { get; set; }

	}

    public class GetLgwMwhCaseDataHisByCaseNoInput : PagedAndSortedResultRequestDto
    {
        public virtual DateTime? StartDevanningDate { get; set; }

        public virtual DateTime? FinishDevanningDate { get; set; }

        public virtual string CaseNo { get; set; }

        public virtual string Renban { get; set; }

        public virtual string ZoneCd { get; set; }

        public virtual string AreaCd { get; set; }

    }



    

}


