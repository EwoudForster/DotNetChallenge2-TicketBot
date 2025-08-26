using System.Collections.Generic;
using Microsoft.Bot.Schema;
using CoreBot.Models;
using AdaptiveCards;

namespace CoreBot.Cards
{
	public static class ScheduleCard
	{
		public static Attachment CreateCardAttachment(List<Schedule> schedules)
		{
			var card = new AdaptiveCard(new AdaptiveSchemaVersion(1, 3))
			{
				Body = new List<AdaptiveElement>()
			};

			foreach (var s in schedules)
			{
				string movieName = s.Movie?.Name ?? "Unknown Movie";
				string theaterName = s.MovieHall?.Name ?? "Unknown Theater";
				string time = s.Date.ToString("HH:mm");

				// Each schedule in its own container
				var container = new AdaptiveContainer
				{
					Items = new List<AdaptiveElement>
					{
						new AdaptiveTextBlock
						{
							Text = $"🎬 {movieName}",
							Weight = AdaptiveTextWeight.Bolder,
							Size = AdaptiveTextSize.Medium,
							Wrap = true
						},
						new AdaptiveTextBlock
						{
							Text = $"🏛️ {theaterName}   ⏰ {time}",
							Weight = AdaptiveTextWeight.Default,
							Size = AdaptiveTextSize.Small,
							Color = AdaptiveTextColor.Attention,
							Wrap = true,
							Spacing = AdaptiveSpacing.Small
						},
						new AdaptiveTextBlock
						{
							Text = "─ ─ ─ ─ ─ ─ ─",
							Color = AdaptiveTextColor.Accent,
							Size = AdaptiveTextSize.Small,
							Separator = true
						}
					}
				};

				card.Body.Add(container);
			}

			return new Attachment
			{
				ContentType = AdaptiveCard.ContentType,
				Content = card
			};
		}
	}
}
