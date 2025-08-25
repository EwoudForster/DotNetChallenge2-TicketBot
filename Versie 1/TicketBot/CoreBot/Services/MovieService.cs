using CoreBot.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CoreBot.Services
{
    public class MovieService
    {
        public static async Task<List<Movie>> GetMoviesAsync()
        {
            return await ApiService<List<Movie>>.GetAsync("/movies");
        }
    }
}
