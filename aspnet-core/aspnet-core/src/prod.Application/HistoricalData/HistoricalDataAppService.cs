using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Dapper.Repositories;
using Abp.Domain.Entities;
using Abp.Json;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using NPOI.SS.Formula.Functions;
using prod.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Twilio.Rest.Api.V2010.Account;

namespace prod.HistoricalData
{
    public interface IHistoricalDataAppService : IApplicationService
    {
        Task<List<string>> GetHistoricalDataById(long id, string tableName, string primaryKeyColumnName, string[] includeCols=null);
        Task<List<string>> GetHistoricalDataByFilter(string tableName, DateTime? fromDate, DateTime? toDate, int?[] CdcAction, string[] includeCols = null);
        Task<List<long?>> GetChangedRecordIds(string tableName);
    }
    public class HistoricalDataAppService : prodAppServiceBase, IHistoricalDataAppService
    {
        private const string CommandId = "__$command_id";
        private const string Action = "__$operation";
        private const string StartLogNum = "__$start_lsn";
        private const string UpdateMask = "__$update_mask";
        private const string ChangedColumnList = "ChangedColumns";
        private const string UpdatedDateTime = "UpdatedDateTime";
        private readonly Dictionary<string, string> FieldJoinMapper = new()
            {
                { "CreatorUserId", "LEFT JOIN AbpUsers c ON ct.CreatorUserId = c.Id " },
                { "LastModifierUserId", "LEFT JOIN AbpUsers l ON ct.LastModifierUserId = l.Id " },
                { "DeleterUserId", "LEFT JOIN AbpUsers d ON ct.DeleterUserId = d.Id " },
        };

        private readonly Dictionary<string, string> FieldSelectMapper = new()
            {
                { "CreatorUserId", "c.Name as Creator, " },
                { "LastModifierUserId", "l.Name as Modifier, " },
                { "DeleterUserId", "d.Name as Deleter, " },
            };

        private readonly Dictionary<string, string> ColNameMapper = new ()
            {
                { "CreatorUserId", "Creator" },
                { "LastModifierUserId", "Modifier" },
                { "DeleterUserId", "Deleter" },
            };

        private readonly List<string> connections = new()
            {
                "Default",
                //"historicData"
            };

        private readonly ILogger<HistoricalDataAppService> _logger;
        public HistoricalDataAppService(ILogger<HistoricalDataAppService> logger)
        {
            _logger = logger;
        }

