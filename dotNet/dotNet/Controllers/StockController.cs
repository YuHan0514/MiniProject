using AutoMapper;
using dotNet.Interface;
using dotNet.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

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
        // GET api/<StockController>/5
        [HttpGet]
        public async Task<StockRespViewModel> GetStockInfo(string id, DateTime tradeDate)
        {
            var stockRespServiceModel= await _service.GetStockInfo(id, tradeDate);
            return _mapper.Map<StockRespViewModel>(stockRespServiceModel);
        }

        
    }
}
