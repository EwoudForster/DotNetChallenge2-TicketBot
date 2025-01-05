using Microsoft.Bot.Builder.Dialogs.Choices;
using CoreBot.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreBot.Models
{
	public class MovieDataService
	{
		public static async Task<List<Movie>> GetMoviesAsync()
		{
			return await ApiService<List<Movie>>.GetAsync($"movie");
		}

		public async static Task<Movie> GetMovieByNameAsync(string name)
		{
			return await ApiService<Movie>.GetAsync($"movie/by-name/{name}");
		}

	}
}
