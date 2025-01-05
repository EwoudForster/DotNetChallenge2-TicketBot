namespace TicketBot.DAL.Dtos
{

    public class TicketDto
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public int ScheduleId { get; set; }
        public ScheduleDto Schedule { get; set; }

        public DateTime OrderDate { get; set; }
    }
}