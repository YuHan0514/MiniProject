using AutoMapper;
using dotNet.Models;
using dotNet.Service;
using dotNet.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;

namespace dotNet.Profiles
{
    public class TradeProfile : Profile
    {
        public TradeProfile()
        {
            CreateMap<TradeRespServiceModel, TradeRespViewModel>();
            CreateMap<JoinTable, TradeRespViewModel>();
            CreateMap<JoinTable, TradeRespServiceModel>();
            CreateMap<TradeRespViewModel, TradeRespViewModel>();
            CreateMap<JoinTable, StockTable>();
            CreateMap<List<object>, JoinTable>()
                    .ForMember(dest => dest.TradeDate, opt => opt.MapFrom(src => TransDateTime(src[0].ToString())))
                    .ForMember(dest => dest.StockId, opt => opt.MapFrom(src => GetStockId(src[1].ToString())))
                    .ForMember(dest => dest.Name, opt => opt.MapFrom(src => GetStockName(src[1].ToString())))
                    .ForMember(dest => dest.Type, opt => opt.MapFrom(src => TransType(src[2].ToString())))
                    .ForMember(dest => dest.Volume, opt => opt.MapFrom(src => Convert.ToInt64(src[3].ToString().Replace(",", ""))))
                    .ForMember(dest => dest.Fee, opt => opt.MapFrom(src => float.Parse(src[4].ToString())))
                    .ForMember(dest => dest.Price, opt => opt.MapFrom(src => float.Parse(src[5].ToString())))
                    .ForMember(dest => dest.LendingPeriod, opt => opt.MapFrom(src => src[7].ToString()));

            CreateMap<JoinTable, StockTable>()
                    .ForMember(dest => dest.StockId, opt => opt.MapFrom(src => src.StockId))
                    .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));

            CreateMap<JoinTable, TradeTable>()
                    .ForMember(dest => dest.TradeDate, opt => opt.MapFrom(src => src.TradeDate))
                    .ForMember(dest => dest.StockId, opt => opt.MapFrom(src => src.StockId))
                    .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type))
                    .ForMember(dest => dest.Volume, opt => opt.MapFrom(src => src.Volume))
                    .ForMember(dest => dest.Fee, opt => opt.MapFrom(src => src.Fee))
                    .ForMember(dest => dest.LendingPeriod, opt => opt.MapFrom(src => src.LendingPeriod));

            CreateMap<JoinTable, ClosingPriceTable>()
                    .ForMember(dest => dest.TradeDate, opt => opt.MapFrom(src => src.TradeDate))
                    .ForMember(dest => dest.StockId, opt => opt.MapFrom(src => src.StockId))
                    .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price));

        }
        private static string GetStockId(string stockString)
        {
            var splitList = stockString.Trim().Split(' ');
            return splitList[0];
        }
        private static string GetStockName(string stockString)
        {
            var splitList = stockString.Trim().Split(' ');
            return splitList[^1];
        }
        private static DateTime TransDateTime(string twDate)
        {
            CultureInfo culture = new CultureInfo("zh-TW");
            culture.DateTimeFormat.Calendar = new TaiwanCalendar();
            return DateTime.Parse(twDate, culture);
        }
        private static string TransType(string type)
        {
            switch (type)
            {
                default:
                    type = null;
                    break;
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
            return type;
        }
        
}
}
