using System;

namespace TicketBotApi.Models
{
	public class Ticket
	{
		public int Id { get; set; }
		public string CustomerName { get; set; }
		public int ScheduleId { get; set; }
		public DateTime OrderDate { get; set; }
	}
}
