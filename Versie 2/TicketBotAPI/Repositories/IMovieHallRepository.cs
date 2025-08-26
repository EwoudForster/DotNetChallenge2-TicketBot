// Repositories/IMovieHallRepository.cs
using System.Collections.Generic;
using TicketBotApi.Models;

namespace CoreBot.Repositories
{
	public interface IMovieHallRepository
	{
		List<MovieHall> GetAll();
		MovieHall GetById(int id);
		MovieHall Add(MovieHall hall);
		void Update(MovieHall hall);
		void Delete(int id);
	}
}
