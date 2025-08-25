using System.Collections.Generic;
using Microsoft.Bot.Schema;
using CoreBot.Models;

namespace CoreBot.Cards
{
	public static class ScheduleCard
	{
		public static Attachment CreateCardAttachment(List<Schedule> schedules)
		{
			var card = new AdaptiveCards.AdaptiveCard("1.3")
			{
				Body = new List<AdaptiveCards.AdaptiveElement>()
			};

			foreach (var s in schedules)
			{
				// Old simple version:
				// card.Body.Add(new AdaptiveCards.AdaptiveTextBlock($"{s.MovieName} - {s.Time}"));

				// Updated version using new model
				string movieName = s.Movie?.Name ?? "Unknown Movie";
				string theaterName = s.MovieHall?.Name ?? "Unknown Theater";
				string time = s.Date.ToString("HH:mm"); // Display just the time
				card.Body.Add(new AdaptiveCards.AdaptiveTextBlock($"{movieName} - {theaterName} - {time}"));
			}

			return new Attachment
			{
				ContentType = AdaptiveCards.AdaptiveCard.ContentType,
				Content = card
			};
		}
	}
}
