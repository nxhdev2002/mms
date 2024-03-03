using Abp.Application.Services.Dto;
using Abp.Web.Mvc.Alerts;
using prod.MultiTenancy.Payments.Stripe.Dto;
using System;
using System.Collections.Generic;
using System.Security.Policy;
using System.Text;

namespace prod.Inventory.CKD.Vehicle.Dto
{
    public class InvCkdVehicleDto : EntityDto<long?>
    {
        public virtual string Cfc { get; set; }
        public virtual string BodyNo { get; set; }
        public virtual string Color { get; set; }
        public virtual string Grade { get; set; }
        public virtual string LotNo { get; set; }
        public virtual int? NoInLot { get; set; }
        public virtual string SequenceNo { get; set; }
        public virtual string Vin { get; set; }
        public virtual string EngineId { get; set; }
        public virtual string TransId { get; set; }
        public virtual string SalesSfx { get; set; }
        public virtual string ColorType { get; set; }
        public virtual string IndentLine { get; set; }
        public virtual string SsNo { get; set; }
        public virtual string EdNumber { get; set; }
        public virtual string GoshiCar { get; set; }
        public virtual string Name { get; set; }
        public virtual string ProdLine { get; set; }




        public virtual DateTime? WInActualDate { get; set; }
        public virtual string WInActualDate_DDMMYYYY
        {
            get
            {
                try
                {
                    return string.Format("{0:dd/MM/yyyy}", WInActualDate);
                }
                catch (Exception ex)
                {
                    return "";
                }
            }
            set { }
        }

        public virtual DateTime? WOutActualDate { get; set; }
        public virtual string WOutActualDate_DDMMYYYY
        {
            get
            {
                try
                {
                    return string.Format("{0:dd/MM/yyyy }", WOutActualDate);
                }
                catch (Exception ex)
                {
                    return "";
                }
            }
            set { }
        }


        public virtual DateTime? TInActualDate { get; set; }
        public virtual string TInActualDate_DDMMYYYY
        {
            get
            {
                try
                {
                    return string.Format("{0:dd/MM/yyyy}", TInActualDate);
                }
                catch (Exception ex)
                {
                    return "";
                }
            }
            set { }
        }

        public virtual DateTime? TOutActualDate { get; set; }
        public virtual string TOutActualDate_DDMMYYYY
        {
            get
            {
                try
                {
                    return string.Format("{0:dd/MM/yyyy}", TOutActualDate);
                }
                catch (Exception ex)
                {
                    return "";
                }
            }
            set { }
        }

        public virtual DateTime? AInActualDate { get; set; }
        public virtual string AInActualDate_DDMMYYYY
        {
            get
            {
                try
                {
                    return string.Format("{0:dd/MM/yyyy}", AInActualDate);
                }
                catch (Exception ex)
                {
                    return "";
                }
            }
            set { }
        }


        public virtual DateTime? AOutActualDate { get; set; }
        public virtual string AOutActualDate_DDMMYYYY
        {
            get
            {
                try
                {
                    return string.Format("{0:dd/MM/yyyy}", AOutActualDate);
                }
                catch (Exception ex)
                {
                    return "";
                }
            }
            set { }
        }

        public virtual DateTime? LineoffDate { get; set; }
        public virtual string LineoffDate_DDMMYYYY
        {
            get
            {
                try
                {
                    return string.Format("{0:dd/MM/yyyy}", LineoffDate);
                }
                catch (Exception ex)
                {
                    return "";
                }
            }
            set { }
        }

        public virtual DateTime? PdiDate { get; set; }
        public virtual string PdiDate_DDMMYYYY
        {
            get
            {
                try
                {
                    return string.Format("{0:dd/MM/yyyy}", PdiDate);
                }
                catch (Exception ex)
                {
                    return "";
                }
            }
            set { }
        }

