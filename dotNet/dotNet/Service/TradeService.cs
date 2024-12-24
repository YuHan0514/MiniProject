using AutoMapper;
using dotNet.DBModels;
using dotNet.Interface;
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

        ///取得TWSE資料並新增進DB，startDate固定為DB最後一筆+1天，目前外部API已掛掉
        public async Task<TradeTwseRespServiceModel> InsertTwseDataToDB(DateTime endDate)
        {
            var startDate = "20241212";
            //var startDate = _context.ClosingPriceTables.OrderByDescending(a => a.TradeDate).FirstOrDefault().TradeDate.AddDays(1).ToString("yyyyMMdd");
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
                var tradeTwseRespServiceModel = new TradeTwseRespServiceModel()
                {
                    code = 200,
                    message = "success"
                };
                return tradeTwseRespServiceModel;
            }
            catch (System.Exception ex)
            {
                var tradeTwseRespServiceModel = new TradeTwseRespServiceModel()
                {
                    code = 500,
                    message = ex.Message
                };
                return tradeTwseRespServiceModel;
            }
            
        }

        ///取得前端要求的的DB資料
        public async Task<TradeQueryRespServiceModel> GetStockListFromDB(TradeQueryServiceModel tradeQueryServiceModel)
        {
            //if pageIndex<1, pageIndex=1
            tradeQueryServiceModel.pageIndex = tradeQueryServiceModel.pageIndex < 1 ? 1 : tradeQueryServiceModel.pageIndex;
            var startIndex = (tradeQueryServiceModel.pageIndex - 1) * tradeQueryServiceModel.pageSize;
            tradeQueryServiceModel.pageIndex = startIndex;
            switch (tradeQueryServiceModel.sortDirection)
            {
                default:
                    tradeQueryServiceModel.sortDirection = "";
                    break;
                case "↑":
                    tradeQueryServiceModel.sortDirection = "";
                    break;
                case "↓":
                    tradeQueryServiceModel.sortDirection = "DESC";
                    break;
            }
            var joinTables = (await _stockRepository.JoinAndFilterAllTable(tradeQueryServiceModel)).ToList();
            var tradeRespServiceModels = _mapper.Map<List<TradeRespServiceModel>>(joinTables);
            var totalCount = await _stockRepository.GetJoinAndFilterAllTableCount(tradeQueryServiceModel);
            var totalPage = (int)Math.Ceiling(totalCount / (double)tradeQueryServiceModel.pageSize);
            var tradeQueryRespServiceModel = new TradeQueryRespServiceModel()
            {
                Items = tradeRespServiceModels,
                TotalCount = totalCount,
                TotalPage = totalPage
            };
            return tradeQueryRespServiceModel;
        }

        ///從DB抓取要刪除的資料，並將status改為2
        public async Task DeleteStockByStatus(int id)
        {
            var tradeTable = await _context.TradeTables.FirstOrDefaultAsync(d => d.Id == id);
            tradeTable.Status = 2;
            tradeTable.UpdateDate = DateTime.Today;
            tradeTable.UpdateUser = "User";
            _context.SaveChanges();

        }

        ///利用ID從DB抓取一筆資料
        public async Task<TradeRespServiceModel> GetStockById(int id)
        {
            var tradeRespServiceModels = await _stockRepository.GetTradeInfoById(id);
            return tradeRespServiceModels;
        }

        ///修改DB中一筆資料，只修改TradeTables
        public async Task<TradeTwseRespServiceModel> UpdateTradeInfoById(TradeServiceModel stock)
        {
            try
            {
                var tradeTable = await _context.TradeTables.FirstOrDefaultAsync(d => d.Id == stock.Id);
                _mapper.Map(stock, tradeTable);
                tradeTable.Status = 1;
                tradeTable.UpdateDate = DateTime.Today;
                tradeTable.UpdateUser = "User";
                _context.SaveChanges();
                var tradeTwseRespServiceModel = new TradeTwseRespServiceModel()
                {
                    code = 200,
                    message = "success"
                };
                return tradeTwseRespServiceModel;
            }
            catch (Exception ex)
            {
                var tradeTwseRespServiceModel = new TradeTwseRespServiceModel()
                {
                    code = 400,
                    message = ex.Message
                };
                throw;
            }
            
        }
        
        //自行新增一筆資料
        public async Task<TradeTwseRespServiceModel> InsertDataToDB(TradeServiceModel stock)
        {
            try
            {
                string createUser = "Admin";
                var today = DateTime.Today;
                var stockTable = _mapper.Map<StockTable>(stock);
                stockTable.CreateDate = today;
                stockTable.CreateUser = createUser;
                using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    await _context.StockTables.AddAsync(stockTable);
                    scope.Complete();
                }
                _context.SaveChanges();
                var closingPriceTable = _mapper.Map<ClosingPriceTable>(stock);
                closingPriceTable.CreateDate = today;
                closingPriceTable.CreateUser = createUser;
                using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    await _context.ClosingPriceTables.AddAsync(closingPriceTable);
                    scope.Complete();
                }
                _context.SaveChanges();
                var tradeTable = _mapper.Map<TradeTable>(stock);
                tradeTable.CreateDate = today;
                tradeTable.CreateUser = createUser;
                using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    await _context.TradeTables.AddAsync(tradeTable);
                    scope.Complete();
                }
                _context.SaveChanges();
                var tradeTwseRespServiceModel = new TradeTwseRespServiceModel()
                {
                    code = 200,
                    message = "success"
                };
                return tradeTwseRespServiceModel;
            }
            catch (System.Exception ex)
            {
                var tradeTwseRespServiceModel = new TradeTwseRespServiceModel()
                {
                    code = 500,
                    message = ex.Message
                };
                return tradeTwseRespServiceModel;
            }

        }
    }
}
