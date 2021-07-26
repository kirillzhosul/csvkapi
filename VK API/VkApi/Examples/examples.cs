namespace vkapi
{
    namespace examples
    {
        using events;

        class EchoBot
        {
            public EchoBot(string accessToken, int groupIndex)
            {
                // Creating api.
                VkApiBotLongpoll api = new VkApiBotLongpoll(accessToken, groupIndex);

                // Subscribing on new messages.
                api.EventSubscribeType("message_new");

                // Starting listening with sending out.
                api.ListenLongpoll((IEvent updateEvent) =>
                {
                    EventMessageNew updateMessage = (EventMessageNew)updateEvent;
                    VkApiMethods.MessagesSend(api, updateMessage.message.Text, updateMessage.message.FromId);
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
                api.ListenLongpoll((IEvent update) =>
                {
                    EventMessageNew message = (EventMessageNew)update;
                    VkApiMethods.MessagesSend(api, message.message.Text, message.message.FromId);
                });
            }
        }
    }
}