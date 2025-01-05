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
				Text = "How would you rate this movie on a scale of 1 to 5?",
				Wrap = true
			},
			new AdaptiveChoiceSetInput
			{
				Id = "rating", // The ID used to retrieve this input
                Style = AdaptiveChoiceInputStyle.Compact,
				Choices = new List<AdaptiveChoice>
				{
					new AdaptiveChoice { Title = "1", Value = "1" },
					new AdaptiveChoice { Title = "2", Value = "2" },
					new AdaptiveChoice { Title = "3", Value = "3" },
					new AdaptiveChoice { Title = "4", Value = "4" },
					new AdaptiveChoice { Title = "5", Value = "5" }
				},
				IsMultiSelect = false
			},
		},
				Actions = new List<AdaptiveAction>
		{
			new AdaptiveSubmitAction
			{
				Title = "Submit",
				Data = new { MovieName = movieName } // Pass the movie name back with the submission
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
