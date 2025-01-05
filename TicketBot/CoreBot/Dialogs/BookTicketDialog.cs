using CoreBot.Cards;
using CoreBot.Services;
using CoreBot.Models;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Bot.Builder.Dialogs.Choices;
using System.Linq;
using System;

namespace CoreBot.Dialogs
{
    public class BookTicketDialog : CancelAndHelpDialog
    {
        public BookTicketDialog()
            : base(nameof(BookTicketDialog))
        {

            var waterfallSteps = new WaterfallStep[]
            {
				AskMovieAsync,	//Step1: ask for movie
				ProcessMovieSelectionAsync,	//Step2: process choice
				AskHallAsync,	//Step3: ask for moviehall
				ProcessHallSelectionAsync,	//Step4: process choice
				AskNameAsync,	//Step5: ask for name
				ProcessNameAsync,	//Step6: process name
				CreateTicketAsync, //Step7: make ticket
            };

            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), waterfallSteps));

			// The initial child Dialog to run
            InitialDialogId = nameof(WaterfallDialog);
        }

		private async Task<DialogTurnResult> AskMovieAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
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

		private async Task<DialogTurnResult> AskHallAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
			var selectedMovieName = (string)stepContext.Result;

			var schedules = await ApiService<List<Schedule>>.GetAsync("/schedule");

			var movieHalls = await ApiService<List<MovieHall>>.GetAsync("/moviehall");

			// Use the MoviesCard to generate the card attachment with the list of movies
			var hallSelectionCard = MovieHallsCard.CreateCardAttachment(movieHalls, schedules, selectedMovieName);

			// Convert the card to an attachment
			var response = MessageFactory.Attachment(hallSelectionCard);

			// Send the ScheduleCard to the user
			await stepContext.Context.SendActivityAsync(response, cancellationToken);

			// End the dialog
			return await stepContext.PromptAsync(nameof(ChoicePrompt), new PromptOptions
            {
				Prompt = MessageFactory.Text("Please select a movie from the options."),
				RetryPrompt = MessageFactory.Text("Sorry, I didn't understand that. Please select a valid movie."),
			}, cancellationToken);
            }

		private async Task<DialogTurnResult> ProcessHallSelectionAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
		{
			// Retrieve the user's selection
			var selectedHall = ((FoundChoice)stepContext.Result).Value; // The movie name

			// Process the selected movie
			await stepContext.Context.SendActivityAsync(MessageFactory.Text($"You selected: {selectedHall}"), cancellationToken);

			// Optionally, end the dialog or proceed with further processing
			return await stepContext.EndDialogAsync(cancellationToken);
		}
		private async Task<DialogTurnResult> AskNameAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
			// Ask the user to provide their name
			var promptOptions = new PromptOptions
            {
				Prompt = MessageFactory.Text("What is your full name?"),
				RetryPrompt = MessageFactory.Text("Please enter your name.")
			};

			// Step 2: Wait for the user's response (name)
			return await stepContext.PromptAsync(nameof(TextPrompt), promptOptions, cancellationToken);
        }

		private async Task<Schedule> GetScheduleAsync(string movieName, string movieHallName)
        {
			// Make a request to your API to retrieve schedules
			var schedules = await ApiService<List<Schedule>>.GetAsync("/schedule");
			var movies = await ApiService<List<Movie>>.GetAsync("/movie");
			var movieHalls = await ApiService<List<MovieHall>>.GetAsync("/moviehall");

			var matchedMovie = movies.FirstOrDefault(movie => movie.Name.Equals(movieName, StringComparison.OrdinalIgnoreCase));
			var matchedHall = movieHalls.FirstOrDefault(hall => hall.Name.Equals(movieHallName, StringComparison.OrdinalIgnoreCase));

			if (matchedMovie == null || matchedHall == null)
            {
				return null;  // Return null if no matching movie or hall is found
            }

			var matchedSchedule = schedules.FirstOrDefault(schedule =>
			schedule.Movie.Id == matchedMovie.Id && schedule.MovieHall.Id == matchedHall.Id);

			return matchedSchedule;
        }

		private async Task<DialogTurnResult> ProcessNameAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
			// Retrieve the user's name from the prompt result
			var userName = (string)stepContext.Result;
			var selectedMovie = stepContext.Values["selectedMovie"] as string;
			var selectedMovieHall = stepContext.Values["selectedMovieHall"] as string;

			// Respond with a confirmation message
			await stepContext.Context.SendActivityAsync(MessageFactory.Text($"Hello, {userName}!"), cancellationToken);

			// Continue with the next step or end the dialog
			return await stepContext.EndDialogAsync(cancellationToken);
        }

		private async Task<DialogTurnResult> CreateTicketAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
            {
			// Retrieve the stored user's name from dialog state
			var userName = (string)stepContext.Values["userName"];

			// Retrieve the selected movie and movie hall from dialog state
			var selectedMovie = stepContext.Values["selectedMovie"] as string;
			var selectedMovieHall = stepContext.Values["selectedMovieHall"] as string;

			// Assume you have a method to get the schedule based on the selected movie and movie hall
			var selectedSchedule = await GetScheduleAsync(selectedMovie, selectedMovieHall);

			// Create a new Ticket using the userName and other selected information
			var ticket = new Ticket
        {
				CustomerName = userName,  // Use the user's name
				ScheduleId = selectedSchedule?.Id ?? 0,  // Set the ScheduleId from the matched schedule
				OrderDate = DateTime.UtcNow  // Set the order date to current UTC time
			};

			await ApiService<Ticket>.PostAsync("/ticket", ticket);

			var ticketCard = TicketCard.CreateTicketCard(ticket);

			await stepContext.Context.SendActivityAsync(MessageFactory.Attachment(ticketCard), cancellationToken);

        }
    }
}
