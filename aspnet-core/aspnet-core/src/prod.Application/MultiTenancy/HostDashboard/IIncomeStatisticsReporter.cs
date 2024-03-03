using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using prod.MultiTenancy.HostDashboard.Dto;

namespace prod.MultiTenancy.HostDashboard
{
    public interface IIncomeStatisticsService
    {
        Task<List<IncomeStastistic>> GetIncomeStatisticsData(DateTime startDate, DateTime endDate,
            ChartDateInterval dateInterval);
    }
}