using Microsoft.Bot.Builder;
using Microsoft.BotBuilderSamples.Clu;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace CoreBot.CognitiveModels
{
	public class TicketBotModel : IRecognizerConvert
	{
		public enum Intent
		{
			BookTicket,
			LookShowTimes,
			ReadMovieDetails,
			LookAvailableSeats,
			None
		}

		public string Text { get; set; }

		public string AlteredText { get; set; }

		public Dictionary<Intent, IntentScore> Intents { get; set; }

		public CluEntities Entities { get; set; }

		public IDictionary<string, object> Properties { get; set; }

		public void Convert(dynamic result)
		{
			var jsonResult = JsonConvert.SerializeObject(result, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
			var app = JsonConvert.DeserializeObject<TicketBotModel>(jsonResult);

			Text = app.Text;
			AlteredText = app.AlteredText;
			Intents = app.Intents;
			Entities = app.Entities;
			Properties = app.Properties;
		}

		public (Intent intent, double score) GetTopIntent()
		{
			var maxIntent = Intent.None;
			var max = 0.0;
			foreach (var entry in Intents)
			{
				if (entry.Value.Score > max)
				{
					maxIntent = entry.Key;
					max = entry.Value.Score.Value;
				}
			}

			return (maxIntent, max);
		}

		public class CluEntities
		{
			public CluEntity[] Entities;

			public string GetMovieName() => Entities.Where(e => e.Category == "MovieName").ToArray().FirstOrDefault()?.Text;
			public string GetDate() => Entities.Where(e => e.Category == "Date").ToArray().FirstOrDefault()?.Text;
			public string GetCount() => Entities.Where(e => e.Category == "Count").ToArray().FirstOrDefault()?.Text;
			public string GetTime() => Entities.Where(e => e.Category == "Time").ToArray().FirstOrDefault()?.Text;
			public string GetEmail() => Entities.Where(e => e.Category == "Email").ToArray().FirstOrDefault()?.Text;
			public string GetPhone() => Entities.Where(e => e.Category == "Phone").ToArray().FirstOrDefault()?.Text;

			public string GetSeatPreference() => Entities.Where(e => e.Category == "SeatPreference").ToArray().FirstOrDefault()?.Text;

		}
	}

}
