using TicketBot.DAL.Data;
using TicketBot.DAL.Models;

namespace TicketBot.DAL.Repositories
{
    public class ScheduleRepository : Repository<Schedule>, IScheduleRepository
    {
        public ScheduleRepository(AppDbContext context) : base(context) { }
    }
}
