using AutoMapper;
using dotNet.Interface;
using dotNet.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace dotNet.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly ILogger<StockController> _logger;
        private IStockService _service;
        private IMapper _mapper;
        public StockController(ILogger<StockController> logger, IStockService service, IMapper mapper)
        {
            _logger = logger;
            _service = service;
            _mapper = mapper;
        }
        // 取得某筆資料的昨日收盤價
        [HttpGet]
        public async Task<StockRespViewModel> GetStockInfo(string stockId, string tradeDate)
        {
            var stockRespServiceModel= await _service.GetStockInfo(stockId, tradeDate);
            return _mapper.Map<StockRespViewModel>(stockRespServiceModel);
        }

        
    }
}
