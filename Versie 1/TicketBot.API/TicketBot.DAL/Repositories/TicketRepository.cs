using TicketBot.DAL.Data;
using TicketBot.DAL.Models;

namespace TicketBot.DAL.Repositories
{
    public class TicketRepository : Repository<Ticket>, ITicketRepository
    {
        public TicketRepository(AppDbContext context) : base(context) { }
    }
}
