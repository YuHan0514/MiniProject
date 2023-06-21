using AutoMapper;
using MiniProject.Interface;
using MiniProject.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MiniProject.Service
{
    public class TradeService : ITradeService
    {
        private readonly IHttpClientFactory _clientFactory;
        private MiniProjectDBContext _context;
        private IMapper _mapper;
        private readonly IStockRepository _stockRepository;


        public TradeService(IHttpClientFactory clientFactory, MiniProjectDBContext context, IMapper mapper, IStockRepository stockRepository)
        {
            _context = context;
            _clientFactory = clientFactory;
            _mapper = mapper;
            _stockRepository = stockRepository;
        }
        public async Task UpdateDB(string startDate, string endDate)
        {
            HttpClient httpClient = _clientFactory.CreateClient();
            string MainURL = "https://www.twse.com.tw/rwd/zh/lending/t13sa710";
            string apiUrl = $"{MainURL}?startDate={startDate}&endDate={endDate}&tradeType=&stockNo=&response=json";
            var message = await httpClient.GetStringAsync(apiUrl);
            JsonDocument jsonDocument = JsonDocument.Parse(message);
            var value = jsonDocument.RootElement.GetProperty("data");
            if (value.ValueKind == JsonValueKind.Array)
            {
                foreach (JsonElement x in value.EnumerateArray())
                {
                    var stockString = x[1].ToString().Trim();
                    var name = Regex.Replace(stockString, @"\s+", " ").Split(' ');
                    var volumeString = x[3].ToString();
                    if (volumeString.Contains(','))
                    {
                        volumeString = volumeString.Replace(",", "");
                    }
                    CultureInfo culture = new CultureInfo("zh-TW");
                    culture.DateTimeFormat.Calendar = new TaiwanCalendar();
                    var day = DateTime.Parse(x[0].ToString(), culture);
                    if (_context.StockTables.FirstOrDefault(x => x.StockId == name[0]) == null)
                    {
                        StockTable stockTable = new StockTable()
                        {
                            StockId = name[0],
                            Name = name[1],
                            CreateUser = "Admin",
                            CreateDate = DateTime.Today.Date
                        };
                        await _context.StockTables.AddAsync(stockTable);
                    }
                    var priceList = _context.ClosingPriceTables.Where(x => x.StockId == name[0]).ToList();
                    if (priceList.Count == 0 || priceList.Where(x => x.TradeDate == day.Date).ToList().Count == 0)
                    {
                        ClosingPriceTable closingPriceTable = new ClosingPriceTable()
                        {
                            StockId = name[0],
                            TradeDate = day.Date,
                            Price = float.Parse(x[5].ToString()),
                            CreateUser = "Admin",
                            CreateDate = DateTime.Today.Date
                        };
                        await _context.ClosingPriceTables.AddAsync(closingPriceTable);

                    }
                    string type = "";
                    switch (x[2].ToString())
                    {
                        case "定價":
                            type = "F";
                            break;
                        case "競價":
                            type = "C";
                            break;
                        case "議借":
                            type = "N";
                            break;
                    }
                    TradeTable tradeData = new TradeTable
                    {
                        StockId = name[0],
                        TradeDate = day.Date,
                        Type = type,
                        Volume = int.Parse(volumeString),
                        Fee = float.Parse(x[4].ToString()),
                        LendingPeriod = int.Parse(x[7].ToString()),
                        Status = 0,
                        CreateUser = "Admin",
                        CreateDate = DateTime.Today.Date
                    };
                    await _context.TradeTables.AddAsync(tradeData);
                    _context.SaveChanges();
                    //break;
                }
            }
        }
        // i am branch file
        public async Task<List<TradeRespServiceModel>> GetList()
        {
            var data = await _stockRepository.GetList();
            var dataList = data.ToList();
            var map = _mapper.Map<List<TradeRespServiceModel>>(dataList);
            return map;
        }
    }
}
