using AutoMapper;
using Core.Models;

namespace Infrastructure.Mapping
{
    public class TickerProfile : Profile
    {
        public TickerProfile()
        {
            CreateMap<List<object>, Ticker>()
                .ForMember(dest => dest.Bid, opt => opt.MapFrom(src => Convert.ToDecimal(src[0])))
                .ForMember(dest => dest.BidSize, opt => opt.MapFrom(src => Convert.ToDecimal(src[1])))
                .ForMember(dest => dest.Ask, opt => opt.MapFrom(src => Convert.ToDecimal(src[2])))
                .ForMember(dest => dest.AskSize, opt => opt.MapFrom(src => Convert.ToDecimal(src[3])))
                .ForMember(dest => dest.DailyChange, opt => opt.MapFrom(src => Convert.ToDecimal(src[4])))
                .ForMember(dest => dest.DailyChangeRelative, opt => opt.MapFrom(src => Convert.ToDecimal(src[5])))
                .ForMember(dest => dest.LastPrice, opt => opt.MapFrom(src => Convert.ToDecimal(src[6])))
                .ForMember(dest => dest.Volume, opt => opt.MapFrom(src => Convert.ToDecimal(src[7])))
                .ForMember(dest => dest.High, opt => opt.MapFrom(src => Convert.ToDecimal(src[8])))
                .ForMember(dest => dest.Low, opt => opt.MapFrom(src => Convert.ToDecimal(src[9])))
                .ForMember(dest => dest.Pair, opt => opt.Ignore());
        }
    }
}
