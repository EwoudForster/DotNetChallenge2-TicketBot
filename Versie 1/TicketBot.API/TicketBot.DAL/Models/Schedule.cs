using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace TicketBot.DAL.Models
{
    public class Schedule
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int MovieHallId { get; set; }
        [ForeignKey(nameof(MovieHallId))]
        public MovieHall MovieHall { get; set; }

        public int MovieId { get; set; }
        [ForeignKey(nameof(MovieId))]
        public Movie Movie { get; set; }

        [JsonIgnore]
        public ICollection<Ticket>? Tickets { get; set; }
    }
}
