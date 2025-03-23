using AutoMapper;
using Core.Models;

namespace BL.Mapping
{
    public class CandleProfile : Profile
    {
        public CandleProfile()
        {
            CreateMap<List<object>, Candle>()
                .ForMember(dest => dest.OpenTime, opt => opt.MapFrom(src => DateTimeOffset.FromUnixTimeMilliseconds(Convert.ToInt64(src[0]))))
                .ForMember(dest => dest.OpenPrice, opt => opt.MapFrom(src => Convert.ToDecimal(src[1])))
                .ForMember(dest => dest.ClosePrice, opt => opt.MapFrom(src => Convert.ToDecimal(src[2])))
                .ForMember(dest => dest.HighPrice, opt => opt.MapFrom(src => Convert.ToDecimal(src[3])))
                .ForMember(dest => dest.LowPrice, opt => opt.MapFrom(src => Convert.ToDecimal(src[4])))
                .ForMember(dest => dest.TotalVolume, opt => opt.MapFrom(src => Convert.ToDecimal(src[5])))
                .ForMember(dest => dest.Pair, opt => opt.Ignore())
                .ForMember(dest => dest.TotalPrice, opt => opt.Ignore());
        }
    }
}
