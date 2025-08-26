using AdaptiveCards;
using Microsoft.Bot.Schema;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace CoreBot.Cards
{
	public class MoviesCard
	{
		public static Attachment CreateCardAttachment(List<string> movieNames)
		{
			var card = new AdaptiveCard(new AdaptiveSchemaVersion(1, 3))
			{
				Body = new List<AdaptiveElement>
				{
					new AdaptiveTextBlock
					{
						Text = "🎬 Please select a movie:",
						Weight = AdaptiveTextWeight.Bolder,
						Size = AdaptiveTextSize.Medium,
						Wrap = true,
						Separator = true
					},
					new AdaptiveChoiceSetInput
					{
						Id = "selectedMovie",
						Style = AdaptiveChoiceInputStyle.Compact,
						Choices = movieNames.ConvertAll(name => new AdaptiveChoice
						{
							Title = name,
							Value = name
						}),
						IsMultiSelect = false
					}
				},
				Actions = new List<AdaptiveAction>
				{
					new AdaptiveSubmitAction
					{
						Title = "✅ Confirm Selection"
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
