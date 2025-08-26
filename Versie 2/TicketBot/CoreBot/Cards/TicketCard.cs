using AdaptiveCards;
using CoreBot.Models;
using Microsoft.Bot.Schema;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace CoreBot.Cards
{
	public static class TicketCard
	{
		public static Attachment CreateTicketCard(Ticket ticket)
		{
			var card = new AdaptiveCard(new AdaptiveSchemaVersion(1, 0))
			{
				Body = new List<AdaptiveElement>
				{
					new AdaptiveTextBlock
					{
						Text = "Your Ticket",
						Weight = AdaptiveTextWeight.Bolder,
						Size = AdaptiveTextSize.Large
					},
					new AdaptiveTextBlock { Text = $"Customer: {ticket.CustomerName}" },
					new AdaptiveTextBlock { Text = $"Schedule ID: {ticket.ScheduleId}" },
					new AdaptiveTextBlock { Text = $"Order Date: {ticket.OrderDate:yyyy-MM-dd HH:mm}" }
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
