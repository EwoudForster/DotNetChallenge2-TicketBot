// Repositories/InMemoryMovieHallRepository.cs
using System.Collections.Generic;
using System.Linq;
using TicketBotApi.Models;

namespace CoreBot.Repositories
{
	public class InMemoryMovieHallRepository : IMovieHallRepository
	{
		private readonly List<MovieHall> _halls = new()
		{
			new MovieHall { Id = 1, Name = "Theater 1" },
			new MovieHall { Id = 2, Name = "Theater 2" },
			new MovieHall { Id = 3, Name = "Theater 3" }
		};

		public List<MovieHall> GetAll() => _halls;

		public MovieHall GetById(int id) => _halls.FirstOrDefault(h => h.Id == id);

		public MovieHall Add(MovieHall hall)
		{
			hall.Id = _halls.Max(h => h.Id) + 1;
			_halls.Add(hall);
			return hall;
		}

		public void Update(MovieHall hall)
		{
			var existing = GetById(hall.Id);
			if (existing != null)
				existing.Name = hall.Name;
		}

		public void Delete(int id)
		{
			var hall = GetById(id);
			if (hall != null)
				_halls.Remove(hall);
		}
	}
}
