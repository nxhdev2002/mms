using System.Collections.Generic;

namespace prod.MultiTenancy.HostDashboard.Dto
{
    public class GetEditionTenantStatisticsOutput
    {
        public GetEditionTenantStatisticsOutput(List<TenantEdition> editionStatistics)
        {
            EditionStatistics = editionStatistics;
        }

        public List<TenantEdition> EditionStatistics { get; set; }
    }
}