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

using System.Globalization;
using AutoMapper;
using System.Text.RegularExpressions;
using MiniProject.Interface;
using MiniProject.Models;

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
                    //var tradeLast = _context.TradeTables.OrderByDescending(d => d.Id).FirstOrDefault();
                    //var closeLast = _context.ClosingPriceTables.OrderByDescending(d => d.Id).FirstOrDefault();
                    var stockString = x[1].ToString().Trim();
                    //int tradeId;
                    //int closeId;
                    var name = Regex.Replace(stockString, @"\s+", " ").Split(' ');
                    //第一次執行要跑以下
                    //if (tradeLast != null)
                    //    tradeId = tradeLast.Id;
                    //else
                    //    tradeId = 0;
                    //if (closeLast != null)
                    //    closeId = closeLast.Id;
                    //else
                    //    closeId = 0;
                    ///////////
                    //資料庫內有值時跑以下
                    //tradeLast = _context.TradeTables.OrderByDescending(d => d.Id).FirstOrDefault();
                    //closeLast = _context.ClosingPriceTables.OrderByDescending(d => d.Id).FirstOrDefault();
                    //
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
                        _context.StockTables.Add(stockTable);
                        //_context.SaveChanges();
                    }
                    var priceList = _context.ClosingPriceTables.Where(x => x.StockId == name[0]).ToList();
                    if (priceList.Count == 0 || priceList.Where(x => x.TradeDate == day.Date).ToList().Count == 0)
                    {
                        ClosingPriceTable closingPriceTable = new ClosingPriceTable()
                        {
                            //Id = closeId + 1,
                            StockId = name[0],
                            TradeDate = day.Date,
                            Price = float.Parse(x[5].ToString()),
                            CreateUser = "Admin",
                            CreateDate = DateTime.Today.Date
                        };
                        _context.ClosingPriceTables.Add(closingPriceTable);
                        //_context.SaveChanges();

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
                        //Id = tradeId + 1,
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
                    _context.TradeTables.Add(tradeData);
                    _context.SaveChanges();
                    //break;
                }
            }
        }

        public async Task<List<TradeRespServiceModel>> GetList()
        {
            var data = await _stockRepository.GetList();
            var dataList = data.ToList();
            var map = _mapper.Map<List<TradeRespServiceModel>>(dataList);
            return map;

        }
    }
}
