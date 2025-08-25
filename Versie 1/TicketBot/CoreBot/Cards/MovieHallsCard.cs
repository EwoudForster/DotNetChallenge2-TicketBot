using AdaptiveCards;
using System.Collections.Generic;
using System;
using Microsoft.Bot.Schema;
using CoreBot.Models;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace CoreBot.Cards
{
	public class MovieHallsCard
	{
		public static Attachment CreateCardAttachment(List<MovieHall> movieHalls, List<Schedule> schedules, string selectedMovieName)
		{
			var movieHallsForSelectedMovie = schedules
			   .Where(schedule => schedule.Movie.Name == selectedMovieName)
			   .Select(schedule => schedule.MovieHall)
			   .Distinct()  // To avoid duplicate movie halls if a movie is scheduled multiple times
			   .ToList();

			var filteredMovieHalls = movieHalls
			   .Where(hall => movieHallsForSelectedMovie.Any(mh => mh.Id == hall.Id))
			   .ToList();

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
										Text = "All moviehalls",
										Weight = AdaptiveTextWeight.Bolder,
										Size = AdaptiveTextSize.Large
									}
								}
							}
						}
					},
					new AdaptiveChoiceSetInput
					{
						Id = "movieHallChoice",  // ID to reference the selected choice
                        Style = AdaptiveChoiceInputStyle.Compact,
						Choices = movieHalls.Select(hall => new AdaptiveChoice
						{
							Title = hall.Name,  // The name of the moviehall
                            Value = hall.Name   // The value returned when the moviehall is selected
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

