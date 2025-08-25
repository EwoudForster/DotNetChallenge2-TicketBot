using AdaptiveCards;
using Microsoft.Bot.Schema;
using Newtonsoft.Json.Linq;
using System;
using CoreBot.Models;
using System.Collections.Generic;

namespace CoreBot.Cards
{
	public class TicketCard
	{
		public static Attachment CreateTicketCard(Ticket ticket)
		{
			// Create the Ticket Information Card
			var ticketCard = new AdaptiveCard(new AdaptiveSchemaVersion(1, 0))
			{
				Body = new List<AdaptiveElement>
				{
					new AdaptiveTextBlock
					{
						Text = "Ticket Confirmation",
						Weight = AdaptiveTextWeight.Bolder,
						Size = AdaptiveTextSize.Large
					},
					new AdaptiveTextBlock
					{
						Text = $"**Movie:** {ticket.Schedule.Movie.Name}",
						Size = AdaptiveTextSize.Medium
					},
					new AdaptiveTextBlock
					{
						Text = $"**Movie Hall:** {ticket.Schedule.MovieHall.Name}",
						Size = AdaptiveTextSize.Medium
					},
					new AdaptiveTextBlock
					{
						Text = $"**Order Date:** {ticket.OrderDate:MMMM dd, yyyy hh:mm tt}",
						Size = AdaptiveTextSize.Medium
					},
					new AdaptiveTextBlock
					{
						Text = $"**Customer Name:** {ticket.CustomerName}",
						Size = AdaptiveTextSize.Medium
					}
				}
			};

			var cardAttachment = new Attachment
			{
				ContentType = "application/vnd.microsoft.card.adaptive",
				Content = JObject.FromObject(ticketCard)
			};

			return cardAttachment;
		}
	}
}
