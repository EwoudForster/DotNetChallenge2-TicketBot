using FirstBot.Helpers;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using System.Threading;
using System.Threading.Tasks;

namespace CoreBot.Dialogs
{
    public class LookPlayingMoviesDialog : CancelAndHelpDialog
    {
        public LookPlayingMoviesDialog()
            : base(nameof(LookPlayingMoviesDialog))
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
            // Show card with opening hours
            var response = MessageFactory.Attachment(CardHelper.CreateCardAttachment("openinghoursCard"));
            await stepContext.Context.SendActivityAsync(response, cancellationToken);

            // End the dialog
            return await stepContext.EndDialogAsync(cancellationToken: cancellationToken);
        }

    }
}
