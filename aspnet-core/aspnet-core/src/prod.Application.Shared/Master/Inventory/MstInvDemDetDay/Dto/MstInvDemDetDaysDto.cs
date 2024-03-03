using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace prod.Master.Inventory.Dto
{
	public class MstInvDemDetDaysDto : EntityDto<long?>
	{
		public virtual string Exp { get; set; }
		public virtual string Carrier { get; set; }
		public virtual string CombineDEMDET { get; set; }
		public virtual int FreeDEM { get; set; }
		public virtual int FreeDET { get; set; }
		public virtual int CombineDEMDETFree { get; set; }
	}

	public class CreateOrEditMstInvDemDetDaysDto : EntityDto<long?>
	{
		public const int MaxExpLength = 50;
		public const int MaxCarrierLength = 50;
		public const int MaxCombineDEMDETLength = 50;

		[StringLength(MaxExpLength)]
		public virtual string Exp { get; set; }
		[StringLength(MaxCarrierLength)]
		public virtual string Carrier { get; set; }
		[StringLength(MaxCombineDEMDETLength)]
		public virtual string CombineDEMDET { get; set; }
		public virtual int FreeDEM { get; set; }
		public virtual int FreeDET { get; set; }
		public virtual int CombineDEMDETFree { get; set; }
	}
	public class GetMstInvDemDetDaysInput : PagedAndSortedResultRequestDto
	{

		public virtual string Exp { get; set; }
		public virtual string Carrier { get; set; }
		public virtual string CombineDEMDET { get; set; }
		public virtual int FreeDEM { get; set; }
		public virtual int FreeDET { get; set; }
		public virtual int CombineDEMDETFree { get; set; }

	}
	public class MstInvDemDetDaysImportDto
	{
		public virtual string Exp { get; set; }
		public virtual string Carrier { get; set; }
		public virtual string CombineDEMDET { get; set; }
		public virtual int FreeDEM { get; set; }
		public virtual int FreeDET { get; set; }
		public virtual int CombineDEMDETFree { get; set; }
		public virtual string Guid { get; set; }
        public virtual string ErrorDescription { get; set; }
    }

    public class GetExpDto
    {
        public string Exp { get; set; }
    }
    public class GetCarrierDto
    {
        public string Carrier { get; set; }
    }
}
