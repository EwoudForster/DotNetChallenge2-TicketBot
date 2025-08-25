using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketBot.DAL.Dtos
{

    public class TicketDto
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public int ScheduleId { get; set; }
        public ScheduleDto Schedule { get; set; }

        public DateTime OrderDate { get; set; }
    }
}