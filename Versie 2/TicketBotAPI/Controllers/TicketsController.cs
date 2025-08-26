using Microsoft.AspNetCore.Mvc;
using CoreBot.Repositories;
using TicketBotApi.Models;
using TicketBotApi.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace CoreBot.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class TicketsController : ControllerBase
	{
		private readonly ITicketRepository _repository;
		private readonly BotDbContext _context;

		public TicketsController(ITicketRepository repository)
		{
			_repository = repository;
		}

		[HttpGet]
		public async Task<ActionResult<List<Ticket>>> GetAll() =>
			await _repository.GetAllAsync();

		[HttpGet("{id}")]
		public async Task<ActionResult<Ticket>> GetById(int id)
		{
			var ticket = await _repository.GetByIdAsync(id);
			if (ticket == null) return NotFound();
			return ticket;
		}
		[HttpPost]
		public async Task<IActionResult> PostTicket([FromBody] TicketCreateDto dto)
		{
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			var ticket = new Ticket
			{
				CustomerName = dto.CustomerName,
				ScheduleId = dto.ScheduleId,
				OrderDate = DateTime.UtcNow
			};

			var addedTicket = await _repository.AddAsync(ticket);

			return CreatedAtAction(nameof(GetById), new { id = addedTicket.Id }, addedTicket);
		}


		[HttpPut("{id}")]
		public async Task<IActionResult> Update(int id, Ticket ticket)
		{
			if (id != ticket.Id) return BadRequest();
			await _repository.UpdateAsync(ticket);
			return NoContent();
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(int id)
		{
			await _repository.DeleteAsync(id);
			return NoContent();
		}
	}
}
