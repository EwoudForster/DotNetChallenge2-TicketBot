using AdaptiveCards;
using Microsoft.Bot.Schema;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace CoreBot.Cards
{
    public class ConfirmCard
    {
        public static Attachment CreateCardAttachment(string name, string contact, string number, string date)
        {
            var card = new AdaptiveCard(new AdaptiveSchemaVersion(1, 0))
            {
                Body = new List<AdaptiveElement>
                {
                    new AdaptiveColumnSet
                    {
                        Columns = new List<AdaptiveColumn>
                        {
                            new AdaptiveColumn
                            {
                                Width = "stretch",
                                Items = new List<AdaptiveElement>
                                {
                                    new AdaptiveTextBlock
                                    {
                                        Text = "Cinema",
                                        Weight = AdaptiveTextWeight.Bolder,
                                        Size = AdaptiveTextSize.Large
                                    },
                                    new AdaptiveTextBlock
                                    {
                                        Text = "Ticket",
                                        Weight = AdaptiveTextWeight.Bolder,
                                        Size = AdaptiveTextSize.Medium,
                                        Spacing = AdaptiveSpacing.None
                                    }
                                }
                            },
                            new AdaptiveColumn
                            {
                                Width = "auto",
                                Items = new List<AdaptiveElement>
                                {
                                    new AdaptiveImage
                                    {
                                        Url = new Uri("https://as1.ftcdn.net/v2/jpg/01/25/57/92/1000_F_125579217_HL9SYmJR8KzVZ5Jfddr4BPyD3QxSSHtZ.jpg"),
                                        Size = AdaptiveImageSize.Medium,
                                        Style = AdaptiveImageStyle.Default
                                    }
                                }
                            }
                        }
                    },
                    new AdaptiveTextBlock
                    {
                        Text = $"• {name}",
                        Weight = AdaptiveTextWeight.Default,
                        Size = AdaptiveTextSize.Medium,
                        Spacing = AdaptiveSpacing.Large
                    },
                    new AdaptiveTextBlock
                    {
                        Text = $"• {contact}",
                        Weight = AdaptiveTextWeight.Default,
                        Size = AdaptiveTextSize.Medium,
                        Spacing = AdaptiveSpacing.None
                    },
                    new AdaptiveTextBlock
                    {
                        Text = $"• {number}",
                        Weight = AdaptiveTextWeight.Default,
                        Size = AdaptiveTextSize.Medium,
                        Spacing = AdaptiveSpacing.None
                    },
                    new AdaptiveTextBlock
                    {
                        Text = $"• {date}",
                        Weight = AdaptiveTextWeight.Default,
                        Size = AdaptiveTextSize.Medium,
                        Spacing = AdaptiveSpacing.None
                    }
                }
            };

            var adaptiveCardAttachment = new Attachment()
            {
                ContentType = "application/vnd.microsoft.card.adaptive",
                Content = JObject.FromObject(card)
            };

            return adaptiveCardAttachment;
        }
    }
}