        public virtual DateTime? PIOActualDate { get; set; }
        public virtual string PIOActualDate_DDMMYYYY
        {
            get
            {
                try
                {
                    return string.Format("{0:dd/MM/yyyy}", PIOActualDate);
                }
                catch (Exception ex)
                {
                    return "";
                }
            }
            set { }
        }

        public virtual DateTime? SalesActualDate { get; set; }
        public virtual string SalesActualDate_DDMMYYYY
        {
            get
            {
                try
                {
                    return string.Format("{0:dd/MM/yyyy}", SalesActualDate);
                }
                catch (Exception ex)
                {
                    return "";
                }
            }
            set { }
        }

        public virtual DateTime? WInActualDateTime { get; set; }
        public virtual string WInActualDateTime_DDMMYYYY
        {
            get
            {
                try
                {
                    return string.Format("{0:dd/MM/yyyy HH:mm:ss}", WInActualDateTime);
                }
                catch (Exception ex)
                {
                    return "";
                }
            }
            set { }
        }

        public virtual string WInActualDateTime_YYYYDDMM
        {
            get
            {
                try
                {
                    return string.Format("{0:yyyy-MM-ddThh:mm}", WInActualDateTime);
                }
                catch (Exception ex)
                {
                    return "";
                }
            }
            set { }
        }

        public virtual DateTime? WOutActualDateTime { get; set; }
        public virtual string WOutActualDateTime_DDMMYYYY
        {
            get
            {
                try
                {
                    return string.Format("{0:dd/MM/yyyy HH:mm:ss}", WOutActualDateTime);
                }
                catch (Exception ex)
                {
                    return "";
                }
            }
            set { }
        }


        public virtual string WOutActualDateTime_YYYYDDMM
        {
            get
            {
                try
                {
                    return string.Format("{0:yyyy-MM-ddThh:mm}", WOutActualDateTime);
                }
                catch (Exception ex)
                {
                    return "";
                }
            }
            set { }
        }



        public virtual DateTime? TInActualDateTime { get; set; }
        public virtual string TInActualDateTime_DDMMYYYY
        {
            get
            {
                try
                {
                    return string.Format("{0:dd/MM/yyyy HH:mm:ss}", TInActualDateTime);
                }
                catch (Exception ex)
                {
                    return "";
                }
            }
            set { }
        }


        public virtual string TInActualDateTime_YYYYDDMM
        {
            get
            {
                try
                {
                    return string.Format("{0:yyyy-MM-ddThh:mm}", TInActualDateTime);
                }
                catch (Exception ex)
                {
                    return "";
                }
            }
            set { }
        }

        public virtual DateTime? TOutActualDateTime { get; set; }
        public virtual string TOutActualDateTime_DDMMYYYY
        {
            get
            {
                try
                {
                    return string.Format("{0:dd/MM/yyyy HH:mm:ss}", TOutActualDateTime);
                }
                catch (Exception ex)
                {
                    return "";
                }
            }
            set { }
        }

        public virtual string TOutActualDateTime_YYYYDDMM
        {
            get
            {
                try
                {
                    return string.Format("{0:yyyy-MM-ddThh:mm}", TOutActualDateTime);
                }
                catch (Exception ex)
                {
                    return "";
                }
            }
            set { }
        }

        public virtual DateTime? AInActualDateTime { get; set; }
        public virtual string AInActualDateTime_DDMMYYYY
        {
            get
            {
                try
                {
                    return string.Format("{0:dd/MM/yyyy HH:mm:ss}", AInActualDateTime);
                }
                catch (Exception ex)
                {
                    return "";
                }
            }
            set { }
        }

        public virtual string AInActualDateTime_YYYYDDMM
        {
            get
            {
                try
                {
                    return string.Format("{0:yyyy-MM-ddThh:mm}", AInActualDateTime);
                }
                catch (Exception ex)
                {
                    return "";
                }
            }
            set { }
        }

        public virtual DateTime? AOutActualDateTime { get; set; }
        public virtual string AOutActualDateTime_DDMMYYYY
        {
            get
            {
                try
                {
                    return string.Format("{0:dd/MM/yyyy HH:mm:ss}", AOutActualDateTime);
                }
                catch (Exception ex)
                {
                    return "";
                }
            }
            set { }
        }

