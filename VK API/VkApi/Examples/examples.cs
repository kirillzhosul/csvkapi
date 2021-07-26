using vkapi;

namespace vkapi_examples
{
    class main
    {
        private const string ACCESS_TOKEN_USER = "";
        private const string ACCESS_TOKEN_GROUP = "";

        private const int GROUP_INDEX = 0;

        public static void Main(string[] args)
        {
            // New echo bot.
            new EchoBot(ACCESS_TOKEN_GROUP, GROUP_INDEX);

            // New echo user.
            //new EchoUser(ACCESS_TOKEN_USER);
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
            api.ListenLongpoll((Event update) => {
                EventMessageNew message = (EventMessageNew)update;
                VkApiMethods.MessagesSend(api, message.message.text, message.message.fromId); 
            });
        }
    }

    class EchoUser
    {
        public EchoUser(string accessToken)
        {
            // Creating api.
            VkApiUserLongpoll api = new VkApiUserLongpoll(accessToken);

            // Subscribing on new messages.
            api.EventSubscribeType("message_new");

            // Starting listening with sending out.
            api.ListenLongpoll((Event update) => {
                EventMessageNew message = (EventMessageNew)update;
                VkApiMethods.MessagesSend(api, message.message.text, message.message.fromId);
            });
        }
    }
}