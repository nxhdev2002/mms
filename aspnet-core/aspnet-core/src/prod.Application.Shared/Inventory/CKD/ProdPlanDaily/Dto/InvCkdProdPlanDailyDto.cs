using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Inventory.CKD.Dto
{

    public class InvCkdProdPlanDailyDto : EntityDto<long?>
    {

        public virtual long? No { get; set; }
        public virtual string Prodline { get; set; }
        public virtual string Cfc { get; set; }
        public virtual string Model { get; set; }
        public virtual string Grade { get; set; }
        public virtual string Color { get; set; }
        public virtual string BodyNo { get; set; }
        public virtual string LotNo { get; set; }
        public virtual int? NoInLot { get; set; }
        public virtual string VinNo { get; set; }

        public virtual DateTime? WInPlanDate { get; set; }

       // public virtual TimeSpan? Winplantime { get; set; }

        public virtual DateTime? WInPlanDateTime { get; set; }

        public virtual string WInPlanDate_DDMMYYYY
        {
            get
            {
                try
                {
                    return string.Format("{0:dd/MM/yyyy }", WInPlanDate);
                }
                catch
                {
                    return "";
                }
            }
            set { }
        }

        public virtual string WInPlanDateTime_DDMMYYYY_HHMMSS
        {
            get
            {
                try
                {
                    return string.Format("{0:dd/MM/yyyy HH:mm:ss }", WInPlanDateTime);
                }
                catch
                {
                    return "";
                }
            }
            set { }
        }

        public virtual DateTime? WOutPlanDate { get; set; }

      //  public virtual TimeSpan? Woutplantime { get; set; }

        public virtual DateTime? WOutPlanDateTime { get; set; }

        public virtual string WOutPlanDate_DDMMYYYY
        {
            get
            {
                try
                {
                    return string.Format("{0:dd/MM/yyyy}", WOutPlanDate);
                }
                catch
                {
                    return "";
                }
            }
            set { }
        }

        public virtual string WOutPlanDateTime_DDMMYYYY_HHMMSS
        {
            get
            {
                try
                {
                    return string.Format("{0:dd/MM/yyyy HH:mm:ss}", WOutPlanDateTime);
                }
                catch
                {
                    return "";
                }
            }
            set { }
        }

        public virtual DateTime? TInPlanDate { get; set; }

       // public virtual TimeSpan? Tinplantime { get; set; }

        public virtual DateTime? TInPlanDatetime { get; set; }

        public virtual string TInPlanDate_DDMMYYYY
        {
            get
            {
                try
                {
                    return string.Format("{0:dd/MM/yyyy}", TInPlanDate);
                }
                catch
                {
                    return "";
                }
            }
            set { }
        }

        public virtual string TInPlanDatetime_DDMMYYYY_HHMMSS
        {
            get
            {
                try
                {
                    return string.Format("{0:dd/MM/yyyy HH:mm:ss}", TInPlanDatetime);
                }
                catch
                {
                    return "";
                }
            }
            set { }
        }

        public virtual DateTime? TOutPlanDate { get; set; }

      //  public virtual TimeSpan? Toutplantime { get; set; }

        public virtual DateTime? TOutPlanDatetime { get; set; }

        public virtual string TOutPlanDate_DDMMYYYY
        {
            get
            {
                try
                {
                    return string.Format("{0:dd/MM/yyyy}", TOutPlanDate);
                }
                catch
                {
                    return "";
                }
            }
            set { }
        }

        public virtual string TOutPlanDatetime_DDMMYYYY_HHMMSS
        {
            get
            {
                try
                {
                    return string.Format("{0:dd/MM/yyyy HH:mm:ss}", TOutPlanDatetime);
                }
                catch
                {
                    return "";
                }
            }
            set { }
        }

        public virtual DateTime? AInPlanDate { get; set; }

       // public virtual TimeSpan? Ainplantime { get; set; }

        public virtual DateTime? AInPlanDatetime { get; set; }

        public virtual string AInPlanDate_DDMMYYYY
        {
            get
            {
                try
                {
                    return string.Format("{0:dd/MM/yyyy}", AInPlanDate);
                }
                catch
                {
                    return "";
                }
            }
            set { }
        }

        public virtual string AInPlanDateTime_DDMMYYYY_HHMMSS
        {
            get
            {
                try
                {
                    return string.Format("{0:dd/MM/yyyy HH:mm:ss}", AInPlanDatetime);
                }
                catch
                {
                    return "";
                }
            }
            set { }
        }

        public virtual DateTime? AOutPlanDate { get; set; }

      //  public virtual TimeSpan? Aoutplantime { get; set; }

        public virtual DateTime? AOutPlanDateTime { get; set; }

        public virtual string AOutPlanDate_DDMMYYYY
        {
            get
            {
                try
                {
                    return string.Format("{0:dd/MM/yyyy}", AOutPlanDate);
                }
                catch
                {
                    return "";
                }
            }
            set { }
        }

        public virtual string AOutPlanDateTime_DDMMYYYY_HHMMSS
        {
            get
            {
                try
                {
                    return string.Format("{0:dd/MM/yyyy HH:mm:ss}", AOutPlanDateTime);
                }
                catch
                {
                    return "";
                }
            }
            set { }
        }

        public virtual DateTime? LineoffDate { get; set; }

        public virtual DateTime? LineoffDateTime { get; set; }
        public virtual string LineoffDateTime_DDMMYYYY_HHMMSS
        {
            get
            {
                try
                {
                    return string.Format("{0:dd/MM/yyyy HH:mm:ss}", LineoffDateTime);
                }
                catch
                {
                    return "";
                }
            }
            set { }
        }

        public virtual string LineoffDate_DDMMYYYY
        {
            get
            {
                try
                {
                    return string.Format("{0:dd/MM/yyyy}", LineoffDate);
                }
                catch
                {
                    return "";
                }
            }
            set { }
        }


        public virtual TimeSpan? Lineofftime { get; set; }
        public virtual DateTime? Lineoffdatetime { get; set; }
        public virtual DateTime? PdiDate { get; set; }
        public virtual DateTime? PdiDateTime { get; set; }      
        public virtual string PdiDate_DDMMYYYY
        {
            get
            {
                try
                {
                    return string.Format("{0:dd/MM/yyyy}", PdiDate);
                }
                catch
                {
                    return "";
                }
            }
            set { }
        }

        public virtual string PdiDateTime_DDMMYYYY_HHMMSS
        {
            get
            {
                try
                {
                    return string.Format("{0:dd/MM/yyyy HH:mm:ss}", PdiDateTime);
                }
                catch
                {
                    return "";
                }
            }
            set { }
        }

        public virtual TimeSpan? Pditime { get; set; }

        public virtual DateTime? Pdidatetime { get; set; }

        public virtual string Isproject { get; set; }

        public virtual string Vehicleid { get; set; }

        public virtual string Indentline { get; set; }

        public virtual string Colortype { get; set; }

        public virtual string Engineid { get; set; }

        public virtual string Goshicar { get; set; }

        public virtual string Salessfx { get; set; }

        public virtual string EdNumber { get; set; }

        public virtual string Ssno { get; set; }

        public virtual string Transid { get; set; }

        public virtual int? CountWinPlan { get; set; }
        public virtual int? CountWoutPlan { get; set; }
        public virtual int? CountTinPlan { get; set; }
        public virtual int? CountToutPlan { get; set; }
        public virtual int? CountAinPlan { get; set; }
        public virtual int? CountAoutPlan { get; set; }
        public virtual int? CountPdiDate { get; set; }
        public virtual int? CountLineoffdate { get; set; }

    }

    public class CreateOrEditInvCkdProdPlanDailyDto : EntityDto<long?>
    {

        public virtual long? No { get; set; }

        [StringLength(InvCkdProdPlanDailyConsts.MaxProdlineLength)]
        public virtual string Prodline { get; set; }

        [StringLength(InvCkdProdPlanDailyConsts.MaxCfcLength)]
        public virtual string Cfc { get; set; }

        [StringLength(InvCkdProdPlanDailyConsts.MaxModelLength)]
        public virtual string Model { get; set; }

        [StringLength(InvCkdProdPlanDailyConsts.MaxGradeLength)]
        public virtual string Grade { get; set; }

        [StringLength(InvCkdProdPlanDailyConsts.MaxColorLength)]
        public virtual string Color { get; set; }

        [StringLength(InvCkdProdPlanDailyConsts.MaxBodynoLength)]
        public virtual string Bodyno { get; set; }

        [StringLength(InvCkdProdPlanDailyConsts.MaxLotnoLength)]
        public virtual string Lotno { get; set; }

        public virtual int? Noinlot { get; set; }

        [StringLength(InvCkdProdPlanDailyConsts.MaxVinnoLength)]
        public virtual string Vinno { get; set; }

        public virtual DateTime? Winplandate { get; set; }

        public virtual TimeSpan? Winplantime { get; set; }

        public virtual DateTime? Winplandatetime { get; set; }

        public virtual DateTime? Woutplandate { get; set; }

        public virtual TimeSpan? Woutplantime { get; set; }

        public virtual DateTime? Woutplandatetime { get; set; }

        public virtual DateTime? Tinplandate { get; set; }

        public virtual TimeSpan? Tinplantime { get; set; }

        public virtual DateTime? Tinplandatetime { get; set; }

        public virtual DateTime? Toutplandate { get; set; }

        public virtual TimeSpan? Toutplantime { get; set; }

        public virtual DateTime? Toutplandatetime { get; set; }

        public virtual DateTime? Ainplandate { get; set; }

        public virtual TimeSpan? Ainplantime { get; set; }

        public virtual DateTime? Ainplandatetime { get; set; }

        public virtual DateTime? Aoutplandate { get; set; }

        public virtual TimeSpan? Aoutplantime { get; set; }

        public virtual DateTime? Aoutplandatetime { get; set; }

        public virtual DateTime? Lineoffdate { get; set; }

        public virtual TimeSpan? Lineofftime { get; set; }

        public virtual DateTime? Lineoffdatetime { get; set; }

        public virtual DateTime? Pdidate { get; set; }

        public virtual TimeSpan? Pditime { get; set; }

        public virtual DateTime? Pdidatetime { get; set; }

        [StringLength(InvCkdProdPlanDailyConsts.MaxIsprojectLength)]
        public virtual string Isproject { get; set; }

        [StringLength(InvCkdProdPlanDailyConsts.MaxVehicleidLength)]
        public virtual string Vehicleid { get; set; }

        [StringLength(InvCkdProdPlanDailyConsts.MaxIndentlineLength)]
        public virtual string Indentline { get; set; }

        [StringLength(InvCkdProdPlanDailyConsts.MaxColortypeLength)]
        public virtual string Colortype { get; set; }

        [StringLength(InvCkdProdPlanDailyConsts.MaxEngineidLength)]
        public virtual string Engineid { get; set; }

        [StringLength(InvCkdProdPlanDailyConsts.MaxGoshicarLength)]
        public virtual string Goshicar { get; set; }

        [StringLength(InvCkdProdPlanDailyConsts.MaxSalessfxLength)]
        public virtual string Salessfx { get; set; }

        public virtual string EdNumber { get; set; }

        [StringLength(InvCkdProdPlanDailyConsts.MaxSsnoLength)]
        public virtual string Ssno { get; set; }

        [StringLength(InvCkdProdPlanDailyConsts.MaxTransidLength)]
        public virtual string Transid { get; set; }
    }

    public class GetInvCkdProdPlanDailyInput : PagedAndSortedResultRequestDto
    {

        public virtual string Vin { get; set; }

        public virtual string LotNo { get; set; }

        public virtual int? NoInLot { get; set; }

        public virtual string Cfc { get; set; }

        public virtual string BodyNo { get; set; }

        public virtual DateTime? DateFrom { get; set; }

        public virtual DateTime? DateTo { get; set; }

        public virtual string IsPdiDate { get; set; }

        public virtual int SelectDate { get; set; }

    }

    public class InvCkdProductionPlanDailyReportInput
    {
        public virtual DateTime FromDate { get; set; }

        public virtual DateTime ToDate { get; set; }

        public virtual string isPdiDate { get; set; }
    }

    public class InvCkdProductionPlanDailyReportDataDto
    {
        public virtual string CFC { get; set; }
        public virtual string Grade { get; set; }
        public virtual string Color { get; set; }
        public virtual DateTime PdiDate { get; set; }
        public virtual int Count { get; set; }

        public virtual string PdiDateStr { get; set; }
    }
    public class GetInvCkdProdPlanDailyHistoryInput : PagedAndSortedResultRequestDto
    {
        public virtual long Id { get; set; }
        public virtual string TableName { get; set; }
    }
     public class GetInvCkdProdPlanDailyHistoryExcelInput
    {
        public virtual long Id { get; set; }
        public virtual string TableName { get; set; }
    }

}


