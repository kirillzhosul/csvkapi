namespace vkapi.longpoll.events
{
    public class Event
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
        public Event(json.JsonLongpollUpdate updateEvent)
        {
            // Passing update directly to field, as this is unknown event,
            // and you should process update as you want by self.
            update = updateEvent;
        }
    };
}
