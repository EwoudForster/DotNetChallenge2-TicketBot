using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TicketBotApi.Data;
using TicketBotApi.Models;

namespace CoreBot.Repositories
{
	public class DatabaseMovieHallRepository : IMovieHallRepository
	{
		private readonly BotDbContext _context;

		public DatabaseMovieHallRepository(BotDbContext context)
		{
			_context = context;
		}

		public async Task<List<MovieHall>> GetAllAsync()
		{
			return await _context.MovieHalls.ToListAsync();
		}

		public async Task<MovieHall> GetByIdAsync(int id)
		{
			return await _context.MovieHalls.FindAsync(id);
		}

		public async Task<MovieHall> AddAsync(MovieHall hall)
		{
			_context.MovieHalls.Add(hall);
			await _context.SaveChangesAsync();
			return hall;
		}

		public async Task UpdateAsync(MovieHall hall)
		{
			var existing = await _context.MovieHalls.FindAsync(hall.Id);
			if (existing != null)
			{
				existing.Name = hall.Name;
				await _context.SaveChangesAsync();
			}
		}

		public async Task DeleteAsync(int id)
		{
			var hall = await _context.MovieHalls.FindAsync(id);
			if (hall != null)
			{
				_context.MovieHalls.Remove(hall);
				await _context.SaveChangesAsync();
			}
		}
	}
}
