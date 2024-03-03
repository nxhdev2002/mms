using Abp.AutoMapper;
using prod.MultiTenancy.Dto;

namespace prod.Web.Models.TenantRegistration
{
    [AutoMapFrom(typeof(EditionsSelectOutput))]
    public class EditionsSelectViewModel : EditionsSelectOutput
    {
    }
}
