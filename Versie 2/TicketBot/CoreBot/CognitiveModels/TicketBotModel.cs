using Microsoft.Bot.Builder;
using Microsoft.BotBuilderSamples.Clu;
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
		public Dictionary<Intent, IntentScore> Intents { get; set; } = new();
		public CluEntities Entities { get; set; } = new();
		public IDictionary<string, object> Properties { get; set; } = new Dictionary<string, object>();

		public void Convert(dynamic result)
		{
			// Only copy necessary fields; no JSON serialization needed
			Text = result.Text;
			AlteredText = result.AlteredText;
			Intents = result.Intents;
			Entities = result.Entities;
			Properties = result.Properties;
		}

		public (Intent intent, double score) GetTopIntent()
		{
			if (Intents == null || Intents.Count == 0)
				return (Intent.None, 0.0);

			var top = Intents.OrderByDescending(i => i.Value.Score ?? 0).First();
			return (top.Key, top.Value.Score ?? 0.0);
		}

		public class CluEntities
		{
			public CluEntity[] Entities { get; set; } = new CluEntity[0];

			public string GetEntity(string category)
				=> Entities.FirstOrDefault(e => e.Category == category)?.Text;

			public string GetMovieName() => GetEntity("MovieName");
			public string GetDate() => GetEntity("Date");
			public string GetCount() => GetEntity("Count");
			public string GetTime() => GetEntity("Time");
			public string GetEmail() => GetEntity("Email");
			public string GetPhone() => GetEntity("Phone");
			public string GetSeatPreference() => GetEntity("SeatPreference");
		}
	}
}
