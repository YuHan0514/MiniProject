using Dapper;
using Microsoft.Data.SqlClient;
using MiniProject.Interface;
using MiniProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniProject.Repository
{
    public class StockRepository : IStockRepository
    {
        private readonly string _connectString = @"Server=(LocalDB)\MSSQLLocalDB;Database=MiniProjectDB;Trusted_Connection=True;";
        public async Task<IEnumerable<JoinTable>> GetList()
        {
            string sqlQuery = @"
                SELECT 
                    TradeTable.TradeDate,
                    StockTable.StockId,
                    StockTable.Name,
                    TradeTable.Type,
                    TradeTable.Volume,
                    TradeTable.Fee,
                    ClosingPriceTable.Price,
                    TradeTable.LendingPeriod
                FROM 
                    TradeTable
                    INNER JOIN StockTable ON TradeTable.StockId = StockTable.StockId
                    INNER JOIN ClosingPriceTable ON TradeTable.StockId = ClosingPriceTable.StockId";

            using (var conn = new SqlConnection(_connectString))
            {
                var result = await conn.QueryAsync<JoinTable>(sqlQuery);
                return result;
            }
        }
    }
}
