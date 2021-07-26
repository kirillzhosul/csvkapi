#region Usings.

// Other.
using Newtonsoft.Json;

#endregion

namespace vkapi
{
    public interface Event { };

    public class EventMessageNew : Event
    {
        private class MessageContainer
        {
            [JsonProperty("message")]
            public Message message { get; set; }
        }

        public class Message
        {
            #region JSON Fields.

            [JsonProperty("date")]
            public long date { get; set; }

            [JsonProperty("from_id")]
            public long fromId { get; set; }

            [JsonProperty("id")]
            public long id { get; set; }

            [JsonProperty("out")]
            public bool isOut { get; set; }

            [JsonProperty("peer_id")]
            public long peerId { get; set; }

            [JsonProperty("text")]
            public string text { get; set; }

            [JsonProperty("conversation_message_id")]
            public long conversationMessageId { get; set; }

            [JsonProperty("important")]
            public bool important { get; set; }

            [JsonProperty("random_id")]
            public long randomId { get; set; }

            [JsonProperty("is_hidden")]
            public bool isHidden { get; set; }

            // fwd_messages
            // attachments

            #endregion
        }

        // Fields.

        // Message object.
        public Message message = null;

        public EventMessageNew(VkApiLongpoll.JsonLongpollUpdate updateEvent)
        {
            // Converting.
            message = JsonConvert.DeserializeObject<MessageContainer>(updateEvent.object_.ToString()).message;
        }
    }

    public class EventUnknown : Event
    {
        // Update object.
        public VkApiLongpoll.JsonLongpollUpdate update;

        public EventUnknown(VkApiLongpoll.JsonLongpollUpdate updateEvent)
        {
            // Setting field.
            update = updateEvent;
        }
    };
}
