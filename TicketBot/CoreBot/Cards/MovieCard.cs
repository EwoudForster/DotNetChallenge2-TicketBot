using AdaptiveCards;
using CoreBot.Models;
using Microsoft.Bot.Schema;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

namespace CoreBot.Cards
{
    public class MovieCard
    {
        public static Attachment CreateCardAttachment(List<Movie> movies)
        {
            var card = new AdaptiveCard(new AdaptiveSchemaVersion(1, 0))
            {
                Body = new List<AdaptiveElement>
                {
                    // Title TextBlock
                    new AdaptiveTextBlock
                    {
                        Text = "Movies",
                        Weight = AdaptiveTextWeight.Bolder,
                        Size = AdaptiveTextSize.Large
                    },
                    // Subtitle TextBlock
                    new AdaptiveTextBlock
                    {
                        Text = "Choose a Movie:",
                        Wrap = true
                    },

                    new AdaptiveChoiceSetInput
                    {
                        Id = "movieChoice",
                        Value = movies[0].Id,
                        Style = AdaptiveChoiceInputStyle.Compact,
                        Choices = movies.Select(category => new AdaptiveChoice
                        {
                            Title = category.Name,
                            Value = category.Id
                        }).ToList()
                    }
                },
                Actions = new List<AdaptiveAction>
                {
                    // Submit Action
                    new AdaptiveSubmitAction
                    {
                        Title = "Show Movies",
                        Data = new { action = "showMovies" }
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
