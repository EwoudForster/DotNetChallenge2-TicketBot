using CoreBot.Models;
using CoreBot.Services;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Choices;
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
			AddDialog(new ChoicePrompt(nameof(ChoicePrompt)));
			AddDialog(new TextPrompt(nameof(TextPrompt)));

			var waterfallSteps = new WaterfallStep[]
			{
				AskForMovieAsync,
				ProcessMovieSelectionAsync,
				AskRatingAsync,
				ProcessRatingAsync
			};

			AddDialog(new WaterfallDialog(nameof(WaterfallDialog), waterfallSteps));
			InitialDialogId = nameof(WaterfallDialog);
		}

		private async Task<DialogTurnResult> AskForMovieAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
		{
			var movies = await ApiService<List<Movie>>.GetAsync("/movies"); // API call

			var choices = movies.ConvertAll(m => new Choice(m.Name));

			return await stepContext.PromptAsync(nameof(ChoicePrompt), new PromptOptions
			{
				Prompt = MessageFactory.Text("Please select a movie to rate:"),
				Choices = choices,
				RetryPrompt = MessageFactory.Text("Sorry, I didn't understand. Please select a valid movie.")
			}, cancellationToken);
		}

		private async Task<DialogTurnResult> ProcessMovieSelectionAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
		{
			var selectedMovie = ((FoundChoice)stepContext.Result).Value;
			stepContext.Values["selectedMovie"] = selectedMovie;

			await stepContext.Context.SendActivityAsync(MessageFactory.Text($"You selected: {selectedMovie}"), cancellationToken);

			return await stepContext.NextAsync(null, cancellationToken);
		}

		private async Task<DialogTurnResult> AskRatingAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
		{
			var movieName = (string)stepContext.Values["selectedMovie"];

			return await stepContext.PromptAsync(nameof(TextPrompt), new PromptOptions
			{
				Prompt = MessageFactory.Text($"Please give a rating for {movieName} (1-5):")
			}, cancellationToken);
		}

		private async Task<DialogTurnResult> ProcessRatingAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
		{
			var movieName = (string)stepContext.Values["selectedMovie"];
			var rating = int.Parse((string)stepContext.Result);

			// Get the movie by name to retrieve its Id
			var movies = await ApiService<List<Movie>>.GetAsync("/movies");
			var movie = movies.Find(m => m.Name.Equals(movieName, StringComparison.OrdinalIgnoreCase));

			if (movie != null)
			{
				movie.Rating = rating;

				// Use movie.Id in the PUT endpoint
				await ApiService<Movie>.PutAsync($"/movies/{movie.Id}", movie);
				await stepContext.Context.SendActivityAsync(
					MessageFactory.Text($"Thanks! You rated **{movieName}** a {rating}/5."), cancellationToken);
			}
			else
			{
				await stepContext.Context.SendActivityAsync(
					MessageFactory.Text($"Error: Movie {movieName} not found."), cancellationToken);
			}

			return await stepContext.EndDialogAsync(null, cancellationToken);
		}

	}
}
