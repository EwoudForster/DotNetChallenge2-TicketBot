using AdaptiveCards;
using CoreBot.Models;
using Microsoft.Bot.Schema;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

namespace CoreBot.Cards
{
    public class ScheduleCard
    {
        public static Attachment CreateCardAttachment(List<Schedule> schedules)
        {
            var card = new AdaptiveCard(new AdaptiveSchemaVersion(1, 0))
            {
                Body = new List<AdaptiveElement>
                {
                    // Title TextBlock
                    new AdaptiveTextBlock
                    {
                        Text = "Schedules",
                        Weight = AdaptiveTextWeight.Bolder,
                        Size = AdaptiveTextSize.Large
                    },
                    // Subtitle TextBlock
                    new AdaptiveTextBlock
                    {
                        Text = "Choose a Schedule:",
                        Wrap = true
                    },

                    new AdaptiveChoiceSetInput
                    {
                        Id = "scheduleChoice",
                        Value = schedules[0].Id,
                        Style = AdaptiveChoiceInputStyle.Compact,
                        Choices = schedules.Select(schedule => new AdaptiveChoice
                        {
                            Title = schedule.Date,
                            Value = schedule.Id
                        }).ToList()
                    }
                },
                Actions = new List<AdaptiveAction>
                {
                    // Submit Action
                    new AdaptiveSubmitAction
                    {
                        Title = "Show Schedules",
                        Data = new { action = "showSchedules" }
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
