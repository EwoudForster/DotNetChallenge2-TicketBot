using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TicketBotApi.Data;
using TicketBotApi.Models;
using CoreBot.Repositories;

namespace CoreBot.Repositories
{
	public class DatabaseTicketRepository : ITicketRepository
	{
		private readonly BotDbContext _context;

		public DatabaseTicketRepository(BotDbContext context)
		{
			_context = context;
		}

		public async Task<List<Ticket>> GetAllAsync()
		{
			return await _context.Tickets
				.Include(t => t.Schedule)
					.ThenInclude(s => s.Movie)
				.Include(t => t.Schedule)
					.ThenInclude(s => s.MovieHall)
				.ToListAsync();
		}

		public async Task<Ticket> GetByIdAsync(int id)
		{
			return await _context.Tickets
				.Include(t => t.Schedule)
					.ThenInclude(s => s.Movie)
				.Include(t => t.Schedule)
					.ThenInclude(s => s.MovieHall)
				.FirstOrDefaultAsync(t => t.Id == id);
		}

		public async Task<Ticket> AddAsync(Ticket ticket)
		{
			ticket.OrderDate = DateTime.UtcNow;
			_context.Tickets.Add(ticket);
			await _context.SaveChangesAsync();
			return ticket;
		}

		public async Task UpdateAsync(Ticket ticket)
		{
			_context.Tickets.Update(ticket);
			await _context.SaveChangesAsync();
		}

		public async Task DeleteAsync(int id)
		{
			var ticket = await _context.Tickets.FindAsync(id);
			if (ticket != null)
			{
				_context.Tickets.Remove(ticket);
				await _context.SaveChangesAsync();
			}
		}
	}
}
