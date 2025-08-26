using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CoreBot.Services
{
	public static class ApiService<T>
	{
		// Change this to your local API URL
		private static readonly string BASE_URL = "https://localhost:5001/api";
		private static readonly HttpClient client = new HttpClient() { Timeout = TimeSpan.FromSeconds(60) };

		// ------------------- MOCK DATA (for fallback/testing) -------------------
		/*
        private static List<Movie> mockMovies = new List<Movie>
        {
            new Movie { Id = 1, Name = "Avengers: Endgame", Rating = 5 },
            new Movie { Id = 2, Name = "Spider-Man: No Way Home", Rating = 4 },
            new Movie { Id = 3, Name = "Inception", Rating = 5 }
        };

        private static List<Schedule> mockSchedules = new List<Schedule>
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
            }
        };
        */

		public static async Task<T> GetAsync(string endPoint)
		{
			try
			{
				var response = await client.GetAsync(BASE_URL + endPoint);
				response.EnsureSuccessStatusCode();
				var jsonData = await response.Content.ReadAsStringAsync();
				return JsonConvert.DeserializeObject<T>(jsonData);
			}
			catch (Exception ex)
			{
				// Optional: fallback to mock data
				// if (typeof(T) == typeof(List<Movie>)) return (T)(object)mockMovies;
				// if (typeof(T) == typeof(List<Schedule>)) return (T)(object)mockSchedules;

				throw new Exception("GET request failed: " + ex.Message, ex);
			}
		}

		public static async Task<T> PostAsync(string endPoint, object data)
		{
			var json = JsonConvert.SerializeObject(data, new JsonSerializerSettings
			{
				ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver()
			});
			var content = new StringContent(json, Encoding.UTF8, "application/json");
			var response = await client.PostAsync(BASE_URL + endPoint, content);

			response.EnsureSuccessStatusCode();

			var jsonData = await response.Content.ReadAsStringAsync();
			return JsonConvert.DeserializeObject<T>(jsonData);
		}


		public static async Task PutAsync(string endPoint, T data)
		{
			try
			{
				var json = JsonConvert.SerializeObject(data, new JsonSerializerSettings
				{
					ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver()
				});
				var content = new StringContent(json, Encoding.UTF8, "application/json");
				var response = await client.PutAsync(BASE_URL + endPoint, content);

				if (!response.IsSuccessStatusCode)
				{
					var errorContent = await response.Content.ReadAsStringAsync();
					throw new Exception($"PUT request failed ({response.StatusCode}): {errorContent}");
				}
			}
			catch (Exception ex)
			{
				throw new Exception("PUT request failed: " + ex.Message, ex);
			}
		}

		public static async Task DeleteAsync(string endPoint)
		{
			try
			{
				var response = await client.DeleteAsync(BASE_URL + endPoint);

				if (!response.IsSuccessStatusCode)
				{
					var errorContent = await response.Content.ReadAsStringAsync();
					throw new Exception($"DELETE request failed ({response.StatusCode}): {errorContent}");
				}
			}
			catch (Exception ex)
			{
				throw new Exception("DELETE request failed: " + ex.Message, ex);
			}
		}
	}
}