        public async Task<List<long?>> GetChangedRecordIds(string tableName) 
        {
            var sql = "SELECT DISTINCT Id FROM cdc.dbo_" + tableName + "_CT";
            // query sql variable use connection string
            var result = new List<long?>();
            string connectionString = Commons.getConnectionString();
            // create connection
            using (SqlConnection connection = new(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = sql;
                    connection.Open();
                    try
                    {
                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync().ConfigureAwait(false))
                        {
                            // Process the results
                            while (reader.Read())
                            {
                                result.Add((long?)reader["Id"]);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(message: ex?.ToString());
                    }
                }
            }
            return result;

        }

        public async Task<List<string>> GetHistoricalDataById(long id, string tableName, string primaryKeyColumnName, string[] includeCols = null)
        {
            var result = new List<string>();
            var columnChanges = await GetChangedColumn(id, tableName, primaryKeyColumnName);
            foreach (var connection in connections)
            {
                string connectionString = Commons.getConnectionString(connection);
                if (!string.IsNullOrEmpty(connectionString))
                result.AddRange( await GetInfoChangedById(columnChanges, id, tableName, primaryKeyColumnName, connectionString, includeCols) );
            }

            return result;
        }

        public async Task<List<string>> GetHistoricalDataByFilter(string tableName, DateTime? fromDate, DateTime? toDate, int?[] CdcAction, string[] includeCols = null)
        {
            var result = new List<string>();
            foreach (var connection in connections)
            {
                string connectionString = Commons.getConnectionString(connection);
                if (!string.IsNullOrEmpty(connectionString))
                    result.AddRange(await GetInfoChangedByFilter(tableName, fromDate, toDate, CdcAction, connectionString, includeCols));
            }

            return result;
        }



        private async Task<List<string>> GetInfoChangedById(List<ColumnChange> inputs, long id, string tableName, string primaryKeyColumnName, string connectionString, string[] includeCols)
        {
            var result = new List<string>();


            using (SqlConnection connection = new(connectionString))
            {
                connection.Open();

                foreach (var item in inputs)
                {
                    var colChanged = item.ColumnChanged;

                    string[] keys1 = colChanged.Split(',').Select(key => key.Trim()).ToArray();
                    string[] keys2 = includeCols?.Select(x => "ct.[" + x + "]").ToArray();

                    // Combine the arrays and create a distinct list of keys
                    var mergedKeys = keys1.Concat(keys2 ?? new string[0]).Distinct().ToArray();

                    // Create a new string by joining the distinct keys
                    colChanged = string.Join(", ", mergedKeys);

                    var queryStr = BuildQueryString(colChanged);

                    var queryBuilder = new StringBuilder($"""
                            SELECT {colChanged},
                                    {Action} AS {nameof(Action)}, {queryStr.SelectQuery}
                                    CONVERT(VARCHAR(1000), {UpdateMask}, 1) AS {nameof(UpdateMask)},
                                    sys.fn_cdc_map_lsn_to_time({StartLogNum}) as {UpdatedDateTime}
                            FROM cdc.dbo_{tableName}_CT ct
                            {queryStr.JoinQuery}
                            WHERE ct.{primaryKeyColumnName} = {id} AND {UpdateMask} = {item.UpdateMask}
                        """);

                    string query = queryBuilder.ToString();

                    using (SqlCommand cmd = new())
                    {
                        cmd.Connection = connection;
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = query;
                        try
                        {
                            using (SqlDataReader reader = await cmd.ExecuteReaderAsync().ConfigureAwait(false))
                            {
                                // Process the results
                                while (reader.Read())
                                {
                                    var expando = new ExpandoObject();
                                    var expandoDict = expando as IDictionary<string, object>;

                                    expandoDict.Add(nameof(Action), reader[nameof(Action)]);
                                    expandoDict.Add(nameof(UpdatedDateTime), reader[nameof(UpdatedDateTime)]);
                                    expandoDict.Add(nameof(UpdateMask), reader[nameof(UpdateMask)]);

                                    foreach (var col in colChanged.TrimEnd(',').Split(','))
                                    {
                                        var column = col.Replace("ct.", "").Replace("[", "").Replace("]", "").Trim();
                                        var value = reader[column];
                                        if (expandoDict.ContainsKey(column))
                                        {
                                            if (value != null && value != System.DBNull.Value)
                                                expandoDict[column] = value;
                                        }
                                        else
                                        {
                                            if (value != null && value != System.DBNull.Value)
                                                expandoDict.Add(column, value);
                                        }
                                    }

                                    // thêm các trường fullname vào expando
                                    // nếu colCnceled chứa các trường CreatorUserId, LastModifierUserId, DeleterUserId thì thêm các trường fullname vào expando
                                    foreach (var item2 in ColNameMapper)
                                    {
                                        if (colChanged.Contains(item2.Key))
                                        {
                                            if (reader[item2.Value] != null && reader[item2.Value] != System.DBNull.Value)
                                            {
                                                expandoDict.Add(item2.Value, reader[item2.Value]);
                                            }
                                        }
                                    }

                                    result.Add(expando.ToJsonString());
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(message: ex.ToString());
                        }
                    }
                }

                connection.Close();
            }

            return result;
        }

        private async Task<List<string>> GetInfoChangedByFilter(string tableName, DateTime? fromDate, DateTime? toDate, int?[] CdcAction, string connectionString, string[] includeCols)
        {
            var result = new List<string>();

            fromDate ??= new DateTime(1990, 01, 01);
            toDate ??= new DateTime(2100, 01, 01);

            string fromDateStr = fromDate.Value.ToString("yyyy-MM-dd");
            string toDateStr = toDate.Value.ToString("yyyy-MM-dd");


            using (SqlConnection connection = new(connectionString))
            {
                connection.Open();

                var queryBuilder = new StringBuilder($"EXEC [HIST_GET_HISTORICAL_DATA] '{tableName}', '{fromDateStr}', '{toDateStr}'");

                string query = queryBuilder.ToString();

                using (SqlCommand cmd = new())
                {
                    cmd.Connection = connection;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = query;
                    try
                    {
                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync().ConfigureAwait(false))
                        {
                            // Process the results
                            while (reader.Read())
                            {
                                // if cdcAction ot null and not contain action then continue
                                if (CdcAction != null && !CdcAction.Contains((int)reader[Action]))
                                {
                                    continue;
                                }

                                var colChanged = reader["ChangedColumns"].ToString();
                                string[] keys1 = colChanged.TrimEnd(',').Split(',').Select(key => key.Trim()).ToArray();

                                // Combine the arrays and create a distinct list of keys
                                var mergedKeys = keys1.Concat(includeCols ?? new string[0]).Distinct().ToArray();

                                // Create a new string by joining the distinct keys
                                colChanged = string.Join(",", mergedKeys);

                                var expando = new ExpandoObject();
                                var expandoDict = expando as IDictionary<string, object>;

                                expandoDict.Add(nameof(Action), reader[nameof(Action)]);
                                expandoDict.Add(nameof(UpdatedDateTime), reader[nameof(UpdatedDateTime)]);

                                foreach (var column in colChanged.TrimEnd(',').Split(','))
                                {
                                    var value = reader[column];
                                    if (expandoDict.ContainsKey(column))
                                    {
                                        if (value != null && value != System.DBNull.Value)
                                        {
                                            expandoDict[column] = value;
                                        }
                                    }
                                    else
                                    {
                                        if (value != null && value != System.DBNull.Value)
                                        {
                                            expandoDict.Add(column, value);
                                        }
                                    }
                                } 

                                result.Add(expando.ToJsonString());
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(message: ex.ToString());
                    }
                }

                connection.Close();
            }

            return result;
        }


        private async Task<List<ColumnChange>> GetChangedColumn(long id, string tableName, string primaryKeyColumnName)
        {
            var result = new List<ColumnChange>();
            string connectionString = Commons.getConnectionString();


            using (SqlConnection connection = new(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    //TODO: change to parameter, not direct inject
                    var queryBuilder = new StringBuilder(
                        $"""
                        SELECT DISTINCT
                                CONVERT(VARCHAR(1000), {UpdateMask}, 1) AS {UpdateMask},
                                (SELECT    '['+ CC.column_name + '],'
                                  FROM      cdc.captured_columns CC
                                            INNER JOIN cdc.change_tables CT ON CC.[object_id] = CT.[object_id]
                                  WHERE     capture_instance = 'dbo_{tableName}'
                                            AND sys.fn_cdc_is_bit_set(CC.column_ordinal,
                                                                      PD.{UpdateMask}) = 1
                                FOR
                                  XML PATH('')
                                ) AS {ChangedColumnList}
                        FROM    cdc.dbo_{tableName}_CT PD where {primaryKeyColumnName} = {id}
                        """
                        );
                    string query = queryBuilder.ToString();

                    cmd.Connection = connection;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = query;
                    
                    connection.Open();

                    try
                    {
                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync().ConfigureAwait(false))
                        {
                            // Process the results
                            while (reader.Read())
                            {
                                string columnChanged = reader[ChangedColumnList].ToString();
                                if (columnChanged != null)
                                {
                                    columnChanged = string.Join(", ", columnChanged.TrimEnd(',').Split(',').Select(c => "ct." + c));
                                }
                                // thêm alias là ct vào các trường cần lấy.
                                result.Add(new ColumnChange
                                {
                                    ColumnChanged = columnChanged,
                                    UpdateMask = reader[UpdateMask].ToString(),
                                });
                            }
                            connection.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(message: ex?.ToString());
                    }
                }
            }

            return result;
        }

        private BuildQueryStringResult BuildQueryString(string colChanged)
        {
            var selectQuery = new StringBuilder();
            var joinQuery = new StringBuilder();

            
            // nếu colChanged chứa các trường CreatorUserId, LastModifierUserId, DeleterUserId thì thêm các join vào joinQuery
            foreach (var item in FieldJoinMapper)
            {
                if (colChanged.Contains(item.Key))
                {
                    joinQuery.Append(item.Value);
                }
            }

            // nếu colChanged chứa các trường CreatorUserId, LastModifierUserId, DeleterUserId thì thêm các trường fullname vào selectQuery
            foreach (var item in FieldSelectMapper)
            {
                if (colChanged.Contains(item.Key))
                {
                    selectQuery.Append(item.Value);
                }
            }
            
            return new BuildQueryStringResult
            {
                SelectQuery = selectQuery.ToString().Trim(','),
                JoinQuery = joinQuery.ToString(),
            };

        }

        private class BuildQueryStringResult
        {
            public string SelectQuery { get; set; }
            public string JoinQuery { get; set; }
        }

        private class ColumnChange
        {
            public string UpdateMask { get; set; }
            public string ColumnChanged { get; set; }
        }
    }
}