        public virtual string AOutActualDateTime_YYYYDDMM
        {
            get
            {
                try
                {
                    return string.Format("{0:yyyy-MM-ddThh:mm}", AOutActualDateTime);
                }
                catch (Exception ex)
                {
                    return "";
                }
            }
            set { }
        }

        public virtual DateTime? LineoffDateTime { get; set; }
        public virtual string LineoffDateTime_DDMMYYYY
        {
            get
            {
                try
                {
                    return string.Format("{0:dd/MM/yyyy HH:mm:ss}", LineoffDateTime);
                }
                catch (Exception ex)
                {
                    return "";
                }
            }
            set { }
        }

        public virtual string LineoffDateTime_YYYYDDMM
        {
            get
            {
                try
                {
                    return string.Format("{0:yyyy-MM-ddThh:mm}", LineoffDateTime);
                }
                catch (Exception ex)
                {
                    return "";
                }
            }
            set { }
        }

        public virtual DateTime? PdiDateTime { get; set; }
        public virtual string PdiDateTime_DDMMYYYY
        {
            get
            {
                try
                {
                    return string.Format("{0:dd/MM/yyyy HH:mm:ss}", PdiDateTime);
                }
                catch (Exception ex)
                {
                    return "";
                }
            }
            set { }
        }

        public virtual string PdiDateTime_YYYYDDMM
        {
            get
            {
                try
                {
                    return string.Format("{0:yyyy-MM-ddThh:mm}", PdiDateTime);
                }
                catch (Exception ex)
                {
                    return "";
                }
            }
            set { }
        }

        public virtual DateTime? PIOActualDatetime { get; set; }
        public virtual string PIOActualDatetime_DDMMYYYY
        {
            get
            {
                try
                {
                    return string.Format("{0:dd/MM/yyyy HH:mm:ss}", PIOActualDatetime);
                }
                catch (Exception ex)
                {
                    return "";
                }
            }
            set { }
        }

        public virtual DateTime? SalesActualDatetime { get; set; }
        public virtual string SalesActualDatetime_DDMMYYYY
        {
            get
            {
                try
                {
                    return string.Format("{0:dd/MM/yyyy HH:mm:ss}", SalesActualDatetime);
                }
                catch (Exception ex)
                {
                    return "";
                }
            }
            set { }
        }


        public virtual TimeSpan? PIOActualTime { get; set; }
        public virtual TimeSpan? SalesActualTime { get; set; }

        public virtual int? CountWin { get; set; }
        public virtual int? CountWout { get; set; }
        public virtual int? CountTin { get; set; }
        public virtual int? CountTout { get; set; }
        public virtual int? CountAin { get; set; }
        public virtual int? CountAout { get; set; }
        public virtual int? CountLineOut { get; set; }
        public virtual int? CountPdiDate { get; set; }
        public virtual int? CountLineoffDate { get; set; }
        public virtual int? CountPioDate { get; set; }
        public virtual int? CountSalesDate { get; set; }

    }


    public class InvCkdVehicleDetailOutPartDto : EntityDto<long?>
    {
        public virtual string VinNo { get; set; }

        public virtual string LotNo { get; set; }

        public virtual int? NoInLot { get; set; }

        public virtual string SequenceNo { get; set; }

        public virtual string PartNo { get; set; }

        public virtual string Cfc { get; set; }

        public virtual string SupplierNo { get; set; }

        public virtual string Grade { get; set; }

        public virtual string Shop { get; set; }

        public virtual string BodyColor { get; set; }

        public virtual int? UsageQty { get; set; }
    }

    public class InvVehicleDetailOutPartInput : PagedAndSortedResultRequestDto
    {
        public virtual string Vin { get; set; }

        public virtual string LotNo { get; set; }

        public virtual string NoInLot { get; set; }

