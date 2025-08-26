using CoreBot.Cards;
using CoreBot.Services;
using CoreBot.Models;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
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
			var movieNames = movies.Select(m => m.Name).ToList();

			var movieCard = MoviesCard.CreateCardAttachment(movieNames);
			await stepContext.Context.SendActivityAsync(MessageFactory.Attachment(movieCard), cancellationToken);

			return Dialog.EndOfTurn; // Wait for card submission
		}

		private async Task<DialogTurnResult> ProcessMovieSelectionAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
		{
			if (stepContext.Context.Activity.Value is not null)
			{
				var submission = stepContext.Context.Activity.Value as dynamic;
				string selectedMovie = submission.selectedMovie;
				stepContext.Values["selectedMovie"] = selectedMovie;

				return await stepContext.NextAsync(null, cancellationToken);
			}

			await stepContext.Context.SendActivityAsync("⚠️ Please select a movie first.", cancellationToken: cancellationToken);
			return await stepContext.ReplaceDialogAsync(nameof(BookTicketDialog), cancellationToken: cancellationToken);
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

			var hallCard = MovieHallsCard.CreateCardAttachment(movieHalls);
			await stepContext.Context.SendActivityAsync(MessageFactory.Attachment(hallCard), cancellationToken);

			return Dialog.EndOfTurn; // Wait for card submission
		}

		private async Task<DialogTurnResult> ProcessHallSelectionAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
		{
			if (stepContext.Context.Activity.Value is not null)
			{
				var submission = stepContext.Context.Activity.Value as dynamic;
				string selectedHall = submission.selectedHall;
				stepContext.Values["selectedMovieHall"] = selectedHall;

				return await stepContext.NextAsync(null, cancellationToken);
			}

			await stepContext.Context.SendActivityAsync("⚠️ Please select a hall first.", cancellationToken: cancellationToken);
			return await stepContext.ReplaceDialogAsync(nameof(BookTicketDialog), cancellationToken: cancellationToken);
		}

		private async Task<DialogTurnResult> AskNameAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
		{
			return await stepContext.PromptAsync(nameof(TextPrompt), new PromptOptions
			{
				Prompt = MessageFactory.Text("👤 Please enter your full name:"),
				RetryPrompt = MessageFactory.Text("⚠️ Name cannot be empty. Please enter your full name.")
			}, cancellationToken);
		}

		private async Task<DialogTurnResult> ProcessNameAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
		{
			var userName = (string)stepContext.Result;
			stepContext.Values["userName"] = userName;

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
				await stepContext.Context.SendActivityAsync(MessageFactory.Text("❌ Sorry, no matching schedule found."), cancellationToken);
				return await stepContext.EndDialogAsync(cancellationToken: cancellationToken);
			}

			// Post the ticket DTO and get back the created Ticket from API
			var ticketDto = new TicketCreateDto
			{
				CustomerName = userName,
				ScheduleId = schedule.Id
			};

			// Change ApiService call to return the created Ticket
			var createdTicket = await ApiService<Ticket>.PostAsync("/tickets", ticketDto);

			// Send ticket card
			var ticketCard = TicketCard.CreateTicketCard(createdTicket);
			await stepContext.Context.SendActivityAsync(MessageFactory.Attachment(ticketCard), cancellationToken);

			return await stepContext.EndDialogAsync(cancellationToken: cancellationToken);
		}
	}
}
