using Microsoft.EntityFrameworkCore;
using TicketBotApi.Models;

namespace TicketBotApi.Data
{
	public class BotDbContext : DbContext
	{
		public BotDbContext(DbContextOptions<BotDbContext> options)
			: base(options)
		{
		}

		public DbSet<Movie> Movies { get; set; } = null!;
		public DbSet<MovieHall> MovieHalls { get; set; } = null!;
		public DbSet<Schedule> Schedules { get; set; } = null!;
		public DbSet<Ticket> Tickets { get; set; } = null!;

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			// Schedule relationships
			modelBuilder.Entity<Schedule>()
				.HasOne(s => s.Movie)
				.WithMany()
				.HasForeignKey(s => s.MovieId)
				.OnDelete(DeleteBehavior.Cascade);

			modelBuilder.Entity<Schedule>()
				.HasOne(s => s.MovieHall)
				.WithMany()
				.HasForeignKey(s => s.MovieHallId)
				.OnDelete(DeleteBehavior.Cascade);

			// Seed Movies
			modelBuilder.Entity<Movie>().HasData(
				new Movie { Id = 1, Name = "Inception", Rating = 9 },
				new Movie { Id = 2, Name = "The Matrix", Rating = 10 },
				new Movie { Id = 3, Name = "Titanic", Rating = 8 }
			);

			// Seed MovieHalls
			modelBuilder.Entity<MovieHall>().HasData(
				new MovieHall { Id = 1, Name = "Hall A" },
				new MovieHall { Id = 2, Name = "Hall B" }
			);

			// Seed Schedules with fixed DateTime values
			modelBuilder.Entity<Schedule>().HasData(
				new Schedule { Id = 1, MovieId = 1, MovieHallId = 1, Date = new DateTime(2025, 8, 26, 16, 25, 0) },
				new Schedule { Id = 2, MovieId = 2, MovieHallId = 2, Date = new DateTime(2025, 8, 26, 18, 25, 0) },
				new Schedule { Id = 3, MovieId = 3, MovieHallId = 1, Date = new DateTime(2025, 8, 26, 20, 25, 0) }
			);
		}
	}
}
