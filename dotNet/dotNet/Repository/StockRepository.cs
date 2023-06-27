﻿using Dapper;
using dotNet.Interface;
using dotNet.Models;
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
        

        //join只取status不為2的資料
        public async Task<IEnumerable<JoinTable>> JoinAllTable()
        {
            using (var conn = new SqlConnection(_connectString))
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


                var result =await conn.QueryAsync<JoinTable>(sqlQuery);
                return result;
            }
            
        }

        //join某id的資料
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
