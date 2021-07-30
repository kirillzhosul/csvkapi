#region Usings.

// Methods for handling message sending.
using vkapi.methods;

// Longpoll for handling longpoll.
using vkapi.longpoll;

// Events for handling events. 
using vkapi.longpoll.events;

#endregion

namespace vkapi.examples
{
    public class BanBot
    {
        /// Bot that bans user on message ban from you.

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="accessToken">Access token from group</param>
        /// <param name="groupIndex">Group index</param>
        public BanBot(string accessToken, int groupIndex)
        {
            // Creating API object.
            VkApiBotLongpoll api = new VkApiBotLongpoll(accessToken, groupIndex);

            // Subscribing on our messages (As there we may get any other events that we dont expect to use).
            api.EventSubscribeType(EventMessageReply.SubscriptionEventName);

            // Adding callback event.
            api.OnNewEvent += CallbackMessageReply;

            // Starting listening.
            api.ListenLongpoll();
        }

        /// </summary>
        /// Callback for message reply event.
        /// </summary>
        /// <param name="sender"> Sender (VkApi)</param>
        /// <param name="e"> Event arguments (LongpollEventArgs)</param>
        private void CallbackMessageReply(object sender, LongpollEventArgs e)
        {
            // Upcasting update event with type IEvent to EventMessageReply.
            // (As we want get message object).
            EventMessageReply.Message message = ((EventMessageReply)e.Update).message;

            // Checking text.
            if (message.Text != "ban") return;

            // Sending message to user.
            Methods.MessagesSend((VkApi)sender, "[BanBot] Banned!", message.FromId);

            // Banning.
            Methods.GroupsBan((VkApi)sender, ((VkApiBotLongpoll)sender).groupIndex, message.FromId, null, 0, "Banned by BanBot", true);
        }
    }
}