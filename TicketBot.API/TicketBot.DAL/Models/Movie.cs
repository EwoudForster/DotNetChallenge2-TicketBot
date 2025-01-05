using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketBot.DAL.Models
{
    public class Movie
    {
        [Key]
        [Column("Id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        [Range(1, 5)]
        public int Rating { get; set; }
        public ICollection<Schedule>? Schedules { get; set; }

    }
}
