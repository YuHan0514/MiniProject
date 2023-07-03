using AutoMapper;
using dotNet.DBModels;
using dotNet.ServiceModels;
using dotNet.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace dotNet.Profiles
{
    public class TradeProfile : Profile
    {
        public TradeProfile()
        {
            CreateMap<TradeRespServiceModel, TradeRespViewModel>()
                     .ForMember(dest => dest.TradeDate, opt => opt.MapFrom(src => src.TradeDate.ToString("yyyy-MM-dd")))
                     .ForMember(dest => dest.ReturnDate, opt => opt.MapFrom(src => src.ReturnDate.ToString("yyyy-MM-dd")));
            CreateMap<ClosingPriceTable, StockRespServiceModel>();
            CreateMap<JoinTable, TradeRespViewModel>();
            CreateMap<JoinTable, TradeRespServiceModel>()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => RestoredType(src.Type)));
            CreateMap<TradeViewModel, TradeServiceModel>();
            CreateMap<TradeQueryViewModel, TradeQueryServiceModel>();
            CreateMap<TradeQueryRespServiceModel, TradeQueryRespViewModel>();
            CreateMap<StockRespServiceModel, StockRespViewModel>();
            CreateMap<StockViewModel, StockServiceModel>();
            CreateMap<JoinTable, StockTable>();
            CreateMap<List<object>, JoinTable>()
                    .ForMember(dest => dest.TradeDate, opt => opt.MapFrom(src => TransDateTime(src[0].ToString())))
                    .ForMember(dest => dest.StockId, opt => opt.MapFrom(src => GetStockId(src[1].ToString())))
                    .ForMember(dest => dest.Name, opt => opt.MapFrom(src => GetStockName(src[1].ToString())))
                    .ForMember(dest => dest.Type, opt => opt.MapFrom(src => TransType(src[2].ToString())))
                    .ForMember(dest => dest.Volume, opt => opt.MapFrom(src => Convert.ToInt64(src[3].ToString().Replace(",", ""))))
                    .ForMember(dest => dest.Fee, opt => opt.MapFrom(src => float.Parse(src[4].ToString())))
                    .ForMember(dest => dest.Price, opt => opt.MapFrom(src => float.Parse(src[5].ToString())))
                    .ForMember(dest => dest.LendingPeriod, opt => opt.MapFrom(src => src[7].ToString()))
                    .ForMember(dest => dest.ReturnDate, opt => opt.MapFrom(src => TransDateTime(src[6].ToString())));

            CreateMap<JoinTable, StockTable>()
                    .ForMember(dest => dest.StockId, opt => opt.MapFrom(src => src.StockId))
                    .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));

            CreateMap<JoinTable, TradeTable>()
                    .ForMember(dest => dest.TradeDate, opt => opt.MapFrom(src => src.TradeDate))
                    .ForMember(dest => dest.StockId, opt => opt.MapFrom(src => src.StockId))
                    .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type))
                    .ForMember(dest => dest.Volume, opt => opt.MapFrom(src => src.Volume))
                    .ForMember(dest => dest.Fee, opt => opt.MapFrom(src => src.Fee))
                    .ForMember(dest => dest.LendingPeriod, opt => opt.MapFrom(src => src.LendingPeriod))
                    .ForMember(dest => dest.ReturnDate, opt => opt.MapFrom(src => src.ReturnDate));

            CreateMap<JoinTable, ClosingPriceTable>()
                    .ForMember(dest => dest.TradeDate, opt => opt.MapFrom(src => src.TradeDate))
                    .ForMember(dest => dest.StockId, opt => opt.MapFrom(src => src.StockId))
                    .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price));

            CreateMap<TradeServiceModel, TradeTable>()
                    .ForMember(dest => dest.TradeDate, opt => opt.MapFrom(src => src.TradeDate))
                    .ForMember(dest => dest.StockId, opt => opt.MapFrom(src => src.StockId))
                    .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type))
                    .ForMember(dest => dest.Volume, opt => opt.MapFrom(src => src.Volume))
                    .ForMember(dest => dest.Fee, opt => opt.MapFrom(src => src.Fee))
                    .ForMember(dest => dest.LendingPeriod, opt => opt.MapFrom(src => src.LendingPeriod))
                    .ForMember(dest => dest.ReturnDate, opt => opt.MapFrom(src => src.ReturnDate));

        }
        private static string GetStockId(string stockString)
        {
            var splitList = stockString.Trim().Split(' ').ToList();
            return splitList.First();
        }
        private static string GetStockName(string stockString)
        {
            var splitList = stockString.Trim().Split(' ').ToList();
            return splitList.Last();
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
        private static string RestoredType(string type)
        {
            switch (type)
            {
                default:
                    type = null;
                    break;
                case "F":
                    type = "定價";
                    break;
                case "C":
                    type = "競價";
                    break;
                case "N":
                    type = "議借";
                    break;
            }
            return type;
        }
    }
}
