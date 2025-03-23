using AutoMapper;
using Core.Models;

namespace BL.Mapping
{
    public class TradeProfile : Profile
    {
        public TradeProfile()
        {
            CreateMap<List<object>, Trade>()
                .ForMember(dest => dest.Time, opt => opt.MapFrom(src => DateTimeOffset.FromUnixTimeMilliseconds(Convert.ToInt64(src[1]))))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Convert.ToInt64(src[0])))
                .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => Convert.ToDecimal(src[2])))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => Convert.ToDecimal(src[3])))
                .ForMember(dest => dest.Pair, opt => opt.Ignore());
        }
    }
}
