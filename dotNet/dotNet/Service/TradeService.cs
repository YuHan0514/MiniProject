using AutoMapper;
using dotNet.Interface;
using dotNet.DBModels;
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

        ///取得TWSE資料並新增近DB，startDate固定為DB最後一筆+1天
        public async Task<string> InsertTwseDataToDB(string startDate, DateTime endDate)
        {
            string returnString;
            startDate = _context.ClosingPriceTables.OrderByDescending(a => a.TradeDate).FirstOrDefault().TradeDate.AddDays(1).ToString("yyyyMMdd");
            try
            {
                var joinTables = await _tradeHelper.GetStockListFromTwse(startDate, endDate);
                var vs = joinTables.Select(x => x.StockId);
                var stockTables = _context.StockTables.Where(x => vs.Contains(x.StockId)).ToList();
                var dateTimes = joinTables.Select(x => x.TradeDate);
                var closingPriceTables = _context.ClosingPriceTables.Where(x => dateTimes.Contains(x.TradeDate) && vs.Contains(x.StockId)).ToList();
                var today = DateTime.Today;
                string createUser = "Admin";

                foreach (JoinTable joinTable in joinTables)
                {
                    if (stockTables.Where(x => x.StockId == joinTable.StockId).FirstOrDefault() == null)
                    {
                        var stockTable = _mapper.Map<StockTable>(joinTable);
                        stockTable.CreateDate = today;
                        stockTable.CreateUser = createUser;
                        using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                        {
                            await _context.StockTables.AddAsync(stockTable);
                            scope.Complete();
                        }
                        stockTables.Add(stockTable);
                    }

                    if (closingPriceTables.Where(x => x.TradeDate == joinTable.TradeDate && x.StockId == joinTable.StockId).ToList().Count == 0)
                    {
                        var closingPriceTable = _mapper.Map<ClosingPriceTable>(joinTable);
                        closingPriceTable.CreateDate = today;
                        closingPriceTable.CreateUser = createUser;
                        using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                        {
                            await _context.ClosingPriceTables.AddAsync(closingPriceTable);
                            scope.Complete();
                        }
                        closingPriceTables.Add(closingPriceTable);
                    }
                    var tradeTable = _mapper.Map<TradeTable>(joinTable);
                    tradeTable.CreateDate = today;
                    tradeTable.CreateUser = createUser;
                    using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                    {
                        await _context.TradeTables.AddAsync(tradeTable);
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

        ///取得前端要求的的DB資料
        public async Task<Tuple<List<TradeRespServiceModel>, int>> GetStockListFromDB(int pageIndex, int pageSize, string sortColumn, DateTime startDate, DateTime endDate, string tradeType, string stockId, string sortDirection)
        {
            //if pageIndex<1, pageIndex=1
            pageIndex = pageIndex < 1 ? 1 : pageIndex;
            var startIndex = (pageIndex - 1) * pageSize;
            switch (sortDirection)
            {
                default:
                    sortDirection = "";
                    break;
                case "↑":
                    sortDirection = "";
                    break;
                case "↓":
                    sortDirection = "DESC";
                    break;
            }
            var joinTables = (await _stockRepository.JoinAndFilterAllTable(startIndex, pageSize, sortColumn, startDate, endDate, tradeType, stockId, sortDirection)).ToList();
            var count = await _stockRepository.GetJoinAndFilterAllTableCount(startDate, endDate, tradeType, stockId);
            var pageCount = (int)Math.Ceiling(count / (double)pageSize);
            var tradeRespServiceModels = _mapper.Map<List<TradeRespServiceModel>>(joinTables);
            return Tuple.Create(tradeRespServiceModels, pageCount);
        }

        ///從DB抓取要刪除的資料，並將status改為2
        public async Task<string> DeleteStockByStatus(int id)
        {
            string returnString;
            try
            {
                var tradeTable = await _context.TradeTables.FirstOrDefaultAsync(d => d.Id == id);
                tradeTable.Status = 2;
                tradeTable.UpdateDate = DateTime.Today;
                tradeTable.UpdateUser = "User";
                _context.SaveChanges();
                returnString = "200";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
            return returnString;
        }

        ///利用ID從DB抓取一筆資料
        public async Task<TradeRespServiceModel> GetStockById(int id)
        {
            var joinTable = await _stockRepository.GetTradeInfoById(id);
            var tradeRespServiceModel = _mapper.Map<TradeRespServiceModel>(joinTable);
            return tradeRespServiceModel;
        }

        ///修改DB中一筆資料，只修改TradeTables
        public async Task<string> UpdateTradeInfoById(TradeServiceModel stock)
        {
            string returnString;
            try
            {
                var tradeTable = await _context.TradeTables.FirstOrDefaultAsync(d => d.Id == stock.Id);
                _mapper.Map(stock, tradeTable);
                tradeTable.Status = 1;
                tradeTable.UpdateDate = DateTime.Today;
                tradeTable.UpdateUser = "User";
                _context.SaveChanges();
                returnString = "200";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
            return returnString;
        }
    }
}
