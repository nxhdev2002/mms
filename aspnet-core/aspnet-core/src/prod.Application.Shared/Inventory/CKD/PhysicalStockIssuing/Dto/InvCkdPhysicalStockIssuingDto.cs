using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace prod.Inventory.CKD.PhysicalStockIssuing.Dto
{
    public class InvCkdPhysicalStockIssuingDto : EntityDto<long?>
    {
        public virtual string PartNo { get; set; }

        public virtual string PartNoNormalized { get; set; }

        public virtual string PartName { get; set; }

        public virtual string PartNoNormalizedS4 { get; set; }

        public virtual string ColorSfx { get; set; }

        public virtual long? PartListId { get; set; }

        public virtual long? PartListGradeId { get; set; }

        public virtual long? MaterialId { get; set; }

        public virtual int? Qty { get; set; }

        public virtual DateTime? TransactionDatetime { get; set; }

        public virtual long? ReferenceId { get; set; }

        public virtual DateTime? WorkingDate { get; set; }

        public virtual long? PeriodId { get; set; }

        public virtual string SupplierNo { get; set; }

        public virtual string Cfc { get; set; }

        public virtual string LotNo { get; set; }

        public virtual int? NoInLot { get; set; }
        public virtual string TransactionDatetime_DDMMYYYY
        {
            get
            {
                try
                {
                    return string.Format("{0:dd/MM/yyyy HH:mm:ss}", TransactionDatetime);
                }
                catch
                {
                    return "";
                }
            }
            set { }
        }

        public virtual string VinNo { get; set; }

        public virtual string BodyNo { get; set; }


        public virtual string Color { get; set; }

        public virtual long? Carid { get; set; }

        public virtual string UseLot { get; set; }
        public virtual decimal? GrandTotal { get; set; }

        public virtual string IsActive { get; set; }

    }

    public class InvCkdPhysicalStockIssuingReportSummartDto 
    {
        public virtual string PartNo { get; set; }

        public virtual string PartName { get; set; }

        public virtual string Cfc { get; set; }

        public virtual string SupplierNo { get; set; }

        public virtual DateTime? WorkingDate { get; set; }

        public virtual int? Qty { get; set; }
    }
    public class InvCkdPhysicalStockIssuingInputDto : PagedAndSortedResultRequestDto
    {
        public virtual string PartNo { get; set; }

        public virtual DateTime? WorkingDateFrom { get; set; }

        public virtual DateTime? WorkingDateTo { get; set; }

        public virtual string Cfc { get; set; }

        public virtual string SupplierNo { get; set; }

        public virtual string ColorSfx { get; set; }

        public virtual string VinNo { get; set; }

        public virtual string LotNo { get; set; }

        public virtual string UseLot { get; set; }

        public virtual int? NoInLot { get; set; }

        public virtual int? p_mode { get; set; }

        public virtual int? PeriodId { get; set; }
       
    }

    public class InvCkdPhysicalStockIssuingDetailsDataDto
    {
        public virtual string PartNo { get; set; }

        public virtual string Cfc { get; set; }

        public virtual string SupplierNo { get; set; }

        public virtual string LotNo { get; set; }

        public virtual string ModuleNo { get; set; }

        public virtual string ContainerNo { get; set; }

        public virtual string InvoiceNo { get; set; }

        public virtual int? Out_Qty { get; set; }

        public virtual string PartName { get; set; }
    }
}
