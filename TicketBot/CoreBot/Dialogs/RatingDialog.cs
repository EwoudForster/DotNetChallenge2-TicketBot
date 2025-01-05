using CoreBot.Cards;
using FirstBot.Helpers;
using CoreBot.Services;
using CoreBot.Models;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Bot.Builder.Dialogs.Choices;
using System;

namespace CoreBot.Dialogs
{
	public class RatingDialog : CancelAndHelpDialog
	{
		public RatingDialog()
			: base(nameof(RatingDialog))
		{

			var waterfallSteps = new WaterfallStep[]
			{
				AskForMovieAsync,
				ProcessMovieSelectionAsync,
				AskRatingAsync,
				ProcessRatingAsync
			};

			AddDialog(new WaterfallDialog(nameof(WaterfallDialog), waterfallSteps));

			// The initial child Dialog to run
			InitialDialogId = nameof(WaterfallDialog);
		}

		private async Task<DialogTurnResult> AskForMovieAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
		{

			var movies = await ApiService<List<Movie>>.GetAsync("/movie");

			// Use the MoviesCard to generate the card attachment with the list of movies
			var movieSelectionCard = MoviesCard.CreateCardAttachment(movies);

			// Convert the card to an attachment
			var response = MessageFactory.Attachment(movieSelectionCard);

			// Send the ScheduleCard to the user
			await stepContext.Context.SendActivityAsync(response, cancellationToken);

			// End the dialog
			return await stepContext.PromptAsync(nameof(ChoicePrompt), new PromptOptions
			{
				Prompt = MessageFactory.Text("Please select a movie from the options."),
				RetryPrompt = MessageFactory.Text("Sorry, I didn't understand that. Please select a valid movie."),
			}, cancellationToken);
		}

		private async Task<DialogTurnResult> ProcessMovieSelectionAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
		{
			// Retrieve the user's selection
			var selectedMovie = ((FoundChoice)stepContext.Result).Value; // The movie name

			// Process the selected movie
			await stepContext.Context.SendActivityAsync(MessageFactory.Text($"You selected: {selectedMovie}"), cancellationToken);

			// Optionally, end the dialog or proceed with further processing
			return await stepContext.EndDialogAsync(cancellationToken);
		}

		private async Task<DialogTurnResult> AskRatingAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
		{
			// Retrieve the movie name from the previous dialog or step
			var movieName = stepContext.Values["selectedMovie"] as string ?? "the movie";

			// Create the rating card
			var ratingCard = RatingCard.CreateCardAttachment(movieName);

			// Send the card to the user
			var cardMessage = MessageFactory.Attachment(ratingCard);
			await stepContext.Context.SendActivityAsync(cardMessage, cancellationToken);

			// End this step and wait for the user's input
			return await stepContext.PromptAsync(
				nameof(TextPrompt), // Use TextPrompt to capture the rating
				new PromptOptions
				{
					Prompt = MessageFactory.Text("Please select a rating, then click Submit.")
				},
				cancellationToken
			);
		}

		private async Task<DialogTurnResult> ProcessRatingAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
		{
			// Retrieve the values submitted by the user
			var submittedData = stepContext.Context.Activity.Value as IDictionary<string, object>;

			if (submittedData != null)
			{
				var movieName = submittedData["MovieName"].ToString();
				var userRating = int.Parse(submittedData["rating"].ToString());
				var comments = submittedData["comments"]?.ToString();

				// Fetch the movie from your database or API
				var movie = await ApiService<Movie>.GetAsync($"/movie/{movieName}");

				if (movie != null)
				{
					// Get the current rating from the Movie object
					var currentRating = movie.Rating;

					// Calculate the average rating
					var averageRating = (currentRating + userRating) / 2.0;

					// Round the average rating to the nearest whole number
					var roundedRating = (int)Math.Round(averageRating);

					// Update the movie's rating
					movie.Rating = roundedRating;

					// Use PutAsync to update the movie
					await ApiService<Movie>.PutAsync($"/movie/{movie.Id}", movie); // PUT for updating the movie

					// Respond to the user
					await stepContext.Context.SendActivityAsync(
						MessageFactory.Text($"Thank you for rating {movieName}! The current average rating is {roundedRating}/5." +
											(string.IsNullOrEmpty(comments) ? "" : $" Your comments: {comments}")),
						cancellationToken);
				}
				else
				{
					await stepContext.Context.SendActivityAsync(
						MessageFactory.Text($"Sorry, we couldn't find the movie '{movieName}' to update its rating."),
						cancellationToken);
				}
			}

			return await stepContext.EndDialogAsync(cancellationToken);
		}
	}
}