        public virtual string Cfc { get; set; }

        public virtual string SequenceNo { get; set; }

        public virtual string BodyNo { get; set; }

        public virtual DateTime? DateFrom { get; set; }

        public virtual DateTime? DateTo { get; set; }
    }

    public class InvCkdVehicleInput : PagedAndSortedResultRequestDto
    {
        public virtual string Vin { get; set; }

        public virtual string LotNo { get; set; }

        public virtual int? NoInLot { get; set; }

        public virtual string Cfc { get; set; }

        public virtual string BodyNo { get; set; }

        public virtual string Color { get; set; }

        public virtual string SequenceNo { get; set; }

        public virtual DateTime? DateFrom { get; set; }

        public virtual DateTime? DateTo { get; set; }

         public virtual string TypeDate { get; set; }

        public virtual int SelectDate { get; set; }

        public virtual string GoshiCar { get; set; }

    }


    public class InvCkdVehiclePDIInput : PagedAndSortedResultRequestDto
    {
        public virtual string Vin { get; set; }

        public virtual string LotNo { get; set; }

        public virtual int? NoInLot { get; set; }

        public virtual string Cfc { get; set; }

        public virtual string BodyNo { get; set; }

        public virtual string Color { get; set; }

        public virtual string SequenceNo { get; set; }

        public virtual DateTime? DateFrom { get; set; }

        public virtual DateTime? DateTo { get; set; }

        public virtual string TypeDate { get; set; }

        public virtual int SelectDate { get; set; }

        public virtual string GoshiCar { get; set; }

        public virtual string PartType { get; set; }

    }

    public class ViewIF
    {
        public virtual int RecordId { get; set; }

        public virtual string Vin { get; set; }

        public virtual string Urn { get; set; }

        public virtual string SpecSheetNo { get; set; }

        public virtual string IdLine { get; set; }

        public virtual string Katashiki { get; set; }

        public virtual string SaleKatashiki { get; set; }

        public virtual string SaleSuffix { get; set; } 

        public virtual string Spec200Digits { get; set; } 

        public virtual string ProductionSuffix { get; set; } 

        public virtual string LotCode { get; set; }

        public virtual string EnginePrefix { get; set; }

        public virtual string EngineNo { get; set;}

        public virtual string PlantCode { get; set;}

        public virtual string CurrentStatus { get; set; }

        public virtual string LineOffDatetime { get; set;} 

        public virtual string InteriorColor { get; set; } 

        public virtual string ExteriorColor { get; set; }

        public virtual string DestinationCode { get; set; }

        public virtual string EdOdno { get; set;}

        public virtual string CancelFlag { get; set; }

        public virtual string SmsCarFamilyCode { get; set; }

        public virtual string OrderType { get; set; }

        public virtual string KatashikiCode { get; set;}

        public virtual string EndOfRecord { get; set; }

    }

    public class InvCkdVehicleViewIFInput :PagedAndSortedResultRequestDto
    {
        public virtual DateTime? DateFrom { get; set; }

        public virtual DateTime? DateTo { get; set; }

    }

    public class GetViewIFExcelList : PagedAndSortedResultRequestDto
    {
        public virtual DateTime? DateFrom { get; set; }

        public virtual DateTime? DateTo { get; set; }
    }

    public class EditCkdVehicleInput : EntityDto<long?>
    {

        public virtual DateTime? WInDateActual { get; set; }
        public virtual DateTime? WOutActualDateTime { get; set; }
        public virtual DateTime? TInActualDateTime { get; set; }
        public virtual DateTime? TOutActualDate { get; set; }
        public virtual DateTime? AInDateActual { get; set; }
        public virtual DateTime? AOutActualDateTime { get; set; }
        public virtual DateTime? LineoffDateTime { get; set; }
        public virtual DateTime? PdiDateTime { get; set; }
        public virtual DateTime? PIOActualDate { get; set; }
        public virtual DateTime? SalesActualDate { get; set; }
 
    }
} 
