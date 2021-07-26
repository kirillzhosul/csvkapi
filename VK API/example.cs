#region Usings.

// Local.
using vkapi;

// Other.
using Newtonsoft.Json;

// System.
using System;
using System.Collections.Generic;

#endregion

namespace example
{
    class Example
    {


        public static void Callback(VkApiBotLongpoll.JsonLongpollUpdate longpollEvent)
        {
            // Exig if not message.
            if (longpollEvent.type != "message_new")
            {
                // Returning.
                Console.WriteLine($"[Debug] Got unknown event with type {longpollEvent.type}!");
                return;
            }

            // Logging.
            Console.WriteLine($"[Debug] Got new message event!");

            // Getting update message.
            string message = longpollEvent.object_.GetValue("message").ToString();

            // Getting message text
            string text = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(message)["text"];

            // Logging.
            Console.WriteLine(text);
        }

        public static void Main(string[] args)
        {
            // Creating api.
            // TOKEN + GROUP_ID.
            VkApiBotLongpoll api = new VkApiBotLongpoll("", 0);

            // Getting delegate.
            VkApiBotLongpoll.CallbackDelegate callbackDelegate = Callback;

            // Starting listening.
            api.ListenLongpoll(callbackDelegate);
        }
    }
}