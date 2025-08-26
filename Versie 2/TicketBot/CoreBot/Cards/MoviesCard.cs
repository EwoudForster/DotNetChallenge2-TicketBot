using AdaptiveCards;
using CoreBot.Models;
using Microsoft.Bot.Schema;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

namespace CoreBot.Cards
{
	public static class MoviesCard
	{
		public static Attachment CreateCardAttachment(List<Movie> movies)
		{
			var card = new AdaptiveCard(new AdaptiveSchemaVersion(1, 0))
			{
				Body = new List<AdaptiveElement>
				{
					new AdaptiveTextBlock
					{
						Text = "All movies",
						Weight = AdaptiveTextWeight.Bolder,
						Size = AdaptiveTextSize.Large
					},
					new AdaptiveChoiceSetInput
					{
						Id = "movieChoice",
						Style = AdaptiveChoiceInputStyle.Compact,
						Choices = movies.Select(m => new AdaptiveChoice { Title = m.Name, Value = m.Name }).ToList(),
						IsMultiSelect = false
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
