using AutoMapper;
using dotNet.Interface;
using dotNet.ServiceModels;
using dotNet.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
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
        public async Task<string> InsertTwseDataToDB(string startDate, DateTime endDate)
        {
            var msg = await _service.InsertTwseDataToDB(startDate, endDate);
            return msg;
        }


        [HttpPost]
        public async Task<Tuple<List<TradeRespViewModel>, int>> GetStockListFromDB([FromBody] FrontEndViewModel filterCondition /*string sortColumn, *//*, string endDate, string tradeType, string stockId, string sortDirection*/)
        {
            //int pageIndex = 1;
            //string sortColumn = "Id";
            //string startDate = "2023-01-04"; 
            //string endDate = "2023-01-04";
            //string tradeType = null;
            //string stockId = "";
            //string sortDirection = "";
            var pageSize = 10;
            var tradeRespServiceModels = await _service.GetStockListFromDB(filterCondition.pageIndex, pageSize, filterCondition.sortColumn, filterCondition.startDate, filterCondition.endDate, filterCondition.tradeType, filterCondition.stockId, filterCondition.sortDirection);
            var tradeRespViewModels = _mapper.Map<List<TradeRespViewModel>>(tradeRespServiceModels.Item1);
            return Tuple.Create(tradeRespViewModels, tradeRespServiceModels.Item2);

        }

        [HttpPost]
        public async Task<string> DeleteStockByStatus(int id)
        {
            var msg= await _service.DeleteStockByStatus(id);
            return msg;
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
            var msg = await _service.UpdateStockById(tradeService);
            return msg;
        }
    }
}
