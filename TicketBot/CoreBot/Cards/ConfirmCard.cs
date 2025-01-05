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
                                        Text = "Restaurant",
                                        Weight = AdaptiveTextWeight.Bolder,
                                        Size = AdaptiveTextSize.Large
                                    },
                                    new AdaptiveTextBlock
                                    {
                                        Text = "Reservation",
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
                                        Url = new Uri("https://koenvangeel.pythonanywhere.com/static/restaurant.jpg"),
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
