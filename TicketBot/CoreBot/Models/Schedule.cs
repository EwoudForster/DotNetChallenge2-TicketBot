using System;
using System.Collections.Generic;

namespace CoreBot.Models
{
    public class Schedule
    {
        public int Id { get; set; }
        public int MovieHallId { get; set; }
        public MovieHall MovieHall { get; set; }
        public int MovieId { get; set; }
        public Movie Movie { get; set; }
        public DateTime Date { get; set; }

        public ICollection<Ticket>? Tickets { get; set; }

    }
}
