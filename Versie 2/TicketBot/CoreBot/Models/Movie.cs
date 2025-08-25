using System.Collections.Generic;

namespace CoreBot.Models
{
	public class Movie
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public int Rating { get; set; }
		public ICollection<Schedule>? Schedules { get; set; }

	}
}
