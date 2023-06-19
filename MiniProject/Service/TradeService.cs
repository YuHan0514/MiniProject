using MiniProject.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using Newtonsoft.Json;
using System.IO;
using Newtonsoft.Json.Linq;
using static MiniProject.ServiceModels.TradeServiceModel;
using MiniProject.ServiceModels;
using MiniProject.Models;
using System.Globalization;
using AutoMapper;

namespace MiniProject.Service
{
    public class TradeService : ITradeService
    {
        private readonly IHttpClientFactory _clientFactory;
        private MiniProjectDBContext _context;
        private IMapper _mapper;

        public TradeService(IHttpClientFactory clientFactory, MiniProjectDBContext context, IMapper mapper)
        {
            _context = context;
            _clientFactory = clientFactory;
            _mapper = mapper;
        }
        public async Task<TradeRespServiceModel> Get()
        {
            TradeTable res =null;
            HttpClient httpClient = _clientFactory.CreateClient();
            var message = await httpClient.GetStringAsync("https://www.twse.com.tw/rwd/zh/lending/t13sa710?startDate=20230616&endDate=20230619&tradeType=&stockNo=&response=json");
            JsonDocument jsonDocument = JsonDocument.Parse(message);
            string json = jsonDocument.RootElement.GetRawText();
            var value = jsonDocument.RootElement.GetProperty("data");
            string[] list;
            int last;
            if (value.ValueKind == JsonValueKind.Array)
            {
                foreach (JsonElement x in value.EnumerateArray())
                {
                    list = x.ToString().Split('"', ',');
                    var name = list[4].Split(' ');
                    if (_context.TradeTables.OrderByDescending(d => d.Id).FirstOrDefault() != null)
                        last = _context.TradeTables.OrderByDescending(d => d.Id).FirstOrDefault().Id;
                    else
                        last = 0;
                    CultureInfo culture = new CultureInfo("zh-TW");
                    culture.DateTimeFormat.Calendar = new TaiwanCalendar();
                    var day = DateTime.Parse(list[1], culture);
                    StockTable stockTable = new StockTable()
                    {
                        StockId = name[0],
                        Name = name[1],
                        CreateUser = "Admin",
                        CreateDate = DateTime.Today.Date
                    };
                    //_context.StockTables.Add(stockTable);
                    //_context.SaveChanges();
                    TradeTable tradeData = new TradeTable
                    {
                        Id = last + 1,
                        StockId = name[0],
                        TradeDate = day.Date,
                        Type = list[7],
                        Volume = int.Parse(list[10]),
                        Fee = float.Parse(list[13]),
                        LendingPeriod = int.Parse(list[21]),
                        Status = 0,
                        CreateUser = "Admin",
                        CreateDate = DateTime.Today.Date
                    };
                    //_context.TradeTables.Add(tradeData);
                    //_context.SaveChanges();
                    ClosingPriceTable closingPriceTable = new ClosingPriceTable()
                    {
                        Id = last + 1,
                        StockId = name[0],
                        TradeDate = day.Date,
                        Price = float.Parse(list[16]),
                        CreateUser = "Admin",
                        CreateDate = DateTime.Today.Date
                    };
                    _context.StockTables.Add(stockTable);
                    _context.TradeTables.Add(tradeData);
                    _context.ClosingPriceTables.Add(closingPriceTable);
                    _context.SaveChanges();
                    res = tradeData;
                    break;
                }
            }
            var data = _context.TradeTables.ToList();
            var map = _mapper.Map<TradeRespServiceModel>(res);
            return map;
        }
    }
}
