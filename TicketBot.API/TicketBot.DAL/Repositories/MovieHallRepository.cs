using TicketBot.DAL.Data;
using TicketBot.DAL.Models;

namespace TicketBot.DAL.Repositories
{
    public class MovieHallRepository : Repository<MovieHall>, IMovieHallRepository
    {
        public MovieHallRepository(AppDbContext context) : base(context) { }
    }
}
