using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Inventory.PIO.Stock.Dto
{

    public class InvPIOStockDto : EntityDto<long?>
    {

        public virtual string PartNo { get; set; }

        public virtual string PartName { get; set; }

        public virtual string MktCode { get; set; }

        public virtual DateTime? WorkingDate { get; set; }

        public virtual int? Qty { get; set; }

        public virtual DateTime? ScanDate { get; set; }

        public virtual string ScanDate_DDMMYYYY
        {
            get
            {
                try
                {
                    return string.Format("{0:dd/MM/yyyy}", ScanDate);
                }
                catch
                {
                    return "";
                }
            }
            set { }
        }

        public virtual string VinNo { get; set; }

        public virtual string PartType { get; set; }

        public virtual string Shop { get; set; }

        public virtual string CarType { get; set; }

        public virtual string InteriorColor { get; set; }

    }


    public class GetInvPIOStockInput : PagedAndSortedResultRequestDto
    {
        public virtual string MktCode { get; set; }

        public virtual string PartNo { get; set; }

        public virtual DateTime? WorkingDateFrom { get; set; }

        public virtual DateTime? WorkingDateTo { get; set; }

    }
    public class GetInvPIOStockExportInput
    {
        public virtual string MktCode { get; set; }

        public virtual string PartNo { get; set; }

        public virtual DateTime? WorkingDateFrom { get; set; }

        public virtual DateTime? WorkingDateTo { get; set; }
    }
}


