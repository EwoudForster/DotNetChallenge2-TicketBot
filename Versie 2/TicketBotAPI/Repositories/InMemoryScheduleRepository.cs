// Repositories/InMemoryScheduleRepository.cs
using System;
using System.Collections.Generic;
using System.Linq;
using TicketBotApi.Models;

namespace CoreBot.Repositories
{
	public class InMemoryScheduleRepository : IScheduleRepository
	{
		private readonly List<Schedule> _schedules = new()
		{
			new Schedule
			{
				Id = 1,
				MovieId = 1,
				Movie = new Movie { Id = 1, Name = "Avengers: Endgame", Rating = 5 },
				MovieHallId = 1,
				MovieHall = new MovieHall { Id = 1, Name = "Theater 1" },
				Date = DateTime.Today.AddHours(12)
			},
			new Schedule
			{
				Id = 2,
				MovieId = 2,
				Movie = new Movie { Id = 2, Name = "Spider-Man: No Way Home", Rating = 4 },
				MovieHallId = 2,
				MovieHall = new MovieHall { Id = 2, Name = "Theater 2" },
				Date = DateTime.Today.AddHours(15)
			},
			new Schedule
			{
				Id = 3,
				MovieId = 3,
				Movie = new Movie { Id = 3, Name = "Inception", Rating = 5 },
				MovieHallId = 3,
				MovieHall = new MovieHall { Id = 3, Name = "Theater 3" },
				Date = DateTime.Today.AddHours(18).AddMinutes(30)
			}
		};

		public List<Schedule> GetAll() => _schedules;

		public Schedule GetById(int id) => _schedules.FirstOrDefault(s => s.Id == id);

		public Schedule Add(Schedule schedule)
		{
			schedule.Id = _schedules.Max(s => s.Id) + 1;
			_schedules.Add(schedule);
			return schedule;
		}

		public void Update(Schedule schedule)
		{
			var existing = GetById(schedule.Id);
			if (existing != null)
			{
				existing.MovieId = schedule.MovieId;
				existing.MovieHallId = schedule.MovieHallId;
				existing.Movie = schedule.Movie;
				existing.MovieHall = schedule.MovieHall;
				existing.Date = schedule.Date;
			}
		}

		public void Delete(int id)
		{
			var schedule = GetById(id);
			if (schedule != null)
				_schedules.Remove(schedule);
		}
	}
}
