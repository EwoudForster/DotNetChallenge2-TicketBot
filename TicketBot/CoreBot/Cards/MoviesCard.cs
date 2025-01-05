using AdaptiveCards;
using System.Collections.Generic;
using System;
using Microsoft.Bot.Schema;
using CoreBot.Models;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace CoreBot.Cards
{
	public class MoviesCard
	{
		public static Attachment CreateCardAttachment(List<Movie> movies)
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
										Text = "All movies",
										Weight = AdaptiveTextWeight.Bolder,
										Size = AdaptiveTextSize.Large
									}
								}
							}
						}
					},
					new AdaptiveChoiceSetInput
					{
						Id = "movieChoice",  // ID to reference the selected choice
                        Style = AdaptiveChoiceInputStyle.Compact,
						Choices = movies.Select(movie => new AdaptiveChoice
						{
							Title = movie.Name,  // The name of the movie
                            Value = movie.Name    // The value returned when the movie is selected
                        }).ToList(),
						IsMultiSelect = false // Allow only single selection
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

