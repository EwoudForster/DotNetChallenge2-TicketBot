namespace TicketBot.DAL.Dtos
{
    public class MovieHallDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Location { get; set; }
        public ICollection<ScheduleDto>? Schedules { get; set; }
    }
}
