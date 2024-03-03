using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Master.Inventory.Dto
{

    public class MstInvHrOrgStructureDto  
    {

        public virtual string Id { get; set; }

        public virtual string Code { get; set; }

        public virtual string Name { get; set; }

        public virtual string Description { get; set; }

        public virtual int Published { get; set; }

        public virtual string Orgstructuretypename { get; set; }

        public virtual string Orgstructuretypecode { get; set; }

        public virtual string Parentid { get; set; }

        public virtual string IsActive { get; set; }

    }

    public class GetMstInvHrOrgStructureInput : PagedAndSortedResultRequestDto
    {

     /*   public virtual string Id { get; set; }*/

        public virtual string Code { get; set; }

        public virtual string Name { get; set; }

   /*     public virtual string Description { get; set; }

        public virtual int Published { get; set; }

        public virtual string Orgstructuretypename { get; set; }

        public virtual string Orgstructuretypecode { get; set; }

        public virtual string Parentid { get; set; }

        public virtual string IsActive { get; set; }*/

    }

}
