using Abp.Dapper.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using prod.LogW.Mwh.Dto;
using prod.LogW.Pup;

namespace prod.LogW.Mwh
{
    public class StockAppService : prodAppServiceBase, IStockAppService
    {
        private readonly IDapperRepository<LgwPupPxPUpPlan, long> _dapperRepo;

        public StockAppService(IDapperRepository<LgwPupPxPUpPlan, long> dapperRepo)
        {
            _dapperRepo = dapperRepo;
        }


        //
        #region SMALL PART GETDATA

        public async Task<List<object>> StockGetData(string p_screen_mode)
        {
            Dapper.SqlMapper.Settings.CommandTimeout = 300;
            List<object> list = new List<object>();

            //Small Part Data
            string _sql1 = "Exec LGW_MWH_STOCK_AT_WAREHOUSE_DATA";
            var data1 = await Task.Run(() => _dapperRepo.QueryAsync<StockDto>(_sql1, new {}));
            var rsStockData = from o in data1
                                 select new StockDto
                                 {
                                     CaseNo = o.CaseNo,
                                     SupplierNo = o.SupplierNo,
                                     CasePrefix = o.CasePrefix,
                                     Type = o.Type,
                                     Sort = o.Sort,
                                     Renban = o.Renban
                                 };

            //Small Part Ovf Data
            string _sql2 = "Exec LGW_MWH_STOCK_AT_WAREHOUSE_OVF_DATA";
            var data2 = await _dapperRepo.QueryAsync<StockDto>(_sql2, new {});
            var rsStockOvfData = from o in data2
                                   select new StockDto
                                   {
                                       RowId = o.RowId,
                                       CaseNo = o.CaseNo,
                                       CasePrefix = o.CasePrefix,
                                       ColumnAlias = o.ColumnAlias,
                                       Type = o.Type,
                                       Sort = o.Sort
                                   };


            //Small Part Rob
            string _sql3 = "Exec LGW_MWH_STOCK_AT_WAREHOUSE_ROBBING_DATA";
            var data3 = await _dapperRepo.QueryAsync<StockDto>(_sql3, new {});
            var rsStockRob = from o in data3
                                    select new StockDto
                                    {
                                        LaneNo = o.LaneNo,
                                        LaneName = o.LaneName,
                                        ContNo = o.ContNo,
                                        DevDate = o.DevDate,
                                        Renban = o.Renban,
                                        CaseNo = o.CaseNo,
                                        ShowOnly = o.ShowOnly,
                                        IsActive = o.IsActive,
                                        CallFlag = o.CallFlag
                                    };


            //Add obj to list
            list.Add(rsStockData);
            list.Add(rsStockOvfData);
            list.Add(rsStockRob);
            return list;
        }
        #endregion


        //
        #region SMALL PART LOAD FORM

        public async Task<List<object>> StockGetDataLoadForm()
        {
            Dapper.SqlMapper.Settings.CommandTimeout = 300;
            List<object> list = new List<object>();

            string _sql1 = "Exec LGW_MWH_STOCK_AT_WAREHOUSE_BY_SCREEN @p_screen_mode";
            var data1 = await Task.Run(() => _dapperRepo.QueryAsync<StockDto>(_sql1, new { @p_screen_mode = "1" }));
            var rstNextUp = from o in data1
                                  select new StockDto
                                  {
                                      Id = o.Id,
                                      Renban = o.Renban,
                                      CaseNo = o.CaseNo,
                                      SupplierNo = o.SupplierNo,
                                      TriggerCall = o.TriggerCall,
                                      MinModule = o.MinModule,
                                      MaxModule = o.MaxModule,
                                      ModuleCapacity = o.ModuleCapacity,
                                      ModuleSize = o.ModuleSize,
                                      WhLoc = o.WhLoc,
                                      IsSmall = o.IsSmall,
                                      MinMod = o.MinMod,
                                      MaxMod = o.MaxMod,
                                      CaseOrder = o.CaseOrder,
                                      CaseType = o.CaseType,
                                      Remark = o.Remark,
                                      ProdLine = o.ProdLine,
                                      Cfc = o.Cfc,
                                      IsActive = o.IsActive,
                                      UpDate = o.UpDate,
                                      UpTime = o.UpTime
                                  };

            string _sql2 = "Exec LGW_MWH_STOCK_AT_WAREHOUSE_OVF";
            var data2 = await Task.Run(() => _dapperRepo.QueryAsync<StockDto>(_sql2, new { }));
            var rsPartOvf = from o in data2
                                  select new StockDto
                                  {
                                      RowId = o.RowId,
                                      ColumnAlias = o.ColumnAlias
                                  };


            string _sql3 = "Exec LGW_MWH_STOCK_AT_WAREHOUSE_ROBBING_DATA";
            var data3 = await Task.Run(() => _dapperRepo.QueryAsync<StockDto>(_sql3, new { }));
            var rsPartRob = from o in data3
                            select new StockDto
                            {
                                LaneNo = o.LaneNo,
                                LaneName = o.LaneName,
                                ContNo = o.ContNo,
                                DevDate = o.DevDate,
                                Renban = o.Renban,
                                CaseNo = o.CaseNo,
                                ShowOnly = o.ShowOnly,
                                IsActive = o.IsActive,
                                CallFlag = o.CallFlag
                            };


            //Add obj to list
            list.Add(rstNextUp);
            list.Add(rsPartOvf);
            list.Add(rsPartRob);
            return list;
        }

        #endregion

    }
}
