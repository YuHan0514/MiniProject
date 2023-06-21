using Dapper;
using Microsoft.Data.SqlClient;
using MiniProject.Interface;
using MiniProject.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Transactions;

namespace MiniProject.Repository
{
    public class StockRepository : IStockRepository
    {
        private readonly string _connectString = @"Server=(LocalDB)\MSSQLLocalDB;Database=MiniProjectDB;Trusted_Connection=True;";
        public async Task<IEnumerable<JoinTable>> GetList()
        {
            using (var conn = new SqlConnection(_connectString))
            {
                using (var scope = new TransactionScope())
                {
                    string sqlQuery = @"
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
                    TradeTable.Status <> 2";


                    var result = conn.Query<JoinTable>(sqlQuery);
                    scope.Complete();
                    return result;
                }
            }
            
        }
    }
}
