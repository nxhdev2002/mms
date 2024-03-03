using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;

namespace prod.Master.LogW.Dto
{

    public class MstLgwEciPartModuleExportInput
    {

        public virtual string PartNo { get; set; }

        public virtual string CaseNo { get; set; }

        public virtual string SupplierNo { get; set; }

        public virtual string ContainerNo { get; set; }

        public virtual string Renban { get; set; }

        public virtual string CasePrefix { get; set; }

        public virtual long? ChkEciPartId { get; set; }

        public virtual string EciType { get; set; }

        public virtual string IsActive { get; set; }

    }

}


