using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using System.Threading;
using System.Threading.Tasks;

namespace CoreBot.DialogDetails
{
	public class BookTicketDialog : ComponentDialog
	{
		public BookTicketDialog() : base(nameof(BookTicketDialog))
		{
			// Add prompts
			AddDialog(new TextPrompt(nameof(TextPrompt)));

			// Define waterfall steps
			var waterfallSteps = new WaterfallStep[]
			{
				AskMovieStepAsync,
				ConfirmBookingStepAsync
			};

			AddDialog(new WaterfallDialog(nameof(WaterfallDialog), waterfallSteps));

			// Set the initial dialog
			InitialDialogId = nameof(WaterfallDialog);
		}

		private async Task<DialogTurnResult> AskMovieStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
		{
			return await stepContext.PromptAsync(nameof(TextPrompt),
				new PromptOptions
				{
					Prompt = MessageFactory.Text("Which movie would you like to book a ticket for?")
				}, cancellationToken);
		}

		private async Task<DialogTurnResult> ConfirmBookingStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
		{
			var movieName = (string)stepContext.Result;

			await stepContext.Context.SendActivityAsync(
				MessageFactory.Text($"Great! I've booked a ticket for **{movieName}**."),
				cancellationToken);

			return await stepContext.EndDialogAsync(null, cancellationToken);
		}
	}
}
