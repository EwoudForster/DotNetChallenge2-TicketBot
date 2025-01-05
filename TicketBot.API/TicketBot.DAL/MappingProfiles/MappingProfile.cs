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
    .ForMember(dest => dest.Schedules, opt => opt.MapFrom(src =>
        src.Schedules != null
            ? src.Schedules.Select(schedule => new ScheduleDto
            {
                Id = schedule.Id,
                MovieHallId = schedule.MovieHallId,
                MovieHall = schedule.MovieHall != null ? new MovieHallDto
                {
                    Id = schedule.MovieHall.Id,
                    Name = schedule.MovieHall.Name,
                    Location = schedule.MovieHall.Location
                } : new MovieHallDto()
            }).ToList()
            : new List<ScheduleDto>()))
;
            ;

            // Schedule to ScheduleDto mapping (unchanged)
            CreateMap<Schedule, ScheduleDto>()
                .ForMember(dest => dest.Movie, opt => opt.MapFrom(src => new MovieDto
                {
                    Id = src.Movie.Id,
                    Name = src.Movie.Name,
                    Description = src.Movie.Description,
                    Rating = src.Movie.Rating
                }))
                .ForMember(dest => dest.MovieHall, opt => opt.MapFrom(src => new MovieHallDto
                {
                    Id = src.MovieHall.Id,
                    Name = src.MovieHall.Name,
                    Location = src.MovieHall.Location
                }))
                .ForMember(dest => dest.Tickets, opt => opt.MapFrom(src => src.Tickets.Select(ticket => new TicketDto
                {
                    Id = ticket.Id,
                    CustomerName = ticket.CustomerName,
                    OrderDate = ticket.OrderDate,
                    ScheduleId = ticket.ScheduleId
                }).ToList()));

            CreateMap<MovieHall, MovieHallDto>()
                .ForMember(dest => dest.Schedules, opt => opt.MapFrom(src => src.Schedules.Select(schedule => new ScheduleDto
                {
                    Id = schedule.Id,
                    MovieHallId = schedule.MovieHallId,
                    MovieId = schedule.MovieId,
                    Movie = new MovieDto
                    {
                        Id = schedule.Movie.Id,
                        Name = schedule.Movie.Name,
                        Description = schedule.Movie.Description,
                        Rating = schedule.Movie.Rating
                    },
                    Tickets = schedule.Tickets.Select(ticket => new TicketDto
                    {
                        Id = ticket.Id,
                        CustomerName = ticket.CustomerName,
                        OrderDate = ticket.OrderDate,
                        ScheduleId = ticket.ScheduleId
                    }).ToList()
                }).ToList()));

            CreateMap<Ticket, TicketDto>()
     .ForMember(dest => dest.Schedule, opt => opt.MapFrom(src => new ScheduleDto
     {
         Id = src.Schedule.Id,
         MovieHallId = src.Schedule.MovieHallId,
         MovieId = src.Schedule.MovieId,
         MovieHall = src.Schedule.MovieHall != null ? new MovieHallDto
         {
             Id = src.Schedule.MovieHall.Id,
             Name = src.Schedule.MovieHall.Name,
             Location = src.Schedule.MovieHall.Location
         } : new MovieHallDto(),
         Movie = src.Schedule.Movie != null ? new MovieDto
         {
             Id = src.Schedule.Movie.Id,
             Name = src.Schedule.Movie.Name,
             Description = src.Schedule.Movie.Description,
             Rating = src.Schedule.Movie.Rating
         } : new MovieDto()
     }));


            CreateMap<MovieDto, Movie>()
                .ForMember(dest => dest.Schedules, opt => opt.MapFrom(src => src.Schedules.Select(schedule => new Schedule
                {
                    Id = schedule.Id,
                    MovieHallId = schedule.MovieHallId,
                    MovieId = schedule.MovieId
                }).ToList()));

            CreateMap<ScheduleDto, Schedule>()
                .ForMember(dest => dest.Movie, opt => opt.MapFrom(src => new Movie
                {
                    Id = src.Movie.Id,
                    Name = src.Movie.Name,
                    Description = src.Movie.Description,
                    Rating = src.Movie.Rating
                }))
                .ForMember(dest => dest.MovieHall, opt => opt.MapFrom(src => new MovieHall
                {
                    Id = src.MovieHall.Id,
                    Name = src.MovieHall.Name,
                    Location = src.MovieHall.Location
                }))
                .ForMember(dest => dest.Tickets, opt => opt.MapFrom(src => src.Tickets.Select(ticket => new Ticket
                {
                    Id = ticket.Id,
                    CustomerName = ticket.CustomerName,
                    OrderDate = ticket.OrderDate,
                    ScheduleId = ticket.ScheduleId
                }).ToList()));

            CreateMap<MovieHallDto, MovieHall>()
                .ForMember(dest => dest.Schedules, opt => opt.MapFrom(src => src.Schedules.Select(schedule => new Schedule
                {
                    Id = schedule.Id,
                    MovieHallId = schedule.MovieHallId,
                    MovieId = schedule.MovieId
                }).ToList()));

            CreateMap<TicketDto, Ticket>()
                .ForMember(dest => dest.Schedule, opt => opt.MapFrom(src => new Schedule
                {
                    Id = src.Schedule.Id,
                    MovieId = src.Schedule.MovieId,
                    MovieHallId = src.Schedule.MovieHallId
                }));
        }

    }
}
