using vkapi;
using System;

namespace vkapi_examples
{
    class main
    {
        public static void Main(string[] args)
        {
            // Your connectiong info.
            string accessToken = "";
            int groupIndex = 0; 

            // New echo bot.
            new EchoBot(accessToken, groupIndex);
        }
    }

    class EchoBot
    {
        public EchoBot(string accessToken, int groupIndex)
        {
            // Creating api.
            VkApiBotLongpoll api = new VkApiBotLongpoll(accessToken, groupIndex);

            // Subscribing on new messages.
            api.EventSubscribeType("message_new");

            // Starting listening with sending out.
            api.ListenLongpoll((EventMessageNew update) => { 
                    VkApiMethods.MessagesSend(api, update.message.text, update.message.fromId); 
            });
        }
    }
}