using Microsoft.EntityFrameworkCore;
using TicketBot.DAL.Models;

namespace TicketBot.DAL.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Movie> Movies { get; set; }
        public DbSet<MovieHall> MovieHalls { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<Ticket> Tickets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Movie>().ToTable("Movie");
            modelBuilder.Entity<MovieHall>().ToTable("MovieHall");
            modelBuilder.Entity<Schedule>().ToTable("Schedule");
            modelBuilder.Entity<Ticket>().ToTable("Ticket");

            // Configure relationships and constraints
            modelBuilder.Entity<Schedule>()
                .HasOne(s => s.Movie)
                .WithMany(m => m.Schedules)
                .HasForeignKey(s => s.MovieId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Schedule>()
                .HasOne(s => s.MovieHall)
                .WithMany(mh => mh.Schedules)
                .HasForeignKey(s => s.MovieHallId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.Schedule)
                .WithMany(s => s.Tickets)
                .HasForeignKey(s => s.ScheduleId)
                .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);
        }
    }
}
