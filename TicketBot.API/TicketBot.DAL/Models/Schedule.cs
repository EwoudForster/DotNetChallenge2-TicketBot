using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketBot.DAL.Models
{
    public class Schedule
    {

        [Key]
        [Column("Id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int MovieHallId { get; set; }
        public MovieHall MovieHall { get; set; }
        public int MovieId { get; set; }
        public Movie Movie { get; set; }
        public ICollection<Ticket>? Tickets { get; set; }
    }
}
