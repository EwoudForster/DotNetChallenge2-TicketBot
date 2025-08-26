using Microsoft.AspNetCore.Mvc;
using CoreBot.Repositories;
using System.Collections.Generic;
using TicketBotApi.Models;

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
		public ActionResult<List<Schedule>> GetAll() => _repository.GetAll();

		[HttpGet("{id}")]
		public ActionResult<Schedule> GetById(int id)
		{
			var schedule = _repository.GetById(id);
			if (schedule == null) return NotFound();
			return schedule;
		}

		[HttpPost]
		public IActionResult Post([FromBody] ScheduleDto dto)
		{
			var schedule = new Schedule
			{
				MovieId = dto.MovieId,
				MovieHallId = dto.MovieHallId,
				Date = dto.Date
			};

			_repository.Add(schedule);
			return CreatedAtAction(nameof(GetById), new { id = schedule.Id }, schedule);
		}


		[HttpPut("{id}")]
		public IActionResult Put(int id, [FromBody] ScheduleDto dto)
		{
			var existing = _repository.GetById(id);
			if (existing == null) return NotFound();

			existing.MovieId = dto.MovieId;
			existing.MovieHallId = dto.MovieHallId;
			existing.Date = dto.Date;

			_repository.Update(existing);
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
