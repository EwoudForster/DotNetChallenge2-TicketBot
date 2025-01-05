using AutoMapper;
using TicketBot.DAL.Dtos;
using TicketBot.DAL.Models;
namespace TicketBot.DAL.MappingProfiles
{

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Movie, MovieDto>()
                .ForMember(dest => dest.Schedules, opt => opt.MapFrom(src => src.Schedules));

            CreateMap<Schedule, ScheduleDto>()
                .ForMember(dest => dest.MovieHall, opt => opt.MapFrom(src => src.MovieHall))
                .ForMember(dest => dest.Movie, opt => opt.MapFrom(src => src.Movie))
                .ForMember(dest => dest.Tickets, opt => opt.MapFrom(src => src.Tickets));

            CreateMap<MovieHall, MovieHallDto>()
                .ForMember(dest => dest.Schedules, opt => opt.MapFrom(src => src.Schedules));

            CreateMap<Ticket, TicketDto>()
                .ForMember(dest => dest.Schedule, opt => opt.MapFrom(src => src.Schedule));
        }
    }
}
