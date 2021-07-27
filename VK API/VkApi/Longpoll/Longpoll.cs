#region Usings.

// System.
using System;
using System.Collections.Generic;

// JSON Parsing.
using Newtonsoft.Json;

// Events for working with longpoll events.
using vkapi.longpoll.events;

#endregion

namespace vkapi
{
    namespace longpoll
    {
        /// Longpoll namespace. Includes working with User / Bots longpoll API.
        /// Use this if you want process message and other events.

        /// Callback delegate.
        /// Will be called when got new event from longpoll.
        /// OBSOLETE!
        /// 
        /// public delegate void CallbackDelegate(IEvent longpollEvent);
        ///

        public class LongpollEventArgs : EventArgs
        {
            /// Longpoll event args for event handler.

            /// Update event itself.
            public IEvent Update { get; set; }
        }

        /// Thrown when calling StopListeningLongpoll() when longpoll is not currently listening.
        public class IsNotListeningException : Exception{}

        public abstract class VkApiLongpoll : VkApi
        {
            /// Default VK API longpoll class,
            /// Implements base for VK longpoll, used as parent for user / bot longpoll.
            /// You should dont use this, use User / Bot class instead, this is only abstract wrapper.

            #region Fields.

            // Longpoll server information.
            // Should be changed in GetServer();
            protected Dictionary<string, string> _longpollServer = null;

            // Event that will be called,
            // When got new update longpoll event.
            public event EventHandler<LongpollEventArgs> OnNewEvent;

            // List of subscribed update types.
            // Changed in EventSubscribeType (Also implemented in EventTypeIsSubscribed)
            private List<string> _subscribedUpdateTypes = null;

            // Is we currently listening server?
            // Checked in ListenLongpoll()
            // May be used as stop for listening.
            private bool _isListening = false;

            #endregion

            #region Methods.

            /// <summary>
            /// Constructor. Just calls base VkApi constructor.
            /// </summary>
            /// <param name="accessToken">VK API access token</param>
            public VkApiLongpoll(string accessToken) : base(accessToken){}

            #region Events Subscriptions.

            /// <summary>
            /// Subscibes on event type.
            /// </summary>
            /// <param name="eventType">Event type from VK docs. Example: message_new </param>
            public void EventSubscribeType(string eventType)
            {
                // If subscribed types is not set - set it.
                if (_subscribedUpdateTypes == null) _subscribedUpdateTypes = new List<string>();

                // Adding event type.
                _subscribedUpdateTypes.Add(eventType);
            }

            /// <summary>
            /// Checks is event type is subscribed and should be returned or not.
            /// </summary>
            /// <param name="type">Type to check is subscibed or not</param>
            /// <returns></returns>
            protected bool EventTypeIsSubscribed(string type)
            {
                // Returning true if null (not set).
                if (_subscribedUpdateTypes == null) return true;

                // Returning if subscribeed.
                return _subscribedUpdateTypes.Contains(type);
            }

            #endregion

            /// <summary>
            /// Checks new updates at longpoll server.
            /// </summary>
            /// <returns>Updates list</returns>
            protected abstract json.JsonLongpollUpdates CheckUpdates();

            /// <summary>
            /// Gets server from VK API.
            /// </summary>
            protected abstract void GetServer();

            /// <summary>
            /// Stops listening longpoll if we listening.
            /// </summary>
            public void StopListeningLongpoll()
            {
                if (!_isListening)
                {
                    // If we dont listening right now.

                    // Exception.
                    throw new IsNotListeningException();
                }

                // Stopping.
                _isListening = false;
            }

            /// <summary>
            /// Listens longpoll and passing new events to event OnNewEvent.
            /// </summary>
            public void ListenLongpoll()
            {
                // If server is not set - getting server.
                if (_longpollServer == null) GetServer();

                // Starting listening.
                _isListening = true;

                while (_isListening)
                {
                    // While we running longpoll listener.

                    // Getting updates.
                    json.JsonLongpollUpdates updates = CheckUpdates();

                    foreach (json.JsonLongpollUpdate update in updates.parsed)
                    {
                        // If not subscribed on event - continue.
                        if (!EventTypeIsSubscribed(update.type)) continue;

                        // Calling callback events.
                        OnNewEvent?.Invoke(this, new LongpollEventArgs(){
                            Update = ParseEvent(update)
                        });
                    }

                    // Updating TS.
                    _longpollServer["ts"] = updates.Ts;
                }
            }

            #region Parsers.

            /// <summary>
            /// Parses event to IEvent.
            /// </summary>
            /// <param name="updateEvent">Update from server</param>
            /// <returns>Upcasted event IEvent</returns>
            protected IEvent ParseEvent(json.JsonLongpollUpdate updateEvent)
            {
                // Parsing.
                switch (updateEvent.type)
                {
                    case "message_new":   return new EventMessageNew(updateEvent);
                    case "message_reply": return new EventMessageReply(updateEvent);
                    default:              return new EventUnknown(updateEvent);
                }
            }

