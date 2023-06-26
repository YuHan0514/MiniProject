using AutoMapper;
using dotNet.Helper;
using dotNet.Interface;
using dotNet.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Transactions;

namespace dotNet.Service
{
    public class TradeService : ITradeService
    {
        private readonly IHttpClientFactory _clientFactory;
        private MiniProjectDBContext _context;
        private IMapper _mapper;
        private readonly IStockRepository _stockRepository;
        private readonly ITradeHelper _tradeHelper;


        public TradeService(IHttpClientFactory clientFactory, MiniProjectDBContext context, IMapper mapper, IStockRepository stockRepository, ITradeHelper tradeHelper)
        {
            _context = context;
            _clientFactory = clientFactory;
            _mapper = mapper;
            _stockRepository = stockRepository;
            _tradeHelper = tradeHelper;
        }
        public async Task InsertDataToDB(string startDate, string endDate)
        {

            startDate = _context.ClosingPriceTables.OrderByDescending(a => a.TradeDate).FirstOrDefault().TradeDate.AddDays(1).ToString("yyyyMMdd");
            var dataList = await _tradeHelper.GetDataFromURL(startDate, endDate);
            var stockids = dataList.Select(x=>x.StockId);
            var stockList = _context.StockTables.Where(x=> stockids.Contains(x.StockId)).ToList();
            var closingDate = dataList.Select(x => x.TradeDate);
            var closingList = _context.ClosingPriceTables.Where(x => closingDate.Contains(x.TradeDate) && stockids.Contains(x.StockId)).ToList();

            foreach (JoinTable joinTable in dataList)
            {
                if (stockList.Where(x => x.StockId == joinTable.StockId).FirstOrDefault() == null)
                {
                    var map = _mapper.Map<StockTable>(joinTable);
                    map.CreateDate = DateTime.Today;
                    map.CreateUser = "Admin";
                    using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                    {
                        await _context.StockTables.AddAsync(map);
                        scope.Complete();
                    }
                    stockList.Add(map);
                }

                
                if (closingList.Where(x => x.TradeDate == joinTable.TradeDate &&  x.StockId == joinTable.StockId).ToList().Count == 0)
                {
                    var map = _mapper.Map<ClosingPriceTable>(joinTable);
                    map.CreateUser = "Admin";
                    map.CreateDate = DateTime.Today;
                    using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                    {
                        await _context.ClosingPriceTables.AddAsync(map);
                        scope.Complete();
                    }
                    closingList.Add(map);
                }

                var mapTrade = _mapper.Map<TradeTable>(joinTable);
                mapTrade.CreateUser = "Admin";
                mapTrade.CreateDate = DateTime.Today.Date;
                using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    await _context.TradeTables.AddAsync(mapTrade);
                    scope.Complete();
                }
                _context.SaveChanges();
            }
        }
        public async Task<List<TradeRespServiceModel>> GetDataFromDB()
        {
            var data = await _stockRepository.GetList();
            var dataList = data.ToList();
            var map = _mapper.Map<List<TradeRespServiceModel>>(dataList);
            return map;
        }

        public async Task DeleteData(int id)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var data = await _context.TradeTables.FirstOrDefaultAsync(d => d.Id == id);
                    data.Status = 2;
                    _context.SaveChanges();
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    throw;
                }
            }
        }
    }
}
