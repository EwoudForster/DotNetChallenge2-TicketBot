using System.Collections.Generic;
using System.Threading.Tasks;
using TicketBotApi.Models;

namespace CoreBot.Repositories
{
	public interface ITicketRepository
	{
		Task<List<Ticket>> GetAllAsync();
		Task<Ticket> GetByIdAsync(int id);
		Task<Ticket> AddAsync(Ticket ticket);
		Task UpdateAsync(Ticket ticket);
		Task DeleteAsync(int id);
	}
}
