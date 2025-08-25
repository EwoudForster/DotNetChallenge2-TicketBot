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
			// Add necessary prompts
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
			// Get dummy movies
			var movies = await ApiService<List<Movie>>.GetAsync("/movie");

			// Create choices for the user
			var choices = new List<Choice>();
			foreach (var m in movies)
			{
				choices.Add(new Choice(m.Name));
			}

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

			await stepContext.Context.SendActivityAsync(
				MessageFactory.Text($"You selected: {selectedMovie}"), cancellationToken);

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

			// Mock update
			await ApiService<Movie>.PutAsync($"/movie/{movieName}", new Movie { Name = movieName, Rating = rating });

			await stepContext.Context.SendActivityAsync(
				MessageFactory.Text($"Thanks! You rated **{movieName}** a {rating}/5."), cancellationToken);

			return await stepContext.EndDialogAsync(null, cancellationToken);
		}
	}
}
