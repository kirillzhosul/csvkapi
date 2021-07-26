#region Usings.

// Local.
using vkapi;

// Other.
using Newtonsoft.Json;

// System.
using System;
using System.Collections.Generic;

#endregion

namespace vkapi_examples
{
    class EchoBot
    {
        // Fields.

        // Api.
        private VkApiBotLongpoll api;

        private string ProcessAnswer(string text)
        {
            // Simple echo bot.
            return text;
        }

        public void Callback(VkApiBotLongpoll.JsonLongpollUpdate longpollEvent)
        {
            // Exit if not message.
            if (longpollEvent.type != "message_new") return;

            // Getting update message.
            string message = longpollEvent.object_.GetValue("message").ToString();

            // Getting message text, peer_id.
            string text = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(message)["text"];
            long from = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(message)["from_id"];

            // Our bot answer.
            string answer = ProcessAnswer(text);
            
            // If answer is not exists - return.
            if (answer == null) return;

            // Logging.
            Console.WriteLine($"[EchoBot] Got new message with text - {text} from id {from}, answered it with text - {answer}");

            // Getting arguments for method.
            string[] arguments = {
                $"random_id=0",
                $"peer_id={from}",
                $"message={answer}",
            };

            // Asnwering.
            api.Method("messages.send", arguments);
        }

        public EchoBot(string accessToken, int groupIndex)
        {
            // Creating api.
            api = new VkApiBotLongpoll(accessToken, groupIndex);

            // Logging.
            Console.WriteLine($"[EchoBot] Connected to VK API!");

            // Getting delegate.
            VkApiBotLongpoll.CallbackDelegate callbackDelegate = Callback;

            // Starting listening.
            api.ListenLongpoll(callbackDelegate);
        }
    }
}