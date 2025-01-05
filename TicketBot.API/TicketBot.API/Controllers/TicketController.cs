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
    public class TicketController : ControllerBase
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly IMapper _mapper;

        public TicketController(ITicketRepository ticketRepository, IMapper mapper)
        {
            _ticketRepository = ticketRepository;
            _mapper = mapper;
        }

        // GET: api/Tickets
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TicketDto>>> GetTickets()
        {
            var tickets = await _ticketRepository.GetAllAsync(d => d.Schedule);
            var ticketDtos = _mapper.Map<IEnumerable<TicketDto>>(tickets);
            return Ok(ticketDtos);
        }

        // GET: api/Tickets/{id}
        [HttpGet("{id:int}")]
        public async Task<ActionResult<TicketDto>> GetTicket(int id)
        {
            var ticket = await _ticketRepository.GetByIDAsync(id, d => d.Schedule);
            if (ticket == null)
            {
                return NotFound();
            }

            var ticketDto = _mapper.Map<TicketDto>(ticket);
            return Ok(ticketDto);
        }

        // POST: api/Tickets
        [HttpPost]
        public async Task<ActionResult<TicketDto>> PostTicket(Ticket ticket)
        {
            await _ticketRepository.AddAsync(ticket);
            await _ticketRepository.SaveChangesAsync();

            var ticketDto = _mapper.Map<TicketDto>(ticket);
            return CreatedAtAction(nameof(GetTicket), new { id = ticket.Id }, ticketDto);
        }

        // PUT: api/Tickets/{id}
        [HttpPut("{id:int}")]
        public async Task<IActionResult> PutTicket(int id, Ticket ticket)
        {
            if (id != ticket.Id)
            {
                return BadRequest();
            }

            _ticketRepository.Update(ticket);

            try
            {
                await _ticketRepository.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await _ticketRepository.GetByIDAsync(id) == null)
                {
                    return NotFound();
                }

                throw;
            }

            return NoContent();
        }

        // DELETE: api/Tickets/{id}
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteTicket(int id)
        {
            var ticket = await _ticketRepository.GetByIDAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }

            _ticketRepository.Remove(ticket);
            await _ticketRepository.SaveChangesAsync();

            return NoContent();
        }
    }
}
