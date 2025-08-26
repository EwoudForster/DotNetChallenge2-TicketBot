using CoreBot.Cards;
using CoreBot.Services;
using CoreBot.Models;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Choices;
using Microsoft.Bot.Schema;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System;

namespace CoreBot.Dialogs
{
	public class BookTicketDialog : CancelAndHelpDialog
	{
		public BookTicketDialog() : base(nameof(BookTicketDialog))
		{
			AddDialog(new ChoicePrompt(nameof(ChoicePrompt)));
			AddDialog(new TextPrompt(nameof(TextPrompt)));

			var waterfallSteps = new WaterfallStep[]
			{
				AskMovieAsync,
				ProcessMovieSelectionAsync,
				AskHallAsync,
				ProcessHallSelectionAsync,
				AskNameAsync,
				ProcessNameAsync,
				CreateTicketAsync
			};

			AddDialog(new WaterfallDialog(nameof(WaterfallDialog), waterfallSteps));
			InitialDialogId = nameof(WaterfallDialog);
		}

		private async Task<DialogTurnResult> AskMovieAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
		{
			var movies = await ApiService<List<Movie>>.GetAsync("/movies"); // API call

			var choices = movies.Select(m => new Choice(m.Name)).ToList();

			return await stepContext.PromptAsync(nameof(ChoicePrompt), new PromptOptions
			{
				Prompt = MessageFactory.Text("Please select a movie:"),
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

		private async Task<DialogTurnResult> AskHallAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
		{
			var selectedMovie = (string)stepContext.Values["selectedMovie"];

			var schedules = await ApiService<List<Schedule>>.GetAsync("/schedules"); // API call
			var movieHalls = schedules
				.Where(s => s.Movie.Name.Equals(selectedMovie, StringComparison.OrdinalIgnoreCase))
				.Select(s => s.MovieHall.Name)
				.Distinct()
				.ToList();

			var choices = movieHalls.Select(h => new Choice(h)).ToList();

			return await stepContext.PromptAsync(nameof(ChoicePrompt), new PromptOptions
			{
				Prompt = MessageFactory.Text("Please select a movie hall:"),
				Choices = choices,
				RetryPrompt = MessageFactory.Text("Sorry, I didn't understand. Please select a valid hall.")
			}, cancellationToken);
		}

		private async Task<DialogTurnResult> ProcessHallSelectionAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
		{
			var selectedHall = ((FoundChoice)stepContext.Result).Value;
			stepContext.Values["selectedMovieHall"] = selectedHall;

			await stepContext.Context.SendActivityAsync(MessageFactory.Text($"You selected hall: {selectedHall}"), cancellationToken);
			return await stepContext.NextAsync(null, cancellationToken);
		}

		private async Task<DialogTurnResult> AskNameAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
		{
			return await stepContext.PromptAsync(nameof(TextPrompt), new PromptOptions
			{
				Prompt = MessageFactory.Text("Please enter your full name:"),
				RetryPrompt = MessageFactory.Text("Name cannot be empty. Please enter your full name.")
			}, cancellationToken);
		}

		private async Task<DialogTurnResult> ProcessNameAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
		{
			var userName = (string)stepContext.Result;
			stepContext.Values["userName"] = userName;

			await stepContext.Context.SendActivityAsync(MessageFactory.Text($"Hello, {userName}!"), cancellationToken);
			return await stepContext.NextAsync(null, cancellationToken);
		}

		private async Task<DialogTurnResult> CreateTicketAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
		{
			var userName = (string)stepContext.Values["userName"];
			var selectedMovie = (string)stepContext.Values["selectedMovie"];
			var selectedHall = (string)stepContext.Values["selectedMovieHall"];

			var schedules = await ApiService<List<Schedule>>.GetAsync("/schedules"); // API call
			var schedule = schedules.FirstOrDefault(s =>
				s.Movie.Name.Equals(selectedMovie, StringComparison.OrdinalIgnoreCase) &&
				s.MovieHall.Name.Equals(selectedHall, StringComparison.OrdinalIgnoreCase));

			if (schedule == null)
			{
				await stepContext.Context.SendActivityAsync(MessageFactory.Text("Sorry, no matching schedule found."), cancellationToken);
				return await stepContext.EndDialogAsync(cancellationToken: cancellationToken);
			}

			var ticket = new Ticket
			{
				CustomerName = userName,
				ScheduleId = schedule.Id,
				OrderDate = DateTime.UtcNow
			};

			await ApiService<Ticket>.PostAsync("/tickets", ticket); // API call

			var ticketCard = TicketCard.CreateTicketCard(ticket);
			await stepContext.Context.SendActivityAsync(MessageFactory.Attachment(ticketCard), cancellationToken);

			return await stepContext.EndDialogAsync(cancellationToken: cancellationToken);
		}
	}
}
