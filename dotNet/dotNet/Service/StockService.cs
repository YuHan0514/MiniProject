using AutoMapper;
using dotNet.DBModels;
using dotNet.Interface;
using dotNet.ServiceModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Net.Http;
using System.Threading.Tasks;

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
            DateTime time = DateTime.Parse(stock.searchDate);
            var closingPriceTables = await _context.ClosingPriceTables.FirstOrDefaultAsync(x => x.TradeDate == time && x.StockId == stock.stockId);
            return _mapper.Map<StockRespServiceModel>(closingPriceTables);
        }
    }
}
