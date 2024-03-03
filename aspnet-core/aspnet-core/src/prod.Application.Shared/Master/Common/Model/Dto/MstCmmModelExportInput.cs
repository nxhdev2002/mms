using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Master.Cmm.Dto
{

	public class MstCmmModelExportInput
	{

        public virtual string Code { get; set; }

        public virtual string Name { get; set; }

        public virtual string Cfc { get; set; }

        public virtual string ModelVin { get; set; }

        public virtual string ModelCode { get; set; }

        public virtual string IsActive { get; set; }

    }

}


