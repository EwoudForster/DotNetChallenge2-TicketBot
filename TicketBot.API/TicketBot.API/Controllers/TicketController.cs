using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TicketBot.DAL.Dtos;
using TicketBot.DAL.Models;
using TicketBot.DAL.Repositories;

namespace TicketBot.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly TicketRepository _ticketRepository;
        private readonly IMapper _mapper;

        public TicketController(TicketRepository ticketRepository, IMapper mapper)
        {
            _ticketRepository = ticketRepository;
            _mapper = mapper;
        }

        // GET: api/Tickets
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TicketDto>>> GetTickets()
        {
            var tickets = await _ticketRepository.GetAllWithIncludesAsync();
            var ticketDtos = _mapper.Map<IEnumerable<TicketDto>>(tickets);
            return Ok(ticketDtos);
        }

        // GET: api/Tickets/{id}
        [HttpGet("{id:int}")]
        public async Task<ActionResult<TicketDto>> GetTicket(int id)
        {
            var ticket = await _ticketRepository.GetByIdAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }

            var ticketDto = _mapper.Map<TicketDto>(ticket);
            return Ok(ticketDto);
        }

        // POST: api/Tickets
        [HttpPost]
        public async Task<ActionResult<TicketDto>> CreateTicket(CreateTicketDto createTicketDto)
        {
            var ticket = _mapper.Map<Ticket>(createTicketDto);

            await _ticketRepository.AddAsync(ticket);
            await _ticketRepository.SaveChangesAsync();

            var ticketDto = _mapper.Map<TicketDto>(ticket);
            return CreatedAtAction(nameof(GetTicket), new { id = ticket.Id }, ticketDto);
        }

        // DELETE: api/Tickets/{id}
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteTicket(int id)
        {
            var ticket = await _ticketRepository.GetByIdAsync(id);
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
