using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MiniProject.Service;
using MiniProject.Service.Interface;
using MiniProject.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniProject.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class TradeController : ControllerBase
    {
        //private static readonly string[] Summaries = new[]
        //{
        //    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        //};

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
        public async Task<TradeRespServiceModel> Get()
        {
            var map =  _service.Get();
            return await map;
        }   
        
    }
}
