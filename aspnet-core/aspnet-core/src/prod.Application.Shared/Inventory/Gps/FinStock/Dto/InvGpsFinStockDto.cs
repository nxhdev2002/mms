using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace prod.Inventory.Gps.FinStock.Dto
{
    public class InvGpsFinStockDto : EntityDto<long?>
    {
        public string PartNo { get; set; }
        public string PartName { get; set; }
        public long? PartId { get; set; }
        public long? Qty { get; set; }
        public long? Price { get; set; }
        public DateTime? WorkingDate { get; set; }
        public long? TransactionId { get; set; }
        public string Dock { get; set; }
        public string Location { get; set; }
    }

    public class InvGpsFinStockImput : PagedAndSortedResultRequestDto
    {
        public string PartNo { get; set; }
        public string Location { get; set; }
    }

    public class InvGpsFinStockImportDto
    {
        public string PartNo { get; set; }
        public long? Qty { get; set; }
        public string Location { get; set; }
        public virtual long? ROW_NO { get; set; }

        [StringLength(128)]
        public virtual string Guid { get; set; }
        public string ErrorDescription { get; set; }
        public bool CanBeImported()
        {
            return string.IsNullOrEmpty(ErrorDescription);
        }

    }




}
