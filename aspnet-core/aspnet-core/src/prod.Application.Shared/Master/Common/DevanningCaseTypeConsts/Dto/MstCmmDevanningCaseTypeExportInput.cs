using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Master.Common.Dto
{

    public class MstCmmDevanningCaseTypeExportInput
    {

        public virtual string CaseNo { get; set; }

        public virtual string CarFamilyCode { get; set; }

        public virtual string ShoptypeCode { get; set; }

        public virtual string SupplierNo { get; set; }

        public virtual string IsActive { get; set; }

    }

}


