using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Abp.Application.Services.Dto;

namespace prod.Inventory.Gps.User.Dto
{
    public class InvGpsUserDto : EntityDto<long?>
    {
        public virtual string EmployeeCode { get; set; }

        public virtual string Name { get; set; }

        public virtual string Shop { get; set; }

        public virtual string Team { get; set; }

        public virtual string CostCenter { get; set; }

        public virtual string Group { get; set; }

        public virtual string SubGroup { get; set; }

        public virtual string Division { get; set; }

        public virtual string Dept { get; set; }
    }

    public class GetInvGpsUserInput : PagedAndSortedResultRequestDto
    {
        public virtual string EmployeeCode { get; set; }

        public virtual string Name { get; set; }


    }
    public class GetInvGpsUserExportInput 
    {
        public virtual string EmployeeCode { get; set; }

        public virtual string Name { get; set; }


    }
}
