namespace TicketBot.DAL.Dtos
{
    public class MovieDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public int Rating { get; set; }
        public ICollection<ScheduleDto>? Schedules { get; set; }
    }
}
