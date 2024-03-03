using Abp.AutoMapper;
using prod.MultiTenancy;
using prod.MultiTenancy.Dto;
using prod.Web.Areas.App.Models.Common;

namespace prod.Web.Areas.App.Models.Tenants
{
    [AutoMapFrom(typeof (GetTenantFeaturesEditOutput))]
    public class TenantFeaturesEditViewModel : GetTenantFeaturesEditOutput, IFeatureEditViewModel
    {
        public Tenant Tenant { get; set; }
    }
}