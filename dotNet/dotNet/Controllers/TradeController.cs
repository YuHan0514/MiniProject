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
        public async Task<string> InsertDataToDB(string startDate, string endDate)
        {
            return await _service.InsertDataToDB(startDate, endDate);
        }


        [HttpGet]
        public async Task<List<TradeRespViewModel>> GetDataFromDB()
        {
            var data = await _service.GetDataFromDB();
            var map = _mapper.Map<List<TradeRespViewModel>>(data);
            return map;

        }

        [HttpPost]
        public async Task<string> DeleteData(int id)
        {
            return await _service.DeleteData(id);
        }

        [HttpPost]
        public async Task<TradeRespViewModel> GetById(int id)
        {
            var data =  await _service.GetById(id);
            var map = _mapper.Map<TradeRespViewModel>(data);
            return map;
        }

        [HttpPost]
        public async Task<string> UpdateById(TradeViewModel stock)
        {
            var map = _mapper.Map<TradeServiceModel>(stock);
            return await _service.UpdateById(map);
        }
    }
}
