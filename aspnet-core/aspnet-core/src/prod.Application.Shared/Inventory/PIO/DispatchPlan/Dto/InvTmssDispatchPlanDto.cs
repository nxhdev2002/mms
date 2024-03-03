using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Inventory.Tmss.Dto
{
    public class InvTmssDispatchPlanDto : EntityDto<long?>
    {

        public virtual string VehicleType { get; set; }

        public virtual string Model { get; set; }

        public virtual string MarketingCode { get; set; }

        public virtual string ProductionCode { get; set; }

        public virtual string Dealer { get; set; }

        public virtual string Vin { get; set; }

        public virtual string ExtColor { get; set; }

        public virtual string IntColor { get; set; }

        public virtual DateTime? InstallDate { get; set; }

        public virtual DateTime? PInstallDate { get; set; }

        public virtual DateTime? DlrDispatchPlan { get; set; }

        public virtual DateTime? DlrDispatchDate { get; set; }

        public virtual string Katashiki { get; set; }
        public virtual string Route { get; set; }
    }

   
    public class GetInvTmssDispatchPlanInput : PagedAndSortedResultRequestDto
    {

        public virtual string VehicleType { get; set; }

        public virtual string Model { get; set; }

        public virtual string MarketingCode { get; set; }

        public virtual DateTime? DlrDispatchPlanDateFrom { get; set; }

        public virtual DateTime? DlrDispatchPlanDateTo { get; set; }

        public virtual DateTime? DlrDispatchDateFrom { get; set; }

        public virtual DateTime? DlrDispatchDateTo { get; set; }

        public virtual string Vin { get; set; }
    }

	public class GetInvPIOTmssDispatchPlanInput
	{
		public virtual DateTime FromDate { get; set; }

		public virtual DateTime ToDate { get; set; }
	}
	public class InvPIOTmssDispatchPlanDailyReportDataDto
	{
		public virtual string MarketingCode { get; set; }
		public virtual int Count { get; set; }
		public virtual DateTime DlrDispatchPlan { get; set; }
        public virtual string DlrMonth { get; set; }

		public virtual string IntColor { get; set; }
	}

    public class GetInvTmssDispatchPlanHistoryInput : PagedAndSortedResultRequestDto
    {
        public virtual long Id { get; set; }
        public virtual string TableName { get; set; }
    }
    public class GetInvTmssDispatchPlanHistoryExcelInput
    {
        public virtual long Id { get; set; }
        public virtual string TableName { get; set; }
    }
}


