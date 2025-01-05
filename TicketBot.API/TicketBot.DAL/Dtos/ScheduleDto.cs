namespace TicketBot.DAL.Dtos
{
    public class ScheduleDto
    {
        public int Id { get; set; }
        public int MovieHallId { get; set; }
        public MovieHallDto MovieHall { get; set; }
        public int MovieId { get; set; }
        public MovieDto Movie { get; set; }
        public ICollection<TicketDto>? Tickets { get; set; }
    }
}
