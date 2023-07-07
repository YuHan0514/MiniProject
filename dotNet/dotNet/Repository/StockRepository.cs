using Dapper;
using dotNet.DBModels;
using dotNet.Interface;
using dotNet.ServiceModels;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
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
        public async Task<IEnumerable<JoinTable>> JoinAndFilterAllTable(TradeQueryServiceModel tradeQueryServiceModel)
        {
            if (tradeQueryServiceModel.sortColumn == "Price")
                tradeQueryServiceModel.sortColumn = "ClosingPriceTable.Price";
            else
                tradeQueryServiceModel.sortColumn = $"TradeTable.{tradeQueryServiceModel.sortColumn}";
            var startDateTime= tradeQueryServiceModel.startDate.ToString("yyyy-MM-dd HH:mm:ss");
            var endDateTime = tradeQueryServiceModel.endDate.ToString("yyyy-MM-dd HH:mm:ss");
            string sqlQuery = queryString + $@"
                WHERE
                    TradeTable.Status <> 2
                    AND TradeTable.TradeDate >='{startDateTime}' AND  TradeTable.TradeDate <='{endDateTime}'";

            if (tradeQueryServiceModel.tradeType != "")
            {
                sqlQuery += $" AND TradeTable.Type = '{tradeQueryServiceModel.tradeType}'";
            }
            if (tradeQueryServiceModel.stockId != "")
            {
                sqlQuery += $" AND TradeTable.StockId LIKE '%{tradeQueryServiceModel.stockId}%'";
            }
            sqlQuery += @$" ORDER BY {tradeQueryServiceModel.sortColumn} {tradeQueryServiceModel.sortDirection}
                                OFFSET {tradeQueryServiceModel.pageIndex}ROWS FETCH NEXT {tradeQueryServiceModel.pageSize}ROWS ONLY";
            using (var conn = new SqlConnection(_connectString))
            {
                var result =await conn.QueryAsync<JoinTable>(sqlQuery);
                return result;
            }
            
        }

        /// 取出JoinAllTable的data count 前端顯示頁數用
        public async Task<int> GetJoinAndFilterAllTableCount(TradeQueryServiceModel tradeQueryServiceModel)
        {
            var startDateTime = tradeQueryServiceModel.startDate.ToString("yyyy-MM-dd HH:mm:ss");
            var endDateTime = tradeQueryServiceModel.endDate.ToString("yyyy-MM-dd HH:mm:ss"); 
            string sqlQuery = queryString+$@"
                WHERE
                    TradeTable.Status <> 2
                    AND TradeTable.TradeDate >='{startDateTime}' AND  TradeTable.TradeDate <='{endDateTime}'";

                if (tradeQueryServiceModel.tradeType != "")
                {
                    sqlQuery += $" AND TradeTable.Type = '{tradeQueryServiceModel.tradeType}'";
                }
                if (tradeQueryServiceModel.stockId != "")
                {
                    sqlQuery += $" AND TradeTable.StockId LIKE '%{tradeQueryServiceModel.stockId}%'";
                }

            using (var conn = new SqlConnection(_connectString))
            {
                var result = await conn.QueryAsync<JoinTable>(sqlQuery);
                return result.Count();
            }

        }

        ///join某id的資料
        public async Task<TradeRespServiceModel> GetTradeInfoById(int id)
        {
            string sqlQuery = queryString + $@"
                WHERE
                    TradeTable.Id = {id}";
            using (var conn = new SqlConnection(_connectString))
            {
                var result = await conn.QueryAsync<TradeRespServiceModel>(sqlQuery);
                return result.First();
            }
        }
    }
}
