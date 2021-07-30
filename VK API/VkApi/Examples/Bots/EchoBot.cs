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
    public class EchoBot
    {
        /// Bot that echo your message with same text.

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="accessToken">Access token from group</param>
        /// <param name="groupIndex">Group index</param>
        public EchoBot(string accessToken, int groupIndex)
        {
            // Creating API object.
            VkApiBotLongpoll api = new VkApiBotLongpoll(accessToken, groupIndex);

            // Subscribing on new messages (As there we may get any other events that we dont expect to use).
            api.EventSubscribeType(EventMessageNew.SubscriptionEventName);

            // Adding callback event.
            api.OnNewEvent += CallbackMessageNew;

            // Starting listening with our callback for message.
            api.ListenLongpoll();
        }

        /// </summary>
        /// Callback for messa new events.
        /// </summary>
        /// <param name="sender"> Sender (VkApi)</param>
        /// <param name="e"> Event arguments (LongpollEventArgs)</param>
        private void CallbackMessageNew(object sender, LongpollEventArgs e)
        {
            // Upcasting update event with type IEvent to EventMessageNew.
            // (As we want get message object).
            EventMessageNew.Message message = ((EventMessageNew)e.Update).message;

            // Sending message to user.
            Methods.MessagesSend((VkApi)sender, message.Text, message.FromId);
        }
    }
}