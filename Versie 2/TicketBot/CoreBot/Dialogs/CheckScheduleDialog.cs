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
			var waterfallSteps = new WaterfallStep[]
			{
				ShowScheduleStepAsync
			};

			AddDialog(new WaterfallDialog(nameof(WaterfallDialog), waterfallSteps));
			InitialDialogId = nameof(WaterfallDialog);
		}

		private async Task<DialogTurnResult> ShowScheduleStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
		{
			var schedules = await ApiService<List<Schedule>>.GetAsync("/schedules"); // API call

			var scheduleCard = ScheduleCard.CreateCardAttachment(schedules);

			var response = MessageFactory.Attachment(scheduleCard);
			await stepContext.Context.SendActivityAsync(response, cancellationToken);

			return await stepContext.EndDialogAsync(cancellationToken: cancellationToken);
		}
	}
}
