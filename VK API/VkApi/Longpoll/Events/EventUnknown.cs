namespace vkapi.longpoll.events
{
    public class EventUnknown : Event
    {
        /// Uknown event.
        /// Used as result when event is unknown.
        /// You shouldn`t use this, please create new class even if you dont fill it for now.

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="updateEvent">Update event, will be passed from library</param>
        public EventUnknown(json.JsonLongpollUpdate updateEvent) : base(updateEvent) { }
    };
}