using CoreBot.Cards;
using CoreBot.Helpers;
using CoreBot.Models;
using CoreBot.Services;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Choices;
using Microsoft.Bot.Schema;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace CoreBot.Dialogs
{
    public class BookTicketDialog : CancelAndHelpDialog
    {
        private const string NameStepMsgText = "Under what name you want to book a cinematicket?";
        private const string TypeSeatStepMsgText = "Would you like to book a cozy seat or regular seat?";
        private const string MovieStepMsgText = "For Which movie would you like a seat?";
        private const string WhenStepMsgText = "Which time would you like to book the ticket?";
        private const string EmailStepMsgText = "To what email address may we send the confirmation?";
        private const string PhoneStepMsgText = "What is your phone number?";
        private const string ConfirmStepMsgText = "Is this correct?";

        private readonly string EmailDialogID = "EmailDialogID";
        private readonly string PhoneDialogID = "PhoneDialogID";

        public BookTicketDialog()
            : base(nameof(BookTicketDialog))
        {
            AddDialog(new TextPrompt(nameof(TextPrompt)));
            AddDialog(new DateResolverDialog());
            AddDialog(new ChoicePrompt(nameof(ChoicePrompt)));
            AddDialog(new TextPrompt(EmailDialogID, EmailValidation));
            AddDialog(new TextPrompt(PhoneDialogID, PhoneValidation));

            var waterfallSteps = new WaterfallStep[]
            {
                FirstNameStepAsync,
                NameSeatStepAsync,
                SeatMovieStepAsync,
                MovieWhenStepAsync,
                WhenEmailStepAsync,
                EmailPhoneStepAsync,
                PhoneConfirmStepAsync,
                ConfirmActStepAsync,
            };

            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), waterfallSteps));
            InitialDialogId = nameof(WaterfallDialog);
        }

        private async Task<DialogTurnResult> FirstNameStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var bookTicketDetails = (BookTicketDetails)stepContext.Options;

            if (bookTicketDetails.CustomerName == null)
            {
                var promptMessage = MessageFactory.Text(NameStepMsgText, NameStepMsgText, InputHints.ExpectingInput);
                return await stepContext.PromptAsync(nameof(TextPrompt), new PromptOptions { Prompt = promptMessage }, cancellationToken);
            }

            return await stepContext.NextAsync(bookTicketDetails.CustomerName, cancellationToken);
        }

        private async Task<DialogTurnResult> NameSeatStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var bookTicketDetails = (BookTicketDetails)stepContext.Options;
            bookTicketDetails.CustomerName = (string)stepContext.Result;

            if (bookTicketDetails.CozySeat == false)
            {

                await stepContext.Context.SendActivityAsync(MessageFactory.Text(TypeSeatStepMsgText), cancellationToken);
                List<string> seatOptions = new List<string> { "Cozy Seat", "Regular Seat" };
                return await ChoicePromptHelper.PromptChoiceAsync(seatOptions, stepContext, cancellationToken);
            }

            return await stepContext.NextAsync(bookTicketDetails.CozySeat, cancellationToken);
        }

        private async Task<DialogTurnResult> SeatMovieStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var bookTicketDetails = (BookTicketDetails)stepContext.Options;
            bookTicketDetails.CozySeat = (bool)stepContext.Result;

            // Fetch  from API
            var movies = await ScheduleService.G();

            // Create and send the schedule selection card
            var scheduleCardAttachment = ScheduleCard.CreateCardAttachment(movies);
            var scheduleCardActivity = MessageFactory.Attachment(scheduleCardAttachment);
            await stepContext.Context.SendActivityAsync(scheduleCardActivity, cancellationToken);

            // Use the SelectPromptHelper to wait for user selection
            return await stepContext.PromptAsync(
                "scheduleSelection",
                new PromptOptions { Prompt = MessageFactory.Text("Please select a schedule from the card above.") },
                cancellationToken
            );
        }




        private async Task<DialogTurnResult> MovieWhenStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var bookTicketDetails = (BookTicketDetails)stepContext.Options;
            bookTicketDetails.Movie = (Movie)stepContext.Result;

            // Get the selected schedule from the previous step
            string selectedMovie = ((JObject)stepContext.Result)?.ToObject<Dictionary<string, string>>()["Selection"];

            // Find the schedule ID from the list of schedules
            var movies = await MovieService.GetMoviesAsync();
            var movie = movies?.Find(m => selectedMovie.Contains(m.Name));

            // Store the selected schedule ID
            bookTicketDetails.MovieId = movie.Id;

            // Fetch schedule for the selected schedule
            var schedules = await ScheduleService.GetSchedulesByMovieIdAsync(movie.Id);


            // Prepare schedule options for display
            List<string> scheduleOptions = new List<string>();
            foreach (var schedule in schedules)
            {
                scheduleOptions.Add($"{schedule.MovieHall.Name} - {schedule.MovieHall.Location} @ {schedule.Id}");
            }

            // Ask the user to select a schedule
            await stepContext.Context.SendActivityAsync(MessageFactory.Text($"Available showtimes for {movie.Name}:"), cancellationToken);
            return await SelectPromptHelper("scheduleSelection", stepContext, scheduleOptions, cancellationToken);
        }


        private async Task<DialogTurnResult> WhenEmailStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var bookTicketDetails = (BookTicketDetails)stepContext.Options;
            bookTicketDetails.Schedule = (Schedule)stepContext.Result;

            if (bookTicketDetails.Email == null)
            {
                var promptMessage = MessageFactory.Text(EmailStepMsgText, EmailStepMsgText, InputHints.ExpectingInput);
                return await stepContext.PromptAsync(EmailDialogID, new PromptOptions { Prompt = promptMessage }, cancellationToken);
            }

            return await stepContext.NextAsync(bookTicketDetails.Email, cancellationToken);
        }

        private async Task<DialogTurnResult> EmailPhoneStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var bookTableDetails = (BookTicketDetails)stepContext.Options;
            bookTableDetails.Email = (string)stepContext.Result;

            if (bookTableDetails.Phone == null)
            {
                var promptMessage = MessageFactory.Text(PhoneStepMsgText, PhoneStepMsgText, InputHints.ExpectingInput);
                return await stepContext.PromptAsync(PhoneDialogID, new PromptOptions { Prompt = promptMessage }, cancellationToken);
            }

            return await stepContext.NextAsync(bookTableDetails.Phone, cancellationToken);
        }

        private async Task<DialogTurnResult> PhoneConfirmStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var bookTicketDetails = (BookTicketDetails)stepContext.Options;
            bookTicketDetails.Phone = (string)stepContext.Result;

            // Determine seat type
            string seatType = bookTicketDetails.CozySeat ? "Cozy Seat" : "Regular Seat";

            // Create the confirmation card content
            var attachment = ConfirmCard.CreateCardAttachment(
                $"Name: {bookTicketDetails.CustomerName}",
                $"Contact Details: {bookTicketDetails.Email} ({bookTicketDetails.Phone})",
                $"Type of seat: {seatType}",
                $"Movie: {bookTicketDetails.Movie?.Name} for {bookTicketDetails.M?.Date} in schedule hall {bookTicketDetails.Schedule?.ScheduleHall?.Name}"
            );

            // Send the confirmation card
            var activity = MessageFactory.Attachment(attachment);
            await stepContext.Context.SendActivityAsync(activity, cancellationToken);

            // Send confirmation message and prompt for user choice
            await stepContext.Context.SendActivityAsync(MessageFactory.Text(ConfirmStepMsgText), cancellationToken);
            var yesnoList = new List<string> { "yes", "no" };
            return await ChoicePromptHelper.PromptChoiceAsync(yesnoList, stepContext, cancellationToken);
        }


        private async Task<DialogTurnResult> ConfirmActStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            if (((FoundChoice)stepContext.Result).Value == "yes")
            {
                var bookTableDetails = (BookTicketDetails)stepContext.Options;
                // This is the place to save the booking into the database.
                await stepContext.Context.SendActivityAsync(MessageFactory.Text("Your ticket is booked. Thank you."), cancellationToken);

                return await stepContext.EndDialogAsync(bookTableDetails, cancellationToken);
            }

            return await stepContext.EndDialogAsync(null, cancellationToken);
        }

        private async Task<bool> EmailValidation(PromptValidatorContext<string> promptcontext, CancellationToken cancellationtoken)
        {
            const string EmailValidationError = "The email you entered is not valid, please enter a valid email.";

            string email = promptcontext.Recognized.Value;
            if (Regex.IsMatch(email, @"^[\w\.-]+@[a-zA-Z\d\.-]+\.[a-zA-Z]{2,}$"))
            {
                return true;
            }
            await promptcontext.Context.SendActivityAsync(EmailValidationError,
                        cancellationToken: cancellationtoken);
            return false;
        }

        private async Task<bool> PhoneValidation(PromptValidatorContext<string> promptcontext, CancellationToken cancellationtoken)
        {
            const string PhoneValidationError = "The phone number is not valid. Please use these formats: \"014 58 03 35\", \"0465 05 32 63\", \"+32 569 32 65 21\", \"+1 586 32 65 02\"";

            string number = promptcontext.Recognized.Value;
            if (Regex.IsMatch(number, @"^(\+?\d{1,3} )?\d{3,4}( \d{2}){2,4}$"))
            {
                return true;
            }
            await promptcontext.Context.SendActivityAsync(PhoneValidationError,
                        cancellationToken: cancellationtoken);
            return false;
        }
    }
}
