#region Usings.

// System.
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text.Json.Serialization;

// Other.
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

#endregion

namespace vkapi
{
    public class EventMessageNew
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

        public EventMessageNew(VkApiBotLongpoll.JsonLongpollUpdate updateEvent)
        {
            // Converting.
            message = JsonConvert.DeserializeObject<MessageContainer>(updateEvent.object_.ToString()).message;
        }
    }

    public class EventUnknown 
    {
        // Update object.
        public VkApiBotLongpoll.JsonLongpollUpdate update;

        public EventUnknown(VkApiBotLongpoll.JsonLongpollUpdate updateEvent) 
        {
            // Setting field.
            update = updateEvent;
        } 
    };

    public class VkApiBotLongpoll : VkApi
    {
        #region JSON.

        protected class JsonLongpollServer
        {
            // Fields.

            // Server response.
            [JsonProperty("response")]
            public JsonLongpollServerResponse response { get; set; }
        }

        protected class JsonLongpollServerResponse

        {
            // Fields.

            // Key for request.
            [JsonProperty("key")]
            public string key { get; set; }

            // TS for getting only latest update.
            [JsonProperty("ts")]
            public string ts { get; set; }

            // Server URL.
            [JsonProperty("server")]
            public string server { get; set; }
        }

        protected class JsonLongpollUpdates
        {
            // Fields.

            // TS for updating our ts.
            [JsonProperty("ts")]
            public string ts { get; set; }

            // Updates list.
            [JsonProperty("updates")]
            public List<Dictionary<string, dynamic>> updates { get; set; }

            // Raw response.
            public string raw;

            // Parsed updates.
            public List<JsonLongpollUpdate> parsed;
        }

        public class JsonLongpollUpdate
        {
            // Fields.

            // Type of update.
            public string type;

            // Update object itself.
            //Dictionary<string, dynamic> object_;
            public JObject object_; 
        }

        #endregion

        #region Delegates.

        // Declaring callback.
        public delegate void CallbackDelegate(EventMessageNew longpollEvent);

        #endregion

        #region Fields.

        // Group index.
        private int _groupIndex;

        // Longpoll server information.
        private Dictionary<string, string> _longpollServer = null;

        // List of subscribed update types.
        private List<string> _subscribedUpdateTypes = null;

        #endregion

        #region Methods.

        #region Constructor.

        public VkApiBotLongpoll(string accessToken, int groupIndex) : base(accessToken)
            {
                // Group index.
                _groupIndex = groupIndex;

                // Ovveride virtual method.
                GenerateDefaultParameters();
            }

        #endregion

        #region Longpoll.

        #region Events Subscriptions.

        public void EventSubscribeType(string eventType)
        {
            // If subscribed types is not set - set it.
            if (_subscribedUpdateTypes == null) _subscribedUpdateTypes = new List<string>();

            // Adding event type.
            _subscribedUpdateTypes.Add(eventType);
        }

        private bool EventTypeIsSubscribed(string type)
        {
            // Returning true if null (not set).
            if (_subscribedUpdateTypes == null) return true;

            // Returning if subscribeed.
            return _subscribedUpdateTypes.Contains(type);
        }

        #endregion

        #region Parsers.

        private dynamic ParseEvent(JsonLongpollUpdate updateEvent)
        {
            // Parsing.

            switch (updateEvent.type)
            {
                case "message_new":
                    // New message event.
                    return new EventMessageNew(updateEvent);
                default:
                    // Unknown event.
                    return new EventUnknown(updateEvent);
            }
        }

        private List<JsonLongpollUpdate> ParseUpdates(JsonLongpollUpdates updates)
        {
            // New list.
            List<JsonLongpollUpdate> updatesParsed = new List<JsonLongpollUpdate>();

            foreach (Dictionary<string, dynamic> update in updates.updates)
            {
                // New update.
                JsonLongpollUpdate updateParsed = new JsonLongpollUpdate();

                // Fields.
                updateParsed.type = Convert.ToString(update["type"]);
                updateParsed.object_ = update["object"];

                // Adding.
                updatesParsed.Add(updateParsed);
            }

            // Returning.
            return updatesParsed;
        }

        #endregion

        public void ListenLongpoll(CallbackDelegate callback)
        {
            // If server is not set - getting server.
            if (_longpollServer == null) GetServer();

            while (true)
            {
                // While we running longpoll listener.

                // Getting updates.
                JsonLongpollUpdates updates = CheckUpdates();

                // If no updates - pass.
                if (updates.parsed.Count == 0) continue;

                foreach (JsonLongpollUpdate update in updates.parsed)
                {
                    // If not subscribed on event - continue.
                    if (!EventTypeIsSubscribed(update.type)) continue;

                    // Parsing ?.
                    dynamic updateEvent = ParseEvent(update);

                    // Calling callback.
                    callback(updateEvent);
                }

                // Updating TS.
                _longpollServer["ts"] = updates.ts;
            }
        }

        private JsonLongpollUpdates CheckUpdates()
        {
            // If server is not set - getting server.
            if (_longpollServer == null) GetServer();

            // Getting response.
            string _response = UrlGet($"{_longpollServer["url"]}?act=a_check&key={_longpollServer["key"]}&ts={_longpollServer["ts"]}&wait=25");

            // Getting updates object.
            JsonLongpollUpdates updates = JsonConvert.DeserializeObject<JsonLongpollUpdates>(_response);

            // Setting raw field.
            updates.raw = _response;

            // Parsing.
            updates.parsed = ParseUpdates(updates);

            // Returning updates.
            return updates;
        }

        private void GetServer()
        {
            // Getting method response.
            string response = Method("groups.getLongPollServer");

            // Getting JSON longpoll object.
            JsonLongpollServer longpollServer = JsonConvert.DeserializeObject<JsonLongpollServer>(response);

            // Setting server.
            _longpollServer = new Dictionary<string, string>()
            {
                { "ts", longpollServer.response.ts },
                { "url", longpollServer.response.server },
                { "key", longpollServer.response.key },
            };
        }

        #endregion

        #region Other.

        protected override void GenerateDefaultParameters()
        {
            // Generating default parameters.
            _urlDefaultParameters = $"v={_version}&access_token={_accessToken}&group_id={_groupIndex}";
        }

        #endregion

        #endregion
    }

    public class VkApi
    {
        #region Fields.
        // Version for API requests.
        protected string _version = "5.131";

        // Access token.
        protected string _accessToken;

        // Default GET parameters for method url.
        protected string _urlDefaultParameters;

        #endregion

        #region Constants.

        // Url to method.
        protected const string _urlMethod = "https://api.vk.com/method";

        #endregion

        #region Methods.

        #region Constructor.

        public VkApi(string accessToken)
        {
            // Access token.
            _accessToken = accessToken;

            // Generating new default params.
            GenerateDefaultParameters();
        }

        #endregion

        #region API.

        public string Method(string name)
        {
            // Returning response.
            return UrlGet($"{_urlMethod}/{name}?{_urlDefaultParameters}");
        }

        public string Method(string name, string[] arguments)
        {
            // Returning response.
            return UrlGet($"{_urlMethod}/{name}?{_urlDefaultParameters}&{string.Join("&", arguments)}");
        }

        #endregion

        #region Other.

        public void ChangeVersion(string version)
        {
            // Change version.
            _version = version;

            // Generating new default params.
            GenerateDefaultParameters();
        }

        protected virtual void GenerateDefaultParameters()
        {
            // Generating default parameters.
            _urlDefaultParameters = $"v={_version}&access_token={_accessToken}";
        }

        #endregion

        #region System.

        protected string UrlGet(string url)
        {
            // Getting request / response.
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();

            // Downloading and returning.
            using (StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream()))
            {
                return streamReader.ReadToEnd();
            }
        }

        #endregion

        #endregion
    }

    class VkApiMethods
    {
        public static void MessagesSend(VkApi api, string message, long peer_id)
        {
            // Getting arguments for method.
            string[] arguments = {
                $"random_id=0",
                $"peer_id={peer_id}",
                $"message={message}",
            };

            // Asnwering.
            api.Method("messages.send", arguments);
        }
    }
}
