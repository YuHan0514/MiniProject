using AutoMapper;
using dotNet.Interface;
using dotNet.ServiceModels;
using dotNet.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace dotNet.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class TradeController : ControllerBase
    {

        private readonly ILogger<TradeController> _logger;
        private ITradeService _service;
        private IMapper _mapper;
        public TradeController(ILogger<TradeController> logger, ITradeService service, IMapper mapper)
        {
            _logger = logger;
            _service = service;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<string> InsertTwseDataToDB(string startDate, string endDate)
        {
            return await _service.InsertTwseDataToDB(startDate, endDate);
        }


        [HttpGet]
        public async Task<List<TradeRespViewModel>> GetStockListFromDB(int pageIndex, string sortColumn, string startDate, string endDate, string tradeType, string stockId, string sortDirection)
        {
            var pageSize = 10;
            var tradeRespServiceModels = await _service.GetStockListFromDB(pageIndex, pageSize, sortColumn, startDate, endDate, tradeType, stockId, sortDirection);
            var tradeRespViewModels = _mapper.Map<List<TradeRespViewModel>>(tradeRespServiceModels);
            return tradeRespViewModels;

        }

        [HttpPost]
        public async Task<string> DeleteStockByStatus(int id)
        {
            return await _service.DeleteStockByStatus(id);
        }

        [HttpPost]
        public async Task<TradeRespViewModel> GetStockById(int id)
        {
            var tradeRespServiceModel =  await _service.GetStockById(id);
            var trade = _mapper.Map<TradeRespViewModel>(tradeRespServiceModel);
            return trade;
        }

        [HttpPost]
        public async Task<string> UpdateStockById(TradeViewModel trade)
        {
            var tradeService = _mapper.Map<TradeServiceModel>(trade);
            return await _service.UpdateStockById(tradeService);
        }
    }
}
