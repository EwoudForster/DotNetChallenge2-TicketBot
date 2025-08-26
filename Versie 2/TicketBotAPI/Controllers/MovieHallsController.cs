using Microsoft.AspNetCore.Mvc;
using CoreBot.Repositories;
using System.Collections.Generic;
using TicketBotApi.Models;

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
		public ActionResult<List<MovieHall>> GetAll() => _repository.GetAll();

		[HttpGet("{id}")]
		public ActionResult<MovieHall> GetById(int id)
		{
			var hall = _repository.GetById(id);
			if (hall == null) return NotFound();
			return hall;
		}

		[HttpPost]
		public ActionResult<MovieHall> Add(MovieHall hall)
		{
			var created = _repository.Add(hall);
			return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
		}

		[HttpPut("{id}")]
		public IActionResult Update(int id, MovieHall hall)
		{
			if (id != hall.Id) return BadRequest();
			_repository.Update(hall);
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
