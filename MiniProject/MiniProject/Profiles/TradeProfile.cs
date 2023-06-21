using AutoMapper;
using MiniProject.Models;
using MiniProject.Service;
using MiniProject.ViewModels;

namespace MiniProject.Profiles
{
    public class TradeProfile : Profile
    {
        public TradeProfile()
        {
            CreateMap<TradeRespServiceModel, TradeRespViewModel>();
            CreateMap<JoinTable, TradeRespViewModel>();
            CreateMap<JoinTable, TradeRespServiceModel>();
            CreateMap<TradeRespViewModel, TradeRespViewModel>();

            

        }

    }
}
