// Repositories/InMemoryMovieRepository.cs
using System.Collections.Generic;
using System.Linq;
using TicketBotApi.Models;

namespace CoreBot.Repositories
{
	public class InMemoryMovieRepository : IMovieRepository
	{
		private readonly List<Movie> _movies = new()
		{
			new Movie { Id = 1, Name = "Avengers: Endgame", Rating = 5 },
			new Movie { Id = 2, Name = "Spider-Man: No Way Home", Rating = 4 },
			new Movie { Id = 3, Name = "Inception", Rating = 5 }
		};

		public List<Movie> GetAll() => _movies;

		public Movie GetById(int id) => _movies.FirstOrDefault(m => m.Id == id);

		public Movie Add(Movie movie)
		{
			movie.Id = _movies.Max(m => m.Id) + 1;
			_movies.Add(movie);
			return movie;
		}

		public void Update(Movie movie)
		{
			var existing = GetById(movie.Id);
			if (existing != null)
			{
				existing.Name = movie.Name;
				existing.Rating = movie.Rating;
			}
		}

		public void Delete(int id)
		{
			var movie = GetById(id);
			if (movie != null)
				_movies.Remove(movie);
		}
	}
}
