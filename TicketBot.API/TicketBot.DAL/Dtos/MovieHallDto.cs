using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketBot.DAL.Dtos
{
    public class MovieHallDto
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Location { get; set; }
        public ICollection<ScheduleDto>? Schedules { get; set; }
    }
}
