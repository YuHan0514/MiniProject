using AutoMapper;
using MiniProject.Service;
using MiniProject.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiniProject.Profiles
{
    public class TradeProfile : Profile
    {
        public TradeProfile()
        {
            CreateMap<TradeRespServiceModel, TradeRespViewModel>();

        }

    }
}
