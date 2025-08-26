using AdaptiveCards;
using Microsoft.Bot.Schema;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace CoreBot.Cards
{
	public class MovieHallsCard
	{
		public static Attachment CreateCardAttachment(List<string> hallNames)
		{
			var card = new AdaptiveCard(new AdaptiveSchemaVersion(1, 3))
			{
				Body = new List<AdaptiveElement>
				{
					new AdaptiveTextBlock
					{
						Text = "🏛️ Please select a movie hall:",
						Weight = AdaptiveTextWeight.Bolder,
						Size = AdaptiveTextSize.Medium,
						Wrap = true,
						Separator = true
					},
					new AdaptiveChoiceSetInput
					{
						Id = "selectedHall",
						Style = AdaptiveChoiceInputStyle.Compact,
						Choices = hallNames.ConvertAll(name => new AdaptiveChoice
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
