using System;

namespace CoreBot.Models
{
    public class Ticket
    {
        public int Id { get; set; }

        public string CustomerName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        public int ScheduleId { get; set; }
        public Schedule Schedule { get; set; }

        public DateTime OrderDate { get; set; }
    }
}
