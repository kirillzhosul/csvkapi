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

            #region Bases.

            public interface IEvent
            {
                /// Longpoll event interface, used for upcasting.
            };

            public class EventIBase : IEvent
            {
                /// Event base.
                /// Used as result when event is unknown.
                /// You should dont use this, please create new class even if you dont fill it for now.

                // Subscription Name,
                public static string SubscriptionEventName = null;

                // Update object.
                public json.JsonLongpollUpdate update;

                /// <summary>
                /// Constructor.
                /// </summary>
                /// <param name="updateEvent">Update event, will be passed from library</param>
                public EventIBase(json.JsonLongpollUpdate updateEvent)
                {
                    // Passing update directly to field, as this is unknown event,
                    // and you should process update as you want by self.
                    update = updateEvent;
                }
            };

            #endregion

            #region Unknown, Default event.

            public class EventUnknown : EventIBase
            {
                /// Uknown event.
                /// Used as result when event is unknown.
                /// You should dont use this, please create new class even if you dont fill it for now.

                /// <summary>
                /// Constructor.
                /// </summary>
                /// <param name="updateEvent">Update event, will be passed from library</param>
                public EventUnknown(json.JsonLongpollUpdate updateEvent) : base(updateEvent) { }
            };

            #endregion

            #region Events.

            public class EventMessageNew : EventIBase
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
                public EventMessageNew(json.JsonLongpollUpdate updateEvent): base(updateEvent)
                {
                    // Parsing JSON.

                    // Getting event object string -> Parsing as message container -> Get mesasge property.
                    message = JsonConvert.DeserializeObject<MessageContainer>(updateEvent.object_.ToString()).Message;
                }
            }

            public class EventMessageReply : EventIBase
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

            #endregion
        }
    }
}
