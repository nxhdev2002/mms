using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace prod.Inventory.CKD.Dto
{
    public class InvCkdProductionPlanMonthlyDto : EntityDto<long?>
    {
        public virtual string Cfc { get; set; }

        public virtual string Grade { get; set; }

        public virtual DateTime? ProductionMonth { get; set; }

        public virtual int? N_3 { get; set; }

        public virtual int? N_2 { get; set; }

        public virtual int? N_1 { get; set; }

        public virtual int? N { get; set; }

        public virtual int? N1 { get; set; }

        public virtual int? N2 { get; set; }

        public virtual int? N3 { get; set; }

        public virtual int? N4 { get; set; }

        public virtual int? N5 { get; set; }

        public virtual int? N6 { get; set; }

        public virtual int? N7 { get; set; }

        public virtual int? N8 { get; set; }

        public virtual int? N9 { get; set; }

        public virtual int? N10 { get; set; }

        public virtual int? N11 { get; set; }

        public virtual int? N12 { get; set; }

        //Total

        public virtual int? Total_N_3 { get; set; }

        public virtual int? Total_N_2 { get; set; }

        public virtual int? Total_N_1 { get; set; }

        public virtual int? Total_N { get; set; }

        public virtual int? Total_N1 { get; set; }

        public virtual int? Total_N2 { get; set; }

        public virtual int? Total_N3 { get; set; }

        public virtual int? Total_N4 { get; set; }

        public virtual int? Total_N5 { get; set; }

        public virtual int? Total_N6 { get; set; }

        public virtual int? Total_N7 { get; set; }

        public virtual int? Total_N8 { get; set; }

        public virtual int? Total_N9 { get; set; }

        public virtual int? Total_N10 { get; set; }

        public virtual int? Total_N11 { get; set; }

        public virtual int? Total_N12 { get; set; }
    }

    public class InvCkdProductionPlanMonthlyInput : PagedAndSortedResultRequestDto
    {
        public virtual string Cfc { get; set; }

        public virtual string Grade { get; set; }

        public virtual DateTime ProdMonth { get; set; }
        
    }

    public class InvCkdProductionPlanMonthlyImportDto
    {
        public virtual long? ROW_NO { get; set; }

        [StringLength(128)]
        public virtual string Guid { get; set; }

        [StringLength(4)]
        public virtual string Cfc { get; set; }

        [StringLength(3)]
        public virtual string Grade { get; set; }

        public virtual DateTime? ProductionMonth { get; set; }

        public virtual int? N_3 { get; set; }

        public virtual int? N_2 { get; set; }

        public virtual int? N_1 { get; set; }

        public virtual int? N { get; set; }

        public virtual int? N1 { get; set; }

        public virtual int? N2 { get; set; }

        public virtual int? N3 { get; set; }

        public virtual int? N4 { get; set; }

        public virtual int? N5 { get; set; }

        public virtual int? N6 { get; set; }

        public virtual int? N7 { get; set; }

        public virtual int? N8 { get; set; }

        public virtual int? N9 { get; set; }

        public virtual int? N10 { get; set; }

        public virtual int? N11 { get; set; }

        public virtual int? N12 { get; set; }

        public string ErrorDescription { get; set; }
        
    }
}
