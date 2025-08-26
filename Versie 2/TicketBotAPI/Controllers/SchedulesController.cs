using Microsoft.AspNetCore.Mvc;
using CoreBot.Repositories;
using TicketBotApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreBot.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class SchedulesController : ControllerBase
	{
		private readonly IScheduleRepository _repository;

		public SchedulesController(IScheduleRepository repository)
		{
			_repository = repository;
		}

		[HttpGet]
		public async Task<ActionResult<List<Schedule>>> GetAll() =>
			await _repository.GetAllAsync();

		[HttpGet("{id}")]
		public async Task<ActionResult<Schedule>> GetById(int id)
		{
			var schedule = await _repository.GetByIdAsync(id);
			if (schedule == null) return NotFound();
			return schedule;
		}

		[HttpPost]
		public async Task<ActionResult<Schedule>> Add(Schedule schedule)
		{
			var created = await _repository.AddAsync(schedule);
			return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> Update(int id, Schedule schedule)
		{
			if (id != schedule.Id) return BadRequest();
			await _repository.UpdateAsync(schedule);
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
