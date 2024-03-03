using Abp.Application.Services.Dto;
using System;
using System.ComponentModel.DataAnnotations;
namespace prod.Inventory.CKD.Dto
{

    public class InvCkdContainerRentalWHPlanTDto 
    {
        public virtual string RowNumber { get; set; }
        public virtual string Guid { get; set; }

        public virtual string ContainerNo { get; set; }

        public virtual string Renban { get; set; }   

        public virtual string SealNo { get; set; }

        public virtual string ListcaseNo { get; set; }
      
        public virtual DateTime? DevanningDate { get; set; }
      
        public virtual DateTime? GateInPlanDate { get; set; }

        public virtual string Transport { get; set; }

        public virtual DateTime? DevanningTime { get; set; }

        public virtual DateTime? GateInPlanTime { get; set; }

        public virtual string ErrorDescription { get; set; }

        public virtual DateTime? CreationTime { get; set; }

        public virtual int CreatorUserId { get; set; }

        public virtual DateTime? LastModificationTime { get; set; }

        public virtual int LastModifierUserId { get; set; }

        public virtual int IsDeleted { get; set; }

        public virtual int DeleterUserId { get; set; }


    }


}


