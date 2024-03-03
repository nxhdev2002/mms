using System.Collections.Generic;
using Abp.Application.Services.Dto;
using prod.Editions.Dto;

namespace prod.Web.Areas.App.Models.Common
{
    public interface IFeatureEditViewModel
    {
        List<NameValueDto> FeatureValues { get; set; }

        List<FlatFeatureDto> Features { get; set; }
    }
}