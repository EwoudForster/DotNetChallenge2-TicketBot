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
    public class ScheduleController : ControllerBase
    {
        private readonly IScheduleRepository _scheduleRepository;
        private readonly IMapper _mapper;

        public ScheduleController(IScheduleRepository scheduleRepository, IMapper mapper)
        {
            _scheduleRepository = scheduleRepository;
            _mapper = mapper;
        }

        // GET: api/Schedules
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ScheduleDto>>> GetSchedules()
        {
            var schedules = await _scheduleRepository.GetAllAsync();
            var scheduleDtos = _mapper.Map<IEnumerable<ScheduleDto>>(schedules);
            return Ok(scheduleDtos);
        }

        // GET: api/Schedules/{id}
        [HttpGet("{id:int}")]
        public async Task<ActionResult<ScheduleDto>> GetSchedule(int id)
        {
            var schedule = await _scheduleRepository.GetByIdAsync(id);
            if (schedule == null)
            {
                return NotFound();
            }

            var scheduleDto = _mapper.Map<ScheduleDto>(schedule);
            return Ok(scheduleDto);
        }

        // POST: api/Schedules
        [HttpPost]
        public async Task<ActionResult<ScheduleDto>> PostSchedule(Schedule schedule)
        {
            await _scheduleRepository.AddAsync(schedule);
            await _scheduleRepository.SaveChangesAsync();

            var scheduleDto = _mapper.Map<ScheduleDto>(schedule);
            return CreatedAtAction(nameof(GetSchedule), new { id = schedule.Id }, scheduleDto);
        }

        // PUT: api/Schedules/{id}
        [HttpPut("{id:int}")]
        public async Task<IActionResult> PutSchedule(int id, Schedule schedule)
        {
            if (id != schedule.Id)
            {
                return BadRequest();
            }

            _scheduleRepository.Update(schedule);

            try
            {
                await _scheduleRepository.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await _scheduleRepository.GetByIdAsync(id) == null)
                {
                    return NotFound();
                }

                throw;
            }

            return NoContent();
        }

        // DELETE: api/Schedules/{id}
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteSchedule(int id)
        {
            var schedule = await _scheduleRepository.GetByIdAsync(id);
            if (schedule == null)
            {
                return NotFound();
            }

            _scheduleRepository.Remove(schedule);
            await _scheduleRepository.SaveChangesAsync();

            return NoContent();
        }
    }
}
