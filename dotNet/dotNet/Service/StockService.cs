using AutoMapper;
using dotNet.DBModels;
using dotNet.Interface;
using dotNet.ServiceModels;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace dotNet.Service
{
    public class StockService : IStockService
    {
        private readonly IHttpClientFactory _clientFactory;
        private MiniProjectDBContext _context;
        private IMapper _mapper;
        private readonly IStockRepository _stockRepository;
        private readonly ITradeHelper _tradeHelper;


        public StockService(IHttpClientFactory clientFactory, MiniProjectDBContext context, IMapper mapper, IStockRepository stockRepository, ITradeHelper tradeHelper)
        {
            _context = context;
            _clientFactory = clientFactory;
            _mapper = mapper;
            _stockRepository = stockRepository;
            _tradeHelper = tradeHelper;
        }
        public async Task<StockRespServiceModel> GetStockInfo(StockServiceModel stock)
        {
            var closingPriceTable = await _context.ClosingPriceTables.OrderByDescending(x => x.TradeDate).FirstAsync(x => x.StockId == stock.stockId);
            var stockResp = _mapper.Map<StockRespServiceModel>(closingPriceTable);
            return stockResp;
        }
    }
}
