#region Usings.

// JSON Parsing.
using Newtonsoft.Json;

#endregion

namespace vkapi
{
    namespace longpoll
    {
        /// Longpoll namespace. Includes working with User / Bots longpoll api.
        /// Use this is if you want process message and other events.

        namespace events
        {
            /// Events namespace. Includes event interface / classes.
            /// Use this is if you using longpoll.

            public interface IEvent
            {
                /// Longpoll event interface, used for upcasting.
            };

            public class EventMessageNew : IEvent
            {
                /// New Message event.

                // Subscription name.
                public static string SubscriptionEventName = "message_new";

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
                public EventMessageNew(json.JsonLongpollUpdate updateEvent)
                {
                    // Parsing JSON.

                    // Getting event object string -> Parsing as message container -> Get mesasge property.
                    message = JsonConvert.DeserializeObject<MessageContainer>(updateEvent.object_.ToString()).Message;
                }
            }

            public class EventUnknown : IEvent
            {
                /// Uknown event.
                /// Used as result when event is unknown.
                /// You should dont use this, please create new class even if you dont fill it for now.

                // Subscription Name,
                public static string SubscriptionEventName= "message_new";

                // Update object.
                public json.JsonLongpollUpdate update;

                /// <summary>
                /// Constructor.
                /// </summary>
                /// <param name="updateEvent">Update event, will be passed from library</param>
                public EventUnknown(json.JsonLongpollUpdate updateEvent)
                {
                    // Passing update directly to field, as this is unknown event,
                    // and you should process update as you want by self.
                    update = updateEvent;
                }
            };
        }
    }
}
