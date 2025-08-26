using AdaptiveCards;
using Microsoft.Bot.Schema;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace CoreBot.Cards
{
	public class RatingCard
	{
		public static Attachment CreateCardAttachment(string movieName)
		{
			var card = new AdaptiveCard(new AdaptiveSchemaVersion(1, 0))
			{
				Body = new List<AdaptiveElement>
				{
					new AdaptiveTextBlock
					{
						Text = $"Rate your experience with the movie: {movieName}",
						Weight = AdaptiveTextWeight.Bolder,
						Size = AdaptiveTextSize.Large,
						Wrap = true
					},
					new AdaptiveTextBlock
					{
						Text = "Click the number of stars to rate this movie:",
						Wrap = true
					}
				},
				Actions = new List<AdaptiveAction>
				{
					new AdaptiveSubmitAction { Title = "⭐ 1", Data = new { MovieName = movieName, Rating = "1" } },
					new AdaptiveSubmitAction { Title = "⭐⭐ 2", Data = new { MovieName = movieName, Rating = "2" } },
					new AdaptiveSubmitAction { Title = "⭐⭐⭐ 3", Data = new { MovieName = movieName, Rating = "3" } },
					new AdaptiveSubmitAction { Title = "⭐⭐⭐⭐ 4", Data = new { MovieName = movieName, Rating = "4" } },
					new AdaptiveSubmitAction { Title = "⭐⭐⭐⭐⭐ 5", Data = new { MovieName = movieName, Rating = "5" } }
				}
			};

			return new Attachment
			{
				ContentType = "application/vnd.microsoft.card.adaptive",
				Content = JObject.FromObject(card)
			};
		}
	}
}
