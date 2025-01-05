using System.Linq.Expressions;
using TicketBot.DAL.Data;
using TicketBot.DAL.Models;

namespace TicketBot.DAL.Repositories
{
    public class TicketRepository : Repository<Ticket>, ITicketRepository
    {
        public TicketRepository(AppDbContext context) : base(context) { }

		public async Task<IEnumerable<Ticket>> GetAllWithIncludesAsync(params Expression<Func<Ticket, object>>[] includes)
		{
			// Add default includes if not passed in the parameters
			var defaultIncludes = new Expression<Func<Ticket, object>>[]
			{
				t => t.Schedule,
				s => s.Schedule.Movie
			};

			// Call the base method with merged includes
			return await base.GetAllAsync(defaultIncludes.Concat(includes).ToArray());
		}
	}
}
