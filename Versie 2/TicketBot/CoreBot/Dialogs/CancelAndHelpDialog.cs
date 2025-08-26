using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;

namespace CoreBot.Dialogs
{
	public class CancelAndHelpDialog : ComponentDialog
	{
		public CancelAndHelpDialog(string id)
			: base(id)
		{
		}

		protected override async Task<DialogTurnResult> OnContinueDialogAsync(DialogContext innerDc, CancellationToken cancellationToken = default)
		{
			var activity = innerDc.Context.Activity;
			var text = activity.Text?.Trim()?.ToLowerInvariant();

			// Handle text commands like "cancel" or "help"
			if (!string.IsNullOrEmpty(text))
			{
				switch (text)
				{
					case "cancel":
					case "quit":
						await innerDc.Context.SendActivityAsync(
							MessageFactory.Text("Cancelling..."),
							cancellationToken);

						return await innerDc.CancelAllDialogsAsync(cancellationToken);

					case "help":
						await innerDc.Context.SendActivityAsync(
							MessageFactory.Text("Here’s what you can do:\n- Say 'book ticket' to reserve a seat.\n- Say 'check schedule' to view available movies.\n- Say 'rate movie' to leave a rating.\n- Say 'cancel' to exit the current dialog."),
							cancellationToken);

						return EndOfTurn;
				}
			}

			// Continue with normal dialog flow if no intercept
			return await base.OnContinueDialogAsync(innerDc, cancellationToken);
		}
	}
}
