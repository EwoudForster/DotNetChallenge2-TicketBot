using AdaptiveCards;
using System.Collections.Generic;
using System;
using Microsoft.Bot.Schema;
using CoreBot.Models;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace CoreBot.Cards
{
	public class ScheduleCard
	{
		public static Attachment CreateCardAttachment(List<Schedule> schedules)
		{
			var card = new AdaptiveCard(new AdaptiveSchemaVersion(1, 0))
			{
				Body = new List<AdaptiveElement>
				{
					new AdaptiveColumnSet
					{
						Columns = new List<AdaptiveColumn>
						{
							new AdaptiveColumn
							{
								Width = "stretch",
								Items = new List<AdaptiveElement>
								{
									new AdaptiveTextBlock
									{
										Text = "Schedule",
										Weight = AdaptiveTextWeight.Bolder,
										Size = AdaptiveTextSize.Large
									}
								}
							}
						}
					},
					new AdaptiveTextBlock
					{
						Text = $"This is the schedule",
						Weight = AdaptiveTextWeight.Default,
						Size = AdaptiveTextSize.Medium,
						Spacing = AdaptiveSpacing.Large
					},
					new AdaptiveContainer {
						Items = schedules.Select(schedule => new AdaptiveTextBlock
						{
							Text = $"• {schedule.Movie.Name} {schedule.MovieHall.Name} - _{schedule.MovieHall.Location}_",
							Wrap = true,
							Spacing = AdaptiveSpacing.None
						}).Cast<AdaptiveElement>().ToList()
					}
				}
			};

			var adaptiveCardAttachment = new Attachment()
			{
				ContentType = "application/vnd.microsoft.card.adaptive",
				Content = JObject.FromObject(card)
			};

			return adaptiveCardAttachment;
		}

	}
}

