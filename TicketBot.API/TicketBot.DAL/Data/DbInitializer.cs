using TicketBot.DAL.Models;

namespace TicketBot.DAL.Data
{
    public static class DbInitializer
    {
        public static void Initialize(AppDbContext context)
        {
            // Ensure database is created
            context.Database.EnsureCreated();

            // Check if database is already seeded
            if (context.Movies.Any() || context.MovieHalls.Any() || context.Schedules.Any() || context.Tickets.Any())
                return;

            // Seed Movies
            var movies = new[]
            {
                new Movie { Name = "Inception", Description = "A mind-bending sci-fi thriller", Rating = 5 },
                new Movie { Name = "The Matrix", Description = "A hacker discovers the nature of reality", Rating = 5 }
            };
            context.Movies.AddRange(movies);
            context.SaveChanges(); // Save to generate IDs

            // Seed MovieHalls
            var movieHalls = new[]
            {
                new MovieHall { Name = "Hall A", Location = "First Floor" },
                new MovieHall { Name = "Hall B", Location = "Second Floor" }
            };
            context.MovieHalls.AddRange(movieHalls);
            context.SaveChanges(); // Save to generate IDs

            // Seed Schedules
            var schedules = new[]
            {
                new Schedule { MovieId = movies[0].Id, MovieHallId = movieHalls[0].Id },
                new Schedule { MovieId = movies[1].Id, MovieHallId = movieHalls[1].Id }
            };
            context.Schedules.AddRange(schedules);
            context.SaveChanges(); // Save to generate IDs

            // Seed Tickets
            var tickets = new[]
            {
                new Ticket { CustomerName = "John Doe", Schedule = schedules[0], OrderDate = DateTime.UtcNow },
                new Ticket { CustomerName = "Jane Smith", Schedule = schedules[1], OrderDate = DateTime.UtcNow }
            };
            context.Tickets.AddRange(tickets);

            // Save all changes
            context.SaveChanges();
        }
    }
}
