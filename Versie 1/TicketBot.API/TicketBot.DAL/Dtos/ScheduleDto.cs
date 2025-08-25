using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketBot.DAL.Dtos
{
    public class ScheduleDto
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }
        public int MovieHallId { get; set; }
        public MovieHallDto MovieHall { get; set; }
        public int MovieId { get; set; }
        public MovieDto Movie { get; set; }
        public ICollection<TicketDto>? Tickets { get; set; }
    }
}
