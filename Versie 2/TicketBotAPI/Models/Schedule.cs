using System;

namespace TicketBotApi.Models
{
	public class Schedule
	{
		public int Id { get; set; }
		public int MovieId { get; set; }
		public Movie Movie { get; set; }
		public int MovieHallId { get; set; }
		public MovieHall MovieHall { get; set; }
		public DateTime Date { get; set; }
	}
}
