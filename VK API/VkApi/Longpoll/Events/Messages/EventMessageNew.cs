#region Usings.

// JSON Parsing.
using Newtonsoft.Json;

#endregion

namespace vkapi.longpoll.events
{
    public class EventMessageNew : Event
    {
        /// New Message event.

        // Subscription name.
        public static new string SubscriptionEventName = "message_new";

        #region JSON Containers.

        private class MessageContainer
        {
            /// JSON Parsing containter for field MESSAGE.

            // Message property.
            [JsonProperty("message")]
            public Message Message { get; set; }
        }

        public class Message
        {
            #region JSON Fields.

            [JsonProperty("date")]
            public long Date { get; set; }

            [JsonProperty("from_id")]
            public long FromId { get; set; }

            [JsonProperty("id")]
            public long Id { get; set; }

            [JsonProperty("out")]
            public bool IsOut { get; set; }

            [JsonProperty("peer_id")]
            public long PeerId { get; set; }

            [JsonProperty("text")]
            public string Text { get; set; }

            [JsonProperty("conversation_message_id")]
            public long ConversationMessageId { get; set; }

            [JsonProperty("important")]
            public bool Important { get; set; }

            [JsonProperty("random_id")]
            public long RandomId { get; set; }

            [JsonProperty("is_hidden")]
            public bool IsHidden { get; set; }

            // fwd_messages
            // attachments

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
        public EventMessageNew(json.JsonLongpollUpdate updateEvent) : base(updateEvent)
        {
            // Parsing JSON.

            // Getting event object string -> Parsing as message container -> Get mesasge property.
            message = JsonConvert.DeserializeObject<MessageContainer>(updateEvent.object_.ToString()).Message;
        }
    }
}
