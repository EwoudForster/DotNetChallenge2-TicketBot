using CoreBot.Cards;
using CoreBot.DialogDetails;
using CoreBot.Helpers;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Choices;
using Microsoft.Bot.Schema;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace CoreBot.Dialogs
{
    public class BookTableDialog : CancelAndHelpDialog
    {
        private const string NameStepMsgText = "Under what name may the table be reserved?";
        private const string TimeStepMsgText = "Would you like to book a table for lunch or dinner?";
        private const string CountStepMsgText = "For how many people do you wish to reserve?";
        private const string EmailStepMsgText = "To what email address may we send the confirmation?";
        private const string PhoneStepMsgText = "What is your phone number?";
        private const string ConfirmStepMsgText = "Is this correct?";

        private readonly string EmailDialogID = "EmailDialogID";
        private readonly string PhoneDialogID = "PhoneDialogID";

        public BookTableDialog()
            : base(nameof(BookTableDialog))
        {
            AddDialog(new TextPrompt(nameof(TextPrompt)));
            AddDialog(new DateResolverDialog());
            AddDialog(new ChoicePrompt(nameof(ChoicePrompt)));
            AddDialog(new TextPrompt(EmailDialogID, EmailValidation));
            AddDialog(new TextPrompt(PhoneDialogID, PhoneValidation));

            var waterfallSteps = new WaterfallStep[]
            {
                FirstNameStepAsync,
                NameDateStepAsync,
                DateTimeStepAsync,
                TimeCountStepAsync,
                CountEmailStepAsync,
                EmailPhoneStepAsync,
                PhoneConfirmStepAsync,
                ConfirmActStepAsync,
            };

            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), waterfallSteps));
            InitialDialogId = nameof(WaterfallDialog);
        }

        private async Task<DialogTurnResult> FirstNameStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var bookTableDetails = (BookTableDetails)stepContext.Options;

            if (bookTableDetails.Name == null)
            {
                var promptMessage = MessageFactory.Text(NameStepMsgText, NameStepMsgText, InputHints.ExpectingInput);
                return await stepContext.PromptAsync(nameof(TextPrompt), new PromptOptions { Prompt = promptMessage }, cancellationToken);
            }

            return await stepContext.NextAsync(bookTableDetails.Name, cancellationToken);
        }

        private async Task<DialogTurnResult> NameDateStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var bookTableDetails = (BookTableDetails)stepContext.Options;
            bookTableDetails.Name = (string)stepContext.Result;

            if (bookTableDetails.Date == null)
            {
                return await stepContext.BeginDialogAsync(nameof(DateResolverDialog), bookTableDetails.Date, cancellationToken);
            }

            return await stepContext.NextAsync(bookTableDetails.Date, cancellationToken);
        }

        private async Task<DialogTurnResult> DateTimeStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var bookTableDetails = (BookTableDetails)stepContext.Options;
            bookTableDetails.Date = (string)stepContext.Result;

            if (bookTableDetails.Time == null)
            {
                await stepContext.Context.SendActivityAsync(MessageFactory.Text(TimeStepMsgText), cancellationToken);
                List<string> timeList = new List<string> { "lunch", "dinner" };
                return await ChoicePromptHelper.PromptChoiceAsync(timeList, stepContext, cancellationToken);
            }

            return await stepContext.NextAsync(bookTableDetails.Time, cancellationToken);
        }

        private async Task<DialogTurnResult> TimeCountStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var bookTableDetails = (BookTableDetails)stepContext.Options;
            try
            {
                bookTableDetails.Time = ((FoundChoice)stepContext.Result).Value;
            }
            catch (Exception)
            {
                bookTableDetails.Time = (string)stepContext.Result;
            }

            if (bookTableDetails.Count == null)
            {
                var promptMessage = MessageFactory.Text(CountStepMsgText, CountStepMsgText, InputHints.ExpectingInput);
                return await stepContext.PromptAsync(nameof(TextPrompt), new PromptOptions { Prompt = promptMessage }, cancellationToken);
            }

            return await stepContext.NextAsync(bookTableDetails.Count, cancellationToken);
        }

        private async Task<DialogTurnResult> CountEmailStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var bookTableDetails = (BookTableDetails)stepContext.Options;
            bookTableDetails.Count = (string)stepContext.Result;

            if (bookTableDetails.Email == null)
            {
                var promptMessage = MessageFactory.Text(EmailStepMsgText, EmailStepMsgText, InputHints.ExpectingInput);
                return await stepContext.PromptAsync(EmailDialogID, new PromptOptions { Prompt = promptMessage }, cancellationToken);
            }

            return await stepContext.NextAsync(bookTableDetails.Email, cancellationToken);
        }

        private async Task<DialogTurnResult> EmailPhoneStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var bookTableDetails = (BookTableDetails)stepContext.Options;
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
            var bookTableDetails = (BookTableDetails)stepContext.Options;
            bookTableDetails.Phone = (string)stepContext.Result;

            var attachment = ConfirmCard.CreateCardAttachment(
                $"Name: {bookTableDetails.Name}",
                $"Contact Details: {bookTableDetails.Email} ({bookTableDetails.Phone})",
                $"Number of People: {bookTableDetails.Count}",
                $"On: {bookTableDetails.Date} for {bookTableDetails.Time}");

            var activity = MessageFactory.Attachment(attachment);
            await stepContext.Context.SendActivityAsync(activity, cancellationToken);

            await stepContext.Context.SendActivityAsync(MessageFactory.Text(ConfirmStepMsgText), cancellationToken);
            var yesnoList = new List<string> { "yes", "no" };
            return await ChoicePromptHelper.PromptChoiceAsync(yesnoList, stepContext, cancellationToken);
        }

        private async Task<DialogTurnResult> ConfirmActStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            if (((FoundChoice)stepContext.Result).Value == "yes")
            {
                var bookTableDetails = (BookTableDetails)stepContext.Options;
                // This is the place to save the booking into the database.
                await stepContext.Context.SendActivityAsync(MessageFactory.Text("Your reservation is saved. Thank you."), cancellationToken);

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
