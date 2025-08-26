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

			// Optional: Seed Movies
			modelBuilder.Entity<Movie>().HasData(
				new Movie { Id = 1, Name = "Inception", Rating = 9 },
				new Movie { Id = 2, Name = "The Matrix", Rating = 10 },
				new Movie { Id = 3, Name = "Titanic", Rating = 8 }
			);

			// Optional: Seed MovieHalls
			modelBuilder.Entity<MovieHall>().HasData(
				new MovieHall { Id = 1, Name = "Hall A" },
				new MovieHall { Id = 2, Name = "Hall B" }
			);

			// Optional: Seed Schedules
			modelBuilder.Entity<Schedule>().HasData(
				new Schedule { Id = 1, MovieId = 1, MovieHallId = 1, Date = DateTime.Now.AddHours(2) },
				new Schedule { Id = 2, MovieId = 2, MovieHallId = 2, Date = DateTime.Now.AddHours(4) },
				new Schedule { Id = 3, MovieId = 3, MovieHallId = 1, Date = DateTime.Now.AddHours(6) }
			);
		}
	}
}
