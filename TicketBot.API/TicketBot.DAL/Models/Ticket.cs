using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace TicketBot.DAL.Models
{
    public class Ticket
    {
        [Key]
        [Column("Id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string CustomerName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }

        public int ScheduleId { get; set; }
        [JsonIgnore]
        public Schedule Schedule { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
    }
}
