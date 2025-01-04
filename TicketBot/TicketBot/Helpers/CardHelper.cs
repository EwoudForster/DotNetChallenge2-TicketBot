using Microsoft.Bot.Schema;
using Newtonsoft.Json;
using System.IO;

namespace FirstBot.Helpers
{
    public class CardHelper
    {
        public static Attachment CreateCardAttachment(string name)
        {
            var paths = new[] { ".", "Cards", $"{name}.json" };
            var adaptiveCardJson = File.ReadAllText(Path.Combine(paths));

            var adaptiveCardAttachment = new Attachment()
            {
                ContentType = "application/vnd.microsoft.card.adaptive",
                Content = JsonConvert.DeserializeObject(adaptiveCardJson),
            };

            return adaptiveCardAttachment;
        }
    }
}

