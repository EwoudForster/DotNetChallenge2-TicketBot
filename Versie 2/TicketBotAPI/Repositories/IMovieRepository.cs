// Repositories/IMovieRepository.cs
using System.Collections.Generic;
using TicketBotApi.Models;

namespace CoreBot.Repositories
{
	public interface IMovieRepository
	{
		List<Movie> GetAll();
		Movie GetById(int id);
		Movie Add(Movie movie);
		void Update(Movie movie);
		void Delete(int id);
	}
}
