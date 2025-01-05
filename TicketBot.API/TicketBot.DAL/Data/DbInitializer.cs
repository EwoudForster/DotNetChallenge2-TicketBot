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
                new Movie { Name = "The Matrix", Description = "A hacker discovers the nature of reality", Rating = 5 },
                new Movie { Name = "Interstellar", Description = "A journey through space to save humanity", Rating = 5 },
                new Movie { Name = "The Dark Knight", Description = "Batman fights against the Joker in Gotham", Rating = 5 },
                new Movie { Name = "The Godfather", Description = "The story of the powerful Corleone crime family", Rating = 5 },
                new Movie { Name = "Pulp Fiction", Description = "A non-linear story about crime in Los Angeles", Rating = 5 },
                new Movie { Name = "Fight Club", Description = "An underground fight club becomes a form of rebellion", Rating = 5 },
                new Movie { Name = "The Shawshank Redemption", Description = "Two prisoners form a friendship that transcends time", Rating = 5 }
            };
            context.Movies.AddRange(movies);
            context.SaveChanges(); // Save to generate IDs

            // Seed MovieHalls
            var movieHalls = new[]
            {
                new MovieHall { Name = "Hall A", Location = "First Floor" },
                new MovieHall { Name = "Hall B", Location = "Second Floor" },
                new MovieHall { Name = "Hall C", Location = "Third Floor" },
                new MovieHall { Name = "Hall D", Location = "Fourth Floor" }
            };
            context.MovieHalls.AddRange(movieHalls);
            context.SaveChanges(); // Save to generate IDs

            // Seed Schedules
            var schedules = new[]
            {
                new Schedule { MovieId = movies[0].Id, MovieHallId = movieHalls[0].Id },
                new Schedule { MovieId = movies[1].Id, MovieHallId = movieHalls[1].Id },
                new Schedule { MovieId = movies[2].Id, MovieHallId = movieHalls[2].Id },
                new Schedule { MovieId = movies[3].Id, MovieHallId = movieHalls[3].Id },
                new Schedule { MovieId = movies[4].Id, MovieHallId = movieHalls[0].Id },
                new Schedule { MovieId = movies[5].Id, MovieHallId = movieHalls[1].Id },
                new Schedule { MovieId = movies[6].Id, MovieHallId = movieHalls[2].Id },
                new Schedule { MovieId = movies[7].Id, MovieHallId = movieHalls[3].Id }
            };
            context.Schedules.AddRange(schedules);
            context.SaveChanges(); // Save to generate IDs

            // Seed Tickets with phone numbers and email addresses
            var tickets = new[]
            {
                new Ticket { CustomerName = "John Doe", Phone = "0412345678", Email = "johndoe@example.com", Schedule = schedules[0], OrderDate = DateTime.UtcNow },
                new Ticket { CustomerName = "Jane Smith", Phone = "0423456789", Email = "janesmith@example.com", Schedule = schedules[1], OrderDate = DateTime.UtcNow },
                new Ticket { CustomerName = "Alex Johnson", Phone = "0434567890", Email = "alexjohnson@example.com", Schedule = schedules[2], OrderDate = DateTime.UtcNow },
                new Ticket { CustomerName = "Maria Garcia", Phone = "0445678901", Email = "mariagarcia@example.com", Schedule = schedules[3], OrderDate = DateTime.UtcNow },
                new Ticket { CustomerName = "David Lee", Phone = "0456789012", Email = "davidlee@example.com", Schedule = schedules[4], OrderDate = DateTime.UtcNow },
                new Ticket { CustomerName = "Sophia Brown", Phone = "0467890123", Email = "sophiabrown@example.com", Schedule = schedules[5], OrderDate = DateTime.UtcNow },
                new Ticket { CustomerName = "Michael White", Phone = "0478901234", Email = "michaelwhite@example.com", Schedule = schedules[6], OrderDate = DateTime.UtcNow },
                new Ticket { CustomerName = "Emily Davis", Phone = "0489012345", Email = "emilydavis@example.com", Schedule = schedules[7], OrderDate = DateTime.UtcNow }
            };
            context.Tickets.AddRange(tickets);

            // Save all changes
            context.SaveChanges();
        }
    }
}
