using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TicketBotApi.Data;
using TicketBotApi.Models;

namespace CoreBot.Repositories
{
	public class DatabaseMovieRepository : IMovieRepository
	{
		private readonly BotDbContext _context;

		public DatabaseMovieRepository(BotDbContext context)
		{
			_context = context;
		}

		public async Task<List<Movie>> GetAllAsync()
		{
			return await _context.Movies.ToListAsync();
		}

		public async Task<Movie> GetByIdAsync(int id)
		{
			return await _context.Movies.FindAsync(id);
		}

		public async Task<Movie> AddAsync(Movie movie)
		{
			_context.Movies.Add(movie);
			await _context.SaveChangesAsync();
			return movie;
		}

		public async Task UpdateAsync(Movie movie)
		{
			var existing = await _context.Movies.FindAsync(movie.Id);
			if (existing != null)
			{
				existing.Name = movie.Name;
				existing.Rating = movie.Rating;
				await _context.SaveChangesAsync();
			}
		}

		public async Task DeleteAsync(int id)
		{
			var movie = await _context.Movies.FindAsync(id);
			if (movie != null)
			{
				_context.Movies.Remove(movie);
				await _context.SaveChangesAsync();
			}
		}
	}
}
