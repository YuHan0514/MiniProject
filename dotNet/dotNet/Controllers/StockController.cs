using AutoMapper;
using dotNet.Interface;
using dotNet.ServiceModels;
using dotNet.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
        // 取得某筆資料的昨收價
        [HttpPost]
        public async Task<string> GetStockInfo([FromBody] StockViewModel searchStockInfo)
        {
            var stock = _mapper.Map<StockServiceModel>(searchStockInfo);
            var stockRespServiceModel= await _service.GetStockInfo(stock);
            var stockRespViewModel= _mapper.Map<StockRespViewModel>(stockRespServiceModel);
            return stockRespViewModel.Price.ToString();
        }

        
    }
}
