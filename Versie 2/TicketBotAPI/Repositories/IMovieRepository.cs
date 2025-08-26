using System.Collections.Generic;
using System.Threading.Tasks;
using TicketBotApi.Models;

namespace CoreBot.Repositories
{
	public interface IMovieRepository
	{
		Task<List<Movie>> GetAllAsync();
		Task<Movie> GetByIdAsync(int id);
		Task<Movie> AddAsync(Movie movie);
		Task UpdateAsync(Movie movie);
		Task DeleteAsync(int id);
	}
}
