using Dapper;
using dotNet.Interface;
using dotNet.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotNet.Repository
{
    public class StockRepository : IStockRepository
    {
        private readonly string _connectString;
        public  StockRepository(IConfiguration configuration)
        {
            Configuration = configuration;
            _connectString = configuration.GetValue<string>("DBConnectionString");
        }
        public IConfiguration Configuration { get; }


        ///join只取status不為2的資料
        ///sortColumn默認為id
        ///sortDirection默認為正序 反序=DESC
        public async Task<IEnumerable<JoinTable>> JoinAllTable(int startIndex,int pageSize,string sortColumn, string startDate, string endDate, string tradeType, string stockId, string sortDirection)
        {
            var startDateTime= DateTime.Parse(startDate);
            var endDateTime = DateTime.Parse(endDate);
            using (var conn = new SqlConnection(_connectString))
            {
                string sqlQuery = $@"
                SELECT 
                    TradeTable.Id,
                    TradeTable.TradeDate,
                    StockTable.StockId,
                    StockTable.Name,
                    TradeTable.Type,
                    TradeTable.Volume,
                    TradeTable.Fee,
                    ClosingPriceTable.Price,
                    TradeTable.LendingPeriod,
                    TradeTable.Status
                FROM 
                    TradeTable
                    LEFT JOIN StockTable ON TradeTable.StockId = StockTable.StockId
                    LEFT JOIN ClosingPriceTable ON TradeTable.StockId = ClosingPriceTable.StockId
                                                AND TradeTable.TradeDate = ClosingPriceTable.TradeDate
                WHERE
                    TradeTable.Status <> 2
                    AND TradeTable.TradeDate >='{startDate} 00:00:00' AND  TradeTable.TradeDate <='{endDate} 00:00:00'";

                if (tradeType != null)
                {
                    sqlQuery += $" AND TradeTable.Type = '{tradeType}'";
                }
                if (stockId != null)
                {
                    sqlQuery += $" AND TradeTable.StockId = '{stockId}'";
                }
                sqlQuery += @$" ORDER BY TradeTable.{sortColumn} {sortDirection}
                                OFFSET {startIndex}ROWS FETCH NEXT {pageSize}ROWS ONLY";
                var result =await conn.QueryAsync<JoinTable>(sqlQuery);
                return result;
            }
            
        }

        public async Task<int> GetJoinAllTableCount(int startIndex, int pageSize, string sortColumn, string startDate, string endDate, string tradeType, string stockId, string sortDirection)
        {
            var startDateTime = DateTime.Parse(startDate);
            var endDateTime = DateTime.Parse(endDate);
            using (var conn = new SqlConnection(_connectString))
            {
                string sqlQuery = $@"
                SELECT 
                    TradeTable.Id,
                    TradeTable.TradeDate,
                    StockTable.StockId,
                    StockTable.Name,
                    TradeTable.Type,
                    TradeTable.Volume,
                    TradeTable.Fee,
                    ClosingPriceTable.Price,
                    TradeTable.LendingPeriod,
                    TradeTable.Status
                FROM 
                    TradeTable
                    LEFT JOIN StockTable ON TradeTable.StockId = StockTable.StockId
                    LEFT JOIN ClosingPriceTable ON TradeTable.StockId = ClosingPriceTable.StockId
                                                AND TradeTable.TradeDate = ClosingPriceTable.TradeDate
                WHERE
                    TradeTable.Status <> 2
                    AND TradeTable.TradeDate >='{startDate} 00:00:00' AND  TradeTable.TradeDate <='{endDate} 00:00:00'";

                if (tradeType != null)
                {
                    sqlQuery += $" AND TradeTable.Type = '{tradeType}'";
                }
                if (stockId != null)
                {
                    sqlQuery += $" AND TradeTable.StockId = '{stockId}'";
                }

                var result = await conn.QueryAsync<JoinTable>(sqlQuery);
                return result.Count();
            }

        }

        ///join某id的資料
        public async Task<JoinTable> JoinAllTableById(int id)
        {
            using (var conn = new SqlConnection(_connectString))
            {
                string sqlQuery = $@"
                SELECT 
                    TradeTable.Id,
                    TradeTable.TradeDate,
                    StockTable.StockId,
                    StockTable.Name,
                    TradeTable.Type,
                    TradeTable.Volume,
                    TradeTable.Fee,
                    ClosingPriceTable.Price,
                    TradeTable.LendingPeriod,
                    TradeTable.Status
                FROM 
                    TradeTable
                    LEFT JOIN StockTable ON TradeTable.StockId = StockTable.StockId
                    LEFT JOIN ClosingPriceTable ON TradeTable.StockId = ClosingPriceTable.StockId
                                                AND TradeTable.TradeDate = ClosingPriceTable.TradeDate
                WHERE
                    TradeTable.Id = {id}";


                var result = await conn.QueryAsync<JoinTable>(sqlQuery);
                return result.FirstOrDefault();
            }

        }
    }
}
