// Repositories/ITicketRepository.cs
using System.Collections.Generic;
using TicketBotApi.Models;

namespace CoreBot.Repositories
{
	public interface ITicketRepository
	{
		List<Ticket> GetAll();
		Ticket GetById(int id);
		Ticket Add(Ticket ticket);
		void Update(Ticket ticket);
		void Delete(int id);
	}
}
