using Dapper;
using dotNet.DBModels;
using dotNet.Interface;
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

        string queryString = $@"
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
                    TradeTable.Status,
                    TradeTable.ReturnDate
                FROM 
                    TradeTable
                    LEFT JOIN StockTable ON TradeTable.StockId = StockTable.StockId
                    LEFT JOIN ClosingPriceTable ON TradeTable.StockId = ClosingPriceTable.StockId
                                                AND TradeTable.TradeDate = ClosingPriceTable.TradeDate";
        ///join只取status不為2的資料
        ///sortColumn默認為id
        ///sortDirection默認為正序 反序=DESC
        public async Task<IEnumerable<JoinTable>> JoinAndFilterAllTable(int startIndex,int pageSize,string sortColumn, DateTime startDate, DateTime endDate, string tradeType, string stockId, string sortDirection)
        {
            if (sortColumn == "Price")
                sortColumn = "ClosingPriceTable.Price";
            else
                sortColumn = $"TradeTable.{sortColumn}";
            var startDateTime= startDate.ToString("yyyy-MM-dd HH:mm:ss");
            var endDateTime = endDate.ToString("yyyy-MM-dd HH:mm:ss");
            using (var conn = new SqlConnection(_connectString))
            {
                string sqlQuery = queryString+$@"
                WHERE
                    TradeTable.Status <> 2
                    AND TradeTable.TradeDate >='{startDateTime}' AND  TradeTable.TradeDate <='{endDateTime}'";

                if (tradeType != "")
                {
                    sqlQuery += $" AND TradeTable.Type = '{tradeType}'";
                }
                if (stockId != "")
                {
                    sqlQuery += $" AND TradeTable.StockId LIKE '%{stockId}%'";
                }
                sqlQuery += @$" ORDER BY {sortColumn} {sortDirection}
                                OFFSET {startIndex}ROWS FETCH NEXT {pageSize}ROWS ONLY";
                var result =await conn.QueryAsync<JoinTable>(sqlQuery);
                return result;
            }
            
        }

        /// 取出JoinAllTable的data count 前端顯示頁數用
        public async Task<int> GetJoinAndFilterAllTableCount(DateTime startDate, DateTime endDate, string tradeType, string stockId)
        {
            var startDateTime = startDate.ToString("yyyy-MM-dd HH:mm:ss");
            var endDateTime = endDate.ToString("yyyy-MM-dd HH:mm:ss");
            using (var conn = new SqlConnection(_connectString))
            {
                string sqlQuery = queryString+$@"
                WHERE
                    TradeTable.Status <> 2
                    AND TradeTable.TradeDate >='{startDateTime}' AND  TradeTable.TradeDate <='{endDateTime}'";

                if (tradeType != "")
                {
                    sqlQuery += $" AND TradeTable.Type = '{tradeType}'";
                }
                if (stockId != "")
                {
                    sqlQuery += $" AND TradeTable.StockId = '{stockId}'";
                }

                var result = await conn.QueryAsync<JoinTable>(sqlQuery);
                return result.Count();
            }

        }

        ///join某id的資料
        public async Task<JoinTable> GetTradeInfoById(int id)
        {
            using (var conn = new SqlConnection(_connectString))
            {
                string sqlQuery = queryString+$@"
                WHERE
                    TradeTable.Id = {id}";

                var result = await conn.QueryAsync<JoinTable>(sqlQuery);
                return result.First();
            }
        }
    }
}
