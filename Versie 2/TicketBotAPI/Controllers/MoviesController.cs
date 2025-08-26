using Microsoft.AspNetCore.Mvc;
using CoreBot.Repositories;
using System.Collections.Generic;
using TicketBotApi.Models;

namespace CoreBot.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class MoviesController : ControllerBase
	{
		private readonly IMovieRepository _repository;

		public MoviesController(IMovieRepository repository)
		{
			_repository = repository;
		}

		[HttpGet]
		public ActionResult<List<Movie>> GetAll() => _repository.GetAll();

		[HttpGet("{id}")]
		public ActionResult<Movie> GetById(int id)
		{
			var movie = _repository.GetById(id);
			if (movie == null) return NotFound();
			return movie;
		}

		[HttpPost]
		public ActionResult<Movie> Add(Movie movie)
		{
			var created = _repository.Add(movie);
			return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
		}

		[HttpPut("{id}")]
		public IActionResult Update(int id, Movie movie)
		{
			if (id != movie.Id) return BadRequest();
			_repository.Update(movie);
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
