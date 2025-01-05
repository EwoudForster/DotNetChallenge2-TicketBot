using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketBot.DAL.Dtos;
using TicketBot.DAL.Models;
using TicketBot.DAL.Repositories;

namespace TicketBot.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieHallController : ControllerBase
    {
        private readonly IMovieHallRepository _movieHallRepository;
        private readonly IMapper _mapper;

        public MovieHallController(IMovieHallRepository movieHallRepository, IMapper mapper)
        {
            _movieHallRepository = movieHallRepository;
            _mapper = mapper;
        }

        // GET: api/MovieHalls
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MovieHallDto>>> GetMovieHalls()
        {
            var movieHalls = await _movieHallRepository.GetAllAsync();
            var movieHallDtos = _mapper.Map<IEnumerable<MovieHallDto>>(movieHalls);
            return Ok(movieHallDtos);
        }

        // GET: api/MovieHalls/{id}
        [HttpGet("{id:int}")]
        public async Task<ActionResult<MovieHallDto>> GetMovieHall(int id)
        {
            var movieHall = await _movieHallRepository.GetByIdAsync(id);
            if (movieHall == null)
            {
                return NotFound();
            }

            var movieHallDto = _mapper.Map<MovieHallDto>(movieHall);
            return Ok(movieHallDto);
        }

        // POST: api/MovieHalls
        [HttpPost]
        public async Task<ActionResult<MovieHallDto>> PostMovieHall(MovieHall movieHall)
        {
            await _movieHallRepository.AddAsync(movieHall);
            await _movieHallRepository.SaveChangesAsync();

            var movieHallDto = _mapper.Map<MovieHallDto>(movieHall);
            return CreatedAtAction(nameof(GetMovieHall), new { id = movieHall.Id }, movieHallDto);
        }

        // PUT: api/MovieHalls/{id}
        [HttpPut("{id:int}")]
        public async Task<IActionResult> PutMovieHall(int id, MovieHall movieHall)
        {
            if (id != movieHall.Id)
            {
                return BadRequest();
            }

            _movieHallRepository.Update(movieHall);

            try
            {
                await _movieHallRepository.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await _movieHallRepository.GetByIdAsync(id) == null)
                {
                    return NotFound();
                }

                throw;
            }

            return NoContent();
        }

        // DELETE: api/MovieHalls/{id}
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteMovieHall(int id)
        {
            var movieHall = await _movieHallRepository.GetByIdAsync(id);
            if (movieHall == null)
            {
                return NotFound();
            }

            _movieHallRepository.Remove(movieHall);
            await _movieHallRepository.SaveChangesAsync();

            return NoContent();
        }
    }
}
