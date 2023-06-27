using AutoMapper;
using dotNet.Interface;
using dotNet.Models;
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
        public async Task<List<JoinTable>> GetDataFromURL(string startDate, string endDate)
        {
            HttpClient httpClient = _clientFactory.CreateClient();
            string apiUrl = $"{_twseUrl}?StartDate={startDate}&EndDate={endDate}&Response=json";
            var message = await httpClient.GetStringAsync(apiUrl);
            var jsonObject = JsonSerializer.Deserialize<TwseTable>(message);
            var jsonArray = JsonSerializer.Deserialize<List<List<object>>>(jsonObject.data.ToString());
            List<JoinTable> DataList = _mapper.Map<List<JoinTable>>(jsonArray);
            return DataList;
            
            
        }
    }
}
