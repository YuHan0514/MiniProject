using AutoMapper;
using dotNet.Interface;
using dotNet.Models;
using dotNet.ServiceModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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

        //取得TWSE資料並新增近DB，startDate固定為DB最後一筆+1天
        public async Task<string> InsertDataToDB(string startDate, string endDate)
        {
            string returnString;
            startDate = _context.ClosingPriceTables.OrderByDescending(a => a.TradeDate).FirstOrDefault().TradeDate.AddDays(1).ToString("yyyyMMdd");
            try
            {
                var dataList = await _tradeHelper.GetDataFromURL(startDate, endDate);
                var stockids = dataList.Select(x => x.StockId);
                var stockList = _context.StockTables.Where(x => stockids.Contains(x.StockId)).ToList();
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


                    if (closingList.Where(x => x.TradeDate == joinTable.TradeDate && x.StockId == joinTable.StockId).ToList().Count == 0)
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
                    mapTrade.CreateDate = DateTime.Today;
                    using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                    {
                        await _context.TradeTables.AddAsync(mapTrade);
                        scope.Complete();
                    }
                    _context.SaveChanges();
                }
                returnString = "200 資料庫新增完成";
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
                returnString = "500 TWSE資料格式錯誤";
            }
            return returnString;
        }

        //取得DB所有資料
        public async Task<List<TradeRespServiceModel>> GetDataFromDB()
        {
            var data = await _stockRepository.JoinAllTable();
            var dataList = data.ToList();
            var map = _mapper.Map<List<TradeRespServiceModel>>(dataList);
            return map;
        }

        //從DB抓取資料，並將status改為2
        public async Task<string> DeleteData(int id)
        {
            string returnString;
            try
            {
                var data = await _context.TradeTables.FirstOrDefaultAsync(d => d.Id == id);
                data.Status = 2;
                data.UpdateDate = DateTime.Today;
                data.UpdateUser = "User";
                _context.SaveChanges();
                returnString = "200 資料刪除完成";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
            return returnString;
        }

        //利用ID從DB抓取一筆資料
        public async Task<TradeRespServiceModel> GetById(int id)
        {
            var data = await _stockRepository.JoinTableById(id);
            var map = _mapper.Map<TradeRespServiceModel>(data);
            return map;
        }

        //修改DB中一筆資料，只修改TradeTables
        public async Task<string> UpdateById(TradeServiceModel stock)
        {
            string returnString;
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var data = await _context.TradeTables.FirstOrDefaultAsync(d => d.Id == stock.Id);
                    _mapper.Map(stock, data);
                    data.Status = 1;
                    data.UpdateDate = DateTime.Today;
                    data.UpdateUser = "User";
                    _context.SaveChanges();
                    scope.Complete();
                    returnString = "200 資料更新完成";
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    throw;
                }
            }
            return returnString;
        }
    }
}
