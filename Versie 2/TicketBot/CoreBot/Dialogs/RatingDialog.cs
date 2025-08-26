using CoreBot.Cards;
using CoreBot.Models;
using CoreBot.Services;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CoreBot.Dialogs
{
	public class RatingDialog : ComponentDialog
	{
		public RatingDialog() : base(nameof(RatingDialog))
		{
			var waterfallSteps = new WaterfallStep[]
			{
				ShowMoviesCardAsync,
				ProcessMovieSelectionAsync,
				ShowRatingCardAsync,
				ProcessRatingCardAsync
			};

			AddDialog(new WaterfallDialog(nameof(WaterfallDialog), waterfallSteps));
			InitialDialogId = nameof(WaterfallDialog);
		}

		private async Task<DialogTurnResult> ShowMoviesCardAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
		{
			var movies = await ApiService<List<Movie>>.GetAsync("/movies");
			var movieNames = movies.ConvertAll(m => m.Name);

			var card = MoviesCard.CreateCardAttachment(movieNames);
			await stepContext.Context.SendActivityAsync(MessageFactory.Attachment(card), cancellationToken);

			return Dialog.EndOfTurn; // wait for card submission
		}

		private async Task<DialogTurnResult> ProcessMovieSelectionAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
		{
			var submission = stepContext.Context.Activity.Value as dynamic;
			string selectedMovie = submission.selectedMovie;
			stepContext.Values["selectedMovie"] = selectedMovie;

			return await stepContext.NextAsync(null, cancellationToken);
		}

		private async Task<DialogTurnResult> ShowRatingCardAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
		{
			var movieName = (string)stepContext.Values["selectedMovie"];
			var card = RatingCard.CreateCardAttachment(movieName);

			await stepContext.Context.SendActivityAsync(MessageFactory.Attachment(card), cancellationToken);
			return Dialog.EndOfTurn; // wait for rating submission
		}

		private async Task<DialogTurnResult> ProcessRatingCardAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
		{
			if (stepContext.Context.Activity.Value is not null)
			{
				try
				{
					var submission = stepContext.Context.Activity.Value as dynamic;
					string movieName = submission.MovieName;
					int rating = int.Parse((string)submission.Rating);

					var movies = await ApiService<List<Movie>>.GetAsync("/movies");
					var movie = movies.Find(m => m.Name.Equals(movieName, StringComparison.OrdinalIgnoreCase));

					if (movie != null)
					{
						movie.Rating = rating;
						await ApiService<Movie>.PutAsync($"/movies/{movie.Id}", movie);

						var ratingCard = MovieRatingCard.CreateCardAttachment(movieName, rating);
						await stepContext.Context.SendActivityAsync(MessageFactory.Attachment(ratingCard), cancellationToken);
					}
					else
					{
						await stepContext.Context.SendActivityAsync(
							MessageFactory.Text($"Error: Movie {movieName} not found."), cancellationToken);
					}
				}
				catch (Exception ex)
				{
					await stepContext.Context.SendActivityAsync(
						MessageFactory.Text($"Oops! Something went wrong: {ex.Message}"), cancellationToken);
				}
			}
			else
			{
				await stepContext.Context.SendActivityAsync(
					MessageFactory.Text("No rating data received."), cancellationToken);
			}

			return await stepContext.EndDialogAsync(null, cancellationToken);
		}
	}
}
