using CoreBot.Cards;
using CoreBot.Services;
using CoreBot.Models;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CoreBot.Dialogs
{
	public class CheckScheduleDialog : CancelAndHelpDialog
	{
		public CheckScheduleDialog()
			: base(nameof(CheckScheduleDialog))
		{
			// Define the single-step waterfall
			var waterfallSteps = new WaterfallStep[]
			{
				ShowScheduleStepAsync
			};

			AddDialog(new WaterfallDialog(nameof(WaterfallDialog), waterfallSteps));

			// Set the initial dialog
			InitialDialogId = nameof(WaterfallDialog);
		}

		private async Task<DialogTurnResult> ShowScheduleStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
		{
			// Fetch schedules from your API
			var schedules = await ApiService<List<Schedule>>.GetAsync("/schedule");

			// Create the ScheduleCard attachment
			var scheduleCard = ScheduleCard.CreateCardAttachment(schedules);

			// Send the card as a message
			var response = MessageFactory.Attachment(scheduleCard);
			await stepContext.Context.SendActivityAsync(response, cancellationToken);

			// End the dialog
			return await stepContext.EndDialogAsync(cancellationToken: cancellationToken);
		}
	}
}
