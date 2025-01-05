using System;

namespace CoreBot.Models
{
    public class BookTicketDetails
    {
        public string CustomerName { get; set; }
        public bool CozySeat { get; set; }

        public int MovieId { get; set; }
        public Movie Movie { get; set; }

        public int ScheduleId { get; set; }
        public Schedule Schedule { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public DateTime OrderDate { get; set; }
    }
}
