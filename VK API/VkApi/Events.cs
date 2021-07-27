#region Usings.

// Other.
using Newtonsoft.Json;

#endregion

namespace vkapi
{
    namespace events
    {
        public interface IEvent 
        {
            string subscriptionEventName { get; set; }
        };

        public class EventMessageNew : IEvent
        {
            // Subscription Name,
            public string subscriptionEventName { get; set; } = "message_new";

            private class MessageContainer
            {
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

            // Fields.

            // Message object.
            public Message message = null;

            public EventMessageNew(VkApiLongpoll.JsonLongpollUpdate updateEvent)
            {
                // Converting.
                message = JsonConvert.DeserializeObject<MessageContainer>(updateEvent.object_.ToString()).Message;
            }
        }

        public class EventUnknown : IEvent
        {
            // Subscription Name,
            public string subscriptionEventName { get; set; } = "ievent_unkown";

            // Update object.
            public VkApiLongpoll.JsonLongpollUpdate update;

            public EventUnknown(VkApiLongpoll.JsonLongpollUpdate updateEvent)
            {
                // Setting field.
                update = updateEvent;
            }
        };
    }
}
