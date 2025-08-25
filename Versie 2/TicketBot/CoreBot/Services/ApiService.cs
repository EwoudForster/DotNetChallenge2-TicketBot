using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using CoreBot.Models;
using Newtonsoft.Json;

namespace CoreBot.Services
{
	public static class ApiService<T>
	{
		// Original BASE_URL and HttpClient setup
		// private static readonly string BASE_URL = "https://ticketbotapi.azure-api.net/api";
		// static HttpClient client = new HttpClient() { Timeout = TimeSpan.FromSeconds(60) };

		// Mock storage for testing
		private static List<Movie> mockMovies = new List<Movie>
		{
			new Movie { Id = 1, Name = "Avengers: Endgame", Rating = 5 },
			new Movie { Id = 2, Name = "Spider-Man: No Way Home", Rating = 4 },
			new Movie { Id = 3, Name = "Inception", Rating = 5 }
		};

		public static async Task<T> GetAsync(string endPoint)
		{
			// Dummy data for testing
			if (typeof(T) == typeof(List<Schedule>))
			{
				var dummySchedules = new List<Schedule>
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

				return (T)(object)dummySchedules;
			}

			if (typeof(T) == typeof(List<Movie>))
			{
				return (T)(object)mockMovies;
			}

			if (typeof(T) == typeof(Movie))
			{
				// Simple retrieval by name (endPoint example: "/movie/Avengers: Endgame")
				var parts = endPoint.Split('/');
				var name = parts.Length > 2 ? parts[2] : "";
				var movie = mockMovies.Find(m => m.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
				return (T)(object)movie;
			}

			// Original HTTP request code
			/*
            try
            {
                string url = BASE_URL + endPoint;
                var response = await client.GetAsync(url);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var jsonData = await response.Content.ReadAsStringAsync();
                    if (!string.IsNullOrWhiteSpace(jsonData))
                    {
                        return JsonConvert.DeserializeObject<T>(jsonData);
                    }
                    else
                    {
                        throw new Exception("Resource Not Found");
                    }
                }
                else
                {
                    throw new Exception("Request failed with status code " + response.StatusCode);
                }
            }
            catch
            {
                throw;
            }
            */

			return default(T);
		}

		public static Task PostAsync(string endPoint, T data)
		{
			// Original implementation
			// try { ... } 
			// catch { throw; }

			// Mock behavior: add movie to list
			if (data is Movie newMovie)
			{
				newMovie.Id = mockMovies.Count + 1;
				mockMovies.Add(newMovie);
			}

			return Task.CompletedTask;
		}

		public static Task PutAsync(string endPoint, T data)
		{
			// Original implementation
			// try { ... } 
			// catch { throw; }

			// Mock behavior: update movie in list
			if (data is Movie updatedMovie)
			{
				var existing = mockMovies.Find(m => m.Id == updatedMovie.Id);
				if (existing != null)
				{
					existing.Name = updatedMovie.Name;
					existing.Rating = updatedMovie.Rating;
				}
			}

			return Task.CompletedTask;
		}

		public static Task DeleteAsync(string endPoint)
		{
			// Original implementation
			// try { ... } 
			// catch { throw; }

			// Mock behavior: delete movie by ID
			var parts = endPoint.Split('/');
			if (parts.Length > 2 && int.TryParse(parts[2], out int id))
			{
				var existing = mockMovies.Find(m => m.Id == id);
				if (existing != null)
				{
					mockMovies.Remove(existing);
				}
			}

			return Task.CompletedTask;
		}
	}
}
