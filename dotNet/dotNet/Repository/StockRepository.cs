using Dapper;
using dotNet.Interface;
using dotNet.DBModels;
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
        public async Task<IEnumerable<JoinTable>> JoinAllTable(int startIndex,int pageSize,string sortColumn, DateTime startDate, DateTime endDate, string tradeType, string stockId, string sortDirection)
        {
            if (sortColumn == "Price")
                sortColumn = "ClosingPriceTable.Price";
            //else if (sortColumn == "ReturnDate")
            //    sortColumn = $"TradeTable.TradeDate {sortDirection}, TradeTable.LendingPeriod";
            else
                sortColumn = $"TradeTable.{sortColumn}";
            var startDateTime= startDate.ToString("yyyy-MM-dd HH:mm:ss");
            var endDateTime = endDate.ToString("yyyy-MM-dd HH:mm:ss");
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
                    TradeTable.Status,
                    TradeTable.ReturnDate
                FROM 
                    TradeTable
                    LEFT JOIN StockTable ON TradeTable.StockId = StockTable.StockId
                    LEFT JOIN ClosingPriceTable ON TradeTable.StockId = ClosingPriceTable.StockId
                                                AND TradeTable.TradeDate = ClosingPriceTable.TradeDate
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
                sqlQuery += @$" ORDER BY {sortColumn} {sortDirection}
                                OFFSET {startIndex}ROWS FETCH NEXT {pageSize}ROWS ONLY";
                var result =await conn.QueryAsync<JoinTable>(sqlQuery);
                return result;
            }
            
        }

        public async Task<int> GetJoinAllTableCount(DateTime startDate, DateTime endDate, string tradeType, string stockId)
        {
            var startDateTime = startDate.ToString("yyyy-MM-dd HH:mm:ss");
            var endDateTime = endDate.ToString("yyyy-MM-dd HH:mm:ss");
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
                    TradeTable.Status,
                    TradeTable.ReturnDate
                FROM 
                    TradeTable
                    LEFT JOIN StockTable ON TradeTable.StockId = StockTable.StockId
                    LEFT JOIN ClosingPriceTable ON TradeTable.StockId = ClosingPriceTable.StockId
                                                AND TradeTable.TradeDate = ClosingPriceTable.TradeDate
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
                    TradeTable.Status,
                    TradeTable.ReturnDate
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