            /// <summary>
            /// Parses update list from server.
            /// </summary>
            /// <param name="updates">Updates from server</param>
            /// <returns>List of parsed updates</returns>
            protected List<json.JsonLongpollUpdate> ParseUpdates(json.JsonLongpollUpdates updates)
            {
                // New list.
                List<json.JsonLongpollUpdate> updatesParsed = new List<json.JsonLongpollUpdate>();

                foreach (Dictionary<string, dynamic> update in updates.Updates)
                {
                    // New update.
                    json.JsonLongpollUpdate updateParsed = new json.JsonLongpollUpdate
                    {
                        // Fields.
                        type = Convert.ToString(update["type"]),
                        object_ = update["object"]
                    };

                    // Adding.
                    updatesParsed.Add(updateParsed);
                }

                // Returning.
                return updatesParsed;
            }

            #endregion

            #endregion
        }
       
        public class VkApiBotLongpoll : VkApiLongpoll
        {
            /// VK API Bots longpoll class,
            /// Implements VK bots longpoll.

            #region Fields.

            // Group index.
            // Set in constructor.
            public readonly int groupIndex;

            #endregion

            #region Methods.

            /// <summary>
            /// Constructor.
            /// </summary>
            /// <param name="accessToken">VK API access token</param>
            /// <param name="groupIndex">Group index</param>
            public VkApiBotLongpoll(string accessToken, int groupIndex) : base(accessToken)
            {
                // Set group index.
                this.groupIndex = groupIndex;

                // Ovveride virtual method.
                APIGenerateDefaultParameters();
            }

            /// <summary>
            /// Checks new updates event from server.
            /// </summary>
            /// <returns>Updates list</returns>
            protected override json.JsonLongpollUpdates CheckUpdates()
            {
                // If server is not set - getting server.
                if (_longpollServer == null) GetServer();

                // Getting response.
                string _response = UrlGet($"{_longpollServer["server"]}?act=a_check&key={_longpollServer["key"]}&ts={_longpollServer["ts"]}&wait=25");

                // Getting updates object.
                json.JsonLongpollUpdates updates = JsonConvert.DeserializeObject<json.JsonLongpollUpdates>(_response);

                // Setting raw field.
                updates.raw = _response;

                // Parsing.
                updates.parsed = ParseUpdates(updates);

                // Returning updates.
                return updates;
            }

            /// <summary>
            /// Gets longpoll server information.
            /// </summary>
            protected override void GetServer()
            {
                // Getting JSON longpoll object.
                json.JsonLongpollServer longpollServer = JsonConvert.DeserializeObject<json.JsonLongpollServer>(APIMethod("groups.getLongPollServer"));

                // Setting server.
                _longpollServer = new Dictionary<string, string>()
                {
                    { "ts", longpollServer.Response.Ts },
                    { "server", longpollServer.Response.Server },
                    { "key", longpollServer.Response.Key },
                };
            }

            /// <summary>
            /// Generates default GET requests for API calls.
            /// </summary>
            protected override void APIGenerateDefaultParameters()
            {
                // Generating default parameters.
                _urlDefaultParameters = $"v={_version}&access_token={_accessToken}&group_id={groupIndex}";
            }

            #endregion
        }
        
        public class VkApiUserLongpoll : VkApiLongpoll
        {
            /// VK API User longpoll class,
            /// Implements VK user longpoll.

            /// CURRENTLY UNDONE!
            /// CANT BE USED IN ANY WAYS!

            /// <summary>
            /// Constructor.
            /// </summary>
            public VkApiUserLongpoll(string accessToken) : base(accessToken) { }

            /// <summary>
            /// Checks new updates event from server.
            /// </summary>
            /// <returns>Updates list</returns>
            protected override json.JsonLongpollUpdates CheckUpdates()
            {
                // If server is not set - getting server.
                if (_longpollServer == null) GetServer();

                // Getting response.
                string _response = UrlGet($"{_longpollServer["server"]}?act=a_check&key={_longpollServer["key"]}&ts={_longpollServer["ts"]}&wait=25&mode=8&version=3");

                Console.WriteLine(_response);

                // Getting updates object.
                json.JsonLongpollUpdates updates = JsonConvert.DeserializeObject<json.JsonLongpollUpdates>(_response);

                // Setting raw field.
                updates.raw = _response;

                // Parsing.
                updates.parsed = ParseUpdates(updates);

                // Returning updates.
                return updates;
            }

            /// <summary>
            /// Gets longpoll server information.
            /// </summary>
            protected override void GetServer()
            {
                // Getting JSON longpoll object.
                json.JsonLongpollServer longpollServer = JsonConvert.DeserializeObject<json.JsonLongpollServer>(APIMethod("messages.getLongPollServer"));

                // Setting server.
                _longpollServer = new Dictionary<string, string>()
                {
                    { "ts", longpollServer.Response.Ts },
                    { "server", "http://" + longpollServer.Response.Server },
                    { "key", longpollServer.Response.Key },
                };
            }
        }
    }
}
