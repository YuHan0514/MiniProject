using AutoMapper;
using dotNet.Interface;
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
            await _service.InsertDataToDB(startDate, endDate);
            return "OK";
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
            await _service.DeleteData(id);
            return "OK";
        }
    }
}
