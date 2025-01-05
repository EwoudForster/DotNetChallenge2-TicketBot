using CoreBot.Cards;
using FirstBot.Helpers;
using CoreBot.Services;
using CoreBot.Models;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace CoreBot.Dialogs
{
    public class CheckScheduleDialog : CancelAndHelpDialog
    {
        public CheckScheduleDialog()
            : base(nameof(CheckScheduleDialog))
        {

            var waterfallSteps = new WaterfallStep[]
            {
                FirstActStepAsync
            };

            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), waterfallSteps));

            // The initial child Dialog to run
            InitialDialogId = nameof(WaterfallDialog);
        }

        private async Task<DialogTurnResult> FirstActStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {

			var schedules = await ApiService<List<Schedule>>.GetAsync("/schedule");
			// Create the ScheduleCard
			var scheduleCard = ScheduleCard.CreateCardAttachment(schedules);

			// Convert the card to an attachment
			var response = MessageFactory.Attachment(scheduleCard);

			// Send the ScheduleCard to the user
			await stepContext.Context.SendActivityAsync(response, cancellationToken);

			// End the dialog
			return await stepContext.EndDialogAsync(cancellationToken: cancellationToken);
		}

    }
}
