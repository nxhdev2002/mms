using System.Collections.Generic;
using prod.Caching.Dto;

namespace prod.Web.Areas.App.Models.Maintenance
{
    public class MaintenanceViewModel
    {
        public IReadOnlyList<CacheDto> Caches { get; set; }
    }
}