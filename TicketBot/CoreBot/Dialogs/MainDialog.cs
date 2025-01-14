using CoreBot.CognitiveModels;
using CoreBot.DialogDetails;
using CoreBot.Models;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Choices;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CoreBot.Dialogs
{
    public class MainDialog : ComponentDialog
    {
        private readonly ILogger _logger;
        private readonly TicketBotCLURecognizer _cluRecognizer;

        public MainDialog(CheckScheduleDialog checkScheduleDialog, BookTicketDialog bookTicketDialog, TicketBotCLURecognizer cluRecognizer, ILogger<MainDialog> logger)
            : base(nameof(MainDialog))
        {
            _logger = logger;
            _cluRecognizer = cluRecognizer;

            // Add sub-dialogs
            AddDialog(checkScheduleDialog);
            AddDialog(bookTicketDialog);

            // Add prompts
            AddDialog(new ChoicePrompt(nameof(ChoicePrompt)));

            // Define waterfall steps
            var waterfallSteps = new WaterfallStep[]
            {
                ShowOptionsStepAsync,  // Step to show main options
                HandleOptionStepAsync, // Step to handle selected option
                RestartOrEndStepAsync  // Step to restart or end the dialog
            };

            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), waterfallSteps));

            // Set the initial dialog
            InitialDialogId = nameof(WaterfallDialog);
        }

        private async Task<DialogTurnResult> ShowOptionsStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var options = new List<string> { "Check the schedule", "Book a Ticket", "Rate a Movie", "Exit" };

            var promptOptions = new PromptOptions
            {
                Prompt = MessageFactory.Text("What would you like to do?"),
                Choices = ChoiceFactory.ToChoices(options),
                RetryPrompt = MessageFactory.Text("Please select one of the available options.")
            };

            return await stepContext.PromptAsync(nameof(ChoicePrompt), promptOptions, cancellationToken);
        }

        private async Task<DialogTurnResult> HandleOptionStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var choice = ((FoundChoice)stepContext.Result).Value;

			//if (_cluRecognizer.IsConfigured)
	  //      {
			//	var result = await _cluRecognizer.RecognizeAsync(stepContext.Context, cancellationToken);
			//	var topIntent = result.GetTopScoringIntent();

   //             if (topIntent.intent == TicketBotModel.Intent.BookTicket.ToString())
   //             {
   //                 return await stepContext.BeginDialogAsync(nameof(BookTableDialog), null, cancellationToken);
   //             }
   //         }

			switch (choice)
            {
                case "Check the schedule":
                    // Start the CheckScheduleDialog
                    return await stepContext.BeginDialogAsync(nameof(CheckScheduleDialog), null, cancellationToken);

                case "Book a Ticket":
                    // Start the BookTableDialog
                    return await stepContext.BeginDialogAsync(nameof(BookTicketDialog), null, cancellationToken);

                case "Rate a Movie":
                    //Start the RatingDialog
                    return await stepContext.BeginDialogAsync(nameof(RatingDialog), null, cancellationToken);
                
                case "Exit":
                    await stepContext.Context.SendActivityAsync(MessageFactory.Text("Goodbye!"), cancellationToken);
                    return await stepContext.EndDialogAsync(null, cancellationToken);

                default:
                    // If no match, restart the dialog
                    await stepContext.Context.SendActivityAsync(MessageFactory.Text("Sorry, I didn't understand that."), cancellationToken);
                    return await stepContext.ReplaceDialogAsync(InitialDialogId, null, cancellationToken);
            }
        }

        private async Task<DialogTurnResult> RestartOrEndStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            await stepContext.Context.SendActivityAsync(MessageFactory.Text("What else can I do for you?"), cancellationToken);
            return await stepContext.ReplaceDialogAsync(InitialDialogId, null, cancellationToken);
        }
    }
}
