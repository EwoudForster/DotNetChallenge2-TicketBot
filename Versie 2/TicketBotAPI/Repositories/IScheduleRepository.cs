using System.Collections.Generic;
using System.Threading.Tasks;
using TicketBotApi.Models;

namespace CoreBot.Repositories
{
	public interface IScheduleRepository
	{
		Task<List<Schedule>> GetAllAsync();
		Task<Schedule> GetByIdAsync(int id);
		Task<Schedule> AddAsync(Schedule schedule);
		Task UpdateAsync(Schedule schedule);
		Task DeleteAsync(int id);
	}
}
