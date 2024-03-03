using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace prod.Master.Inventory.MstInvHrEmployee.Dto
{
    public class MstInvHrEmployeeDto : EntityDto<long?>
    {
        public string EmployeeCode { get; set; }
        public string UserName { get; set; }
        public string EmailAddress { get; set; }
        public string TitleName { get; set; }
        public string PositionName { get; set; }
        public string OrgStructureName { get; set; }
        public bool IsActive { get; set; }
    }

    public class MstInvHrEmployeeInput : PagedAndSortedResultRequestDto
    {
        public string p_employee_code { get; set; }
        public string p_name { get; set; }
        public string p_email_address { get; set; }
    }

    public class MstInvHrEmployeeInputExcel
    { 
        public string p_employee_code { get; set; }
        public string p_name { get; set; }
        public string p_email_address { get; set; }
    }
}
