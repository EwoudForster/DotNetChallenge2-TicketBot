using CoreBot.Cards;
using CoreBot.DialogDetails;
using CoreBot.Helpers;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CoreBot.Dialogs
{
    public class ReadMenuDialog : CancelAndHelpDialog
    {
        private readonly string CategoryDialogID = "CategoryDialogID";


        public ReadMenuDialog()
        : base(nameof(ReadMenuDialog))
        {

            AddDialog(new TextPrompt(nameof(TextPrompt)));
            AddDialog(new SelectPromptHelper(CategoryDialogID));

            var waterfallSteps = new WaterfallStep[]
            {
FirstCategoryStepAsync,
CategoryActStepAsync
            };

            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), waterfallSteps));

            // The initial child Dialog to run.
            InitialDialogId = nameof(WaterfallDialog);
        }

        private async Task<DialogTurnResult> FirstCategoryStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var readMenuDetails = (ReadMenuDetails)stepContext.Options;

            if (readMenuDetails.Category == null)
            {
                var categories = await CategoryDataService.GetCategoriesAsync();
                var attachment = CategoryCard.CreateCardAttachment(categories);

                var options = new PromptOptions
                {
                    Prompt = new Activity
                    {
                        Attachments = new List<Attachment>() { attachment },
                        Type = ActivityTypes.Message
                    }
                };

                return await stepContext.PromptAsync(CategoryDialogID, options, cancellationToken);
            }

            return await stepContext.NextAsync(readMenuDetails.Category, cancellationToken);
        }

        private async Task<DialogTurnResult> CategoryActStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var readMenuDetails = (ReadMenuDetails)stepContext.Options;

            try
            {
                var result = stepContext.Result.ToString();
                dynamic data = JObject.Parse(result);
                readMenuDetails.Category = data.categoryChoice.ToString();
            }
            catch (Exception)
            {
                readMenuDetails.Category = (string)stepContext.Result;
            }

            var category = await CategoryDataService.GetCategoryByCodeAsync(readMenuDetails.Category);
            var dishes = await DishesDataService.GetDishesByCategoryAsync(category.CategoryId);

            var attachment = MenuCard.CreateCardAttachment(category, dishes);
            var activity = MessageFactory.Attachment(attachment);
            await stepContext.Context.SendActivityAsync(activity, cancellationToken);

            // End the dialog
            return await stepContext.EndDialogAsync(cancellationToken: cancellationToken);
        }

    }
}

