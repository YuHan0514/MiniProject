using AutoMapper;
using dotNet.Interface;
using dotNet.DBModels;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace dotNet.Helper
{
    public class TradeHelper : ITradeHelper
    {
        private readonly IHttpClientFactory _clientFactory;
        private IMapper _mapper;
        public IConfiguration Configuration { get; }
        private readonly string _twseUrl;
        public TradeHelper(IHttpClientFactory clientFactory, IMapper mapper, IConfiguration configuration)
        {
            _clientFactory = clientFactory;
            _mapper = mapper;
            Configuration = configuration;
            _twseUrl = configuration.GetValue<string>("TwseUrl");
        }
        public async Task<List<JoinTable>> GetStockListFromTwse(string startDate, DateTime endDate)
        {
            var endDateString = endDate.ToString("yyyyMMdd");
            HttpClient httpClient = _clientFactory.CreateClient();
            string apiUrl = $"{_twseUrl}?StartDate={startDate}&EndDate={endDateString}&Response=json";
            var twseData = await httpClient.GetStringAsync(apiUrl);
            var twse = JsonSerializer.Deserialize<TwseTable>(twseData);
            List<JoinTable> joinTables = _mapper.Map<List<JoinTable>>(twse.data);
            return joinTables;
        }
    }
}
