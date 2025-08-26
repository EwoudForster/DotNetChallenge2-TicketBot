using AdaptiveCards;
using CoreBot.Models;
using Microsoft.Bot.Schema;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

namespace CoreBot.Cards
{
	public static class MovieHallsCard
	{
		public static Attachment CreateCardAttachment(List<MovieHall> halls, List<Schedule> schedules, string movieName)
		{
			var hallsForMovie = schedules
				.Where(s => s.Movie.Name == movieName)
				.Select(s => s.MovieHall)
				.Distinct()
				.ToList();

			var card = new AdaptiveCard(new AdaptiveSchemaVersion(1, 0))
			{
				Body = new List<AdaptiveElement>
				{
					new AdaptiveTextBlock
					{
						Text = $"Select a hall for '{movieName}'",
						Weight = AdaptiveTextWeight.Bolder,
						Size = AdaptiveTextSize.Large
					},
					new AdaptiveChoiceSetInput
					{
						Id = "movieHallChoice",
						Style = AdaptiveChoiceInputStyle.Compact,
						Choices = hallsForMovie.Select(h => new AdaptiveChoice { Title = h.Name, Value = h.Name }).ToList(),
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
