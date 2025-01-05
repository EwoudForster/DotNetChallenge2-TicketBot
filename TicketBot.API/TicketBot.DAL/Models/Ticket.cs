using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketBot.DAL.Models
{
    public class Ticket
    {
        [Key]
        [Column("Id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string CustomerName { get; set; } // Name of the person ordering the ticket

        public int ScheduleId { get; set; } // Reference to the scheduled movie
        public Schedule Schedule { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.UtcNow; // Timestamp for the ticket order
    }
}
