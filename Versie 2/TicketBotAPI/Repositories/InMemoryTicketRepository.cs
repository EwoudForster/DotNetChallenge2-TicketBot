// Repositories/InMemoryTicketRepository.cs
using System;
using System.Collections.Generic;
using System.Linq;
using TicketBotApi.Models;

namespace CoreBot.Repositories
{
	public class InMemoryTicketRepository : ITicketRepository
	{
		private readonly List<Ticket> _tickets = new();

		public List<Ticket> GetAll() => _tickets;

		public Ticket GetById(int id) => _tickets.FirstOrDefault(t => t.Id == id);

		public Ticket Add(Ticket ticket)
		{
			ticket.Id = _tickets.Any() ? _tickets.Max(t => t.Id) + 1 : 1;
			ticket.OrderDate = DateTime.UtcNow;
			_tickets.Add(ticket);
			return ticket;
		}

		public void Update(Ticket ticket)
		{
			var existing = GetById(ticket.Id);
			if (existing != null)
			{
				existing.CustomerName = ticket.CustomerName;
				existing.ScheduleId = ticket.ScheduleId;
				existing.OrderDate = ticket.OrderDate;
			}
		}

		public void Delete(int id)
		{
			var ticket = GetById(id);
			if (ticket != null)
				_tickets.Remove(ticket);
		}
	}
}
