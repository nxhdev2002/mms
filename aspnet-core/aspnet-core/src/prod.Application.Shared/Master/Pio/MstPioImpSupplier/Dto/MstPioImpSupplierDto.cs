using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;

namespace prod.Master.Pio.Dto
{
    public class MstPioImpSupplierDto : EntityDto<long?>
    {
        public virtual string SupplierNo { get; set; }
        public virtual string SupplierName { get; set; }
        public virtual string Remarks { get; set; }
        public virtual string SupplierType { get; set; }
        public virtual string SupplierNameVn { get; set; }       
        public virtual string Exporter { get; set; }       
        public virtual string IsActive { get; set; }

    }

    public class CreateOrEditMstPioImpSupplierDto : EntityDto<long?>
    {

        public const int MaxSupplierNoLength = 10;
        public const int MaxSupplierNameLength = 50;
        public const int MaxRemarksLength = 100;
        public const int MaxSupplierTypeLength = 10;
        public const int MaxSupplierNameVnLength = 50;
        public const int MaxExporterLength = 200;
        public const int MaxIsActiveLength = 1;

        [StringLength(MaxSupplierNoLength)]
        public virtual string SupplierNo { get; set; }
        [StringLength(MaxSupplierNameLength)]
        public virtual string SupplierName { get; set; }
        [StringLength(MaxRemarksLength)]
        public virtual string Remarks { get; set; }
        [StringLength(MaxSupplierTypeLength)]
        public virtual string SupplierType { get; set; }

        [StringLength(MaxSupplierNameVnLength)]
        public virtual string SupplierNameVn { get; set; }

        [StringLength(MaxExporterLength)]
        public virtual string Exporter { get; set; }
        [StringLength(MaxIsActiveLength)]
        public virtual string IsActive { get; set; }

    }

    public class GetMstPioImpSupplierInput : PagedAndSortedResultRequestDto
    {
        public virtual string SupplierNo { get; set; }
        public virtual string SupplierName { get; set; }
        public virtual string SupplierType { get; set; }
    } 
    
    public class GetMstPioImpSupplierExportInput 
    {
        public virtual string SupplierNo { get; set; }
        public virtual string SupplierName { get; set; }
        public virtual string SupplierType { get; set; }
    }
}
