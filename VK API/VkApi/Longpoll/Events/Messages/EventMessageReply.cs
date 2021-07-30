#region Usings.

// JSON Parsing.
using Newtonsoft.Json;

#endregion

namespace vkapi.longpoll.events
{
    public class EventMessageReply : Event
    {
        /// Message from event.

        // Subscription name.
        public static new string SubscriptionEventName = "message_reply";

        #region JSON Containers.

        public class Message : EventMessageNew.Message
        {
            #region JSON Fields.

            [JsonProperty("admin_author_id")]
            public bool AdminAuthorId { get; set; }

            #endregion
        }

        #endregion

        // Fields.

        // Message object.
        public Message message = null;

        /// <summary>
        /// Constructor, handles parsing of message.
        /// </summary>
        /// <param name="updateEvent">Update event, will be passed from library</param>
        public EventMessageReply(json.JsonLongpollUpdate updateEvent) : base(updateEvent)
        {
            // Parsing JSON.

            // Getting event object string -> Parsing as message container -> Get mesasge property.
            message = JsonConvert.DeserializeObject<Message>(updateEvent.object_.ToString());
        }
    }
}
