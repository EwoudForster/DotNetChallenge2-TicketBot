using AdaptiveCards;
using Microsoft.Bot.Schema;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CoreBot.Cards
{
    public class MenuCard
    {
        public static Attachment CreateCardAttachment(Category category, List<Dish> dishes)
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
                                        Text = "Menu",
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
                        Text = $"These are our _{category.Name}_",
                        Weight = AdaptiveTextWeight.Default,
                        Size = AdaptiveTextSize.Medium,
                        Spacing = AdaptiveSpacing.Large
                    },
                    new AdaptiveContainer {
                        Items = dishes.Select(dish => new AdaptiveTextBlock
                        {
                            Text = $"• {dish.Name}  {dish.Price.ToString("C", new System.Globalization.CultureInfo("fr-FR"))} - _{dish.Description}_",
Wrap = true,
                            Spacing = AdaptiveSpacing.None
                        }).Cast<AdaptiveElement>().ToList()
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

