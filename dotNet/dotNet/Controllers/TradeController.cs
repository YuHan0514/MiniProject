using AutoMapper;
using dotNet.Interface;
using dotNet.ServiceModels;
using dotNet.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
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

        [HttpPost]
        public async Task<IActionResult> InsertTwseDataToDB([FromBody] DateTime endDate)
        {
            var result = await _service.InsertTwseDataToDB(endDate);
            if (result.code == 200)
                return Ok(result);
            else
                return BadRequest(result);
        }


        [HttpPost]
        public async Task<TradeQueryRespViewModel> GetStockListFromDB([FromBody] TradeQueryViewModel filterCondition)
        {
            var serviceModel = _mapper.Map<TradeQueryServiceModel>(filterCondition);
            serviceModel.pageSize = 10;
            var tradeRespServiceModels = await _service.GetStockListFromDB(serviceModel);
            var tradeQueryRespViewModel = _mapper.Map<TradeQueryRespViewModel>(tradeRespServiceModels);
            return tradeQueryRespViewModel;
        }

        [HttpPost]
        public async Task DeleteStockByStatus([FromBody]int id)
        {
            await _service.DeleteStockByStatus(id);
        }

        [HttpPost]
        public async Task<TradeRespViewModel> GetStockById([FromBody] int id)
        {
            var tradeRespServiceModel =  await _service.GetStockById(id);
            var trade = _mapper.Map<TradeRespViewModel>(tradeRespServiceModel);
            return trade;
        }

        [HttpPost]
        public async Task<IActionResult> UpdateStockById([FromBody] TradeViewModel trade)
        {
            var tradeService = _mapper.Map<TradeServiceModel>(trade);
            var result = await _service.UpdateTradeInfoById(tradeService);
            if (result.code == 200)
                return Ok(result);
            else
                return BadRequest(result);
        }
        [HttpPost]
        public async Task<IActionResult> InsertDataToDB([FromBody] TradeViewModel trade)
        {
            var tradeService = _mapper.Map<TradeServiceModel>(trade);
            var result = await _service.InsertDataToDB(tradeService);
            if (result.code == 200)
                return Ok(result);
            else
                return BadRequest(result);
        }
    }
}
