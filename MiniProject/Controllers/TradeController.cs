﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MiniProject.Interface;
using MiniProject.Service;
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
        public async Task<string> UpdateDB(string startDate, string endDate)
        {
            await _service.UpdateDB(startDate, endDate);
            return "OK";
        }


        [HttpGet]
        public async Task<List<TradeRespViewModel>> GetList()
        {
            var data = await _service.GetList();
            var dataList = data.ToList();
            var map = _mapper.Map<List<TradeRespViewModel>>(dataList);
            return map;

        }
    }
}
