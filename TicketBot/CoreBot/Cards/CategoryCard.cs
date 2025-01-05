using AdaptiveCards;
using Microsoft.Bot.Schema;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

namespace CoreBot.Cards
{
    public class CategoryCard
    {
        public static Attachment CreateCardAttachment(List<Category> categories)
        {
            var card = new AdaptiveCard(new AdaptiveSchemaVersion(1, 0))
            {
                Body = new List<AdaptiveElement>
                {
                    // Title TextBlock
                    new AdaptiveTextBlock
                    {
                        Text = "Restaurant Menu",
                        Weight = AdaptiveTextWeight.Bolder,
                        Size = AdaptiveTextSize.Large
                    },
                    // Subtitle TextBlock
                    new AdaptiveTextBlock
                    {
                        Text = "Choose a category:",
                        Wrap = true
                    },
                    // ChoiceSet for categories
                    new AdaptiveChoiceSetInput
                    {
                        Id = "categoryChoice",
                        Value = categories[0].Code,
                        Style = AdaptiveChoiceInputStyle.Compact,
                        Choices = categories.Select(category => new AdaptiveChoice
                        {
                            Title = category.Name,
                            Value = category.Code
                        }).ToList()
                    }
                },
                Actions = new List<AdaptiveAction>
                {
                    // Submit Action
                    new AdaptiveSubmitAction
                    {
                        Title = "Show Menu",
                        Data = new { action = "showMenu" }
                    }
                }
            };

            // Create an attachment
            var adaptiveCardAttachment = new Attachment
            {
                ContentType = "application/vnd.microsoft.card.adaptive",
                Content = JObject.FromObject(card)
            };

            return adaptiveCardAttachment;
        }
    }
}
