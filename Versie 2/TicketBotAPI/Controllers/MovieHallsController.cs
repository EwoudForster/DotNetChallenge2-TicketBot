using Microsoft.AspNetCore.Mvc;
using CoreBot.Repositories;
using TicketBotApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreBot.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class MovieHallsController : ControllerBase
	{
		private readonly IMovieHallRepository _repository;

		public MovieHallsController(IMovieHallRepository repository)
		{
			_repository = repository;
		}

		[HttpGet]
		public async Task<ActionResult<List<MovieHall>>> GetAll() =>
			await _repository.GetAllAsync();

		[HttpGet("{id}")]
		public async Task<ActionResult<MovieHall>> GetById(int id)
		{
			var hall = await _repository.GetByIdAsync(id);
			if (hall == null) return NotFound();
			return hall;
		}

		[HttpPost]
		public async Task<ActionResult<MovieHall>> Add(MovieHall hall)
		{
			var created = await _repository.AddAsync(hall);
			return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> Update(int id, MovieHall hall)
		{
			if (id != hall.Id) return BadRequest();
			await _repository.UpdateAsync(hall);
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
