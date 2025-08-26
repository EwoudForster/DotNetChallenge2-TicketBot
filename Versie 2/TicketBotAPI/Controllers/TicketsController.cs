using Microsoft.AspNetCore.Mvc;
using CoreBot.Repositories;
using System.Collections.Generic;
using TicketBotApi.Models;

namespace CoreBot.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class TicketsController : ControllerBase
	{
		private readonly ITicketRepository _repository;

		public TicketsController(ITicketRepository repository)
		{
			_repository = repository;
		}

		[HttpGet]
		public ActionResult<List<Ticket>> GetAll() => _repository.GetAll();

		[HttpGet("{id}")]
		public ActionResult<Ticket> GetById(int id)
		{
			var ticket = _repository.GetById(id);
			if (ticket == null) return NotFound();
			return ticket;
		}

		[HttpPost]
		public ActionResult<Ticket> Add(Ticket ticket)
		{
			var created = _repository.Add(ticket);
			return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
		}

		[HttpPut("{id}")]
		public IActionResult Update(int id, Ticket ticket)
		{
			if (id != ticket.Id) return BadRequest();
			_repository.Update(ticket);
			return NoContent();
		}

		[HttpDelete("{id}")]
		public IActionResult Delete(int id)
		{
			_repository.Delete(id);
			return NoContent();
		}
	}
}
