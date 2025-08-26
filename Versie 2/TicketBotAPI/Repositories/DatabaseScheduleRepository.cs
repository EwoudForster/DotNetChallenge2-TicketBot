using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TicketBotApi.Data;
using TicketBotApi.Models;
using CoreBot.Repositories;

namespace CoreBot.Repositories
{
	public class DatabaseScheduleRepository : IScheduleRepository
	{
		private readonly BotDbContext _context;

		public DatabaseScheduleRepository(BotDbContext context)
		{
			_context = context;
		}

		public async Task<List<Schedule>> GetAllAsync()
		{
			return await _context.Schedules
				.Include(s => s.Movie)
				.Include(s => s.MovieHall)
				.ToListAsync();
		}

		public async Task<Schedule> GetByIdAsync(int id)
		{
			return await _context.Schedules
				.Include(s => s.Movie)
				.Include(s => s.MovieHall)
				.FirstOrDefaultAsync(s => s.Id == id);
		}

		public async Task<Schedule> AddAsync(Schedule schedule)
		{
			_context.Schedules.Add(schedule);
			await _context.SaveChangesAsync();
			return schedule;
		}

		public async Task UpdateAsync(Schedule schedule)
		{
			var existing = await _context.Schedules.FindAsync(schedule.Id);
			if (existing != null)
			{
				existing.MovieId = schedule.MovieId;
				existing.MovieHallId = schedule.MovieHallId;
				existing.Date = schedule.Date;
				await _context.SaveChangesAsync();
			}
		}

		public async Task DeleteAsync(int id)
		{
			var schedule = await _context.Schedules.FindAsync(id);
			if (schedule != null)
			{
				_context.Schedules.Remove(schedule);
				await _context.SaveChangesAsync();
			}
		}
	}
}
