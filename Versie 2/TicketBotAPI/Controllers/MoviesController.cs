using Microsoft.AspNetCore.Mvc;
using CoreBot.Repositories;
using TicketBotApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

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
		public async Task<ActionResult<List<Movie>>> GetAll() =>
			await _repository.GetAllAsync();

		[HttpGet("{id}")]
		public async Task<ActionResult<Movie>> GetById(int id)
		{
			var movie = await _repository.GetByIdAsync(id);
			if (movie == null) return NotFound();
			return movie;
		}

		[HttpPost]
		public async Task<ActionResult<Movie>> Add(Movie movie)
		{
			var created = await _repository.AddAsync(movie);
			return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> Update(int id, Movie movie)
		{
			if (id != movie.Id) return BadRequest();
			await _repository.UpdateAsync(movie);
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
