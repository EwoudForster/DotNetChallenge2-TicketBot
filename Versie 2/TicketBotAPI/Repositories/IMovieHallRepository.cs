using System.Collections.Generic;
using System.Threading.Tasks;
using TicketBotApi.Models;

namespace CoreBot.Repositories
{
	public interface IMovieHallRepository
	{
		Task<List<MovieHall>> GetAllAsync();
		Task<MovieHall> GetByIdAsync(int id);
		Task<MovieHall> AddAsync(MovieHall hall);
		Task UpdateAsync(MovieHall hall);
		Task DeleteAsync(int id);
	}
}
