using TicketBot.DAL.Data;
using TicketBot.DAL.Models;

namespace TicketBot.DAL.Repositories
{
    public class MovieRepository : Repository<Movie>, IMovieRepository
    {
        public MovieRepository(AppDbContext context) : base(context) { }
    }
}
