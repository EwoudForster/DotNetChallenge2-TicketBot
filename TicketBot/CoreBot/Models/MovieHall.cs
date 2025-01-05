using System.Collections.Generic;

namespace CoreBot.Models
{
    public class MovieHall
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }

        public ICollection<Schedule>? Schedules { get; set; }
    }
}
