using AdaptiveCards;
using Microsoft.Bot.Schema;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace CoreBot.Cards
{
	public class MovieRatingCard
	{
		public static Attachment CreateCardAttachment(string movieName, int rating)
		{
			// Generate star display: filled stars for the rating, empty stars for the rest
			string stars = new string('★', rating) + new string('☆', 5 - rating);

			var card = new AdaptiveCard(new AdaptiveSchemaVersion(1, 0))
			{
				Body = new List<AdaptiveElement>
				{
					new AdaptiveTextBlock
					{
						Text = $"You rated the movie: {movieName}",
						Weight = AdaptiveTextWeight.Bolder,
						Size = AdaptiveTextSize.Large,
						Wrap = true
					},
					new AdaptiveTextBlock
					{
						Text = stars,
						Weight = AdaptiveTextWeight.Bolder,
						Color = AdaptiveTextColor.Attention,
						Size = AdaptiveTextSize.Large,
						Wrap = true
					}
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
